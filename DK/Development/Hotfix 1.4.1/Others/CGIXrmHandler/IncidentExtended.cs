using CGIXrmHandler.CrmClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmHandler
{
    public class IncidentExtended : Incident
    {
        #region Public Properties
        // nulable didnt work in CgiXrmWin attribute decorated classes
        // always serialize to false, even with defaultvalue attribute 
        // to fix CgiXrmWin needs to be handeling bool? diffrently
        // therefor subclassed for this specific instance
        private bool _cgi_ContactCustomer;
        [Xrm("cgi_contactcustomer")]
        public bool ContactCustomer
        {
            get { return _cgi_ContactCustomer; }
            set { _cgi_ContactCustomer = value; }

        }
        #endregion
    }
}
