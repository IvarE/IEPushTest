using System.Runtime.Serialization;
using System;


namespace CGIXrmExtConnectorService
{
    #region Public Enums
    [DataContract]
    public enum TransportType
    {
        [EnumMember]
        CITYBUS=0,
        [EnumMember]
        REGIONBUS = 1,
        [EnumMember]
        TRAIN = 2
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
        SUCCESS = 0,
        [EnumMember]
        FAILED = 1
    }
#endregion
}