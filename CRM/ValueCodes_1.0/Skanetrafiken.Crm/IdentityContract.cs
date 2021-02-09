using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.Crm
{
    [DataContract]
    public class IdentityContract
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int accountId { get; set; }
        [DataMember]
        public string locale { get; set; }
        [DataMember]
        public int userId { get; set; }
        [DataMember]
        public long exp { get; set; }
        [DataMember]
        public DateTime expirationTime { get; set; }
        [DataMember]
        public string token { get; set; }
        [DataMember]
        public Guid crmId { get; set; }

        public IdentityContract()
        {
            expirationTime = new DateTime(1970, 1, 1).AddSeconds(exp);
        }
    }

    
}
