using System.Runtime.Serialization;
using CGICRMPortalService.Shared.Models;

namespace CGICRMPortalService.Customer.Models
{
    [DataContract]
    public class GetCustomerResponse:Response
    {
        #region Public Properties
        [DataMember]
        public CGICRMPortalService.Customer.Models.Customer Customer { get; set; }
        #endregion
    }
}