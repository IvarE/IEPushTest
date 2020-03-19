using System.Runtime.Serialization;

namespace CGIXrmTravelCard.TravelCardClasses.Models
{
    [DataContract]
    public class GetOutstandingChargesResponse
    {
        #region Public Properties

        /// <summary>
        /// OutstandingChargesResponse från eHandel
        /// </summary>
        [DataMember]
        public string OutstandingCharges { get; set; }

        /// <summary>
        /// Errormessage
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }

        #endregion
    }
}