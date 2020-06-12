using Endeavor.Crm;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.MultiQService
{
    static class Program
    {
        static ILog _log = LogManager.GetLogger(typeof(Program));

        static void Main()
        {
            try
            {
                _log.Debug("Main Started");

                string passwordArgument = null;
                string passwordCreditsafeArgument = null;

                string[] args = System.Environment.GetCommandLineArgs();
                if (args != null)
                {
                    var passwordArgs = args.Where(s => s.Contains("Password:"));
                    if (passwordArgs.Count() > 0)
                    {
                        passwordArgument = passwordArgs.First();
                    }
                    var passwordCreditsafeArgs = args.Where(s => s.Contains("PasswordCreditsafe:"));
                    if (passwordCreditsafeArgs.Count() > 0)
                    {
                        passwordCreditsafeArgument = passwordCreditsafeArgs.First();
                    }
                }

                if (!string.IsNullOrEmpty(passwordArgument))
                {
                    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
                    string password = passwordArgument.Substring(passwordArgument.IndexOf(":") + 1);

                    CrmConnection.SaveCredentials(OrdersService.CredentialFilePath, password, OrdersService.Entropy);
                }
                //if (!string.IsNullOrEmpty(passwordCreditsafeArgument))
                //{
                //    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
                //    string password = passwordCreditsafeArgument.Substring(passwordCreditsafeArgument.IndexOf(":") + 1);

                //    CrmConnection.SaveCredentials(OrdersService.CreditsafeCredentialFilePath, password, OrdersService.Entropy);
                //}

#if DEBUG
                //Workaround to make it possible to debug a service.
                OrdersService service = new OrdersService();
                service.Execute();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			    { 
				    new OrdersService() 
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
