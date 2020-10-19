using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Common.Logging;
using System.Xml.Linq;

namespace Endeavor.Crm.MultiQService
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

                string[] args = Environment.GetCommandLineArgs();
                if (args != null)
                {
                    var passwordArgs = args.Where(s => s.Contains("Password:"));
                    if (passwordArgs.Count() > 0)
                        passwordArgument = passwordArgs.First();
                }

                if (!string.IsNullOrEmpty(passwordArgument))
                {
                    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
                    string password = passwordArgument.Substring(passwordArgument.IndexOf(":") + 1);

                    CrmConnection.SaveCredentials(OrdersService.CredentialFilePath, password, OrdersService.Entropy);
                }

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
