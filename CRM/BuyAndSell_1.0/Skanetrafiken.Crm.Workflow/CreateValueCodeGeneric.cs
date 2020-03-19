using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Skanetrafiken.Crm.ValueCodes;
using System;
using System.Activities;
using System.Net;

namespace Skanetrafiken.Crm.Entities
{
    public class CreateValueCodeGeneric : CodeActivity
    {
        [Input("VoucherType")]
        [RequiredArgument()]
        public InArgument<int> VoucherType { get; set; }

        [Input("DeliveryType")]
        public InArgument<int> DeliveryType { get; set; }

        [Input("Amount")]
        [RequiredArgument()]
        public InArgument<decimal> Amount { get; set; }

        [Input("Mobile")]
        public InArgument<string> Mobile { get; set; }

        [Input("Email")]
        public InArgument<string> Email { get; set; }

        [Input("PeriodPrice")]
        public InArgument<decimal> PeriodPrice { get; set; }

        [Input("TravelCardId")]
        [ReferenceTarget("cgi_travelcard")]
        public InArgument<EntityReference> TravelCardId { get; set; }

        [Input("RefundId")]
        [ReferenceTarget("cgi_refund")]
        public InArgument<EntityReference> RefundId { get; set; }

        [Input("LeadId")]
        [ReferenceTarget("lead")]
        public InArgument<EntityReference> LeadId { get; set; }

        [Input("ContactId")]
        [ReferenceTarget("contact")]
        public InArgument<EntityReference> ContactId { get; set; }

        [Input("ValueCodeApprovalId")]
        [ReferenceTarget("ed_valuecodeapproval")]
        public InArgument<EntityReference> ValueCodeApprovalId { get; set; }

        [Output("ValueCodeId")]
        [ReferenceTarget("ed_valuecode")]
        public OutArgument<EntityReference> ValueCodeId { get; set; }

        [Output("ResultCreateValueCodeGeneric")]
        public OutArgument<string> ResultCreateValueCodeGeneric { get; set; }

        private Plugin.LocalPluginContext GetLocalContext(CodeActivityContext activityContext)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 |
                                       SecurityProtocolType.Tls11;

            IWorkflowContext workflowContext = activityContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService organizationService = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);
            ITracingService tracingService = activityContext.GetExtension<ITracingService>();


