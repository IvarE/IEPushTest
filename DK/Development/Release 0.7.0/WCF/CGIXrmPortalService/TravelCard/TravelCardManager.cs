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
        private XrmManager xrmMgr;
        private XrmHelper xrmHelper;
        LogToCrm log2Crm = new LogToCrm();

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

        #region [Private Methods]
        //private CrmTravelCard GetCrmTravelCardFromTravelCard(Guid customerId, TravelCard travelCard)
        //{
        //    //Guid accountId = xrmHelper.GetIdByValue(accountnumber, "accountnumber", "account", xrmMgr);

        //    CrmTravelCard crmTravelCard = new CrmTravelCard
        //    {
        //        CardNumber = travelCard.CardNumber,
        //        Account = new EntityReference("account", customerId),
        //        TravelCardId = GetTravelCardID(travelCard),
        //        //CardType = new EntityReference("cgi_travelcardtype", xrmHelper.GetIdByValue(travelCard.CardType, "cgi_travelcardtypename", "cgi_travelcardtype", xrmMgr)),
        //        PeriodCardTypeTitle = travelCard.PeriodCardTypeTitle,
        //        ValueCardTypeTitle = travelCard.ValueCardTypeTitle,
        //        //PeriodValidFrom = travelCard.PeriodValidFrom!=null?travelCard.PeriodValidFrom:null,
        //        //PeriodValidTo = travelCard.PeriodValidTo,
        //        Blocked = travelCard.Blocked
        //    };
        //    return crmTravelCard;
        //}
        private Entity GetCrmTravelCardEntityFromTravelCard(TravelCard travelCard, bool bUpdate)
        {
            //Guid accountId = xrmHelper.GetIdByValue(accountnumber, "accountnumber", "account", xrmMgr);
            Entity crmTravelCardEntity = new Entity("cgi_travelcard");
            if (bUpdate)
            {
                Guid travelCardId= GetTravelCardID(travelCard);
                crmTravelCardEntity.Id=travelCardId;
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
            

            //CrmTravelCard crmTravelCard = new CrmTravelCard
            //{
            //    CardNumber = travelCard.CardNumber,
            //    Account = new EntityReference("account", customerId),
            //    TravelCardId = GetTravelCardID(travelCard),
            //    //CardType = new EntityReference("cgi_travelcardtype", xrmHelper.GetIdByValue(travelCard.CardType, "cgi_travelcardtypename", "cgi_travelcardtype", xrmMgr)),
            //    PeriodCardTypeTitle = travelCard.PeriodCardTypeTitle,
            //    ValueCardTypeTitle = travelCard.ValueCardTypeTitle,
            //    //PeriodValidFrom = travelCard.PeriodValidFrom!=null?travelCard.PeriodValidFrom:null,
            //    //PeriodValidTo = travelCard.PeriodValidTo,
            //    Blocked = travelCard.Blocked
            //};
            return crmTravelCardEntity;
        }
        private Guid GetTravelCardID(TravelCard travelCard)
        {
            Guid travelCardId = Guid.Empty;
            string travelCardNumber = travelCard.CardNumber;
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
                AccountId = crmTravelCard.Account != null ? crmTravelCard.Account.Id : (crmTravelCard.Contact!=null?crmTravelCard.Contact.Id:Guid.Empty),
                CardName=crmTravelCard.CardName,
                CardNumber = crmTravelCard.CardNumber,
                //CardType = crmTravelCard.CardTypeName,
                CustomerType=crmTravelCard.Account != null ? AccountCategoryCode.Company : AccountCategoryCode.Private,
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
                LatestAutoloadAmount = crmTravelCard.LatestAutoloadAmount!=null?crmTravelCard.LatestAutoloadAmount.Value:0,
                LatestChargeDate = crmTravelCard.LatestChargeDate,
                LatestFailedAttempt = crmTravelCard.LatestFailedAttempt,
                PeriodCardTypeId = crmTravelCard.PeriodCardTypeId,
                ValueCardTypeId = crmTravelCard.ValueCardTypeId,
                VerifyId = crmTravelCard.VerifyId
            };
            return travelcard;
        }

        private bool IsCardRegistered(string travelCardNumber)
        {
            bool isAlreadRegistered = false;

            QueryByAttribute queryByAttribute = new QueryByAttribute("cgi_travelcard");
            queryByAttribute.TopCount = 1;
            queryByAttribute.AddAttributeValue("cgi_travelcardnumber", travelCardNumber);
            queryByAttribute.AddAttributeValue("statecode", 0);
            queryByAttribute.ColumnSet = new ColumnSet(new string[] { "cgi_travelcardid" });
            
            ObservableCollection<CrmTravelCard> travelCardCollection = xrmMgr.Get<CrmTravelCard>(queryByAttribute);

            if (travelCardCollection != null && travelCardCollection.Count > 0)
            {
                isAlreadRegistered = true;
            }

            return isAlreadRegistered;
        }

        private bool IsCardRegistered(Guid customerId, string travelCardNumber,AccountCategoryCode customerType, out Guid travelCardId)
        {
            bool isAlreadyRegistered = false;

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
            
            queryByAttribute.AddAttributeValue("cgi_travelcardnumber", travelCardNumber);
            queryByAttribute.ColumnSet = new ColumnSet(new string[] { "cgi_travelcardid" });
            ObservableCollection<CrmTravelCard> travelCardCollection = xrmMgr.Get<CrmTravelCard>(queryByAttribute);
            travelCardId = Guid.NewGuid();
            if (travelCardCollection != null && travelCardCollection.Count > 0)
            {
                travelCardId = travelCardCollection.FirstOrDefault().TravelCardId;
                isAlreadyRegistered = true;
            }

            return isAlreadyRegistered;
        }
        #endregion

        #region [Travel Card : Public Methods]
        internal RegisterTravelCardResponse RegisterTravelCard(TravelCard travelCard)
        {
            RegisterTravelCardResponse regTravelCardResponse = new RegisterTravelCardResponse();

            _createLogfile("start registertravelcard");

            //bool bRegisterTravelCard = false;
            try
            {

                if (!IsCardRegistered(travelCard.CardNumber))
                {
                    //Guid customerId = travelCard.AccountId;
                    //CrmTravelCard crmTravelCard = GetCrmTravelCardFromTravelCard(travelCard.AccountId, travelCard);
                    //xrmMgr.Create<CrmTravelCard>(crmTravelCard);
                    //bRegisterTravelCard = true;
                    Entity crmTravelCard = GetCrmTravelCardEntityFromTravelCard(travelCard, false);
                    Guid crmTravelCardId=xrmMgr.Service.Create(crmTravelCard);
                    regTravelCardResponse.Status = ProcessingStatus.SUCCESS;
                    _createLogfile("travelcard registerd.");
                }
                else
                {
                    regTravelCardResponse.Status = ProcessingStatus.FAILED;
                    regTravelCardResponse.Message = "Card is already Registered";
                    _createLogfile("travelcard registerd failed. card already exists.");
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
                if (IsCardRegistered(travelCard.CardNumber))
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

        internal UnRegisterTravelCardResponse UnregisterTravelCard(Guid customerId,AccountCategoryCode customerType,string travelCardNumber)
        {
            //bool bRemoveTravelCard = false;

            UnRegisterTravelCardResponse unregTravelCardResponse = new UnRegisterTravelCardResponse();
            try
            {
                Guid travelCardId;
                //GetTravelCardID(travelCardNumber);
                if (IsCardRegistered(customerId, travelCardNumber,customerType, out travelCardId))
                {
                    // xrmMgr.Delete("cgi_travelcard", travelCardId);

                    SetStateRequest setStateReq = new SetStateRequest();
                    setStateReq.EntityMoniker = new EntityReference("cgi_travelcard", travelCardId);
                    setStateReq.State = new OptionSetValue(1);//1-Inactive 0-Active
                    setStateReq.Status = new OptionSetValue(2);//2-Inactive 1-Active

                    unregTravelCardResponse.Status = ProcessingStatus.SUCCESS;

                    xrmMgr.Service.Execute(setStateReq);
                }
                else
                {
                    unregTravelCardResponse.Status = ProcessingStatus.FAILED;
                    unregTravelCardResponse.Message = "Card already Unregisterd";
                }
                // bRemoveTravelCard = true;

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

    }
}