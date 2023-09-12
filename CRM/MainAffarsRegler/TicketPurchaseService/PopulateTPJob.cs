using Common.Logging;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Quartz;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using Generated = Skanetrafiken.Crm.Schema.Generated;


namespace TicketPurchaseService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class PopulateTPJob : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterTP";
        public const string GroupName = "TP Schedule";
        public const string TriggerDescription = "TP Schedule Trigger";
        public const string JobDescription = "TP Schedule Job";
        public const string TriggerName = "TPTrigger";
        public const string JobName = "TPJob";

        private ILog _log = LogManager.GetLogger(typeof(PopulateTPJob));

        public void Execute(IJobExecutionContext context)
        {
            _log.Debug(string.Format(Properties.Resources.TriggerExecuting, context.Trigger.Description ?? context.Trigger.Key.Name));

            JobDataMap dataMap = context.JobDetail.JobDataMap;

            DateTime modifiedAfter = dataMap.GetDateTime(DataMapModifiedAfter);

            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuting, context.JobDetail.Description ?? context.JobDetail.Key.Name ?? "NULL", modifiedAfter.ToString() ?? "NULL"));

            ExecuteJob();

            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuted, context.JobDetail.Description ?? context.JobDetail.Key.Name, modifiedAfter.ToString()));
        }

        public void ExecuteJob()
        {
            Plugin.LocalPluginContext localContext = null;
            try
            {
                localContext = TicketPurchaseHelper.GenerateLocalContext();

                _log.Info($"Fetching TicketPurchasesPerCustomerData");
                IList<TicketPurchasesPerCustomerDataEntity> ticketInfoData = FetchABatchOfActiveQueuePosts(localContext);

                _log.Info($"Updating ticket info with ticket purchase data");
                UpdateTicketInfoWithTicketPurchaseData(localContext, ticketInfoData);

                _log.Info($"UploadJob Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in ExecuteJob():\n{e.Message}\n\n{e}");

            }
        }

        public void UpdateTicketInfoWithTicketPurchaseData(Plugin.LocalPluginContext localContext, IList<TicketPurchasesPerCustomerDataEntity> ticketInfoDataRecords)
        {

            foreach (TicketPurchasesPerCustomerDataEntity tppcData in ticketInfoDataRecords)
            {

                TicketInfoEntity ticketInfo = null;

                if (tppcData.ed_MKLid != null)
                {
                    ticketInfo = XrmRetrieveHelper.RetrieveFirst<TicketInfoEntity>(localContext, new ColumnSet(
                       TicketInfoEntity.Fields.ed_CRMNumber,
                       TicketInfoEntity.Fields.ed_MklId,
                       TicketInfoEntity.Fields.ed_name,
                       TicketInfoEntity.Fields.ed_NumberofTickets,
                       TicketInfoEntity.Fields.ed_Offer_Name,
                       TicketInfoEntity.Fields.ed_OfferName,
                       TicketInfoEntity.Fields.ed_TotalSum),
                        new FilterExpression
                        {
                            Conditions =
                                   {
                                new ConditionExpression(TicketInfoEntity.Fields.ed_MklId, ConditionOperator.Equal, tppcData.ed_MKLid),
                                new ConditionExpression(TicketInfoEntity.Fields.ed_Offer_Name, ConditionOperator.Equal, tppcData.ed_OfferName)
                                   }
                        });
                }

                if (ticketInfo != null) // Existing Ticket Info found
                {
                    bool updateTicketInfo = false;
                    Money ticketInfoTotalSum = new Money(0);
                    Generated.st_singaporetickettype offerName;

                    if (tppcData.ed_TotalSumTicketOffer != null)
                        ticketInfoTotalSum = new Money(tppcData.ed_TotalSumTicketOffer.Value);

                    if (tppcData.ed_NumberOfTickets != null && tppcData.ed_NumberOfTickets != ticketInfo.ed_NumberofTickets)
                    {
                        ticketInfo.ed_NumberofTickets = tppcData.ed_NumberOfTickets;
                        updateTicketInfo = true;
                    }
                        

                    if (tppcData.ed_TotalSumTicketOffer != null && ticketInfoTotalSum != ticketInfo.ed_TotalSum)
                    {
                        ticketInfo.ed_TotalSum = new Money(tppcData.ed_TotalSumTicketOffer.Value);
                        updateTicketInfo = true;
                    }
                        

                    if (ticketInfo.ed_OfferName == null)
                    {
                        offerName = setOfferNameOptionset(localContext, tppcData);

                        if (offerName != ticketInfo.ed_OfferName)
                        {
                            ticketInfo.ed_OfferName = offerName;
                            updateTicketInfo = true;
                        }
                             
                    }

                    if(updateTicketInfo == true)
                        XrmHelper.Update(localContext, ticketInfo);

                }
                else // No ticket info found - Creating new ticket info
                {
                    TicketInfoEntity createTicketInfo = new TicketInfoEntity
                    {

                        ed_CRMNumber = tppcData.ed_ContactNumber,
                        ed_MklId = tppcData.ed_MKLid,
                        ed_name = tppcData.ed_ContactNumber + "_" + tppcData.ed_OfferName,
                        ed_NumberofTickets = tppcData.ed_NumberOfTickets,
                        ed_Offer_Name = tppcData.ed_OfferName,
                        ed_TotalSum = new Money(tppcData.ed_TotalSumTicketOffer.Value)
                    };

                    createTicketInfo.ed_OfferName = setOfferNameOptionset(localContext, tppcData);

                    XrmHelper.Create(localContext, createTicketInfo);
                }

            }


        }

        private static Generated.st_singaporetickettype setOfferNameOptionset(Plugin.LocalPluginContext localContext, TicketPurchasesPerCustomerDataEntity tppcData)
        {
            return tppcData.ed_OfferName switch
            {
                "30-dagarsbiljett" => Generated.st_singaporetickettype.dagarsbiljett,
                "30 dagar + Metro" => Generated.st_singaporetickettype.dagarPlusMetro,
                "10/30" => Generated.st_singaporetickettype._1030,
                "10/30 DK ink Metro" => Generated.st_singaporetickettype.DKinkMetro,
                "Enkelbiljett" => Generated.st_singaporetickettype.Enkelbiljett,
                "10 enkla 5%" => Generated.st_singaporetickettype.enkla5,
                "Dygnsbiljett" => Generated.st_singaporetickettype.Dygnsbiljett,
                "10 enkla app 5%" => Generated.st_singaporetickettype.enklaapp5,
                "Sommarbiljett" => Generated.st_singaporetickettype.Sommarbiljett,
                _ => (Generated.st_singaporetickettype)(-1)
            };
        }


        public IList<TicketPurchasesPerCustomerDataEntity> FetchABatchOfActiveQueuePosts(Plugin.LocalPluginContext localContext)
        {
            QueryExpression batchQuery = new QueryExpression
            {
                EntityName = TicketPurchasesPerCustomerDataEntity.EntityLogicalName,
                ColumnSet = TicketPurchaseHelper.ticketInfoDataColumnSet
            };

            //// Assign the pageinfo properties to the query expression. // TODO - vet ej om detta behövs
            //batchQuery.PageInfo = new PagingInfo();
            //batchQuery.PageInfo.Count = 5000;
            //batchQuery.PageInfo.PageNumber = 1;

            // The current paging cookie. When retrieving the first page, 
            // pagingCookie should be null.
            batchQuery.PageInfo.PagingCookie = null;

            return XrmRetrieveHelper.RetrieveMultiple<TicketPurchasesPerCustomerDataEntity>(localContext, batchQuery);
        }





    }
}
