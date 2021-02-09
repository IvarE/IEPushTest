using System;
using Generic = System.Collections.Generic;
using System.Linq;
using System.Web;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.ObjectModel;
using System.Threading;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.ServiceModel;
using CGIXrmLogger;
using CGICRMPortalService.Models;
using Microsoft.Crm.Sdk.Messages;

namespace CGICRMPortalService
{
    public class TravelCardManager : Response
    {
        #region Global Variables
        private XrmManager xrmMgr;
        private XrmHelper xrmHelper;
        LogToCrm log2Crm = new LogToCrm();
        #endregion

        #region Constructors
        public TravelCardManager()
        {
            xrmHelper = new XrmHelper();
            xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
            log2Crm = new LogToCrm();
            _createLogfile("Enter TravelCardManager with no params");
        }

        public TravelCardManager(Guid callerId)
        {
            xrmHelper = new XrmHelper();
            xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(callerId);
            log2Crm = new LogToCrm();
            _createLogfile("Enter TravelCardManager with params");
        }
        #endregion

        #region Private Methods
        private Entity GetCrmTravelCardEntityFromTravelCard(TravelCard travelCard, bool bUpdate)
        {
            //Guid accountId = xrmHelper.GetIdByValue(accountnumber, "accountnumber", "account", xrmMgr);
            Entity crmTravelCardEntity = new Entity("cgi_travelcard");
            if (bUpdate)
            {
                Guid travelCardId = GetTravelCardID(travelCard);
                crmTravelCardEntity.Id = travelCardId;
                //crmTravelCardEntity.Attributes.Add("cgi_travelcardid",);

            }
            else
            {
                if (travelCard.CustomerType == AccountCategoryCode.Company)
                {
                    crmTravelCardEntity.Attributes.Add("cgi_accountid", new EntityReference("account", travelCard.AccountId));
                }
                else
                {
                    crmTravelCardEntity.Attributes.Add("cgi_contactid", new EntityReference("contact", travelCard.AccountId));
                }
            }
            crmTravelCardEntity.Attributes.Add("cgi_travelcardnumber", travelCard.CardNumber);
            crmTravelCardEntity.Attributes.Add("cgi_travelcardname", travelCard.CardName);
            crmTravelCardEntity.Attributes.Add("cgi_periodic_card_type", travelCard.PeriodCardTypeTitle);

            if (travelCard.PeriodValidFrom != DateTime.MinValue)
            {
                crmTravelCardEntity.Attributes.Add("cgi_validfrom", travelCard.PeriodValidFrom);
            }
            if (travelCard.PeriodValidTo != DateTime.MinValue)
            {
                crmTravelCardEntity.Attributes.Add("cgi_validto", travelCard.PeriodValidTo);
            }
            crmTravelCardEntity.Attributes.Add("cgi_value_card_type", travelCard.ValueCardTypeTitle);

            crmTravelCardEntity.Attributes.Add("cgi_blocked", travelCard.Blocked);

            if (travelCard.AutoloadConnectionDate != DateTime.MinValue)
                crmTravelCardEntity.Attributes.Add("cgi_autoloadconnectiondate", travelCard.AutoloadConnectionDate);

            if (travelCard.AutoloadDisconnectionDate != DateTime.MinValue)
                crmTravelCardEntity.Attributes.Add("cgi_autoloaddisconnectiondate", travelCard.AutoloadDisconnectionDate);

            crmTravelCardEntity.Attributes.Add("cgi_autoloadstatus", travelCard.AutoloadStatus);
            crmTravelCardEntity.Attributes.Add("cgi_cardcategory", travelCard.CardCategory);
            crmTravelCardEntity.Attributes.Add("cgi_creditcardmask", travelCard.CreditCardMask);
            crmTravelCardEntity.Attributes.Add("cgi_currency", travelCard.Currency);
            crmTravelCardEntity.Attributes.Add("cgi_failedattemptstochargemoney", travelCard.FailedAttemptsToChargeMoney);
            crmTravelCardEntity.Attributes.Add("cgi_latestautoloadamount", new Money(travelCard.LatestAutoloadAmount));

            if (travelCard.LatestChargeDate != DateTime.MinValue)
                crmTravelCardEntity.Attributes.Add("cgi_latestchargedate", travelCard.LatestChargeDate);

            if (travelCard.LatestFailedAttempt != DateTime.MinValue)
                crmTravelCardEntity.Attributes.Add("cgi_latestfailedattempt", travelCard.LatestFailedAttempt);

            crmTravelCardEntity.Attributes.Add("cgi_periodcardtypeid", travelCard.PeriodCardTypeId);
            crmTravelCardEntity.Attributes.Add("cgi_valuecardtypeid", travelCard.ValueCardTypeId);
            crmTravelCardEntity.Attributes.Add("cgi_verifyid", travelCard.VerifyId);

            return crmTravelCardEntity;
        }

