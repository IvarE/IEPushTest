using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CGIXrmWin;
using System.Configuration;
using System.Linq;
using CRM2013.SkanetrafikenPlugins;

namespace CRM2013.SkanetrafikenPlugins
{
    [TestClass]
    public class SkanetrafikenPluginsTest
    {
        #region Declarations ----------------------------------------------------------------------------------------

        private readonly XrmManager _manager;
        private readonly account_Create_Pre _accountCreate;
        private readonly account_Post _accountPost;
        private readonly cgi_casecategory_Pre _caseCategoryPre;
        private readonly cgi_travelcard_Post _travelCardPost;
        private readonly cgi_travelcard_Pre _travelCardPre;
        private readonly contact_Create_Pre _contactCreatePre;
        private readonly contact_Post _contactPost;
        private readonly email_Post _emailPost;
        private readonly email_Pre _emailPre;
        private readonly refund_Post _refundPost;
        private readonly refund_Pre _refundPre;
        private readonly refund_update _refundUpdate;
        private readonly refund_updatePreValidation _refundUpdatePre;

        #endregion

        #region Public Methods --------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SkanetrafikenPluginsTest()
        {
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];
            var domain = ConfigurationManager.AppSettings["Domain"];
            var serverAdress = ConfigurationManager.AppSettings["CrmAdress"];

            _manager = new XrmManager(serverAdress, domain, username, password);
        }

        #endregion

        #region Private Methods -------------------------------------------------------------------------------------
        
        #endregion
    }
}
