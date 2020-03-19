using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class CreateValueCodeApproval : CodeActivity
    {
        [Input("Amount")]
        public InArgument<decimal> Amount { get; set; }

        [Input("CardNumber")]
        public InArgument<string> CardNumber { get; set; }

        [Input("ContactId")]
        //[ReferenceTarget("contact")]
        public InArgument<string> Contact { get; set; }

        [Input("DeliveryMethod")]
        public InArgument<int> DeliveryMethod { get; set; }

        [Input("Email")]
        public InArgument<string> Email { get; set; }

        [Input("Firstname")]
        public InArgument<string> Firstname { get; set; }

        [Input("Lastname")]
        public InArgument<string> Lastname { get; set; }

        [Input("Mobile")]
        public InArgument<string> Mobile { get; set; }

        [Input("NeedsManualApproval")]
        public InArgument<bool> NeedsManualApproval { get; set; }

        [Input("ValidTo")]
        public InArgument<DateTime> ValidTo { get; set; }

        [Input("TypeOfValueCode")]
        public InArgument<int> TypeOfValueCode { get; set; }

        [Output("ValueCodeApprovalId")]
        //[ReferenceTarget("ed_valuecodeapproval")]
        public OutArgument<string> ValueCodeApprovalId { get; set; }

        [Output("ResultCreateValueCodeApproval")]
        public OutArgument<string> ResultCreateValueCodeApproval { get; set; }

        private Plugin.LocalPluginContext GetLocalContext(CodeActivityContext activityContext)
        {
            IWorkflowContext workflowContext = activityContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService organizationService = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);
            ITracingService tracingService = activityContext.GetExtension<ITracingService>();


            return new Plugin.LocalPluginContext(null, organizationService, null, tracingService);
        }

        protected override void Execute(CodeActivityContext activityContext)
        {
            //TRY EXECUTE
            try
            {
                //GENERATE CONTEXT
                Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);

                localContext.TracingService.Trace($"CreateValueCodeApproval started.");



                //GET VALUE(S)
                decimal amount = Amount.Get(activityContext);
                string cardNumber = CardNumber.Get(activityContext);
                string contactId = Contact.Get(activityContext);
                int deliveryMethod = DeliveryMethod.Get(activityContext);
                string email = Email.Get(activityContext);
                string firstname = Firstname.Get(activityContext);
                string lastname = Lastname.Get(activityContext);
                string mobile = Mobile.Get(activityContext);
                bool needsManualApproval = NeedsManualApproval.Get(activityContext);
                DateTime validTo = ValidTo.Get(activityContext);
                int typeOfValueCode = TypeOfValueCode.Get(activityContext);


                ValueCodeApprovalEntity approval = ExecuteCodeActivity(localContext, amount, cardNumber, new EntityReference(ContactEntity.EntityLogicalName, Guid.Parse(contactId)), deliveryMethod, email, firstname, lastname, mobile, needsManualApproval, validTo, typeOfValueCode);
                localContext.Trace($"CreateValueCodeApproval finished. ValueCodeApprovalId: {approval.Id}");

                ValueCodeApprovalId.Set(activityContext, approval.Id.ToString());
            }
            catch (Exception ex)
            {
                ResultCreateValueCodeApproval.Set(activityContext, $"Kunde inte skapa värdkod. Vänligen kontakta kundtjänst. StripError (CreateValueCodeApproval): { ex.Message}");
            }
        }
        public static ValueCodeApprovalEntity ExecuteCodeActivity(Plugin.LocalPluginContext localContext, decimal amount, string cardNumber, EntityReference contact, int deliveryMethod,
            string email, string firstname, string lastname, string mobile, bool needsManualApproval, DateTime validTo, int typeOfValueCode)
        {
            ValueCodeApprovalEntity approval = ValueCodeApprovalEntity.CreateValueCodeApproval(localContext, amount, cardNumber, contact, deliveryMethod, email, firstname, lastname, mobile, needsManualApproval, validTo, typeOfValueCode);
            return approval;
        }
    }
}
