using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.Collections.ObjectModel;
using System.IO;
using CGIXrmRainDanceExport.Classes;
using Endeavor.Crm;
using Microsoft.Xrm.Tooling.Connector;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Skanetrafiken.Crm.Schema.Generated;

namespace CGIXrmRainDanceExport
{
    public class RunBatch_RainDance2_Sverige
    {
        #region Declarations
        string _fileName = "";
        int _countInvoince;
        decimal _totalsum;
        Plugin.LocalPluginContext localContext = null;
        OptionMetadataCollection optionsMetadata = null;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // INFO: (hest) The entropy should be unique for each application. DON'T COPY THIS VALUE INTO A NEW PROJECT!!!!
        internal static byte[] Entropy = System.Text.Encoding.Unicode.GetBytes("RainDanceExport");

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePath);
            }
        }
        #endregion

        #region Constructors
        public RunBatch_RainDance2_Sverige()
        {
            try
            {
                localContext = GenerateLocalContext();
                optionsMetadata = RetrieveOptionSetMetadata(RefundEntity.EntityLogicalName, RefundEntity.Fields.cgi_vat_code);
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

                _fileName = string.Format("{0}\\RainDance2_DK_utbet_inland_{1}", exportdir, now + ".txt");
                _log.Debug("_fileName:" + _fileName);

                ObservableCollection<ExportData> lines = new ObservableCollection<ExportData>();
                int count = 0;
                _countInvoince = 0;
                _totalsum = 0;

                //get all refunds to export.
                List<RefundEntity> lRefunds = _getPendingRefunds();
                _log.Debug("Number of refunds to process: " + lRefunds.Count);

                foreach (RefundEntity refund in lRefunds)
                {
                    try
                    {
                        _log.Debug(string.Format("Processing refundnumber: {0} | CaseId: {1}", _formatString(refund.cgi_refundnumber), _formatString(refund.cgi_Caseid?.Id.ToString())));

                        if (refund.cgi_Caseid != null)
                        {
                            _log.Debug(string.Format("(RD2) Processing refundnumber: {0} | CaseId: {1}", _formatString(refund.cgi_refundnumber), _formatString(refund.cgi_Caseid.Id.ToString())));

                            IncidentEntity incident = _getCurrentIncident(refund.cgi_Caseid.Id);
                            ContactEntity contact = _getCurrentContact(incident.cgi_Contactid != null ? incident.cgi_Contactid.Id : Guid.Empty);
                            RefundResponsibleEntity responsible = _getCurrentResponsible(refund.cgi_responsibleId != null ? refund.cgi_responsibleId.Id : Guid.Empty);
                            RefundProductEntity refundproduct = _getCurrentRefundProduct(refund.cgi_Productid != null ? refund.cgi_Productid.Id : Guid.Empty);
                            UserEntity user = _getUser(refund.CreatedBy.Id);
                            RefundAccountEntity refundaccount = _getRefundAccount(refund.cgi_Accountid != null ? refund.cgi_Accountid.Id : Guid.Empty);

                            _log.Debug("Creating Customer/Invoice/Information/Accounting Record");

                            if (contact != null)
                            {
                                _log.Debug("Contact is not null");
                                if (incident != null)
                                {
                                    _log.Debug("Incident is not null");
                                    string custinfo = CreateCustomerRecord(refund, contact, incident);
                                    _log.Debug("Customer Info: " + custinfo);
                                    string inviceinfo = CreateInvoiceHeader(refund, contact);
                                    _log.Debug("Invoice Row: " + inviceinfo);
                                    string inforecord = CreateInvoiceRow(refund, incident); // _createInformationRecord(refund, incident);
                                    _log.Debug("Information Info: " + inforecord);
                                    string accountrecord = CreateAccountingRecord(refund, contact, incident, responsible, refundproduct, user, refundaccount); // _createAcountingRecord(refund, contact, incident, responsible, refundproduct, user, refundaccount);
                                    _log.Debug("Account Info: " + accountrecord);

                                    _log.Debug("Setting Refund Record to Exported");

                                    // Don't run this line while RunBatch is still alive
                                    //_setRecordToExported((Guid)refund.cgi_refundId);

                                    _log.Debug(string.Format("Refund exported: {0} | CaseId: {1}", _formatString(refund.cgi_refundnumber), _formatString(refund.cgi_Caseid.Id.ToString())));

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

                                    exportdata = new ExportData
                                    {
                                        Counter = count,
                                        Data = accountrecord
                                    };
                                    lines.Add(exportdata);
                                    count++;
                                }
                                else
                                    _logErrorOnRefund((Guid)refund.cgi_refundId, "Ingen ärende koppling hittas på ersättningsposten.");
                            }
                            else
                                _logErrorOnRefund((Guid)refund.cgi_refundId, "Ingen kontakt hittas på ärendet.");
                        }
                        else
                            _logErrorOnRefund((Guid)refund.cgi_refundId, "Ingen ärende koppling hittas på ersättningsposten.");
                    }
                    catch (Exception ex)
                    {
                        _logErrorOnRefund((Guid)refund.cgi_refundId, ex.Message);
                        _log.Error("Exception Caught: " + ex.Message);
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
            string line2 = DateTime.Now.ToShortDateString().PadLeft(149 + 8 - 2).Replace("-", "");
            string line3 = DateTime.Now.ToShortTimeString().PadLeft(157 - 149 - 6 - 2).Replace(":", "") + "00";
            string line = string.Format("{0}{1}{2}", line1, line2, line3);
            return line;
        }

        //Kundpost
        private string CreateCustomerRecord(RefundEntity refund, ContactEntity contact, IncidentEntity incident)
        {
            string postmarkering = "02";
            string kundidentitet = contact.cgi_ContactNumber.SetToFixedLengthPadRight(12); // _formatSocNumber(_formatString(refund.cgi_soc_sec_number), _formatString(refund.cgi_foreign_payment)).SetToFixedLengthPadRight(12);
            string kundnamn = string.Format("{0} {1}", _formatString(contact.LastName), _formatString(contact.FirstName)).SetToFixedLengthPadRight(30);
            string kundco = contact.Address1_Line1 != null ? ($"C/O {contact.Address1_Line1}").SetToFixedLengthPadRight(30) : "".SetToFixedLengthPadRight(30);
            string postadress = contact.Address1_Line2.SetToFixedLengthPadRight(30);
            string postnummer = _formatString(FormatPostnummer(contact.Address1_PostalCode)).SetToFixedLengthPadRight(30);
            string blank = "".SetToFixedLengthPadRight(10);
            string ort = _formatString(contact.Address1_City).SetToFixedLengthPadRight(30);
            string momskod = FormatVatCode(refund.cgi_vat_code).SetToFixedLengthPadRight(16);
           string blank2 = "".SetToFixedLengthPadRight(4);
            string personnummer = _formatSocNumber(_formatString(refund.cgi_soc_sec_number), _formatString(refund.cgi_foreign_payment)).SetToFixedLengthPadRight(12);         // "99".PadLeft(16, '0');
            string blank3 = "".SetToFixedLengthPadRight(3);

            string landskod = "".SetToFixedLengthPadRight(2);
            string blank4 = "".SetToFixedLengthPadRight(3);
            string motpart = "150000".SetToFixedLengthPadRight(6);
            string blank5 = "".SetToFixedLengthPadRight(4);
            string kundtyp = "PRIV".SetToFixedLengthPadRight(10);
            string peppoId = "".SetToFixedLengthPadRight(15);
            string plusgiro = "".SetToFixedLengthPadRight(10);
            string bankgiro = "".SetToFixedLengthPadRight(10);
            string bankkonto = "".SetToFixedLengthPadRight(16);
            string blank6 = "".SetToFixedLengthPadRight(4);
            string gln = "".SetToFixedLengthPadRight(13);
            //string blank7 = "".SetToFixedLengthPadRight(7);
            //string iban = "".SetToFixedLengthPadRight(34);
            //string bicswift = "".SetToFixedLengthPadRight(11);
            //string riksbankskode = "223".SetToFixedLengthPadRight(3);

            //Special handling of adresses for RGOL cases
            if (!string.IsNullOrWhiteSpace(incident.cgi_rgol_fullname))//if (incident.Caseorigincode == 285050007)
            {
                kundnamn = incident.cgi_rgol_fullname.SetToFixedLengthPadRight(30);
                postadress = incident.cgi_rgol_address1_line2.SetToFixedLengthPadRight(30);
                postnummer = $"{_formatString(FormatPostnummer(incident.cgi_rgol_address1_postalcode))}".SetToFixedLengthPadRight(30);
                ort = _formatString(incident.cgi_rgol_address1_city).SetToFixedLengthPadRight(30);
            }

            string line = string.Format($"{postmarkering}{kundidentitet}{kundnamn}{kundco}{postadress}{postnummer}{blank}{ort}{momskod}{blank2}{personnummer}{blank3}{landskod}{blank4}{motpart}{blank5}{kundtyp}{peppoId}{plusgiro}{bankgiro}{bankkonto}{blank6}{gln}"); // {blank7}{iban}{bicswift}{riksbankskode}");
            return line;
        }

        //Fakturauppgifter
        //TODO : contract never used
        private string CreateInvoiceHeader(RefundEntity refund, ContactEntity contact)
        {
            string postmarkering = "04";
            string kundidentitet = contact.cgi_ContactNumber.SetToFixedLengthPadRight(12);
            string fakturadatum = _formatCreateDate(refund.CreatedOn).SetToFixedLengthPadRight(8);
            string forfallodatum = _formatCreateDate(refund.CreatedOn).SetToFixedLengthPadRight(8);
            string bokforingsnamn = _formatCreateDate(refund.CreatedOn).SetToFixedLengthPadRight(8);
            string blank1 = "".SetToFixedLengthPadRight(1);
            string erreferens = "".SetToFixedLengthPadRight(30);
            string blank2 = "".SetToFixedLengthPadRight(5);
            string fakturanummer = "".SetToFixedLengthPadRight(8);
            //string varreferens = "".SetToFixedLengthPadRight(30);
            //string fakturaavser = "".SetToFixedLengthPadRight(40);
            string blank3 = "".SetToFixedLengthPadRight(52);
            string motpart = "".SetToFixedLengthPadRight(10);
            string personnummer = "".SetToFixedLengthPadRight(12);
            string blank4 = "".SetToFixedLengthPadRight(23);
            string reskontraposttyp = "".SetToFixedLengthPadRight(20);
            string ikpart = "".SetToFixedLengthPadRight(4);

            ////Calculate totalsum of all invoivcerows..
            //_totalsum = _totalsum + refund.cgi_Amount.Value;

            //int vatCode = refund.cgi_vat_code != null ? (int)refund.cgi_vat_code : int.MinValue;

            //string line7 = "";
            //string line8 = "";
            //string line9 = "";
            //if (vatCode == int.MinValue)
            //{
            //    line7 = _calculatenetamount(refund.cgi_Amount, null).SetToFixedLengthPadRight(16); //ex moms
            //    line8 = _calculateVatAmount(refund.cgi_Amount, null).SetToFixedLengthPadRight(16);
            //    line9 = _formatVatCode(null).SetToFixedLengthPadRight(2);
            //}
            //else
            //{
            //    string vatName = getlabelFromValueOptionSet(vatCode); //Enum.GetName(typeof(Generated.cgi_refund_cgi_vat_code), vatCode.Value);
            //    line7 = _calculatenetamount(refund.cgi_Amount, vatName).SetToFixedLengthPadRight(16); //ex moms
            //    line8 = _calculateVatAmount(refund.cgi_Amount, vatName).SetToFixedLengthPadRight(16);
            //    line9 = _formatVatCode(vatName).SetToFixedLengthPadRight(2);
            //}

            string line = string.Format($"{postmarkering}{kundidentitet}{fakturadatum}{forfallodatum}{bokforingsnamn}{blank1}{erreferens}{blank2}{fakturanummer}{blank3}{motpart}{personnummer}{blank4}{reskontraposttyp}{ikpart}");
            _countInvoince++;

            return line;
        }

        private string CreateInvoiceRow(RefundEntity refund, IncidentEntity incident)
        {
            string postmarkering = "06";
            string betalningsmeddelande = _formatString(incident.TicketNumber).SetToFixedLengthPadRight(150);
            string blank1 = "".SetToFixedLengthPadRight(1);
            int vatCode = refund.cgi_vat_code != null ? (int)refund.cgi_vat_code : int.MinValue;
            string belopp = "";
            string momskod = "";

            if (vatCode == int.MinValue)
            {
                belopp = _calculatenetamount(refund.cgi_Amount, null).SetToFixedLengthPadRight(15); //ex moms
                momskod = _formatVatCode(null).SetToFixedLengthPadRight(3);
            }
            else
            {
                string vatName = getlabelFromValueOptionSet(vatCode);
                belopp = _calculatenetamount(refund.cgi_Amount, vatName).SetToFixedLengthPadRight(15); //ex moms
                momskod = _formatVatCode(vatName).SetToFixedLengthPadRight(3);
            }
            string blank2 = "".SetToFixedLengthPadRight(1);
            string kreditmarkering = "-".SetToFixedLengthPadRight(1);
            string blank3 = "".SetToFixedLengthPadRight(1);

            string line = string.Format($"{postmarkering}{betalningsmeddelande}{blank1}{belopp}{blank2}{kreditmarkering}{blank3}{momskod}");
            return line;

        }


        //Fakturauppgifter
        //TODO : contract never used
        private string _createInvoiceRecord(RefundEntity refund)
        {
            string line1 = "03";
            string line2 = _formatSocNumber(_formatString(refund.cgi_soc_sec_number), _formatString(refund.cgi_foreign_payment)).SetToFixedLengthPadRight(15);
            string line3 = _formatCreateDate(refund.CreatedOn).SetToFixedLengthPadRight(8);
            string line4 = _formatCreateDate(refund.CreatedOn).SetToFixedLengthPadRight(8);
            string line5 = _formatCreateDate(refund.CreatedOn).SetToFixedLengthPadRight(8);
            string line6 = "".SetToFixedLengthPadRight(8);

            //Calculate totalsum of all invoivcerows..
            _totalsum = _totalsum + refund.cgi_Amount.Value;

            int vatCode = refund.cgi_vat_code != null ? (int)refund.cgi_vat_code : int.MinValue;

            string line7 = "";
            string line8 = "";
            string line9 = "";
            if (vatCode == int.MinValue)
            {
                line7 = _calculatenetamount(refund.cgi_Amount, null).SetToFixedLengthPadRight(16); //ex moms
                line8 = _calculateVatAmount(refund.cgi_Amount, null).SetToFixedLengthPadRight(16);
                line9 = _formatVatCode(null).SetToFixedLengthPadRight(2);
            }
            else
            {
                string vatName = getlabelFromValueOptionSet(vatCode); //Enum.GetName(typeof(Generated.cgi_refund_cgi_vat_code), vatCode.Value);
                line7 = _calculatenetamount(refund.cgi_Amount, vatName).SetToFixedLengthPadRight(16); //ex moms
                line8 = _calculateVatAmount(refund.cgi_Amount, vatName).SetToFixedLengthPadRight(16);
                line9 = _formatVatCode(vatName).SetToFixedLengthPadRight(2);
            }

            string line = string.Format($"{line1}{line2}{line3}{line4}{line5}{line6}{line7}{line8}{line9}");
            _countInvoince++;

            return line;
        }

        //Meddelandeuppgifter
        //TODO : contract never used
        private string _createInformationRecord(RefundEntity refund, IncidentEntity incident)
        {
            int vatCode = refund.cgi_vat_code != null ? (int)refund.cgi_vat_code : int.MinValue;

            string line1 = "04";
            string line2 = _formatString(incident.TicketNumber).SetToFixedLengthPadRight(50);

            string line3 = "";
            string line4 = "";

            if (vatCode == int.MinValue)
            {
                line3 = _calculatenetamount(refund.cgi_Amount, null).SetToFixedLengthPadRight(16); //ex moms
                line4 = _formatVatCode(null).SetToFixedLengthPadRight(2);
            }
            else
            {
                string vatName = getlabelFromValueOptionSet(vatCode);
                line3 = _calculatenetamount(refund.cgi_Amount, vatName).SetToFixedLengthPadRight(16); //ex moms
                line4 = _formatVatCode(vatName).SetToFixedLengthPadRight(2);
            }

            string line = string.Format("{0}{1}{2}{3}", line1, line2, line3, line4);
            return line;
        }

        //Konteringspost
        private string CreateAccountingRecord(RefundEntity refund, ContactEntity contact, IncidentEntity incident, RefundResponsibleEntity responsible, RefundProductEntity refundproduct, UserEntity user, RefundAccountEntity refundaccount)
        {
            string postmarkering = "09";

            string account = "";
            if (refundaccount != null)
                account = refundaccount.cgi_Account;

            string konto = _formatString(account).SetToFixedLengthPadRight(10);

            string ansvar;
            if (responsible != null)
                ansvar = _formatString(responsible.cgi_responsible).SetToFixedLengthPadRight(10);
            else
                ansvar = _formatString("").SetToFixedLengthPadRight(10);
            string proj = "".SetToFixedLengthPadRight(10);
            string aktiv = "".SetToFixedLengthPadRight(10);
            string objekt = "".SetToFixedLengthPadRight(10);
            string motpart = "150000".SetToFixedLengthPadRight(10);
            string momscode = "";
            string belopp = "";

            //string line8 = "";
            //string line10 = "";

            int vatCode = refund.cgi_vat_code != null ? (int)refund.cgi_vat_code : int.MinValue;

            if (vatCode == int.MinValue)
            {
                momscode = _formatVatCode(null).SetToFixedLengthPadRight(10);
                belopp = _calculatenetamount(refund.cgi_Amount, null).SetToFixedLengthPadRight(15); //ex moms
            }
            else
            {
                string vatName = getlabelFromValueOptionSet(vatCode);
                momscode = _formatVatCode(vatName).SetToFixedLengthPadRight(10);
                belopp = _calculatenetamount(refund.cgi_Amount, vatName).SetToFixedLengthPadRight(15); //ex moms
            }
            string blank1 = "".SetToFixedLengthPadRight(52);
            string kreditmarkering = "+".SetToFixedLengthPadRight(1);
            string blank2 = "".SetToFixedLengthPadRight(4);
            string bokforingstext = _formatString(incident.TicketNumber).SetToFixedLengthPadRight(30);
            string periodiseringsnyckel = "".SetToFixedLengthPadRight(10);
            string periodiseringfrom = "".SetToFixedLengthPadRight(10);
            string periodiseringto = "".SetToFixedLengthPadRight(10);

            string line6;
            if (refundproduct != null)
                line6 = _formatString(refundproduct.cgi_Account).SetToFixedLengthPadRight(10);
            else
                line6 = _formatString("").SetToFixedLengthPadRight(10);


            string attestSAK = (!string.IsNullOrEmpty(user.cgi_RSID)) ? user.cgi_RSID.SetToFixedLengthPadRight(10) : "".SetToFixedLengthPadRight(10);   // "".SetToFixedLength(10); //RSID
            string attestBES = "".SetToFixedLengthPadRight(10);

            //Special handling of adresses for RGOL cases
            string line9 = "";
            if (!string.IsNullOrWhiteSpace(incident.cgi_rgol_fullname))//if (incident.Caseorigincode == 285050007)
                line9 = _checkLenght(string.Format("{0} {1}", _formatString(incident.TicketNumber), _formatString(incident.cgi_rgol_fullname)).SetToFixedLengthPadRight(30));
            else
                line9 = _checkLenght(string.Format("{0} {1} {2}", _formatString(incident.TicketNumber), _formatString(contact.LastName), _formatString(contact.FirstName)).SetToFixedLengthPadRight(30));

            string line = string.Format($"{postmarkering}{konto}{ansvar}{proj}{aktiv}{objekt}{motpart}{momscode}{blank1}{belopp}{kreditmarkering}{blank2}{bokforingstext}{periodiseringsnyckel}{periodiseringfrom}{periodiseringto}{attestSAK}{attestBES}");
            return line;
        }

        //Konteringspost
        private string _createAcountingRecord(RefundEntity refund, ContactEntity contact, IncidentEntity incident, RefundResponsibleEntity responsible, RefundProductEntity refundproduct, UserEntity user, RefundAccountEntity refundaccount)
        {
            string _line1 = "05";

            string account = "";
            if (refundaccount != null)
                account = refundaccount.cgi_Account;

            string line2 = _formatString(account).SetToFixedLengthPadRight(10);

            string line3;
            if (responsible != null)
                line3 = _formatString(responsible.cgi_responsible).SetToFixedLengthPadRight(10);
            else
                line3 = _formatString("").SetToFixedLengthPadRight(10);

            string line4 = "1500".SetToFixedLengthPadRight(10);
            string line5 = "".SetToFixedLengthPadRight(10);

            string line6;
            if (refundproduct != null)
                line6 = _formatString(refundproduct.cgi_Account).SetToFixedLengthPadRight(10);
            else
                line6 = _formatString("").SetToFixedLengthPadRight(10);

            string line7 = "".SetToFixedLengthPadRight(10);
            string line8 = "";
            string line9 = "";
            string line10 = "";

            int vatCode = refund.cgi_vat_code != null ? (int)refund.cgi_vat_code : int.MinValue;

            if (vatCode == int.MinValue)
            {
                line8 = _formatVatCode(null).SetToFixedLengthPadRight(10);
                line10 = _calculatenetamount(refund.cgi_Amount, null).SetToFixedLengthPadRight(16); //ex moms
            }
            else
            {
                string vatName = getlabelFromValueOptionSet(vatCode);
                line8 = _formatVatCode(vatName).SetToFixedLengthPadRight(10);
                line10 = _calculatenetamount(refund.cgi_Amount, vatName).SetToFixedLengthPadRight(16); //ex moms
            }

            string line11 = (!string.IsNullOrEmpty(user.cgi_RSID)) ? user.cgi_RSID.SetToFixedLengthPadRight(10) : "".SetToFixedLengthPadRight(10);   // "".SetToFixedLength(10); //RSID
            string line12 = "".SetToFixedLengthPadRight(10);

            //Special handling of adresses for RGOL cases
            if (!string.IsNullOrWhiteSpace(incident.cgi_rgol_fullname))//if (incident.Caseorigincode == 285050007)
                line9 = _checkLenght(string.Format("{0} {1}", _formatString(incident.TicketNumber), _formatString(incident.cgi_rgol_fullname)).SetToFixedLengthPadRight(30));
            else
                line9 = _checkLenght(string.Format("{0} {1} {2}", _formatString(incident.TicketNumber), _formatString(contact.LastName), _formatString(contact.FirstName)).SetToFixedLengthPadRight(30));

            string line = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", _line1, line2, line3, line4, line5, line6, line7, line8, line9, line10, line11, line12);
            return line;
        }

        //Avslutningspost
        private string CreateAvslutningspost()
        {
            string postmarkering = "10";
            string antal = _countInvoince.ToString().PadLeft(13, '0');
            string summa = _formatSum(_totalsum, 15);
            string line = string.Format("{0}{1}{2}", postmarkering, antal, summa);
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
            _log.Debug("_createFile() File Name: " + filename);
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

            string footer = CreateAvslutningspost();
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

        private string _calculateVatAmount(Money amount, string vatcodename)
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
                returnValue = temp;
            }

            if (string.IsNullOrEmpty(socnr) && !string.IsNullOrEmpty(foreignnumber))
            {
                returnValue = foreignnumber;
            }

            return returnValue;
        }

        private string FormatVatCode(cgi_refund_cgi_vat_code? vatcode)
        {
            if (vatcode == null)
                return "".SetToFixedLengthPadRight(16);
            var vCode = "SE" + vatcode.ToString() + "01";
            return vCode/*.PadLeft(16, ' ')*/;
        }

        private string _formatSum(decimal sum, int pad)
        {
            var temp = sum.ToString("0.00");
            temp = temp.Replace(",", "").Replace(".", "").PadLeft(pad, '0');
            return temp;
        }

        private string getlabelFromValueOptionSet(int code)
        {
            return optionsMetadata.Where(x => x.Value == code).FirstOrDefault().Label.UserLocalizedLabel.Label;
        }

        private OptionMetadataCollection RetrieveOptionSetMetadata(string entityName, string attributename)
        {
            RetrieveAttributeRequest attributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributename,
                RetrieveAsIfPublished = true
            };

            var attributeResponse = (RetrieveAttributeResponse)localContext.OrganizationService.Execute(attributeRequest);
            EnumAttributeMetadata attributeMetadata = (EnumAttributeMetadata)attributeResponse.AttributeMetadata;

            return attributeMetadata.OptionSet.Options;
        }

        private List<RefundEntity> _getPendingRefunds()
        {
            ColumnSet columns = new ColumnSet(RefundEntity.Fields.cgi_refundId, RefundEntity.Fields.cgi_refundnumber, RefundEntity.Fields.CreatedOn, RefundEntity.Fields.CreatedBy, RefundEntity.Fields.cgi_Caseid,
                RefundEntity.Fields.cgi_vat_code, RefundEntity.Fields.cgi_value_code, RefundEntity.Fields.cgi_travelcard_number, RefundEntity.Fields.cgi_transportcompanyid,
                RefundEntity.Fields.cgi_taxi_company, RefundEntity.Fields.cgi_swift, RefundEntity.Fields.cgi_soc_sec_number, RefundEntity.Fields.cgi_responsibleId, RefundEntity.Fields.cgi_ReInvoicing,
                RefundEntity.Fields.cgi_ReimbursementFormid, RefundEntity.Fields.cgi_car_reg, RefundEntity.Fields.cgi_RefundTypeid, RefundEntity.Fields.cgi_Reference, RefundEntity.Fields.OverriddenCreatedOn,
                RefundEntity.Fields.cgi_Quantity, RefundEntity.Fields.cgi_Productid, RefundEntity.Fields.cgi_milage_compensation, RefundEntity.Fields.cgi_milage, RefundEntity.Fields.cgi_last_valid,
                RefundEntity.Fields.cgi_InvoiceRecipient, RefundEntity.Fields.cgi_iban, RefundEntity.Fields.cgi_foreign_payment, RefundEntity.Fields.cgi_ExportedRaindance, RefundEntity.Fields.TransactionCurrencyId,
                RefundEntity.Fields.cgi_Contactid, RefundEntity.Fields.cgi_comments, RefundEntity.Fields.cgi_Caseid, RefundEntity.Fields.cgi_Attestation, RefundEntity.Fields.cgi_amountwithtax_Base,
                RefundEntity.Fields.cgi_AmountwithTAX, RefundEntity.Fields.cgi_amount_Base, RefundEntity.Fields.cgi_Amount, RefundEntity.Fields.cgi_accountno, RefundEntity.Fields.cgi_Accountid);

            QueryExpression query_refund = new QueryExpression(RefundEntity.EntityLogicalName);
            query_refund.NoLock = true;
            query_refund.ColumnSet = columns;
            query_refund.AddOrder(RefundEntity.Fields.cgi_refundnumber, OrderType.Ascending);
            query_refund.Criteria.AddCondition(RefundEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.cgi_refundState.Active);
            query_refund.Criteria.AddCondition(RefundEntity.Fields.cgi_Attestation, ConditionOperator.Equal, (int)Generated.cgi_refund_cgi_attestation.Done);

            FilterExpression cgi_refund_Criteria0 = new FilterExpression();
            query_refund.Criteria.AddFilter(cgi_refund_Criteria0);

            cgi_refund_Criteria0.FilterOperator = LogicalOperator.Or;
            cgi_refund_Criteria0.AddCondition(RefundEntity.Fields.cgi_ExportedRaindance, ConditionOperator.Equal, false);
            cgi_refund_Criteria0.AddCondition(RefundEntity.Fields.cgi_ExportedRaindance, ConditionOperator.Null);

            LinkEntity cgi_refund_cgi_reimbursementformLinkEntity = query_refund.AddLink(ReimbursementFormEntity.EntityLogicalName, RefundEntity.Fields.cgi_ReimbursementFormid, ReimbursementFormEntity.Fields.cgi_reimbursementformId);
            cgi_refund_cgi_reimbursementformLinkEntity.EntityAlias = "ac";

            cgi_refund_cgi_reimbursementformLinkEntity.LinkCriteria.AddCondition(ReimbursementFormEntity.Fields.cgi_attestation, ConditionOperator.Equal, true);
            cgi_refund_cgi_reimbursementformLinkEntity.LinkCriteria.AddCondition(ReimbursementFormEntity.Fields.cgi_payment, ConditionOperator.Equal, true);

            return XrmRetrieveHelper.RetrieveMultiple<RefundEntity>(localContext, query_refund);
        }

        private ContactEntity _getCurrentContact(Guid contactid)
        {
            if (contactid == Guid.Empty || contactid == null)
            {
                _log.Error("The Contact was empty: " + contactid.ToString());
                return null;
            }

            ColumnSet columns = new ColumnSet(ContactEntity.Fields.ContactId, ContactEntity.Fields.LastName, ContactEntity.Fields.FirstName,
                ContactEntity.Fields.Address1_Line2, ContactEntity.Fields.Address1_City, ContactEntity.Fields.Address1_PostalCode, ContactEntity.Fields.cgi_ContactNumber);
            return XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, ContactEntity.EntityLogicalName, contactid, columns);
        }

        private IncidentEntity _getCurrentIncident(Guid caseid)
        {
            ColumnSet columns = new ColumnSet(IncidentEntity.Fields.TicketNumber, IncidentEntity.Fields.cgi_Contactid, IncidentEntity.Fields.cgi_Accountid,
                IncidentEntity.Fields.cgi_rgol_address1_line1, IncidentEntity.Fields.cgi_rgol_address1_line2, IncidentEntity.Fields.cgi_rgol_address1_postalcode,
                IncidentEntity.Fields.cgi_rgol_address1_city, IncidentEntity.Fields.cgi_rgol_address1_country, IncidentEntity.Fields.cgi_rgol_fullname,
                IncidentEntity.Fields.cgi_soc_sec_number, IncidentEntity.Fields.cgi_rgol_socialsecuritynumber, IncidentEntity.Fields.CaseOriginCode);
            return XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, IncidentEntity.EntityLogicalName, caseid, columns);
        }

        private RefundResponsibleEntity _getCurrentResponsible(Guid responsibleid)
        {
            if (responsibleid == Guid.Empty || responsibleid == null)
            {
                _log.Error("The Responsible was empty: " + responsibleid.ToString());
                return null;
            }

            ColumnSet columns = new ColumnSet(RefundResponsibleEntity.Fields.cgi_responsible);
            return XrmRetrieveHelper.Retrieve<RefundResponsibleEntity>(localContext, RefundResponsibleEntity.EntityLogicalName, responsibleid, columns);
        }

        private RefundProductEntity _getCurrentRefundProduct(Guid refundproductid)
        {
            if (refundproductid == Guid.Empty || refundproductid == null)
            {
                _log.Error("The Refund Product was empty: " + refundproductid.ToString());
                return null;
            }

            ColumnSet columns = new ColumnSet(RefundProductEntity.Fields.cgi_refundproductname, RefundProductEntity.Fields.cgi_Account);
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(RefundProductEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.cgi_refundproductState.Active);
            filter.AddCondition(RefundProductEntity.Fields.cgi_refundproductId, ConditionOperator.Equal, refundproductid);

            return XrmRetrieveHelper.RetrieveFirst<RefundProductEntity>(localContext, columns, filter);
        }

        private UserEntity _getUser(Guid userid)
        {
            ColumnSet columns = new ColumnSet(UserEntity.Fields.cgi_RSID);
            return XrmRetrieveHelper.Retrieve<UserEntity>(localContext, UserEntity.EntityLogicalName, userid, columns);
        }

        private RefundAccountEntity _getRefundAccount(Guid refundaccountid)
        {
            if (refundaccountid == Guid.Empty || refundaccountid == null)
            {
                _log.Error("The Refund Account was empty: " + refundaccountid.ToString());
                return null;
            }

            ColumnSet columns = new ColumnSet(RefundAccountEntity.Fields.cgi_refundaccountname, RefundAccountEntity.Fields.cgi_Account, RefundAccountEntity.Fields.cgi_refundaccountId);
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(RefundAccountEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.cgi_refundaccountState.Active);
            filter.AddCondition(RefundAccountEntity.Fields.cgi_refundaccountId, ConditionOperator.Equal, refundaccountid);

            return XrmRetrieveHelper.RetrieveFirst<RefundAccountEntity>(localContext, columns, filter);
        }

        private void _logErrorOnRefund(Guid refundid, string ex)
        {
            _log.Error("_logErrorOnRefund: " + ex);

            RefundEntity eRefund = new RefundEntity();
            eRefund.Id = refundid;
            eRefund.cgi_ExportMessage = ex;
            eRefund.cgi_ExportedRaindance = false;
            eRefund.cgi_ExportDate = DateTime.Now;

            XrmHelper.Update(localContext, eRefund);
        }

        private void _setRecordToExported(Guid refundid)
        {
            RefundEntity eRefund = new RefundEntity();
            eRefund.Id = refundid;
            eRefund.cgi_ExportMessage = "";
            eRefund.cgi_ExportedRaindance = true;
            eRefund.cgi_ExportDate = DateTime.Now;

            XrmHelper.Update(localContext, eRefund);
        }

        private static Plugin.LocalPluginContext GenerateLocalContext()
        {
            try
            {
                _log.Debug("Trying to get the Connection to Dynamics.");

                // Connect to the CRM web service using a connection string.
                CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(RunBatch.CredentialFilePath, RunBatch.Entropy));

                // Cast the proxy client to the IOrganizationService interface.
                IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                if (serviceProxy == null)
                    _log.Error("Connection to Dynamics failed.");
                else
                    _log.Info("Connection to Dynamics succeeded.");

                return new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());
            }
            catch (Exception e)
            {
                _log.Error("Error while initiating GenerateLocalContext. " + e.Message);
                throw new Exception("Error while initiating GenerateLocalContext. " + e.Message);
            }
        }

        public string FormatPostnummer(string postnummer)
        {
            if (string.IsNullOrEmpty(postnummer))
                return "";

            postnummer = postnummer.Replace(" ", "").Trim();
            if (postnummer.Length >= 5)
                postnummer = $"{postnummer.Substring(0, 3)} {postnummer.Substring(3)}";
            return postnummer;
        }

        private string _convertToRandanceVatCode(string vat)
        {
            if (vat == "25")
                return "K25";
            else if (vat == "12")
                return "K12";
            else if (vat == "6UT")
                return "K06";
            else if (vat == "6IN")
                return "K06";
            else
                return "K00";
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
            xml += "           <condition attribute='cgi_attestation' operator='eq' value='285050004' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_exportedraindance' operator='eq' value='0' />";
            xml += "               <condition attribute='cgi_exportedraindance' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "       <link-entity name='cgi_reimbursementform' from='cgi_reimbursementformid' to='cgi_reimbursementformid' alias='ac'>";
            xml += "           <filter type='and'>";
            xml += "               <condition attribute='cgi_attestation' operator='eq' value='1' />";
            xml += "               <condition attribute='cgi_payment' operator='eq' value='1' />";
            xml += "           </filter>";
            xml += "       </link-entity>";
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