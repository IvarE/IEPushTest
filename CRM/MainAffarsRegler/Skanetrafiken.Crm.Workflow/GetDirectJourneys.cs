﻿using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using Skanetrafiken.Crm.Entities;
using System.ServiceModel;
using System.Data;
using System.Web.Script.Serialization;

namespace Skanetrafiken.Crm
{
    public class GetDirectJourneys : CodeActivity
    {

        [Input("FromStopAreaGid")]
        [RequiredArgument()]
        public InArgument<string> FromStopAreaGid { get; set; }

        [Input("ToStopAreaGid")]
        [RequiredArgument()]
        public InArgument<string> ToStopAreaGid { get; set; }

        [Input("TripDateTime")]
        [RequiredArgument()]
        public InArgument<string> TripDateTime { get; set; }

        [Input("ForLineGids")]
        [RequiredArgument()]
        public InArgument<string> ForLineGids { get; set; }

        [Input("TransportType")]
        [RequiredArgument()]
        public InArgument<string> TransportType { get; set; }

        [Output("DirectJourneysResponse")]
        public OutArgument<string> DirectJourneysResponse { get; set; }

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
            throw new NotImplementedException("The method 'Skanetrafiken.Crm.Workflow.GetDirectJourneys' is no longer implemented.");

            // GENERATE CONTEXT
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
            localContext.Trace($"GetDirectJourneys started.");

            // GET VALUE(S)
            string fromStopArea = FromStopAreaGid.Get(activityContext);
            string toStopArea = ToStopAreaGid.Get(activityContext);
            string tripDateTime = TripDateTime.Get(activityContext);
            string forLineGids = ForLineGids.Get(activityContext);
            string transportType = TransportType.Get(activityContext);

            // Execute-metod
            try
            {
                string responsetext = ExecuteCodeActivity(localContext, fromStopArea, toStopArea, tripDateTime, forLineGids, transportType);
                DirectJourneysResponse.Set(activityContext, responsetext);
            }
            catch (Exception ex)
            {
                DirectJourneysResponse.Set(activityContext, "An error occurred when contacting TravelInformation webservice: " + ex.Message);
            }

            localContext.Trace($"GetDirectJourneys finished.");

        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string fromStopAreaGid, string toStopAreaGid, string tripDateTime, string forLineGids, string transportType)
        {

            // ONLY ASSIGNED IF TRAIN. REQUIRED FOR REQUEST
            string aot = "";
            string district = "";
            if (transportType == "TRAIN")
            {
                aot = "AOT";
                district = "DISTRICT";
            }

            // MAKE SOAP REQUEST
            string soapmessage =
                "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='http://www.skanetrafiken.com/DK/INTSTDK001/GetDirectJourneyBetweenStopsRequest/20141023'>" +
                "<soapenv:Header/>" +
                    "<soapenv:Body>" +
                        "<ns:GetDirectJourneysBetweenStops>" +
                            "<fromStopAreaGid>" + fromStopAreaGid + "</fromStopAreaGid>" +
                            "<toStopAreaGid>" + toStopAreaGid + "</toStopAreaGid>" +
                            "<forTimeWindowStartDateTime>" + tripDateTime + "</forTimeWindowStartDateTime>" +
                            "<forTimeWindowDuration>" + 120 + "</forTimeWindowDuration>" +
                            "<withDepartureMaxCount>" + 9999 + "</withDepartureMaxCount>" +
                            "<forLineGids>" + forLineGids + "</forLineGids>" +
                            "<forProducts>" + aot + "</forProducts>" +
                            "<purposeOfLineGroupingCode>" + district + "</purposeOfLineGroupingCode>" +
                        "</ns:GetDirectJourneysBetweenStops>" +
                    "</soapenv:Body>" +
                "</soapenv:Envelope>";

            string soapResponse = "";
            string bizTalkUrl = "";

            //TRY GET SERVICE URL
            try
            {
                //TODO
                bizTalkUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_BizTalkGetDirectJourneysService);
            }
            catch (Exception ex)
            {
                localContext.Trace($"An error occurred when retreiving BizTalk URL: {ex.Message}");
                throw new Exception($"An error occurred when retreiving BizTalk URL: {ex.Message}", ex);
            }

            // REMOVE SUFFIX OF URL
            string[] parts = bizTalkUrl.Split('/');
            string soapActionAddress = "";
            for (int x = 0; x < parts.Length - 1; x++)
            {
                soapActionAddress += parts[x];
                soapActionAddress += "/";
            }

            //TRY SEND REQUEST
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                    client.Encoding = Encoding.UTF8;
                    //client.Credentials = new System.Net.NetworkCredential("crmadmin", "__", "d1");
                    soapResponse = client.UploadString("" + bizTalkUrl + "", soapmessage);
                }
            }
            catch (Exception ex)
            {
                localContext.Trace($"An error occurred when contacting BizTalk service: {ex.Message}");
                throw new Exception($"An error occurred when contacting BizTalk service: {ex.Message}", ex);
            }

            return soapResponse;

        }
    }
}