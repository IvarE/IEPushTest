﻿using Endeavor.Crm.CleanRecordsService.PandoraExtensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;

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
                string runInactivatePermits = ConfigurationManager.AppSettings["runInactivatePermits"];
                string runDeleteQueueItems = ConfigurationManager.AppSettings["runDeleteQueueItems"];
                string runDeleteMarketingLists = ConfigurationManager.AppSettings["runDeleteMarketingLists"];
                string runInactivateDeceasedContacts = ConfigurationManager.AppSettings["runInactivateDeceasedContacts"];

                string passwordArgument = null; //password place holder

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

                if (!string.IsNullOrEmpty(passwordArgument) && runInactivatePermits == "true")
                {
                    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
                    string password = passwordArgument.Substring(passwordArgument.IndexOf(":") + 1);

                    CrmConnection.SaveCredentials(PermitsService.CredentialFilePath, password, PermitsService.Entropy);
                }

                if (!string.IsNullOrEmpty(passwordArgument) && runDeleteQueueItems == "true")
                {
                    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
                    string password = passwordArgument.Substring(passwordArgument.IndexOf(":") + 1);

                    CrmConnection.SaveCredentials(QueueItemsService.CredentialFilePath, password, QueueItemsService.Entropy);
                }

                if (!string.IsNullOrEmpty(passwordArgument) && runDeleteMarketingLists == "true")
                {
                    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
                    string password = passwordArgument.Substring(passwordArgument.IndexOf(":") + 1);

                    CrmConnection.SaveCredentials(MarketingListsService.CredentialFilePath, password, MarketingListsService.Entropy);
                }

                if (!string.IsNullOrEmpty(passwordArgument) && runInactivateDeceasedContacts == "true")
                {
                    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
                    string password = passwordArgument.Substring(passwordArgument.IndexOf(":") + 1);

                    CrmConnection.SaveCredentials(DeceasedContactsService.CredentialFilePath, password, DeceasedContactsService.Entropy);
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

                if(runInactivatePermits == "true")
                {
                    _log.Info($"Running Permits Service...");
                    PermitsService service = new PermitsService();
                    service.Execute();
                }

                if (runDeleteQueueItems == "true")
                {
                    _log.Info($"Running QueueItems Service...");
                    QueueItemsService service = new QueueItemsService();
                    service.Execute();
                }

                if (runDeleteMarketingLists == "true")
                {
                    _log.Info($"Running MarketingLists Service...");
                    MarketingListsService service = new MarketingListsService();
                    service.Execute();
                }

                if (runInactivateDeceasedContacts == "true")
                {
                    _log.Info($"Running Deceased Contacts Service...");
                    DeceasedContactsService service = new DeceasedContactsService();
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

                if(runInactivatePermits == "true")
                {
                    _log.Info($"Running Permits Service...");
                    servicesToRun.Add(new PermitsService());
                }

                if (runDeleteQueueItems == "true")
                {
                    _log.Info($"Running QueueItems Service...");
                    servicesToRun.Add(new QueueItemsService());
                }

                if (runDeleteMarketingLists == "true")
                {
                    _log.Info($"Running MarketingLists Service...");
                    servicesToRun.Add(new MarketingListsService());
                }

                if (runInactivateDeceasedContacts == "true")
                {
                    _log.Info($"Running Deceased Contacts Service...");
                    servicesToRun.Add(new DeceasedContactsService());
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
