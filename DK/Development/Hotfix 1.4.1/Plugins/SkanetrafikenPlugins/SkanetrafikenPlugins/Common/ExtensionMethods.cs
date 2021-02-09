using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    // Not good to add specific class methods on Entity.
    public static class ExtensionMethods
    {
        #region Public Methods
        public static bool RefundedViaSms(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_couponsms") && reimbursementForm.GetAttributeValue<bool>("cgi_couponsms") == true;
        }

        public static bool RefundedViaEmail(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_couponemail") && reimbursementForm.GetAttributeValue<bool>("cgi_couponemail") == true;
        }

        public static bool RefundedViaLoadCard(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_loadcard") && reimbursementForm.GetAttributeValue<bool>("cgi_loadcard") == true;
        }

        public static bool RefundedViaGiftCard(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_giftcard") && reimbursementForm.GetAttributeValue<bool>("cgi_giftcard") == true;
        }
        #endregion
    }
}
