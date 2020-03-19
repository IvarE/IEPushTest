using System.Runtime.Serialization;
using CGIXrmExtConnectorService;

[DataContract]
public class CallGuideRequest
{
    string _CallGuideSessionID;
    [DataMember]
    public string CallGuideSessionID
    {
        get { return _CallGuideSessionID; }
        set { _CallGuideSessionID = value; }
    }

    string _QueueTime;
    [DataMember]
    public string QueueTime
    {
        get { return _QueueTime; }
        set { _QueueTime = value; }
    }
    string _APhoneNumber;
    [DataMember]
    public string APhoneNumber
    {
        get { return _APhoneNumber; }
        set { _APhoneNumber = value; }
    }


    string _BPhoneNumber;
    [DataMember]
    public string BPhoneNumber
    {
        get { return _BPhoneNumber; }
        set { _BPhoneNumber = value; }
    }


    string _ErrandTaskType;
    [DataMember]
    public string ErrandTaskType
    {
        get { return _ErrandTaskType; }
        set { _ErrandTaskType = value; }
    }

    string _ScreenPopChoice;
    [DataMember]
    public string ScreenPopChoice
    {
        get { return _ScreenPopChoice; }
        set { _ScreenPopChoice = value; }
    }

    string _ContactSourceType;
    [DataMember]
    public string ContactSourceType
    {
        get { return _ContactSourceType; }
        set { _ContactSourceType = value; }
    }

    string _CId;
    [DataMember]
    public string CId
    {
        get { return _CId; }
        set { _CId = value; }
    }

    string _AgentName;
    [DataMember]
    public string AgentName
    {
        get { return _AgentName; }
        set { _AgentName = value; }
    }

    CallDirection _CallDirection;
    [DataMember]
    public CallDirection CallDirection
    {
        get { return _CallDirection; }
        set { _CallDirection = value; }
    }

    CallGuideAction _CallGuideAction;
    [DataMember]
    public CallGuideAction CallGuideAction
    {
        get { return _CallGuideAction; }
        set { _CallGuideAction = value; }
    }
    
}
