using CGIXrmHandler.Shared;

namespace CGIXrmHandler.CallGuide.Models
{
    public class CallGuideRequest
    {
        #region Public Properties

        public string CallGuideSessionId { get; set; }

        public string QueueTime { get; set; }

        public string APhoneNumber { get; set; }

        public string BPhoneNumber { get; set; }

        public string ErrandTaskType { get; set; }

        public string ScreenPopChoice { get; set; }

        public string ContactSourceType { get; set; }

        public string CId { get; set; }

        public string AgentName { get; set; }

        public string CallDuration { get; set; }

        public string ChatCustomerAlias { get; set; }

        public CallDirection CallDirection { get; set; }

        public CallGuideRouteAction CallRouteAction { get; set; }

        internal AccountCategoryCode CustomerType { get; set; }

        #endregion
    }
}
