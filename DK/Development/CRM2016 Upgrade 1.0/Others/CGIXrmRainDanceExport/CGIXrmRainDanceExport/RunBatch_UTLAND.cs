using System;
using System.Linq;
using System.Text;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.Collections.ObjectModel;

using System.IO;

using CGIXrmRainDanceExport.Classes;

namespace CGIXrmRainDanceExport
{
    public class RunBatch_UTLAND
    {
        #region Declarations
        readonly XrmManager _xrmManager;
        string _fileName = "";
        int _countInvoince;
        decimal _totalsum;
        #endregion

        #region Constructors
        public RunBatch_UTLAND()
        {
            try
            {
                _xrmManager = _initManager();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Public Methods
        public bool Run()
        {
            try
            {
                string exportdir = ConfigurationManager.AppSettings["FilesDir"];

                string year = DateTime.Now.Year.ToString("0000");
                string month = DateTime.Now.Month.ToString("00");
                string day = DateTime.Now.Day.ToString("00");
                string hour = DateTime.Now.Hour.ToString("00");
                string minute = DateTime.Now.Minute.ToString("00");
                string second = DateTime.Now.Second.ToString("00");

                string now = string.Format("{0}{1}{2}{3}{4}{5}", year, month, day, hour, minute, second);

                _fileName = string.Format("{0}\\DK_utbet_utland_{1}", exportdir, now + ".txt");
                Console.WriteLine("_fileName:" + _fileName);
                ObservableCollection<ExportData> lines = new ObservableCollection<ExportData>();
                int count = 0;
                _countInvoince = 0;
                _totalsum = 0;

                //get all refunds to export.
                //ObservableCollection<refund> refunds = _xrmManager.Get<refund>(_getxmlPendingRefunds());
                ObservableCollection<refund> refunds = _xrmManager.Get<refund>(_getTestPendingRegunds());
                foreach (refund refund in refunds)
                {
                    try
                    {
                        Console.WriteLine("Processing refundnumber: {0} | CaseId: {1}", _formatString(refund.Refundnumber), _formatString(refund.Caseid.ToString()));


                        if (refund.Caseid != null)
                        {
                            incident incident = _getCurrentIncident(refund.Caseid.ToString());
                            contact contact = _getCurrentContact(incident.Contactid.ToString());
                            responsible responsible = _getCurrentResponsible(refund.Responsibleid.ToString());
                            refundproduct refundproduct = _getCurrentRefundProduct(refund.Productid.ToString());
                            user user = _getUser(refund.CreatedBy.Id.ToString());
                            refundaccount refundaccount = _getRefundAccount(refund.Accountid.ToString());

                            if (contact != null)
                            {
                                if (incident != null)
                                {
                                    string custinfo = _createCustomerRecord(refund, contact, incident);
                                    string inviceinfo = _createInvoiceRecord(refund);
                                    string inforecord = _createInformationRecord(refund, incident);
                                    string accountrecord = _createAcountingRecord(refund, contact, incident, responsible, refundproduct, user, refundaccount);

                                    _setRecordToExported(refund.Refundid);
                                    Console.WriteLine("Refund exported: {0} | CaseId: {1}", _formatString(refund.Refundnumber), _formatString(refund.Caseid.ToString()));

                                    ExportData exportdata = new ExportData
                                    {
                                        Counter = count,
                                        Data = custinfo
                                    };
                                    lines.Add(exportdata);
                                    count++;

                                    exportdata = new ExportData
                                    {
                                        Counter = count,
                                        Data = inviceinfo
                                    };
                                    lines.Add(exportdata);
                                    count++;

                                    exportdata = new ExportData
                                    {
                                        Counter = count,
                                        Data = inforecord
                                    };
                                    lines.Add(exportdata);
                                    count++;

                                    exportdata = new ExportData();
                                    exportdata.Counter = count;
                                    exportdata.Data = accountrecord;
                                    lines.Add(exportdata);
                                    count++;
                                }
                                else
                                {
                                    _logErrorOnRefund(refund.Refundid, "Ingen ärende koppling hittas på ersättningsposten.");
                                }
                            }
                            else
                            {
                                _logErrorOnRefund(refund.Refundid, "Ingen kontakt hittas på ärendet.");
                            }
                        }
                        else
                        {
                            _logErrorOnRefund(refund.Refundid, "Ingen ärende koppling hittas på ersättningsposten.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logErrorOnRefund(refund.Refundid, ex.Message);
                        Console.WriteLine(ex.ToString());
                    }
                }

                _createFile(_fileName, lines);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }
        #endregion

        #region Private Methods
        //Buntpost
        private string _createHeader()
        {
            string line1 = "01";
            string line2 = DateTime.Now.ToShortDateString().PadLeft(149 + 8).Replace("-", "");
            string line3 = DateTime.Now.ToShortTimeString().PadLeft(157 - 149 - 6).Replace(":", "") + "00";
            string line = string.Format("{0}{1}{2}", line1, line2, line3);
            return line;
        }

        //Kundpost
        private string _createCustomerRecord(refund refund, contact contact, incident incident)
        {
            string line1 = "02";
            string line2 = _formatSocNumber(_formatString(refund.Soc_sec_number), _formatString(refund.Foreign_payment)).SetToFixedLengthPadRight(15);
            string line3 = string.Format("{0} {1}", _formatString(contact.Lastname), _formatString(contact.Firstname)).SetToFixedLengthPadRight(30);
            string line4 = contact.Address1_line2.SetToFixedLengthPadRight(30);
            string line5 = string.Format("{0}  {1}", _formatString(contact.Address1_postalcode), _formatString(contact.Address1_city)).SetToFixedLengthPadRight(30);
            string line6 = "1500".SetToFixedLengthPadRight(6);
            string line7 = refund.Iban.Substring(0, refund.Iban.Length >= 2 ? 2 : refund.Iban.Length).SetToFixedLengthPadRight(2);
            string line8 = refund.Swift.SetToFixedLengthPadRight(30);
            string line9 = refund.Iban.SetToFixedLengthPadRight(30);
            string line10 = "".PadLeft(2);
            string line11 = "".PadLeft(2);

            //Special handling of adresses for RGOL cases
            if(!string.IsNullOrWhiteSpace(incident.RgolFullname))//if (incident.Caseorigincode == 285050007)
            {
                line3 = incident.RgolFullname.SetToFixedLengthPadRight(30);
                line4 = incident.RgolAddressLine2.SetToFixedLengthPadRight(30);
                line5 = string.Format("{0}  {1}", _formatString(incident.RgolAddress1Postalcode).SetMaxLength(6), _formatString(incident.RgolAddress1City)).SetToFixedLengthPadRight(30);
            }

            string line = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", line1, line2, line3, line4, line5, line6, line7, line8, line9, line10, line11);
            return line;
        }

        //Fakturauppgifter
        //TODO : unused variable contract
        private string _createInvoiceRecord(refund refund)
        {
            string line1 = "03";
            string line2 = _formatSocNumber(_formatString(refund.Soc_sec_number), _formatString(refund.Foreign_payment)).SetToFixedLengthPadRight(15);
            string line3 = _formatCreateDate(refund.CreatedOn).SetToFixedLengthPadRight(8);
            string line4 = _formatCreateDate(refund.CreatedOn).SetToFixedLengthPadRight(8);
            string line5 = _formatCreateDate(refund.CreatedOn).SetToFixedLengthPadRight(8);
            string line6 = "".SetToFixedLengthPadRight(8);
            string line7 = _calculatenetamount(refund.Amount, refund.Vat_code_name).SetToFixedLengthPadRight(16); //ex vat

            //Calculate totalsum of all invoivcerows.
            _totalsum = _totalsum + refund.Amount.Value;

            string line8 = _calculateVatAmount(refund.Amount, refund.Vat_code_name).SetToFixedLengthPadRight(16);
            string line9 = _formatVatCode(refund.Vat_code_name).SetToFixedLengthPadRight(2);
            string line = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", line1, line2, line3, line4, line5, line6, line7, line8, line9);
            _countInvoince++;
            return line;
        }

        //Meddelandeuppgifter
        //TODO : unused variable contract
        private string _createInformationRecord(refund refund, incident incident)
        {
            string line1 = "04";
            string line2 = _formatString(incident.Ticketnumber).SetToFixedLengthPadRight(50);
            string line3 = _calculatenetamount(refund.Amount, refund.Vat_code_name).SetToFixedLengthPadRight(16); //ex vat
            string line4 = _formatVatCode(refund.Vat_code_name).SetToFixedLengthPadRight(2);
            string line = string.Format("{0}{1}{2}{3}", line1, line2, line3, line4);
            return line;
        }

        //Konteringspost
        private string _createAcountingRecord(refund refund, contact contact, incident incident, responsible responsible, refundproduct refundproduct, user user, refundaccount refundaccount)
        {
            string _line1 = "05";

            string account = "";
            if (refundaccount != null)
                account = refundaccount.Account;

            string line2 = _formatString(account).SetToFixedLengthPadRight(10);

            string line3;
            if (responsible != null)
                line3 = _formatString(responsible.Responsible).SetToFixedLengthPadRight(10);
            else
                line3 = _formatString("").SetToFixedLengthPadRight(10);

            string line4 = "1500".SetToFixedLengthPadRight(10);
            string line5 = "".SetToFixedLengthPadRight(10);

            string line6;
            if (refundproduct != null)
                line6 = _formatString(refundproduct.Account).SetToFixedLengthPadRight(10);
            else
                line6 = _formatString("").SetToFixedLengthPadRight(10);

            string line7 = "".SetToFixedLengthPadRight(10);
            string line8 = _formatVatCode(refund.Vat_code_name).SetToFixedLengthPadRight(10);
            string line9 = _checkLenght(string.Format("{0} {1} {2}", _formatString(incident.Ticketnumber), _formatString(contact.Lastname), _formatString(contact.Firstname)).SetToFixedLengthPadRight(30));
            string line10 = _calculatenetamount(refund.Amount, refund.Vat_code_name).SetToFixedLengthPadRight(16); //ex vat
            string line11 = (!string.IsNullOrEmpty(user.RsId)) ? user.RsId.SetToFixedLengthPadRight(10) : "".SetToFixedLengthPadRight(10);   // "".SetToFixedLength(10); //RSID
            string line12 = "".SetToFixedLengthPadRight(10);

            //Special handling of adresses for RGOL cases
            if (!string.IsNullOrWhiteSpace(incident.RgolFullname))//if (incident.Caseorigincode == 285050007)
            {
                line9 = _checkLenght(string.Format("{0} {1}", _formatString(incident.Ticketnumber), _formatString(incident.RgolFullname)).SetToFixedLengthPadRight(30));
            }

            string line = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", _line1, line2, line3, line4, line5, line6, line7, line8, line9, line10, line11, line12);
            return line;
        }

        //Avslutningspost
        private string _createFooter()
        {
            string line1 = "06";
            string line2 = _countInvoince.ToString().PadLeft(10, '0');
            string line3 = _formatSum(_totalsum, 15);
            string line = string.Format("{0}{1}{2}", line1, line2, line3);
            return line;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private bool _createFile(string filename, ObservableCollection<ExportData> lines)
        {

            if (lines == null)
                return false;

            if (!lines.Any())
                return false;

            string header = _createHeader();
            if (header == null)
                return false;

            if (header.Length <= 0)
                return false;

            StreamWriter sr = new StreamWriter(filename, true, Encoding.GetEncoding(1252));
            sr.WriteLine(header);

            var expLines = lines.OrderBy(x => x.Counter).ToList();
            foreach (ExportData d in expLines)
            {
                sr.WriteLine(d.Data);
            }

            string footer = _createFooter();
            sr.WriteLine(footer);

            sr.Flush();
            sr.Close();

            return true;
        }

        private string _formatString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return input.Trim();
        }

        private string _formatCreateDate(DateTime? input)
        {
            if (input == null)
                return "";

            return input.Value.ToShortDateString().Replace("-", "");

        }

        private string _calculatenetamount(Money amount, string vatcodename)
        {
            string returnValue = "";

            if (amount.Value == 0)
            {
                returnValue = "0";
            }

            if (!string.IsNullOrEmpty(vatcodename))
            {
                //MaxP 2015-04-23
                if (amount.Value < 0)
                {
                    string svat = vatcodename.ToUpper().Replace("MOMS", "").Replace("IN", "").Replace("UT", "");
                    svat = svat.Replace("%", "");
                    decimal vat = Convert.ToDecimal(svat);
                    decimal percent = 1 + (vat / 100);
                    decimal value = Math.Abs(amount.Value) / percent;
                    returnValue = string.Format("-{0}", value.ToString("0.00").Replace(".", "").Replace(",", ""));
                }

                if (amount.Value > 0)
                {
                    string svat = vatcodename.ToUpper().Replace("MOMS", "").Replace("IN", "").Replace("UT", "");
                    svat = svat.Replace("%", "");
                    decimal vat = Convert.ToDecimal(svat);
                    decimal percent = 1 + (vat / 100);
                    decimal value = Math.Abs(amount.Value) / percent;
                    returnValue = string.Format("+{0}", value.ToString("0.00").Replace(".", "").Replace(",", ""));
                }
            }
            else
            {
                returnValue = "0";
            }

            return returnValue;
        }

        private string _calculateVatAmount(Money amount, string vatcodename)
        {
            string returnValue = "";

            if (amount.Value == 0)
            {
                returnValue = "0";
            }

            if (!string.IsNullOrEmpty(vatcodename))
            {
                //MaxP 2015-04-23
                if (amount.Value < 0)
                {
                    string svat = vatcodename.ToUpper().Replace("MOMS", "").Replace("IN", "").Replace("UT", "");
                    svat = svat.Replace("%", "");
                    decimal vatFactor1 = Convert.ToDecimal(svat); //Ex. 12%
                    decimal vatFactor2 = vatFactor1 / 100;  //0,12
                    decimal vatFactor3 = 1 + vatFactor2;    //1,12
                    decimal vatFactor4 = 1 - (1 / vatFactor3);    //0,1071
                    decimal tax = Math.Abs(amount.Value) * vatFactor4;

                    returnValue = string.Format("-{0}", tax.ToString("0.00").Replace(".", "").Replace(",", ""));
                }

                if (amount.Value > 0)
                {
                    string svat = vatcodename.ToUpper().Replace("MOMS", "").Replace("IN", "").Replace("UT", "");
                    svat = svat.Replace("%", "");
                    decimal vatFactor1 = Convert.ToDecimal(svat); //Ex. 12%
                    decimal vatFactor2 = vatFactor1 / 100;  //0,12
                    decimal vatFactor3 = 1 + vatFactor2;    //1,12
                    decimal vatFactor4 = 1 - (1 / vatFactor3);    //0,1071

                    decimal tax = Math.Abs(amount.Value) * vatFactor4;
                    returnValue = string.Format("+{0}", tax.ToString("0.00").Replace(".", "").Replace(",", ""));
                }
            }
            else
            {
                returnValue = "0";
            }

            return returnValue;
        }

        private string _checkLenght(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            string temp = input.Substring(0, 30);
            return temp;
        }

        private string _formatVatCode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return _convertToRandanceVatCode(input.ToUpper().Replace(" ", "").Replace("MOMS", "").Replace("%", ""));
        }

        private string _formatSocNumber(string socnr, string foreignnumber)
        {
            string returnValue = "";

            if (string.IsNullOrEmpty(socnr) && string.IsNullOrEmpty(foreignnumber))
                return returnValue;

            if (!string.IsNullOrEmpty(socnr) && string.IsNullOrEmpty(foreignnumber))
            {
                string temp = socnr.Replace("-", "");
                if (temp.Length == 12)
                    returnValue = temp.Substring(2, 10);
                else
                    returnValue = temp;

            }

            if (string.IsNullOrEmpty(socnr) && !string.IsNullOrEmpty(foreignnumber))
            {
                returnValue = foreignnumber;
            }

            return returnValue;
        }

        private string _formatSum(decimal sum, int pad)
        {
            var temp = sum.ToString("0.00");
            temp = temp.Replace(",", "").Replace(".", "").PadLeft(pad, '0');
            return temp;
        }

        private contact _getCurrentContact(string contactid)
        {
            contact contact = null;

            ObservableCollection<contact> contacts = _xrmManager.Get<contact>(_xmlContact(contactid));
            if (contacts != null && contacts.Any())
                contact = contacts[0];

            return contact;
        }

        private incident _getCurrentIncident(string caseid)
        {
            incident incident = null;

            ObservableCollection<incident> incinents = _xrmManager.Get<incident>(_xmlCase(caseid));
            if (incinents != null && incinents.Any())
                incident = incinents[0];

            return incident;
        }

        private responsible _getCurrentResponsible(string responsibleid)
        {
            responsible responsible = null;

            ObservableCollection<responsible> responsibles = _xrmManager.Get<responsible>(_xmlResponsible(responsibleid));
            if (responsibles != null && responsibles.Any())
                responsible = responsibles[0];

            return responsible;
        }

        private refundproduct _getCurrentRefundProduct(string refundproductid)
        {
            refundproduct refundproduct = null;

            ObservableCollection<refundproduct> refundproducts = _xrmManager.Get<refundproduct>(_xmlRefundProduct(refundproductid));
            if (refundproducts != null && refundproducts.Any())
                refundproduct = refundproducts[0];

            return refundproduct;
        }

        private user _getUser(string userid)
        {
            user user = null;

            ObservableCollection<user> users = _xrmManager.Get<user>(_xmlGetUser(userid));
            if (users != null && users.Any())
                user = users[0];

            return user;
        }

        private refundaccount _getRefundAccount(string refundaccountid)
        {
            refundaccount refundaccount = null;

            ObservableCollection<refundaccount> refundaccounts = _xrmManager.Get<refundaccount>(_xmlGetRefundAccount(refundaccountid));
            if (refundaccounts != null && refundaccounts.Any())
                refundaccount = refundaccounts[0];

            return refundaccount;
        }

        private void _logErrorOnRefund(Guid refundid, string ex)
        {
            Console.WriteLine("_logErrorOnRefund: " + ex);

            Entity refund = new Entity
            {
                LogicalName = "cgi_refund",
                Id = refundid,
                Attributes = new AttributeCollection
                {
                    {"cgi_exportmessage", ex},
                    {"cgi_exportedraindance", false},
                    {"cgi_exportdate", DateTime.Now}
                }
            };

            _xrmManager.Update(refund);
        }

        private void _setRecordToExported(Guid refundid)
        {
            Entity refund = new Entity
            {
                LogicalName = "cgi_refund",
                Id = refundid,
                Attributes = new AttributeCollection
                {
                    {"cgi_exportmessage", ""},
                    {"cgi_exportedraindance", true},
                    {"cgi_exportdate", DateTime.Now}
                }
            };

            _xrmManager.Update(refund);
        }

        private XrmManager _initManager()
        {
            try
            {
                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                string domain = ConfigurationManager.AppSettings["Domain"];
                string username = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);
                return xrmMgr;
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }

        private string _convertToRandanceVatCode(string vat)
        {
            if (vat == "25")
                return "M1";
            else if (vat == "12")
                return "M2";
            else if (vat == "6UT")
                return "M3";
            else if (vat == "6IN")
                return "K3";
            else
                return "MF";
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Fetch

        private string _getxmlPendingRefunds()
        {
            string xml = "";

            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_refund'>";
            xml += "       <attribute name='cgi_refundid' />";
            xml += "       <attribute name='cgi_refundnumber' />";
            xml += "       <attribute name='createdon' />";
            xml += "       <attribute name='createdby' />";
            xml += "       <attribute name='cgi_vat_code' />";
            xml += "       <attribute name='cgi_value_code' />";
            xml += "       <attribute name='cgi_travelcard_number' />";
            xml += "       <attribute name='cgi_transportcompanyid' />";
            xml += "       <attribute name='cgi_taxi_company' />";
            xml += "       <attribute name='cgi_swift' />";
            xml += "       <attribute name='cgi_soc_sec_number' />";
            xml += "       <attribute name='cgi_responsibleid' />";
            xml += "       <attribute name='cgi_reinvoicing' />";
            xml += "       <attribute name='cgi_reimbursementformid' />";
            xml += "       <attribute name='cgi_car_reg' />";
            xml += "       <attribute name='cgi_refundtypeid' />";
            xml += "       <attribute name='cgi_reference' />";
            xml += "       <attribute name='overriddencreatedon' />";
            xml += "       <attribute name='cgi_quantity' />";
            xml += "       <attribute name='cgi_productid' />";
            xml += "       <attribute name='cgi_milage_compensation' />";
            xml += "       <attribute name='cgi_milage' />";
            xml += "       <attribute name='cgi_last_valid' />";
            xml += "       <attribute name='cgi_invoicerecipient' />";
            xml += "       <attribute name='cgi_iban' />";
            xml += "       <attribute name='cgi_foreign_payment' />";
            xml += "       <attribute name='cgi_exportedraindance' />";
            xml += "       <attribute name='transactioncurrencyid' />";
            xml += "       <attribute name='cgi_contactid' />";
            xml += "       <attribute name='cgi_comments' />";
            xml += "       <attribute name='cgi_caseid' />";
            xml += "       <attribute name='cgi_attestation' />";
            xml += "       <attribute name='cgi_amountwithtax_base' />";
            xml += "       <attribute name='cgi_amountwithtax' />";
            xml += "       <attribute name='cgi_amount_base' />";
            xml += "       <attribute name='cgi_amount' />";
            xml += "       <attribute name='cgi_accountno' />";
            xml += "       <attribute name='cgi_accountid' />";
            xml += "       <order attribute='cgi_refundnumber' descending='false' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_exportedraindance' operator='eq' value='0' />";
            xml += "               <condition attribute='cgi_exportedraindance' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "       <link-entity name='cgi_reimbursementform' from='cgi_reimbursementformid' to='cgi_reimbursementformid' alias='ac'>";
            xml += "           <filter type='and'>";
            xml += "               <condition attribute='cgi_attestation' operator='eq' value='1' />";
            xml += "               <condition attribute='cgi_payment_abroad' operator='eq' value='1' />";
            xml += "           </filter>";
            xml += "       </link-entity>";
            xml += "   </entity>";
            xml += "</fetch>";

            return xml;
        }

        private string _getTestPendingRegunds()
        {
            string xml = "";

            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_refund'>";
            xml += "       <attribute name='cgi_refundid' />";
            xml += "       <attribute name='cgi_refundnumber' />";
            xml += "       <attribute name='createdon' />";
            xml += "       <attribute name='createdby' />";
            xml += "       <attribute name='cgi_vat_code' />";
            xml += "       <attribute name='cgi_value_code' />";
            xml += "       <attribute name='cgi_travelcard_number' />";
            xml += "       <attribute name='cgi_transportcompanyid' />";
            xml += "       <attribute name='cgi_taxi_company' />";
            xml += "       <attribute name='cgi_swift' />";
            xml += "       <attribute name='cgi_soc_sec_number' />";
            xml += "       <attribute name='cgi_responsibleid' />";
            xml += "       <attribute name='cgi_reinvoicing' />";
            xml += "       <attribute name='cgi_reimbursementformid' />";
            xml += "       <attribute name='cgi_car_reg' />";
            xml += "       <attribute name='cgi_refundtypeid' />";
            xml += "       <attribute name='cgi_reference' />";
            xml += "       <attribute name='overriddencreatedon' />";
            xml += "       <attribute name='cgi_quantity' />";
            xml += "       <attribute name='cgi_productid' />";
            xml += "       <attribute name='cgi_milage_compensation' />";
            xml += "       <attribute name='cgi_milage' />";
            xml += "       <attribute name='cgi_last_valid' />";
            xml += "       <attribute name='cgi_invoicerecipient' />";
            xml += "       <attribute name='cgi_iban' />";
            xml += "       <attribute name='cgi_foreign_payment' />";
            xml += "       <attribute name='cgi_exportedraindance' />";
            xml += "       <attribute name='transactioncurrencyid' />";
            xml += "       <attribute name='cgi_contactid' />";
            xml += "       <attribute name='cgi_comments' />";
            xml += "       <attribute name='cgi_caseid' />";
            xml += "       <attribute name='cgi_attestation' />";
            xml += "       <attribute name='cgi_amountwithtax_base' />";
            xml += "       <attribute name='cgi_amountwithtax' />";
            xml += "       <attribute name='cgi_amount_base' />";
            xml += "       <attribute name='cgi_amount' />";
            xml += "       <attribute name='cgi_accountno' />";
            xml += "       <attribute name='cgi_accountid' />";
            xml += "       <order attribute='cgi_refundnumber' descending='false' />";
            xml += "       <filter type='and'>";
            xml += "                <condition attribute='cgi_refundid' operator='eq' value='{0E5E7101-85BD-EB11-947E-00505684F96A}' />";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            return xml;
        }
        private string _xmlContact(string contactid)
        {
            string xml = "";

            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "    <entity name='contact'>";
            xml += "        <attribute name='contactid' />";
            xml += "        <attribute name='lastname' />";
            xml += "        <attribute name='firstname' />";
            xml += "        <attribute name='address1_line2' />";
            xml += "        <attribute name='address1_city' />";
            xml += "        <attribute name='address1_postalcode' />";
            xml += "        <filter type='and'>";
            xml += "            <condition attribute='contactid' operator='eq' value='" + contactid + "' />";
            xml += "        </filter>";
            xml += "    </entity>";
            xml += "</fetch>";

            return xml;
        }

        private string _xmlCase(string caseid)
        {
            string xml = "";

            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='incident'>";
            xml += "       <attribute name='ticketnumber' />";
            xml += "       <attribute name='cgi_contactid' />";
            xml += "       <attribute name='cgi_accountid' />";
            xml += "       <attribute name='cgi_rgol_address1_line1' />";
            xml += "       <attribute name='cgi_rgol_address1_line2' />";
            xml += "       <attribute name='cgi_rgol_address1_postalcode' />";
            xml += "       <attribute name='cgi_rgol_address1_city' />";
            xml += "       <attribute name='cgi_rgol_address1_country' />";
            xml += "       <attribute name='cgi_rgol_fullname' />";
            xml += "       <attribute name='cgi_soc_sec_number' />";
            xml += "       <attribute name='cgi_rgol_socialsecuritynumber' />";
            xml += "       <attribute name='caseorigincode' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='incidentid' operator='eq' value='" + caseid + "' />";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            return xml;
        }

        private string _xmlResponsible(string responsibleid)
        {
            string xml = "";

            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_refundresponsible'>";
            xml += "       <attribute name='cgi_responsible' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='cgi_refundresponsibleid' operator='eq' value='" + responsibleid + "' />";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            return xml;
        }

        private string _xmlRefundProduct(string refundproductid)
        {
            string xml = "";

            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_refundproduct'>";
            xml += "       <attribute name='cgi_refundproductname' />";
            xml += "       <attribute name='cgi_account' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <condition attribute='cgi_refundproductid' operator='eq' value='" + refundproductid + "' />";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            return xml;
        }

        private string _xmlGetUser(string userid)
        {
            string xml = "";

            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='systemuser'>";
            xml += "       <attribute name='cgi_rsid' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='systemuserid' operator='eq' value='" + userid + "' />";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            return xml;
        }

        private string _xmlGetRefundAccount(string refundaccountid)
        {
            string xml = "";

            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "<entity name='cgi_refundaccount'>";
            xml += "<attribute name='cgi_refundaccountname' />";
            xml += "<attribute name='cgi_account' />";
            xml += "<attribute name='cgi_refundaccountid' />";
            xml += "<filter type='and'>";
            xml += "<condition attribute='statecode' operator='eq' value='0' />";
            xml += "<condition attribute='cgi_refundaccountid' operator='eq' value='" + refundaccountid + "' />";
            xml += "</filter>";
            xml += "</entity>";
            xml += "</fetch>";

            return xml;
        }
        #endregion
    }
}
