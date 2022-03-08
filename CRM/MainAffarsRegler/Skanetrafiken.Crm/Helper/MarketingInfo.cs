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
        public string pricemodelid { get; set; }
        public string priceid { get; set; }
        public List<string> travelareaids { get; set; }

        public Order()
        {
            createdby = null;
            created = null;
            offerId = null;
            campaignid = null;
            pricemodelid = null;
            priceid = null;
            travelareaids = new List<string>();
        }
    }

    public class OrderRow
    {
        public string mklid { get; set; }
        public string telephone { get; set; }

        public OrderRow()
        {
            mklid = null;
            telephone = null;
        }
    }

    public class MarketingInfo
    {
        public Order order { get; set; }
        public List<OrderRow> orderrows { get; set; }

        public MarketingInfo()
        {
            order = new Order();
            orderrows = new List<OrderRow>();
        }
    }
}
