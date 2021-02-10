using System;

namespace CGIXrmHandler.CallGuide.Models
{
    public class WorkItem
    {
        #region Public Properties

        public Guid ActivityId { get; set; }

        public Guid AccountId { get; set; }

        public string CallguideSessionId { get; set; }

        public string Description { get; set; }

        public string ErrandTaskType { get; set; }

        public string UpdateData { get; set; }

        public string FbUrl { get; set; }

        public Guid CallGuideInfoId { get; set; }

        public Guid CreatedCaseId { get; set; }

        #endregion
    }
}
