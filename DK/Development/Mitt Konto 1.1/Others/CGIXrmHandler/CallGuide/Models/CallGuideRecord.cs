using CGIXrmHandler.Shared;

namespace CGIXrmHandler.CallGuide.Models
{
    public class CallGuideRecord
    {
        #region Public Properties

        public string ContactId { get; set; }

        public string InteractionId { get; set; }

        public string Data { get; set; }

        public string FbUrl { get; set; }

        public CallGuideBatchActivity CallGuideActivity { get; set; }

        #endregion
    }
}
