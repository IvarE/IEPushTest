using Microsoft.VisualStudio.TestTools.UnitTesting;
using CGIXrmWin;
using System.Configuration;

namespace CRM2013.SkanetrafikenPlugins
{
    [TestClass]
    public class SkanetrafikenPluginsTest
    {
        #region Declarations ----------------------------------------------------------------------------------------

        internal readonly XrmManager _manager;

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
