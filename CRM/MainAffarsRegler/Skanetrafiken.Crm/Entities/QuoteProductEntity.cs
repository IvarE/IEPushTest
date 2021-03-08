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
                UpdateOrGenerateSlots(localContext, quoteProduct);
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

            if (preImage != null)
            {
                localContext.Trace("PreImage not null.");
                if (quoteProduct.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_FromDate))
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
                        if(quoteProduct.ed_FromDate == null)
                        {
                            removeAllSlots = true;
                        }
                    }
                }
                else
                {
                    startDate = preImage.ed_FromDate;
                }

                if (quoteProduct.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_ToDate))
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
                        if(quoteProduct.ed_ToDate == null)
                        {
                            removeAllSlots = true;
                        } 
                    }
                }
                else
                {
                    endDate = preImage.ed_ToDate;
                }

                if(removeAllSlots == true)
                {

                }
            }

            //validate if unit is equal to 1 day
            if(!UnitEntity.IsOneDayUnit(localContext, quoteProduct.UoMId))
            {
                localContext.Trace("Not 1 day Unit.");
                return;
            }

            if(DateTime.Compare(quoteProduct.ed_FromDate.Value,quoteProduct.ed_ToDate.Value) > 0)
            {
                localContext.Trace("FromDate is later than EndDate");
                return;
            }
            
            if(quoteProduct.Quantity <= 0)
            {
                localContext.Trace("Quantity is lower than 1.");
                return;
            }
            //validate and return the emptySlotsAvailable for this product on the Dates requested.
            List<SlotsEntity> availableSlots = SlotsEntity.AvailableSlots(localContext, quoteProduct.ProductId, quoteProduct.ed_FromDate.Value, quoteProduct.ed_ToDate.Value);

            if(availableSlots != null && availableSlots.Count > 0)
            {
                

            }
            else
            {

            }
        }
    }
}
