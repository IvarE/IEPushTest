using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmTravelCard.Test.Models
{
    public class XmlStatus
    {
        public string ErrorMessage { get; set; }
        public string StatusMessage { get; set; }
        public string StatusCodeMessage { get; set; }
        public int? Amount { get; set; }
    }
}
