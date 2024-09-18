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
using System.Activities.Expressions;

namespace Skanetrafiken.Crm
{
    public class CheckTravelCardExpired : CodeActivity
    { 
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
               
                localContext.Trace($"GetTravelCard started.");

                Entity taskLog = CreateNewTaskLog(localContext.OrganizationService);
                Entity taskLogUpdate = new Entity("task", taskLog.Id);
                taskLogUpdate["description"] = taskLog["description"];
                LogWithTask(localContext.OrganizationService, "\r\nGetTravelCardsOlderThan10Years....", taskLogUpdate);

                EntityCollection allExpiredCards = getTravelCardOlderThan10Years(localContext.OrganizationService);
                
                //localContext.Trace($"GetTravelCard Total."+allExpiredCards.Entities.Count);
                LogWithTask(localContext.OrganizationService, "\r\n\tTotal: "+allExpiredCards.Entities.Count + " - "+ DateTime.Now.ToString("u").Replace("Z", ""), taskLogUpdate);
                
                UpdateTravelCards(localContext.OrganizationService, allExpiredCards, taskLogUpdate);

                
            }
            catch (Exception ex)
            { 
              throw new InvalidPluginExecutionException(ex.Message);
                 
            }
        }
        private void UpdateTravelCards(IOrganizationService service, EntityCollection allExpiredCards, Entity taskLogUpdate)
        {
            DateTime _startTimeBegin = DateTime.Now;
            string msgError = "";
            foreach (Entity entTravelCard in allExpiredCards.Entities)
            {
                try
                {
                    Entity upCard = new Entity("cgi_travelcard", entTravelCard.Id);
                    upCard["st_isexpired"] = true;
                    service.Update(upCard);
                }
                catch (Exception error)
                {
                    msgError += "\r\nError: " + entTravelCard["cgi_travelcardnumber"].ToString() +", error: " + error.Message;
                }
            }
            //Update Task
            TimeSpan _diff = DateTime.Now - _startTimeBegin;
            double durationX = Math.Round(_diff.TotalMinutes, 0, MidpointRounding.AwayFromZero);
            taskLogUpdate["scheduledend"] = DateTime.Now;
            taskLogUpdate["scheduleddurationminutes"] = (int)durationX;
            taskLogUpdate["actualdurationminutes"] = (int)durationX;
            taskLogUpdate["statecode"] = new OptionSetValue(1); //completed
            taskLogUpdate["statuscode"] = new OptionSetValue(5);//completed
            LogWithTask(service, $"\r\n\tUpdateTravelCards done: {durationX} - min | {DateTime.Now.ToString("u").Replace("Z", "")}", taskLogUpdate);
        }
        private Entity CreateNewTaskLog(IOrganizationService service)
        {
            Entity task = new Entity("task");
            task["subject"] = "CRM Service Job: CheckTravelCardExpired Log " + DateTime.Now.ToString("u").Replace("Z", "");
            task["scheduledstart"] = DateTime.Now;
            task["description"] = "CheckTravelCardExpired running... " + DateTime.Now.ToString("u").Replace("Z", "");
            task.Id = service.Create( task);

            return task;

        }
        private void LogWithTask(IOrganizationService service, string text, Entity taskEntity)
        {
            if ((taskEntity["description"].ToString().Length + text.Length) > 2000)
            {
                taskEntity["description"] = taskEntity["description"].ToString().Substring(0, (taskEntity["description"].ToString().Length - text.Length) - 10) + "...";
            }

            taskEntity["description"] += text;

            service.Update(taskEntity);
        }
        private EntityCollection getTravelCardOlderThan10Years(IOrganizationService service)
        {
            string xml =  "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>"
                  +"<entity name='cgi_travelcard'>"
                  +"<attribute name='cgi_travelcardnumber' />"
                  +"<attribute name='cgi_travelcardid' />    "
                  +"<attribute name='st_lasttransactiondate' />    "
                  +"<order attribute='modifiedon' descending='true' /> "
                  +"<filter type='and'>"
                  +"  <condition attribute='statecode' operator='eq' value='0' />"
                  +"  <condition attribute='st_lasttransactiondate' operator='olderthan-x-years' value='10' />"
                  +"  <condition attribute='st_isexpired' operator='ne' value='1' />"
                  + "  <condition attribute='cgi_blocked' operator='ne' value='1' />"
                  + "</filter>"
                  + "</entity>"
                  + "</fetch>";
            return  service.RetrieveMultiple(new FetchExpression(xml));
        }
        
    }
}
