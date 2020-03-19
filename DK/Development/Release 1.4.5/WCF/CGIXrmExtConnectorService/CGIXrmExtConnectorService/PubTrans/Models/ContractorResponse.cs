using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [DataContract]
    public class ContractorResponse
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [DataMember]
        public List<Contractor> ContractorList { get; set; }


        [DataMember]
        public string Errormessage { get; set; }

        #endregion
    }
}