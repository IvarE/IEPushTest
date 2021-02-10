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
    public class invoicerecipient
    {


        [Xrm("cgi_invoicerecipientid", DecodePart = XrmDecode.Value)]
        public Guid? cgi_invoicerecipientid
        {
            get;
            set;
        }



        [Xrm("cgi_address1")]
        public string cgi_address1 { get; set; }
        [Xrm("cgi_customer_no")]
        public string cgi_customer_no { get; set; }
        [Xrm("cgi_inv_reference")]
        public string cgi_inv_reference { get; set; }
        [Xrm("cgi_invoicerecipientname")]
        public string cgi_invoicerecipientname { get; set; }
        [Xrm("cgi_postal_city")]
        public string cgi_postal_city { get; set; }
        [Xrm("cgi_postalcode")]
        public string cgi_postalcode { get; set; }


    }
}
