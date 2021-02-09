using System;
using Microsoft.Xrm.Sdk;
using CRM2013.SkanetrafikenPlugins.Common;

namespace CRM2013.SkanetrafikenPlugins
{
    public class refund_Post : IPlugin
    {
        #region Public Methods
        /// <summary>
        /// Related to CR - Skapa epostmallar-beslutsvärden (redmine 2109)
        /// If a refund is added to a case
        /// the refund information (data field values) should be copied to the case.
        /// The refund information can then be used in email templates.
        /// The refund informaiton shall always be from the newest refund. 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData data = new PluginData(serviceProvider);
           
            try
            {
                if (data.Context.InputParameters.Contains("Target") && data.Context.InputParameters["Target"] is Entity)
                {
                    data.Target = (Entity)data.Context.InputParameters["Target"];
                   
                    HandleRelatedCase(data.Target, data);
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = String.Format("The refund_Post plugin failed for " +
                    "refund {0} " +
                    "with message {1} ",
                    data.Target.Id,
                    ex.Message);
                throw new InvalidPluginExecutionException(exceptionMessage, ex);
            }
        }
        #endregion

        #region Private Methods
        //Every time a refund is saved, the refund informaiton is saved to the related case 
        //for access when creating email/templates.
        private void HandleRelatedCase( Entity refund, PluginData data)
        {
            if (!RefundHandler.IsRelatedToCase(refund)) //Refund created without case id - no need to continue
            {
               return;
            }

            Entity caseToUpdate = RefundHandler.CreateCaseWithUpdatedRefundInformation(refund);
            
            RefundHandler.UpdateCase(caseToUpdate, data.Service);

        }
        #endregion
    }
}
