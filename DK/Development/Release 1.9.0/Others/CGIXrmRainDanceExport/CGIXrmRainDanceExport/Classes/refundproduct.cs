using CGIXrmWin;

namespace CGIXrmRainDanceExport.Classes
{
    public class refundproduct
    {
        #region Public Properties
        //<attribute name='cgi_refundproductname' />
        [Xrm("cgi_refundproductname")]
        public string Refundproductname { get; set; }

        //<attribute name='cgi_account' />
        [Xrm("cgi_account")]
        public string Account { get; set; }

        #endregion
    }
}
