using System.Runtime.Serialization;
using CGIXrmEAIConnectorService.Shared.Models;

namespace CGIXrmEAIConnectorService.AllBinary.Models
{
    [DataContract]
    public class GetCustomerDetailsResponse : Response
    {
        #region Public Properties

        [DataMember]
        public Customer Customer { get; set; }

        [DataMember]
        public AccountCategoryCode RequestAccountCategoryCode { get; set; }

        #endregion
    }
}