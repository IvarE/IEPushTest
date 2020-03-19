using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using System.Net;
using System.IO;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.ValueCodes;
using Skanetrafiken.Crm.Entities;
using static Skanetrafiken.Crm.Entities.TravelCardEntity;

namespace Skanetrafiken.Crm
{
    public class ParseCardDetailsFromBiztalk : CodeActivity
    {
        [Input("BiztalkResponse")]
        [RequiredArgument()]
        public InArgument<string> BiztalkResponse { get; set; }

        [Output("CardNumberField")]
        public OutArgument<string> CardNumberField { get; set; }

        [Output("CardIssuerField")]
        public OutArgument<string> CardIssuerField { get; set; }

        [Output("CardHotlistedField")]
        public OutArgument<bool> CardHotlistedField { get; set; }

        [Output("CardTypePeriodField")]
        public OutArgument<int> CardTypePeriodField { get; set; }

        [Output("CardTypeValueField")]
        public OutArgument<int> CardTypeValueField { get; set; }

        [Output("CardValueProductTypeField")]
        public OutArgument<string> CardValueProductTypeField { get; set; }

        [Output("CardCategoryField")]
        public OutArgument<string> CardCategoryField { get; set; }

        [Output("BalanceField")]
        public OutArgument<decimal> BalanceField { get; set; }

        [Output("CurrencyField")]
        public OutArgument<string> CurrencyField { get; set; }

        [Output("OutstandingDirectedAutoloadField")]
        public OutArgument<bool> OutstandingDirectedAutoloadField { get; set; }

        [Output("OutstandingEnableThresholdAutoloadField")]
        public OutArgument<bool> OutstandingEnableThresholdAutoloadField { get; set; }

        [Output("PurseHotlistedField")]
        public OutArgument<bool> PurseHotlistedField { get; set; }

        [Output("PeriodCardCategoryField")]
        public OutArgument<string> PeriodCardCategoryField { get; set; }

        [Output("ProductTypeField")]
        public OutArgument<string> ProductTypeField { get; set; }

        [Output("PeriodStartField")]
        public OutArgument<DateTime> PeriodStartField { get; set; }

        [Output("PeriodEndField")]
        public OutArgument<DateTime> PeriodEndField { get; set; }

        [Output("WaitingPeriodsField")]
        public OutArgument<string> WaitingPeriodsField { get; set; }

        [Output("ZoneListIDField")]
        public OutArgument<string> ZoneListIDField { get; set; }

        [Output("PricePaidField")]
        public OutArgument<decimal> PricePaidField { get; set; }

        [Output("ContractSerialNumberField")]
        public OutArgument<string> ContractSerialNumberField { get; set; }

        [Output("PeriodCurrencyField")]
        public OutArgument<string> PeriodCurrencyField { get; set; }

        [Output("PeriodHotlistedField")]
        public OutArgument<bool> PeriodHotlistedField { get; set; }

        [Output("PeriodOutstandingDirectedAutoloadField")]
        public OutArgument<bool> PeriodOutstandingDirectedAutoloadField { get; set; }

        [Output("PeriodOutstandingEnableThresholdAutoload")]
        public OutArgument<bool> PeriodOutstandingEnableThresholdAutoload { get; set; }

        [Output("RequestParseCardDetailsResult")]
        public OutArgument<string> RequestParseCardDetailsResult { get; set; }

        [Output("ZonesListField")]
        public OutArgument<string> PeriodZonesField { get; set; }

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
            //TRY EXECUTE
            try
            {
                //GENERATE CONTEXT
                Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);

                localContext.Trace($"ParseCardDetailsFromBiztalk started.");


                //GET VALUE(S)
                localContext.Trace($"ParseCardDetailsFromBiztalk. Get biztalkResponse");

                string biztalkResponse = BiztalkResponse.Get(activityContext);

                CardDetailsEnvelope.Envelope envelope = ExecuteCodeActivity(localContext, biztalkResponse);


                CardHotlistedField.Set(activityContext, false);
                CardNumberField.Set(activityContext, "");
                CardCategoryField.Set(activityContext, "");
                CardIssuerField.Set(activityContext, "");
                CardTypePeriodField.Set(activityContext, -1);
                CardTypeValueField.Set(activityContext, -1);
                CardValueProductTypeField.Set(activityContext, "");
                BalanceField.Set(activityContext, -1m);
                CurrencyField.Set(activityContext, "");
                OutstandingDirectedAutoloadField.Set(activityContext, false);
                OutstandingEnableThresholdAutoloadField.Set(activityContext, false);
                PurseHotlistedField.Set(activityContext, false);
                ProductTypeField.Set(activityContext, "");
                PeriodStartField.Set(activityContext, DateTime.MinValue);
                PeriodEndField.Set(activityContext, DateTime.MinValue);
                WaitingPeriodsField.Set(activityContext, "");
                ZoneListIDField.Set(activityContext, "");
                PricePaidField.Set(activityContext, -1M);
                ContractSerialNumberField.Set(activityContext, "");
                PeriodCardCategoryField.Set(activityContext, "");
                PeriodCurrencyField.Set(activityContext, "");
                PeriodHotlistedField.Set(activityContext, false);
                PeriodOutstandingDirectedAutoloadField.Set(activityContext, false);
                PeriodOutstandingEnableThresholdAutoload.Set(activityContext, false);
                PeriodZonesField.Set(activityContext, "");


