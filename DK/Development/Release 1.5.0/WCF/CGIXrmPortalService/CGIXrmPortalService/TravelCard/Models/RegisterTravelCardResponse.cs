using System.Runtime.Serialization;
using CGICRMPortalService.Shared.Models;
using System;

namespace CGICRMPortalService.TravelCard.Models
{
    [DataContract]
    public class RegisterTravelCardResponse : Response
    {
        public Guid TravelCardId { get; set; }
        public Guid AccountId { get; set; }
    }
}