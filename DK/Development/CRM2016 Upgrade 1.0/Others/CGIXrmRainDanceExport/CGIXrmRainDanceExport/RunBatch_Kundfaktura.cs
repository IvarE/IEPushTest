using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.Collections.ObjectModel;

using System.IO;

using CGIXrmRainDanceExport.Classes;
using System.Text.RegularExpressions;
using Microsoft.Xrm.Sdk.Query;
using System.Globalization;
using Endeavor.Crm;
using Microsoft.Xrm.Tooling.Connector;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Messages;

namespace CGIXrmRainDanceExport
{
    public class RunBatch_Kundfaktura
    {
        #region Declarations
        string _fileName = "";
        int _countInvoince;
        decimal _totalsum;
        Plugin.LocalPluginContext localContext = null;
        OptionMetadataCollection optionsMetadata = null;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructors
        public RunBatch_Kundfaktura()
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

                _fileName = string.Format("{0}\\DK_Kundfaktura_{1}", exportdir, now + ".txt");
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
                    int vatCode = refund.cgi_vat_code != null ? (int)refund.cgi_vat_code : int.MinValue;

                    if (vatCode != int.MinValue)
                    {
                        string vatName = getlabelFromValueOptionSet(vatCode);
                        _log.Debug(string.Format("Refundnr: {0}; amount: {1}; vat code name: {2}; net amount: {3}", refund.cgi_refundnumber, refund.cgi_Amount.Value.ToString("#,0.00", CultureInfo.CreateSpecificCulture("sv-SE")), vatName, _calculatenetamount(refund.cgi_Amount, vatName)));
                    }

