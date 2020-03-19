using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using CGI.CRM2013.Skanetrafiken.CGIXrmLogger;
using CGICRMPortalService.Shared;
using CGICRMPortalService.Shared.Models;
using CGICRMPortalService.TravelCard.CrmClasses;
using CGICRMPortalService.TravelCard.Models;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CGICRMPortalService.TravelCard
{
    public class TravelCardManager : Response
    {
        #region Declarations
        private readonly XrmManager _xrmMgr;
        readonly LogToCrm _log2Crm;
        #endregion

        #region Constructors
        public TravelCardManager()
        {
            var xrmHelper = new XrmHelper();
            _xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
            _log2Crm = new LogToCrm();
            _createLogfile("Enter TravelCardManager with no params");
        }

        public TravelCardManager(Guid callerId)
        {
            var xrmHelper = new XrmHelper();
            _xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(callerId);
            _log2Crm = new LogToCrm();
            _createLogfile("Enter TravelCardManager with params");
        }
        #endregion

        #region Private Methods
        private Entity GetCrmTravelCardEntityFromTravelCard(Models.TravelCard travelCard, bool bUpdate)
        {
            Entity crmTravelCardEntity = new Entity("cgi_travelcard");
            if (bUpdate)
            {
                Guid travelCardId = GetTravelCardId(travelCard);
                crmTravelCardEntity.Id = travelCardId;

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

        private Entity PopulateCrmTravelCardEntityFromTravelCard(Models.TravelCard travelCard)
        {
            Entity crmTravelCardEntity = new Entity("cgi_travelcard")
            {
                Id = GetTravelCardId(travelCard)
            };


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

        private Guid GetTravelCardId(Models.TravelCard travelCard)
        {
            return GetTravelCardId(travelCard.CardNumber);
        }

        private Guid GetTravelCardId(string travelCardNumber)
        {
            QueryByAttribute queryByAttribute = new QueryByAttribute("cgi_travelcard");
            queryByAttribute.AddAttributeValue("cgi_travelcardnumber", travelCardNumber);
            queryByAttribute.AddAttributeValue("statecode", 0);
            queryByAttribute.ColumnSet = new ColumnSet("cgi_travelcardid");
            ObservableCollection<CrmTravelCard> travelCardCollection = _xrmMgr.Get<CrmTravelCard>(queryByAttribute);
            var travelCardId = Guid.NewGuid();
            if (travelCardCollection != null && travelCardCollection.Count > 0)
            {
                var firstOrDefault = travelCardCollection.FirstOrDefault();
                if (firstOrDefault != null)
                    travelCardId = firstOrDefault.TravelCardId;
            }
            return travelCardId;
        }


        private Models.TravelCard GetTravelCardFromCrmTravelCard(CrmTravelCard crmTravelCard)
        {
            Models.TravelCard travelcard = new Models.TravelCard
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

            QueryByAttribute queryByAttribute = new QueryByAttribute("cgi_travelcard")
            {
                TopCount = 1
            };
            queryByAttribute.AddAttributeValue("cgi_travelcardnumber", travelCardNumber);
            queryByAttribute.AddAttributeValue("cgi_accountid", null);
            queryByAttribute.AddAttributeValue("cgi_contactid", null);
            queryByAttribute.AddAttributeValue("statecode", 0); // 0 active
            queryByAttribute.ColumnSet = new ColumnSet("cgi_travelcardid");

            ObservableCollection<CrmTravelCard> travelCardCollection = _xrmMgr.Get<CrmTravelCard>(queryByAttribute);

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

            QueryByAttribute queryByAttribute = new QueryByAttribute("cgi_travelcard")
            {
                TopCount = 1
            };
            queryByAttribute.AddAttributeValue("cgi_travelcardnumber", travelCardNumber);
            queryByAttribute.AddAttributeValue("statecode", 0); // 0 active
            queryByAttribute.ColumnSet = new ColumnSet("cgi_travelcardid");

            ObservableCollection<CrmTravelCard> travelCardCollection = _xrmMgr.Get<CrmTravelCard>(queryByAttribute);

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
            }

            queryByAttribute.AddAttributeValue("statecode", 0); // 0 active
            queryByAttribute.AddAttributeValue("cgi_travelcardnumber", travelCardNumber);
            queryByAttribute.ColumnSet = new ColumnSet("cgi_travelcardid");
            ObservableCollection<CrmTravelCard> travelCardCollection = _xrmMgr.Get<CrmTravelCard>(queryByAttribute);
            
            if (travelCardCollection != null && travelCardCollection.Count > 0)
            {
                var firstOrDefault = travelCardCollection.FirstOrDefault();
                if (firstOrDefault != null)
                    travelCardId = firstOrDefault.TravelCardId;
                isAlreadyRegistered = true;
            }

            return isAlreadyRegistered;
        }

        private void _createLogfile(string input)
        {
            try
            {
                StreamWriter sw = new StreamWriter("C:\\Temp\\travelcard.log", true);
                string message = string.Format("{0} : {1}", DateTime.Now, input);
                sw.WriteLine(message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
        }
        #endregion

        #region Public Methods
        internal RegisterTravelCardResponse RegisterTravelCard_CreateOrUpdate(Models.TravelCard travelCard)
        {
            // NOTE: there could (possibly) be occations where users of Sekund use that system in a way that do not
            // follow instructions. If hard to find bugs accour, this is proberbly the reason. More development is 
            // then needed to make sure Sekund usage could only be performed the instructed way. ( plugins that block etc )
            _createLogfile("Start registertravelcard");
            RegisterTravelCardResponse regTravelCardResponse = new RegisterTravelCardResponse();

            try
            {
                Entity crmTravelCard;
                if (IsCardExisting(travelCard.CardNumber) == false)
                {
                    // Check if card does not exist

                    crmTravelCard = GetCrmTravelCardEntityFromTravelCard(travelCard, false);
                    Guid crmTravelCardId = _xrmMgr.Service.Create(crmTravelCard);
                    regTravelCardResponse.Status = ProcessingStatus.SUCCESS;
                    _createLogfile("Travelcard registered (created) with id " + crmTravelCardId);

                }
                else if (IsCardExistingAndNotRegistered(travelCard.CardNumber))
                {
                    // Check if card exist and that cgi_accountid and cgi_contactid is null
                    // NOTE: if card is inactive, update will throw a error message. ( but IsCardExistingAndNotRegistered checks active state )

                    crmTravelCard = PopulateCrmTravelCardEntityFromTravelCard(travelCard);
                    _xrmMgr.Service.Update(crmTravelCard);
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
            catch (FaultException<OrganizationServiceFault> ex)
            {
                string exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                //throw new Exception(ExceptionMsg, ex);
                regTravelCardResponse.Status = ProcessingStatus.FAILED;
                regTravelCardResponse.Message = exceptionMsg;
                _createLogfile(ex.Message);
            }
            catch (TimeoutException ex)
            {
                string exceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException ? "No Inner Fault" : ex.InnerException.Message);
                _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                regTravelCardResponse.Status = ProcessingStatus.FAILED;
                regTravelCardResponse.Message = exceptionMsg;
                _createLogfile(ex.Message);
            }
            catch (Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string exceptionMsg = ex.InnerException.Message;

                    FaultException<OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<OrganizationServiceFault>;

                    if (fe != null)
                    {
                        exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    }
                    _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    regTravelCardResponse.Status = ProcessingStatus.FAILED;
                    regTravelCardResponse.Message = exceptionMsg;
                    _createLogfile(ex.Message);
                }
                else
                {
                    string exceptionMsg = "The application terminated with an error." + ex.Message;
                    _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    regTravelCardResponse.Status = ProcessingStatus.FAILED;
                    regTravelCardResponse.Message = exceptionMsg;
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

        internal UpdateTravelCardResponse UpdateTravelCard(Models.TravelCard travelCard)
        {
            UpdateTravelCardResponse updateTravelCardResponse = new UpdateTravelCardResponse();

            try
            {
                if (IsCardExisting(travelCard.CardNumber))
                {
                    Entity crmTravelCard = GetCrmTravelCardEntityFromTravelCard(travelCard, true);
                    _xrmMgr.Service.Update(crmTravelCard);
                    updateTravelCardResponse.Status = ProcessingStatus.SUCCESS;
                }
                else
                {
                    updateTravelCardResponse.Status = ProcessingStatus.FAILED;
                    updateTravelCardResponse.Message = "Card NOT Registered";
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                string exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                updateTravelCardResponse.Status = ProcessingStatus.FAILED;
                updateTravelCardResponse.Message = exceptionMsg;
            }
            catch (TimeoutException ex)
            {
                string exceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException ? "No Inner Fault" : ex.InnerException.Message);
                _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                updateTravelCardResponse.Status = ProcessingStatus.FAILED;
                updateTravelCardResponse.Message = exceptionMsg;
            }

            catch (Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string exceptionMsg = ex.InnerException.Message;

                    FaultException<OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<OrganizationServiceFault>;

                    if (fe != null)
                    {
                        exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    }
                    _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    updateTravelCardResponse.Status = ProcessingStatus.FAILED;
                    updateTravelCardResponse.Message = exceptionMsg;
                }
                else
                {
                    string exceptionMsg = "The application terminated with an error." + ex.Message;
                    _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    updateTravelCardResponse.Status = ProcessingStatus.FAILED;
                    updateTravelCardResponse.Message = exceptionMsg;
                }
            }
            return updateTravelCardResponse;
        }

        internal GetCardsForCustomerResponse GetCardsForCustomer(Guid customerId, AccountCategoryCode customerType)
        {
            GetCardsForCustomerResponse getCardsForCustomerResponse = new GetCardsForCustomerResponse();

            try
            {
                QueryByAttribute queryByAttribute = new QueryByAttribute("cgi_travelcard")
                {
                    ColumnSet = new ColumnSet(true)
                };

                queryByAttribute.AddAttributeValue(
                    customerType == AccountCategoryCode.Company ? "cgi_accountid" : "cgi_contactid", customerId);
                queryByAttribute.AddAttributeValue("statecode", 0);
                ObservableCollection<CrmTravelCard> crmTravelCards = _xrmMgr.Get<CrmTravelCard>(queryByAttribute);

                var lstTravelCard = (from crmTravelCard in crmTravelCards
                    select GetTravelCardFromCrmTravelCard(crmTravelCard)).ToList();
                getCardsForCustomerResponse.Status = ProcessingStatus.SUCCESS;
                getCardsForCustomerResponse.TravelCards = lstTravelCard;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                string exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                getCardsForCustomerResponse.Status = ProcessingStatus.FAILED;
                getCardsForCustomerResponse.Message = exceptionMsg;
            }
            catch (TimeoutException ex)
            {
                string exceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException ? "No Inner Fault" : ex.InnerException.Message);
                _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                getCardsForCustomerResponse.Status = ProcessingStatus.FAILED;
                getCardsForCustomerResponse.Message = exceptionMsg;
            }
            catch (Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string exceptionMsg = ex.InnerException.Message;

                    FaultException<OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<OrganizationServiceFault>;

                    if (fe != null)
                    {
                        exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    }
                    _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    getCardsForCustomerResponse.Status = ProcessingStatus.FAILED;
                    getCardsForCustomerResponse.Message = exceptionMsg;
                }
                else
                {
                    string exceptionMsg = "The application terminated with an error." + ex.Message;
                    _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    getCardsForCustomerResponse.Status = ProcessingStatus.FAILED;
                    getCardsForCustomerResponse.Message = exceptionMsg;
                }
            }
            return getCardsForCustomerResponse;
        }

        internal UnRegisterTravelCardResponse UnregisterTravelCard_RemoveRelationships(Guid customerId, AccountCategoryCode customerType, string travelCardNumber)
        {
            UnRegisterTravelCardResponse unregTravelCardResponse = new UnRegisterTravelCardResponse();
            try
            {
                Guid travelCardId;
                if (IsCardRegisteredToSuppliedCustomer(customerId, travelCardNumber, customerType, out travelCardId))
                {
                    Entity crmTravelCardEntity = new Entity("cgi_travelcard")
                    {
                        Id = travelCardId
                    };
                    crmTravelCardEntity.Attributes.Add("cgi_accountid", null);
                    crmTravelCardEntity.Attributes.Add("cgi_contactid", null);
                    _xrmMgr.Service.Update(crmTravelCardEntity);
                    unregTravelCardResponse.Status = ProcessingStatus.SUCCESS;
                }
                else
                {
                    unregTravelCardResponse.Status = ProcessingStatus.FAILED;
                    unregTravelCardResponse.Message = "Card already Unregisterd";
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                string exceptionMsg = "The application terminated with an error. Timestamp: " + ex.Detail.Timestamp +
                                     "Code: " + ex.Detail.ErrorCode +
                                     "Message: " + ex.Detail.Message +
                                     "Trace: " + ex.Detail.TraceText +
                                     "Inner Fault: " + (null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                unregTravelCardResponse.Status = ProcessingStatus.FAILED;
                unregTravelCardResponse.Message = exceptionMsg;
            }
            catch (TimeoutException ex)
            {
                string exceptionMsg = "The application terminated with an error. Message:" + ex.Message + " Stack Trace:" + ex.StackTrace + " Inner Fault: {0}" + (null == ex.InnerException ? "No Inner Fault" : ex.InnerException.Message);
                _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                unregTravelCardResponse.Status = ProcessingStatus.FAILED;
                unregTravelCardResponse.Message = exceptionMsg;
            }

            catch (Exception ex)
            {
                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    string exceptionMsg = ex.InnerException.Message;

                    FaultException<OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<OrganizationServiceFault>;

                    if (fe != null)
                    {
                        exceptionMsg = exceptionMsg + "The application terminated with an error. Timestamp: " + fe.Detail.Timestamp +
                                      "Code: " + fe.Detail.ErrorCode +
                                      "Message: " + fe.Detail.Message +
                                      "Trace: " + fe.Detail.TraceText +
                                      "Inner Fault: " + (null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    }
                    _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    unregTravelCardResponse.Status = ProcessingStatus.FAILED;
                    unregTravelCardResponse.Message = exceptionMsg;
                }
                else
                {
                    string exceptionMsg = "The application terminated with an error." + ex.Message;
                    _log2Crm.Exception(exceptionMsg, "IsDuplicateEmail", ex, "Portal Web Service");
                    unregTravelCardResponse.Status = ProcessingStatus.FAILED;
                    unregTravelCardResponse.Message = exceptionMsg;
                }
            }
            return unregTravelCardResponse;

        }
        #endregion
    }
}