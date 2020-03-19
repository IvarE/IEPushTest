using System.Runtime.Serialization;

namespace CGIXrmExtConnectorService.Shared.Models
{
    #region Public Enums ----------------------------------------------------------------------------------------------
    
    [DataContract]
    public enum TransportType
    {
        [EnumMember]
        Citybus=0,
        [EnumMember]
        Regionbus = 1,
        [EnumMember]
        Train = 2
    }

    [DataContract]
    public enum ActionType
    {
        //None=0,
        [EnumMember]
        Intranet = 1,
        [EnumMember]
        ExternalWeb = 2,
        [EnumMember]
        Both = 3
    }

    [DataContract]
    public enum ProcessingStatus
    {
        [EnumMember]
        Success = 0,
        [EnumMember]
        Failed = 1
    }

#endregion
}