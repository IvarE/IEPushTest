using Common.Logging;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Quartz;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Schema.Generated;
using System;
using System.Activities.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Services.Description;
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
            Entity taskLogUpdate = null;
            try
            {
                localContext = TicketPurchaseHelper.GenerateLocalContext();
                 
                _log.Info($"Fetching TicketPurchasesPerCustomerData");

                DateTime _startTimeBegin = DateTime.Now;

                Entity taskLog = CreateTicketInfoNewTaskLog(localContext);// CreateTicketInfoTaskLog(localContext);
                taskLogUpdate = new Entity("task", taskLog.Id);
                taskLogUpdate["description"] = taskLog["description"];

                LogWithTask(localContext, "\r\nFetchABatchOfActiveQueuePosts....", taskLogUpdate);

                DateTime _startTimeX = DateTime.Now;
                //GET ALL customer data
                IList<TicketPurchasesPerCustomerDataEntity> ticketInfoData = FetchABatchOfActiveQueuePosts(localContext);
                 ticketInfoData = ticketInfoData.OrderBy(t => t.ed_MKLid).ToList(); //IMPORTANT to sort by MKLId, one contact could have multipe tickets
                  
                _log.Info($"Fetching all Contact");

                TimeSpan _diff = DateTime.Now - _startTimeX;
                double durationX = Math.Round(_diff.TotalMinutes, 0, MidpointRounding.AwayFromZero);                      
                LogWithTask(localContext, $"\r\n\tDuration: {durationX}- min | total: {ticketInfoData.Count}\r\nFetchAllContacts....", taskLogUpdate);

                _startTimeX = DateTime.Now;
                IList<ContactEntity> existContacts = FetchAllContacts(localContext);
                 
                _log.Info($"Updating ticket info with ticket purchase data, row(s) found: {ticketInfoData.Count}, contacts: {existContacts.Count}");

                _diff = DateTime.Now - _startTimeX;
                durationX = Math.Round(_diff.TotalMinutes, 0, MidpointRounding.AwayFromZero);
                LogWithTask(localContext, $"\r\n\tDuration: {durationX}- min | total: {existContacts.Count}\r\nUpdateTicketInfoWithTicketPurchaseData....", taskLogUpdate);

                var (nUpdate, nNew, nContactUpdate, errorLog) = UpdateTicketInfoWithTicketPurchaseData_new(localContext, ticketInfoData,  existContacts, taskLogUpdate);
                
                _log.Info($"TicketInfo Update: {nUpdate}, New: {nNew}, Contact Kundresan update: {nContactUpdate}");
                _log.Info($"PopulateTPJob Done"+ (errorLog != "" ? ", with error: "+errorLog:""));

                //Update Task
                _diff = DateTime.Now - _startTimeBegin;
                durationX = Math.Round(_diff.TotalMinutes, 0, MidpointRounding.AwayFromZero);
                taskLogUpdate["scheduledend"] = DateTime.Now;
                taskLogUpdate["scheduleddurationminutes"] = (int)durationX;
                taskLogUpdate["actualdurationminutes"] = (int)durationX;
                taskLogUpdate["statecode"] =new OptionSetValue(1); //completed
                taskLogUpdate["statuscode"] = new OptionSetValue(5);//completed
                LogWithTask(localContext, $"\r\n\tTicketInfo Update: {nUpdate}, New: {nNew}, Contact Kundresan update: {nContactUpdate}" +
                                                $"\r\n\r\nDone: {durationX}- min | {DateTime.Now.ToString("u").Replace("Z", "")}", taskLogUpdate);                 
           }
            catch (Exception e)
            {
                _log.Error($"Exception caught in ExecuteJob():\n{e.Message}\n\n{e}");
                if (taskLogUpdate != null && localContext != null)
                {
                    try
                    {
                        LogWithTask(localContext, $"\r\nException caught in ExecuteJob():\r\n{e.Message}", taskLogUpdate);
                    }
                    catch (Exception ex)
                    {
                        _log.Error($"Exception caught in Trying to update Task Log():\n{ex.Message}\n\n{ex}");
                    }
                }
            }
        }
        private Entity CreateTicketInfoNewTaskLog(Plugin.LocalPluginContext localContext)
        {
            Entity task = new Entity("task");
            task["subject"] = "CRM Service Job: TicketPurchaseServiceJob Log " + DateTime.Now.ToString("u").Replace("Z", "");
            task["scheduledstart"] = DateTime.Now;
            task["description"] = "TicketPurchaseService running... " + DateTime.Now.ToString("u").Replace("Z", "");
            task.Id = XrmHelper.Create(localContext, task);

            return task;

        }
        private Entity CreateTicketInfoTaskLog(Plugin.LocalPluginContext localContext)
        {
            Entity task = null;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' count='1' distinct='true'>
                                          <entity name='task'>
                                            <attribute name='activityid' />                                        
                                            <attribute name='description' />                                                                                                                          
                                            <filter type='and'>
                                              <condition attribute='subject' operator='like' value='CRM Service Job: TicketPurchaseServiceJob Log%' /> 
                                            </filter>
                                            
                                          </entity>
                                        </fetch>";
            EntityCollection results = localContext.OrganizationService.RetrieveMultiple(new FetchExpression(fetchXml));
            if (results.Entities.Count > 0)
            {
                task = new Entity("task");
                task.Id = results.Entities[0].Id;
                task["scheduledstart"] = DateTime.Now;
                task["subject"] = "CRM Service Job: TicketPurchaseServiceJob Log " + DateTime.Now.ToString("u").Replace("Z", "");
                task["description"] = "TicketPurchaseService running... " + DateTime.Now.ToString("u").Replace("Z", "");
                XrmHelper.Update(localContext, task);
            }
            else
            {
                task = new Entity("task");
                task["subject"] = "CRM Service Job: TicketPurchaseServiceJob Log " + DateTime.Now.ToString("u").Replace("Z", "");
                task["scheduledstart"] = DateTime.Now;
                task["description"] = "TicketPurchaseService running... " + DateTime.Now.ToString("u").Replace("Z", "");
                task.Id = XrmHelper.Create(localContext, task);
            }
            return task;

        }
        public (int nUpdate, int nNew, int nContactUpdate, string errorLog) UpdateTicketInfoWithTicketPurchaseData_new(Plugin.LocalPluginContext localContext, IList<TicketPurchasesPerCustomerDataEntity> ticketInfoDataRecords, 
                                                                                                             IList<ContactEntity> existContacts, Entity taskLogUpdate)
        { 
            EntityCollection contactAll = new EntityCollection();
            IList<TicketInfoEntity> allTicketInfoCollection = new List<TicketInfoEntity>();
            int counterTotal = 0, counterUpdate = 0, counterNew = 0, counterContactKundresan = 0;
            string errorLog = "";
            foreach (TicketPurchasesPerCustomerDataEntity tppcData in ticketInfoDataRecords)
            {
                counterTotal++;
                TicketInfoEntity existTicket = null;
                //always has a MKL ID
                if (tppcData.ed_MKLid != null)
                {
                    existTicket = FetchExistXTicketInfo(localContext, tppcData.ed_MKLid, tppcData.ed_OfferName);
                }
                // Existing Ticket Info found
                if (existTicket != null)
                    counterUpdate += UpdateTicketInfo(localContext, existTicket, existContacts, tppcData, allTicketInfoCollection);
                else // No ticket info found - Creating new ticket info
                {
                    try
                    {
                        counterNew += CreateTicketInfo(localContext, existContacts, tppcData, allTicketInfoCollection);
                    }
                    catch (Exception error)
                    {
                        errorLog += $"CreateTicketInfo Error: " + error.Message + " mklid: " + tppcData.ed_MKLid + " offert: " + tppcData.ed_OfferName + "\r\n";
                    }
                }
               
                //Collect all unique Contact to run the CalculateClassificationOnContact
                if (tppcData.ed_MKLid != null)
                {
                    var eContact = existContacts.FirstOrDefault(c => c.ed_MklId == tppcData.ed_MKLid);
                    if (eContact != null)
                    {
                        var checkExist = contactAll.Entities.Where(c => c.Id == eContact.Id).ToArray();
                        if (checkExist.Length == 0) contactAll.Entities.Add(eContact);

                        if (contactAll.Entities.Count > 2) //[0, 1, 2, 3], not calculate for FIRST and LAST index
                            counterContactKundresan+= CalculateClassificationOnContact(localContext, contactAll.Entities[contactAll.Entities.Count-2], allTicketInfoCollection);
                    }
                }

                if(counterTotal % 20000 == 0) 
                    LogWithTask(localContext, $"\r\n\t({counterTotal}/{ticketInfoDataRecords.Count}),U:{counterUpdate}, C:{counterNew}, Con:{counterContactKundresan} {DateTime.Now.ToString("u").Replace("Z", "").Split(' ')[1]}", taskLogUpdate);                                    
            }

            //Calculate for the FIRST and the LAST one
            if (contactAll.Entities.Count > 0) {
                counterContactKundresan += CalculateClassificationOnContact(localContext, contactAll.Entities[0], allTicketInfoCollection);
                if(contactAll.Entities.Count > 1)
                    counterContactKundresan += CalculateClassificationOnContact(localContext, contactAll.Entities[contactAll.Entities.Count - 1], allTicketInfoCollection);
            }

            return (counterUpdate, counterNew, counterContactKundresan, errorLog);
        }
        private void LogWithTask(Plugin.LocalPluginContext localContext, string text, Entity taskEntity)
        { 
            if ((taskEntity["description"].ToString().Length + text.Length) > 2000) { 
                taskEntity["description"] = taskEntity["description"].ToString().Substring(0, (taskEntity["description"].ToString().Length-text.Length)-10)+"...";
            }
           
            taskEntity["description"] += text;

            XrmHelper.Update(localContext, taskEntity);
        }
        private int UpdateTicketInfo(Plugin.LocalPluginContext localContext, TicketInfoEntity existTicket, IList<ContactEntity> existContacts, TicketPurchasesPerCustomerDataEntity tppcData, IList<TicketInfoEntity> allTicketInfoCollection)
        { 
            Entity ticketInfoUpdate = new Entity(existTicket.LogicalName);
            //ticketInfo need to update this object to refresh it in the all collection?
            Generated.st_singaporetickettype offerName = setOfferNameOptionset(localContext, tppcData);
            if (existTicket.ed_OfferName != offerName) //not exist
            {
                ticketInfoUpdate["ed_offername"] = offerName;
                ticketInfoUpdate["ed_name"] = tppcData.ed_ContactNumber + "_" + tppcData.ed_OfferName;
                ticketInfoUpdate["ed_offer_name"] = tppcData.ed_OfferName;

                existTicket["ed_offername"] = ticketInfoUpdate["ed_offername"];
                existTicket["ed_name"] = ticketInfoUpdate["ed_name"];
                existTicket["ed_offer_name"] = ticketInfoUpdate["ed_offer_name"];
            }
            decimal existTotal = existTicket.Contains("ed_totalsum") ? ((Money)existTicket["ed_totalsum"]).Value : 0;
            decimal newTotal = tppcData.ed_TotalSumTicketOffer != null ? tppcData.ed_TotalSumTicketOffer.Value : 0;
            int existTickets = existTicket.Contains("ed_numberoftickets") ? (int)existTicket["ed_numberoftickets"] : 0;
            int newTickets = tppcData.ed_NumberOfTickets != null ? (int)tppcData.ed_NumberOfTickets : 0;

            if (existTotal != newTotal)
            {
                ticketInfoUpdate["ed_totalsum"] = newTotal != 0 ? new Money(newTotal) : null;
                existTicket["ed_totalsum"] = ticketInfoUpdate["ed_totalsum"];
            }
            if (existTickets != newTickets)
            {
                if (newTickets != 0) ticketInfoUpdate["ed_numberoftickets"] = newTickets;
                else ticketInfoUpdate["ed_numberoftickets"] = null;

                existTicket["ed_numberoftickets"] = ticketInfoUpdate["ed_numberoftickets"];
            }

            if (existTicket.Contains("con.ed_mklid")) //Contact exist in TicketInfo check Contact diff
            {
                string existMKLId = ((AliasedValue)existTicket["con.ed_mklid"]).Value.ToString();
                if (tppcData.ed_MKLid != null && existMKLId != tppcData.ed_MKLid)
                {
                    var eContact = existContacts.FirstOrDefault(c => c.ed_MklId == tppcData.ed_MKLid);
                    if (eContact != null)
                    { 
                        ticketInfoUpdate["ed_contact"] = eContact.ToEntityReference();
                        existTicket["ed_contact"] = eContact.ToEntityReference();
                    }
                }
            }
            else
            {
                if (tppcData.ed_MKLid != null)
                {
                    var eContact = existContacts.FirstOrDefault(c => c.ed_MklId == tppcData.ed_MKLid);
                    if (eContact != null )
                    { 
                        ticketInfoUpdate["ed_contact"] = eContact.ToEntityReference();
                        existTicket["ed_contact"] = eContact.ToEntityReference();
                    }
                }
            }

            if (ticketInfoUpdate.Attributes.Count >0)
            {
                ticketInfoUpdate.Id = existTicket.Id;
                XrmHelper.Update(localContext, ticketInfoUpdate); 
            }

            allTicketInfoCollection.Add(existTicket);

            return ticketInfoUpdate.Attributes.Count > 0? 1:0;
        }
        private int CreateTicketInfo(Plugin.LocalPluginContext localContext, IList<ContactEntity> existContacts, TicketPurchasesPerCustomerDataEntity tppcData, IList<TicketInfoEntity> allTicketInfoCollection)
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
            if (tppcData.ed_MKLid != null)
            {
                var eContact = existContacts.FirstOrDefault(c=> c.ed_MklId == tppcData.ed_MKLid);
                if (eContact != null) createTicketInfo.ed_Contact = eContact.ToEntityReference();              
            }

            createTicketInfo.Id = XrmHelper.Create(localContext, createTicketInfo);
            allTicketInfoCollection.Add(createTicketInfo);
            
            return 1;
        }

        private int CalculateClassificationOnContact(Plugin.LocalPluginContext localContext, Entity eContact, IList<TicketInfoEntity> allTicketInfoCollection)
        {
            //var everyTicketInfo = TicketInfoEntity.GetEveryTicketInfoQuery(localContext, eContact.Id);
            var everyTicketInfo = allTicketInfoCollection.Where(t => t.ed_MklId == eContact["ed_mklid"].ToString());

            //var ticketListWithAmount = new List<Tuple<int, int>>();
            var trettioDagarsbiljett = 0; //206290000
            var trettioDagarsbiljettMetro = 0; // 	899310002
            var tioTrettioDagarsbiljett = 0; // 899310001
            var tioTrettioDagarsbiljettDKMetro = 0; // 	899310003
            var enkelbiljett = 0; // 	206290002
            var tioEnklaFem = 0; //  	206290003 //motsvarar 10 enkelbiljetter
            var dygnsbiljett = 0; // 	206290001


            var isSallanresenar = false;
            var isVaxlare = false;
            var isPendlare = false;
            var isForloradKund = false;

            foreach (TicketInfoEntity ticket in everyTicketInfo)
            {
                var ticketOfferNameValue = Convert.ToInt32(ticket.ed_OfferName.Value);

                if (ticketOfferNameValue == 206290000)
                {
                    trettioDagarsbiljett = trettioDagarsbiljett + Convert.ToInt32(ticket.ed_NumberofTickets);
                }
                else if (ticketOfferNameValue == 899310002)
                {
                    trettioDagarsbiljettMetro = trettioDagarsbiljettMetro + Convert.ToInt32(ticket.ed_NumberofTickets);
                }
                else if (ticketOfferNameValue == 899310001)
                {
                    tioTrettioDagarsbiljett = tioTrettioDagarsbiljett + Convert.ToInt32(ticket.ed_NumberofTickets);
                }
                else if (ticketOfferNameValue == 899310003)
                {
                    tioTrettioDagarsbiljettDKMetro = tioTrettioDagarsbiljettDKMetro + Convert.ToInt32(ticket.ed_NumberofTickets);
                }
                else if (ticketOfferNameValue == 206290002)
                {
                    enkelbiljett = enkelbiljett + Convert.ToInt32(ticket.ed_NumberofTickets);
                }
                else if (ticketOfferNameValue == 206290003)
                {
                    tioEnklaFem = tioEnklaFem + Convert.ToInt32(ticket.ed_NumberofTickets);
                }
                else if (ticketOfferNameValue == 206290001)
                {
                    dygnsbiljett = dygnsbiljett + Convert.ToInt32(ticket.ed_NumberofTickets);
                }

            }

            //Sällanresnär
            if ((enkelbiljett > 0 || dygnsbiljett > 0 || tioEnklaFem > 0) && trettioDagarsbiljett == 0 && tioTrettioDagarsbiljett == 0 && trettioDagarsbiljettMetro == 0)
            {
                isSallanresenar = true;
            }

            //Växlare
            var manadsbiljetter = trettioDagarsbiljett + tioTrettioDagarsbiljett + tioTrettioDagarsbiljettDKMetro + trettioDagarsbiljettMetro;
            if (manadsbiljetter != 0 && manadsbiljetter < 7)
            {
                isVaxlare = true;
            }

            //Pendlare
            if (manadsbiljetter > 6)
            {
                isPendlare = true;
            }

            //Förlorad kund
            if (trettioDagarsbiljett == 0 && trettioDagarsbiljettMetro == 0 && tioTrettioDagarsbiljett == 0 && tioTrettioDagarsbiljettDKMetro == 0 && enkelbiljett == 0 && tioEnklaFem == 0 && dygnsbiljett == 0)
            {
                isForloradKund = true;
            }
            Entity updateContact = new Entity("contact");
            updateContact["ed_calculateclassification"] = true;
            if (isForloradKund)
            {
                updateContact["ed_kundresan"] = new OptionSetValue(899310004);
            }
            else if (isPendlare)
            {
                updateContact["ed_kundresan"] = new OptionSetValue(899310003);
            }
            else if (isVaxlare)
            {
                updateContact["ed_kundresan"] = new OptionSetValue(899310002);
            }
            else if (isSallanresenar)
            {
                updateContact["ed_kundresan"] = new OptionSetValue(899310001);
            }
            //899310000 = Ny Kund
            int returnValue = 0; 
            if (eContact.Contains("ed_kundresan")) //if exist Kundresan value, check if changed
            {
                int existValue = ((OptionSetValue)eContact["ed_kundresan"]).Value;
                int newValue = updateContact.Contains("ed_kundresan") ? ((OptionSetValue)updateContact["ed_kundresan"]).Value: -1;
                if(existValue != newValue && newValue != -1)
                {
                    updateContact.Id = eContact.Id;
                    XrmHelper.Update(localContext, updateContact);
                    returnValue = 1;
                }
            }
            else
            {
                if(!updateContact.Contains("ed_kundresan"))
                    updateContact["ed_kundresan"] = new OptionSetValue(899310000);

                updateContact.Id = eContact.Id;
                XrmHelper.Update(localContext, updateContact);
                returnValue = 1;
            }
            return returnValue;
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
            batchQuery.Criteria.AddCondition("st_isduplicate", ConditionOperator.NotEqual, true); //This attribute se by p_CheckAndSetDuplicateRecordWithTicketInfo

            return XrmRetrieveHelper.RetrieveMultiple<TicketPurchasesPerCustomerDataEntity>(localContext, batchQuery);
        }
         
        public IList<ContactEntity> FetchAllContacts(Plugin.LocalPluginContext localContext)
        {
            QueryExpression batchQuery = new QueryExpression
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(ContactEntity.Fields.ed_MklId, ContactEntity.Fields.ed_Kundresan)
            };
            batchQuery.Criteria = new FilterExpression();
            batchQuery.Criteria.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, ContactState.Active.ToString());
            batchQuery.Criteria.AddCondition(ContactEntity.Fields.ed_MklId, ConditionOperator.NotNull);
            
            return XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, batchQuery);
        }

        public TicketInfoEntity FetchExistXTicketInfo(Plugin.LocalPluginContext localContext, string mklId, string offerName)
        {
            QueryExpression batchQuery = new QueryExpression
            {
                TopCount=1,
                EntityName = TicketInfoEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(TicketInfoEntity.Fields.ed_CRMNumber,
                       TicketInfoEntity.Fields.ed_MklId,
                     //  TicketInfoEntity.Fields.ed_name,
                       TicketInfoEntity.Fields.ed_NumberofTickets,
                     //TicketInfoEntity.Fields.ed_Offer_Name,
                       TicketInfoEntity.Fields.ed_OfferName,
                       TicketInfoEntity.Fields.ed_TotalSum)
            };
            batchQuery.Criteria = new FilterExpression();
            batchQuery.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            batchQuery.Criteria.AddCondition(TicketInfoEntity.Fields.ed_MklId, ConditionOperator.Equal, mklId);
            batchQuery.Criteria.AddCondition(TicketInfoEntity.Fields.ed_Offer_Name, ConditionOperator.Equal, offerName);
            var con = batchQuery.AddLink("contact", "ed_contact", "contactid", JoinOperator.LeftOuter);
            con.EntityAlias = "con";
            con.Columns.AddColumn("ed_mklid");

            return XrmRetrieveHelper.RetrieveMultiple<TicketInfoEntity>(localContext, batchQuery).FirstOrDefault();
        }
    }
}
