﻿using System;
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

namespace Skanetrafiken.Crm
{
    public class PlaceOrder : CodeActivity
    {
        [Input("CardNumber")]
        [RequiredArgument()]
        public InArgument<string> CardNumber { get; set; }

        [Output("PlaceOrderResponse")]
        public OutArgument<string> PlaceOrderResponse { get; set; }

        [Output("CardNumberResp")]
        public OutArgument<string> CardNumberResp { get; set; }

        [Output("IsClosed")]
        public OutArgument<bool> IsClosed { get; set; }

        [Output("Amount")]
        public OutArgument<decimal> Amount { get; set; }

        [Output("ClosedReason")]
        public OutArgument<string> ClosedReason { get; set; }

        [Output("IsReserved")]
        public OutArgument<bool> IsReserved { get; set; }

        [Output("IsExpired")]
        public OutArgument<bool> IsExpired { get; set; }

        [Output("LastTransactionDate")]
        public OutArgument<DateTime> LastTransactionDate { get; set; }

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

                localContext.Trace($"PlaceOrder started.");


                //GET VALUE(S)
                localContext.Trace($"PlaceOrder. Get CardNumber");
                string cardNumber = CardNumber.Get(activityContext);

                ValueCodeHandler.GetCardProperties getCardProperties = ExecuteCodeActivity(localContext, cardNumber);

                PlaceOrderResponse.Set(activityContext, "");

                CardNumberResp.Set(activityContext, "");
                IsClosed.Set(activityContext, false);
                Amount.Set(activityContext, 0);
                ClosedReason.Set(activityContext, "");
                IsReserved.Set(activityContext, false);
                IsExpired.Set(activityContext, false);
                LastTransactionDate.Set(activityContext, DateTime.MinValue);

                if (getCardProperties != null)
                {
                    PlaceOrderResponse.Set(activityContext, "200 - Success");

                    if (getCardProperties != null)
                    {
                        if (!string.IsNullOrWhiteSpace(getCardProperties.CardNumber))
                        {
                            CardNumberResp.Set(activityContext, getCardProperties.CardNumber);
                        }

                        if (getCardProperties.IsClosed != null)
                        {
                            IsClosed.Set(activityContext, getCardProperties.IsClosed);
                        }

                        if (getCardProperties.Amount != null)
                        {
                            Amount.Set(activityContext, getCardProperties.Amount);
                        }

                        if (!string.IsNullOrWhiteSpace(getCardProperties.ClosedReason))
                        {
                            ClosedReason.Set(activityContext, getCardProperties.ClosedReason);
                        }

                        if (getCardProperties.IsReserved != null)
                        {
                            IsReserved.Set(activityContext, getCardProperties.IsReserved);
                        }
                        if (getCardProperties.IsExpired != null)
                        {
                            IsExpired.Set(activityContext, getCardProperties.IsExpired);
                        }
                        if (getCardProperties.LastTransactionDate != null)
                        {
                            LastTransactionDate.Set(activityContext, getCardProperties.LastTransactionDate);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                PlaceOrderResponse.Set(activityContext, $"Kunde inte spärra kort för att få värdekod. Vänligen försök igen eller kontakta kundtjänst. StripError (PlaceOrder): { ex.Message }");
            }
        }

        public static ValueCodeHandler.GetCardProperties ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            localContext.Trace($"(ExecuteCodeActivity) started.");

            ValueCodeHandler.GetCardProperties getCardProperties = null;

            try
            {
                if (string.IsNullOrWhiteSpace(cardNumber))
                    throw new Exception("CardNumber is null.");
                
                getCardProperties = TravelCardEntity.HandlePlaceOrder(localContext, cardNumber);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"{ex.Message}");
            }

            return getCardProperties;
        }
    }
}
