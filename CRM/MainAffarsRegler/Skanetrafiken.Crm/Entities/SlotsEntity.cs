
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
using System.Net;
using System.Activities;
using System.IO;
using System.Web;

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

        public void HandlePreSlotsEntityCreate(Plugin.LocalPluginContext localContext)
        {
            if(this.ed_Opportunity != null && this.ed_Opportunity.Id != Guid.Empty)
            {
                OpportunityEntity opportunity = XrmRetrieveHelper.Retrieve<OpportunityEntity>(localContext, this.ed_Opportunity, new ColumnSet(OpportunityEntity.Fields.ParentAccountId));

                if(opportunity != null && opportunity.ParentAccountId != null && opportunity.ParentAccountId.Id != Guid.Empty)
                {
                    this.ed_Account = new EntityReference(AccountEntity.EntityLogicalName, opportunity.ParentAccountId.Id);
                }
            }
        }

        public void HandlePreSlotsEntityUpdate(Plugin.LocalPluginContext localContext,  SlotsEntity preImage)
        {
            if(this.IsAttributeModified(preImage,SlotsEntity.Fields.ed_Opportunity))
            {
                if(this.ed_Opportunity != null && this.ed_Opportunity.Id != Guid.Empty)
                {
                    OpportunityEntity opportunity = XrmRetrieveHelper.Retrieve<OpportunityEntity>(localContext, this.ed_Opportunity, new ColumnSet(OpportunityEntity.Fields.ParentAccountId));

                    if (opportunity != null && opportunity.ParentAccountId != null && opportunity.ParentAccountId.Id != Guid.Empty)
                    {
                        this.ed_Account = new EntityReference(AccountEntity.EntityLogicalName, opportunity.ParentAccountId.Id);
                    }
                    else
                    {
                        this.ed_Account = null;
                    }
                }
                else
                {
                    this.ed_Account = null;
                }
            }

            EntityReference QuoteProductER = null;
            EntityReference OrderProductER = null;
            Money standardPrice = new Money(0);
            if(!this.IsAttributeModified(preImage,SlotsEntity.Fields.ed_QuoteProductID))
            {
                QuoteProductER = preImage.ed_QuoteProductID;
            }
            else
            {
                QuoteProductER = this.ed_QuoteProductID;
            }

            if(!this.IsAttributeModified(preImage, SlotsEntity.Fields.ed_QuoteProductID))
            {
                OrderProductER = preImage.ed_OrderProductID;
            }
            else
            {
                OrderProductER = this.ed_OrderProductID;
            }

            if(QuoteProductER == null && OrderProductER == null)
            {
                if(!this.IsAttributeModified(preImage,SlotsEntity.Fields.ed_StandardPrice))
                {
                    standardPrice = preImage.ed_StandardPrice;
                }
                else
                {
                    standardPrice = this.ed_StandardPrice;
                }
            }
        }

        public static void HandleSlotsEntityCreate(Plugin.LocalPluginContext localContext, SlotsEntity target)
        {
            localContext.Trace("Inside HandleSlotsEntityCreate");

            FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
            {
                if (target.ed_OrderProductID != null || target.ed_QuoteProductID != null)
                {
                    SlotsEntity.SlotsEntityUpdateQuantity(localContext, target);
                }

                if (target.ed_Quote != null)
                {
                    //SlotsEntity.SlotsEntityUpdateCustomPrice(localContext, target, null); 
                }
                else if(target.ed_Order != null)
                {
                    //Do Discount depending on Order Product
                }

                if (target.ed_ProductID != null)
                {
                    updateNumberSlot(localContext, target);
                }
            }
        }
        
        public static void updateNumberSlot(Plugin.LocalPluginContext localContext,SlotsEntity target)
        {
            localContext.Trace("Inside updateNumberSlot");
            QueryExpression querySlotsNumber = new QueryExpression();
            querySlotsNumber.EntityName = SlotsEntity.EntityLogicalName;
            querySlotsNumber.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_SlotNumber);

            FilterExpression filter = new FilterExpression();
            filter.FilterOperator = LogicalOperator.And;
            filter.AddCondition(SlotsEntity.Fields.ed_ProductID, ConditionOperator.Equal, target.ed_ProductID.Id);
            filter.AddCondition(SlotsEntity.Fields.ed_SlotsId, ConditionOperator.NotEqual, target.Id);

            querySlotsNumber.Criteria.AddFilter(filter);
            querySlotsNumber.AddOrder(SlotsEntity.Fields.ed_SlotNumber, OrderType.Descending);

            SlotsEntity lastSlot = XrmRetrieveHelper.RetrieveFirst<SlotsEntity>(localContext, querySlotsNumber);

            var slotNumber = 1;
            var slotDayProductNumber = 1;
            if (lastSlot != null)
            {
                localContext.Trace("lastSlot not null");
                if (lastSlot.ed_SlotNumber != null && lastSlot.ed_SlotNumber.Value > 0)
                {
                    localContext.Trace("SlotNumber not null and greater than 0");
                    slotNumber = lastSlot.ed_SlotNumber.Value + 1;
                }
            }
            localContext.Trace("lastSlot Number Value: " + slotNumber);

            QueryExpression querySlotDayProduct = new QueryExpression();
            querySlotDayProduct.TopCount = 1;
            querySlotDayProduct.EntityName = SlotsEntity.EntityLogicalName;
            querySlotDayProduct.ColumnSet.AddColumn(SlotsEntity.Fields.ed_IDSlotPerDayProduct);

            FilterExpression filterSlotDayProduct = new FilterExpression();
            filterSlotDayProduct.FilterOperator = LogicalOperator.And;
            filterSlotDayProduct.AddCondition(SlotsEntity.Fields.ed_BookingDay, ConditionOperator.Equal, target.ed_BookingDay.Value);
            filterSlotDayProduct.AddCondition(SlotsEntity.Fields.ed_ProductID, ConditionOperator.Equal, target.ed_ProductID.Id);

            querySlotDayProduct.Criteria.AddFilter(filterSlotDayProduct);

            querySlotDayProduct.AddOrder(SlotsEntity.Fields.ed_IDSlotPerDayProduct, OrderType.Descending);

            List<SlotsEntity> slotsList = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlotDayProduct);
            if (slotsList != null && slotsList.Count > 0)
            {
                if (slotsList[0].ed_IDSlotPerDayProduct != null && slotsList[0].ed_IDSlotPerDayProduct.Value > 0)
                {
                    slotDayProductNumber = slotsList[0].ed_IDSlotPerDayProduct.Value + 1;
                }
            }

            SlotsEntity slotToUpdate = new SlotsEntity();
            slotToUpdate.Id = target.Id;
            slotToUpdate.ed_SlotNumber = slotNumber;
            slotToUpdate.ed_IDSlotPerDayProduct = slotDayProductNumber;
            slotToUpdate.ed_name = target.ed_name + " - " + slotDayProductNumber;
            XrmHelper.Update(localContext, slotToUpdate);
        }

        public static void HandleSlotsEntityUpdate(Plugin.LocalPluginContext localContext, SlotsEntity target, SlotsEntity preImage)
        {
            FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
            {
                if (target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_OrderProductID) || target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_QuoteProductID))
                {
                    SlotsEntity.SlotsEntityUpdateQuantity(localContext, target, preImage);
                }

                if(target.IsAttributeModified(preImage,SlotsEntity.Fields.ed_Quote))
                {
                    //SlotsEntity.SlotsEntityUpdateCustomPrice(localContext, target, preImage); 
                }
                else if (target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_Order))
                {
                    //Do discount for slots depending on OrderProduct
                }
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
            queryAvailableSlots.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_BookingDay,SlotsEntity.Fields.ed_StandardPrice,SlotsEntity.Fields.ed_CustomPrice,SlotsEntity.Fields.ed_Extended);

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
                    slot.ed_CustomPrice = priceProduct;
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

                                if(filteredSlots[0].ed_Extended.Value == true)
                                {
                                    slot.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.ExtendedPreliminiary;
                                }
                                else
                                {
                                    slot.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Preliminary;
                                }
                                
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
                                
                                if (filteredSlots[0].ed_Extended.Value == true)
                                {
                                    slot.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.ExtendedPreliminiary;
                                }
                                else
                                {
                                    slot.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Preliminary;
                                }

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
                        slot.ed_Extended = true;
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
                                slot.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.ExtendedPreliminiary;
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
                                slot.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.ExtendedPreliminiary;
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
        
        public static void ReleaseSlots(Plugin.LocalPluginContext localContext, bool releaseAll, Guid? quoteProduct = null, Guid? orderProduct = null, DateTime ? startDate = null, DateTime ? endDate = null, int? quantity = null)
        {
            localContext.Trace("Inside ReleaseSlots");

            if(quoteProduct == null && orderProduct == null)
            {
                localContext.Trace("quoteProduct and orderProduct null.");
                return;
            }

            QueryExpression query = new QueryExpression();
            query.EntityName = SlotsEntity.EntityLogicalName;
            query.ColumnSet.AddColumns(SlotsEntity.Fields.ed_BookingDay,SlotsEntity.Fields.ed_Extended,SlotsEntity.Fields.ed_StandardPrice);
            FilterExpression filter = new FilterExpression();
            filter.FilterOperator = LogicalOperator.And;
            if(quoteProduct != null && quoteProduct.Value != Guid.Empty)
            {
                filter.AddCondition(SlotsEntity.Fields.ed_QuoteProductID, ConditionOperator.Equal, quoteProduct.Value);
            }
            else if(orderProduct != null && orderProduct.Value != Guid.Empty)
            {
                filter.AddCondition(SlotsEntity.Fields.ed_OrderProductID, ConditionOperator.Equal, orderProduct.Value);
            }
            
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
                    if (releaseAll)
                    {
                        slotToRelease.ed_QuoteProductID = null;
                        slotToRelease.ed_Quote = null;
                        slotToRelease.ed_OrderProductID = null;
                        slotToRelease.ed_Order = null;
                        slotToRelease.ed_DiscountAmount = null;
                        slotToRelease.ed_Opportunity = null;
                        slotToRelease.ed_CustomPrice = slot.ed_StandardPrice;
                        //Validate generatedFromProduct bool or defaultBookingStatus
                        if (slot.ed_Extended != null && slot.ed_Extended.Value == true)
                        {
                            slotToRelease.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.ExtendedOpen;
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
                            slotToRelease.ed_CustomPrice = slot.ed_StandardPrice;
                            //Validate generatedFromProduct bool or defaultBookingStatus

                            if (slot.ed_Extended != null && slot.ed_Extended.Value == true)
                            {
                                slotToRelease.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.ExtendedOpen;
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

            QuoteEntity preQuote = null;
            QuoteEntity targetQuote = null;

            OrderEntity preOrder = null;
            OrderEntity targetOrder = null;


            if (!target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_Quote))
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
                        preQuote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, preImage.ed_Quote, new ColumnSet(QuoteEntity.Fields.StateCode, QuoteEntity.Fields.StatusCode,QuoteEntity.Fields.DiscountAmount,QuoteEntity.Fields.DiscountPercentage));

                        
                        if(preQuote != null)
                        {
                            if (preQuote.StateCode != Generated.QuoteState.Closed && preQuote.StateCode != Generated.QuoteState.Won)
                            {
                                preQuoteActive = true;
                            }
                        }
                    }
                    

                    if (target.ed_Quote != null && target.ed_Quote.Id != Guid.Empty)
                    {
                        targetQuote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, target.ed_Quote, new ColumnSet(QuoteEntity.Fields.StateCode, QuoteEntity.Fields.StatusCode, QuoteEntity.Fields.DiscountAmount, QuoteEntity.Fields.DiscountPercentage));
                        
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
                        preQuote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, preImage.ed_Quote, new ColumnSet(QuoteEntity.Fields.StateCode, QuoteEntity.Fields.StatusCode, QuoteEntity.Fields.DiscountPercentage, QuoteEntity.Fields.DiscountAmount));

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
                        targetQuote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, target.ed_Quote, new ColumnSet(QuoteEntity.Fields.StateCode, QuoteEntity.Fields.StatusCode, QuoteEntity.Fields.DiscountPercentage, QuoteEntity.Fields.DiscountAmount));

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
                        preOrder = XrmRetrieveHelper.Retrieve<OrderEntity>(localContext, preImage.ed_Order, new ColumnSet(OrderEntity.Fields.StateCode, OrderEntity.Fields.StatusCode, OrderEntity.Fields.DiscountPercentage, OrderEntity.Fields.DiscountAmount));

                        if (preOrder != null && (int)preOrder.StateCode.Value != (int)Generated.ed_SalesOrderState.Inactive)
                        {
                            preOrderActive = true;
                        }
                    }


                    if (target.ed_Order != null && target.ed_Order.Id != Guid.Empty)
                    {
                        targetOrder = XrmRetrieveHelper.Retrieve<OrderEntity>(localContext, target.ed_Order, new ColumnSet(OrderEntity.Fields.StateCode, OrderEntity.Fields.StatusCode, OrderEntity.Fields.DiscountPercentage, OrderEntity.Fields.DiscountAmount));

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
                        preOrder = XrmRetrieveHelper.Retrieve<OrderEntity>(localContext, preImage.ed_Order, new ColumnSet(OrderEntity.Fields.StateCode, OrderEntity.Fields.StatusCode,OrderEntity.Fields.DiscountPercentage, OrderEntity.Fields.DiscountAmount));

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
                        targetOrder = XrmRetrieveHelper.Retrieve<OrderEntity>(localContext, target.ed_Order, new ColumnSet(OrderEntity.Fields.StateCode, OrderEntity.Fields.StatusCode,OrderEntity.Fields.DiscountPercentage,OrderEntity.Fields.DiscountAmount));

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

        //Below to be removed, should probably only work from QuoteProduct/OrderProduct
        public static void SlotsEntityUpdateCustomPrice(Plugin.LocalPluginContext localContext, SlotsEntity target, SlotsEntity preImage)
        {
            if(preImage != null)
            {
                if (target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_Quote))
                {
                    QuoteEntity preQuote = null;
                    QuoteEntity targetQuote = null;

                    if (preImage.ed_Quote != null && preImage.ed_Quote.Id != Guid.Empty)
                    {
                        preQuote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, preImage.ed_Quote, new ColumnSet(QuoteEntity.Fields.DiscountAmount, QuoteEntity.Fields.DiscountPercentage));
                    }
                    if (target.ed_Quote != null && target.ed_Quote.Id != Guid.Empty)
                    {
                        targetQuote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, target.ed_Quote, new ColumnSet(QuoteEntity.Fields.DiscountAmount, QuoteEntity.Fields.DiscountPercentage));
                    }

                    if (preQuote != null)
                    {
                        if (preQuote.DiscountAmount != null && preQuote.DiscountAmount.Value > 0)
                        {
                            QueryExpression querySlotsPreQuote = new QueryExpression();
                            querySlotsPreQuote.EntityName = SlotsEntity.EntityLogicalName;
                            querySlotsPreQuote.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_StandardPrice);

                            FilterExpression filterSlotsPreQuote = new FilterExpression();
                            filterSlotsPreQuote.FilterOperator = LogicalOperator.And;
                            filterSlotsPreQuote.AddCondition(SlotsEntity.Fields.ed_Quote, ConditionOperator.Equal, preQuote.Id);
                            filterSlotsPreQuote.AddCondition(SlotsEntity.Fields.ed_SlotsId, ConditionOperator.NotEqual, target.Id);

                            querySlotsPreQuote.Criteria.AddFilter(filterSlotsPreQuote);

                            List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlotsPreQuote);

                            if (slots != null && slots.Count > 0)
                            {
                                var discountPerSlotPreQuote = preQuote.DiscountAmount.Value / slots.Count;

                                foreach (SlotsEntity slot in slots)
                                {
                                    if (slot.ed_StandardPrice != null)
                                    {
                                        SlotsEntity SlotToUpdatePre = new SlotsEntity();
                                        SlotToUpdatePre.Id = slot.Id;
                                        SlotToUpdatePre.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - discountPerSlotPreQuote);
                                        XrmHelper.Update(localContext, SlotToUpdatePre);
                                    }
                                }
                            }
                        }
                    }

                    if (targetQuote != null)
                    {
                        if (targetQuote.DiscountAmount != null && targetQuote.DiscountAmount.Value > 0)
                        {
                            QueryExpression querySlotsTargetQuote = new QueryExpression();
                            querySlotsTargetQuote.EntityName = SlotsEntity.EntityLogicalName;
                            querySlotsTargetQuote.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_StandardPrice);

                            FilterExpression filterSlotsTargetQuote = new FilterExpression();
                            filterSlotsTargetQuote.FilterOperator = LogicalOperator.And;
                            filterSlotsTargetQuote.AddCondition(SlotsEntity.Fields.ed_Quote, ConditionOperator.Equal, targetQuote.Id);
                            filterSlotsTargetQuote.AddCondition(SlotsEntity.Fields.ed_SlotsId, ConditionOperator.NotEqual, target.Id);

                            querySlotsTargetQuote.Criteria.AddFilter(filterSlotsTargetQuote);

                            List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlotsTargetQuote);

                            if (slots != null && slots.Count > 0)
                            {
                                var discountPerSlotTargetQuote = targetQuote.DiscountAmount.Value / (slots.Count + 1);

                                foreach (SlotsEntity slot in slots)
                                {
                                    if (slot.ed_StandardPrice != null)
                                    {
                                        SlotsEntity SlotToUpdate = new SlotsEntity();
                                        SlotToUpdate.Id = slot.Id;
                                        SlotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - discountPerSlotTargetQuote);
                                        XrmHelper.Update(localContext, SlotToUpdate);
                                    }
                                }
                                SlotsEntity currentSlot = new SlotsEntity();
                                currentSlot.Id = target.Id;

                                decimal standardPrice = 0;

                                if (!target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_StandardPrice))
                                {
                                    if (preImage.ed_StandardPrice != null && preImage.ed_StandardPrice.Value > 0)
                                    {
                                        standardPrice = preImage.ed_StandardPrice.Value;
                                    }
                                }
                                else
                                {
                                    if (target.ed_StandardPrice != null && target.ed_StandardPrice.Value > 0)
                                    {
                                        standardPrice = target.ed_StandardPrice.Value;
                                    }
                                }
                                currentSlot.ed_CustomPrice = new Money(standardPrice - discountPerSlotTargetQuote);

                                XrmHelper.Update(localContext, currentSlot);
                            }
                        }
                        else if (targetQuote.DiscountPercentage != null && targetQuote.DiscountPercentage.Value > 0)
                        {
                            SlotsEntity currentSlot = new SlotsEntity();
                            currentSlot.Id = target.Id;

                            decimal standardPrice = 0;

                            if (!target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_StandardPrice))
                            {
                                if (preImage.ed_StandardPrice != null && preImage.ed_StandardPrice.Value > 0)
                                {
                                    standardPrice = preImage.ed_StandardPrice.Value;
                                }
                            }
                            else
                            {
                                if (target.ed_StandardPrice != null && target.ed_StandardPrice.Value > 0)
                                {
                                    standardPrice = target.ed_StandardPrice.Value;
                                }
                            }
                            currentSlot.ed_CustomPrice = new Money(standardPrice * (targetQuote.DiscountPercentage.Value / 100));

                            XrmHelper.Update(localContext, currentSlot);
                        }
                    }

                    if (target.IsAttributeModified(preImage, SlotsEntity.Fields.ed_Order))
                    {
                        //do the logic for the orders
                    }
                }
            }
            else
            {
                QuoteEntity quote = new QuoteEntity();
                quote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, target.ed_Quote, new ColumnSet(QuoteEntity.Fields.DiscountAmount, QuoteEntity.Fields.DiscountPercentage));

                if (quote.DiscountAmount != null && quote.DiscountAmount.Value > 0)
                {
                    QueryExpression querySlotsTargetQuote = new QueryExpression();
                    querySlotsTargetQuote.EntityName = SlotsEntity.EntityLogicalName;
                    querySlotsTargetQuote.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_StandardPrice);

                    FilterExpression filterSlotsTargetQuote = new FilterExpression();
                    filterSlotsTargetQuote.FilterOperator = LogicalOperator.And;
                    filterSlotsTargetQuote.AddCondition(SlotsEntity.Fields.ed_Quote, ConditionOperator.Equal, quote.Id);
                    filterSlotsTargetQuote.AddCondition(SlotsEntity.Fields.ed_SlotsId, ConditionOperator.NotEqual, target.Id);

                    querySlotsTargetQuote.Criteria.AddFilter(filterSlotsTargetQuote);

                    List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlotsTargetQuote);

                    if (slots != null && slots.Count > 0)
                    {
                        var discountPerSlotTargetQuote = quote.DiscountAmount.Value / (slots.Count + 1);

                        foreach (SlotsEntity slot in slots)
                        {
                            if (slot.ed_StandardPrice != null)
                            {
                                SlotsEntity SlotToUpdate = new SlotsEntity();
                                SlotToUpdate.Id = slot.Id;
                                SlotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - discountPerSlotTargetQuote);
                                XrmHelper.Update(localContext, SlotToUpdate);
                            }
                        }
                        SlotsEntity currentSlot = new SlotsEntity();
                        currentSlot.Id = target.Id;

                        decimal standardPrice = 0;
                        if (target.ed_StandardPrice != null && target.ed_StandardPrice.Value > 0)
                        {
                                standardPrice = target.ed_StandardPrice.Value;
                        }
                        currentSlot.ed_CustomPrice = new Money(standardPrice - discountPerSlotTargetQuote);

                        XrmHelper.Update(localContext, currentSlot);
                    }
                }
                else if (quote.DiscountPercentage != null && quote.DiscountPercentage.Value > 0)
                {
                    SlotsEntity currentSlot = new SlotsEntity();
                    currentSlot.Id = target.Id;

                    decimal standardPrice = 0;
                    
                    if (target.ed_StandardPrice != null && target.ed_StandardPrice.Value > 0)
                    {
                        standardPrice = target.ed_StandardPrice.Value;
                    }

                    currentSlot.ed_CustomPrice = new Money(standardPrice * (quote.DiscountPercentage.Value / 100));

                    XrmHelper.Update(localContext, currentSlot);
                }
            }
        }

        public static string HandleCreateExcelBase64(Plugin.LocalPluginContext localContext, string fromDate, string toDate)
        {
            localContext.TracingService.Trace($"Running HandleCreateExcelBase64.");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
            settingFilter.AddCondition(CgiSettingEntity.Fields.ed_CRMPlusService, ConditionOperator.NotNull);
            settingFilter.AddCondition(CgiSettingEntity.Fields.ed_ClientCertName, ConditionOperator.NotNull);

            CgiSettingEntity settingEntity = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(
                CgiSettingEntity.Fields.ed_CRMPlusService,
                CgiSettingEntity.Fields.ed_ClientCertName), settingFilter);

            if (settingEntity == null || string.IsNullOrWhiteSpace(settingEntity.ed_CRMPlusService) || string.IsNullOrWhiteSpace(settingEntity.ed_ClientCertName))
            {
                //Bad request -> exception
                throw new InvalidWorkflowException($"The WebApi URl or the ClientCertName are empty or null.");
            }

            string apiStatusResponse = "";

            try
            {
                const long tokenLifetimeSeconds = 30 * 60 + 60 * 60 * 1;  // Add 1h for Summertime UTC

                Int32 unixTimeStamp;
                DateTime currentTime = DateTime.Now;
                DateTime zuluTime = currentTime.ToUniversalTime();
                DateTime unixEpoch = new DateTime(1970, 1, 1);
                unixTimeStamp = (Int32)(zuluTime.Subtract(unixEpoch)).TotalSeconds;

                long validTo = unixTimeStamp + tokenLifetimeSeconds;

                Identity tokenClass = new Identity();
                tokenClass.exp = validTo;
                string clientCert = settingEntity.ed_ClientCertName;

                localContext.TracingService.Trace("HandleCreateExcelBase64:");
                string url = settingEntity.ed_CRMPlusService + $"/api/Slots/GetExcelBase64/slots?fromDate={fromDate}&toDate={toDate}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(clientCert));
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                using (WebResponse getCardHttpResponse = httpWebRequest.GetResponse())
                {
                    var checkStatus = (HttpWebResponse)getCardHttpResponse;
                    if (checkStatus.StatusCode != HttpStatusCode.OK)
                    {
                        //Send bad request
                        apiStatusResponse = "";
                    }
                    else
                    {
                        //We are done
                        Stream stream = checkStatus.GetResponseStream();
                        StreamReader reader = new StreamReader(stream);
                        apiStatusResponse = reader.ReadToEnd();
                    }
                }

                localContext.TracingService.Trace($"Successfully exiting HandleCreateExcelBase64.");

                return apiStatusResponse;
            }
            catch (Exception ex)
            {
                localContext.TracingService.Trace("Error generating Excel: " + ex.InnerException.Message + " - " + ex.StackTrace + " - " + ex.ToString());
                throw new InvalidPluginExecutionException("Error generating Excel: " + ex.Message + " - " + ex.StackTrace + " - " + ex.ToString());
            }
        }
    }
}
