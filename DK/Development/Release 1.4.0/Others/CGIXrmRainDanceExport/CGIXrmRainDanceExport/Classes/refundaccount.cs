using System;
using CGIXrmWin;

namespace CGIXrmRainDanceExport.Classes
{
    public class refundaccount
    {
        #region Public Properties
        //"<attribute name='cgi_refundaccountname'
        [Xrm("cgi_refundaccountname")]
        public string Refundaccountname { get; set; }

        //"<attribute name='cgi_account'
        [Xrm("cgi_account")]
        public string Account { get; set; }

        //"<attribute name='cgi_refundaccountid'
        [Xrm("cgi_refundaccountid")]
        public Guid Refundaccountid { get; set; }

        #endregion
    }
}
