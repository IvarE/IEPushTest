using Newtonsoft.Json;
using System.Collections.Generic;

namespace Skanetrafiken.Crm.Helper
{
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
        public string offerId { get; set; }
        public string bearerCategory { get; set; }
        public string campaignId { get; set; }
        
        //public Order order { get; set; }
        public List<OrderRow> orderrows { get; set; }

        public MarketingInfo()
        {
            offerId = null;
            bearerCategory = null;
            campaignId = null;
            orderrows = new List<OrderRow>();
        }
    }
}
