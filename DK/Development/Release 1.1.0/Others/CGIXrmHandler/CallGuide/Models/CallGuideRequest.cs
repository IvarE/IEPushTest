using System.Runtime.Serialization;
using CGIXrmHandler;
using System;



public class CallGuideRequest
{
    string _CallGuideSessionId;
    public string CallGuideSessionId
    {
        get { return _CallGuideSessionId; }
        set { _CallGuideSessionId = value; }
    }

    string _QueueTime;    
    public string QueueTime
    {
        get { return _QueueTime; }
        set { _QueueTime = value; }
    }
    string _APhoneNumber;    
    public string APhoneNumber
    {
        get { return _APhoneNumber; }
        set { _APhoneNumber = value; }
    }


    string _BPhoneNumber;    
    public string BPhoneNumber
    {
        get { return _BPhoneNumber; }
        set { _BPhoneNumber = value; }
    }


    string _ErrandTaskType;    
    public string ErrandTaskType
    {
        get { return _ErrandTaskType; }
        set { _ErrandTaskType = value; }
    }

    string _ScreenPopChoice;    
    public string ScreenPopChoice
    {
        get { return _ScreenPopChoice; }
        set { _ScreenPopChoice = value; }
    }

    string _ContactSourceType;    
    public string ContactSourceType
    {
        get { return _ContactSourceType; }
        set { _ContactSourceType = value; }
    }

    string _CId;    
    public string CId
    {
        get { return _CId; }
        set { _CId = value; }
    }

    string _AgentName;   
    public string AgentName
    {
        get { return _AgentName; }
        set { _AgentName = value; }
    }

    string _CallDuration;
    public string CallDuration
    {
        get { return _CallDuration; }
        set { _CallDuration = value; }
    }
    
    string _ChatCustomerAlias;    
    public string ChatCustomerAlias
    {
        get { return _ChatCustomerAlias; }
        set { _ChatCustomerAlias = value; }
    }
    CallDirection _CallDirection;   
    public CallDirection CallDirection
    {
        get { return _CallDirection; }
        set { _CallDirection = value; }
    }

    CallGuideRouteAction _CallRouteAction;
    public CallGuideRouteAction CallRouteAction
    {
        get { return _CallRouteAction; }
        set { _CallRouteAction = value; }
    }

    AccountCategoryCode _CustomerType;
    internal AccountCategoryCode CustomerType
    {
        get { return _CustomerType; }
        set { _CustomerType = value; }
    }
    
}
