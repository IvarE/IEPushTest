using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using System.Collections.Generic;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class QuoteProductEntity : Generated.QuoteDetail
    {
        public static void HandleQuoteProductEntityCreate(Plugin.LocalPluginContext localContext, QuoteProductEntity quoteProduct)
        {
            localContext.Trace("Inside HandleQuoteProductEntityCreate");
            //validate necessary things to generateSlots
            if (quoteProduct.UoMId != null && quoteProduct.ProductId != null && quoteProduct.ed_FromDate != null && quoteProduct.ed_ToDate != null)
            {
                FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
                if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
                {
                    UpdateOrGenerateSlots(localContext, quoteProduct);
                }
            }
        }

        public static void HandleQuoteProductEntityUpdate(Plugin.LocalPluginContext localContext, QuoteProductEntity target, QuoteProductEntity preImage)
        {
            localContext.Trace("Inside HandleQuoteProductEntityUpdate");

            localContext.Trace("PreImage Info");
            preImage.Trace(localContext.TracingService);
            localContext.Trace("_______________");

            localContext.Trace("Target Info");
            target.Trace(localContext.TracingService);
            localContext.Trace("_______________");

            if (!target.IsAttributeModified(preImage, QuoteProductEntity.Fields.UoMId))
            {
                localContext.Trace("UoMID not modified");
                target.UoMId = preImage.UoMId;
            }
            if (!target.IsAttributeModified(preImage, QuoteProductEntity.Fields.ProductId))
            {
                localContext.Trace("ProductId not modified");
                target.ProductId = preImage.ProductId;
            }
            
            //validate necessary things to generateSlots
            if (target.UoMId != null && target.ProductId != null)
            {
                localContext.Trace("UoMId and ProductId not NULL");
                FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
                if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
                {
                    localContext.Trace("ed_bookingSystem enabled");
                    if(target.IsAttributeModified(preImage,QuoteProductEntity.Fields.ed_FromDate) || target.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_ToDate))
                    {
                        localContext.Trace("ed_FromDate or ed_ToDate modified");
                        UpdateOrGenerateSlots(localContext, target, preImage);
                    }
                    
                }
            }
        }

        public void HandlePreQuoteProductUpdate(Plugin.LocalPluginContext localContext, QuoteProductEntity preImage)
        {
            FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
            {
                updatePriceQuoteProduct(localContext, preImage);
            }
        }

        public void updatePriceQuoteProduct(Plugin.LocalPluginContext localContext, QuoteProductEntity preImage)
        {
            if (this.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_totalslots) || this.IsAttributeModified(preImage, QuoteProductEntity.Fields.PricePerUnit))
            {
                decimal price = 0;
                int quantity = 0;
                int totalSlots = 0;

                if (!this.IsAttributeModified(preImage, QuoteProductEntity.Fields.PricePerUnit))
                {
                    price = preImage.PricePerUnit.Value;
                }
                else
                {
                    price = this.PricePerUnit.Value;
                }

                if (!this.IsAttributeModified(preImage, QuoteProductEntity.Fields.Quantity))
                {
                    quantity = (int)preImage.Quantity.Value;
                }
                else
                {
                    quantity = (int)this.Quantity.Value;
                }
                if (!this.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_totalslots))
                {
                    totalSlots = preImage.ed_totalslots.Value;
                }
                else
                {
                    totalSlots = this.ed_totalslots.Value;
                }


                this.Tax = new Money((totalSlots - quantity) * price);
            }
        }

        /*
         function UpdateOrGenerateSlots
         -> quoteProduct target of the execution (Create/Update)
         -> preImage (preImage Update)
         */
        public static void UpdateOrGenerateSlots(Plugin.LocalPluginContext localContext, QuoteProductEntity quoteProduct, QuoteProductEntity preImage = null)
        {
            localContext.Trace("Inside UpdateOrGenerateSlots");
            //bool removeAllSlots = false;
            //bool fromDateModified = false;
            //bool toDateModified = false;
            List<SlotsEntity> availableSlots = null;
            //DateTime? startDate = quoteProduct.ed_FromDate;
            //DateTime? endDate = quoteProduct.ed_ToDate;

            //DateTime? startRemoveIntervalFrom = null; //used if after Update the preImage.ed_FromDate is before the quoteProduct.ed_FromDate
            //DateTime? endRemoveIntervalFrom = null; //used if after Update the preImage.ed_FromDate is before the quoteProduct.ed_FromDate
            //DateTime? startRemoveIntervalTo = null; //used if after Update the preImage.ed_ToDate is after the quoteProduct.ed_ToDate
            //DateTime? endRemoveIntervalTo = null; //used if after Update the preImage.ed_ToDate is after the quoteProduct.ed_ToDate
            //DateTime? startCreateIntervalFrom = null;
            //DateTime? endCreateIntervalFrom = null;
            //DateTime? startCreateIntervalTo = null;
            //DateTime? endCreateIntervalTo = null;

            Guid? opportunityId = null;

            if (quoteProduct.QuoteId != null && quoteProduct.QuoteId.Id != Guid.Empty)
            {
                QuoteEntity quote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, quoteProduct.QuoteId, new ColumnSet(QuoteEntity.Fields.OpportunityId));

                if (quote != null && quote.OpportunityId != null && quote.OpportunityId.Id != Guid.Empty)
                {
                    opportunityId = quote.OpportunityId.Id;
                }
            }
            else
            {
                if(preImage != null && !quoteProduct.IsAttributeModified(preImage,QuoteProductEntity.Fields.QuoteId) && preImage.QuoteId != null
                    && preImage.QuoteId.Id != Guid.Empty)
                {
                    QuoteEntity quote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, preImage.QuoteId, new ColumnSet(QuoteEntity.Fields.OpportunityId));

                    if (quote != null && quote.OpportunityId != null && quote.OpportunityId.Id != Guid.Empty)
                    {
                        opportunityId = quote.OpportunityId.Id;
                    }

                    quoteProduct.QuoteId = preImage.QuoteId;
                }
            }
            /* OLD
            if (preImage != null && !quoteProduct.IsAttributeModified(preImage,QuoteProductEntity.Fields.ed_FromDate))
            {
                localContext.Trace("ed_FromDate not modified");
                
                startDate = preImage.ed_FromDate;
            }
            else
            {
                fromDateModified = true;
            }

            if (preImage != null && !quoteProduct.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_ToDate))
            {
                localContext.Trace("ed_ToDate not modified");
                endDate = preImage.ed_ToDate;
            }
            else
            {
                toDateModified = true;
            }

            if(preImage != null && !quoteProduct.IsAttributeModified(preImage,QuoteProductEntity.Fields.ProductId))
            {
                localContext.Trace("ProductId not modified");
                quoteProduct.ProductId = preImage.ProductId;
            }
            */

            //validate and return the emptySlotsAvailable for this product on the Dates requested.

            //validate if unit is equal to 1 day
            /*
            if (!UnitEntity.IsOneDayUnit(localContext, quoteProduct.UoMId))
            {
                localContext.Trace("Not 1 day Unit.");
                return;
            }
            */

            if (preImage != null)
            {

                localContext.Trace("PreImage not null.");
                DateTime? preFromDate = null;
                DateTime? preToDate = null;
                DateTime? postFromDate = null;
                DateTime? postToDate = null;

                if (preImage.ed_FromDate != null)
                {
                    preFromDate = preImage.ed_FromDate.Value;
                }

                if(preImage.ed_ToDate != null)
                {
                    preToDate = preImage.ed_ToDate.Value;
                }

                if(quoteProduct.ed_FromDate != null)
                {
                    postFromDate = quoteProduct.ed_FromDate.Value;
                }
                else
                {
                    if(!quoteProduct.IsAttributeModified(preImage,QuoteProductEntity.Fields.ed_FromDate) && preImage.ed_FromDate != null)
                    {
                        postFromDate = preImage.ed_FromDate.Value;
                    }
                }

                if (quoteProduct.ed_ToDate != null)
                {
                    postToDate = quoteProduct.ed_ToDate.Value;
                }
                else
                {
                    if (!quoteProduct.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_ToDate) && preImage.ed_ToDate != null)
                    {
                        postToDate = preImage.ed_ToDate.Value;
                    }
                }
                if (postFromDate == null && postToDate != null)
                {
                    throw new InvalidPluginExecutionException("FromDate cannot be empty if ToDate is set");
                }
                else if(postFromDate != null && postToDate == null)
                {
                    throw new InvalidPluginExecutionException("ToDate cannot be empty if FromDate is set");
                }

                if (postFromDate != null && postToDate != null && postFromDate > postToDate)
                {
                    throw new InvalidPluginExecutionException("FromDate cannot be after ToDate");
                }
                
                if (preFromDate == null && preToDate == null)
                {
                    availableSlots = SlotsEntity.AvailableSlots(localContext, quoteProduct.ProductId, postFromDate.Value, postToDate.Value);
                    SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, postFromDate.Value, postToDate.Value, availableSlots, opportunityId, quoteProduct);
                }
                else if(postFromDate == null && postToDate == null)
                {
                    SlotsEntity.ReleaseSlots(localContext, quoteProduct.Id, true);
                }
                else if(DateTime.Compare(postFromDate.Value,preToDate.Value) > 0 || DateTime.Compare(preFromDate.Value, postToDate.Value) > 0)
                {
                    SlotsEntity.ReleaseSlots(localContext, quoteProduct.Id, false, preFromDate, preToDate, 1);
                    availableSlots = SlotsEntity.AvailableSlots(localContext, quoteProduct.ProductId, postFromDate.Value, postToDate.Value);
                    SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, postFromDate.Value, postToDate.Value, availableSlots,opportunityId,quoteProduct);
                }
                else
                {
                    var compareFrom = DateTime.Compare(preFromDate.Value, postFromDate.Value);
                    if (compareFrom > 0)
                    {
                        availableSlots = SlotsEntity.AvailableSlots(localContext, quoteProduct.ProductId, postFromDate.Value, preFromDate.Value.AddDays(-1));
                        SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, postFromDate.Value, preFromDate.Value.AddDays(-1), availableSlots, opportunityId, quoteProduct);
                    }
                    else if (compareFrom < 0)
                    {
                        SlotsEntity.ReleaseSlots(localContext, quoteProduct.Id, false, preFromDate.Value, postFromDate.Value.AddDays(-1), 1);
                    }

                    var compareTo = DateTime.Compare(preToDate.Value, postToDate.Value);
                    if(compareTo < 0)
                    {
                        availableSlots = SlotsEntity.AvailableSlots(localContext, quoteProduct.ProductId, preToDate.Value.AddDays(1), postToDate.Value);
                        SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, preToDate.Value.AddDays(1), postToDate.Value, availableSlots, opportunityId, quoteProduct);
                    }
                    else if(compareTo > 0)
                    {
                        SlotsEntity.ReleaseSlots(localContext, quoteProduct.Id, false, postToDate.Value.AddDays(1), preToDate.Value, 1);
                    }
                }
            }
            else
            {
                if (quoteProduct.ed_FromDate == null && quoteProduct.ed_ToDate == null)
                {
                    return;
                }
                else if (quoteProduct.ed_FromDate == null && quoteProduct.ed_ToDate != null)
                {
                    throw new InvalidPluginExecutionException("FromDate cannot be empty if ToDate is set");
                }
                else if (quoteProduct.ed_FromDate != null && quoteProduct.ed_ToDate == null)
                {
                    throw new InvalidPluginExecutionException("ToDate cannot be empty if FromDate is set");
                }
                else if(DateTime.Compare(quoteProduct.ed_FromDate.Value,quoteProduct.ed_ToDate.Value) > 0)
                {
                    throw new InvalidPluginExecutionException("FromDate cannot be after ToDate");
                }
                else
                {
                    availableSlots = SlotsEntity.AvailableSlots(localContext, quoteProduct.ProductId, quoteProduct.ed_FromDate.Value, quoteProduct.ed_ToDate.Value);
                    SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, quoteProduct.ed_FromDate.Value, quoteProduct.ed_ToDate.Value, availableSlots, opportunityId, quoteProduct);
                }
                
                
            }
        }
    }
}