using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Endeavor.Crm.UnitTest;
using Microsoft.Crm.Sdk.Samples;
using NUnit.Framework;
using Endeavor.Crm;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class AsyncOperationFixture : PluginFixtureBase
    {
        [Test, Category("DataCorrection")]
        public void SystemJob_ResendValueCodeThatHasNotBeenCreated()
        {

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                try
                {
                    // Get failed records
                    IList<AsyncOperationEntity> asyncOperationEntities = XrmRetrieveHelper.RetrieveMultiple<AsyncOperationEntity>(localContext, new ColumnSet(true)
                                                , new FilterExpression()
                                                    {
                                                        Conditions =
                                                        {
                                                            new ConditionExpression(AsyncOperationEntity.Fields.Name, ConditionOperator.Equal, "Create and Send Value Code from Refund"),
                                                            new ConditionExpression(AsyncOperationEntity.Fields.StatusCode, ConditionOperator.Equal, (int)Generated.asyncoperation_statuscode.Failed),
                                                            //new ConditionExpression(AsyncOperationEntity.Fields.CreatedOn, ConditionOperator.OnOrBefore, new DateTime(2019, 6, 25)),

                                                            //new ConditionExpression(AsyncOperationEntity.Fields.FriendlyMessage, ConditionOperator.Contains, "SecLib::AccessCheckEx failed"),
                                                            //new ConditionExpression(AsyncOperationEntity.Fields.FriendlyMessage, ConditionOperator.Contains, "ObjectTypeCode: 10022,"),
                                                            //new ConditionExpression(AsyncOperationEntity.Fields.FriendlyMessage, ConditionOperator.Contains, "AccessRights: AppendToAccess"),
                                                        }
                                                });

                    if(asyncOperationEntities.Count == 0)
                    {
                        localContext.Trace($"No failed records found. Aborting");
                        return;
                    }

                    localContext.Trace($"Found {asyncOperationEntities.Count} failed records!");



                    int i = 0;
                    int iErrors = 0;
                    foreach (var asyncOperationEntity in asyncOperationEntities)
                    {
                        string FriendlyMessage = asyncOperationEntity.FriendlyMessage;
                        if (FriendlyMessage == null) FriendlyMessage = "";

                        localContext.Trace($"Processing no {i}, asyncoperation with ID {asyncOperationEntity.Id}. Error:{FriendlyMessage}");

                        if (FriendlyMessage.Contains("SendValueCode failed. Exception: Not all parties has a valid value for mobilephone, which is required"))
                        {
                            localContext.Trace($"Known error, take next");
                            continue;
                        }

                        i++;
                        //if (i > 50)
                        //{
                        //    localContext.Trace($"Aborting! Test");
                        //    break;
                        //}

                        try
                        {




                        // Do we have a refund?
                        if (asyncOperationEntity.RegardingObjectId.LogicalName == RefundEntity.EntityLogicalName) {

                            // Get the refund, 
                            RefundEntity refund = XrmRetrieveHelper.RetrieveFirst<RefundEntity>(localContext, new ColumnSet(true)
                                                                    , new FilterExpression()
                                                                    {
                                                                        Conditions =
                                                                        {
                                                                            new ConditionExpression(RefundEntity.Fields.Id, ConditionOperator.Equal, asyncOperationEntity.RegardingObjectId.Id),
                                                                        }
                                                                    });

                            // Delete if not found
                            if (refund == null)
                            {
                                localContext.Trace($"Refund not found, deleting systemJob....");
                                localContext.OrganizationService.Delete(AsyncOperationEntity.EntityLogicalName, asyncOperationEntity.Id);
                                continue;
                            }

                            // Get the case
                            IncidentEntity incident = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, refund.cgi_Caseid, new ColumnSet(true));

                            // Get any connected valuecode
                            IList<ValueCodeEntity> codes = ValueCodeEntity.GetAllValueCodesForARefund(localContext, asyncOperationEntity.RegardingObjectId);
                            localContext.Trace($"Found value codes connected to Refund:{codes.Count}");

                            if(codes.Count > 0)
                            {
                                bool deleteTheSystemJob = false;

                                foreach (var code in codes)
                                {
                                    localContext.Trace($"valueCode found on refund that has status:{code.FormattedValues[ValueCodeEntity.Fields.statuscode]}");

                                    // Need to resend the code?
                                    if (code.statuscode == Generated.ed_valuecode_statuscode.Skapad)
                                    {
                                        localContext.Trace($"Resending value code :{code.Id}");

                                        ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, code.Id, new ColumnSet(true));
                                        valueCode.SendValueCode(localContext);

                                        deleteTheSystemJob = true;
                                        break;
                                    }
                                    else if (code.statuscode == Generated.ed_valuecode_statuscode.Skickad) {
                                        deleteTheSystemJob = true;
                                    }
                                    else
                                    {
                                        localContext.Trace($"valueCode has incorrect status, abort this systemJob");
                                        deleteTheSystemJob = false;
                                        continue;
                                    }
                                }

                                // 
                                if (deleteTheSystemJob == true) {
                                    localContext.Trace($"Value code allready sent! Deleteing systemJob");

                                    // Delete the failed system job
                                    localContext.OrganizationService.Delete(asyncOperationEntity.LogicalName, asyncOperationEntity.Id);
                                    continue;
                                }


                            }

                            if (codes.Count == 0)
                            //if (true)
                            {
                                int iDeliveryType = -1;
                                int iVoucherType = -1;

                                // Get the reimbursement form
                                // Get all reimbursement forms
                                ReimbursementFormEntity reimbursementForm = XrmRetrieveHelper.Retrieve<ReimbursementFormEntity>(localContext, refund.cgi_ReimbursementFormid.Id, new ColumnSet(true));

                                if ((reimbursementForm.cgi_reimbursementname == "Värdekod - E-post Försening" || reimbursementForm.cgi_reimbursementname ==  "Värdekod E-post - Försening")
                                    && refund.cgi_Amount.Value > 0)
                                {
                                    iVoucherType = 1;
                                    iDeliveryType = 1;
                                }
                                else if (reimbursementForm.cgi_reimbursementname == "Värdekod E-post - Ersättning"
                                    && refund.cgi_Amount.Value > 0)
                                {
                                    iVoucherType = 4;
                                    iDeliveryType = 1;
                                }
                                else if (reimbursementForm.cgi_reimbursementname == "Värdekod SMS - Försening"
                                    && refund.cgi_Amount.Value > 0)
                                {
                                    iVoucherType = 1;
                                    iDeliveryType = 2;

                                    // Make sure mobile-phone is in correct format
                                    if(string.IsNullOrWhiteSpace(refund.cgi_MobileNumber) || refund.cgi_MobileNumber.Length < 10)
                                    {
                                        localContext.Trace($"Invalid mobile no: {refund.cgi_MobileNumber}");
                                        continue;
                                    }
                                }
                                else {
                                    localContext.Trace($"No support for reimbursement template: {reimbursementForm.cgi_reimbursementname} Refund: {refund.cgi_refundnumber}");
                                    continue;
                                }

                                if (iVoucherType > 0) {
                                
                                    localContext.Trace($"Creating value code for refund: {refund.cgi_refundnumber}");


                                    // Try recreate it!
                                    ValueCodeFixture vcf = new ValueCodeFixture();
                                    var newValueCode = vcf.CallWorkflowCreateValueCodeAction(localContext,
                                                                                                                    iVoucherType,
                                                                                                                    refund.cgi_Amount.Value,
                                                                                                                    refund.cgi_MobileNumber,
                                                                                                                    refund.cgi_email,
                                                                                                                    refund.ToEntityReference(),
                                                                                                                    null,   // Lead
                                                                                                                    incident.cgi_Contactid,
                                                                                                                    null,
                                                                                                                    iDeliveryType
                                                                                                                    );

                                    localContext.Trace($"Value code created, sending!");


                                    ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, newValueCode.valueCodeRef, new ColumnSet(true));
                                    valueCode.SendValueCode(localContext);

                                    localContext.Trace($"Value code sent! Deleteing systemJob");

                                    // Delete the failed system job
                                    localContext.OrganizationService.Delete(asyncOperationEntity.LogicalName, asyncOperationEntity.Id);

                                    // For test only!!!
                                    //if (1 == 1)
                                    //{
                                    //    localContext.Trace($"This is the first test, abort process");
                                    //    break;
                                    //}



                                }
                            }
                        }

                        }
                        catch (Exception ex)
                        {
                            iErrors++;
                            localContext.Trace($"Exeption during processing of row:{i}. Ex:{ex.Message}. Total error count{iErrors}. Continue processing....");
                        }


                    }


                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                }
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
