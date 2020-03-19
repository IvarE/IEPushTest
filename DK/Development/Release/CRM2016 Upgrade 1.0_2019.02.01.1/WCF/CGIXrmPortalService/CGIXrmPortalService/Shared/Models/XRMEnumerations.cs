using System.Runtime.Serialization;

namespace CGICRMPortalService.Shared.Models
{
    #region Public Enums
    /// <summary>
    /// Enumerator for Account/Contact state in Customer entity record.
    /// </summary>
    public enum StateCode
    {
        Inactive = 2,
        Active = 1,
    }

    /// <summary>
    /// Enumerator for Creditability for a customer.
    /// </summary>
    public enum DebtCollection
    {
        Yes = 1,
        No = 0,
    }
    [DataContract]
    public enum ProcessingStatus
    {
        [EnumMember]
        SUCCESS = 0,
        [EnumMember]
        FAILED = 1
    }
    [DataContract]    
    public enum XRMConstants
    {
        [EnumMember]
        NullValue = 0
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