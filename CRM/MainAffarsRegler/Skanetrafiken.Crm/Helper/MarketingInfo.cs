using Newtonsoft.Json;
using System.Collections.Generic;

namespace Skanetrafiken.Crm.Helper
{
    public class Order
    {
        public string createdby { get; set; }
        public string created { get; set; }
        public string offerId { get; set; }
        public string campaignid { get; set; }
        public string bearerCategory { get; set; }
        //public string pricemodelid { get; set; }
        //public string priceid { get; set; }
        //public List<string> travelareaids { get; set; }

        public Order()
        {
            createdby = null;
            created = null;
            offerId = null;
            campaignid = null;
            bearerCategory = null;
            //pricemodelid = null;
            //priceid = null;
            //travelareaids = new List<string>();
        }
    }

    public class OrderRow
    {
        public int? mklId { get; set; }
        public string telephone { get; set; }

        public OrderRow()
        {
            mklId = null;
            telephone = null;
        }
    }

    public class MarketingInfo
    {
        //public string createdBy { get; set; }
        //public string created { get; set; }
        public string offerId { get; set; }
        public string bearerCategory { get; set; }
        public string campaignId { get; set; }
        
        //public Order order { get; set; }
        public List<OrderRow> orderrows { get; set; }

        public MarketingInfo()
        {
            //createdBy = null;
            //created = null;
            offerId = null;
            bearerCategory = null;
            campaignId = null;
            //order = new Order();
            orderrows = new List<OrderRow>();
        }
    }
}
