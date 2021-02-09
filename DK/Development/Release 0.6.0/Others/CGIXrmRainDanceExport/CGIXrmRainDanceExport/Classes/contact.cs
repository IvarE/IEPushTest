using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CGIXrmWin;
using Microsoft.Crm;
using Microsoft.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System.Configuration;
using System.ServiceModel;

namespace CGIXrmRainDanceExport
{
    public class contact
    {

        //<attribute name='contactid' />";
        private Guid _contactid;
        [Xrm("contactid")]
        public Guid Contactid
        {
            get { return _contactid; }
            set { _contactid = value; }
        }
        
        //<attribute name='lastname' />";
        private string _lastname;
        [Xrm("lastname")]
        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }
        
        //<attribute name='firstname' />";
        private string _firstname;
        [Xrm("firstname")]
        public string Firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }

        //<attribute name='address1_line1' />";
        private string _address1_line1;
        [Xrm("address1_line2")]
        public string Address1_line1
        {
            get { return _address1_line1; }
            set { _address1_line1 = value; }
        }

        //<attribute name='address1_city' />";
        private string _address1_city;
        [Xrm("address1_city")]
        public string Address1_city
        {
            get { return _address1_city; }
            set { _address1_city = value; }
        }

        //<attribute name='address1_postalcode' />";
        private string _address1_postalcode;
        [Xrm("address1_postalcode")]
        public string Address1_postalcode
        {
            get { return _address1_postalcode; }
            set { _address1_postalcode = value; }
        }

    }
}