            return new Plugin.LocalPluginContext(null, organizationService, null, tracingService);
        }

        protected override void Execute(CodeActivityContext activityContext)
        {
            //GENERATE CONTEXT
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);

            //TRY EXECUTE
            try
            {

                localContext.TracingService.Trace($"---> Entering {nameof(CreateValueCodeGeneric)}.");

                //GET VALUE(S)
                localContext.Trace($"CreateValueCodeGeneric. Get Contact");
                EntityReference contact = ContactId.Get(activityContext);

                localContext.Trace($"CreateValueCodeGeneric. Get Lead");
                EntityReference lead = LeadId.Get(activityContext);

                localContext.Trace($"CreateValueCodeGeneric. Get Refund");
                EntityReference refund = RefundId.Get(activityContext);

                localContext.Trace($"CreateValueCodeGeneric. Get Approval");
                EntityReference valueCodeApprovalId = ValueCodeApprovalId.Get(activityContext);

                localContext.Trace($"CreateValueCodeGeneric. Get TravelCardNumber");
                EntityReference travelCardId = TravelCardId.Get(activityContext);

                localContext.Trace($"CreateValueCodeGeneric. Get VoucherType");
                int voucherType = VoucherType.Get(activityContext);

                localContext.Trace($"CreateValueCodeGeneric. Get DeliveryType");
                int deliveryType = DeliveryType.Get(activityContext);

                localContext.Trace($"CreateValueCodeGeneric. Get Amount");
                decimal amount = Amount.Get(activityContext);

                localContext.Trace($"CreateValueCodeGeneric. Get Mobile");
                string mobile = Mobile.Get(activityContext);

                localContext.Trace($"CreateValueCodeGeneric. Get Email");
                string email = Email.Get(activityContext);

                localContext.Trace($"CreateValueCodeGeneric. Get PeriodPrice");
                decimal periodPrice = PeriodPrice.Get(activityContext);

                localContext.Trace($"VoucherType: {voucherType}, Amount: {amount}, Mobile: {mobile}, Email: {Email}");

                localContext.Trace($"CreateValueCodeGeneric. ExecuteCodeActivity...");
                EntityReference valueCodeResponse = ExecuteCodeActivity(localContext, valueCodeApprovalId, contact, lead, voucherType,
                    amount, periodPrice, mobile, email, refund, deliveryType, travelCardId);
                localContext.Trace($"CreateValueCodeGeneric finished. ValueCodeId: {valueCodeResponse.Id}");


                localContext.Trace($"ValueCode is typeof: {valueCodeResponse.GetType()}");
                ValueCodeId.Set(activityContext, valueCodeResponse);
                ResultCreateValueCodeGeneric.Set(activityContext, "OK");

                localContext.TracingService.Trace($"<--- Exiting {nameof(CreateValueCodeGeneric)}.");
            }
            catch (Exception ex)
            {
                EntityReference refund = RefundId.Get(activityContext);

                HandleErrorValueCode(localContext, refund, ex.Message);

                ValueCodeId.Set(activityContext, null);
                ResultCreateValueCodeGeneric.Set(activityContext, $"Kunde inte skapa värdkod. Vänligen kontakta kundtjänst. StripError (CreateValueCodeGeneric): { ex.Message}");
                //throw new InvalidPluginExecutionException($"Kunde inte skapa värdkod. Vänligen kontakta kundtjänst. StripError (CreateValueCodeGeneric): { ex.Message}");
            }
        }


        public void HandleErrorValueCode(Plugin.LocalPluginContext localContext, EntityReference refundRef, string errorMessage)
        {
            localContext.TracingService.Trace($"---> Entering {nameof(HandleErrorValueCode)}.");

            RefundEntity refund = RefundEntity.GetRefundById(localContext, refundRef.Id, new ColumnSet(RefundEntity.Fields.cgi_Caseid));
            IncidentEntity incident = null;

            if (refund == null)
                refund.SetTransactionFailed(localContext, $"Kunde inte hitta beslut med id '{refundRef.Id}'.");

            else if (refund != null)
            {
                incident = IncidentEntity.GetIncidentByRefundCaseId(localContext, refund.cgi_Caseid.Id);
                if (incident == null)
                    refund.SetTransactionFailed(localContext, $"Kunde inte hitta ärende med id '{refund.cgi_Caseid.Id}'.");
            }

            if (refund == null || incident == null)
            {
                incident.ReOpenCase(localContext, Schema.Generated.IncidentState.Active);
                refund.SetTransactionFailed(localContext, errorMessage);
            }

            localContext.TracingService.Trace($"<--- Exiting {nameof(HandleErrorValueCode)}.");
        }



        public static EntityReference ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference valueCodeApprovalId,
            EntityReference contact, EntityReference lead, int voucherType, decimal amount, decimal periodPrice, string mobile, string email, EntityReference refund, int deliveryType, EntityReference travelCardNumber)
        {

            localContext.Trace($"---> Entering {nameof(ExecuteCodeActivity)}.");

            Guid valueCodeId = Guid.Empty;
            bool valueCodeCreated = false;

            try
            {
                if (contact == null && lead == null && email == null && mobile == null)
                {
                    throw new InvalidPluginExecutionException("Kontaktuppgifter saknas");
                }

                if (contact == null)
                {
                    throw new InvalidPluginExecutionException("Kontakt saknas från anrop...");
                }

                TravelCardEntity travelCard = null;
                if (travelCardNumber != null)
                {
                    localContext.Trace($"(ExecuteCodeActivity) Retrieving Travel Card.");
                    travelCard = XrmRetrieveHelper.Retrieve<TravelCardEntity>(localContext, travelCardNumber, new ColumnSet());
                    localContext.Trace($"(ExecuteCodeActivity) Retrieving Travel Card finished.");
                }

                ContactEntity contactEntity = null;
                if (contact != null)
                {

                    localContext.Trace($"(ExecuteCodeActivity) Retrieving Contact.");
                    contactEntity = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact, new ColumnSet(
                        ContactEntity.Fields.MobilePhone, ContactEntity.Fields.EMailAddress1));
                    localContext.Trace($"(ExecuteCodeActivity) Retrieving Contact finished.");
                }

                LeadEntity leadEntity = null;
                if (lead != null)
                {
                    localContext.Trace($"(ExecuteCodeActivity) Retrieving Lead.");
                    leadEntity = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, lead, new ColumnSet(
                        LeadEntity.Fields.MobilePhone, LeadEntity.Fields.EMailAddress1));
                    localContext.Trace($"(ExecuteCodeActivity) Retrieving Lead finished.");
                }

                RefundEntity refundEntity = null;
                if (refund != null)
                {
                    localContext.Trace($"(ExecuteCodeActivity) Retrieving Refund.");
                    refundEntity = XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, refund, new ColumnSet(
                        RefundEntity.Fields.cgi_Caseid));
                    localContext.Trace($"(ExecuteCodeActivity) Retrieving Refund finished.");
                }

                ValueCodeApprovalEntity valueCodeApproval = null;
                if (valueCodeApprovalId != null)
                {
                    localContext.Trace($"(ExecuteCodeActivity) Retrieving Approval.");
                    valueCodeApproval = XrmRetrieveHelper.Retrieve<ValueCodeApprovalEntity>(localContext, valueCodeApprovalId,
                        new ColumnSet(true));
                    localContext.Trace($"(ExecuteCodeActivity) Retrieving Approval finished.");
                }

                DateTime validTo = DateTime.Now.AddDays(365);

                // TODO : Get validToDays or specific date
                //if (validTo == null)
                //{
                //    int validDays = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_CreateValueCodeCoupons);
                //    validTo.AddDays(validDays);
                //}

                // Send to emailaddress given manually
                if (string.IsNullOrWhiteSpace(email) && deliveryType == (int)Schema.Generated.ed_valuecodedeliverytypeglobal.Email)
                {
                    localContext.Trace($"(ExecuteCodeActivity) Entering CreateEmailValueCodeGeneric 1.");
                    valueCodeId = ValueCodeHandler.CreateEmailValueCodeGeneric(localContext, deliveryType, validTo, (float)amount, periodPrice, email, voucherType, contactEntity, valueCodeApproval,
                                        lead: leadEntity, refund: refundEntity, travelCard: travelCard);

                    valueCodeCreated = true;
                }

                // Send to mobilephone given manually
                if (valueCodeCreated == false && string.IsNullOrWhiteSpace(mobile) && deliveryType == (int)Schema.Generated.ed_valuecodedeliverytypeglobal.SMS)
                {

                    localContext.Trace($"(ExecuteCodeActivity) Entering CreateMobileValueCodeGeneric 2.");
                    valueCodeId = ValueCodeHandler.CreateMobileValueCodeGeneric(localContext, deliveryType, validTo, (float)amount, periodPrice, mobile, voucherType, contactEntity, valueCodeApproval, template: null,
                                        lead: leadEntity, refund: refundEntity, travelCard: travelCard);

                    valueCodeCreated = true;
                }

                // Send to emailaddress on contact
                if (valueCodeCreated == false && string.IsNullOrWhiteSpace(contactEntity.EMailAddress1) && deliveryType == (int)Schema.Generated.ed_valuecodedeliverytypeglobal.Email)
                {
                    localContext.Trace($"(ExecuteCodeActivity) Entering CreateEmailValueCodeGeneric 3.");
                    valueCodeId = ValueCodeHandler.CreateEmailValueCodeGeneric(localContext, deliveryType, validTo, (float)amount, periodPrice, email, voucherType, contactEntity, valueCodeApproval,
                                        lead: leadEntity, refund: refundEntity, travelCard: travelCard);

                    valueCodeCreated = true;
                }

                // Send to mobilephone on contact
                if (valueCodeCreated == false && string.IsNullOrWhiteSpace(contactEntity.MobilePhone) && deliveryType == (int)Schema.Generated.ed_valuecodedeliverytypeglobal.SMS)
                {

                    localContext.Trace($"(ExecuteCodeActivity) Entering CreateMobileValueCodeGeneric 4.");
                    valueCodeId = ValueCodeHandler.CreateMobileValueCodeGeneric(localContext, deliveryType, validTo, (float)amount, periodPrice, mobile, voucherType, contactEntity, valueCodeApproval,
                                        lead: leadEntity, refund: refundEntity, travelCard: travelCard);

                    valueCodeCreated = true;
                }

                // Send to emailaddress on contact
                if (valueCodeCreated == false && string.IsNullOrWhiteSpace(leadEntity?.EMailAddress1) && deliveryType == (int)Schema.Generated.ed_valuecodedeliverytypeglobal.Email)
                {
                    localContext.Trace($"(ExecuteCodeActivity) Entering CreateEmailValueCodeGeneric 5.");
                    valueCodeId = ValueCodeHandler.CreateEmailValueCodeGeneric(localContext, deliveryType, validTo, (float)amount, periodPrice, email, voucherType, contactEntity, valueCodeApproval,
                                        lead: leadEntity, refund: refundEntity, travelCard: travelCard);

                    valueCodeCreated = true;
                }

                // Send to mobilephone on contact
                if (valueCodeCreated == false && string.IsNullOrWhiteSpace(leadEntity?.MobilePhone) && deliveryType == (int)Schema.Generated.ed_valuecodedeliverytypeglobal.SMS)
                {
                    localContext.Trace($"(ExecuteCodeActivity) Entering CreateMobileValueCodeGeneric 6.");
                    valueCodeId = ValueCodeHandler.CreateMobileValueCodeGeneric(localContext, deliveryType, validTo, (float)amount, periodPrice, mobile, voucherType, contactEntity, valueCodeApproval,
                                        lead: leadEntity, refund: refundEntity, travelCard: travelCard);

                    valueCodeCreated = true;
                }

                localContext.Trace($"(ExecuteCodeActivity) Returning ValueCodeRef");
                EntityReference valueCodeRef = new EntityReference(ValueCodeEntity.EntityLogicalName, valueCodeId);

                localContext.Trace($"<--- Exiting {nameof(ExecuteCodeActivity)}.");
                return valueCodeRef;
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("(CreateValueCodeGeneric) Error: " + ex);
            }
        }

        private static int GetDayDifference(DateTime validTo)
        {
            return (int)(validTo - DateTime.Now).TotalDays;
        }
    }
}
