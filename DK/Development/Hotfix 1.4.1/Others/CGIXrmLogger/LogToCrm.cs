using CGIXrmWin;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CGIXrmLogger
{
    public class LogToCrm
    {
        #region Global Variables
        object objLock=new object();
        OrganizationService _XrmService;
        #endregion

        #region Public Methods       

        public void LogMessage(string logMessage, string applicationName, OrganizationService xrmService=null)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["LogMessage"]))
                    return;

                if (xrmService == null)
                    xrmService = XrmService;

                lock (objLock)
                {
                    ApplicationLog applicationLog = new ApplicationLog()
                    {
                        ApplicationName = applicationName,
                        LogType = new OptionSetValue(285050001),
                        Message = logMessage
                    };
                    XrmManager xrmManager = new XrmManager(xrmService);
                    xrmManager.Create<ApplicationLog>(applicationLog);
                }
            }            
            catch (Exception ex)
            {
                Log2EventViewer(ex.Message, "LogMessage", applicationName);
            }
            
        }
        public void Trace(string logMessage, string methodName, string applicationName, OrganizationService xrmService=null)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["LogTrace"]))
                    return;

                if (xrmService == null)
                    xrmService = XrmService;

                lock (objLock)
                {

                    ApplicationLog applicationLog = new ApplicationLog()
                    {
                        ApplicationName = applicationName,
                        LogType = new OptionSetValue(285050000),
                        Message = logMessage,
                        MethodName = methodName
                    };
                    XrmManager xrmManager = new XrmManager(xrmService);
                    xrmManager.Create<ApplicationLog>(applicationLog);
                }
            }
            catch (Exception ex)
            {
                Log2EventViewer(ex.Message, methodName, applicationName);
            }
        }
        public void Exception(string logMessage, string methodName, Exception objException, string applicationName, OrganizationService xrmService = null)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["LogError"]))
                    return;

                if (xrmService == null)
                    xrmService = XrmService;

                lock (objLock)
                {

                    ApplicationLog applicationLog = new ApplicationLog()
                        {
                            ApplicationName = applicationName,
                            LogType = new OptionSetValue(285050002),
                            Message = logMessage,
                            MethodName = methodName,
                            SystemExceptionDetail = objException.Message + objException.InnerException != null ? "\n Inner Exception \n" + objException.InnerException.Message : string.Empty
                        };
                    XrmManager xrmManager = new XrmManager(xrmService);
                    xrmManager.Create<ApplicationLog>(applicationLog);
                }
            }
            catch (Exception ex)
            {
                Log2EventViewer(ex.Message, methodName, applicationName);
            }

        }

#endregion

        #region Private Methods
        private OrganizationService XrmService
        {
            get
            {
                if (_XrmService == null)
                    _XrmService = GetOrganizationServiceFromAppSettings();
                return _XrmService;
            }
            set { _XrmService = value; }
        }

        private void Log2EventViewer(string logMessage,
            string methodName,
            string applicationName)
        {
            if (!EventLog.SourceExists(applicationName))
                EventLog.CreateEventSource(applicationName, methodName);

            EventLog.WriteEntry(applicationName, logMessage, EventLogEntryType.Error);
        }
        private OrganizationService GetOrganizationServiceFromAppSettings()
        {
            try
            {

                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"].ToString();
                string domain = ConfigurationManager.AppSettings["Domain"].ToString();
                string username = ConfigurationManager.AppSettings["Username"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();
                if (String.IsNullOrEmpty(crmServerUrl) || 
                    String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                {
                    throw new Exception("Credentials Not Found in Config File");
                }
                    XrmManager xrmManager = new XrmManager(crmServerUrl, domain, username, password);
                   
                    return xrmManager.Service;
                
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }
        #endregion
    }
}
