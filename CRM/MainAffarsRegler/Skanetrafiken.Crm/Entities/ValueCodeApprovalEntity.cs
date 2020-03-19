using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using static Skanetrafiken.Crm.Entities.TravelCardEntity;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class ValueCodeApprovalEntity : Generated.ed_ValueCodeApproval
    {
        internal static void HandlePreValueCodeApprovalCreate(Plugin.LocalPluginContext localContext, ValueCodeApprovalEntity target)
        {
            if (target.Contains(ValueCodeApprovalEntity.Fields.ed_TravelCardNumber))
            {
                if (!String.IsNullOrEmpty(target.ed_TravelCardNumber))
                {
                    CardDetailsEnvelope.Envelope cardDetails = TravelCardEntity.GetAndParseCardDetails(localContext, target.ed_TravelCardNumber);
                    TravelCardEntity.ValidateTravelCard(localContext, cardDetails);

                    target.ed_Amount = (int)cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.Balance;
                }
            }

        }

        internal static void HandlePreValueCodeApprovalUpdate(Plugin.LocalPluginContext localContext, ValueCodeApprovalEntity target, ValueCodeApprovalEntity preImage)
        {
            ValueCodeApprovalEntity combined = new ValueCodeApprovalEntity { Id = target.Id };
            if (preImage != null)
                combined.CombineAttributes(preImage);
            combined.CombineAttributes(target);

            if (combined.Contains(ValueCodeApprovalEntity.Fields.ed_TravelCardNumber))
            {
                if (!String.IsNullOrEmpty(combined.ed_TravelCardNumber))
                {
                    CardDetailsEnvelope.Envelope cardDetails = TravelCardEntity.GetAndParseCardDetails(localContext, combined.ed_TravelCardNumber);
                    TravelCardEntity.ValidateTravelCard(localContext, cardDetails);

                    target.ed_Amount = (int)cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.Balance;
                }
            }
        }

        public static void ChangeState(Plugin.LocalPluginContext localContext, EntityReference valueCodeApproval, Generated.ed_valuecodeapproval_statuscode status,
            string valueCode = null)
        {
            localContext.TracingService.Trace("Entered function ChangeState");

            var vca = XrmRetrieveHelper.Retrieve<ValueCodeApprovalEntity>(localContext, valueCodeApproval.Id, new ColumnSet(ValueCodeApprovalEntity.Fields.statuscode));

            if (vca == null)
                throw new InvalidPluginExecutionException($"Could not find ValueCodeApproval based on id: {valueCodeApproval.Id}");

            if (vca.statuscode == Generated.ed_valuecodeapproval_statuscode.Approved)
            {
                localContext.TracingService.Trace("Value code approval is already approved.");
                return;
            }
                

            if (!string.IsNullOrEmpty(valueCode))
                vca.ed_ValueCode = new EntityReference(ValueCodeEntity.EntityLogicalName, Guid.Parse(valueCode));

            vca.statuscode = status;
            XrmHelper.Update(localContext, vca);
        }

        public static ValueCodeApprovalEntity CreateValueCodeApproval(Plugin.LocalPluginContext localContext, decimal amount, string cardNumber, EntityReference contact, int deliveryMethod,
            string email, string firstname, string lastname, string mobile, bool needsManualApproval, DateTime validTo, int typeOfValueCode)
        {
            ValueCodeApprovalEntity approval = new ValueCodeApprovalEntity();
            approval.ed_Amount = (int)amount;
            approval.ed_TravelCardNumber = cardNumber;
            approval.ed_Contact = contact;
            approval.ed_EmailAddress = email;
            approval.ed_Firstname = firstname;
            approval.ed_Lastname = lastname;
            approval.ed_Mobile = mobile;
            approval.ed_ValidTo = validTo;
            approval.ed_NeedsManualApproval = needsManualApproval;

            if (deliveryMethod == (int)Generated.ed_valuecodedeliverytypeglobal.Email)
            {
                approval.ed_ValueCodeDeliveryTypeGlobal = Generated.ed_valuecodedeliverytypeglobal.Email;
                approval.ed_name = $"{email} - {amount}";
            }
            if (deliveryMethod == (int)Generated.ed_valuecodedeliverytypeglobal.SMS)
            {
                approval.ed_ValueCodeDeliveryTypeGlobal = Generated.ed_valuecodedeliverytypeglobal.SMS;
                approval.ed_name = $"{mobile} - {amount}";
            }

            if (typeOfValueCode == (int)Generated.ed_valuecodetypeglobal.Ersattningsarende)
                approval.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Ersattningsarende;

            if (typeOfValueCode == (int)Generated.ed_valuecodetypeglobal.Forlustgaranti)
                approval.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Forlustgaranti;

            if (typeOfValueCode == (int)Generated.ed_valuecodetypeglobal.Forseningsersattning)
                approval.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Forseningsersattning;

            if (typeOfValueCode == (int)Generated.ed_valuecodetypeglobal.InlostReskassa)
                approval.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.InlostReskassa;

            Guid approvalId = XrmHelper.Create(localContext, approval);
            approval =  XrmRetrieveHelper.Retrieve<ValueCodeApprovalEntity>(localContext, approvalId, new ColumnSet(false));

            return approval;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="localContext"></param>
        ///// <param name="valueCode"></param>
        ///// <param name="travelCard"></param>
        ///// <param name="maxAmount"></param>
        ///// <param name="validDays"></param>
        ///// <param name="contact"></param>
        ///// <returns></returns>
        //public static ValueCodeApprovalEntity CreateValueCodeApproval(Plugin.LocalPluginContext localContext, Skanetrafiken.Crm.Models.ValueCodeCreation valueCode, TravelCardEntity travelCard, 
        //    int maxAmount, int validDays, ContactEntity contact = null)
        //{
        //    try
        //    {
        //        //Create value code approval
        //        var valCodeApproval = new ValueCodeApprovalEntity();
        //        valCodeApproval.ed_Amount = valueCode.Amount;
        //        valCodeApproval.ed_ValidTo = DateTime.Now.AddDays(validDays);
        //        valCodeApproval.ed_Firstname = valueCode.FirstName;
        //        valCodeApproval.ed_Lastname = valueCode.LastName;
        //        valCodeApproval.ed_TravelCardNumber = travelCard.cgi_travelcardnumber;

        //        //Check if it's an anonymous user (Meaning, does not exist in CRM).
        //        if (contact == null)
        //            valCodeApproval.ed_AnonymousCustomer = true;
        //        else valCodeApproval.ed_Contact = contact.ToEntityReference();

        //        //If exceeds max amount, then value code appproval has to be approved manually.
        //        if (valueCode.Amount > maxAmount)
        //            valCodeApproval.ed_NeedsManualApproval = true;
        //        else valCodeApproval.ed_NeedsManualApproval = false;

        //        if (string.IsNullOrEmpty(valueCode.deliveryType.Mobile))
        //            valCodeApproval.ed_EmailAddress = valueCode.deliveryType.Email;
        //        else
        //            valCodeApproval.ed_Mobile = valueCode.deliveryType.Mobile;

        //        valCodeApproval.Id = XrmHelper.Create(localContext, valCodeApproval);
        //        return valCodeApproval;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}

    }
}
