using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using CGIXrmCreateCaseService.Case.Models;

namespace CGIXrmCreateCaseService
{
    [DataContract]
    public class UpdateAutoRgCaseTravelInformationsRequest
    {
        #region Public Properties
        [DataMember]
        public Guid CaseID { get; set; }//used to get the parent Incident

        [DataMember]
        public TravelInformation[] TravelInformations { get; set; }
        #endregion
    }
}


