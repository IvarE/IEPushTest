using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    [DataContract]
    public class BiffTransaction
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [DataMember]
        public string Amount { get; set; }

        [DataMember]
        public string TravelcardId { get; set; } //Guid

        [DataMember]
        public string Cardsect { get; set; }

        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public string Time { get; set; }

        [DataMember]
        public string Deviceid { get; set; }

        [DataMember]
        public string Origzone { get; set; }

        [DataMember]
        public string Origzonename { get; set; }

        [DataMember]
        public string Rectype { get; set; }

        [DataMember]
        public string Route { get; set; }

        [DataMember]
        public string Txntype { get; set; }

        [DataMember]
        public string Travelcard { get; set; }

        [DataMember]
        public string Travelcardtransaction { get; set; }

        #endregion
    }
}