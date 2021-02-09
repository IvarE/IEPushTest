using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CGIXrmWin;

using Microsoft.Crm;
using Microsoft.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System.Configuration;
using System.ServiceModel;
using System.Collections.ObjectModel;

using System.IO;

using CGIXrmRainDanceExport.Classes;

namespace CGIXrmRainDanceExport
{
    public class RunBatch_UTLAND
    {
        XrmManager _xrmManager;
        string _fileName = "";
        int _countInvoince;
        decimal _totalsum;

        public RunBatch_UTLAND()
        {
            try
            {
                _xrmManager = _initManager();
            }
            catch
            {
                throw;
            }
        }

        public bool Run()
        {
            bool _returnValue = true;
            
            try
            {
                string _exportdir = ConfigurationManager.AppSettings["FilesDir"].ToString();

                string _year = DateTime.Now.Year.ToString("0000");
                string _month = DateTime.Now.Month.ToString("00");
                string _day = DateTime.Now.Day.ToString("00");
                string _hour = DateTime.Now.Hour.ToString("00");
                string _minute = DateTime.Now.Minute.ToString("00");
                string _second = DateTime.Now.Second.ToString("00");

                string _now = string.Format("{0}{1}{2}{3}{4}{5}", _year, _month, _day, _hour, _minute, _second);

                _fileName = string.Format("{0}\\DK_utbet_utland_{1}", _exportdir, _now + ".txt");
                Console.WriteLine("_fileName:" + _fileName);
                ObservableCollection<ExportData> _lines = new ObservableCollection<ExportData>();
                int _count = 0;
                _countInvoince = 0;
                _totalsum = 0;

                //get all refunds to export.
                ObservableCollection<refund> _refunds = _xrmManager.Get<refund>(_getxmlPendingRefunds());
                foreach (refund _refund in _refunds)
                {
                    try
                    {
                        Console.WriteLine(string.Format("Processing refundnumber: {0} | CaseId: {1}", _formatString(_refund.Refundnumber), _formatString(_refund.Caseid.ToString())));
                        

                        if (_refund.Caseid != null)
                        {
                            incident _incident = _getCurrentIncident(_refund.Caseid.ToString());
                            contact _contact = _getCurrentContact(_incident.Contactid.ToString());
                            responsible _responsible = _getCurrentResponsible(_refund.Responsibleid.ToString());
                            refundproduct _refundproduct = _getCurrentRefundProduct(_refund.Productid.ToString());
                            user _user = _getUser(_refund.CreatedBy.Id.ToString());
                            refundaccount _refundaccount = _getRefundAccount(_refund.Accountid.ToString());
                            
                            if (_contact != null)
                            {
                                if (_incident != null)
                                {
                                    string _custinfo = _createCustomerRecord(_refund, _contact);
                                    string _inviceinfo = _createInvoiceRecord(_refund, _contact);
                                    string _inforecord = _createInformationRecord(_refund, _contact, _incident);
                                    string _accountrecord = _createAcountingRecord(_refund, _contact, _incident, _responsible, _refundproduct, _user, _refundaccount);

                                    _setRecordToExported(_refund.Refundid);
                                    Console.WriteLine(string.Format("Refund exported: {0} | CaseId: {1}", _formatString(_refund.Refundnumber), _formatString(_refund.Caseid.ToString())));

                                    ExportData _exportdata = new ExportData();
                                    _exportdata.Counter = _count;
                                    _exportdata.Data = _custinfo;
                                    _lines.Add(_exportdata);
                                    _count++;

                                    _exportdata = new ExportData();
                                    _exportdata.Counter = _count;
                                    _exportdata.Data = _inviceinfo;
                                    _lines.Add(_exportdata);
                                    _count++;

                                    _exportdata = new ExportData();
                                    _exportdata.Counter = _count;
                                    _exportdata.Data = _inforecord;
                                    _lines.Add(_exportdata);
                                    _count++;

                                    _exportdata = new ExportData();
                                    _exportdata.Counter = _count;
                                    _exportdata.Data = _accountrecord;
                                    _lines.Add(_exportdata);
                                    _count++;
                                }
                                else
                                {
                                    _logErrorOnRefund(_refund.Refundid, "Ingen ärende koppling hittas på ersättningsposten.");
                                }
                            }
                            else
                            {
                                _logErrorOnRefund(_refund.Refundid, "Ingen kontakt hittas på ärendet.");
                            }
                        }
                        else
                        {
                            _logErrorOnRefund(_refund.Refundid, "Ingen ärende koppling hittas på ersättningsposten.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logErrorOnRefund(_refund.Refundid, ex.Message);
                        Console.WriteLine(ex.ToString());
                    }
                }

                _createFile(_fileName, _lines);
            }
            catch
            {
                _returnValue = false;
                throw;
            }

            return _returnValue;
        }

        //Buntpost
        private string _createHeader()
        {
            string _line1 = "01";
            string _line2 = DateTime.Now.ToShortDateString().PadLeft(149 + 8).Replace("-", "");
            string _line3 = DateTime.Now.ToShortTimeString().PadLeft(157 - 149 - 6).Replace(":", "") + "00";
            string _line = string.Format("{0}{1}{2}", _line1, _line2, _line3);
            return _line;
        }

        //Kundpost
        private string _createCustomerRecord(refund refund, contact contact)
        {
            string _line1 = "02";
            string _line2 = _formatSocNumber(_formatString(refund.Soc_sec_number), _formatString(refund.Foreign_payment)).SetToFixedLengthPadRight(15);
            string _line3 = string.Format("{0} {1}", _formatString(contact.Lastname), _formatString(contact.Firstname)).SetToFixedLengthPadRight(30);
            string _line4 = contact.Address1_line1.SetToFixedLengthPadRight(30);
            string _line5 = string.Format("{0}  {1}", _formatString(contact.Address1_postalcode), _formatString(contact.Address1_city)).SetToFixedLengthPadRight(30);
            string _line6 = "1500".SetToFixedLengthPadRight(6);
            string _line7 = refund.Iban.Substring(0,2);
            string _line8 = refund.Swift.SetToFixedLengthPadRight(30);
            string _line9 = refund.Iban.SetToFixedLengthPadRight(30);
            string _line10 = "".PadLeft(2);
            string _line11 = "".PadLeft(2);

            string _line = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", _line1, _line2, _line3, _line4, _line5, _line6, _line7, _line8, _line9, _line10, _line11);
            return _line;
        }

        //Fakturauppgifter
        private string _createInvoiceRecord(refund refund, contact contact)
        {
            string _line1 = "03";
            string _line2 = _formatSocNumber(_formatString(refund.Soc_sec_number), _formatString(refund.Foreign_payment)).SetToFixedLengthPadRight(15);
            string _line3 = _formatCreateDate(refund.CreatedOn).SetToFixedLengthPadRight(8);
            string _line4 = _formatCreateDate(refund.CreatedOn).SetToFixedLengthPadRight(8);
            string _line5 = _formatCreateDate(refund.CreatedOn).SetToFixedLengthPadRight(8);
            string _line6 = "".SetToFixedLengthPadRight(8);
            string _line7 = _calculatenetamount(refund.Amount, refund.Vat_code_name).SetToFixedLengthPadRight(16); //ex vat

            //Calculate totalsum of all invoivcerows.
            _totalsum = _totalsum + refund.Amount.Value;

            string _line8 = _calculateVatAmount(refund.Amount, refund.Vat_code_name).SetToFixedLengthPadRight(16);
            string _line9 = _formatVatCode(refund.Vat_code_name).SetToFixedLengthPadRight(2);
            string _line = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", _line1, _line2, _line3, _line4, _line5, _line6, _line7, _line8, _line9);
            _countInvoince++;
            return _line;
        }

        //Meddelandeuppgifter
        private string _createInformationRecord(refund refund, contact contact, incident incident)
        {
            string _line1 = "04";
            string _line2 = _formatString(incident.Ticketnumber).SetToFixedLengthPadRight(50);
            string _line3 = _calculatenetamount(refund.Amount, refund.Vat_code_name).SetToFixedLengthPadRight(16); //ex vat
            string _line4 = _formatVatCode(refund.Vat_code_name).SetToFixedLengthPadRight(2);
            string _line = string.Format("{0}{1}{2}{3}", _line1, _line2, _line3, _line4);
            return _line;
        }

        //Konteringspost
        private string _createAcountingRecord(refund refund, contact contact, incident incident, responsible responsible, refundproduct refundproduct, user user, refundaccount refundaccount)
        {
            string _line1 = "05";

            string _account = "";
            if (refundaccount != null)
                _account = refundaccount.Account;

            string _line2 = _formatString(_account).SetToFixedLengthPadRight(10);

            string _line3 = "";
            if (responsible != null)
                _line3 = _formatString(responsible.Responsible).SetToFixedLengthPadRight(10);
            else
                _line3 = _formatString("").SetToFixedLengthPadRight(10);
            
            string _line4 = "1500".SetToFixedLengthPadRight(10);
            string _line5 = "".SetToFixedLengthPadRight(10);

            string _line6 = "";
            if (refundproduct != null)
                _line6 = _formatString(refundproduct.Account).SetToFixedLengthPadRight(10);
            else
                _line6 = _formatString("").SetToFixedLengthPadRight(10);
            
            string _line7 = "".SetToFixedLengthPadRight(10);
            string _line8 = _formatVatCode(refund.Vat_code_name).SetToFixedLengthPadRight(10);
            string _line9 = _checkLenght(string.Format("{0} {1} {2}", _formatString(incident.Ticketnumber), _formatString(contact.Lastname), _formatString(contact.Firstname)).SetToFixedLengthPadRight(30));
            string _line10 = _calculatenetamount(refund.Amount, refund.Vat_code_name).SetToFixedLengthPadRight(16); //ex vat
            string _line11 = (!string.IsNullOrEmpty(user.RsId)) ? user.RsId.SetToFixedLengthPadRight(10) : "".SetToFixedLengthPadRight(10);   // "".SetToFixedLength(10); //RSID
            string _line12 = "".SetToFixedLengthPadRight(10);
            string _line = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", _line1, _line2, _line3, _line4, _line5, _line6, _line7, _line8, _line9, _line10, _line11, _line12);
            return _line;
        }

        //Avslutningspost
        private string _createFooter()
        {
            string _line1 = "06";
            string _line2 = _countInvoince.ToString().PadLeft(10, '0');
            string _line3 = _formatSum(_totalsum, 15);
            string _line = string.Format("{0}{1}{2}", _line1, _line2, _line3);
            return _line;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private bool _createFile(string filename, ObservableCollection<ExportData> lines)
        {

            if (lines == null)
                return false;

            if (lines.Count() <= 0)
                return false;

            string _header = _createHeader();
            if (_header == null)
                return false;

            if (_header.Length <= 0)
                return false;

            StreamWriter _sr = new StreamWriter(filename, true, Encoding.GetEncoding(1252));
            _sr.WriteLine(_header);

            List<ExportData> _lines = lines.OrderBy(x => x.Counter).ToList();
            foreach (ExportData _d in _lines)
            {
                _sr.WriteLine(_d.Data.ToString());
            }

            string _footer = _createFooter();
            _sr.WriteLine(_footer);

            _sr.Flush();
            _sr.Close();

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
            string _returnValue = "";

            if (amount.Value == 0)
            {
                _returnValue = "0";
            }

            if (!string.IsNullOrEmpty(vatcodename))
            {
                //MaxP 2015-04-23
                if (amount.Value < 0)
                {
                    string _svat = vatcodename.ToUpper().Replace("MOMS", "").Replace("IN", "").Replace("UT", "");
                    _svat = _svat.Replace("%", "");
                    decimal _vat = Convert.ToDecimal(_svat);
                    decimal _percent = 1 + (_vat / 100);
                    decimal _value = Math.Abs(amount.Value) / _percent;
                    _returnValue = string.Format("-{0}", _value.ToString("0.00").Replace(".", "").Replace(",", ""));
                }

                if (amount.Value > 0)
                {
                    string _svat = vatcodename.ToUpper().Replace("MOMS", "").Replace("IN", "").Replace("UT", "");
                    _svat = _svat.Replace("%", "");
                    decimal _vat = Convert.ToDecimal(_svat);
                    decimal _percent = 1 + (_vat / 100);
                    decimal _value = Math.Abs(amount.Value) / _percent;
                    _returnValue = string.Format("+{0}", _value.ToString("0.00").Replace(".", "").Replace(",", ""));
                }
            }
            else
            {
                _returnValue = "0";
            }

            return _returnValue;
        }

        private string _calculateVatAmount(Money amount, string vatcodename)
        {
            string _returnValue = "";

            if (amount.Value == 0)
            {
                _returnValue = "0";    
            }

            if (!string.IsNullOrEmpty(vatcodename))
            {
                //MaxP 2015-04-23
                if (amount.Value < 0)
                {
                    string _svat = vatcodename.ToUpper().Replace("MOMS", "").Replace("IN", "").Replace("UT", "");
                    _svat = _svat.Replace("%", "");
                    decimal _vat_factor1 = Convert.ToDecimal(_svat); //Ex. 12%
                    decimal _vat_factor2 = _vat_factor1 / 100;  //0,12
                    decimal _vat_factor3 = 1 + _vat_factor2;    //1,12
                    decimal _vat_factor4 = 1 - (1 / _vat_factor3);    //0,1071
                    decimal _tax = Math.Abs(amount.Value) * _vat_factor4;

                    _returnValue = string.Format("-{0}", _tax.ToString("0.00").Replace(".", "").Replace(",", ""));
                }

                if (amount.Value > 0)
                {
                    string _svat = vatcodename.ToUpper().Replace("MOMS", "").Replace("IN", "").Replace("UT", "");
                    _svat = _svat.Replace("%", "");
                    decimal _vat_factor1 = Convert.ToDecimal(_svat); //Ex. 12%
                    decimal _vat_factor2 = _vat_factor1 / 100;  //0,12
                    decimal _vat_factor3 = 1 + _vat_factor2;    //1,12
                    decimal _vat_factor4 = 1 - (1 / _vat_factor3);    //0,1071

                    decimal _tax = Math.Abs(amount.Value) * _vat_factor4;
                    _returnValue = string.Format("+{0}", _tax.ToString("0.00").Replace(".", "").Replace(",", ""));
                }
            }
            else
            {
                _returnValue = "0";
            }

            return _returnValue;
        }

        private string _formatMoney(Money input)
        {
            string _temp = "";

            if (input.Value > 0)
            {
                _temp = input.Value.ToString("0.00");
                _temp = _temp.Replace(",", "");
                _temp = string.Format("+{0}", _temp);
            }
            else
            {
                _temp = input.Value.ToString("0.00");
                _temp = _temp.Replace(",", "");
                _temp = string.Format("-{0}", _temp);
            }

            return _temp;
        }

        private string _checkLenght(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            string _temp = input.Substring(0, 30);
            return _temp;
        }

        private string _formatVatCode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return _convertToRandanceVatCode(input.ToUpper().Replace(" ", "").Replace("MOMS", "").Replace("%", ""));
        }

        private string _formatSocNumber(string socnr, string foreignnumber)
        {
            string _returnValue = "";

            if (string.IsNullOrEmpty(socnr) && string.IsNullOrEmpty(foreignnumber))
                return _returnValue;

            if (!string.IsNullOrEmpty(socnr) && string.IsNullOrEmpty(foreignnumber))
            {
                string _temp = socnr.Replace("-", "");
                if (_temp.Length == 12)
                    _returnValue = _temp.Substring(2, 10);
                else
                    _returnValue = _temp;

            }

            if (string.IsNullOrEmpty(socnr) && !string.IsNullOrEmpty(foreignnumber))
            {
                _returnValue = foreignnumber;
            }

            return _returnValue;
        }

        private string _formatSum(decimal sum, int pad)
        {
            string _temp = "";

            _temp = sum.ToString("0.00");
            _temp = _temp.Replace(",", "").Replace(".", "").PadLeft(pad, '0');
            return _temp;
        }

        private contact _getCurrentContact(string contactid)
        {
            contact _contact = null;

            ObservableCollection<contact> _contacts = _xrmManager.Get<contact>(_xmlContact(contactid));
            if (_contacts != null && _contacts.Count() > 0)
                _contact = _contacts[0];

            return _contact;
        }

        private incident _getCurrentIncident(string caseid)
        {
            incident _incident = null;

            ObservableCollection<incident> _incinents = _xrmManager.Get<incident>(_xmlCase(caseid));
            if (_incinents != null && _incinents.Count() > 0)
                _incident = _incinents[0];

            return _incident;
        }

        private responsible _getCurrentResponsible(string responsibleid)
        {
            responsible _responsible = null;

            ObservableCollection<responsible> _responsibles = _xrmManager.Get<responsible>(_xmlResponsible(responsibleid));
            if (_responsibles != null && _responsibles.Count() > 0)
                _responsible = _responsibles[0];

            return _responsible;
        }

        private refundproduct _getCurrentRefundProduct(string refundproductid)
        {
            refundproduct _refundproduct = null;

            ObservableCollection<refundproduct> _refundproducts = _xrmManager.Get<refundproduct>(_xmlRefundProduct(refundproductid));
            if (_refundproducts != null && _refundproducts.Count() > 0)
                _refundproduct = _refundproducts[0];

            return _refundproduct;
        }

        private user _getUser(string userid)
        {
            user _user = null;

            ObservableCollection<user> _users = _xrmManager.Get<user>(_xmlGetUser(userid));
            if (_users != null && _users.Count() > 0)
                _user = _users[0];

            return _user;
        }

        private refundaccount _getRefundAccount(string refundaccountid)
        {
            refundaccount _refundaccount = null;

            ObservableCollection<refundaccount> _refundaccounts = _xrmManager.Get<refundaccount>(_xmlGetRefundAccount(refundaccountid));
            if (_refundaccounts != null && _refundaccounts.Count() > 0)
                _refundaccount = _refundaccounts[0];

            return _refundaccount;
        }

        private void _logErrorOnRefund(Guid refundid, string ex)
        {
            Console.WriteLine("_logErrorOnRefund: " + ex);

            Entity _refund = new Entity();
            _refund.LogicalName = "cgi_refund";
            _refund.Id = refundid;

            _refund.Attributes = new AttributeCollection();

            _refund.Attributes.Add("cgi_exportmessage", ex);
            _refund.Attributes.Add("cgi_exportedraindance", false);
            _refund.Attributes.Add("cgi_exportdate", DateTime.Now);

            _xrmManager.Update(_refund);
        }

        private void _setRecordToExported(Guid refundid)
        {
            Entity _refund = new Entity();
            _refund.LogicalName = "cgi_refund";
            _refund.Id = refundid;

            _refund.Attributes = new AttributeCollection();

            _refund.Attributes.Add("cgi_exportmessage", "");
            _refund.Attributes.Add("cgi_exportedraindance", true);
            _refund.Attributes.Add("cgi_exportdate", DateTime.Now);

            _xrmManager.Update(_refund);
        }

        private XrmManager _initManager()
        {
            try
            {

                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"].ToString();
                string domain = ConfigurationManager.AppSettings["Domain"].ToString();
                string username = ConfigurationManager.AppSettings["Username"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                else
                {
                    XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);
                    return xrmMgr;
                }
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
            string _xml = "";

            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_refund'>";
            _xml += "       <attribute name='cgi_refundid' />";
            _xml += "       <attribute name='cgi_refundnumber' />";
            _xml += "       <attribute name='createdon' />";
            _xml += "       <attribute name='createdby' />";
            _xml += "       <attribute name='cgi_vat_code' />";
            _xml += "       <attribute name='cgi_value_code' />";
            _xml += "       <attribute name='cgi_travelcard_number' />";
            _xml += "       <attribute name='cgi_transportcompanyid' />";
            _xml += "       <attribute name='cgi_taxi_company' />";
            _xml += "       <attribute name='cgi_swift' />";
            _xml += "       <attribute name='cgi_soc_sec_number' />";
            _xml += "       <attribute name='cgi_responsibleid' />";
            _xml += "       <attribute name='cgi_reinvoicing' />";
            _xml += "       <attribute name='cgi_reimbursementformid' />";
            _xml += "       <attribute name='cgi_car_reg' />";
            _xml += "       <attribute name='cgi_refundtypeid' />";
            _xml += "       <attribute name='cgi_reference' />";
            _xml += "       <attribute name='overriddencreatedon' />";
            _xml += "       <attribute name='cgi_quantity' />";
            _xml += "       <attribute name='cgi_productid' />";
            _xml += "       <attribute name='cgi_milage_compensation' />";
            _xml += "       <attribute name='cgi_milage' />";
            _xml += "       <attribute name='cgi_last_valid' />";
            _xml += "       <attribute name='cgi_invoicerecipient' />";
            _xml += "       <attribute name='cgi_iban' />";
            _xml += "       <attribute name='cgi_foreign_payment' />";
            _xml += "       <attribute name='cgi_exportedraindance' />";
            _xml += "       <attribute name='transactioncurrencyid' />";
            _xml += "       <attribute name='cgi_contactid' />";
            _xml += "       <attribute name='cgi_comments' />";
            _xml += "       <attribute name='cgi_caseid' />";
            _xml += "       <attribute name='cgi_attestation' />";
            _xml += "       <attribute name='cgi_amountwithtax_base' />";
            _xml += "       <attribute name='cgi_amountwithtax' />";
            _xml += "       <attribute name='cgi_amount_base' />";
            _xml += "       <attribute name='cgi_amount' />";
            _xml += "       <attribute name='cgi_accountno' />";
            _xml += "       <attribute name='cgi_accountid' />";
            _xml += "       <order attribute='cgi_refundnumber' descending='false' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            //_xml += "           <condition attribute='cgi_attestation' operator='eq' value='285050004' />";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_exportedraindance' operator='eq' value='0' />";
            _xml += "               <condition attribute='cgi_exportedraindance' operator='null' />";
            _xml += "           </filter>";
            _xml += "       </filter>";
            _xml += "       <link-entity name='cgi_reimbursementform' from='cgi_reimbursementformid' to='cgi_reimbursementformid' alias='ac'>";
            _xml += "           <filter type='and'>";
            _xml += "               <condition attribute='cgi_attestation' operator='eq' value='1' />";
            _xml += "               <condition attribute='cgi_payment_abroad' operator='eq' value='1' />";
            _xml += "           </filter>";
            _xml += "       </link-entity>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            return _xml;
        }

        private string _xmlContact(string contactid)
        {
            string _xml = "";
            
            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "    <entity name='contact'>";
            _xml += "        <attribute name='contactid' />";
            _xml += "        <attribute name='lastname' />";
            _xml += "        <attribute name='firstname' />";
            _xml += "        <attribute name='address1_line2' />";
            _xml += "        <attribute name='address1_city' />";
            _xml += "        <attribute name='address1_postalcode' />";
            _xml += "        <filter type='and'>";
            _xml += "            <condition attribute='contactid' operator='eq' value='" + contactid + "' />";
            _xml += "        </filter>";
            _xml += "    </entity>";
            _xml += "</fetch>";
            
            return _xml;
        }

        private string _xmlCase(string caseid)
        {
            string _xml = "";

            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='incident'>";
            _xml += "       <attribute name='ticketnumber' />";
            _xml += "       <attribute name='cgi_contactid' />";
            _xml += "       <attribute name='cgi_accountid' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='incidentid' operator='eq' value='" + caseid + "' />";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            return _xml;
        }

        private string _xmlResponsible(string responsibleid)
        {
            string _xml = "";

            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_refundresponsible'>";
            _xml += "       <attribute name='cgi_responsible' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='cgi_refundresponsibleid' operator='eq' value='" + responsibleid + "' />";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            return _xml;
        }

        private string _xmlRefundProduct(string refundproductid)
        {
            string _xml = "";

            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_refundproduct'>";
            _xml += "       <attribute name='cgi_refundproductname' />";
            _xml += "       <attribute name='cgi_account' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "           <condition attribute='cgi_refundproductid' operator='eq' value='" + refundproductid + "' />";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            return _xml;
        }

        private string _xmlGetUser(string userid)
        {
            string _xml = "";

            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='systemuser'>";
            _xml += "       <attribute name='cgi_rsid' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='systemuserid' operator='eq' value='" + userid + "' />";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            return _xml;
        }

        private string _xmlGetRefundAccount(string refundaccountid)
        {
            string _xml = "";

            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "<entity name='cgi_refundaccount'>";
            _xml += "<attribute name='cgi_refundaccountname' />";
            _xml += "<attribute name='cgi_account' />";
            _xml += "<attribute name='cgi_refundaccountid' />";
            _xml += "<filter type='and'>";
            _xml += "<condition attribute='statecode' operator='eq' value='0' />";
            _xml += "<condition attribute='cgi_refundaccountid' operator='eq' value='" + refundaccountid + "' />";
            _xml += "</filter>";
            _xml += "</entity>";
            _xml += "</fetch>";

            return _xml;
        }

    }
}
