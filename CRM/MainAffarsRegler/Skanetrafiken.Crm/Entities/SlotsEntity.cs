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

            GenerateSlotsResponse response = new GenerateSlotsResponse();

            if (endDate < startDate)
            {
                response.OK = false;
                response.Message = "Start date is higher than the End date.";
            }

            FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            

            ColumnSet productColumns = new ColumnSet(false);
            productColumns.AddColumn(ProductEntity.Fields.Price);

            Money priceProduct = new Money(0);

            

            ProductEntity product = XrmRetrieveHelper.Retrieve<ProductEntity>(localContext, productId, productColumns);

            if(product != null)
            {
                //if (product.Contains(ProductEntity.Fields.Price) && product.Price != null)
                    //priceProduct = product.;
            }
            else
            {
                response.OK = false;
                response.Message = "Product not found.";
                return response;
            }

            do
            {
                
            }
            while (DateTime.Compare(startDate,endDate) <= 0);
            
            

            return response;
        }

    }



}
