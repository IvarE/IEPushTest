using System.Runtime.Serialization;
using System;


namespace CGIXrmHandler
{
       
    public enum CallGuideRouteAction
    {

        None = 285050000,
        Case = 285050001,        
        Account = 285050002,        
        Activity = 285050003,
        Chat,
        FaceBook
    }

    public enum CallGuideBatchActivity
    {
        Chat=285050001,
        FaceBook=285050000
    } 
    
    public enum CallDirection
    {
        
        Incoming =0,
        
        Outgoing =1
    }




    internal enum CaseOrgin
    {
        PhoneCall=1,
        Email=2,
        Web=3,
        Chat = 285050001,    
        FaceBook = 285050000
    }

    internal enum CaseType
    {
        Question=1,
        Problem = 2,
        Request = 3
    }

    internal enum AccountCategoryCode
    {
        None=0,
        Company=1,
        Private=2
    }
}