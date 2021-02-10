using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGICRMPortalService.Models
{
    public class GetCardsForCustomerResponse : Response
    {
        #region Public Properties
        List<TravelCard> _TravelCards;
        public List<TravelCard> TravelCards
        {
            get { return _TravelCards; }
            set { _TravelCards = value; }
        }
        #endregion
    }
}