
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Runtime;
using System.Globalization;

namespace Skanetrafiken.Crm.Entities
{
    public class SlotsEntity : Generated.ed_Slots
    {
        [DataContract]
        public class GenerateSlotsResponse
        {
            [DataMember]
            public bool OK { get; set; }

            [DataMember]
            public string Message { get; set; }
        }

        public static void HandleSlotsEntityCreate(Plugin.LocalPluginContext localContext, SlotsEntity target)
        {
            if (target.ed_OrderProductID != null || target.ed_QuoteProductID != null)
            {
                SlotsEntity.SlotsEntityUpdateQuantity(localContext, target);
            }
        }
        public static void HandleSlotsEntityUpdate(Plugin.LocalPluginContext localContext, SlotsEntity target, SlotsEntity preImage)
        {
            if (target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_OrderProductID) || target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_QuoteProductID))
            {
                SlotsEntity.SlotsEntityUpdateQuantity(localContext, target, preImage);
            }
        }
        public static List<SlotsEntity> AvailableSlots (Plugin.LocalPluginContext localContext,EntityReference productER,DateTime starDate,DateTime endDate)
        {
            localContext.Trace("Inside Available Slots");

            if (productER == null || productER.Id == Guid.Empty || starDate == null || endDate == null)
            {
                localContext.Trace("required Attributes Null");
                return null;
            }
            QueryExpression queryAvailableSlots = new QueryExpression();

            
            queryAvailableSlots.EntityName = SlotsEntity.EntityLogicalName;
            queryAvailableSlots.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_BookingDay,SlotsEntity.Fields.ed_StandardPrice,SlotsEntity.Fields.ed_CustomPrice);

            FilterExpression filterExpression = new FilterExpression();
            filterExpression.FilterOperator = LogicalOperator.And;

            filterExpression.AddCondition(SlotsEntity.Fields.ed_ProductID, ConditionOperator.Equal, productER.Id);

            filterExpression.AddCondition(SlotsEntity.Fields.ed_BookingDay, ConditionOperator.OnOrAfter, starDate);
            filterExpression.AddCondition(SlotsEntity.Fields.ed_BookingDay, ConditionOperator.OnOrBefore, endDate);

            filterExpression.AddCondition(SlotsEntity.Fields.ed_Quote, ConditionOperator.Null);
            filterExpression.AddCondition(SlotsEntity.Fields.ed_QuoteProductID, ConditionOperator.Null);
            filterExpression.AddCondition(SlotsEntity.Fields.ed_Order, ConditionOperator.Null);
            filterExpression.AddCondition(SlotsEntity.Fields.ed_OrderProductID, ConditionOperator.Null);

            queryAvailableSlots.Criteria.AddFilter(filterExpression);

            queryAvailableSlots.AddOrder(SlotsEntity.Fields.ed_BookingDay, OrderType.Ascending);

            List<SlotsEntity> slotsEntities = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, queryAvailableSlots);
            slotsEntities.GroupBy(x => x.ed_BookingDay).Select(g => g.OrderBy(x => x.ed_BookingDay).First());

            localContext.Trace("Returning available slotsEntities");

            return slotsEntities;
        }

        public static GenerateSlotsResponse GenerateSlots (Plugin.LocalPluginContext localContext, Guid productId, int quantityPerDay, DateTime startDate, DateTime endDate,Guid? OpportunityGuid = null, QuoteProductEntity quoteProduct = null, OrderProductEntity orderProduct = null)
        {
            localContext.Trace("Inside GenerateSlots.");
            string productName = "";

            GenerateSlotsResponse response = new GenerateSlotsResponse();

            if (endDate < startDate)
            {
                response.OK = false;
                response.Message = "Start date is higher than the End date.";
                return response;
            }

            FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            if(feature == null || feature.ed_bookingsystem == null || feature.ed_bookingsystem == false)
            {
                response.OK = false;
                response.Message = "Booking System not enabled on this environment.";
                return response;
            }

            ColumnSet productColumns = new ColumnSet(false);
            productColumns.AddColumn(ProductEntity.Fields.Name);

            Money priceProduct = new Money(0);
            
            ProductEntity product = XrmRetrieveHelper.Retrieve<ProductEntity>(localContext, productId, productColumns);

            if (product == null)
            {
                response.OK = false;
                response.Message = "Product not found.";
                return response;
            }
            else
            {
                if(product.Contains(ProductEntity.Fields.Name) && !string.IsNullOrEmpty(product.Name))
                {
                    productName = product.Name;
                }
            }
            QueryExpression queryProductPriceLevel = new QueryExpression();
            queryProductPriceLevel.EntityName = ProductPriceLevelEntity.EntityLogicalName;
            queryProductPriceLevel.ColumnSet = new ColumnSet(ProductPriceLevelEntity.Fields.Amount);
            queryProductPriceLevel.Criteria.AddCondition(ProductPriceLevelEntity.Fields.ProductId, ConditionOperator.Equal, productId);
            
            LinkEntity linkEntity = new LinkEntity()
            {
                EntityAlias = UnitEntity.EntityLogicalName,
                LinkFromEntityName = ProductPriceLevelEntity.EntityLogicalName,
                LinkFromAttributeName = ProductPriceLevelEntity.Fields.UoMId,
                LinkToEntityName = UnitEntity.EntityLogicalName,
                LinkToAttributeName = UnitEntity.Fields.UoMId

            };
            linkEntity.LinkCriteria.AddCondition(UnitEntity.Fields.Name, ConditionOperator.Like, "1 dag");

            queryProductPriceLevel.LinkEntities.Add(linkEntity);
            ProductPriceLevelEntity productPL = XrmRetrieveHelper.RetrieveFirst<ProductPriceLevelEntity>(localContext, queryProductPriceLevel);

            if(productPL != null && productPL.Contains(ProductPriceLevelEntity.Fields.Amount) && productPL.Amount != null)
            {
                priceProduct = productPL.Amount;
            }
            else
            {
                response.OK = false;
                response.Message = "Product not found or price not defined.";
                return response;
            }

            do
            {
                var i = 0;
                for(i = 0; i < quantityPerDay; i++)
                {
                    SlotsEntity slot = new SlotsEntity();
                    string date = startDate.ToString("dd/MM/yyyy");

                    slot.ed_name = productName + " - " + date;
                    slot.ed_BookingDay = startDate;
                    slot.ed_StandardPrice = priceProduct;
                    slot.ed_Extended = false;
                    slot.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Open;
                    slot.ed_ProductID = new EntityReference(ProductEntity.EntityLogicalName, productId);

                    if(quoteProduct != null)
                    {
                        slot.ed_QuoteProductID = quoteProduct.ToEntityReference();

                        if(quoteProduct.QuoteId != null && quoteProduct.QuoteId.Id != Guid.Empty)
                        {
                            slot.ed_Quote = quoteProduct.QuoteId;
                        }
                    }
                    if(orderProduct != null)
                    {

                    }
                    if(OpportunityGuid != null)
                    {
                    }
                    XrmHelper.Create(localContext, slot);
                }
                

                startDate = startDate.AddDays(1);
            }
            while (DateTime.Compare(startDate,endDate) <= 0);

            response.OK = true;
            response.Message = "Slots created.";
            return response;
        }
        public static void GenerateSlotsInternal(Plugin.LocalPluginContext localContext, Guid productId, int quantityPerDay, DateTime startDate, DateTime endDate, List<SlotsEntity> availableSlots = null, Guid? OpportunityGuid = null, QuoteProductEntity quoteProduct = null, OrderProductEntity orderProduct = null)
        {
            localContext.Trace("Inside GenerateSlotsInternal");
            string productName = "";
            GenerateSlotsResponse response = new GenerateSlotsResponse();
            if (endDate < startDate)
            {
                localContext.Trace("Start date is higher than the End date.");
                //return availableSlots;
            }
            ColumnSet productColumns = new ColumnSet(false);
            productColumns.AddColumn(ProductEntity.Fields.Name);
            Money priceProduct = new Money(0);
            ProductEntity product = XrmRetrieveHelper.Retrieve<ProductEntity>(localContext, productId, productColumns);
            if (product == null)
            {
                localContext.Trace("Product not found.");
                return;
                //return availableSlots;
            }
            else
            {
                if (product.Contains(ProductEntity.Fields.Name) && !string.IsNullOrEmpty(product.Name))
                {
                    productName = product.Name;
                }
            }
            QueryExpression queryProductPriceLevel = new QueryExpression();
            queryProductPriceLevel.EntityName = ProductPriceLevelEntity.EntityLogicalName;
            queryProductPriceLevel.ColumnSet = new ColumnSet(ProductPriceLevelEntity.Fields.Amount);
            queryProductPriceLevel.Criteria.AddCondition(ProductPriceLevelEntity.Fields.ProductId, ConditionOperator.Equal, productId);
            LinkEntity linkEntity = new LinkEntity()
            {
                EntityAlias = UnitEntity.EntityLogicalName,
                LinkFromEntityName = ProductPriceLevelEntity.EntityLogicalName,
                LinkFromAttributeName = ProductPriceLevelEntity.Fields.UoMId,
                LinkToEntityName = UnitEntity.EntityLogicalName,
                LinkToAttributeName = UnitEntity.Fields.UoMId
            };
            linkEntity.LinkCriteria.AddCondition(UnitEntity.Fields.Name, ConditionOperator.Like, "1 dag");
            queryProductPriceLevel.LinkEntities.Add(linkEntity);
            ProductPriceLevelEntity productPL = XrmRetrieveHelper.RetrieveFirst<ProductPriceLevelEntity>(localContext, queryProductPriceLevel);
            if (productPL != null && productPL.Contains(ProductPriceLevelEntity.Fields.Amount) && productPL.Amount != null)
            {
                priceProduct = productPL.Amount;
            }
            else
            {
                localContext.Trace("Product not found or price not defined.");
                //return availableSlots;
                return;
            }
            do
            {
                List<SlotsEntity> filteredSlots = null;
                if (availableSlots != null && availableSlots.Count > 0)
                {
                    filteredSlots = availableSlots.Where(filteredSlot => filteredSlot.ed_BookingDay == startDate).ToList();
                }
                var i = 0;
                for (i = 0; i < quantityPerDay; i++)
                {
                    SlotsEntity slot = new SlotsEntity();
                    if (filteredSlots != null && filteredSlots.Count > 0)
                    {
                        slot.Id = filteredSlots[0].Id;
                        if (quoteProduct != null)
                        {
                            if (quoteProduct.QuoteId != null && quoteProduct.QuoteId.Id != Guid.Empty)
                            {
                                slot.ed_Quote = quoteProduct.QuoteId;
                            }

                            if (quoteProduct.Id != null && quoteProduct.Id != Guid.Empty)
                            {
                                slot.ed_QuoteProductID = quoteProduct.ToEntityReference();
                                slot.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Preliminary;
                            }
                        }
                        if (orderProduct != null)
                        {
                            if(orderProduct.SalesOrderId != null && orderProduct.SalesOrderId.Id != Guid.Empty)
                            {
                                slot.ed_Order = orderProduct.SalesOrderId;
                            }
                            if(orderProduct.Id != null && orderProduct.Id != Guid.Empty)
                            {
                                slot.ed_OrderProductID = orderProduct.ToEntityReference();
                                slot.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Preliminary;
                            }
                        }
                        if (OpportunityGuid != null)
                        {
                            slot.ed_Opportunity = new EntityReference(OpportunityEntity.EntityLogicalName, OpportunityGuid.Value);
                        }
                        
                        XrmHelper.Update(localContext, slot);

                        var objectToRemove = filteredSlots[0];
                        filteredSlots.Remove(objectToRemove);
                        availableSlots.Remove(objectToRemove);
                    }
                    else
                    {
                        string date = startDate.ToString("dd/MM/yyyy");
                        slot.ed_name = productName + " - " + date;
                        slot.ed_BookingDay = startDate;
                        slot.ed_StandardPrice = priceProduct;
                        slot.ed_ProductID = new EntityReference(ProductEntity.EntityLogicalName, productId);
                        
                        if (quoteProduct != null)
                        {
                            if (quoteProduct.QuoteId != null && quoteProduct.QuoteId.Id != Guid.Empty)
                            {
                                slot.ed_Quote = quoteProduct.QuoteId;
                            }

                            if (quoteProduct.Id != null && quoteProduct.Id != Guid.Empty)
                            {
                                slot.ed_QuoteProductID = quoteProduct.ToEntityReference();
                                slot.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Preliminary;
                            }
                        }
                        if (orderProduct != null)
                        {
                            if (orderProduct.SalesOrderId != null && orderProduct.SalesOrderId.Id != Guid.Empty)
                            {
                                slot.ed_Order = orderProduct.SalesOrderId;
                            }
                            if (orderProduct.Id != null && orderProduct.Id != Guid.Empty)
                            {
                                slot.ed_OrderProductID = orderProduct.ToEntityReference();
                                slot.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Preliminary;
                            }
                        }
                        if (OpportunityGuid != null)
                        {
                            slot.ed_Opportunity = new EntityReference(OpportunityEntity.EntityLogicalName, OpportunityGuid.Value);
                        }
                        XrmHelper.Create(localContext, slot);
                    }
                }

                startDate = startDate.AddDays(1);
            }
            while (DateTime.Compare(startDate, endDate) <= 0);
//return availableSlots;
        }

        public static void UpdateSlotsInternal(Plugin.LocalPluginContext localContext, Guid productId)
        {
            // Will be used when the order is created to update the slots with Order and OrderProduct info
        }
        public static void ReleaseSlots(Plugin.LocalPluginContext localContext, Guid quoteProduct, bool deleteAll, DateTime? startDate = null, DateTime? endDate = null, int? quantity = null)
        {
            localContext.Trace("Inside ReleaseSlots");

            QueryExpression query = new QueryExpression();
            query.EntityName = SlotsEntity.EntityLogicalName;
            query.ColumnSet.AddColumns(SlotsEntity.Fields.ed_BookingDay,SlotsEntity.Fields.ed_Extended);
            FilterExpression filter = new FilterExpression();
            filter.FilterOperator = LogicalOperator.And;
            filter.AddCondition(SlotsEntity.Fields.ed_QuoteProductID, ConditionOperator.Equal, quoteProduct);
            if (startDate != null && endDate != null && DateTime.Compare(startDate.Value, endDate.Value) <= 0)
            {
                filter.AddCondition(SlotsEntity.Fields.ed_BookingDay, ConditionOperator.OnOrAfter, startDate.Value);
                filter.AddCondition(SlotsEntity.Fields.ed_BookingDay, ConditionOperator.OnOrBefore, endDate.Value);
            }
            query.Criteria.AddFilter(filter);
            query.AddOrder(SlotsEntity.Fields.ed_BookingDay, OrderType.Ascending);
            List<SlotsEntity> slotsList = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, query);

            if (slotsList != null && slotsList.Count > 0)
            {
                DateTime? currentDate = null;
                int releasedQuantity = 0;
                foreach (SlotsEntity slot in slotsList)
                {
                    SlotsEntity slotToRelease = new SlotsEntity();
                    slotToRelease.Id = slot.Id;
                    if (deleteAll)
                    {
                        slotToRelease.ed_QuoteProductID = null;
                        slotToRelease.ed_Quote = null;
                        slotToRelease.ed_OrderProductID = null;
                        slotToRelease.ed_Order = null;
                        slotToRelease.ed_Opportunity = null;
                        //Validate generatedFromProduct bool or defaultBookingStatus
                        if(slot.ed_Extended != null && slot.ed_Extended.Value == true)
                        {
                            slotToRelease.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Extended;
                        }
                        else
                        {
                            slotToRelease.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Open;
                        }
                        
                        XrmHelper.Update(localContext, slotToRelease);
                        localContext.Trace("Released Slot Booked for: " + slot.ed_BookingDay.Value.ToString("g", CultureInfo.GetCultureInfo("sv-SE")));
                    }
                    else
                    {
                        if (currentDate == null)
                        {
                            currentDate = slot.ed_BookingDay;
                        }
                        else if (currentDate != slot.ed_BookingDay)
                        {
                            currentDate = slot.ed_BookingDay;
                            releasedQuantity = 0;
                        }
                        if (releasedQuantity < quantity)
                        {
                            slotToRelease.ed_QuoteProductID = null;
                            slotToRelease.ed_Quote = null;
                            slotToRelease.ed_OrderProductID = null;
                            slotToRelease.ed_Order = null;
                            slotToRelease.ed_Opportunity = null;

                            //Validate generatedFromProduct bool or defaultBookingStatus

                            if (slot.ed_Extended != null && slot.ed_Extended.Value == true)
                            {
                                slotToRelease.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Extended;
                            }
                            else
                            {
                                slotToRelease.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Open;
                            }

                            XrmHelper.Update(localContext, slotToRelease);
                            localContext.Trace("Released Slot Booked for: " + slot.ed_BookingDay.Value.ToString("g", CultureInfo.GetCultureInfo("sv-SE")));
                            releasedQuantity++;
                        }
                    }

                }
            }
        }

        public static void SlotsEntityUpdateQuantity(Plugin.LocalPluginContext localContext, SlotsEntity target)
        {
            var numberSlots = 0;
            //Update Quantity in OrderProduct
            QueryExpression query = new QueryExpression();
            query.EntityName = SlotsEntity.EntityLogicalName;
            query.ColumnSet = new ColumnSet(false);
            FilterExpression filter = new FilterExpression();
            filter.FilterOperator = LogicalOperator.And;
            filter.AddCondition(SlotsEntity.Fields.ed_SlotsId, ConditionOperator.NotEqual, target.Id);
            if (target.ed_OrderProductID != null && target.ed_OrderProductID.Id != Guid.Empty)
            {
                filter.AddCondition(SlotsEntity.Fields.ed_OrderProductID, ConditionOperator.Equal, target.ed_OrderProductID.Id);
            }
            else if (target.ed_QuoteProductID != null && target.ed_QuoteProductID.Id != Guid.Empty)
            {
                filter.AddCondition(SlotsEntity.Fields.ed_QuoteProductID, ConditionOperator.Equal, target.ed_QuoteProductID.Id);
            }

            query.Criteria.AddFilter(filter);
            List<SlotsEntity> slotsEntities = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, query);
            if (slotsEntities != null && slotsEntities.Count > 0)
            {
                numberSlots = slotsEntities.Count;
            }
            numberSlots = numberSlots + 1;

            if (target.ed_OrderProductID != null && target.ed_OrderProductID.Id != Guid.Empty)
            {
                OrderProductEntity orderProductToUpdate = new OrderProductEntity();
                orderProductToUpdate.Id = target.ed_OrderProductID.Id;
                orderProductToUpdate.ed_totalslots = numberSlots;
                XrmHelper.Update(localContext, orderProductToUpdate);
            }
            else if (target.ed_QuoteProductID != null && target.ed_QuoteProductID.Id != Guid.Empty)
            {
                QuoteProductEntity quoteProductToUpdate = new QuoteProductEntity();
                quoteProductToUpdate.Id = target.ed_QuoteProductID.Id;
                quoteProductToUpdate.ed_totalslots = numberSlots;
                XrmHelper.Update(localContext, quoteProductToUpdate);
            }
        }
        public static void SlotsEntityUpdateQuantity(Plugin.LocalPluginContext localContext, SlotsEntity target, SlotsEntity preImage)
        {
            localContext.Trace("Inside SlotsEntityUpdateQuantity.");

            localContext.Trace("PreImage Slot");
            preImage.Trace(localContext.TracingService);
            localContext.Trace("_____________");

            localContext.Trace("Target Slot");
            target.Trace(localContext.TracingService);
            localContext.Trace("_____________");

            var totalSlots = 0;
            bool removeCurrentRecordQuotePreImage = false;

            bool removeCurrentRecordOrderPreImage = false;

            bool addCurrentRecordQuote = false;
            bool addCurrentRecordOrder = false;

            bool preQuoteActive = false;
            bool targetQuoteActive = false;

            bool preOrderActive = false;
            bool targetOrderActive = false;

            if(!target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_Quote))
            {
                target.ed_Quote = preImage.ed_Quote;
            }

            if (!target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_Order))
            {
                target.ed_Order = preImage.ed_Order;
            }


            //Update Quantity in OrderProduct
            if (target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_QuoteProductID))
            {
                localContext.Trace("QuoteProduct Modified.");
                if (preImage.ed_QuoteProductID != null && target.ed_QuoteProductID != null)
                {
                    localContext.Trace("ed_QuoteProductID not null on PreImage and Target");
                    //need To Increase Number of Slots On Quote Product target
                    //need To Decrease Number of Slots On Quote Product preImage
                    
                    

                    if(preImage.ed_Quote != null && preImage.ed_Quote.Id != Guid.Empty)
                    {
                        QuoteEntity preQuote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, preImage.ed_Quote, new ColumnSet(QuoteEntity.Fields.StateCode, QuoteEntity.Fields.StatusCode));

                        if(preQuote != null && preQuote.StateCode != Generated.QuoteState.Closed && preQuote.StateCode != Generated.QuoteState.Won)
                        {
                            preQuoteActive = true;
                        }
                    }
                    

                    if (target.ed_Quote != null && target.ed_Quote.Id != Guid.Empty)
                    {
                        QuoteEntity targetQuote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, target.ed_Quote, new ColumnSet(QuoteEntity.Fields.StateCode, QuoteEntity.Fields.StatusCode));

                        if (targetQuote != null && targetQuote.StateCode != Generated.QuoteState.Closed && targetQuote.StateCode != Generated.QuoteState.Won)
                        {
                            targetQuoteActive = true;
                        }
                    }

                    addCurrentRecordQuote = true;
                    removeCurrentRecordQuotePreImage = true;

                }
                else if (preImage.ed_QuoteProductID != null && target.ed_QuoteProductID == null)
                {
                    localContext.Trace("ed_QuoteProductID not null on PreImage but null on Target");
                    //need To Decrease Number of Slots On Quote Product preImage
                    if (preImage.ed_Quote != null && preImage.ed_Quote.Id != Guid.Empty)
                    {
                        QuoteEntity preQuote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, preImage.ed_Quote, new ColumnSet(QuoteEntity.Fields.StateCode, QuoteEntity.Fields.StatusCode));

                        if (preQuote != null && preQuote.StateCode != Generated.QuoteState.Closed && preQuote.StateCode != Generated.QuoteState.Won)
                        {
                            preQuoteActive = true;
                        }
                    }

                    removeCurrentRecordQuotePreImage = true;
                }
                else if (preImage.ed_QuoteProductID == null && target.ed_QuoteProductID != null)
                {
                    localContext.Trace("ed_QuoteProductID null on PreImage but not null on Target");
                    //need To Increase Number of Slots On Quote Product target
                    

                    if (target.ed_Quote != null && target.ed_Quote.Id != Guid.Empty)
                    {
                        QuoteEntity targetQuote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, target.ed_Quote, new ColumnSet(QuoteEntity.Fields.StateCode, QuoteEntity.Fields.StatusCode));

                        if (targetQuote != null && targetQuote.StateCode != Generated.QuoteState.Closed && targetQuote.StateCode != Generated.QuoteState.Won)
                        {
                            targetQuoteActive = true;
                        }
                    }
                    addCurrentRecordQuote = true;
                }
            }

            if (target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_OrderProductID))
            {
                localContext.Trace("OrderProduct Modified.");
                if (preImage.ed_OrderProductID != null && target.ed_OrderProductID != null)
                {
                    localContext.Trace("ed_OrderProductID not null on PreImage and Target");
                    //need To Increase Number of Slots On Order Product target
                    //need To Decrease Number of Slots On Order Product preImage

                    if (preImage.ed_Order != null && preImage.ed_Order.Id != Guid.Empty)
                    {
                        OrderEntity preOrder = XrmRetrieveHelper.Retrieve<OrderEntity>(localContext, preImage.ed_Order, new ColumnSet(OrderEntity.Fields.StateCode, OrderEntity.Fields.StatusCode));

                        if (preOrder != null && (int)preOrder.StateCode.Value != (int)Generated.ed_SalesOrderState.Inactive)
                        {
                            preOrderActive = true;
                        }
                    }


                    if (target.ed_Order != null && target.ed_Order.Id != Guid.Empty)
                    {
                        OrderEntity targetOrder = XrmRetrieveHelper.Retrieve<OrderEntity>(localContext, target.ed_Order, new ColumnSet(OrderEntity.Fields.StateCode, OrderEntity.Fields.StatusCode));

                        if (targetOrder != null && (int)targetOrder.StateCode.Value != (int)Generated.ed_SalesOrderState.Inactive)
                        {
                            targetOrderActive = true;
                        }
                    }

                    addCurrentRecordOrder = true;
                    removeCurrentRecordOrderPreImage = true;
                }
                else if(preImage.ed_OrderProductID != null && target.ed_OrderProductID == null)
                {
                    localContext.Trace("ed_OrderProductID not null on PreImage but null on Target");
                    //need To Decrease Number of Slots On Order Product preImage
                    if (preImage.ed_Order != null && preImage.ed_Order.Id != Guid.Empty)
                    {
                        OrderEntity preOrder = XrmRetrieveHelper.Retrieve<OrderEntity>(localContext, preImage.ed_Order, new ColumnSet(OrderEntity.Fields.StateCode, OrderEntity.Fields.StatusCode));

                        if (preOrder != null && (int)preOrder.StateCode.Value != (int)Generated.ed_SalesOrderState.Inactive)
                        {
                            preOrderActive = true;
                        }
                    }
                    removeCurrentRecordOrderPreImage = true;
                }
                else if(preImage.ed_OrderProductID == null && target.ed_OrderProductID != null)
                {
                    localContext.Trace("ed_OrderProductID null on PreImage but not null on Target");
                    //need To Increase Number of Slots On Order Product target

                    if (target.ed_Order != null && target.ed_Order.Id != Guid.Empty)
                    {
                        OrderEntity targetOrder = XrmRetrieveHelper.Retrieve<OrderEntity>(localContext, target.ed_Order, new ColumnSet(OrderEntity.Fields.StateCode, OrderEntity.Fields.StatusCode));

                        if (targetOrder != null && (int)targetOrder.StateCode.Value != (int)Generated.ed_SalesOrderState.Inactive)
                        {
                            targetOrderActive = true;
                        }
                    }
                    addCurrentRecordOrder = true;
                }
            }
            
            if(addCurrentRecordQuote && targetQuoteActive == true)
            {
                localContext.Trace("addCurrentRecordQuote");
                totalSlots = 0;

                QueryExpression querySlotsProductsQuoteAdd = new QueryExpression();
                querySlotsProductsQuoteAdd.EntityName = SlotsEntity.EntityLogicalName;
                querySlotsProductsQuoteAdd.ColumnSet = new ColumnSet(false);

                FilterExpression filterSlotsProductsQuoteAdd = new FilterExpression();
                filterSlotsProductsQuoteAdd.FilterOperator = LogicalOperator.And;
                filterSlotsProductsQuoteAdd.AddCondition(SlotsEntity.Fields.ed_QuoteProductID, ConditionOperator.Equal, target.ed_QuoteProductID.Id);
                filterSlotsProductsQuoteAdd.AddCondition(SlotsEntity.Fields.Id, ConditionOperator.NotEqual, target.Id);

                querySlotsProductsQuoteAdd.Criteria.AddFilter(filterSlotsProductsQuoteAdd);

                List<SlotsEntity> slotsProductsQuoteAdd = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlotsProductsQuoteAdd);

                if (slotsProductsQuoteAdd != null && slotsProductsQuoteAdd.Count > 0)
                {
                    localContext.Trace("slotsProductsQuoteAdd not null and greater than 0");
                    totalSlots = slotsProductsQuoteAdd.Count + 1;
                }
                else
                {
                    totalSlots = 1;
                }

                QuoteProductEntity quoteProductToAddUpdate = new QuoteProductEntity();
                quoteProductToAddUpdate.Id = target.ed_QuoteProductID.Id;
                quoteProductToAddUpdate.ed_totalslots = totalSlots;
                XrmHelper.Update(localContext, quoteProductToAddUpdate);
            }

            if(removeCurrentRecordQuotePreImage && preQuoteActive == true)
            {
                localContext.Trace("removeCurrentRecordQuotePreImage");

                totalSlots = 0;

                QueryExpression querySlotsProductsQuoteRemove = new QueryExpression();
                querySlotsProductsQuoteRemove.EntityName = SlotsEntity.EntityLogicalName;
                querySlotsProductsQuoteRemove.ColumnSet = new ColumnSet(false);

                FilterExpression filterSlotsProductsQuoteRemove = new FilterExpression();
                filterSlotsProductsQuoteRemove.FilterOperator = LogicalOperator.And;
                filterSlotsProductsQuoteRemove.AddCondition(SlotsEntity.Fields.ed_QuoteProductID, ConditionOperator.Equal, preImage.ed_QuoteProductID.Id);
                filterSlotsProductsQuoteRemove.AddCondition(SlotsEntity.Fields.Id, ConditionOperator.NotEqual, target.Id);

                querySlotsProductsQuoteRemove.Criteria.AddFilter(filterSlotsProductsQuoteRemove);

                List<SlotsEntity> slotsProductsQuoteRemove = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlotsProductsQuoteRemove);

                if (slotsProductsQuoteRemove != null && slotsProductsQuoteRemove.Count > 0)
                {
                    localContext.Trace("slotsProductsQuoteRemove not null and greater than 0");

                    totalSlots = slotsProductsQuoteRemove.Count;
                }

                QuoteProductEntity quoteProductToRemoveUpdate = new QuoteProductEntity();
                quoteProductToRemoveUpdate.Id = preImage.ed_QuoteProductID.Id;
                quoteProductToRemoveUpdate.ed_totalslots = totalSlots;
                XrmHelper.Update(localContext, quoteProductToRemoveUpdate);
            }

            if (addCurrentRecordOrder && targetOrderActive == true)
            {
                localContext.Trace("addCurrentRecordOrder");


                totalSlots = 0;

                QueryExpression querySlotsProductsOrderAdd = new QueryExpression();
                querySlotsProductsOrderAdd.EntityName = SlotsEntity.EntityLogicalName;
                querySlotsProductsOrderAdd.ColumnSet = new ColumnSet(false);

                FilterExpression filterSlotsProductsOrderAdd = new FilterExpression();
                filterSlotsProductsOrderAdd.FilterOperator = LogicalOperator.And;
                filterSlotsProductsOrderAdd.AddCondition(SlotsEntity.Fields.ed_OrderProductID, ConditionOperator.Equal, target.ed_OrderProductID.Id);
                filterSlotsProductsOrderAdd.AddCondition(SlotsEntity.Fields.Id, ConditionOperator.NotEqual, target.Id);

                querySlotsProductsOrderAdd.Criteria.AddFilter(filterSlotsProductsOrderAdd);

                List<SlotsEntity> slotsProductsOrderAdd = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlotsProductsOrderAdd);
                
                if (slotsProductsOrderAdd != null && slotsProductsOrderAdd.Count > 0)
                {
                    localContext.Trace("slotsProductsOrderAdd not null and greater than 0");

                    totalSlots = slotsProductsOrderAdd.Count + 1;
                }
                else
                {
                    totalSlots = 1;
                }

                OrderProductEntity orderProductToAddUpdate = new OrderProductEntity();
                orderProductToAddUpdate.Id = target.ed_OrderProductID.Id;
                orderProductToAddUpdate.ed_totalslots = totalSlots;
                XrmHelper.Update(localContext, orderProductToAddUpdate);
            }

            if (removeCurrentRecordOrderPreImage && preOrderActive == true)
            {
                localContext.Trace("removeCurrentRecordOrderPreImage");

                totalSlots = 0;

                QueryExpression querySlotsProductsOrderRemove = new QueryExpression();
                querySlotsProductsOrderRemove.EntityName = SlotsEntity.EntityLogicalName;
                querySlotsProductsOrderRemove.ColumnSet = new ColumnSet(false);

                FilterExpression filterSlotsProductsOrderRemove = new FilterExpression();
                filterSlotsProductsOrderRemove.FilterOperator = LogicalOperator.And;
                filterSlotsProductsOrderRemove.AddCondition(SlotsEntity.Fields.ed_OrderProductID, ConditionOperator.Equal, preImage.ed_OrderProductID.Id);
                filterSlotsProductsOrderRemove.AddCondition(SlotsEntity.Fields.Id, ConditionOperator.NotEqual, target.Id);

                querySlotsProductsOrderRemove.Criteria.AddFilter(filterSlotsProductsOrderRemove);

                List<SlotsEntity> slotsProductsOrderRemove = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlotsProductsOrderRemove);
                

                if (slotsProductsOrderRemove != null && slotsProductsOrderRemove.Count > 0)
                {
                    localContext.Trace("slotsProductsOrderRemove not null and greater than 0");
                    totalSlots = slotsProductsOrderRemove.Count;
                }
                OrderProductEntity orderProductToRemoveUpdate = new OrderProductEntity();
                orderProductToRemoveUpdate.Id = preImage.ed_OrderProductID.Id;
                orderProductToRemoveUpdate.ed_totalslots = totalSlots;
                XrmHelper.Update(localContext, orderProductToRemoveUpdate);
            }
        }
    }
}
