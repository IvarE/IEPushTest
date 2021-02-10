using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;

namespace CRM2013.SkanetrafikenPlugins
{
    public class travelinformation_Delete : IPlugin
    {
        private class PluginData : PlugindataBase
        {
            public PluginData(IServiceProvider serviceProvider) : base(serviceProvider) { }
        }

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
                if (_data.Context.PreEntityImages.Contains("Before") && _data.Context.PreEntityImages["Before"] is Entity)
                {
                    var Target = (Entity)_data.Context.PreEntityImages["Before"];

                    HandleRelatedCase(Target, _data);
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = String.Format("The travelinformation_Post plugin failed " +
                    "with message {0} ",
                    ex.Message);
                throw new InvalidPluginExecutionException(exceptionMessage, ex);
            }
        }

        private void HandleRelatedCase(Entity travelInformation, PluginData _data)
        {
            if (!TravelInformationHandler.IsRelatedToCase(travelInformation)) //Travel info created without case id - no need to continue
            {
                return;
            }

            var crmCaseId = (EntityReference)travelInformation.Attributes["cgi_caseid"];

            var relatedTravelInfos = TravelInformationHandler.GetRelatedTravelInfos(crmCaseId, _data.Service);

            Entity caseToUpdate = new Entity();
            if (relatedTravelInfos.Any()) //There are still related travel infos - we update the case with the first related travel info
            {
                caseToUpdate = TravelInformationHandler.CreateCaseWithUpdatedTravelInformation(relatedTravelInfos.ElementAt<Entity>(0));
            }

            else //The one we deleted was the last one - should empty travel info fields on case
            {
                caseToUpdate = TravelInformationHandler.CreateCaseWithEmptyTravelInformation(travelInformation);
            }

            TravelInformationHandler.UpdateCase(caseToUpdate, _data.Service);
        }

    }
}
