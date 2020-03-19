using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.Collections.ObjectModel;

using System.IO;

using CGIXrmRainDanceExport.Classes;
using System.Text.RegularExpressions;
using Microsoft.Xrm.Sdk.Query;
using System.Globalization;

namespace CGIXrmRainDanceExport
{
    public class RunBatch_Kundfaktura
    {
        #region Declarations
        XrmManager _xrmManager;
        string _fileName = "";
        int _countInvoince;
        decimal _totalsum;
        #endregion

        #region Constructors
        public RunBatch_Kundfaktura()
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

                _fileName = string.Format("{0}\\DK_Kundfaktura_{1}", exportdir, now + ".txt");

                ObservableCollection<ExportData> lines = new ObservableCollection<ExportData>();
                int count = 0;
                _countInvoince = 0;
                _totalsum = 0;

                //get all refunds to export.
                ObservableCollection<refund> refunds = _xrmManager.Get<refund>(_getxmlPendingRefunds());
                Console.WriteLine("Number of refunds to process: " + refunds.Count);
                foreach (refund refund in refunds)
                {
                    Console.WriteLine("Refundnr: {0}; amount: {1}; vat code name: {2}; net amount: {3}", refund.Refundnumber, refund.Amount.Value.ToString("#,0.00", CultureInfo.CreateSpecificCulture("sv-SE")), refund.Vat_code_name, _calculatenetamount(refund.Amount, refund.Vat_code_name));
                    try
                    {
                        if (refund.Caseid != null)
                        {
                            incident incident = _getCurrentIncident(refund.Caseid.ToString());
                            contact contact = _getCurrentContact(incident.Contactid.ToString());
                            responsible responsible = _getCurrentResponsible(refund.Responsibleid.ToString());
                            refundproduct refundproduct = _getCurrentRefundProduct(refund.Productid.ToString());
                            user user = _getUser(refund.CreatedBy.Id.ToString());
                            refundaccount refundaccount = _getRefundAccount(refund.Accountid.ToString());
                            invoicerecipient invoicerecipient = _getCurrentInvoiceRecipient(refund.Invoicerecipient.ToString());
                            if (contact != null)
                            {
                                if (incident != null)
                                {
                                    if (invoicerecipient != null)
                                    {
                                        //string _custinfo = _createCustomerRecord(_refund, _contact);
                                        string inviceinfo = _createInvoiceRecord(refund, invoicerecipient, incident);
                                        IEnumerable<string> inforecords = _createInformationRecord(refund, incident);
                                        string invoicerow = _createInvoiceRowRecord(refund, incident);
                                        string accountrecord = _createAcountingRecord(refund, contact, incident, responsible, refundproduct, user, refundaccount);

                                        _setRecordToExported(refund.Refundid);
                                        Console.WriteLine("Refund exported: {0} | CaseId: {1}", _formatString(refund.Refundnumber), _formatString(refund.Caseid.ToString()));

                                        ExportData exportdata = new ExportData
                                        {
                                            Counter = count,
                                            Data = inviceinfo
                                        };

                                        lines.Add(exportdata);
                                        count++;

                                        foreach (string inforecord in inforecords)
                                        {
                                            exportdata = new ExportData
                                            {
                                                Counter = count,
                                                Data = inforecord
                                            };
                                            lines.Add(exportdata);
                                            count++;
                                        }

                                        exportdata = new ExportData
                                        {
                                            Counter = count,
                                            Data = invoicerow
                                        };
                                        lines.Add(exportdata);
                                        count++;

                                        exportdata = new ExportData
                                        {
                                            Counter = count,
                                            Data = accountrecord
                                        };
                                        lines.Add(exportdata);
                                        count++;
                                    }
                                    else
                                    {
                                        _logErrorOnRefund(refund.Refundid, "Ingen fakturamottagare hittas på ersättningsposten.");
                                    }
                                }
                                else
                                {
                                    _logErrorOnRefund(refund.Refundid, "Ingen ärendekoppling hittas på ersättningsposten.");
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
                        Console.Write(ex.ToString());
                    }
                }

                _createFile(_fileName, lines);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
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

        //Fakturauppgifter
        private string _createInvoiceRecord(refund refund, invoicerecipient _invoicerecipient, incident incident)
        {
            string reinvoicingphonenumber;
            GetSettings(_xrmManager.Service, out reinvoicingphonenumber);

            string line1 = "03";
            string line2 = _formatString(_invoicerecipient.cgi_customer_no).SetToFixedLengthPadRight(15);
            string line3 = _formatCreateDate(DateTime.Now).SetToFixedLengthPadRight(8);
            string line4 = "".SetToFixedLengthPadRight(8);
            string line5 = _formatCreateDate(DateTime.Now).SetToFixedLengthPadRight(8);
            string line6 = "".SetToFixedLengthPadRight(8);
            string line7 = _formatString(_invoicerecipient.cgi_inv_reference).SetToFixedLengthPadRight(50);
            string line8 = string.Format("Handläggare: {0}", incident.CreatedByName).SetToFixedLengthPadRight(50);
            string line9 = string.Format("Telefonnummer: {0}", reinvoicingphonenumber).SetToFixedLengthPadRight(50);

            //Calculate totalsum of all invoivcerows..
            _totalsum = _totalsum + refund.Amount.Value;

            string line = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", line1, line2, line3, line4, line5, line6, line7, line8, line9);
            _countInvoince++;
            return line;
        }

        //Meddelandeuppgifter
        //TODO : unsued variable refund
        private IEnumerable<string> _createInformationRecord(refund refund, incident incident)
        {
            IEnumerable<string> informationRecords = SplitStringLines(_formatString(incident.Description), 50);

            List<string> returnValue = new List<string>();

            foreach (string s in informationRecords)
            {
                string line1 = "04";
                string line2 = s.SetToFixedLengthPadRight(50);
                string line = string.Format("{0}{1}", line1, line2);
                returnValue.Add(line);
            }

            return returnValue;
        }
        public IEnumerable<string> SplitStringLines(string text, int chunkSize)
        {
            IEnumerable<string> lines = Regex.Split(text, "\r\n|\r|\n");
            IEnumerable<string> all = new string[0];
            foreach (string s in lines)
            {
                IEnumerable<string> chunks = SplitString(s, chunkSize);
                all = all.Concat(chunks);
            }
            return all;
        }

        private IEnumerable<string> SplitString(string s, int chunkSize)
        {
            if (chunkSize <= 0)
                throw new ArgumentOutOfRangeException("chunkSize", chunkSize, "Only positive non zero values are allowed for parameter chunkSize!");
            if (s == null)
                s = "";

            int chunks = s.Length / chunkSize;
            if (s.Length % chunkSize != 0)
                chunks++;
            IEnumerable<string> returnValue = Enumerable.Range(0, chunks).Select(i => s.Substring(i * chunkSize, s.Length - i * chunkSize < chunkSize ? s.Length - i * chunkSize : chunkSize));
            return returnValue;
        }
        //Fakturarad
        private string _createInvoiceRowRecord(refund refund, incident incident)
        {
            string line1 = "05";
            string line2 = string.Format("Ärendenr: {0}", _formatString(incident.Ticketnumber)).SetToFixedLengthPadRight(50);
            string line3 = _calculatenetamount(refund.Amount, refund.Vat_code_name).SetToFixedLengthPadRight(16); //ex moms
            string line4 = _formatVatCode(refund.Vat_code_name).SetToFixedLengthPadRight(2);
            string line = string.Format("{0}{1}{2}{3}", line1, line2, line3, line4);

            return line;
        }

        //Konteringspost
        private string _createAcountingRecord(refund refund, contact contact, incident incident, responsible responsible, refundproduct refundproduct, user user, refundaccount refundaccount)
        {
            string line1 = "06";

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
            string line10 = _calculatenetamount(refund.Amount, refund.Vat_code_name).SetToFixedLengthPadRight(16); //ex moms
            string line11 = (!string.IsNullOrEmpty(user.RsId)) ? user.RsId.SetToFixedLengthPadRight(10) : "".SetToFixedLengthPadRight(10); //RSID
            string line12 = "".SetToFixedLengthPadRight(10);
            string line = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", line1, line2, line3, line4, line5, line6, line7, line8, line9, line10, line11, line12);
            return line;
        }

        //Avslutningspost
        private string _createFooter()
        {
            string _line1 = "07";
            string line2 = _countInvoince.ToString().PadLeft(10, '0');
            string line3 = _formatSum(_totalsum, 15);
            string line = string.Format("{0}{1}{2}", _line1, line2, line3);
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

            List<ExportData> expLines = lines.OrderBy(x => x.Counter).ToList();
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
        private invoicerecipient _getCurrentInvoiceRecipient(string invoicerecipientid)
        {
            invoicerecipient invoicerecipient = null;

            ObservableCollection<invoicerecipient> invoicerecipients = _xrmManager.Get<invoicerecipient>(_xmlInvoiceRecipient(invoicerecipientid));
            if (invoicerecipients != null && invoicerecipients.Any())
                invoicerecipient = invoicerecipients[0];

            return invoicerecipient;
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

            try
            {
                Entity refund = new Entity
                {
                    LogicalName = "cgi_refund",
                    Id = refundid,
                    Attributes = new AttributeCollection
                    {
                        {"cgi_invoiceexportmessage", ex},
                        {"cgi_invoiceexportedraindance", false},
                        {"cgi_invoiceexportdate", DateTime.Now}
                    }
                };

                _xrmManager.Update(refund);
            }
            catch (Exception exe)
            {
                EntityReference refundRef = new EntityReference("cgi_refund", refundid);
                _CreateAnnotation("ExportError", string.Format("Error occured when logging error on refund: {0}", exe.Message), refundRef);
                _CreateAnnotation("ExportLogMessage", string.Format("{0}", exe), refundRef);
            }
        }
        private void _CreateAnnotation(string subject, string notetext, EntityReference reference)
        {

            Entity annotation = new Entity("annotation");
            annotation["objectid"] = reference;
            annotation["subject"] = subject;
            annotation["notetext"] = notetext;
            _xrmManager.Create(annotation);

        }
        private void _setRecordToExported(Guid refundid)
        {
            Entity refund = new Entity
            {
                LogicalName = "cgi_refund",
                Id = refundid,
                Attributes = new AttributeCollection
                {
                    {"cgi_invoiceexportmessage", ""},
                    {"cgi_invoiceexportedraindance", true},
                    {"cgi_invoiceexportdate", DateTime.Now}
                }
            };

            //bugbug uncomment this line
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
            xml += "       <attribute name='cgi_invoiceexportedraindance' />";
            xml += "       <attribute name='cgi_invoicerecipient' />";
            xml += "       <order attribute='cgi_refundnumber' descending='false' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <condition attribute='cgi_reinvoicing' operator='eq' value='1' />";
            xml += "           <condition attribute='cgi_amount' operator='gt' value='0' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_invoiceexportedraindance' operator='eq' value='0' />";
            xml += "               <condition attribute='cgi_invoiceexportedraindance' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            return xml;
        }

        private void GetSettings(IOrganizationService service, out string cgi_reinvoicingphonenumber)
        {
            #region FetchXML

            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_settingid' />";
            xml += "       <attribute name='cgi_reinvoicingphonenumber' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + now + "' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + now + "' />";
            xml += "               <condition attribute='cgi_validto' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            #endregion

            FetchExpression f = new FetchExpression(xml);
            EntityCollection settingscollection = service.RetrieveMultiple(f);

            Entity settings = settingscollection.Entities.First();

            if (settings.Contains("cgi_reinvoicingphonenumber") && settings["cgi_reinvoicingphonenumber"] != null)
                cgi_reinvoicingphonenumber = settings.GetAttributeValue<string>("cgi_reinvoicingphonenumber");
            else
                throw new Exception("Required setting is missing: cgi_reinvoicingphonenumber");

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
        private string _xmlInvoiceRecipient(string invoicerecipientid)
        {
            string xml = "";

            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "    <entity name='cgi_invoicerecipient'>";
            xml += "        <attribute name='cgi_invoicerecipientid' />";
            xml += "        <attribute name='cgi_address1' />";
            xml += "        <attribute name='cgi_customer_no' />";
            xml += "        <attribute name='cgi_inv_reference' />";
            xml += "        <attribute name='cgi_invoicerecipientname' />";
            xml += "        <attribute name='cgi_postal_city' />";
            xml += "        <attribute name='cgi_postalcode' />";
            xml += "        <filter type='and'>";
            xml += "            <condition attribute='cgi_invoicerecipientid' operator='eq' value='" + invoicerecipientid + "' />";
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
            xml += "       <attribute name='description' />";
            xml += "       <attribute name='createdby' />";
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
