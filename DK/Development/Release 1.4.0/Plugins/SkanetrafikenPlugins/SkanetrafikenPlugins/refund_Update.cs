using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using CRM2013.SkanetrafikenPlugins.CancelGiftCardService;
using System.ServiceModel;

using Microsoft.Xrm.Sdk.Query;
using CRM2013.SkanetrafikenPlugins.Common;

namespace CRM2013.SkanetrafikenPlugins
{
    public class refund_update : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData data = new PluginData(serviceProvider);

            if (data.Context.Depth > 1) //This plugin will trigger itself unless this is checked
            {
                return;
            }

            Entity targetEntity;
            if (data.Context.InputParameters.TryGetTargetEntity(out targetEntity))
            {
                data.Target = targetEntity;
            }

            try
            {
                if (data.Context.PreEntityImages.Contains("Before") && data.Context.PreEntityImages["Before"] != null)
                {
                    var target = data.Context.PreEntityImages["Before"];
                    if (target == null)
                    {
                        throw new ArgumentNullException("serviceProvider");
                    }

                    CancelGiftCode(target, data);

                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = String.Format(//"The refund_Update plugin failed " +
                    "Meddelande: {0} ",
                    ex.Message);
                throw new InvalidPluginExecutionException(exceptionMessage, ex);
            }
        }
        #endregion

        #region Private Methods
        private void CancelGiftCode(Entity refund, PluginData pluginData)
        {

            if (pluginData.Target.IsActive())
            {
                return;
            }

            if (refund.GiftCodeHasBeenUsed() || refund.GiftCodeHasExpired()) //Used or expired codes cannot be canceled
            {
                throw new InvalidPluginExecutionException("Värdekoden kan inte makuleras då den har använts eller utgått.");
            }


            var serviceUrl = GetCancelGiftCodeServiceUrl(pluginData);
            var success = CancelGiftCodeAtEhandel(refund.ValueCode(), serviceUrl);

            if (success)
            {
                RemoveGiftCodeFromRefundInCrm(refund, pluginData);
            }
            else
            {
                throw new UserMessageException(String.Format("Värdekoden {0} kunde ej makuleras.", refund.ValueCode()));
            }
        }

        private void RemoveGiftCodeFromRefundInCrm(Entity refund, PluginData pluginData)
        {
            var refundToUpdate = CreateRefundWithNoValueCode(refund);

            pluginData.Service.Update(refundToUpdate);
        }

        private bool CancelGiftCodeAtEhandel(string valueCode, string cancelGiftCardServiceUrl)
        {
            var request = new CancelGiftCardRequest
            {
                GiftCardCode = valueCode
            };


            BasicHttpBinding myBinding = new BasicHttpBinding
            {
                Name = "myBasicHttpBinding"
            };
            EndpointAddress endPointAddress = new EndpointAddress(cancelGiftCardServiceUrl);

            var cancelGiftCardClient = new CancelGiftCardClient(myBinding, endPointAddress);

            var response = cancelGiftCardClient.CancelGiftCard(request);

            return 200 == response.StatusCode;
        }

        private string GetCancelGiftCodeServiceUrl(PluginData data)
        {
            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_cancelgiftcodeservice' />";
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

            FetchExpression f = new FetchExpression(xml);
            EntityCollection settingscollection = data.Service.RetrieveMultiple(f);

            Entity settings = settingscollection.Entities.First();

            if (settings.Contains("cgi_cancelgiftcodeservice") && settings["cgi_cancelgiftcodeservice"] != null)
            {
                return settings.GetAttributeValue<string>("cgi_cancelgiftcodeservice");
            }
            throw new Exception("Required setting is missing: cgi_cancelgiftcodeservice");
        }

        private Entity CreateRefundWithNoValueCode(Entity oldRefund)
        {
            Entity newRefund = new Entity
            {
                LogicalName = oldRefund.LogicalName,
                Id = oldRefund.Id
            };
            return newRefund;
        }
        #endregion
    }
}
