using System;


namespace CGICRMPortalService.Models
{
    public class CreateCaseResponse
    {
        Guid _CaseId;

        public Guid CaseId
        {
            get { return _CaseId; }
            set { _CaseId = value; }
        }

        string _CaseNumber;

        public string CaseNumber
        {
            get { return _CaseNumber; }
            set { _CaseNumber = value; }
        }

        Guid _NotesId;

        public Guid NotesId
        {
            get { return _NotesId; }
            set { _NotesId = value; }
        }
    }
}
