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
            if(this.IsAttributeModified(preImage,QuoteProductEntity.Fields.ed_totalslots) || this.IsAttributeModified(preImage, QuoteProductEntity.Fields.PricePerUnit))
            {
                decimal price = 0;
                int quantity = 0;
                int totalSlots = 0;

                if(!this.IsAttributeModified(preImage,QuoteProductEntity.Fields.PricePerUnit))
                {
                    price = preImage.PricePerUnit.Value;
                }
                else
                {
                    price = this.PricePerUnit.Value;
                }

                if(!this.IsAttributeModified(preImage,QuoteProductEntity.Fields.Quantity))
                {
                    quantity = (int)preImage.Quantity.Value;
                }
                else
                {
                    quantity = (int)this.Quantity.Value;
                }
                if(!this.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_totalslots))
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
            bool removeAllSlots = false;
            bool fromDateModified = false;
            bool toDateModified = false;
            List<SlotsEntity> availableSlots = null;
            DateTime? startDate = quoteProduct.ed_FromDate;
            DateTime? endDate = quoteProduct.ed_ToDate;
            DateTime? startRemoveIntervalFrom = null; //used if after Update the preImage.ed_FromDate is before the quoteProduct.ed_FromDate
            DateTime? endRemoveIntervalFrom = null; //used if after Update the preImage.ed_FromDate is before the quoteProduct.ed_FromDate
            DateTime? startRemoveIntervalTo = null; //used if after Update the preImage.ed_ToDate is after the quoteProduct.ed_ToDate
            DateTime? endRemoveIntervalTo = null; //used if after Update the preImage.ed_ToDate is after the quoteProduct.ed_ToDate
            DateTime? startCreateIntervalFrom = null;
            DateTime? endCreateIntervalFrom = null;
            DateTime? startCreateIntervalTo = null;
            DateTime? endCreateIntervalTo = null;

            if (preImage != null && !quoteProduct.IsAttributeModified(preImage,QuoteProductEntity.Fields.ed_FromDate))
            {
                localContext.Trace("ed_FromDate not modified");
                fromDateModified = true;
                quoteProduct.ed_FromDate = preImage.ed_FromDate;
            }

            if (preImage != null && !quoteProduct.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_ToDate))
            {
                localContext.Trace("ed_ToDate not modified");
                toDateModified = true;
                quoteProduct.ed_ToDate = preImage.ed_ToDate;
            }

            if(preImage != null && !quoteProduct.IsAttributeModified(preImage,QuoteProductEntity.Fields.ProductId))
            {
                localContext.Trace("ProductId not modified");
                quoteProduct.ProductId = preImage.ProductId;
            }

            //validate and return the emptySlotsAvailable for this product on the Dates requested.
            availableSlots = SlotsEntity.AvailableSlots(localContext, quoteProduct.ProductId, quoteProduct.ed_FromDate.Value, quoteProduct.ed_ToDate.Value);
            //validate if unit is equal to 1 day
            /*
            if (!UnitEntity.IsOneDayUnit(localContext, quoteProduct.UoMId))
            {
                localContext.Trace("Not 1 day Unit.");
                return;
            }
            */
            if (DateTime.Compare(startDate.Value, endDate.Value) > 0)
            {
                throw new InvalidPluginExecutionException("FromDate is later than EndDate");
            }
            if (preImage != null)
            {
                localContext.Trace("PreImage not null.");
                if (fromDateModified)
                {
                    localContext.Trace("ed_FromDate modified.");
                    startDate = quoteProduct.ed_FromDate;

                    if (preImage.ed_FromDate != null && quoteProduct.ed_FromDate != null)
                    {
                        var compareFrom = DateTime.Compare(preImage.ed_FromDate.Value, quoteProduct.ed_FromDate.Value);
                        if (compareFrom < 0)
                        {
                            startRemoveIntervalFrom = preImage.ed_FromDate;
                            endRemoveIntervalFrom = endRemoveIntervalFrom.Value.AddDays(-1);
                        }
                        else
                        {
                            startCreateIntervalFrom = quoteProduct.ed_FromDate;
                            endCreateIntervalFrom = preImage.ed_FromDate.Value.AddDays(-1);
                        }
                    }
                    else
                    {
                        if (quoteProduct.ed_FromDate == null)
                        {
                            removeAllSlots = true;
                        }
                    }
                }
                else
                {
                    startDate = preImage.ed_FromDate;
                }
                if (toDateModified)
                {
                    endDate = quoteProduct.ed_ToDate;
                    if (preImage.ed_ToDate != null && quoteProduct.ed_ToDate != null)
                    {
                        var compareTo = DateTime.Compare(preImage.ed_ToDate.Value, quoteProduct.ed_ToDate.Value);
                        if (compareTo < 0)
                        {
                            startCreateIntervalTo = preImage.ed_ToDate.Value.AddDays(1);
                            endCreateIntervalTo = quoteProduct.ed_ToDate.Value;
                        }
                        else
                        {
                            startRemoveIntervalTo = quoteProduct.ed_ToDate.Value.AddDays(1);
                            endRemoveIntervalTo = preImage.ed_ToDate;
                        }
                    }
                    else
                    {
                        if (quoteProduct.ed_ToDate == null)
                        {
                            removeAllSlots = true;
                        }
                    }
                }
                else
                {
                    endDate = preImage.ed_ToDate;
                }
                if (removeAllSlots == true)
                {
                    SlotsEntity.ReleaseSlots(localContext, quoteProduct.Id, true);
                    return;
                }
                else
                {
                    if (startRemoveIntervalFrom != null && endRemoveIntervalFrom != null)
                    {
                        SlotsEntity.ReleaseSlots(localContext, quoteProduct.Id, false, startRemoveIntervalFrom, endRemoveIntervalFrom);
                    }
                    if (startRemoveIntervalTo != null && endRemoveIntervalTo != null)
                    {
                        SlotsEntity.ReleaseSlots(localContext, quoteProduct.Id, false, startRemoveIntervalTo, endRemoveIntervalTo);
                    }
                    if (startCreateIntervalFrom != null && endCreateIntervalFrom != null)
                    {
                        availableSlots = SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, startCreateIntervalFrom.Value, endCreateIntervalFrom.Value, availableSlots, null, quoteProduct);
                    }
                    if (startCreateIntervalTo != null && endCreateIntervalTo != null)
                    {
                        availableSlots = SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, startCreateIntervalTo.Value, endCreateIntervalTo.Value, availableSlots, null, quoteProduct);
                    }
                }
                //availableSlots = SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, startDate.Value, endDate.Value, availableSlots, null, quoteProduct);
            }
            else
            {
                availableSlots = SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, startDate.Value, endDate.Value, availableSlots, null, quoteProduct);
            }
        }
    }
}