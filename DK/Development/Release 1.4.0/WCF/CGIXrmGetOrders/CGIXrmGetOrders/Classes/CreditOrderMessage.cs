using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    [DataContract]
    public class CreditOrderMessage
    {
        #region Publice Properties ------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string OrderNumber { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Sum { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ReferenceNumber { get; set; }

        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Success { get; set; }

        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        //<Date>2014-11-24T10:26:25.6059131+01:00</Date>
        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public string Time { get; set; }


        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string ProductNumber { get; set; }

        [DataMember]
        public string CRMMessage { get; set; }

        [DataMember]
        public string Reason { get; set; }

        #endregion
    }
}