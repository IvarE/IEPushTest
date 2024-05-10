using System;
using System.IO;
using Endeavor.Crm.DeltabatchService;
using NUnit.Framework;
using Microsoft.Crm.Sdk.Samples;
using System.Collections.Generic;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk;
using System.Linq;

namespace Endeavor.Crm.IntegrationTest
{
    [TestFixture]
    public class DeltabatchServiceFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        internal ServerConnection ServerConnection
        {
            get
            {
                if (_serverConnection == null)
                {
                    _serverConnection = new ServerConnection();
                }
                return _serverConnection;
            }
        }

        internal ServerConnection.Configuration Config
        {
            get
            {
                return TestSetup.Config;
            }
        }

        [Test, Category("Debug")]
        public void SendFiles()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                UploadJob job = new UploadJob();
                job.GenerateFiles(localContext, job.FetchABatchOfActiveQueuePosts(localContext));
                job.UploadFiles();


                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        [Test, Category("Debug")]
        public void RetrieveFileAndUpdateExistingContacts()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                DownloadJob job = new DownloadJob();
                job.RetrieveFile();
                //job.UpdateContactsWithNewInfo(localContext);


                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        [Test, Category("Debug")]
        public void ArchiveFiles()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                DownloadJob job = new DownloadJob();
                job.ArchiveFile();

                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        [Test, Category("Debug")]
        public void DownloadJob()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                DownloadJob job = new DownloadJob();
                job.ExecuteJob();

                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        [Test, Category("Debug")]
        public void UploadJob()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                UploadJob job = new UploadJob();
                job.ExecuteJob();

                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        [Test, Category("Debug")]
        public void RetrieveFile()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                DownloadJob job = new DownloadJob();
                job.RetrieveFile();

                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        [Test, Category("Regression")]
        public void FetchQueuePosts()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion


                IList<DeltabatchQueueEntity> controlFetch = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(
                    localContext,
                    new QueryExpression()
                    {
                        EntityName = DeltabatchQueueEntity.EntityLogicalName,
                        ColumnSet = DeltabatchJobHelper.deltabatchQueueColumnSet,
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(DeltabatchQueueEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
                            }
                        },
                        TopCount = 5000
                    });

                UploadJob job = new UploadJob();
                IList<DeltabatchQueueEntity> testFetch = job.FetchABatchOfActiveQueuePosts(localContext);


                NUnit.Framework.Assert.AreEqual(controlFetch.Count, testFetch.Count);
                for (int i = 0; i < controlFetch.Count; i++)
                {
                    NUnit.Framework.Assert.AreEqual(controlFetch[i].Id, testFetch[i].Id);
                    NUnit.Framework.Assert.AreEqual(controlFetch[i].ed_ContactNumber, testFetch[i].ed_ContactNumber);
                    NUnit.Framework.Assert.AreEqual(controlFetch[i].ed_DeltabatchOperation, testFetch[i].ed_DeltabatchOperation);
                    NUnit.Framework.Assert.AreEqual(controlFetch[i].ed_name, testFetch[i].ed_name);
                }

                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        [Test, Category("Debug")]
        public void ModulateFiles()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion
                
                //// Partition file into 20000-row sized files
                //string[] lines = System.IO.File.ReadAllLines(@"C:\Temp\Deltabatch\Retrieved\out_Skane_Consumer_PersonFilter_Partition6.txt");

                //StreamWriter file = null;
                ////int partition1 = 100, partition2 = 2100, partition3 = 52100, partition4 = 102100, partition5 = 152100, partition6 = 202100;
                //for (int i = 1; i < lines.Length; i++)
                //{
                //    if (i % 20000 == 1)
                //    {
                //        file?.Close();
                //        file = File.CreateText(@"C:\Temp\Deltabatch\Retrieved\out_Skane_Consumer_PersonFilter_Partition6_sub" + (int)((i / 20000) + 1) + ".txt");
                //        file.WriteLine(lines[0]);
                //    }
                //    file.WriteLine(lines[i]);
                //}
                //file.Close();
                //file.Dispose();


                //// Join partitions
                //List<string[]> linesArrays = new List<string[]>();
                //for (int i = 4; i <= 8; i++)
                //{
                //    linesArrays.Add(File.ReadAllLines(@"C:\Temp\Deltabatch\Retrieved\out_Skane_Consumer_PersonFilter_Partition3_2_sub" + i + ".txt"));
                //}

                ////int partition1 = 10000;

                //using (StreamWriter file1 = File.CreateText(@"C:\Temp\Deltabatch\Retrieved\out_Skane_Consumer_PersonFilter_Partition3_2_sub4-8.txt"))
                //{
                //    int firstLine = 1;
                //    foreach (string[] array in linesArrays)
                //    {
                //        if (firstLine == 1)
                //        {
                //            file1.WriteLine(array[0]);
                //            firstLine = 0;
                //        }
                //        for (int j = 1; j < array.Length; j++)
                //        {
                //            file1.WriteLine(array[j]);
                //        }
                //    }
                //    //for (int i = 1; i < lines1.Length; i++)
                //    //{
                //    //    file1.WriteLine(lines1[i]);
                //    //}
                //    //for (int i = 1; i < lines2.Length; i++)
                //    //{
                //    //    file1.WriteLine(lines2[i]);
                //    //}
                //}


                // // Filter one partition File into a new
                //string[] lines = System.IO.File.ReadAllLines(@"C:\Temp\Deltabatch\Retrieved\History\NotRun\out_Skane_Consumer_PersonFilter_BigPartition.txt");

                //using (StreamWriter file = File.CreateText(@"C:\Temp\Deltabatch\Retrieved\out_Skane_Consumer_PersonFilter_BigPartition_RejectedPatch.txt"))
                //{
                //    file.WriteLine(lines[0]);
                //    for (int i = 1; i < lines.Length; i++)
                //    {
                //        if (!string.IsNullOrWhiteSpace(lines[i].Split(';')[22]))
                //        {
                //            file.WriteLine(lines[i]);
                //        }
                //    }
                //}
                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }


        [Test, Category("Debug")]
        public void CreateFilesWithoutRemovingQueues()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                UploadJob job = new UploadJob();
                localContext.Trace($"fetchStart, Timer: " + stopwatch.ElapsedMilliseconds);
                IList<DeltabatchQueueEntity> queuePosts = job.FetchABatchOfActiveQueuePosts(localContext);
                localContext.Trace($"GenerateFilesStart, Timer: " + stopwatch.ElapsedMilliseconds);
                job.GenerateFiles(localContext, queuePosts);

                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        [Test, Category("Debug")]
        public void ReadFileSpeedTest()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                StreamReader file = new StreamReader(@"C:\Temp\Deltabatch\Retrieved\out_Skane_Consumer_PersonFilter_BigPartition3-20.txt");
                string outputLine;
                int numberOfOperations = 0;
                while ((outputLine = file.ReadLine()) != null)
                {
                    if (numberOfOperations % 5000 < 3)
                    {
                        localContext.TracingService.Trace($"At iteration {numberOfOperations}, clock is {DateTime.Now.ToLongTimeString()}");
                    }
                    numberOfOperations++;
                }
                localContext.TracingService.Trace($"numberOfOperation = {numberOfOperations}");
                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        [Test, Category("Debug")]
        public void CreateDeltabatchQueues()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion
                for (int i = 0; i < 2000; i++)
                {
                    Guid guid = new Guid();
                    DeltabatchQueueEntity q = new DeltabatchQueueEntity
                    {
                        ed_Contact = new EntityReference(ContactEntity.EntityLogicalName, guid),
                        ed_ContactGuid = guid.ToString(),
                        ed_ContactNumber = ((long)(i * (long)985746298754239876) % (long)1000000000000).ToString(),
                        ed_DeltabatchOperation = Skanetrafiken.Crm.Schema.Generated.ed_deltabatchqueue_ed_deltabatchoperation.Plus,
                        ed_name = "TestQueue" + i
                    };
                    XrmHelper.Create(localContext, q);
                }
            }
        }

        [Test, Category("Debug")]
        public void UploadFilesWithoutRemovingQueues()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                UploadJob job = new UploadJob();
                IList<DeltabatchQueueEntity> queuePosts = job.FetchABatchOfActiveQueuePosts(localContext);
                job.GenerateFiles(localContext, queuePosts);
                job.UploadFiles();

                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        [Test, Category("Debug")]
        public void UploadAllQueues()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                UploadJob job = new UploadJob();
                job.ExecuteJob();

                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        [Test, Category("Debug")]
        public void FetchMoreThanFiveThousandRecords()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                QueryExpression batchQuery = new QueryExpression
                {
                    EntityName = DeltabatchQueueEntity.EntityLogicalName,
                    ColumnSet = DeltabatchJobHelper.deltabatchQueueColumnSet,
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(DeltabatchQueueEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
                        }
                    },
                };

                // Assign the pageinfo properties to the query expression.
                batchQuery.PageInfo = new PagingInfo();
                batchQuery.PageInfo.Count = 5000;
                batchQuery.PageInfo.PageNumber = 1;

                batchQuery.PageInfo.PagingCookie = null;

                IList<DeltabatchQueueEntity> lst = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, batchQuery).Take(80000).ToList();
                //var lst1 = lst.Take(800);

                int counter = lst.Count();

                //IList<DeltabatchQueueEntity> lst2 = lst1;

                int i = 0;
                foreach (var delta in lst)
                {
                    i++;
                }



                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }
    }
}