        private Entity PopulateCrmTravelCardEntityFromTravelCard(TravelCard travelCard)
        {
            Entity crmTravelCardEntity = new Entity("cgi_travelcard");

            crmTravelCardEntity.Id = GetTravelCardID(travelCard);

            if (travelCard.CustomerType == AccountCategoryCode.Company)
            {
                crmTravelCardEntity.Attributes.Add("cgi_accountid", new EntityReference("account", travelCard.AccountId));
                crmTravelCardEntity.Attributes.Add("cgi_contactid", null);
            }
            else
            {
                crmTravelCardEntity.Attributes.Add("cgi_contactid", new EntityReference("contact", travelCard.AccountId));
                crmTravelCardEntity.Attributes.Add("cgi_accountid", null);
            }

            crmTravelCardEntity.Attributes.Add("cgi_travelcardnumber", travelCard.CardNumber);
            crmTravelCardEntity.Attributes.Add("cgi_travelcardname", travelCard.CardName);
            crmTravelCardEntity.Attributes.Add("cgi_periodic_card_type", travelCard.PeriodCardTypeTitle);

            if (travelCard.PeriodValidFrom != DateTime.MinValue)
            {
                crmTravelCardEntity.Attributes.Add("cgi_validfrom", travelCard.PeriodValidFrom);
            }
            if (travelCard.PeriodValidTo != DateTime.MinValue)
            {
                crmTravelCardEntity.Attributes.Add("cgi_validto", travelCard.PeriodValidTo);
            }
            crmTravelCardEntity.Attributes.Add("cgi_value_card_type", travelCard.ValueCardTypeTitle);

            crmTravelCardEntity.Attributes.Add("cgi_blocked", travelCard.Blocked);

            if (travelCard.AutoloadConnectionDate != DateTime.MinValue)
                crmTravelCardEntity.Attributes.Add("cgi_autoloadconnectiondate", travelCard.AutoloadConnectionDate);

            if (travelCard.AutoloadDisconnectionDate != DateTime.MinValue)
                crmTravelCardEntity.Attributes.Add("cgi_autoloaddisconnectiondate", travelCard.AutoloadDisconnectionDate);

            crmTravelCardEntity.Attributes.Add("cgi_autoloadstatus", travelCard.AutoloadStatus);
            crmTravelCardEntity.Attributes.Add("cgi_cardcategory", travelCard.CardCategory);
            crmTravelCardEntity.Attributes.Add("cgi_creditcardmask", travelCard.CreditCardMask);
            crmTravelCardEntity.Attributes.Add("cgi_currency", travelCard.Currency);
            crmTravelCardEntity.Attributes.Add("cgi_failedattemptstochargemoney", travelCard.FailedAttemptsToChargeMoney);
            crmTravelCardEntity.Attributes.Add("cgi_latestautoloadamount", new Money(travelCard.LatestAutoloadAmount));

            if (travelCard.LatestChargeDate != DateTime.MinValue)
                crmTravelCardEntity.Attributes.Add("cgi_latestchargedate", travelCard.LatestChargeDate);

            if (travelCard.LatestFailedAttempt != DateTime.MinValue)
                crmTravelCardEntity.Attributes.Add("cgi_latestfailedattempt", travelCard.LatestFailedAttempt);

            crmTravelCardEntity.Attributes.Add("cgi_periodcardtypeid", travelCard.PeriodCardTypeId);
            crmTravelCardEntity.Attributes.Add("cgi_valuecardtypeid", travelCard.ValueCardTypeId);
            crmTravelCardEntity.Attributes.Add("cgi_verifyid", travelCard.VerifyId);

            return crmTravelCardEntity;
        }

