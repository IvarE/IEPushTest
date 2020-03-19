using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using CRM2013.SkanetrafikenPlugins.Common;

namespace CRM2013.SkanetrafikenPlugins
{
    public class travelinformation_Delete : IPlugin
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
            //PluginData data = new PluginData(serviceProvider);

            try
            {
                /****************This functionality is moved to the assembly TravelInformationPlugin*************/

                //if (data.Context.PreEntityImages.Contains("Before") && data.Context.PreEntityImages["Before"] != null)
                //{
                //    var target = data.Context.PreEntityImages["Before"];

                //    HandleRelatedCase(target, data);
                //}
            }
            catch (Exception ex)
            {
                var exceptionMessage = String.Format("The travelinformation_Post plugin failed " +
                    "with message {0} ",
                    ex.Message);
                throw new InvalidPluginExecutionException(exceptionMessage, ex);
            }
        }
        #endregion

        #region Private Methods
        private void HandleRelatedCase(Entity travelInformation, PluginData data)
        {
            if (!TravelInformationHandler.IsRelatedToCase(travelInformation)) //Travel info created without case id - no need to continue
            {
                return;
            }

            var crmCaseId = (EntityReference)travelInformation.Attributes["cgi_caseid"];

            var relatedTravelInfos = TravelInformationHandler.GetRelatedTravelInfos(crmCaseId, data.Service);

            Entity caseToUpdate;
            if (relatedTravelInfos.Any()) //There are still related travel infos - we update the case with the first related travel info
            {
                caseToUpdate = TravelInformationHandler.CreateCaseWithUpdatedTravelInformation(relatedTravelInfos.ElementAt<Entity>(0));
            }

            else //The one we deleted was the last one - should empty travel info fields on case
            {
                caseToUpdate = TravelInformationHandler.CreateCaseWithEmptyTravelInformation(travelInformation);
            }

            TravelInformationHandler.UpdateCase(caseToUpdate, data.Service);
        }
        #endregion
    }
}
