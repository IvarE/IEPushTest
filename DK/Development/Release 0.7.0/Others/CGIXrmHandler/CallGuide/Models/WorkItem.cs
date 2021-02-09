using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CGIXrmHandler
{
    public class WorkItem
    {
        Guid _ActivityId;
        public Guid ActivityId
        {
            get { return _ActivityId; }
            set { _ActivityId = value; }
        }
        Guid _AccountId;
        public Guid AccountId
        {
            get { return _AccountId; }
            set { _AccountId = value; }
        }
        string _CallguideSessionId;
        public string CallguideSessionId
        {
            get { return _CallguideSessionId; }
            set { _CallguideSessionId = value; }
        }
        string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        string _ErrandTaskType;
        public string ErrandTaskType
        {
            get { return _ErrandTaskType; }
            set { _ErrandTaskType = value; }
        }
        string _UpdateData;
        public string UpdateData
        {
            get { return _UpdateData; }
            set { _UpdateData = value; }
        }
        string _FbUrl;
        public string FbUrl
        {
            get { return _FbUrl; }
            set { _FbUrl = value; }
        }
        Guid _CallGuideInfoId;
        public Guid CallGuideInfoId
        {
            get { return _CallGuideInfoId; }
            set { _CallGuideInfoId = value; }
        }

        Guid _CreatedCaseId;
        public Guid CreatedCaseId
        {
            get { return _CreatedCaseId; }
            set { _CreatedCaseId = value; }
        }   
    }
}
