using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

using Generated = Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Globalization;
using System.Net;
using Skanetrafiken.Crm;
using System.Text.RegularExpressions;
using System.Threading;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture(Category = "Plugin")]
    public class DeltabatchQueueFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void GeneratePlusFilesForAllActiveContacts()
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

                QueryExpression query = new QueryExpression
                {
                    EntityName = ContactEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(ContactEntity.Fields.ContactId, ContactEntity.Fields.cgi_socialsecuritynumber, ContactEntity.Fields.FullName),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.NotNull),
                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active)
                        }
                    }
                };

                IList<ContactEntity> allContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, query);

                Regex regEx = new Regex("^[0-9]{12}$");
                foreach (ContactEntity c in allContacts)
                {
                    if (regEx.IsMatch(c.cgi_socialsecuritynumber))
                    {
                        DeltabatchQueueEntity q = new DeltabatchQueueEntity
                        {
                            ed_Contact = c.ToEntityReference(),
                            ed_ContactGuid = c.ContactId?.ToString(),
                            ed_ContactNumber = c.cgi_socialsecuritynumber,
                            ed_DeltabatchOperation = Generated.ed_deltabatchqueue_ed_deltabatchoperation.Plus,
                            ed_name = $"FirstMegaPlusFile: {c.FullName}, {DateTime.Now.ToString()}".Length > 99 ? $"FirstMegaPlusFile: {c.FullName}, {DateTime.Now.ToString()}".Substring(0, 99) : $"FirstMegaPlusFile: {c.FullName}, {DateTime.Now.ToString()}"
                        };

                        XrmHelper.Create(localContext, q);
                    }
                }


                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        //[Test, Category("Debug")]
        //public void deleteDeltabatchQuePosts()
        //{
        //    #region Test Setup
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        //        stopwatch.Start();
        //        #endregion

        //        ColumnSet deltabatchQueueColumnSet = new ColumnSet(
        //       DeltabatchQueueEntity.Fields.ed_ContactGuid,
        //       DeltabatchQueueEntity.Fields.ed_ContactNumber,
        //       DeltabatchQueueEntity.Fields.ed_DeltabatchOperation,
        //       DeltabatchQueueEntity.Fields.ed_DeltabatchQueueId,
        //       DeltabatchQueueEntity.Fields.ed_name,
        //       DeltabatchQueueEntity.Fields.CreatedOn
        //    );

        //        QueryExpression batchQuery = new QueryExpression
        //        {
        //            EntityName = DeltabatchQueueEntity.EntityLogicalName,
        //            ColumnSet = deltabatchQueueColumnSet,
        //            Criteria =
        //        {
        //            Conditions =
        //            {
        //                new ConditionExpression(DeltabatchQueueEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
        //            }
        //        }
        //            //TopCount = Properties.Settings.Default.DeltabatchQueueCount
        //        };


        //        IList<DeltabatchQueueEntity> currentQueues = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, batchQuery).Take(680000).ToList();

        //        List<DeltabatchQueueEntity> list = new List<DeltabatchQueueEntity>();
        //        foreach (DeltabatchQueueEntity dbq in currentQueues)
        //        {
        //            list.Add(dbq);
        //        }


        //        List<DeltabatchQueueEntity> distinctList = list.GroupBy(x => x.ed_ContactNumber).Select(f => f.First()).ToList();
        //        List<DeltabatchQueueEntity> sortedList = distinctList.OrderByDescending(dbq => dbq.CreatedOn).ToList<DeltabatchQueueEntity>();
        //        List<string> processedSocSecNumbers = new List<string>();

        //        localContext.Trace($"{sortedList?.Count()}  queues.");

        //        int processing = 0;



        //        try
        //        {
        //            foreach (DeltabatchQueueEntity dbq in currentQueues)
        //            {
        //                processing++;

        //                localContext.Trace($"{processing} of {sortedList?.Count()} processed");

        //                if (dbq.ed_DeltabatchQueueId != null && !Guid.Empty.Equals(dbq.ed_DeltabatchQueueId))
        //                {
        //                    SetStateRequest req = new SetStateRequest
        //                    {
        //                        EntityMoniker = dbq.ToEntityReference(),
        //                        State = new Microsoft.Xrm.Sdk.OptionSetValue((int)Generated.ed_DeltabatchQueueState.Inactive),
        //                        Status = new Microsoft.Xrm.Sdk.OptionSetValue((int)Generated.ed_deltabatchqueue_statuscode.Inactive)
        //                    };
        //                    SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {

        //            throw e;
        //        }

        //        #region Test Cleanup
        //        localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
        //    }
        //    #endregion
        //}


        [Test, Category("Debug")]
        public void GenerateFile()
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

                ColumnSet deltabatchQueueColumnSet = new ColumnSet(
                    DeltabatchQueueEntity.Fields.ed_ContactGuid,
                    DeltabatchQueueEntity.Fields.ed_ContactNumber,
                    DeltabatchQueueEntity.Fields.ed_DeltabatchOperation,
                    DeltabatchQueueEntity.Fields.ed_DeltabatchQueueId,
                    DeltabatchQueueEntity.Fields.ed_name,
                    DeltabatchQueueEntity.Fields.CreatedOn
                );


                QueryExpression batchQuery = new QueryExpression
                {
                    EntityName = DeltabatchQueueEntity.EntityLogicalName,
                    ColumnSet = deltabatchQueueColumnSet,
                    Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(DeltabatchQueueEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
                    }
                }
                    //TopCount = Properties.Settings.Default.DeltabatchQueueCount
                };

                // Assign the pageinfo properties to the query expression.
                batchQuery.PageInfo = new PagingInfo();
                batchQuery.PageInfo.Count = 5000;
                batchQuery.PageInfo.PageNumber = 1;

                // The current paging cookie. When retrieving the first page, 
                // pagingCookie should be null.
                batchQuery.PageInfo.PagingCookie = null;

                IList<DeltabatchQueueEntity> currentQueues = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, batchQuery).Take(680000).ToList();

                //try
                //{

                //    //foreach (DeltabatchQueueEntity q in sortedList)
                //    //{


                //    //    processing++;
                //    //    localContext.Trace($"{processing} of {sortedList?.Count()} processed");

                //    //    // Do not add if SocSecNr already processed
                //    //    if (processedSocSecNumbers.Contains(q.ed_ContactNumber) || !CustomerUtility.CheckPersonnummerFormat(q.ed_ContactNumber))
                //    //        continue;

                //    //    string fritext = q.ed_name.Split(":".ToCharArray())[0].Replace(" ", "").Replace("-", "");


                //    //    plusFileBuilder.AppendLine($"{q.ed_ContactNumber};{q.ed_ContactGuid};{fritext}");

                //    //    processedSocSecNumbers.Add(q.ed_ContactNumber);
                //    //}

                //    //// Write Plus File
                //    //var plusFileName = $"C:\\AcneData\\TEST{DateTime.Now.ToShortDateString()}_{DateTime.Now.ToShortTimeString().Replace(':', '-')}.txt";
                //    //if (File.Exists(plusFileName))
                //    //{
                //    //    throw new Exception($"File {plusFileName} already exists.");
                //    //    //string plusFileDatedPath = plusFileName + "_";
                //    //    //System.IO.File.Move(plusFileName, plusFileDatedPath);
                //    //}
                //    //string plusString = plusFileBuilder.ToString();
                //    //if (string.IsNullOrWhiteSpace(plusString))
                //    //{

                //    //    ContactEntity c = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(ContactEntity.Fields.cgi_socialsecuritynumber),
                //    //    new FilterExpression
                //    //    {
                //    //        Conditions =
                //    //            {
                //    //                new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.NotNull),
                //    //                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                //    //                new ConditionExpression(ContactEntity.Fields.ed_HasSwedishSocialSecurityNumber, ConditionOperator.Equal, true),
                //    //                new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.NotNull)
                //    //            }
                //    //    });
                //    //    plusString = $"{c.cgi_socialsecuritynumber};{c.Id.ToString()};ToAvoidEmpyFile";

                //    //}
                //    //using (System.IO.StreamWriter plusFile = new System.IO.StreamWriter(plusFileName))
                //    //{
                //    //    plusFile.WriteLine(plusString);
                //    //    plusFile.Close();
                //    //}



                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}


                int processing = 0;

                localContext.Trace($"{currentQueues?.Count()}  contacts.");

                foreach (DeltabatchQueueEntity dbq in currentQueues)
                {
                    processing++;
                   localContext.Trace($"{processing} of {currentQueues?.Count()} processed");

                    if (dbq.ed_DeltabatchQueueId != null && !Guid.Empty.Equals(dbq.ed_DeltabatchQueueId))
                    {
                        // TODO: teo - is deprecated as of 2016. Update when SDK is updated. to 6+
                        //DeltabatchQueueEntity updateEntity = new DeltabatchQueueEntity
                        //{
                        //    Id = (Guid)dbq.ed_DeltabatchQueueId,
                        //    statuscode = Generated.ed_deltabatchqueue_statuscode.Inactive
                        //};
                        //XrmHelper.Update(localContext.OrganizationService, updateEntity);
                        SetStateRequest req = new SetStateRequest
                        {
                            EntityMoniker = dbq.ToEntityReference(),
                            State = new Microsoft.Xrm.Sdk.OptionSetValue((int)Generated.ed_DeltabatchQueueState.Inactive),
                            Status = new Microsoft.Xrm.Sdk.OptionSetValue((int)Generated.ed_deltabatchqueue_statuscode.Inactive)
                        };
                        SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
                    }
                }
                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);

                #endregion

            }
        }


        [Test, Category("Debug")]
        public void CreateDeactiveDeleteContactCheckDelta()
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

                #region Remove existing data
                QueryExpression existingDelta = new QueryExpression
                {
                    EntityName = DeltabatchQueueEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(false),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_ContactNumber, ConditionOperator.Equal, "199102013936")
                        }
                    },
                    NoLock = true
                };

                var existingDeltaToRemove = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, existingDelta);
                if (existingDeltaToRemove != null && existingDeltaToRemove.Count() > 0)
                {
                    foreach (var deltaQueuePost in existingDeltaToRemove)
                    {
                        XrmHelper.Delete(localContext.OrganizationService, deltaQueuePost.ToEntityReference());
                    }
                }

                QueryExpression existingContact = new QueryExpression
                {
                    EntityName = ContactEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(false),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, "199102013936")
                        }
                    }
                };

                var existingContactsToRemove = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, existingContact);
                if (existingContactsToRemove != null && existingContactsToRemove.Count() > 0)
                {
                    foreach (var contactPost in existingContactsToRemove)
                    {
                        SetStateRequest req1 = new SetStateRequest
                        {
                            EntityMoniker = contactPost.ToEntityReference(),
                            State = new OptionSetValue((int)Generated.ContactState.Inactive),
                            Status = new OptionSetValue((int)Generated.contact_statuscode.Inactive)
                        };
                        SetStateResponse resp1 = (SetStateResponse)localContext.OrganizationService.Execute(req1);

                        XrmHelper.Delete(localContext.OrganizationService, contactPost.ToEntityReference());
                    }
                }
                #endregion

                #region Create New Contact
                ContactEntity newContact = new ContactEntity
                {
                    cgi_socialsecuritynumber = "199102013936",
                    FirstName = "Marcus",
                    LastName = "Stenswed",
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                    ed_HasSwedishSocialSecurityNumber = true
                };
                Guid newContactGuid = XrmHelper.Create(localContext.OrganizationService, newContact);
                #endregion

                Thread.Sleep(60000);

                #region Check if DeltaBatch has been created correctly
                QueryExpression queryDelta1 = new QueryExpression
                {
                    EntityName = DeltabatchQueueEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(DeltabatchQueueEntity.Fields.ed_DeltabatchOperation),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_ContactNumber, ConditionOperator.Equal, "199102013936"),
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_DeltabatchOperation, ConditionOperator.Equal, (int)Generated.ed_deltabatchqueue_ed_deltabatchoperation.Plus)
                            //new ConditionExpression(DeltabatchQueueEntity.Fields.statuscode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
                        }
                    }
                };

                var newlyCreatedDeltas = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, queryDelta1);

                if (newlyCreatedDeltas == null || newlyCreatedDeltas.Count < 1)
                {
                    throw new InvalidPluginExecutionException("No deltabatch queue has been created for the new contact...");
                }
                else if (newlyCreatedDeltas.Count > 1)
                {
                    throw new InvalidPluginExecutionException("Too many deltabatch queue posts exists for the same SSN...");
                }
                else
                {
                    // Everything is fine!
                }
                #endregion

                #region Create another Contact, same Personnummer. Should not create another Deltabatch Queue post
                ContactEntity newContact2 = new ContactEntity
                {
                    cgi_socialsecuritynumber = "199102013936",
                    FirstName = "Marcus1",
                    LastName = "Stenswed1",
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                    ed_HasSwedishSocialSecurityNumber = true
                };
                Guid newContact2Guid = XrmHelper.Create(localContext.OrganizationService, newContact2);

                Thread.Sleep(30000);

                var newlyCreatedDeltas2 = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, queryDelta1);
                if (newlyCreatedDeltas2 == null || newlyCreatedDeltas2.Count < 1)
                {
                    throw new InvalidPluginExecutionException("No deltabatch queue has been created for the new contact...");
                }
                else if (newlyCreatedDeltas2.Count > 1)
                {
                    throw new InvalidPluginExecutionException("Too many deltabatch queue posts exists for the same SSN...");
                }
                else
                {
                    // Everything is fine!
                }
                #endregion

                #region Deactive one of the contacts and make sure deltabatch queue post still exists as plus.
                ContactEntity contact2 = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, newContact2Guid, new ColumnSet(false));
                SetStateRequest req = new SetStateRequest
                {
                    EntityMoniker = contact2.ToEntityReference(),
                    State = new OptionSetValue((int)Generated.ContactState.Inactive),
                    Status = new OptionSetValue((int)Generated.contact_statuscode.Inactive)
                };
                SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);

                Thread.Sleep(60000);

                QueryExpression queryDeltaPlus = new QueryExpression
                {
                    EntityName = DeltabatchQueueEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(false),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_ContactNumber, ConditionOperator.Equal, "199102013936"),
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_DeltabatchOperation, ConditionOperator.Equal, (int)Generated.ed_deltabatchqueue_ed_deltabatchoperation.Plus),
                            //new ConditionExpression(DeltabatchQueueEntity.Fields.statuscode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
                        }
                    }
                };

                var deltaPlus = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, queryDeltaPlus);
                if (deltaPlus == null || deltaPlus.Count < 1)
                {
                    throw new InvalidPluginExecutionException("No deltabatch queue has been created for the new contact...");
                }
                else if (deltaPlus.Count > 1)
                {
                    throw new InvalidPluginExecutionException("Too many deltabatch queue posts exists for the same SSN...");
                }
                else
                {
                    // Everything is fine!
                }


                QueryExpression queryDeltaMinus = new QueryExpression
                {
                    EntityName = DeltabatchQueueEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(false),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_ContactNumber, ConditionOperator.Equal, "199102013936"),
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_DeltabatchOperation, ConditionOperator.Equal, (int)Generated.ed_deltabatchqueue_ed_deltabatchoperation.Minus),
                            //new ConditionExpression(DeltabatchQueueEntity.Fields.statuscode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
                        }
                    }
                };

                var deltaMinus = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, queryDeltaMinus);
                if (deltaMinus == null || deltaMinus.Count > 0)
                {
                    throw new InvalidPluginExecutionException("No deltabatch queue has been created for the new contact...");
                }
                else
                {
                    // Everything is fine!
                }
                #endregion

                #region Deactivate other Contact to make sure deltabatch queue posts are correct

                ContactEntity contact1 = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, newContactGuid, new ColumnSet(false));
                SetStateRequest req2 = new SetStateRequest
                {
                    EntityMoniker = contact1.ToEntityReference(),
                    State = new OptionSetValue((int)Generated.ContactState.Inactive),
                    Status = new OptionSetValue((int)Generated.contact_statuscode.Inactive)
                };
                SetStateResponse resp2 = (SetStateResponse)localContext.OrganizationService.Execute(req2);

                Thread.Sleep(60000);

                QueryExpression queryDeltaMinus2 = new QueryExpression
                {
                    EntityName = DeltabatchQueueEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(DeltabatchQueueEntity.Fields.ed_DeltabatchOperation),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_ContactNumber, ConditionOperator.Equal, "199102013936"),
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_DeltabatchOperation, ConditionOperator.Equal, (int)Generated.ed_deltabatchqueue_ed_deltabatchoperation.Minus),
                            //new ConditionExpression(DeltabatchQueueEntity.Fields.statuscode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
                        }
                    }
                };

                var deltaMinus2 = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, queryDeltaMinus2);
                if (deltaMinus2 == null || deltaMinus2.Count < 1 || deltaMinus2.Count != 1)
                {
                    throw new InvalidPluginExecutionException("No deltabatch queue has been created for the new contact...");
                }
                else
                {
                    // Everything is fine!
                }
                #endregion


                #region Create and Delete Contact
                ContactEntity contactToBeDeleted = new ContactEntity
                {
                    cgi_socialsecuritynumber = "199102013936",
                    FirstName = "Marcus",
                    LastName = "Stenswed",
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                    ed_HasSwedishSocialSecurityNumber = true
                };
                Guid contactIdToBeDeleted = XrmHelper.Create(localContext.OrganizationService, contactToBeDeleted);
                contactToBeDeleted = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contactIdToBeDeleted, new ColumnSet(false));

                Thread.Sleep(60000);

                SetStateRequest req3 = new SetStateRequest
                {
                    EntityMoniker = contactToBeDeleted.ToEntityReference(),
                    State = new OptionSetValue((int)Generated.ContactState.Inactive),
                    Status = new OptionSetValue((int)Generated.contact_statuscode.Inactive)
                };
                SetStateResponse resp3 = (SetStateResponse)localContext.OrganizationService.Execute(req3);

                XrmHelper.Delete(localContext.OrganizationService, contactToBeDeleted.ToEntityReference());

                QueryExpression queryDeltaMinus3 = new QueryExpression
                {
                    EntityName = DeltabatchQueueEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(DeltabatchQueueEntity.Fields.ed_DeltabatchOperation),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_ContactNumber, ConditionOperator.Equal, "199102013936"),
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_DeltabatchOperation, ConditionOperator.Equal, (int)Generated.ed_deltabatchqueue_ed_deltabatchoperation.Minus),
                            //new ConditionExpression(DeltabatchQueueEntity.Fields.statuscode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
                        }
                    }
                };

                var deltaMinus3 = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, queryDeltaMinus3);
                if (deltaMinus3 == null || deltaMinus3.Count < 1 || deltaMinus3.Count != 1)
                {
                    throw new InvalidPluginExecutionException("No deltabatch queue has been created for the new contact...");
                }
                else
                {
                    // Everything is fine!
                }
                #endregion


                #region Remove existing data
                QueryExpression existingDeltaEnd = new QueryExpression
                {
                    EntityName = DeltabatchQueueEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(false),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_ContactNumber, ConditionOperator.Equal, "199102013936")
                        }
                    },
                    NoLock = true
                };

                var existingDeltaToRemoveEnd = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, existingDeltaEnd);
                if (existingDeltaToRemoveEnd != null && existingDeltaToRemoveEnd.Count() > 0)
                {
                    foreach (var deltaQueuePost in existingDeltaToRemoveEnd)
                    {
                        XrmHelper.Delete(localContext.OrganizationService, deltaQueuePost.ToEntityReference());
                    }
                }

                QueryExpression existingContactEnd = new QueryExpression
                {
                    EntityName = ContactEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(false),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, "199102013936")
                        }
                    }
                };

                var existingContactsToRemoveEnd = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, existingContactEnd);
                if (existingContactsToRemoveEnd != null && existingContactsToRemoveEnd.Count() > 0)
                {
                    foreach (var contactPost in existingContactsToRemoveEnd)
                    {
                        SetStateRequest req1End = new SetStateRequest
                        {
                            EntityMoniker = contactPost.ToEntityReference(),
                            State = new OptionSetValue((int)Generated.ContactState.Inactive),
                            Status = new OptionSetValue((int)Generated.contact_statuscode.Inactive)
                        };
                        SetStateResponse resp1End = (SetStateResponse)localContext.OrganizationService.Execute(req1End);

                        XrmHelper.Delete(localContext.OrganizationService, contactPost.ToEntityReference());
                    }
                }
                #endregion


                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

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

    }
}
