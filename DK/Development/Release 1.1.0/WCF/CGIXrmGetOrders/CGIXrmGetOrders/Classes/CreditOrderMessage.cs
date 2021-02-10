using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGIXrmGetOrders
{
    [DataContract]
    public class CreditOrderMessage
    {
        //<OrderNumber>PO5000140</OrderNumber>
        [DataMember]
        public string OrderNumber { get; set; }

        //<Sum>0</Sum>
        [DataMember]
        public string Sum { get; set; }

        //<ReferenceNumber>PO5000140</ReferenceNumber>
        [DataMember]
        public string ReferenceNumber { get; set; }

        //<Success>false</Success>
        [DataMember]
        public string Success { get; set; }

        //<Message>DibsPaymentTotal 640.0000 is less then credit sum 10</Message>
        [DataMember]
        public string Message { get; set; }

        //<Date>2014-11-24T10:26:25.6059131+01:00</Date>
        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public string Time { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string ProductNumber { get; set; }

        [DataMember]
        public string CRMMessage { get; set; }

        [DataMember]
        public string Reason { get; set; }

    }
}