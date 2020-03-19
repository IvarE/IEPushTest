using System;
using Microsoft.Xrm.Sdk;
using CRM2013.SkanetrafikenPlugins.Common;

namespace CRM2013.SkanetrafikenPlugins
{
    public class travelinformation_Update : IPlugin
    {
        #region Public Methods
        /// <summary>
        /// Related to CR -
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

                    HandleTimeDiffrence(data.Target);
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = String.Format("The travelinformation_Update plugin failed for " +
                    "travelinformation {0} " +
                    "with message {1} ",
                    data.Target.Id,
                    ex.Message);
                throw new InvalidPluginExecutionException(exceptionMessage, ex);
            }
        }
        #endregion

        #region Private Methods
        private void HandleTimeDiffrence(Entity travelInformation)
        {
            if (TravelInformationHandler.canceledBeforeStart(travelInformation)) //No diff calculation needed since buss was canceled before start..
            {

                return;
            }
            else if (travelInformation.Attributes.ContainsKey("cgi_startplanned") && travelInformation.Attributes.ContainsKey("cgi_startactual")) //Calculate start diffrence...
            {
                DateTime startPlanned = travelInformation.GetAttributeValue<DateTime>("cgi_startplanned");
                DateTime startActual = Convert.ToDateTime(travelInformation.GetAttributeValue<string>("cgi_startactual"));

                TimeSpan startDiffrence = startActual.Subtract(startPlanned);
            }

            if (TravelInformationHandler.canceledBeforeArrival(travelInformation)) //No diff calculation for arrival needed since buss was canceled before arrival..
            {

                return;
            }
            else if (travelInformation.Attributes.ContainsKey("cgi_arrivalactual") && travelInformation.Attributes.ContainsKey("cgi_arrivalplanned")) //Calculate arrival diffrence...
            {
                DateTime arrivalPlanned = travelInformation.GetAttributeValue<DateTime>("cgi_arrivalactual");
                DateTime arrivalActual = Convert.ToDateTime(travelInformation.GetAttributeValue<string>("cgi_arrivalplanned"));

                TimeSpan arrivalDiffrence = arrivalActual.Subtract(arrivalPlanned);
            }
        }
        #endregion
    }
}
