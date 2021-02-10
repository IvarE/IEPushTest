using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CGIXrmHandler;
using System.Globalization;
using System.Threading;
using CGI.CRM2013.Skanetrafiken.CGIXrmLogger;
using CGIXrmHandler.CallGuide.Models;
using CGIXrmHandler.Shared;

namespace CGIXrmCallGuideJob
{
    class BatchJob
    {
        #region Declarations
        CallGuideBatchHandler _callguideBatchHandler;
        #endregion

        #region Main
        static void Main(string[] args)
        {

            try
            {
                BatchJob batchJob = new BatchJob();
                Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
                batchJob._callguideBatchHandler = new CallGuideBatchHandler();
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
                }

                Console.WriteLine("Process Completed Successfully.");
            }
            catch (Exception Ex)
            {
                LogToCrm log2Crm = new LogToCrm();
                log2Crm.Exception(Ex.Message, string.Empty, Ex,"Batch Process " + ConfigurationManager.AppSettings["JobName"]);
                Console.WriteLine("Process Completed Successfully."); 
            }


        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        void ExecuteChatProcess()
        {
            try
            {
                List<WorkItem> lstCurrentWorkItem = _callguideBatchHandler.GetOpenCrmChatActivity();
                if (lstCurrentWorkItem != null && lstCurrentWorkItem.Count > 0)
                {
                    List<CallGuideRecord> lstCallGuideRecord = _callguideBatchHandler.GetCallGuideDBRecord(String.Join(",", lstCurrentWorkItem.Select(e => e.CallguideSessionId)), CallGuideBatchActivity.Chat);

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


                        List<WorkItem> lstUpdated = _callguideBatchHandler.BulkUpdateChatConversation(lstToUpdate);

                        if (lstUpdated != null && lstUpdated.Count > 0)
                        {
                            List<WorkItem> lstCaseCreated = _callguideBatchHandler.BulkCreateCase(lstUpdated, CallGuideBatchActivity.Chat);

                            if (lstCaseCreated != null && lstCaseCreated.Count > 0)
                            {
                                _callguideBatchHandler.BulkUpdateRegarding(lstCaseCreated, CallGuideBatchActivity.Chat);
                                _callguideBatchHandler.BulkCompleteActivty(lstCaseCreated, CallGuideBatchActivity.Chat);
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

        /// <summary>
        /// 
        /// </summary>
        void ExecuteFbProcess()
        {
            try
            {
                List<WorkItem> lstCurrentWorkItem = _callguideBatchHandler.GetOpenCrmFaceBookActivity();
                if (lstCurrentWorkItem != null && lstCurrentWorkItem.Count > 0)
                {
                    List<CallGuideRecord> lstCallGuideRecord = _callguideBatchHandler.GetCallGuideDBRecord(String.Join(",", lstCurrentWorkItem.Select(e => e.CallguideSessionId)), CallGuideBatchActivity.FaceBook);

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


                        List<WorkItem> lstUpdated = _callguideBatchHandler.BulkUpdateFaceBookPost(lstToUpdate);

                        if (lstUpdated != null && lstUpdated.Count > 0)
                        {
                            List<WorkItem> lstCaseCreated = _callguideBatchHandler.BulkCreateCase(lstUpdated, CallGuideBatchActivity.FaceBook);

                            if (lstCaseCreated != null && lstCaseCreated.Count > 0)
                            {
                                _callguideBatchHandler.BulkUpdateRegarding(lstCaseCreated, CallGuideBatchActivity.FaceBook);
                                _callguideBatchHandler.BulkCompleteActivty(lstCaseCreated, CallGuideBatchActivity.FaceBook);
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
        #endregion
    }
}