                    try
                    {
                        if (refund.cgi_Caseid != null)
                        {
                            IncidentEntity incident = _getCurrentIncident(refund.cgi_Caseid.Id);
                            ContactEntity contact = _getCurrentContact(incident.cgi_Contactid != null ? incident.cgi_Contactid.Id : Guid.Empty);
                            RefundResponsibleEntity responsible = _getCurrentResponsible(refund.cgi_responsibleId != null ? refund.cgi_responsibleId.Id : Guid.Empty);
                            RefundProductEntity refundproduct = _getCurrentRefundProduct(refund.cgi_Productid != null ? refund.cgi_Productid.Id : Guid.Empty);
                            UserEntity user = _getUser(refund.CreatedBy.Id);
                            RefundAccountEntity refundaccount = _getRefundAccount(refund.cgi_Accountid != null ? refund.cgi_Accountid.Id : Guid.Empty);
                            InvoiceRecipientEntity invoicerecipient = _getCurrentInvoiceRecipient(refund.cgi_InvoiceRecipient != null ? refund.cgi_InvoiceRecipient.Id : Guid.Empty);

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

                                        _setRecordToExported((Guid)refund.cgi_refundId);
                                        _log.Debug(string.Format("Refund exported: {0} | CaseId: {1}", _formatString(refund.cgi_refundnumber), _formatString(refund.cgi_Caseid.Id.ToString())));

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
                                        _logErrorOnRefund((Guid)refund.cgi_refundId, "Ingen fakturamottagare hittas på ersättningsposten.");
                                }
                                else
                                    _logErrorOnRefund((Guid)refund.cgi_refundId, "Ingen ärendekoppling hittas på ersättningsposten.");
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
        private string _createInvoiceRecord(RefundEntity refund, InvoiceRecipientEntity _invoicerecipient, IncidentEntity incident)
        {
            string reinvoicingphonenumber;
            GetSettings(localContext.OrganizationService, out reinvoicingphonenumber);

            string line1 = "03";
            string line2 = _formatString(_invoicerecipient.cgi_customer_no).SetToFixedLengthPadRight(15);
            string line3 = _formatCreateDate(DateTime.Now).SetToFixedLengthPadRight(8);
            string line4 = "".SetToFixedLengthPadRight(8);
            string line5 = _formatCreateDate(DateTime.Now).SetToFixedLengthPadRight(8);
            string line6 = "".SetToFixedLengthPadRight(8);
            string line7 = _formatString(_invoicerecipient.cgi_inv_reference).SetToFixedLengthPadRight(50);
            string line8 = string.Format("Handläggare: {0}", incident.CreatedBy.Name).SetToFixedLengthPadRight(50);
            string line9 = string.Format("Telefonnummer: {0}", reinvoicingphonenumber).SetToFixedLengthPadRight(50);

            //Calculate totalsum of all invoivcerows..
            _totalsum = _totalsum + refund.cgi_Amount.Value;

            string line = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", line1, line2, line3, line4, line5, line6, line7, line8, line9);
            _countInvoince++;

            return line;
        }

        //Meddelandeuppgifter
        //TODO : unsued variable refund
        private IEnumerable<string> _createInformationRecord(RefundEntity refund, IncidentEntity incident)
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
        private string _createInvoiceRowRecord(RefundEntity refund, IncidentEntity incident)
        {
            string line1 = "05";
            string line2 = string.Format("Ärendenr: {0}", _formatString(incident.TicketNumber)).SetToFixedLengthPadRight(50);

            int vatCode = refund.cgi_vat_code != null ? (int)refund.cgi_vat_code : int.MinValue;

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
        private string _createAcountingRecord(RefundEntity refund, ContactEntity contact, IncidentEntity incident, RefundResponsibleEntity responsible, RefundProductEntity refundproduct, UserEntity user, RefundAccountEntity refundaccount)
        {
            string line1 = "06";

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

            string line9 = _checkLenght(string.Format("{0} {1} {2}", _formatString(incident.TicketNumber), _formatString(contact.LastName), _formatString(contact.FirstName)).SetToFixedLengthPadRight(30));
            string line11 = (!string.IsNullOrEmpty(user.cgi_RSID)) ? user.cgi_RSID.SetToFixedLengthPadRight(10) : "".SetToFixedLengthPadRight(10); //RSID
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
            ColumnSet columns = new ColumnSet(RefundEntity.Fields.cgi_refundId, RefundEntity.Fields.cgi_refundnumber, RefundEntity.Fields.CreatedOn, RefundEntity.Fields.CreatedBy,
                RefundEntity.Fields.cgi_vat_code, RefundEntity.Fields.cgi_value_code, RefundEntity.Fields.cgi_travelcard_number, RefundEntity.Fields.cgi_transportcompanyid,
                RefundEntity.Fields.cgi_taxi_company, RefundEntity.Fields.cgi_swift, RefundEntity.Fields.cgi_soc_sec_number, RefundEntity.Fields.cgi_responsibleId, RefundEntity.Fields.cgi_ReInvoicing,
                RefundEntity.Fields.cgi_ReimbursementFormid, RefundEntity.Fields.cgi_car_reg, RefundEntity.Fields.cgi_RefundTypeid, RefundEntity.Fields.cgi_Reference, RefundEntity.Fields.OverriddenCreatedOn,
                RefundEntity.Fields.cgi_Quantity, RefundEntity.Fields.cgi_Productid, RefundEntity.Fields.cgi_milage_compensation, RefundEntity.Fields.cgi_milage, RefundEntity.Fields.cgi_last_valid,
                RefundEntity.Fields.cgi_InvoiceRecipient, RefundEntity.Fields.cgi_iban, RefundEntity.Fields.cgi_foreign_payment, RefundEntity.Fields.TransactionCurrencyId, RefundEntity.Fields.cgi_Contactid,
                RefundEntity.Fields.cgi_comments, RefundEntity.Fields.cgi_Caseid, RefundEntity.Fields.cgi_Attestation, RefundEntity.Fields.cgi_amountwithtax_Base, RefundEntity.Fields.cgi_AmountwithTAX,
                RefundEntity.Fields.cgi_amount_Base, RefundEntity.Fields.cgi_Amount, RefundEntity.Fields.cgi_accountno, RefundEntity.Fields.cgi_Accountid, RefundEntity.Fields.cgi_invoiceexportedraindance,
                RefundEntity.Fields.cgi_InvoiceRecipient);

            QueryExpression query_refund = new QueryExpression(RefundEntity.EntityLogicalName);
            query_refund.NoLock = true;
            query_refund.ColumnSet = columns;
            query_refund.AddOrder(RefundEntity.Fields.cgi_refundnumber, OrderType.Ascending);
            query_refund.Criteria.AddCondition(RefundEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.cgi_refundState.Active);
            query_refund.Criteria.AddCondition(RefundEntity.Fields.cgi_ReInvoicing, ConditionOperator.Equal, true);
            query_refund.Criteria.AddCondition(RefundEntity.Fields.cgi_Amount, ConditionOperator.GreaterThan, 0);

            FilterExpression cgi_refund_Criteria0 = new FilterExpression();
            query_refund.Criteria.AddFilter(cgi_refund_Criteria0);

            cgi_refund_Criteria0.FilterOperator = LogicalOperator.Or;
            cgi_refund_Criteria0.AddCondition(RefundEntity.Fields.cgi_invoiceexportedraindance, ConditionOperator.Equal, false);
            cgi_refund_Criteria0.AddCondition(RefundEntity.Fields.cgi_invoiceexportedraindance, ConditionOperator.Null);

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
                ContactEntity.Fields.Address1_Line2, ContactEntity.Fields.Address1_City, ContactEntity.Fields.Address1_PostalCode);
            return XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, ContactEntity.EntityLogicalName, contactid, columns);
        }

