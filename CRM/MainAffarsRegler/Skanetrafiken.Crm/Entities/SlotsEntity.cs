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


        public static GenerateSlotsResponse GenerateSlots (Plugin.LocalPluginContext localContext,Guid productId, int quantityPerDay, DateTime startDate, DateTime endDate)
        {
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

                    XrmHelper.Create(localContext, slot);
                }
                

                startDate = startDate.AddDays(1);
            }
            while (DateTime.Compare(startDate,endDate) <= 0);

            response.OK = true;
            response.Message = "Slots created.";
            return response;
        }

    }



}
