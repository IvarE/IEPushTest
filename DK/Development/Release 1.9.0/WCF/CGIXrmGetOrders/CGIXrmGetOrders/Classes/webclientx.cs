using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net;

namespace CGIXrmGetOrders
{
    public class webclientx : WebClient
    {
        private int _timeout;
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            request.Timeout = _timeout;
            return request;
        }
    }
}