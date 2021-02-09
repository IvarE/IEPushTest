using CGICRMPortalService.Shared.Models;
using System;

namespace CGICRMPortalService.TravelCard.Models
{
    public class UpdateTravelCardResponse:Response
    {
        public Guid TravelCardId { get; set; }
        public Guid AccountId { get; set; }
    }
}