using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace CGIXrmExtConnectorService
{
    [DataContract]
    public class OrganisationalUnitResponse
    {
        #region Public Properties
        [DataMember]
        public List<OrganisationalUnit> OrganisationalUnitList { get; set; }

        [DataMember]
        public string Errormessage { get; set; }
        #endregion
    }
}