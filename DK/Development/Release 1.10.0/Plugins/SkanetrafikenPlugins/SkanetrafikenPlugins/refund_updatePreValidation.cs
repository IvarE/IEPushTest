using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace CRM2013.SkanetrafikenPlugins
{
    public class refund_updatePreValidation : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData data = new PluginData(serviceProvider);
            try
            {
                if (data.Context.PreEntityImages.Contains("Before") && data.Context.PreEntityImages["Before"] != null)
                {
                    data.Target = (Entity)data.Context.InputParameters["Target"];
                    var before = data.Context.PreEntityImages["Before"];


                    //skapa variabel för reimbursement form
                    EntityReference rfId = null;
                    if (data.Target.Contains("cgi_reimbursementformid") && data.Target["cgi_reimbursementformid"] != null)
                    {
                        //reimbursement form har ändrats och finns i plugin-target
                        rfId = data.Target.GetAttributeValue<EntityReference>("cgi_reimbursementformid");
                    }
                    else
                    {

                        //reimbursement form har INTE ändrats och hämtas istället från PreEntityImage
                        if (before.Contains("cgi_reimbursementformid") && before["cgi_reimbursementformid"] != null)
                            rfId = before.GetAttributeValue<EntityReference>("cgi_reimbursementformid");
                    }

                    if (rfId != null)
                    {
                        //Reimbursement form har angivits

                        //kontrollera om cgi_vat_code har angivits
                        bool cgiVatCodeIsNull = true;
                        if (data.Target.Contains("cgi_vat_code"))
                        {
                            //cgi_vat_code har ändrats och vi använder oss av det uppdaterade värdet
                            cgiVatCodeIsNull = data.Target["cgi_vat_code"] == null;
                        }
                        else
                        {
                            //cgi_vat_code har INTE ändrats och hämtas istället från PreEntityImage
                            if (before.Contains("cgi_vat_code"))
                            {
                                cgiVatCodeIsNull = (before["cgi_vat_code"] == null);
                            }
                        }

                        if (cgiVatCodeIsNull)
                        {
                            //vat code har inte angivits och därför kontrolleras ifall det är ett krav för denna reimbursement form
                            Entity reimbursementForm = data.Service.Retrieve(rfId.LogicalName, rfId.Id, new ColumnSet("cgi_useaccount"));
                            if (reimbursementForm.Contains("cgi_useaccount") && reimbursementForm.GetAttributeValue<bool>("cgi_useaccount"))
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