                if (envelope != null &&
                    envelope.Body != null &&
                    envelope.Body.GetCardDetails2Response != null &&
                    envelope.Body.GetCardDetails2Response.GetCardDetails2Result != null &&
                    envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2 != null
                    )
                {


                    // CardInformation
                    if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation != null)
                    {

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardHotlisted != null)
                            CardHotlistedField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardHotlisted);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardNumber != null)
                            CardNumberField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardNumber.ToString());

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardIssuer != null)
                            CardIssuerField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardIssuer);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardTypePeriod != null)
                            CardTypePeriodField.Set(activityContext, (int)envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardTypePeriod);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardTypeValue != null)
                            CardTypeValueField.Set(activityContext, (int)envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardTypeValue);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardValueProductType != null)
                            CardValueProductTypeField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation.CardValueProductType);
                    }

                    // PurseDetails
                    if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails != null)
                    {

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.Balance != null)
                            BalanceField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.Balance);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.Currency != null)
                            CurrencyField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.Currency);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.OutstandingDirectedAutoload != null)
                            OutstandingDirectedAutoloadField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.OutstandingDirectedAutoload);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.OutstandingEnableThresholdAutoload != null)
                            OutstandingEnableThresholdAutoloadField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.OutstandingEnableThresholdAutoload);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.Hotlisted != null)
                            PurseHotlistedField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.Hotlisted);
                    }

                    // PeriodDetails
                    if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails != null)
                    {

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.ProductType != null)
                            ProductTypeField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.ProductType);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.PeriodStart != null)
                            PeriodStartField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.PeriodStart);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.PeriodEnd != null)
                            PeriodEndField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.PeriodEnd);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.WaitingPeriods != null)
                            WaitingPeriodsField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.WaitingPeriods.ToString());

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.ZoneListID != null)
                            ZoneListIDField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.ZoneListID.ToString());

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.PricePaid != null)
                            PricePaidField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.PricePaid);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.ContractSerialNumber != null)
                            ContractSerialNumberField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.ContractSerialNumber.ToString());

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.CardCategory != null)
                            PeriodCardCategoryField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.CardCategory.ToString());

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.Currency != null)
                            PeriodCurrencyField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.Currency);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.Hotlisted != null)
                            PeriodHotlistedField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.Hotlisted);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.OutstandingDirectedAutoload != null)
                            PeriodOutstandingDirectedAutoloadField.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.OutstandingDirectedAutoload);

                        if (envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.OutstandingEnableThresholdAutoload != null)
                            PeriodOutstandingEnableThresholdAutoload.Set(activityContext, envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails.OutstandingEnableThresholdAutoload);
                    }

                    if(envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.ZoneLists != null)
                    {
                        if(envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.ZoneLists.Length > 0)
                        {
                            var zones = string.Empty;
                            foreach (var zone in envelope.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.ZoneLists)
                            {
                                zones += zone.Zone + ";";
                            }
                            PeriodZonesField.Set(activityContext, zones);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //RequestParseCardDetailsResult.Set(activityContext, $"Kunde inte hämta kortuppgifter. Vänligen försök igen eller kontakta kundtjänst. StripError (ParseCardDetailsFromBiztalk): { ex.Message}");
                throw new InvalidPluginExecutionException($"Kunde inte hämta kortuppgifter. Vänligen försök igen eller kontakta kundtjänst. StripError (ParseCardDetailsFromBiztalk): { ex.Message}");
            }
        }

        public static CardDetailsEnvelope.Envelope ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string biztalkResponse)
        {
            if (localContext == null)
                throw new Exception("localCOntext is null");
            if (localContext.TracingService == null)
                throw new Exception("TracingService is null");

            localContext.TracingService.Trace($"(ExecuteCodeActivity) started.");

            CardDetailsEnvelope.Envelope envelope = null;

            try
            {
                if (string.IsNullOrWhiteSpace(biztalkResponse))
                    throw new Exception("resp is null.");

                envelope = TravelCardEntity.ParseCardDetails(localContext, biztalkResponse);
                
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"{ex.Message}");
            }

            return envelope;
        }
    }
}