        private Guid GetTravelCardID(TravelCard travelCard)
        {
            return GetTravelCardID(travelCard.CardNumber);
        }

        private Guid GetTravelCardID(string travelCardNumber)
        {
            Guid travelCardId = Guid.Empty;
            QueryByAttribute queryByAttribute = new QueryByAttribute("cgi_travelcard");
            queryByAttribute.AddAttributeValue("cgi_travelcardnumber", travelCardNumber);
            queryByAttribute.AddAttributeValue("statecode", 0);
            queryByAttribute.ColumnSet = new ColumnSet(new string[] { "cgi_travelcardid" });
            ObservableCollection<CrmTravelCard> travelCardCollection = xrmMgr.Get<CrmTravelCard>(queryByAttribute);
            travelCardId = Guid.NewGuid();
            if (travelCardCollection != null && travelCardCollection.Count > 0)
            {
                travelCardId = travelCardCollection.FirstOrDefault().TravelCardId;
            }
            return travelCardId;
        }


        private TravelCard GetTravelCardFromCrmTravelCard(CrmTravelCard crmTravelCard)
        {
            TravelCard travelcard = new TravelCard
            {
                AccountId = crmTravelCard.Account != null ? crmTravelCard.Account.Id : (crmTravelCard.Contact != null ? crmTravelCard.Contact.Id : Guid.Empty),
                CardName = crmTravelCard.CardName,
                CardNumber = crmTravelCard.CardNumber,
                //CardType = crmTravelCard.CardTypeName,
                CustomerType = crmTravelCard.Account != null ? AccountCategoryCode.Company : AccountCategoryCode.Private,
                PeriodCardTypeTitle = crmTravelCard.PeriodCardTypeTitle,
                ValueCardTypeTitle = crmTravelCard.ValueCardTypeTitle,
                PeriodValidFrom = crmTravelCard.PeriodValidFrom,
                PeriodValidTo = crmTravelCard.PeriodValidTo,
                Blocked = crmTravelCard.Blocked,
                AutoloadConnectionDate = crmTravelCard.AutoloadConnectionDate,
                AutoloadDisconnectionDate = crmTravelCard.AutoloadDisconnectionDate,
                AutoloadStatus = crmTravelCard.AutoloadStatus,
                CardCategory = crmTravelCard.CardCategory,
                CreditCardMask = crmTravelCard.CreditCardMask,
                Currency = crmTravelCard.Currency,
                FailedAttemptsToChargeMoney = crmTravelCard.FailedAttemptsToChargeMoney,
                LatestAutoloadAmount = crmTravelCard.LatestAutoloadAmount != null ? crmTravelCard.LatestAutoloadAmount.Value : 0,
                LatestChargeDate = crmTravelCard.LatestChargeDate,
                LatestFailedAttempt = crmTravelCard.LatestFailedAttempt,
                PeriodCardTypeId = crmTravelCard.PeriodCardTypeId,
                ValueCardTypeId = crmTravelCard.ValueCardTypeId,
                VerifyId = crmTravelCard.VerifyId
            };
            return travelcard;
        }

        private bool IsCardExistingAndNotRegistered(string travelCardNumber)
        {
            // This do involve statecode as per design.
            bool isExisting = false;

            QueryByAttribute queryByAttribute = new QueryByAttribute("cgi_travelcard");
            queryByAttribute.TopCount = 1;
            queryByAttribute.AddAttributeValue("cgi_travelcardnumber", travelCardNumber);
            queryByAttribute.AddAttributeValue("cgi_accountid", null);
            queryByAttribute.AddAttributeValue("cgi_contactid", null);
            queryByAttribute.AddAttributeValue("statecode", 0); // 0 active
            queryByAttribute.ColumnSet = new ColumnSet(new string[] { "cgi_travelcardid" });

            ObservableCollection<CrmTravelCard> travelCardCollection = xrmMgr.Get<CrmTravelCard>(queryByAttribute);

            if (travelCardCollection != null && travelCardCollection.Count > 0)
            {
                isExisting = true;
            }

            return isExisting;
        }

