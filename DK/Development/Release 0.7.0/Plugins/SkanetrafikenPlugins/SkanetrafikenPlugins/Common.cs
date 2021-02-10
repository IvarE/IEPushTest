using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

using System.ServiceModel;
using CRM2013.SkanetrafikenPlugins.CreateGiftcardService;
using Microsoft.Crm.Sdk.Messages;

namespace CRM2013.SkanetrafikenPlugins
{

    public static class EntityExtensions
    {
        ///<summary> 
        ///Extension method to get an attribute value from the entity or image 
        ///</summary> 
        ///<typeparam name="T">The attribute type</typeparam> 
        ///<param name="entity">The primary entity</param> 
        ///<param name="attributeLogicalName">Logical name of the attribute</param> 
        ///<param name="image">Image (pre/post) of the primary entity</param> 
        ///<param name="defaultValue">The default value to use</param> 
        ///<returns>The attribute value of type T</returns> 
        public static T GetAttributeValue<T>(this Entity entity, string attributeLogicalName, Entity image, T defaultValue)
        {
            return entity.Contains(attributeLogicalName)
                ? entity.GetAttributeValue<T>(attributeLogicalName)
                : image != null && image.Contains(attributeLogicalName)
                    ? image.GetAttributeValue<T>(attributeLogicalName)
                    : defaultValue;
        }

        public static T GetValue<T>(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Attributes.ContainsKey(attributeLogicalName))
                return default(T);

