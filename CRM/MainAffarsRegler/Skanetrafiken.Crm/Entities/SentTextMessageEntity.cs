using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.TextMessageSender;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class SentTextMessageEntity : Generated.ed_senttextmessage
    {
        private static BosbecAPIHandler APIHandler;

        public enum Status {
            Sent = 899310000,
            Delivered = 899310001,
            Failed = 899310002,
            NotProcessed = 899310003,
            Processed = 899310004
        }

        private static BosbecAPIHandler GetBosbecAPIHandler(Plugin.LocalPluginContext localContext)
        {
            if (SentTextMessageEntity.APIHandler == null)
            {
                string apiKey = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_BosbecAPIKey);
                string bosbecUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_Bosbecurl);
                string workflowId = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_BosbecWorkflowId);

                APIHandler = new BosbecAPIHandler()
                {
                    apiKey = apiKey,
                    workflowId = workflowId,
                    baseUrlStr = bosbecUrl
                };
            }

            return APIHandler;
        }

        public string GetPhoneNumber()
        {
            if(ed_PhoneNumber.First().Equals('0'))
                return "+46" + ed_PhoneNumber.Substring(1);

            return ed_PhoneNumber;
        }

        public void RetrieveUpdatedStatus(Plugin.LocalPluginContext localContext)
        {
            GetBosbecAPIHandler(localContext);

            if (this.statuscode.Value == (int)SentTextMessageEntity.Status.Delivered)
                return;

            BosbecAPIHandler.ResponseMessage response = GetUpdatedReponse(localContext);

            if (response == null)
                return;

            int? UpdatedStatus = GetMessageStatus(localContext, response);

            if (UpdatedStatus != null && this.statuscode.Value != UpdatedStatus)
                this.statuscode = new OptionSetValue((int)UpdatedStatus);
        }

        private static int? GetMessageStatus(Plugin.LocalPluginContext localContext, BosbecAPIHandler.ResponseMessage response)
        {
            int statusType;
            if (Int32.TryParse(response.statusType, out statusType))
            {
                if (statusType == 6)
                    return (int)SentTextMessageEntity.Status.Delivered;
                else if (statusType == 2 || statusType == 5 || statusType == 7 || statusType == 9 || statusType == 10 || statusType == 13)
                    return (int)SentTextMessageEntity.Status.Failed;
                else if (statusType == 4)
                    return (int)SentTextMessageEntity.Status.Sent;
                else
                    localContext.Trace($"Entered GetMessageStatus() Status Type : { statusType } not implemented.");
            }
            else
                localContext.Trace($"Entered GetMessageStatus() Status Type : { response.statusType } is not an integer.");

            return null;
        }

        private BosbecAPIHandler.ResponseMessage GetUpdatedReponse(Plugin.LocalPluginContext localContext)
        {
            if (this.ed_ProcessId != null)
            {
                if (this.ed_MessageId != null)
                {
                    try
                    {
                        return APIHandler.GetDeliveryReport<BosbecAPIHandler.ResponseMessage>(this.ed_MessageId);
                    }
                    catch (WebException)
                    {
                        return null;
                    }
                }
                else
                {
                    try
                    {
                        string phonenumber = GetPhoneNumber();
                        BosbecAPIHandler.DeliveryReportResponse deliveryReport = APIHandler.GetDeliveryReport<BosbecAPIHandler.DeliveryReportResponse>(this.ed_ProcessId);
                        IEnumerable<BosbecAPIHandler.ResponseMessage> responseMessages = deliveryReport.messages.Where(d => d.to.Equals(phonenumber));

                        if (responseMessages.Count() == 1)
                        {
                            BosbecAPIHandler.ResponseMessage responseMessage = responseMessages.First();
                            this.ed_MessageId = responseMessage.messageId;
                            return responseMessage;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (WebException)
                    {
                        return null;
                    }
                }
            }

            return null;
        }

    }
}
