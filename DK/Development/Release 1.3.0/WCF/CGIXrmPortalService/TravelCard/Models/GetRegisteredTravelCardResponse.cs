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
        //internal ProcessingStatus _Status;
        //[DataMember]
        //public ProcessingStatus Status
        //{
        //    get { return _Status; }
        //}

        //internal string _Message;
        //[DataMember]
        //public string Message
        //{
        //    get { return _Message; }

        //}
        List<TravelCard> _TravelCards;

        public List<TravelCard> TravelCards
        {
            get { return _TravelCards; }
            set { _TravelCards = value; }
        }
    }
}