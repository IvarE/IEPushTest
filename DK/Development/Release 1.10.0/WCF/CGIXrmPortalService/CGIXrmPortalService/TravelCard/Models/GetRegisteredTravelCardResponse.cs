using System.Collections.Generic;
using CGICRMPortalService.Shared.Models;

namespace CGICRMPortalService.TravelCard.Models
{
    public class GetCardsForCustomerResponse : Response
    {
        #region Public Properties

        public List<TravelCard> TravelCards { get; set; }

        #endregion
    }
}