        private bool IsCardExisting(string travelCardNumber)
        {
            // This do involve statecode as per design.
            bool isExisting = false;

            QueryByAttribute queryByAttribute = new QueryByAttribute("cgi_travelcard");
            queryByAttribute.TopCount = 1;
            queryByAttribute.AddAttributeValue("cgi_travelcardnumber", travelCardNumber);
            queryByAttribute.AddAttributeValue("statecode", 0); // 0 active
            queryByAttribute.ColumnSet = new ColumnSet(new string[] { "cgi_travelcardid" });

            ObservableCollection<CrmTravelCard> travelCardCollection = xrmMgr.Get<CrmTravelCard>(queryByAttribute);

            if (travelCardCollection != null && travelCardCollection.Count > 0)
            {
                isExisting = true;
            }

            return isExisting;
        }

        private bool IsCardRegisteredToSuppliedCustomer(Guid customerId, string travelCardNumber, AccountCategoryCode customerType, out Guid travelCardId)
        {
            // This do involve statecode as per design

            bool isAlreadyRegistered = false;
            travelCardId = Guid.Empty;

            QueryByAttribute queryByAttribute = new QueryByAttribute("cgi_travelcard");
            switch (customerType)
            {
                case AccountCategoryCode.Private:
                    queryByAttribute.AddAttributeValue("cgi_contactid", customerId);
                    break;
                case AccountCategoryCode.Company:
                    queryByAttribute.AddAttributeValue("cgi_accountid", customerId);
                    break;
                default:
                    break;
            }

            queryByAttribute.AddAttributeValue("statecode", 0); // 0 active
            queryByAttribute.AddAttributeValue("cgi_travelcardnumber", travelCardNumber);
            queryByAttribute.ColumnSet = new ColumnSet(new string[] { "cgi_travelcardid" });
            ObservableCollection<CrmTravelCard> travelCardCollection = xrmMgr.Get<CrmTravelCard>(queryByAttribute);
            
            if (travelCardCollection != null && travelCardCollection.Count > 0)
            {
                travelCardId = travelCardCollection.FirstOrDefault().TravelCardId;
                isAlreadyRegistered = true;
            }

            return isAlreadyRegistered;
        }

