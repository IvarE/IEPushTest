using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGIXrmGetOrders
{
    [DataContract]
    public class GetContactResponse
    {
        #region Public Properties
        [DataMember]
        public string ErrorMessage { get; set; }

        private string name;
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion
    }
}