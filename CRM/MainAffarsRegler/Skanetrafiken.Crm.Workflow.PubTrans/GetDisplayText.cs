using System;
using System.Activities;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

using Endeavor.Crm;

namespace Skanetrafiken.Crm
{
    public class GetDisplayText : CodeActivity
    {
        [Input("StartPlanned")]
        public InArgument<DateTime> StartPlanned { get; set; }

        [Input("StartActual")]
        public InArgument<DateTime> StartActual { get; set; }

        [Input("ArrivalPlanned")]
        public InArgument<DateTime> ArrivalPlanned { get; set; }

        [Input("ArrivalActual")]
        public InArgument<DateTime> ArrivalActual { get; set; }

        [Input("InputFormat")]
        [RequiredArgument()]
        public InArgument<string> InputFormat { get; set; }

        [Input("Transport")]
        public InArgument<string> Transport { get; set; }

        [Input("City")]
        public InArgument<string> City { get; set; }

        [Input("Line")]
        public InArgument<string> Line { get; set; }

        [Input("LineDesignation")]
        public InArgument<string> LineDesignation { get; set; }

        [Input("DirectionOfText")]
        public InArgument<string> DirectionOfText { get; set; }

        [Input("Tour")]
        public InArgument<string> Tour { get; set; }

        [Input("Start")]
        public InArgument<string> Start { get; set; }

        [Input("Stop")]
        public InArgument<string> Stop { get; set; }

        [Input("Contractor")]
        public InArgument<string> Contractor { get; set; }

        [Output("DisplayText")]
        public OutArgument<string> DisplayText { get; set; }

        private Plugin.LocalPluginContext GetLocalContext(CodeActivityContext activityContext)
        {
            IWorkflowContext workflowContext = activityContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService organizationService = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);
            ITracingService tracingService = activityContext.GetExtension<ITracingService>();

            return new Plugin.LocalPluginContext(null, organizationService, null, tracingService);
        }

        protected override void Execute(CodeActivityContext activityContext)
        {
            //GENERATE CONTEXT
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
            localContext.Trace($"GetDisplayText started.");

            //TRY EXECUTE
            try
            {
                // GET VALUE(S)
                string inputFormat = InputFormat.Get(activityContext);
                string transport = Transport.Get(activityContext);
                string city = City.Get(activityContext);
                string line = Line.Get(activityContext);
                string linedesignation = LineDesignation.Get(activityContext);
                string directionOfText = DirectionOfText.Get(activityContext);
                string tour = Tour.Get(activityContext);
                string start = Start.Get(activityContext);
                string stop = Stop.Get(activityContext);
                string contractor = Contractor.Get(activityContext);

                DateTime dStartPlanned = StartPlanned.Get(activityContext).ToLocalTime();
                DateTime dStartActual = StartActual.Get(activityContext);
                DateTime dArrivalPlanned = ArrivalPlanned.Get(activityContext).ToLocalTime();
                DateTime dArrivalActual = ArrivalActual.Get(activityContext);

                string sTour = "";
                if (start != null || stop != null)
                    sTour = GetTour(inputFormat, tour, start, stop, dStartPlanned, dStartActual, dArrivalPlanned, dArrivalActual);

                string displayText = ExecuteCodeActivity(transport, city, line, linedesignation, directionOfText, sTour, contractor);
                DisplayText.Set(activityContext, displayText);
            }
            catch (Exception ex)
            {
                localContext.Trace("Exception: " + ex.Message);
                DisplayText.Set(activityContext, "Failed to generate 'Display Text'");
            }

            localContext.Trace($"GetDisplayText finished.");
        }