        private void _createLogfile(string input)
        {
            try
            {
                StreamWriter _sw = new StreamWriter("C:\\Temp\\travelcard.log", true);
                string _message = string.Format("{0} : {1}", DateTime.Now.ToString(), input);
                _sw.WriteLine(_message);
                _sw.Flush();
                _sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Public Methods
        internal RegisterTravelCardResponse RegisterTravelCard_CreateOrUpdate(TravelCard travelCard)
        {
            // NOTE: there could (possibly) be occations where users of Sekund use that system in a way that do not
            // follow instructions. If hard to find bugs accour, this is proberbly the reason. More development is 
            // then needed to make sure Sekund usage could only be performed the instructed way. ( plugins that block etc )
            _createLogfile("Start registertravelcard");
            RegisterTravelCardResponse regTravelCardResponse = new RegisterTravelCardResponse();
            Entity crmTravelCard;
            
            try
            {
                if (IsCardExisting(travelCard.CardNumber) == false)
                {
                    // Check if card does not exist

                    crmTravelCard = GetCrmTravelCardEntityFromTravelCard(travelCard, false);
                    Guid crmTravelCardId = xrmMgr.Service.Create(crmTravelCard);
                    regTravelCardResponse.Status = ProcessingStatus.SUCCESS;
                    _createLogfile("Travelcard registered (created) with id " + crmTravelCardId);

                }
                else if (IsCardExistingAndNotRegistered(travelCard.CardNumber) == true)
                {
                    // Check if card exist and that cgi_accountid and cgi_contactid is null
                    // NOTE: if card is inactive, update will throw a error message. ( but IsCardExistingAndNotRegistered checks active state )

                    crmTravelCard = PopulateCrmTravelCardEntityFromTravelCard(travelCard);
                    xrmMgr.Service.Update(crmTravelCard);
                    regTravelCardResponse.Status = ProcessingStatus.SUCCESS;
                    _createLogfile("Travelcard registered (updated) with id " + crmTravelCard.Id);
                }
                else
                {
                    regTravelCardResponse.Status = ProcessingStatus.FAILED;
                    regTravelCardResponse.Message = "Card is already Registered";
                    _createLogfile("Travelcard registerd failed. card already exists.");
                }
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                //throw new Exception(ExceptionMsg, ex);
                regTravelCardResponse.Status = ProcessingStatus.FAILED;
                regTravelCardResponse.Message = ExceptionMsg;
                _createLogfile(ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                regTravelCardResponse.Status = ProcessingStatus.FAILED;
                regTravelCardResponse.Message = ExceptionMsg;
                //throw new Exception(ExceptionMsg, ex);
                _createLogfile(ex.Message);
            }
            catch (System.Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string ExceptionMsg = ex.InnerException.Message;

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe != null)
                    {
                        ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                        //chkCustomerExistResponse.Status = ProcessingStatus.FAILED;
                        //chkCustomerExistResponse.Message = ExceptionMsg;
                        //throw new Exception(ExceptionMsg, fe);
                    }
                    log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    //throw new Exception(ExceptionMsg, ex);
                    regTravelCardResponse.Status = ProcessingStatus.FAILED;
                    regTravelCardResponse.Message = ExceptionMsg;
                    _createLogfile(ex.Message);
                }
                else
                {
                    string ExceptionMsg = "The application terminated with an error." + ex.Message;
                    log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    regTravelCardResponse.Status = ProcessingStatus.FAILED;
                    regTravelCardResponse.Message = ExceptionMsg;
                    //throw new Exception(ExceptionMsg, ex);
                    _createLogfile(ex.Message);
                }
            }
            return regTravelCardResponse;
        }
        //public TravelCard GetTravelCardDetails(Guid customerId, string travelCardNumber)
        //{
        //    //Guid customerId = xrmHelper.GetIdByValue(accountNumber, "accountnumber", "account", xrmMgr);

        //    QueryByAttribute queryByAttribute = new QueryByAttribute("cgi_travelcard");            
        //    queryByAttribute.ColumnSet = new ColumnSet(true);
        //    queryByAttribute.AddAttributeValue("cgi_accountid", customerId);
        //    queryByAttribute.AddAttributeValue("cgi_travelcardnumber", travelCardNumber);
        //    ObservableCollection<CrmTravelCard> crmTravelCards = xrmMgr.Get<CrmTravelCard>(queryByAttribute);
        //    return GetTravelCardFromCrmTravelCard(crmTravelCards.FirstOrDefault());

        //}

        internal UpdateTravelCardResponse UpdateTravelCard(TravelCard travelCard)
        {
            UpdateTravelCardResponse updateTravelCardResponse = new UpdateTravelCardResponse();

            //bool bRegisterTravelCard = false;
            try
            {
                if (IsCardExisting(travelCard.CardNumber))
                {
                    //Entity entity = new Entity();
                    //entity.Id = travelCard.GetType().GUID;
                    //Guid customerId = travelCard.AccountId;
                    //CrmTravelCard crmTravelCard = GetCrmTravelCardFromTravelCard(travelCard.AccountId, travelCard);
                    //xrmMgr.Update<CrmTravelCard>(crmTravelCard);
                    //bRegisterTravelCard = true;
                    Entity crmTravelCard = GetCrmTravelCardEntityFromTravelCard(travelCard, true);
                    xrmMgr.Service.Update(crmTravelCard);
                    updateTravelCardResponse.Status = ProcessingStatus.SUCCESS;
                }
                else
                {
                    updateTravelCardResponse.Status = ProcessingStatus.FAILED;
                    updateTravelCardResponse.Message = "Card NOT Registered";
                }
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                //throw new Exception(ExceptionMsg, ex);
                updateTravelCardResponse.Status = ProcessingStatus.FAILED;
                updateTravelCardResponse.Message = ExceptionMsg;
            }
            catch (System.TimeoutException ex)
            {
                string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                updateTravelCardResponse.Status = ProcessingStatus.FAILED;
                updateTravelCardResponse.Message = ExceptionMsg;
                //throw new Exception(ExceptionMsg, ex);
            }

            catch (System.Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string ExceptionMsg = ex.InnerException.Message;

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe != null)
                    {
                        ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                        //chkCustomerExistResponse.Status = ProcessingStatus.FAILED;
                        //chkCustomerExistResponse.Message = ExceptionMsg;
                        //throw new Exception(ExceptionMsg, fe);
                    }
                    log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    //throw new Exception(ExceptionMsg, ex);
                    updateTravelCardResponse.Status = ProcessingStatus.FAILED;
                    updateTravelCardResponse.Message = ExceptionMsg;
                }
                else
                {
                    string ExceptionMsg = "The application terminated with an error." + ex.Message;
                    log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    updateTravelCardResponse.Status = ProcessingStatus.FAILED;
                    updateTravelCardResponse.Message = ExceptionMsg;
                    //throw new Exception(ExceptionMsg, ex);
                }
            }
            return updateTravelCardResponse;
        }

