#define DEBUG_EXT_WCF
#undef DEBUG_EXT_WCF

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.ServiceModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using CGIXrm;
using CGIXrm.CrmSdk;

using CRM.Travel.Information.Classes;
using CRM.Travel.Information.PubTrans;

namespace CRM.Travel.Information.Classes
{
    public class webservice
    {

        private CrmManager _crmManager;
        public CrmManager CrmManager
        {
            get { return _crmManager; }
            set { _crmManager = value; }
        }

        private BasicHttpBinding _webserviceBinding;
        public BasicHttpBinding WebserviceBinding
        {
            get { return _webserviceBinding; }
            set { _webserviceBinding = value; }
        }

        private EndpointAddress _webserviceEndpointAddress;
        public EndpointAddress WebserviceEndpointAddress
        {
            get { return _webserviceEndpointAddress; }
            set { _webserviceEndpointAddress = value; }
        }
        
        public Action Callback_completed { get; set; }

        public webservice() { }

        public void Init()
        {
            try
            {
                _setHttpBinding();
                _getWebServiceURL();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _setHttpBinding()
        { 
            try
            {
                _webserviceBinding = new BasicHttpBinding();
                _webserviceBinding.MaxReceivedMessageSize = 2147483647;     // int.MaxValue;
                _webserviceBinding.MaxBufferSize = 2147483647;              // int.MaxValue;
                _webserviceBinding.OpenTimeout = TimeSpan.FromMinutes(10);
                _webserviceBinding.CloseTimeout = TimeSpan.FromMinutes(10);
                _webserviceBinding.ReceiveTimeout = TimeSpan.FromMinutes(10);
                _webserviceBinding.SendTimeout = TimeSpan.FromMinutes(10);
                _webserviceBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _getWebServiceURL()
        { 
            try
            {
#if DEBUG_EXT_WCF
                _webserviceEndpointAddress = new EndpointAddress("http://localhost:61340/ExtConnectorService.svc");
                Callback_completed();
#else
                _crmManager.Fetch<crmsetting>(_xmlgetWebServiceURL(), _getWebServiceURL_completed);
#endif
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _getWebServiceURL_completed(ObservableCollection<crmsetting> crmsettings)
        { 
            try
            { 
                if (crmsettings != null && crmsettings.Count > 0)
                {
                    _webserviceEndpointAddress = new EndpointAddress(crmsettings[0].PubTransWebServiceURL);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (Callback_completed != null)
                    Callback_completed();
            }
        }

        private string _xmlgetWebServiceURL()
        {
            string _xml = "";

            string _today = DateTime.Now.ToString("s");

            _xml += "<fetch version='1.0' mapping='logical'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_settingid' />";
            _xml += "       <attribute name='cgi_pubtransservice' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _today + "' />";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _today + "' />";
            _xml += "               <condition attribute='cgi_validto' operator='null' />";
            _xml += "           </filter>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            return _xml;
        }

    }
}
