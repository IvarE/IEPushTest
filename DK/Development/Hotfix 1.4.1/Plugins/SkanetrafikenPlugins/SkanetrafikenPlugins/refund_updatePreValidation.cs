using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using CRM2013.SkanetrafikenPlugins.CancelGiftCardService;
using System.ServiceModel;

using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using System;

namespace CRM2013.SkanetrafikenPlugins
{
    public class refund_updatePreValidation : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData _data = new PluginData(serviceProvider);
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
        #endregion
    }
}
