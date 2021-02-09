using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CGIXrmHandler
{
    public class CallGuideRecord
    {
        string _ContactId;
        public string ContactId
        {
            get { return _ContactId; }
            set { _ContactId = value; }
        }
        string _InteractionId;
        public string InteractionId
        {
            get { return _InteractionId; }
            set { _InteractionId = value; }
        }
        string _Data;
        public string Data
        {
            get { return _Data; }
            set { _Data = value; }
        }
        string _FbUrl;
        public string FbUrl
        {
            get { return _FbUrl; }
            set { _FbUrl = value; }
        }
        CallGuideBatchActivity _CallGuideActivity;
        public CallGuideBatchActivity CallGuideActivity
        {
            get { return _CallGuideActivity; }
            set { _CallGuideActivity = value; }
        }

        
        


    }
}
