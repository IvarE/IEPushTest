using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    #region Public Enums
    [DataContract]   
    public enum CustomerType
    { 
        [EnumMember]
        Private = 0,
        [EnumMember]
        Organisation = 1
    }
    #endregion
}