        internal GetCardsForCustomerResponse GetCardsForCustomer(Guid customerId, AccountCategoryCode customerType)
        {
            GetCardsForCustomerResponse getCardsForCustomerResponse = new GetCardsForCustomerResponse();
            List<TravelCard> lstTravelCard = null;
            //Guid customerId = xrmHelper.GetIdByValue(accountNumber, "accountnumber", "account", xrmMgr);

            try
            {
                QueryByAttribute queryByAttribute = new QueryByAttribute("cgi_travelcard");

                queryByAttribute.ColumnSet = new ColumnSet(true);
                if (customerType == AccountCategoryCode.Company)
                {
                    queryByAttribute.AddAttributeValue("cgi_accountid", customerId);
                }
                else
                {
                    queryByAttribute.AddAttributeValue("cgi_contactid", customerId);
                }
                queryByAttribute.AddAttributeValue("statecode", 0);
                ObservableCollection<CrmTravelCard> crmTravelCards = xrmMgr.Get<CrmTravelCard>(queryByAttribute);

                lstTravelCard = (from crmTravelCard in crmTravelCards
                                 select GetTravelCardFromCrmTravelCard(crmTravelCard)).ToList();
                getCardsForCustomerResponse.Status = ProcessingStatus.SUCCESS;
                getCardsForCustomerResponse.TravelCards = lstTravelCard;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                //throw new Exception(ExceptionMsg, ex);
                getCardsForCustomerResponse.Status = ProcessingStatus.FAILED;
                getCardsForCustomerResponse.Message = ExceptionMsg;
            }
            catch (System.TimeoutException ex)
            {
                string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                getCardsForCustomerResponse.Status = ProcessingStatus.FAILED;
                getCardsForCustomerResponse.Message = ExceptionMsg;
                //throw new Exception(ExceptionMsg, ex);
            }
            catch (System.Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string ExceptionMsg = ex.InnerException.Message;

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe != null)
                    {
                        ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                        //chkCustomerExistResponse.Status = ProcessingStatus.FAILED;
                        //chkCustomerExistResponse.Message = ExceptionMsg;
                        //throw new Exception(ExceptionMsg, fe);
                    }
                    log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    //throw new Exception(ExceptionMsg, ex);
                    getCardsForCustomerResponse.Status = ProcessingStatus.FAILED;
                    getCardsForCustomerResponse.Message = ExceptionMsg;
                }
                else
                {
                    string ExceptionMsg = "The application terminated with an error." + ex.Message;
                    log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    getCardsForCustomerResponse.Status = ProcessingStatus.FAILED;
                    getCardsForCustomerResponse.Message = ExceptionMsg;
                    //throw new Exception(ExceptionMsg, ex);
                }
            }
            return getCardsForCustomerResponse;
        }
        //public bool BlockTravelCard(Guid customerId, string travelCardNumber)
        //{
        //    bool bBlockTravelCard = false;

        //    try
        //    {
        //        Guid travelCardId;
        //        if (IsCardRegistered(customerId, travelCardNumber, out travelCardId))
        //        {
        //            Entity entTravelCard = new Entity("cgi_travelcard");
        //            entTravelCard.Id = travelCardId;
        //            entTravelCard.Attributes.Add("cgi_blocked", true);
        //            xrmMgr.Service.Update(entTravelCard);
        //            bBlockTravelCard = true;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return bBlockTravelCard;
        //}
        //public bool RemoveTravelCard(Guid customerId, string travelCardNumber)
        //{
        //    bool bRemoveTravelCard = false;
        //    try
        //    {
        //        Guid travelCardId;
        //        if (IsCardRegistered(customerId, travelCardNumber, out travelCardId))
        //        {
        //            xrmMgr.Delete("cgi_travelcard", travelCardId);
        //        }
        //        bRemoveTravelCard=true;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return bRemoveTravelCard;

        //}

        internal UnRegisterTravelCardResponse UnregisterTravelCard_RemoveRelationships(Guid customerId, AccountCategoryCode customerType, string travelCardNumber)
        {
            UnRegisterTravelCardResponse unregTravelCardResponse = new UnRegisterTravelCardResponse();
            try
            {
                Guid travelCardId;
                if (IsCardRegisteredToSuppliedCustomer(customerId, travelCardNumber, customerType, out travelCardId))
                {
                    Entity crmTravelCardEntity = new Entity("cgi_travelcard");
                    crmTravelCardEntity.Id = travelCardId;//GetTravelCardID(travelCardNumber);
                    crmTravelCardEntity.Attributes.Add("cgi_accountid", null);
                    crmTravelCardEntity.Attributes.Add("cgi_contactid", null);
                    xrmMgr.Service.Update(crmTravelCardEntity);
                    unregTravelCardResponse.Status = ProcessingStatus.SUCCESS;
                }
                else
                {
                    unregTravelCardResponse.Status = ProcessingStatus.FAILED;
                    unregTravelCardResponse.Message = "Card already Unregisterd";
                }
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                string ExceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                //throw new Exception(ExceptionMsg, ex);
                unregTravelCardResponse.Status = ProcessingStatus.FAILED;
                unregTravelCardResponse.Message = ExceptionMsg;
            }
            catch (System.TimeoutException ex)
            {
                string ExceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                unregTravelCardResponse.Status = ProcessingStatus.FAILED;
                unregTravelCardResponse.Message = ExceptionMsg;
                //throw new Exception(ExceptionMsg, ex);
            }

            catch (System.Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string ExceptionMsg = ex.InnerException.Message;

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;

                    if (fe != null)
                    {
                        ExceptionMsg = ExceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                        //chkCustomerExistResponse.Status = ProcessingStatus.FAILED;
                        //chkCustomerExistResponse.Message = ExceptionMsg;
                        //throw new Exception(ExceptionMsg, fe);
                    }
                    log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    //throw new Exception(ExceptionMsg, ex);
                    unregTravelCardResponse.Status = ProcessingStatus.FAILED;
                    unregTravelCardResponse.Message = ExceptionMsg;
                }
                else
                {
                    string ExceptionMsg = "The application terminated with an error." + ex.Message;
                    log2Crm.Exception(ExceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    unregTravelCardResponse.Status = ProcessingStatus.FAILED;
                    unregTravelCardResponse.Message = ExceptionMsg;
                    //throw new Exception(ExceptionMsg, ex);
                }
            }
            return unregTravelCardResponse;

        }
        #endregion
    }
}