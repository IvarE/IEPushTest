using System;
using System.Configuration;
using System.Diagnostics;
using CGI.CRM2013.Skanetrafiken.CGIXrmLogger.CrmClasses;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

namespace CGI.CRM2013.Skanetrafiken.CGIXrmLogger
{
    public class LogToCrm
    {
        #region Declarations
        readonly object _objLock = new object();
        IOrganizationService _xrmService;
        #endregion

        #region Public Methods
        public void LogMessage(string logMessage, string applicationName, IOrganizationService xrmService=null)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["LogMessage"]))
                    return;

                if (xrmService == null)
                    xrmService = XrmService;

                lock (_objLock)
                {
                    ApplicationLog applicationLog = new ApplicationLog()
                    {
                        ApplicationName = applicationName,
                        LogType = new OptionSetValue(285050001),
                        Message = logMessage
                    };
                    XrmManager xrmManager = new XrmManager(xrmService);
                    xrmManager.Create(applicationLog);
                }
            }            
            catch (Exception ex)
            {
                Log2EventViewer(ex.Message, "LogMessage", applicationName);
            }
            
        }
        public void Trace(string logMessage, string methodName, string applicationName, IOrganizationService xrmService=null)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["LogTrace"]))
                    return;

                if (xrmService == null)
                    xrmService = XrmService;

                lock (_objLock)
                {

                    ApplicationLog applicationLog = new ApplicationLog()
                    {
                        ApplicationName = applicationName,
                        LogType = new OptionSetValue(285050000),
                        Message = logMessage,
                        MethodName = methodName
                    };
                    XrmManager xrmManager = new XrmManager(xrmService);
                    xrmManager.Create(applicationLog);
                }
            }
            catch (Exception ex)
            {
                Log2EventViewer(ex.Message, methodName, applicationName);
            }
        }
        public void Exception(string logMessage, string methodName, Exception objException, string applicationName, IOrganizationService xrmService = null)
        {
            try
            {
                Console.WriteLine(string.Format($"Exception log called:{logMessage}"));

                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["LogError"]))
                    return;

                if (xrmService == null)
                    xrmService = XrmService;

                lock (_objLock)
                {

                    ApplicationLog applicationLog = new ApplicationLog()
                        {
                            ApplicationName = applicationName,
                            LogType = new OptionSetValue(285050002),
                            Message = logMessage,
                            MethodName = methodName,
                            SystemExceptionDetail = objException != null ? "\n Inner Exception \n" + objException.InnerException.Message : string.Empty
                        };
                    XrmManager xrmManager = new XrmManager(xrmService);
                    xrmManager.Create(applicationLog);
                }
            }
            catch (Exception ex)
            {
                Log2EventViewer(ex.Message, methodName, applicationName);
            }

        }

        #endregion

        #region Private Methods
        private IOrganizationService XrmService
        {
            get { return _xrmService ?? (_xrmService = GetOrganizationServiceFromAppSettings()); }
        }

        private void Log2EventViewer(string logMessage,
            string methodName,
            string applicationName)
        {
            if (!EventLog.SourceExists(applicationName))
                EventLog.CreateEventSource(applicationName, methodName);

            EventLog.WriteEntry(applicationName, logMessage, EventLogEntryType.Error);
        }
        private IOrganizationService GetOrganizationServiceFromAppSettings()
        {
            try
            {

                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                string domain = ConfigurationManager.AppSettings["Domain"];
                string username = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
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
