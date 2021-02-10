using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    [DataContract]
    public class GetAccountResponse
    {
        #region Public Properties
        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string Name { get; set; }

        #endregion
    }
}