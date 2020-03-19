using CGICRMPortalService.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmPortalServiceTest.Models
{
    public class TravelCardResponseTest
    {
        public Guid AccountId { get; set; }
        public Guid TravelCardId { get; set; }
        public ProcessingStatus Status { get; set; }
    }
}
