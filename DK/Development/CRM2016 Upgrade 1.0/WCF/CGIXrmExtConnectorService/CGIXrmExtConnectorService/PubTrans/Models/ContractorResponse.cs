﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace CGIXrmExtConnectorService
{
    [DataContract]
    public class ContractorResponse
    {

        [DataMember]
        public List<Contractor> ContractorList { get; set; }

        [DataMember]
        public string Errormessage { get; set; }

    }
}