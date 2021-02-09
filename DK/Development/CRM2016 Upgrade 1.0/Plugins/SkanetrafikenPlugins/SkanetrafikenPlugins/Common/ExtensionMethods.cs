using Microsoft.Xrm.Sdk;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    // Not good to add specific class methods on Entity.
    public static class ExtensionMethods
    {
        #region Public Methods
        public static bool RefundedViaSms(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_couponsms") && reimbursementForm.GetAttributeValue<bool>("cgi_couponsms");
        }

        public static bool RefundedViaEmail(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_couponemail") && reimbursementForm.GetAttributeValue<bool>("cgi_couponemail");
        }

        public static bool RefundedViaClearon(this Entity reimbursementForm)
        {
            if (reimbursementForm.Contains("ed_clearonvaluecode") && reimbursementForm.GetAttributeValue<bool>("ed_clearonvaluecode") == true)
                return true;
            else
                return false;
        }

        public static bool RefundedViaLoadCard(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_loadcard") && reimbursementForm.GetAttributeValue<bool>("cgi_loadcard");
        }

        public static bool RefundedViaGiftCard(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_giftcard") && reimbursementForm.GetAttributeValue<bool>("cgi_giftcard");
        }
        public static bool RefundedViaPayment(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_payment") && reimbursementForm.GetAttributeValue<bool>("cgi_payment");
        }
        public static bool RefundedViaPaymentAbroad(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_payment_abroad") && reimbursementForm.GetAttributeValue<bool>("cgi_payment_abroad");
        }
        public static bool RefundedViaPrint(this Entity reimbursementForm)
        {
            return reimbursementForm.Contains("cgi_print") && reimbursementForm.GetAttributeValue<bool>("cgi_print");
        }
        #endregion
    }
}
