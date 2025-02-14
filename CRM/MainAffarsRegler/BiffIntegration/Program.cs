﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Common.Logging;
using System.Xml.Linq;

namespace Endeavor.Crm.BiffIntegration
{
    static class Program
    {
        static ILog _log = LogManager.GetLogger(typeof(Program));
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {
                string passwordArgument = null;
                string passwordSQLArgument = null;

                string[] args = System.Environment.GetCommandLineArgs();
                if (args != null)
                {
                    var passwordArgs = args.Where(s => s.Contains("Password"));
                    if(passwordArgs.Count() > 0)
                    {
                        passwordArgument = passwordArgs.First();
                    }

                    var passwordSQLArgs = args.Where(s => s.Contains("PassSQLDatabaseUser"));
                    if (passwordSQLArgs.Count() > 0)
                    {
                        passwordSQLArgument = passwordSQLArgs.First();
                    }

                }

                if (!string.IsNullOrEmpty(passwordArgument))
                {
                    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
                    string password = passwordArgument.Substring(passwordArgument.IndexOf(":") + 1);

                    CrmConnection.SaveCredentials(BiffIntegration.CredentialFilePath, password, BiffIntegration.Entropy);
                    Console.WriteLine($"Credentials file saved! Location:{BiffIntegration.CredentialFilePath}");
                }

                if (!string.IsNullOrEmpty(passwordSQLArgument))
                {
                    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
                    string password = passwordSQLArgument.Substring(passwordSQLArgument.IndexOf(":") + 1);

                    CrmConnection.SaveCredentials(BiffIntegration.CredentialSQLFilePath, password, BiffIntegration.Entropy);
                    Console.WriteLine($"Credentials file saved! Location:{BiffIntegration.CredentialFilePath}");
                }

#if DEBUG
                //Workaround to make it possible to debug a service.
                BiffIntegration engine = new BiffIntegration();
                engine.Execute();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			    { 
				    new BiffIntegration() 
			    };
                ServiceBase.Run(ServicesToRun);
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
