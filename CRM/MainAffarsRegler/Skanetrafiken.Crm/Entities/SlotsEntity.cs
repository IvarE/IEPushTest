using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;
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

        public static List<SlotsEntity> AvailableSlots (Plugin.LocalPluginContext localContext,EntityReference productER,DateTime starDate,DateTime endDate)
        {
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

            return slotsEntities;
        }

        public static GenerateSlotsResponse GenerateSlots (Plugin.LocalPluginContext localContext,Guid productId, int quantityPerDay, DateTime startDate, DateTime endDate,Guid? OpportunityGuid = null, QuoteProductEntity quoteProduct = null,OrderProductEntity orderProduct = null)
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

        public static void removeSlots(Plugin.LocalPluginContext localContext, Guid quoteProduct,DateTime? startDate = null,DateTime? endDate = null,int? quantity = null)
        {
            if (quantity == null)
            {
                quantity = 1;
            }

            QueryExpression query = new QueryExpression();

            query.EntityName = SlotsEntity.EntityLogicalName;
            query.ColumnSet.AddColumn(SlotsEntity.Fields.ed_BookingDay);

            FilterExpression filter = new FilterExpression();
            filter.FilterOperator = LogicalOperator.And;

            filter.AddCondition(SlotsEntity.Fields.ed_QuoteProductID, ConditionOperator.Equal, quoteProduct);

            if (startDate != null && endDate != null && DateTime.Compare(startDate.Value,endDate.Value) <= 0)
            {
                filter.AddCondition(SlotsEntity.Fields.ed_BookingDay, ConditionOperator.OnOrAfter, startDate.Value);
                filter.AddCondition(SlotsEntity.Fields.ed_BookingDay, ConditionOperator.OnOrBefore, endDate.Value);
            }

            query.Criteria.AddFilter(filter);

            query.AddOrder(SlotsEntity.Fields.ed_BookingDay, OrderType.Ascending);

            List<SlotsEntity> slotsList = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, query);
            
            if(slotsList != null && slotsList.Count > 0)
            {
                DateTime? currentDate = null;
                int deletedQuantity = 0;
                foreach (SlotsEntity slot in slotsList)
                {
                    if(currentDate == null)
                    {
                        currentDate = slot.ed_BookingDay;
                    }
                    else if(currentDate != slot.ed_BookingDay)
                    {
                        currentDate = slot.ed_BookingDay;
                        deletedQuantity = 0;
                    }

                    if(deletedQuantity < quantity)
                    {
                        XrmHelper.Delete(localContext, slot.ToEntityReference());
                        localContext.Trace("Deleted Slot Booked for: " + slot.ed_BookingDay.Value.ToString("g", CultureInfo.GetCultureInfo("sv-SE")));
                        deletedQuantity++;
                    }
                }
            }
        }
        
    }



}
