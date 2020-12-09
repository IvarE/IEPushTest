using Endeavor.Crm;
using System;
using System.Globalization;
using System.Linq;

namespace CGIXrmRainDanceImport
{
    class Program
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Main
        static void Main(string[] args)
        {
            try
            {
                _log.Debug("Main Started");

                string passwordArgument = "uSEme2!nstal1";

                if (args != null)
                {
                    var passwordArgs = args.Where(s => s.Contains("Password:"));
                    if (passwordArgs.Count() > 0)
                        passwordArgument = passwordArgs.First();
                }

                if (!string.IsNullOrEmpty(passwordArgument))
                {
                    _log.DebugFormat(CultureInfo.InvariantCulture, "Credentials parsed from command line.");
                    string password = passwordArgument.Substring(passwordArgument.IndexOf(":") + 1);

                    CrmConnection.SaveCredentials(RunBatch.CredentialFilePath, password, RunBatch.Entropy);
                }

                RunBatch run = new RunBatch();
                run.Run();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                throw;
            }

        }
        #endregion
    }
}
