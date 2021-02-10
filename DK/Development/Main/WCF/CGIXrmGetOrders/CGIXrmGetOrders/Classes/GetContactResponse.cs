using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    [DataContract]
    public class GetContactResponse
    {
        #region Public Properties
        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string Name { get; set; }

        #endregion
    }
}