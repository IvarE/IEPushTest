using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Endeavor.Crm.CleanRecordsService
{
    static class Program
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            try
            {
                _log.Debug("Main Started");
                string runCloseCases = ConfigurationManager.AppSettings["runCloseCases"];
                string runInactivateContacts = ConfigurationManager.AppSettings["runInactivateContacts"];
                string runDeleteAudits = ConfigurationManager.AppSettings["runDeleteAudits"];

                string passwordArgument = "uSEme2!nstal1"; //passwordplace holder

                string[] args = Environment.GetCommandLineArgs();
                if (args != null)
                {
                    var passwordArgs = args.Where(s => s.Contains("Password:"));
                    if (passwordArgs.Count() > 0)
                        passwordArgument = passwordArgs.First();
                }

                if (!string.IsNullOrEmpty(passwordArgument) && runCloseCases == "true")
                {
                    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
                    string password = passwordArgument.Substring(passwordArgument.IndexOf(":") + 1);

                    CrmConnection.SaveCredentials(CasesService.CredentialFilePath, password, CasesService.Entropy);
                }

                if (!string.IsNullOrEmpty(passwordArgument) && runInactivateContacts == "true")
                {
                    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
                    string password = passwordArgument.Substring(passwordArgument.IndexOf(":") + 1);

                    CrmConnection.SaveCredentials(ContactsService.CredentialFilePath, password, ContactsService.Entropy);
                }

                if (!string.IsNullOrEmpty(passwordArgument) && runDeleteAudits == "true")
                {
                    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
                    string password = passwordArgument.Substring(passwordArgument.IndexOf(":") + 1);

                    CrmConnection.SaveCredentials(AuditsService.CredentialFilePath, password, AuditsService.Entropy);
                }

#if DEBUG
                //Workaround to make it possible to debug a service.
                if (runCloseCases == "true")
                {
                    _log.Info($"Running Cases Service...");
                    CasesService service = new CasesService();
                    service.Execute();
                }

                if (runInactivateContacts == "true")
                {
                    _log.Info($"Running Contacts Service...");
                    ContactsService service = new ContactsService();
                    service.Execute();
                }

                if (runDeleteAudits == "true")
                {
                    _log.Info($"Running Audits Service...");
                    AuditsService service = new AuditsService();
                    service.Execute();
                }

                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
                List<ServiceBase> servicesToRun = new List<ServiceBase>();

                if (runCloseCases == "true")
                {
                    _log.Info($"Running Cases Service...");
                    servicesToRun.Add(new CasesService());
                }

                if (runInactivateContacts == "true")
                {
                    _log.Info($"Running Contacts Service...");
                    servicesToRun.Add(new ContactsService());
                }

                if (runDeleteAudits == "true")
                {
                    _log.Info($"Running Audits Service...");
                    servicesToRun.Add(new AuditsService());
                }

                ServiceBase.Run(servicesToRun.ToArray());
#endif
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
