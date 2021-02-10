using System.Runtime.Serialization;
using System;


namespace CGIXrmExtConnectorService
{
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
    //[DataContract]
    //public enum CallDirection
    //{
    //    [EnumMember]
    //    Incoming =0,
    //    [EnumMember]
    //    Outgoing =1
    //}

    //internal enum CaseOrgin
    //{
    //    PhoneCall=1,
    //    Email=2,
    //    Web=3,
    //    Chat = 285050001,    
    //    FaceBook = 285050000
    //}

    //internal enum CaseType
    //{
    //    Question=1,
    //    Problem = 2,
    //    Request = 3
    //}
}