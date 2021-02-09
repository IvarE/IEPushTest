using System.Runtime.Serialization;

namespace CGIXrmEAIConnectorService.Shared.Models
{
    #region Public Enums
    [DataContract]
    public enum ProcessingStatus
    {
        [EnumMember]
        SUCCESS = 0,
        [EnumMember]
        FAILED = 1
    }
    [DataContract]
    public enum AccountCategoryCode
    {
        [EnumMember]
        Private = 1,
        [EnumMember]
        Company = 2
    }

    [DataContract]
    public enum AddressTypeCode
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Invoice = 1,
        [EnumMember]
        Delivery = 2

    }
#endregion
}