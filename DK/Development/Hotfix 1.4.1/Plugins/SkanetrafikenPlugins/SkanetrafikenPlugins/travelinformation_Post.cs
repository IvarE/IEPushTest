using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
using CRM2013.SkanetrafikenPlugins.Common;

namespace CRM2013.SkanetrafikenPlugins
{
    public class travelinformation_Post : IPlugin
    {
        #region Public Methods
        /// <summary>
        /// Related to CR - Trafikinfo på ärende.
        /// If a travel information is added to a case (where there previously was none),
        /// the travel information should be copied to the case. The travel information
        /// can then be used in email templates.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData _data = new PluginData(serviceProvider);

            try
            {
                if (_data.Context.InputParameters.Contains("Target") && _data.Context.InputParameters["Target"] is Entity)
                {
                    _data.Target = (Entity)_data.Context.InputParameters["Target"];

                    HandleRelatedCase(_data.Target, _data);
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = String.Format("The travelinformation_Post plugin failed for " +
                    "travelinformation {0} " +
                    "with message {1} ",
                    _data.Target.Id,
                    ex.Message);
                throw new InvalidPluginExecutionException(exceptionMessage, ex);
            }
        }
        #endregion

        #region Private Methods
        private void HandleRelatedCase(Entity travelInformation, PluginData _data)
        {
            if (!TravelInformationHandler.IsRelatedToCase(travelInformation)) //Travel info created without case id - no need to continue
            {
                return;
            }

            var crmCaseId = (EntityReference)travelInformation.Attributes["cgi_caseid"];

            var relatedTravelInfos = TravelInformationHandler.GetRelatedTravelInfos(crmCaseId, _data.Service);

            if (relatedTravelInfos.Count == 1) //The one that was just saved is the only travel info => we update the case with the info
            {
                Entity caseToUpdate = TravelInformationHandler.CreateCaseWithUpdatedTravelInformation(travelInformation);
                TravelInformationHandler.UpdateCase(caseToUpdate, _data.Service);
            }
        }
        #endregion
    }
}
