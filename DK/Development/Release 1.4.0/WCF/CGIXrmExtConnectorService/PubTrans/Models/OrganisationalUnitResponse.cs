using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [DataContract]
    public class OrganisationalUnitResponse
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [DataMember]
        public List<OrganisationalUnit> OrganisationalUnitList { get; set; }

        [DataMember]
        public string Errormessage { get; set; }
        
        #endregion
    }
}