using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using CRM2013.SkanetrafikenPlugins.CancelGiftCardService;
using System.ServiceModel;

using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using CRM2013.SkanetrafikenPlugins.Common;

namespace CRM2013.SkanetrafikenPlugins
{
    public class refund_update : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData _data = new PluginData(serviceProvider);

            if (_data.Context.Depth > 1) //This plugin will trigger itself unless this is checked
            {
                return;
            }

            Entity target;
            if (_data.Context.InputParameters.TryGetTargetEntity(out target))
            {
                _data.Target = target;
            }

            try
            {
                if (_data.Context.PreEntityImages.Contains("Before") && _data.Context.PreEntityImages["Before"] is Entity)
                {
                    var Target = (Entity)_data.Context.PreEntityImages["Before"];

                    CancelGiftCode(Target, _data);

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
            var request = new CancelGiftCardRequest();

            request.GiftCardCode = valueCode;

            BasicHttpBinding myBinding = new BasicHttpBinding();
            myBinding.Name = "myBasicHttpBinding";
            EndpointAddress endPointAddress = new EndpointAddress(cancelGiftCardServiceUrl);

            var cancelGiftCardClient = new CancelGiftCardClient(myBinding, endPointAddress);

            var response = cancelGiftCardClient.CancelGiftCard(request);

            return 200 == response.StatusCode;
        }

        private string GetCancelGiftCodeServiceUrl(PluginData _data)
        {
            string _now = DateTime.Now.ToString("s");
            string _xml = "";
            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_cancelgiftcodeservice' />";
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

            FetchExpression _f = new FetchExpression(_xml);
            EntityCollection settingscollection = _data.Service.RetrieveMultiple(_f);

            Entity settings = settingscollection.Entities.First();

            if (settings.Contains("cgi_cancelgiftcodeservice") && settings["cgi_cancelgiftcodeservice"] != null)
            {
                return settings.GetAttributeValue<string>("cgi_cancelgiftcodeservice");
            }
            else
            {
                throw new Exception("Required setting is missing: cgi_cancelgiftcodeservice");
            }
        }

        private Entity CreateRefundWithNoValueCode(Entity oldRefund)
        {
            Entity newRefund = new Entity();
            newRefund.LogicalName = oldRefund.LogicalName;
            newRefund.Id = oldRefund.Id;
            //newRefund["cgi_value_code"] = null; should not be nulled
            //newRefund["cgi_value_code_used"] = DateTime.Now; should not be set
            return newRefund;
        }
        #endregion
    }
}
