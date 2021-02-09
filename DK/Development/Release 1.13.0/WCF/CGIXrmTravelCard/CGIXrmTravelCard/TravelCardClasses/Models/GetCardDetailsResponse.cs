using System.Runtime.Serialization;

namespace CGIXrmTravelCard.TravelCardClasses.Models
{
    [DataContract]
    public class GetCardDetailsResponse
    {
        #region Public Properties

        /// <summary>
        /// Carddetails from BIFF
        /// </summary>
        [DataMember]
        public string CardDetails { get; set; }

        /// <summary>
        /// Errormessage
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }

        #endregion
    }
}