        private InvoiceRecipientEntity _getCurrentInvoiceRecipient(Guid invoicerecipientid)
        {
            if (invoicerecipientid == Guid.Empty || invoicerecipientid == null)
            {
                _log.Error("The Invoice Recipient was empty: " + invoicerecipientid.ToString());
                return null;
            }

            ColumnSet columns = new ColumnSet(InvoiceRecipientEntity.Fields.cgi_invoicerecipientId, InvoiceRecipientEntity.Fields.cgi_address1, InvoiceRecipientEntity.Fields.cgi_customer_no,
                InvoiceRecipientEntity.Fields.cgi_inv_reference, InvoiceRecipientEntity.Fields.cgi_invoicerecipientname, InvoiceRecipientEntity.Fields.cgi_postal_city, InvoiceRecipientEntity.Fields.cgi_postalcode);
            return XrmRetrieveHelper.Retrieve<InvoiceRecipientEntity>(localContext, InvoiceRecipientEntity.EntityLogicalName, invoicerecipientid, columns);
        }

        private IncidentEntity _getCurrentIncident(Guid caseid)
        {
            ColumnSet columns = new ColumnSet(IncidentEntity.Fields.TicketNumber, IncidentEntity.Fields.cgi_Contactid, IncidentEntity.Fields.cgi_Accountid,
                IncidentEntity.Fields.Description, IncidentEntity.Fields.CreatedBy);
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

            try
            {
                RefundEntity refund = new RefundEntity();
                refund.Id = refundid;
                refund.cgi_invoiceexportmessage = ex;
                refund.cgi_invoiceexportedraindance = false;
                refund.cgi_invoiceexportdate = DateTime.Now;

                XrmHelper.Update(localContext, refund);
            }
            catch (Exception exe)
            {
                EntityReference refundRef = new EntityReference(RefundEntity.EntityLogicalName, refundid);
                _CreateAnnotation("ExportError", string.Format("Error occured when logging error on refund: {0}", exe.Message), refundRef);
                _CreateAnnotation("ExportLogMessage", string.Format("{0}", exe), refundRef);
            }
        }

        private void _CreateAnnotation(string subject, string notetext, EntityReference reference)
        {
            AnnotationEntity annotation = new AnnotationEntity();
            annotation.ObjectId = reference;
            annotation.Subject = subject;
            annotation.NoteText = notetext;

            XrmHelper.Update(localContext, annotation);
        }

        private void _setRecordToExported(Guid refundid)
        {
            RefundEntity refund = new RefundEntity();
            refund.Id = refundid;
            refund.cgi_invoiceexportmessage = "";
            refund.cgi_invoiceexportedraindance = true;
            refund.cgi_invoiceexportdate = DateTime.Now;

            //bugbug uncomment this line
            XrmHelper.Update(localContext, refund);
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
                    _log.Error("Connection to Dynamics succeeded.");

                return new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());
            }
            catch (Exception e)
            {
                _log.Error("Error while initiating GenerateLocalContext. " + e.Message);
                throw new Exception("Error while initiating GenerateLocalContext. " + e.Message);
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
