using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CGIXrmHandler;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace CGIXrmCallGuideJob
{
    class BatchJob
    {
        CallGuideBatchHandler callguideBatchHandler;

        static void Main(string[] args)
        {

            try
            {
                BatchJob batchJob = new BatchJob();
                Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
                batchJob.callguideBatchHandler = new CallGuideBatchHandler();
                Console.WriteLine("Title " + ConfigurationManager.AppSettings["JobName"]);
                string activityType = ConfigurationManager.AppSettings["ActivityType"];
                switch (activityType.ToLowerInvariant())
                {
                    case "chat":
                        batchJob.ExecuteChatProcess();
                        break;
                    case "facebook":
                        batchJob.ExecuteFbProcess();
                        break;
                    default:
                        break;
                }

                Console.WriteLine("Process Completed Successfully.");
            }
            catch (Exception Ex)
            {
                CGIXrmLogger.LogToCrm log2Crm = new CGIXrmLogger.LogToCrm();
                log2Crm.Exception(Ex.Message, string.Empty, Ex,"Batch Process " + ConfigurationManager.AppSettings["JobName"]);
            }


        }
        void ExecuteChatProcess()
        {
            try
            {
                List<WorkItem> lstCurrentWorkItem = callguideBatchHandler.GetOpenCrmChatActivity();
                if (lstCurrentWorkItem != null && lstCurrentWorkItem.Count > 0)
                {
                    List<CallGuideRecord> lstCallGuideRecord = callguideBatchHandler.GetCallGuideDBRecord(String.Join(",", lstCurrentWorkItem.Select(e => e.CallguideSessionId)), CallGuideBatchActivity.Chat);

                    if (lstCallGuideRecord != null && lstCallGuideRecord.Count > 0)
                    {

                        
                        List<WorkItem> lstToUpdate = (from jobItem in lstCurrentWorkItem
                                                      join callGuideRecord in lstCallGuideRecord on jobItem.CallguideSessionId equals callGuideRecord.ContactId
                                                      select new WorkItem
                                                      {
                                                          AccountId = jobItem.AccountId,
                                                          ActivityId = jobItem.ActivityId,
                                                          CallGuideInfoId = jobItem.CallGuideInfoId,
                                                          CallguideSessionId = jobItem.CallguideSessionId,
                                                          ErrandTaskType = jobItem.ErrandTaskType,
                                                          Description = jobItem.Description,
                                                          UpdateData = callGuideRecord.Data

                                                      }).ToList();


                        List<WorkItem> lstUpdated = callguideBatchHandler.BulkUpdateChatConversation(lstToUpdate);

                        if (lstUpdated != null && lstUpdated.Count > 0)
                        {
                            List<WorkItem> lstCaseCreated = callguideBatchHandler.BulkCreateCase(lstUpdated, CallGuideBatchActivity.Chat);

                            if (lstCaseCreated != null && lstCaseCreated.Count > 0)
                            {
                                callguideBatchHandler.BulkUpdateRegarding(lstCaseCreated, CallGuideBatchActivity.Chat);
                                callguideBatchHandler.BulkCompleteActivty(lstCaseCreated, CallGuideBatchActivity.Chat);
                                //List<WorkItem> lstCaseCategoryCreated = callguideBatchHandler.BulkCreateCaseCategory(lstUpdated, CallGuideBatchActivity.Chat);

                                //if (lstCaseCategoryCreated != null && lstCaseCategoryCreated.Count > 0)
                                //{
                                //    callguideBatchHandler.BulkUpdateRegarding(lstCaseCategoryCreated, CallGuideBatchActivity.Chat);
                                //    callguideBatchHandler.BulkCompleteActivty(lstCaseCategoryCreated, CallGuideBatchActivity.Chat);
                                //}
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Processing the Request", ex);
            }

        }
        void ExecuteFbProcess()
        {
            try
            {
                List<WorkItem> lstCurrentWorkItem = callguideBatchHandler.GetOpenCrmFaceBookActivity();
                if (lstCurrentWorkItem != null && lstCurrentWorkItem.Count > 0)
                {
                    List<CallGuideRecord> lstCallGuideRecord = callguideBatchHandler.GetCallGuideDBRecord(String.Join(",", lstCurrentWorkItem.Select(e => e.CallguideSessionId)), CallGuideBatchActivity.FaceBook);

                    if (lstCallGuideRecord != null && lstCallGuideRecord.Count > 0)
                    {


                        List<WorkItem> lstToUpdate = (from jobItem in lstCurrentWorkItem
                                                      join callGuideRecord in lstCallGuideRecord on jobItem.CallguideSessionId equals callGuideRecord.ContactId
                                                      select new WorkItem
                                                      {
                                                          AccountId = jobItem.AccountId,
                                                          ActivityId = jobItem.ActivityId,
                                                          CallGuideInfoId = jobItem.CallGuideInfoId,
                                                          CallguideSessionId = jobItem.CallguideSessionId,
                                                          ErrandTaskType = jobItem.ErrandTaskType,
                                                          Description = jobItem.Description,
                                                          UpdateData = callGuideRecord.Data,
                                                          FbUrl = callGuideRecord.FbUrl

                                                      }).ToList();


                        List<WorkItem> lstUpdated = callguideBatchHandler.BulkUpdateFaceBookPost(lstToUpdate);

                        if (lstUpdated != null && lstUpdated.Count > 0)
                        {
                            List<WorkItem> lstCaseCreated = callguideBatchHandler.BulkCreateCase(lstUpdated, CallGuideBatchActivity.FaceBook);

                            if (lstCaseCreated != null && lstCaseCreated.Count > 0)
                            {
                                callguideBatchHandler.BulkUpdateRegarding(lstCaseCreated, CallGuideBatchActivity.FaceBook);
                                callguideBatchHandler.BulkCompleteActivty(lstCaseCreated, CallGuideBatchActivity.FaceBook);

                                //List<WorkItem> lstCaseCategoryCreated = callguideBatchHandler.BulkCreateCaseCategory(lstUpdated, CallGuideBatchActivity.FaceBook);

                                //if (lstCaseCategoryCreated != null && lstCaseCategoryCreated.Count > 0)
                                //{
                                //    callguideBatchHandler.BulkUpdateRegarding(lstCaseCategoryCreated, CallGuideBatchActivity.FaceBook);
                                //    callguideBatchHandler.BulkCompleteActivty(lstCaseCategoryCreated, CallGuideBatchActivity.FaceBook);
                                //}
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Processing the Request", ex);
            }
        }
    }
 
}
