using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Endeavor.Crm.DeltabatchService;
using NUnit.Framework;
using Microsoft.Crm.Sdk.Samples;
using System.Collections.Generic;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
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
                job.GenerateFiles(localContext, job.FetchAllActiveQueuePosts(localContext));
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
                Guid guid = new Guid();
                DownloadJob job = new DownloadJob();
                job.RetrieveFile();
                job.UpdateContactsWithNewInfo(localContext);


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

        [Test, Category("Run Always")]
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

                IList<DeltabatchQueueEntity> controlFetch = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, UploadJob.deltabatchQueueColumnSet,
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(DeltabatchQueueEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
                        }
                    });
                UploadJob job = new UploadJob();
                IList<DeltabatchQueueEntity> testFetch = job.FetchAllActiveQueuePosts(localContext);


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
                IList<DeltabatchQueueEntity> queuePosts = job.FetchAllActiveQueuePosts(localContext);
                job.GenerateFiles(localContext, queuePosts);

                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
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
                IList<DeltabatchQueueEntity> queuePosts = job.FetchAllActiveQueuePosts(localContext);
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
    }
}
