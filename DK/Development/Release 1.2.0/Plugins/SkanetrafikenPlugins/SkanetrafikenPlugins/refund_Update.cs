using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using CRM2013.SkanetrafikenPlugins.CancelGiftCardService;
using System.ServiceModel;

using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;

namespace CRM2013.SkanetrafikenPlugins
{
    public class refund_updatePreValidation : IPlugin
    {
        private class plugindata : PlugindataBase
        {
            public plugindata(IServiceProvider serviceProvider) : base(serviceProvider) { }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            plugindata _data = new plugindata(serviceProvider);
            try
            {
                if (_data.Context.PreEntityImages.Contains("Before") && _data.Context.PreEntityImages["Before"] is Entity)
                {
                    _data.Target = (Entity)_data.Context.InputParameters["Target"];
                    var Before = (Entity)_data.Context.PreEntityImages["Before"];
                    
                   
                    //skapa variabel för reimbursement form
                    EntityReference rf_id = null;
                    if (_data.Target.Contains("cgi_reimbursementformid") && _data.Target["cgi_reimbursementformid"] != null)
                    {
                        //reimbursement form har ändrats och finns i plugin-target
                        rf_id = _data.Target.GetAttributeValue<EntityReference>("cgi_reimbursementformid");
                    }
                    else
                    {

                        //reimbursement form har INTE ändrats och hämtas istället från PreEntityImage
                        if (Before.Contains("cgi_reimbursementformid") && Before["cgi_reimbursementformid"] != null)
                            rf_id = Before.GetAttributeValue<EntityReference>("cgi_reimbursementformid");
                    }
            
                    if (rf_id != null)
                    {
                        //Reimbursement form har angivits

                        //kontrollera om cgi_vat_code har angivits
                        bool cgi_vat_codeIsNull = true;
                        if (_data.Target.Contains("cgi_vat_code"))
                        {
                            //cgi_vat_code har ändrats och vi använder oss av det uppdaterade värdet
                            cgi_vat_codeIsNull = (_data.Target["cgi_vat_code"] == null);
                        }
                        else
                        {
                            //cgi_vat_code har INTE ändrats och hämtas istället från PreEntityImage
                            if (Before.Contains("cgi_vat_code"))
                            {
                                cgi_vat_codeIsNull = (Before["cgi_vat_code"] == null);
                            }
                        }
            
                        if (cgi_vat_codeIsNull)
                        {
                            //vat code har inte angivits och därför kontrolleras ifall det är ett krav för denna reimbursement form
                            Entity reimbursementForm = _data.Service.Retrieve(rf_id.LogicalName, rf_id.Id, new ColumnSet("cgi_useaccount"));
                            if (reimbursementForm.Contains("cgi_useaccount") && reimbursementForm.GetAttributeValue<bool>("cgi_useaccount") == true)
                            {
                                throw new InvalidPluginExecutionException("Momskod måste anges för angiven utbetalningsform!");
                            }
                        }
                    }
                }
                else
                    throw new InvalidPluginExecutionException("Bad configuration! Plugin PreEntityImage named 'Before' is missing!");

            }
            catch (Exception ex)
            {
                var exceptionMessage = String.Format(//"The refund_Update plugin failed " +
                    "Meddelande: {0} ",
                    ex.Message);
                throw new InvalidPluginExecutionException(exceptionMessage, ex);
            }
        }
    }


    public class refund_update : IPlugin
    {
        private class plugindata : PlugindataBase
        {
            public plugindata(IServiceProvider serviceProvider) : base(serviceProvider) { }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            plugindata _data = new plugindata(serviceProvider);

            if (_data.Context.Depth > 1) //This plugin will trigger itself unless this is checked
            {
                return;
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

        private void CancelGiftCode(Entity refund, plugindata pluginData)
        {
            if (refund.IsAutoGenerated()) //Generated from RGOL and inactivated automatically, we shall not deal with these
            {
                return;
            }

            if (refund.IsActive())
            {
                throw new InvalidPluginExecutionException("Värdekoden kan inte makuleras då den är aktiv.");
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

        private void RemoveGiftCodeFromRefundInCrm(Entity refund, plugindata pluginData)
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

        private string GetCancelGiftCodeServiceUrl(plugindata _data)
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

    }

    public static class RefundExtensionMethods
    {
        public static bool IsAutoGenerated(this Entity refund)
        {
            return refund.Contains("cgi_isautogenerated") && refund.GetAttributeValue<bool>("cgi_isautogenerated") == true;
        }

        public static bool IsActive(this Entity refund)
        {
            var inactive = new OptionSetValue(0);

            return refund.Contains("statecode") && refund.GetAttributeValue<OptionSetValue>("statecode") == inactive;
        }

        public static bool GiftCodeHasBeenUsed(this Entity refund)
        {
            //A used date field is present in CRM, but there is currently no functionality to fill it when a code is used
            var dateUsed = refund.GetValue<DateTime>("cgi_value_code_used");

            return dateUsed != default(DateTime);
        }

        public static bool GiftCodeHasExpired(this Entity refund)
        {
            var expiryDate = refund.GetValue<DateTime>("cgi_last_valid");

            if (expiryDate == default(DateTime)) //no date present, gift code cannot expire
            {
                return false;
            }
            else
            {
                return expiryDate < DateTime.Now;
            }
        }

        public static string ValueCode(this Entity refund)
        {
            return refund.GetAttributeValue<string>("cgi_value_code");
        }
    }
}
