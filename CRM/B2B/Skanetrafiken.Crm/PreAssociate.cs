using System;
using System.Collections;
using Microsoft.Xrm.Sdk;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.Crm
{
    public class PreAssociate : Plugin
    {
        public PreAssociate()
            : base(typeof(PreAssociate))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(
                (int)Plugin.SdkMessageProcessingStepStage.PreOperation,
                "Associate",
                null,
                new Action<LocalPluginContext>(PreExecuteAssociate)));
        }

        protected void PreExecuteAssociate(LocalPluginContext localContext)
        {
            //throw new Exception("Debug @Teo");
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }

            // Must be Post operation
            if (localContext.PluginExecutionContext.Stage != (int)Plugin.SdkMessageProcessingStepStage.PreOperation)
            {
                throw new InvalidPluginExecutionException("Plugin must run in Pre-operation mode!");
            }

            // INFO: (joan) Don't do anything in offline mode
            if (localContext.PluginExecutionContext.IsExecutingOffline)
                return;

            localContext.TracingService.Trace("{0} {1} {2} {3}", localContext.PluginExecutionContext.Stage, localContext.PluginExecutionContext.MessageName, localContext.PluginExecutionContext.PrimaryEntityName, localContext.PluginExecutionContext.Depth);

            if (localContext.PluginExecutionContext.InputParameters.Contains("Target") &&
                localContext.PluginExecutionContext.InputParameters["Target"] is EntityReference)
            {
                // Get the "Relationship" Key from context
                string strRelationshipName = null;
                if (localContext.PluginExecutionContext.InputParameters.Contains("Relationship"))
                {
                    strRelationshipName = localContext.PluginExecutionContext.InputParameters["Relationship"].ToString();
                }

                localContext.TracingService.Trace("RelationshipName: {0}", strRelationshipName);

                EntityReference targetId = (EntityReference)localContext.PluginExecutionContext.InputParameters["Target"];
                EntityReferenceCollection relatedEntityIds = (EntityReferenceCollection)localContext.PluginExecutionContext.InputParameters["RelatedEntities"];

                localContext.TracingService.Trace("Target: {0}", targetId.LogicalName);


                // Check the "Relationship Name" with your intended one
                if (strRelationshipName.StartsWith("campaignproduct_association"))
                {
                    try
                    {
                        if (targetId.LogicalName == CampaignEntity.EntityLogicalName)
                        {
                            CampaignEntity.HandlePreAssociateWithProduct(localContext, targetId, relatedEntityIds);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidPluginExecutionException(ex.Message, ex);
                    }
                }

            }
        }
    }
}