        public static string ExecuteCodeActivity(string transport, string city, string line, string linedesignation, string directionOfText, string tour, string contractor)
        {
            string displayText = string.Empty;

            string sLine = line;
            string sLineDesignation = linedesignation;
            if (directionOfText != null && !string.IsNullOrEmpty(directionOfText))
            {
                sLine = line + " (" + directionOfText + ")";
                sLineDesignation = linedesignation + " (" + directionOfText + ")";
            }


            if (city != null)
                displayText = SetDisplayTextCitybus(transport, city, sLineDesignation, tour, contractor);
            else if (transport.ToUpper() == "REGIONBUS" || transport == "SkåneExpressen")
                displayText = SetDisplayTextRegionbus(transport, sLine, tour, contractor);
            else
                displayText = SetDisplayTextTrain(transport, sLine, tour, contractor);

            return displayText;
        }

        public static string SetDisplayTextTrain(string transport, string line, string tour, string contractor)
        {
            return "Trafikslag: " + transport + " Linje: " + line + " " + tour + " Entreprenör: " + contractor;
        }
        public static string SetDisplayTextRegionbus(string transport, string line, string tour, string contractor)
        {
            string trafik = "Trafikslag: ";

            int lineNumber = int.Parse(line);
            if (lineNumber > 400 && lineNumber < 430)
                trafik += "SkåneExpressen";
            else
                trafik += transport;

            return trafik + " Linje: " + line + " " + tour + " Entreprenör: " + contractor;
        }
        public static string SetDisplayTextCitybus(string transport, string city, string linedesignation, string tour, string contractor)
        {
            return "Trafikslag: " + transport + " Stad: " + city + " Linje: " + linedesignation + " " + tour + " Entreprenör: " + contractor;
        }
        public static string GetTour(string inputFormat, string tour, string start, string stop, DateTime dStartPlanned, DateTime dStartActual, DateTime dArrivalPlanned, DateTime dArrivalActual)
        {
            DateTime minDateLocal = DateTime.MinValue;

            string startplannedtime = "X";
            if (dStartPlanned != null && dStartPlanned != minDateLocal)
                startplannedtime = dStartPlanned.ToString(inputFormat);

            string arrivalplannedtime = "X";
            if (dArrivalPlanned != null && dArrivalPlanned != minDateLocal)
                arrivalplannedtime = dArrivalPlanned.ToString(inputFormat);

            string startactualtime = "X";
            if (dStartActual != null && dStartActual != minDateLocal)
                startactualtime = dStartActual.ToString(inputFormat);

            string arrivalactualtime = "X";
            if (dArrivalActual != null && dArrivalActual != minDateLocal)
                arrivalactualtime = dArrivalActual.ToString(inputFormat);

            var actualStartTimes = " [X] ";
            if (startactualtime != "X" && startplannedtime != "X")
            {
                var diferenceMinuts = MinuteDiff(dStartActual, dStartPlanned);
                actualStartTimes = " [" + startactualtime + " (" + diferenceMinuts + ")] ";
            }

            var actualArrivalTimes = " [X] ";
            if (arrivalactualtime != "X" && arrivalplannedtime != "X")
            {
                var diferenceMinuts = MinuteDiff(dArrivalActual, dArrivalPlanned);
                actualArrivalTimes = " [" + arrivalactualtime + " (" + diferenceMinuts + ")] ";
            }

            return "Tur: [" + tour + "] " + startplannedtime + actualStartTimes + start + " - " + arrivalplannedtime + actualArrivalTimes + stop;
        }
        public static string MinuteDiff(DateTime realTime, DateTime plannedDate)
        {
            var plannedMinutes = (int)Math.Abs(DateTime.MinValue.Subtract(plannedDate).TotalMinutes);
            var realtimeMinutes = (int)Math.Abs(DateTime.MinValue.Subtract(realTime).TotalMinutes);

            return realtimeMinutes - plannedMinutes >= 0 ? "+"+(realtimeMinutes - plannedMinutes) : ""+(realtimeMinutes - plannedMinutes);
        }

        public static string GetDifferenceBetweenDates(DateTime actualDate, DateTime plannedDate)
        {
            double totalMinuts = ((double)actualDate.Subtract(plannedDate).TotalMinutes);

            if (totalMinuts == 0)
                return "0";
            else
                return totalMinuts.ToString("+0;-#");
        }
    }
}
