using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.UECCIntegration.Model
{
    public class SkaneRequests
    {
        public List<CreateRequest> lCreateRequests { get; set; }
        public List<UpdateRequest> lUpdateRequests { get; set; }

        public SkaneRequests()
        {
            lCreateRequests = new List<CreateRequest>();
            lUpdateRequests = new List<UpdateRequest>();
        }
    }
}