            if (entity[attributeLogicalName] is AliasedValue)
            {
                if (((AliasedValue)entity[attributeLogicalName]).Value == null)
                {
                    return default(T);
                }

                T _value = default(T);
                try
                {
                    _value = (T)((AliasedValue)entity[attributeLogicalName]).Value;
                }
                catch { }

                return _value;
            }
            else
            {
                return entity.GetAttributeValue<T>(attributeLogicalName);
            }
        }
    }

    public static class AttributeExtensions
    {
        /// <summary>
        /// If attribute was updated, its availible at least as a key on update message.
        /// If value is null, then update was a delete of value,
        /// then try to get value from preimage for use in your logic.
        /// For use on update in a post event.
        /// Preimage must be registred.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="preImage"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetUpdatedOrPreImageAttributeValue(this AttributeCollection collection, Entity preImage, string key, out object value)
        {

            if (collection.TryGetValue(key, out value))
            {
                if (value == null)
                {
                    return (preImage != null && preImage.Attributes != null)
                       ? preImage.Attributes.TryGetValue(key, out value)
                       : false;
                }
                return true;
            }
            return false;
        }

        /*
     public static bool TryGetAttributeValue<T>(this AttributeCollection collection, Entity image, string key, out T value)
     {

         return TryGetAttributeValue(collection, image, key, out value, default(T));
     }

     public static bool TryGetAttributeValue<T>(this AttributeCollection collection, Entity image, string key, out T value, T defaultValue)
     {
         value = collection.Contains(key)
            ? (T)collection[key] : image != null && image.Contains(key)
                ? image.GetAttributeValue<T>(key) : defaultValue;

         return Compare<T>(value, defaultValue);
     }

     static bool Compare<T>(T x, T y) where T : class
     {
         return x == y;
     }*/
    }

    public static class ParemeterCollectionExtensions
    {
        public static bool TryGetTargetEntity(this ParameterCollection collection, out Entity value)
        {
            value = null;
            if (collection.Contains("Target") && collection["Target"] is Entity)
            {
                value = (Entity)collection["Target"];
                return true;
            }
            return false;
        }

        /* For future reference: PlugindataBase is less accessible so it doesnt work if access level is not changed
        public static bool SetTargetEntity(this ParameterCollection collection, PlugindataBase data)
        {}
        */
    }

    internal class Common
    {
        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }

        public Common(IOrganizationService service, ITracingService tracingService)
        {
            Service = service;
            TracingService = tracingService;
        }

        public bool _isDebug = true;

        public void SetMissingEntityRef(Entity destEntity, Entity sourceEntity, string attribute, string destAttribute)
        {
            // Verify if attribute exists
            if (destEntity.Attributes.Contains(destAttribute) || !sourceEntity.Attributes.Contains(attribute))
                return;

            destEntity.Attributes.Add(new KeyValuePair<string, object>(destAttribute, sourceEntity.Attributes[attribute]));
        }

        public void SetMissingEntityRef(Entity destEntity, Entity sourceEntity, string attribute)
        {
            SetMissingEntityRef(destEntity, sourceEntity, attribute, attribute);
        }
    }

    internal abstract class PlugindataBase
    {
        public PlugindataBase(IServiceProvider serviceProvider)
        {
            Context = (Microsoft.Xrm.Sdk.IPluginExecutionContext)serviceProvider.GetService(typeof(Microsoft.Xrm.Sdk.IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            Service = serviceFactory.CreateOrganizationService(Context.UserId);
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Common = new Common(Service, TracingService);
        }

        public Common Common { get; set; }

        public IPluginExecutionContext Context { get; set; }
        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }

        public Entity Target { get; set; }
        public Entity PreImage { get; set; }
        public Entity PostImage { get; set; }
        public EntityReference TargetReference { get; set; }

        public void InitPreImage(string preImageName)
        {
            TracingService.Trace("Try to init pre image {0}", preImageName);
            if (Context.PreEntityImages.ContainsKey(preImageName) && Context.PreEntityImages[preImageName] is Entity)
            {
                PreImage = Context.PreEntityImages[preImageName];
            }
            TracingService.Trace("Pre image {0} : {1}", preImageName, (PreImage != null).ToString());
        }

        public void InitPostImage(string postImageName)
        {
            TracingService.Trace("Try to init post image {0}", postImageName);
            if (Context.PostEntityImages.ContainsKey(postImageName) && Context.PostEntityImages[postImageName] is Entity)
            {
                PostImage = Context.PostEntityImages[postImageName];
            }
            TracingService.Trace("Post image {0} : {1}", postImageName, (PostImage != null).ToString());
        }
    }

    public static class TravelCardHandler
    {
        // TODO flytta innehåll till extern metod likt de andra?
        public static void ExecuteTravelCardSyncronization(IOrganizationService service, string customerId)
        {
            try
            {
                string SyncCustomerCardServiceUrl = TravelCardHandler.GetSetting(service, "cgi_synccustomercardservice");

                SyncCustomerCards.SyncCustomerCardsRequest request = new SyncCustomerCards.SyncCustomerCardsRequest
                {
                    SyncFromCrmtoEPiRequestParameters = new SyncCustomerCards.SyncFromCrmtoEPiRequestParameters
                    {
                        CustomerId = customerId // Cyrus ACC contact id "e0f9c312-d2e4-e411-80d7-005056906ae2"
                    }
                };

                BasicHttpBinding myBinding = new BasicHttpBinding();
                myBinding.Name = "myBasicHttpBinding";
                EndpointAddress endPointAddress = new EndpointAddress(SyncCustomerCardServiceUrl);
                
                SyncCustomerCards.SyncCustomerCardsClient client = new SyncCustomerCards.SyncCustomerCardsClient(myBinding, endPointAddress);
                
                //SyncCustomerCard.SyncCustomerCardResponse respons = client.SyncCustomerCard(request);
                // TODO varför inte en "vanlig" request?
                
                SyncCustomerCards.SyncFromCrmtoEPiResponseParameters responsParameters = client.SyncCustomerCards(request.SyncFromCrmtoEPiRequestParameters);
                
                uint statusCode = responsParameters.StatusCode;
                
                if (statusCode != 200)
                {
                    string errorMessage = "Ett fel uppstod vid synkronisering av kort. ";
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        errorMessage += " CustomerId: ";
                        errorMessage += customerId;
                    }
                    if (!string.IsNullOrEmpty(responsParameters.Message))
                    {
                        errorMessage += " Felinformation: ";
                        errorMessage += responsParameters.Message;
                    }
                    if (!string.IsNullOrEmpty(responsParameters.ErrorMessage))
                    {
                        errorMessage += " Detaljerat felmeddelande: " + responsParameters.ErrorMessage;
                    }
                    errorMessage += " Statuskod: " + statusCode.ToString();
                    throw new InvalidPluginExecutionException(errorMessage);
                }

            }
            catch (InvalidPluginExecutionException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                // previous UserMessageException but I dont know why/how its used, maybe for SL?
                throw new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i ExecuteTravelCardSyncronization, customerId = {0}. Kontakta din administratör. Detaljerat felmeddelande: {1}", customerId,ex.Message));
            }
        }

        // TODO move to common?
        private static string GetSetting(IOrganizationService service, string serviceAttributeNameInLowerCase)
        {
            #region FetchXML

            string _now = DateTime.Now.ToString("s");
            string _xml = "";
            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_settingid' />";
            _xml += "       <attribute name='" + serviceAttributeNameInLowerCase + "' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _now + "' />";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _now + "' />";
            _xml += "               <condition attribute='cgi_validto' operator='null' />";
            _xml += "           </filter>";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            #endregion

            FetchExpression _f = new FetchExpression(_xml);
            EntityCollection settingscollection = service.RetrieveMultiple(_f);

            Entity settings = settingscollection.Entities.First();

            if (settings.Contains(serviceAttributeNameInLowerCase) && settings[serviceAttributeNameInLowerCase] != null)
            {
                return settings.GetAttributeValue<string>(serviceAttributeNameInLowerCase);
            }
            else
            {
                throw new Exception("Required setting is missing: " + serviceAttributeNameInLowerCase);
            }
        }
    }

    /// <summary>
    /// Used by travelinformation_Post and travelinformation_Delete
    /// </summary>
    public static class TravelInformationHandler
    {
        public static bool IsRelatedToCase(this Entity travelInformation)
        {
            return travelInformation.Attributes.ContainsKey("cgi_caseid");
        }

        public static DataCollection<Entity> GetRelatedTravelInfos(EntityReference crmCaseId, IOrganizationService service)
        {
            QueryExpression relatedTravelInfosQuery = CreateQueryForAllTravelInformationsRelatedToCase(crmCaseId.Id);

            EntityCollection relatedTravelInfos = service.RetrieveMultiple(relatedTravelInfosQuery);

            return relatedTravelInfos.Entities;
        }

        public static void UpdateCase(Entity crmCase, IOrganizationService service)
        {
            service.Update(crmCase);
        }

        public static Entity CreateCaseWithUpdatedTravelInformation(Entity travelInformation)
        {            
            Entity crmCase = new Entity("incident");
          
            crmCase.Id = ((EntityReference)travelInformation.Attributes["cgi_caseid"]).Id;
           
            crmCase["cgi_travelinformationlookup"] = new EntityReference
            {
                LogicalName = "cgi_travelinformation",
                Id = (Guid)travelInformation.Attributes["cgi_travelinformationid"]
            };
           
            crmCase["cgi_travelinformation"] = travelInformation.GetValue<string>("cgi_travelinformation");
            crmCase["cgi_travelinformationtransport"] = travelInformation.GetValue<string>("cgi_transport");
            crmCase["cgi_travelinformationarrivalactual"] = travelInformation.GetValue<string>("cgi_arivalactual");
            crmCase["cgi_travelinformationcity"] = travelInformation.GetValue<string>("cgi_city");
            crmCase["cgi_travelinformationcompany"] = travelInformation.GetValue<string>("cgi_contractor");
            crmCase["cgi_travelinformationdeviationmessage"] = travelInformation.GetValue<string>("cgi_deviationmessage");
            crmCase["cgi_travelinformationdirectiontext"] = travelInformation.GetValue<string>("cgi_directiontext");
            crmCase["cgi_travelinformationline"] = travelInformation.GetValue<string>("cgi_line");
            crmCase["cgi_travelinformationstart"] = travelInformation.GetValue<string>("cgi_start");
            crmCase["cgi_travelinformationstartactual"] = travelInformation.GetValue<string>("cgi_startactual");
            crmCase["cgi_travelinformationstop"] = travelInformation.GetValue<string>("cgi_stop");
            crmCase["cgi_travelinformationtour"] = travelInformation.GetValue<string>("cgi_tour");
            crmCase["cgi_travelinformationdisplaytext"] = travelInformation.GetValue<string>("cgi_displaytext");

            //Using GetValue when these dates are missing sets these dates to 00010101. We want them to be empty which is why we can't use GetValue here
            crmCase["cgi_travelinformationarrivalplanned"] = travelInformation.Attributes.ContainsKey("cgi_arivalplanned") ? (DateTime?)travelInformation.Attributes["cgi_arivalplanned"] : null;
            crmCase["cgi_travelinformationstartplanned"] = travelInformation.Attributes.ContainsKey("cgi_startplanned") ? (DateTime?)travelInformation.Attributes["cgi_startplanned"] : null;

            return crmCase;
        }

        public static Entity CreateCaseWithEmptyTravelInformation(Entity travelInformation)
        {
            Entity crmCase = new Entity("incident");

            crmCase.Id = ((EntityReference)travelInformation.Attributes["cgi_caseid"]).Id;

            crmCase["cgi_travelinformationlookup"] = null;
            crmCase["cgi_travelinformation"] = null;
            crmCase["cgi_travelinformationtransport"] = null;
            crmCase["cgi_travelinformationarrivalactual"] = null;
            crmCase["cgi_travelinformationcity"] = null;
            crmCase["cgi_travelinformationcompany"] = null;
            crmCase["cgi_travelinformationdeviationmessage"] = null;
            crmCase["cgi_travelinformationdirectiontext"] = null;
            crmCase["cgi_travelinformationline"] = null;
            crmCase["cgi_travelinformationstart"] = null;
            crmCase["cgi_travelinformationstartactual"] = null;
            crmCase["cgi_travelinformationstop"] = null;
            crmCase["cgi_travelinformationtour"] = null;
            crmCase["cgi_travelinformationdisplaytext"] = null;
            crmCase["cgi_travelinformationarrivalplanned"] = null;
            crmCase["cgi_travelinformationstartplanned"] = null;

            return crmCase;
        }

        private static QueryExpression CreateQueryForAllTravelInformationsRelatedToCase(Guid crmCaseId)
        {
            ConditionExpression condition = new ConditionExpression
            {
                AttributeName = "cgi_caseid",
                Operator = ConditionOperator.Equal,
            };
            condition.Values.Add(crmCaseId.ToString());

            QueryExpression relatedTravelInfosQuery = new QueryExpression
            {
                ColumnSet = new ColumnSet("cgi_travelinformation",
                    "cgi_caseid",
                    "cgi_travelinformationid",
                    "cgi_transport",
                    "cgi_arivalactual",
                    "cgi_city",
                    "cgi_contractor",
                    "cgi_deviationmessage",
                    "cgi_directiontext",
                    "cgi_line",
                    "cgi_start",
                    "cgi_startactual",
                    "cgi_stop",
                    "cgi_tour",
                    "cgi_displaytext",
                    "cgi_arivalplanned",
                    "cgi_startplanned"),
                EntityName = "cgi_travelinformation"
            };
            relatedTravelInfosQuery.Criteria.AddCondition(condition);

            return relatedTravelInfosQuery;
        }
    }

    /// <summary>
    /// Used by refund_Post
    /// </summary>
    public static class RefundHandler
    {
        public static bool IsRelatedToCase(this Entity refund)
        {
            return refund.Attributes.ContainsKey("cgi_caseid");
        }

        public static DataCollection<Entity> GetRelatedRefundInfos(EntityReference crmCaseId, IOrganizationService service)
        {
            QueryExpression relatedRefundInfosQuery = CreateQueryForAllRefundInformationsRelatedToCase(crmCaseId.Id);

            EntityCollection relatedRefundInfos = service.RetrieveMultiple(relatedRefundInfosQuery);

            return relatedRefundInfos.Entities;
        }

        public static void UpdateCase(Entity crmCase, IOrganizationService service)
        {
            service.Update(crmCase);
        }

        public static Entity CreateCaseWithUpdatedRefundInformation(Entity refund)
        {
            Entity crmCase = new Entity("incident");
           
            crmCase.Id = ((EntityReference)refund.Attributes["cgi_caseid"]).Id;

            //RefundTypes, RefundTransportCompanyId and RefundReimbursementForm are lookups and requires more than GetValue
            crmCase["cgi_refundtypes"] = new EntityReference
            {
                LogicalName = "cgi_refundtype",
                Id = ((EntityReference)refund.Attributes["cgi_refundtypeid"]).Id
            };
            
            if(refund.Attributes.Contains("cgi_reimbursementformid")){
            crmCase["cgi_refundreimbursementform"] = new EntityReference
                {
                    LogicalName = "cgi_reimbursementform",
                    Id = ((EntityReference)refund.Attributes["cgi_reimbursementformid"]).Id
                };
            }

            if (refund.Attributes.Contains("cgi_transportcompanyid"))
            {
                crmCase["cgi_refundtransportcompanyid"] = new EntityReference
                {
                    LogicalName = "cgi_refundtransportcompanyid",
                    Id = ((EntityReference)refund.Attributes["cgi_refundtransportcompanyid"]).Id
                };
            }
           
            //Test if new refund contains attributes, then copy
            if(refund.Attributes.Contains("cgi_milage")){
                 crmCase["cgi_refundmilagekm"] = refund.GetValue<decimal>("cgi_milage");
            }
            if(refund.Attributes.Contains("cgi_milage_compensation")){
            crmCase["cgi_refundmilagecompensation"] = refund.GetValue<decimal>("cgi_milage_compensation");
            }
            if(refund.Attributes.Contains("cgi_quantity")){
            crmCase["cgi_refundquantity"] = refund.GetValue<int>("cgi_quantity");
            }
            if(refund.Attributes.Contains("cgi_calculated_amount")){
            crmCase["cgi_refundcalculatedamount"] = refund.GetValue<decimal>("cgi_calculated_amount");
            }
            if(refund.Attributes.Contains("cgi_amount")){
            crmCase["cgi_refundamount"] = refund.GetValue<Money>("cgi_amount");
            }
            if(refund.Attributes.Contains("cgi_value_code")){
            crmCase["cgi_refundvaluecode"] = refund.GetValue<string>("cgi_value_code");
            }
            if(refund.Attributes.Contains("cgi_travelcard_number")){
            crmCase["cgi_refundtravelcardno"] = refund.GetValue<string>("cgi_travelcard_number");
            }
            if (refund.Attributes.Contains("cgi_checknumber"))
            {
                crmCase["cgi_refundcheckno"] = refund.GetValue<string>("cgi_checknumber");
            }
            if (refund.Attributes.Contains("cgi_comments"))
            {
                crmCase["cgi_refundcomments"] = refund.GetValue<string>("cgi_comments");
            }
            if(refund.Attributes.Contains("cgi_last_valid")){
            crmCase["cgi_refundlastvalid"] = refund.GetValue<DateTime>("cgi_last_valid");
            }
            if(refund.Attributes.Contains("cgi_accountno")){
            crmCase["cgi_refundaccountno"] = refund.GetValue<string>("cgi_accountno");
            }
 
            return crmCase;
        }

        public static Entity CreateCaseWithEmptyRefundInformation(Entity travelInformation)
        {
            Entity crmCase = new Entity("incident");

            crmCase.Id = ((EntityReference)travelInformation.Attributes["cgi_caseid"]).Id;

            crmCase["cgi_RefundTypes"] = null;
            crmCase["cgi_RefundReimbursementForm"] = null;
            crmCase["cgi_RefundMilagekm"] = null;
            crmCase["cgi_RefundMilageCompensation"] = null;
            crmCase["cgi_RefundQuantity"] = null;
            crmCase["cgi_RefundCalculatedAmount"] = null;
            crmCase["cgi_RefundAmount"] = null;
            crmCase["cgi_RefundValueCode"] = null;
            crmCase["cgi_RefundTravelCardNo"] = null;
            crmCase["cgi_RefundLastValid"] = null;
            crmCase["cgi_RefundAccountNo"] = null;

            return crmCase;
        }

        private static QueryExpression CreateQueryForAllRefundInformationsRelatedToCase(Guid crmCaseId)
        {
            ConditionExpression condition = new ConditionExpression
            {
                AttributeName = "cgi_caseid",
                Operator = ConditionOperator.Equal,
            };
            condition.Values.Add(crmCaseId.ToString());

            QueryExpression relatedRefundInfosQuery = new QueryExpression
            {
                ColumnSet = new ColumnSet("cgi_refund",
                    "cgi_refundtypeid",
                    "cgi_reimbursementformid",
                    "cgi_milage",
                    "cgi_milage_compensation",
                    "cgi_quantity",
                    "cgi_calculated_amount",
                    "cgi_amount",
                    "cgi_value_code",
                    "cgi_travelcard_number",
                    "cgi_last_valid",
                    "cgi_accountno"),


                EntityName = "cgi_refund"
            };
            relatedRefundInfosQuery.Criteria.AddCondition(condition);

            return relatedRefundInfosQuery;
        }
    }

    public class ReimbursementHandler
    {
        public void ExecuteRefundAndUpdatesStatus(Guid RefundId, IOrganizationService service)
        {
            Entity Refund = service.Retrieve("cgi_refund", RefundId, new ColumnSet(new string[] { 
                "cgi_refundtypeid", 
                "cgi_reimbursementformid", 
                "cgi_travelcard_number", 
                "cgi_accountid", 
                "cgi_contactid", 
                "cgi_refundnumber", 
                "cgi_caseid",
                "cgi_amount", 
                "cgi_mobilenumber",
                "statecode", 
                "statuscode",
                "cgi_last_valid"
            }));

            int state = ((OptionSetValue)Refund.Attributes["statecode"]).Value;
            int status = ((OptionSetValue)Refund.Attributes["statuscode"]).Value;

            //CreateAnnotation("ExecuteRefundAndUpdatesStatus message", "this is a test message", new EntityReference(Refund.LogicalName, Refund.Id));
            //Genomför eventuell utbetalning och sätt status på beslutet [Refund].
            //State value: 0 = active, corresponding Status values: 1 = New, 285050003 = Approved - Transaction Pending, 285050004 = Approved -Transaction failed, 285050005 = Approved
            //State value: 1 = inactive, corresponding Status values: 2 = Declined, 285050000 = Approved

            if ((state == 0 && status == 285050005) || (state == 0 && status == 285050004))
            {
                //285050003 = Approved - Transaction Pending
                //285050004 = Approved -Transaction failed
                //try to execute the Refund
                ReimbursementHandler rh = new ReimbursementHandler();
                if (rh.ExecuteRefundTransaction(Refund, true, service))
                {
                    //close the incident if refund is executed

                    _SetRefundDescisionStatus(RefundId, 0, 285050005, service);//Active - Approved
                    if (Refund.Attributes.Contains("cgi_caseid") && Refund.Attributes["cgi_caseid"] != null)
                    {
                        _closeIncident(((EntityReference)Refund.Attributes["cgi_caseid"]).Id, service);
                    }
                }
                else
                {
                    _SetRefundDescisionStatus(RefundId, 0, 285050004, service);//Active - Approved -Transaction failed
                    //leave incident open
                }
            }
        }

        private void _SetRefundDescisionStatus(Guid RefundId, int state, int status, IOrganizationService service)
        {
            //State value: 0 = active, corresponding Status values: 1 = New
            //State value: 1 = inactive, corresponding Status values: 2 = Declined, 285050000 = Approved, 285050001 =Approved -Transaction failed


            SetStateRequest statusrequest = new SetStateRequest();
            statusrequest.EntityMoniker = new EntityReference("cgi_refund", RefundId);
            statusrequest.State = new OptionSetValue(state);
            statusrequest.Status = new OptionSetValue(status);
            SetStateResponse response = (SetStateResponse)service.Execute(statusrequest);
        }

        private void _closeIncident(Guid CaseId, IOrganizationService service)
        {
            Entity _caseresolution = new Entity("incidentresolution");
            _caseresolution.Attributes.Add("incidentid", new EntityReference("incident", CaseId));
            _caseresolution.Attributes.Add("subject", "Problemet löst.");

            CloseIncidentRequest _closerequest = new CloseIncidentRequest()
            {
                IncidentResolution = _caseresolution,
                //RequestName = "CloseIncident",
                Status = new OptionSetValue(5)
            };
            CloseIncidentResponse _closeresponse = (CloseIncidentResponse)service.Execute(_closerequest);

        }

        public bool ExecuteRefundTransaction(Entity Refund, bool IsAutomatic, IOrganizationService service)
        {
            try
            {
                string EHOrderNumber = "";
                string ValueCode = "";

                //attempt to execute the refund

                if (!Refund.Contains("cgi_reimbursementformid"))
                {
                    throw new UserMessageException("Kunde inte behandla utbetalning eftersom utbetalningsform saknas.");
                }

                EntityReference rf_id = Refund.GetAttributeValue<EntityReference>("cgi_reimbursementformid");
                Entity reimbursementForm = service.Retrieve(rf_id.LogicalName, rf_id.Id, new ColumnSet("cgi_loadcard", "cgi_giftcard", "cgi_couponsms", "cgi_couponemail"));

                //Avbryt om ingen automatisk utbetalning ska exekveras
                if (!(reimbursementForm.RefundedViaLoadCard()
                    || reimbursementForm.RefundedViaGiftCard()
                    || reimbursementForm.RefundedViaSms()
                    || reimbursementForm.RefundedViaEmail()))
                {
                    if (IsAutomatic)
                        throw new UserMessageException("Utbetalningsform kunde inte avgöras. Tillåtna värden är cgi_loadcard, cgi_giftcard, cgi_couponsms");
                    else
                        return true;
                }


                EntityReference incidentReference = Refund.GetAttributeValue<EntityReference>("cgi_caseid");
                Entity incident = service.Retrieve(incidentReference.LogicalName, incidentReference.Id, new ColumnSet("customerid", "cgi_customer_email"));

                EntityReference CustomerId = null;
                if (incident.Contains("customerid"))
                    CustomerId = incident.GetAttributeValue<EntityReference>("customerid");
                else
                    throw new UserMessageException("Ärendet saknar kund!");

                string EmailAddress = null;

                if (Refund.Contains("cgi_email"))
                {
                    EmailAddress = Refund.GetAttributeValue<string>("cgi_email");
                }
                else if (incident.Contains("cgi_customer_email"))
                {            
                    EmailAddress = incident.GetAttributeValue<string>("cgi_customer_email");
                }
                else if (!(reimbursementForm.RefundedViaSms()
                    || reimbursementForm.RefundedViaLoadCard())) //If the code is sent via SMS, we don't need an email address.
                    throw new UserMessageException("Ärendet saknar epostadress!");

                string _CardSerialNumber = Refund.Contains("cgi_travelcard_number") ? Refund.GetAttributeValue<string>("cgi_travelcard_number") : null;
                decimal _Amount = Refund.Contains("cgi_amount") ? Refund.GetAttributeValue<Money>("cgi_amount").Value : 0;

                string setting_cgi_chargeorderservice;
                string setting_cgi_createcouponservice;
                string setting_cgi_createemailcouponservice;
                string setting_cgi_sendvaluecodemailservice;
                string setting_cgi_giftcardservice;
                EntityReference setting_cgi_defaultcustomeroncase;
                int cgi_valuecodevalidformonths;
                GetSettings(service, out setting_cgi_chargeorderservice, out setting_cgi_createcouponservice, out setting_cgi_createemailcouponservice, out setting_cgi_sendvaluecodemailservice, out setting_cgi_giftcardservice, out setting_cgi_defaultcustomeroncase, out cgi_valuecodevalidformonths);

                EntityReference ERRefund = new EntityReference(Refund.LogicalName, Refund.Id);

                if (reimbursementForm.RefundedViaLoadCard())
                {
                    if (_Amount == 0 || _CardSerialNumber == null)
                        return true;

                    LoadCard(_Amount, _CardSerialNumber, CustomerId.Id, EmailAddress, ERRefund, setting_cgi_chargeorderservice, ref EHOrderNumber, service, IsAutomatic);
                }
                else if (reimbursementForm.RefundedViaGiftCard())
                {
                    if (setting_cgi_defaultcustomeroncase.Id.Equals(CustomerId.Id))
                        throw new UserMessageException("Kunde inte skapa värdekod för anonym kund!");

                    DateTime cgi_last_valid = DateTime.Now.AddMonths(6);
                    if (Refund.Contains("cgi_last_valid") && Refund["cgi_last_valid"] != null)
                        cgi_last_valid = Refund.GetAttributeValue<DateTime>("cgi_last_valid");

                    CreateGiftcard(CustomerId, EmailAddress, cgi_last_valid, _Amount, ERRefund, setting_cgi_giftcardservice, setting_cgi_sendvaluecodemailservice, ref ValueCode, service, IsAutomatic);
                }
                else if (reimbursementForm.RefundedViaSms())
                {
                    string cgi_mobilenumber = null;
                    if (Refund.Contains("cgi_mobilenumber"))
                        cgi_mobilenumber = Refund.GetAttributeValue<string>("cgi_mobilenumber");
                    else
                        throw new UserMessageException("Ärendet saknar telefonnummer!");

                    string smstext = String.IsNullOrEmpty(EmailAddress) ? "," : EmailAddress; //Cannot send an empty string as argument to the service, see RedMine #2437

                    CreateCouponSMS(_Amount, CustomerId.Id, smstext, cgi_mobilenumber, "0", ERRefund, setting_cgi_createcouponservice, ref EHOrderNumber, service, IsAutomatic);
                }
                else if (reimbursementForm.RefundedViaEmail())
                {
                    CreateEmailCoupon(_Amount, _CardSerialNumber, CustomerId.Id, EmailAddress, EmailAddress, "0", ERRefund, setting_cgi_createemailcouponservice, ref EHOrderNumber, service, IsAutomatic);
                }
                else
                {
                    if (IsAutomatic)
                        throw new UserMessageException("Utbetalningsform kunde inte avgöras baserat på vald utbetalningsform. Tillåtna värden är cgi_loadcard, cgi_giftcard, cgi_couponsms, cgi_couponemail");
                }

                Refund.Attributes["cgi_ehordernumber"] = EHOrderNumber;
                Refund.Attributes["cgi_value_code"] = ValueCode;

                if (IsAutomatic)
                    service.Update(Refund); //not needed when called from pre-executing plugin

                return true;
            }
            catch (Exception ex)
            {
                if (IsAutomatic)
                {
                    try
                    {
                        if (ex is UserMessageException)
                        {
                            _CreateAnnotation("Error in ExecuteRefundTransaction", "User messge:" + ex.Message + " | Original error:" + (ex.InnerException != null ? ex.InnerException.ToString() : ""), new EntityReference(Refund.LogicalName, Refund.Id), service);
                            Refund.Attributes["cgi_errormessage"] = ex.Message;
                        }
                        else
                        {
                            _CreateAnnotation("Error in ExecuteRefundTransaction", ex.ToString(), new EntityReference(Refund.LogicalName, Refund.Id), service);
                            Refund.Attributes["cgi_errormessage"] = "Ett oförutsett fel inträffade. Se återbetalningens anteckningar för teknisk information.";
                        }
                        service.Update(Refund);
                    }
                    catch { }
                    return false;
                }
                else
                {
                    if (ex is UserMessageException)
                    {
                        throw new InvalidPluginExecutionException(ex.Message, ex);
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException("Ett oförutsett fel inträffade. Försök igen eller välj en annan återbetalningsform.", ex);
                    }
                }
            }
        }

        private void _CreateAnnotation(string subject, string notetext, EntityReference reference, IOrganizationService service)
        {
            try
            {
                Entity annotation = new Entity("annotation");
                annotation["objectid"] = reference;
                //                annotation["objecttypecode"] = reference.LogicalName;
                annotation["subject"] = subject;
                annotation["notetext"] = notetext;
                service.Create(annotation);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(string.Format("subject={0}, notetext={1}, reference.Id={2}, reference.LogicalName = {3} Message: {4}", subject, notetext, reference.Id, reference.LogicalName, ex.Message));
            }
        }

        #region Create Coupon

        private void CreateCouponSMS(decimal amount, Guid CustomerId, string FullName, string MobilePhone, string CampaignCode, EntityReference RefundER, string CreateCouponServiceUrl, ref string EHOrderNumber, IOrganizationService service, bool IsAutomatic)
        {
            try
            {
                CreateSMSCouponService.CreateSMSCouponRequest request = new CreateSMSCouponService.CreateSMSCouponRequest();
                request.Amount = amount;
                request.AmountSpecified = true;
                request.CampaignCode = CampaignCode;
                request.Currency = "SEK";
                request.CustomerId = CustomerId.ToString();
                //request.DistributionType = 1; Field not included in request?
                request.Email = FullName; //Redmine #2437:
                request.MobilePhone = MobilePhone;

                BasicHttpBinding myBinding = new BasicHttpBinding();
                myBinding.Name = "myBasicHttpBinding";
                EndpointAddress endPointAddress = new EndpointAddress(CreateCouponServiceUrl);

                CreateSMSCouponService.CreateSMSCouponClient client = new CreateSMSCouponService.CreateSMSCouponClient(myBinding, endPointAddress);
                CreateSMSCouponService.CreateSMSCouponResponse respons = client.CreateSMSCoupon(request);
                string Message = respons.Message;
                string ErrorMessage = respons.ErrorMessage;
                uint StatusCode = respons.StatusCode;
                bool StatusCodeSpecified = respons.StatusCodeSpecified;
                bool OrderCreated = respons.OrderCreated;
                bool OrderCreatedSpecified = respons.OrderCreatedSpecified;

                EHOrderNumber = respons.OrderNumber;

                if (IsAutomatic)
                    _CreateAnnotation("CreateCouponResponse:", string.Format("ErrorMessage={0}, Message={1}, OrderCreated={2}, OrderCreatedSpecified={3}, StatusCode={4}, StatusCodeSpecified={5}, EHOrderNumber={6}", ErrorMessage, Message, OrderCreated, OrderCreatedSpecified, StatusCode, StatusCodeSpecified, EHOrderNumber), RefundER, service);
                if (!OrderCreated)
                    throw new InvalidPluginExecutionException("Ett fel uppstod vid skapande av SMS-kupong." + ErrorMessage);
            }
            catch (Exception ex)
            {
                string UserMessage = "Ett fel uppstod vid skapande av SMS-kupong.";
                if (ex is UserMessageException)
                {
                    UserMessage = ex.Message;
                }
                throw new UserMessageException(UserMessage, new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i CreateCouponSMS, amount = {0}, CustomerId = {1}, FullName={2}, MobilePhone={3}, CampaignCode={4}, Exception: {5}", amount.ToString(), CustomerId.ToString(), FullName, MobilePhone, CampaignCode, ex.ToString())));
            }
        }
        private void CreateEmailCoupon(decimal amount, string _CardSerialNumber, Guid CustomerId, string FullName, string EmailAddress, string CampaignCode, EntityReference RefundER, string CreateCouponEmailServiceUrl, ref string EHOrderNumber, IOrganizationService service, bool IsAutomatic)
        {
            try
            {
                CreateEmailCouponService.CreateEmailCouponRequest request = new CreateEmailCouponService.CreateEmailCouponRequest();
                request.Amount = amount;
                request.AmountSpecified = true;
                request.CampaignCode = CampaignCode;
                request.Currency = "SEK";
                request.CustomerId = CustomerId.ToString();
                request.Email = EmailAddress;
               
                BasicHttpBinding myBinding = new BasicHttpBinding();
                myBinding.Name = "myBasicHttpBinding";
                EndpointAddress endPointAddress = new EndpointAddress(CreateCouponEmailServiceUrl);

                CreateEmailCouponService.CreateEmailCouponClient client = new CreateEmailCouponService.CreateEmailCouponClient(myBinding, endPointAddress);
                CreateEmailCouponService.CreateEmailCouponResponse respons = client.CreateEmailCoupon(request);
                string Message = respons.Message;
                string ErrorMessage = respons.ErrorMessage;
                uint StatusCode = respons.StatusCode;
                bool StatusCodeSpecified = respons.StatusCodeSpecified;
                bool OrderCreated = respons.OrderCreated;
                bool OrderCreatedSpecified = respons.OrderCreatedSpecified;

                EHOrderNumber = respons.OrderNumber;

                if (IsAutomatic)
                    _CreateAnnotation("CreateEmailCouponResponse:", string.Format("ErrorMessage={0}, Message={1}, OrderCreated={2}, OrderCreatedSpecified={3}, StatusCode={4}, StatusCodeSpecified={5}, EHOrderNumber={6}", ErrorMessage, Message, OrderCreated, OrderCreatedSpecified, StatusCode, StatusCodeSpecified, EHOrderNumber), RefundER, service);
                if (!OrderCreated)
                    throw new InvalidPluginExecutionException("Ett fel uppstod vid skapande av Email-kupong." + ErrorMessage);
            }
            catch (Exception ex)
            {
                string UserMessage = "Ett fel uppstod vid skapande av Email-kupong.";
                if (ex is UserMessageException)
                {
                    UserMessage = ex.Message;
                }
                throw new UserMessageException(UserMessage, new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i CreateEmailCoupon, amount = {0}, _CardSerialNumber={1}, CustomerId = {2}, FullName={3}, EmailAddress={4} Exception: {5}", amount.ToString(), _CardSerialNumber, CustomerId.ToString(), FullName, EmailAddress, ex.ToString())));
            }
        }
        #endregion

        #region Create giftcard

        private void CreateGiftcard(EntityReference CustomerId, string EmailAddress, DateTime cgi_last_valid, decimal cgi_amount, EntityReference RefundER, string CreateGiftcardServiceUrl, string SendValueCodeMailServiceUrl, ref string ValueCode, IOrganizationService service, bool IsAutomatic)
        {
            try
            {
                using (CreateGiftcardService.BizTalkServiceInstance createvaluecodeclient = new CreateGiftcardService.BizTalkServiceInstance())
                {
                    createvaluecodeclient.Url = CreateGiftcardServiceUrl;

                    CreateGiftCardRequest req = new CreateGiftCardRequest();

                    req.CustomerId = null;

                    req.CampaignTrackingCode = "";
                    req.Currency = "SEK";

                    req.Sum = cgi_amount;
                    req.SumSpecified = true;

                    req.ValidTo = cgi_last_valid;
                    req.ValidToSpecified = true;
                    CreateGiftCardResponse res = createvaluecodeclient.GreateGiftCard(req);


                    string CampaignTrackingCode = res.CampaignTrackingCode;
                    ValueCode = res.Code;
                    string Currency = res.Currency;
                    string _CustomerId = res.CustomerId;
                    decimal Sum = res.Sum;
                    bool SumSpecified = res.SumSpecified;
                    DateTime ValidTo = res.ValidTo;
                    bool ValidToSpecified = res.ValidToSpecified;

                    if (IsAutomatic)
                        _CreateAnnotation("CreateGiftCardResponse:", string.Format("CampaignTrackingCode={0}, Code={1}, Currency={2}, CustomerId={3}, Sum={4}, SumSpecified={5}, ValidTo={6}, ValidToSpecified={7}"
                        , CampaignTrackingCode, ValueCode, Currency, _CustomerId, Sum, SumSpecified, ValidTo, ValidToSpecified), RefundER, service);

                    if (string.IsNullOrEmpty(res.Code))
                        throw new UserMessageException("Ett fel uppstod vid skapandet eller utskicket av värdekod via mail: CreateGiftCard tjänsten returerade ingen kod, försök igen!");

                    //skicka värdekod med epost
                    SendValueCodeMailService.SendValueCodeMailRequest request = new SendValueCodeMailService.SendValueCodeMailRequest();

                    request.Email = EmailAddress;
                    request.ValueCode = res.Code;
                    request.CustomerId = CustomerId.Id.ToString();
                    if (IsAutomatic)
                        _CreateAnnotation("SendValueCodeMailRequest:", string.Format("Email={0}, ValueCode={1}, Currency={2}", request.Email, request.ValueCode, request.CustomerId), RefundER, service);

                    BasicHttpBinding myBinding = new BasicHttpBinding();
                    myBinding.Name = "myBasicHttpBinding";
                    EndpointAddress endPointAddress = new EndpointAddress(SendValueCodeMailServiceUrl);
                    SendValueCodeMailService.SendValueCodeMailClient sendvaluecodeclient = new SendValueCodeMailService.SendValueCodeMailClient(myBinding, endPointAddress);

                    SendValueCodeMailService.SendValueCodeMailResponse response = sendvaluecodeclient.SendValueCodeMail(request);
                    if (response == null)
                    {
                        if (IsAutomatic)
                            _CreateAnnotation("Service returned null", "SendValueCodeMailResponse is null", RefundER, service);
                        else
                            throw new InvalidPluginExecutionException("SendValueCodeMail returned null. SendValueCodeMailResponse is null.");
                    }


                    string Message = response.Message;
                    string ErrorMessage = response.ErrorMessage;
                    int StatusCode = response.StatusCode;
                    bool StatusCodeSpecified = response.StatusCodeSpecified;
                    bool Success = response.Success;
                    bool SuccessSpecified = response.SuccessSpecified;


                    if (IsAutomatic)
                        _CreateAnnotation("SendValueCodeMailResponse:", string.Format("ErrorMessage={0}, Message={1}, Success={2}, SuccessSpecified={3}, StatusCode={4}, StatusCodeSpecified={5}", ErrorMessage, Message, Success, SuccessSpecified, StatusCode, StatusCodeSpecified), RefundER, service);
                    if (!Success)
                        throw new UserMessageException("Ett fel uppstod vid utskick av värdekod via mail: " + ErrorMessage);

                }
            }
            catch (Exception ex)
            {
                string UserMessage = "Ett fel uppstod vid skapandet eller utskicket av värdekod via mail.";
                if (ex is UserMessageException)
                {
                    UserMessage = ex.Message;
                }

                throw new UserMessageException(UserMessage, new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i CreateGiftcard/SendValueCodeMail, CustomerId = {0}, EmailAddress={1}, cgi_last_valid = {2}, cgi_amount={3}. Exception: {4}", CustomerId.Id.ToString(), EmailAddress, cgi_last_valid.ToString(), cgi_amount.ToString(), ex.ToString())));
            }
        }

        #endregion

        #region Load card

        private void LoadCard(decimal amount, string _CardSerialNumber, Guid CustomerId, string EmailAddress, EntityReference RefundER, string serviceUrl, ref string EHOrderNumber, IOrganizationService service, bool IsAutomatic)
        {
            try
            {
                ChargeOrderService.CreateChargeOrderRequest ccorequest = new ChargeOrderService.CreateChargeOrderRequest();
                ccorequest.Amount = amount;
                ccorequest.AmountSpecified = true;
                ccorequest.CardNumber = _CardSerialNumber;
                ccorequest.Currency = "SEK";
                ccorequest.CustomerId = CustomerId.ToString();
                ccorequest.Email = EmailAddress;

                BasicHttpBinding myBinding = new BasicHttpBinding();
                myBinding.Name = "myBasicHttpBinding";
                EndpointAddress endPointAddress = new EndpointAddress(serviceUrl);
                ChargeOrderService.ChargeOrderClient client = new ChargeOrderService.ChargeOrderClient(myBinding, endPointAddress);
                ChargeOrderService.CreateChargeOrderResponse ccoresponse = client.ChargeOrder(ccorequest);

                string ErrorMessage = ccoresponse.ErrorMessage;
                string Message = ccoresponse.Message;
                bool OrderCreated = ccoresponse.OrderCreated;
                bool OrderCreatedSpecified = ccoresponse.OrderCreatedSpecified;
                EHOrderNumber = ccoresponse.OrderNumber;
                int StatusCode = ccoresponse.StatusCode;
                bool StatusCodeSpecified = ccoresponse.StatusCodeSpecified;
                if (IsAutomatic)
                    _CreateAnnotation("CreateChargeOrderResponse:", string.Format("ErrorMessage={0}, Message={1}, OrderCreated={2}, OrderCreatedSpecified={3}, EHOrderNumber={4}, StatusCode={5}, StatusCodeSpecified={6}", ErrorMessage, Message, OrderCreated, OrderCreatedSpecified, EHOrderNumber, StatusCode, StatusCodeSpecified), RefundER, service);
                if (!OrderCreated)
                    throw new UserMessageException("Ett fel uppstod vid laddning av kort: " + ErrorMessage);

            }
            catch (Exception ex)
            {
                string UserMessage = "Ett fel uppstod vid laddning av kort.";
                if (ex is UserMessageException)
                {
                    UserMessage = ex.Message;
                }
                throw new UserMessageException(UserMessage, new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i LoadCard, amount = {0}, _CardSerialNumber={1}, CustomerId = {2}, EmailAddress={3}. Exception: {4}", amount, _CardSerialNumber, CustomerId, EmailAddress, ex.ToString())));
            }
        }

        #endregion

        #region Common

        private void GetSettings(IOrganizationService service, out string cgi_chargeorderservice, out  string cgi_createcouponservice, out  string cgi_createemailcouponservice, out string cgi_sendvaluecodemailservice, out string cgi_giftcardservice, out EntityReference cgi_defaultcustomeroncase, out int cgi_valuecodevalidformonths)
        {
            #region FetchXML

            string _now = DateTime.Now.ToString("s");
            string _xml = "";
            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_settingid' />";
            _xml += "       <attribute name='cgi_cubicservice' />";
            _xml += "       <attribute name='cgi_giftcardservice' />";
            _xml += "       <attribute name='cgi_chargeorderservice' />";
            _xml += "       <attribute name='cgi_createcouponservice' />";
            _xml += "       <attribute name='cgi_createemailcouponservice' />";
            _xml += "       <attribute name='cgi_getoutstandingchargesservice' />";
            _xml += "       <attribute name='cgi_rechargecardservice' />";
            _xml += "       <attribute name='cgi_sendvaluecodemailservice' />";
            _xml += "       <attribute name='cgi_defaultcustomeroncase' />";
            _xml += "       <attribute name='cgi_raindanceprefix' />";
            _xml += "       <attribute name='cgi_valuecodevalidformonths' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _now + "' />";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _now + "' />";
            _xml += "               <condition attribute='cgi_validto' operator='null' />";
            _xml += "           </filter>";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            #endregion

            FetchExpression _f = new FetchExpression(_xml);
            EntityCollection settingscollection = service.RetrieveMultiple(_f);

            Entity settings = settingscollection.Entities.First();

            if (settings.Contains("cgi_chargeorderservice") && settings["cgi_chargeorderservice"] != null)
                cgi_chargeorderservice = settings.GetAttributeValue<string>("cgi_chargeorderservice");
            else
                throw new Exception("Required setting is missing: cgi_chargeorderservice");

            if (settings.Contains("cgi_createcouponservice") && settings["cgi_createcouponservice"] != null)
                cgi_createcouponservice = settings.GetAttributeValue<string>("cgi_createcouponservice");
            else
                throw new Exception("Required setting is missing: cgi_createcouponservice");

            if (settings.Contains("cgi_createemailcouponservice") && settings["cgi_createemailcouponservice"] != null)
                cgi_createemailcouponservice = settings.GetAttributeValue<string>("cgi_createemailcouponservice");
            else
                throw new Exception("Required setting is missing: cgi_createemailcouponservice");

            if (settings.Contains("cgi_sendvaluecodemailservice") && settings["cgi_sendvaluecodemailservice"] != null)
                cgi_sendvaluecodemailservice = settings.GetAttributeValue<string>("cgi_sendvaluecodemailservice");
            else
                throw new Exception("Required setting is missing: cgi_sendvaluecodemailservice");

            if (settings.Contains("cgi_giftcardservice") && settings["cgi_giftcardservice"] != null)
                cgi_giftcardservice = settings.GetAttributeValue<string>("cgi_giftcardservice");
            else
                throw new Exception("Required setting is missing: cgi_giftcardservice");

            if (settings.Contains("cgi_defaultcustomeroncase") && settings["cgi_defaultcustomeroncase"] != null)
                cgi_defaultcustomeroncase = settings.GetAttributeValue<EntityReference>("cgi_defaultcustomeroncase");
            else
                throw new Exception("Required setting is missing: cgi_defaultcustomeroncase");

            if (settings.Contains("cgi_valuecodevalidformonths") && settings["cgi_valuecodevalidformonths"] != null)
                cgi_valuecodevalidformonths = settings.GetAttributeValue<int>("cgi_valuecodevalidformonths");
            else
                throw new Exception("Required setting is missing: cgi_valuecodevalidformonths");


        }

        

        #endregion
    }

    public static class ExtensionMethods
    {
        public static bool RefundedViaSms(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_couponsms") && reimbursementForm.GetAttributeValue<bool>("cgi_couponsms") == true;
        }

        public static bool RefundedViaEmail(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_couponemail") && reimbursementForm.GetAttributeValue<bool>("cgi_couponemail") == true;
        }

        public static bool RefundedViaLoadCard(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_loadcard") && reimbursementForm.GetAttributeValue<bool>("cgi_loadcard") == true;
        }

        public static bool RefundedViaGiftCard(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_giftcard") && reimbursementForm.GetAttributeValue<bool>("cgi_giftcard") == true;
        }
    }

    public class UserMessageException : Exception
    {
        public UserMessageException(string UserMessage, Exception ex)
            : base(UserMessage, ex)
        {
        }
        public UserMessageException(string UserMessage)
            : base(UserMessage)
        {
        }

    }
}
