using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Configuration;
using System.IO;
using Endeavor.Crm;
using Microsoft.Xrm.Tooling.Connector;

namespace CGIXrmRainDanceImport
{
    public class RunBatch
    {
        #region Declarations
        List<string> _rsidFromFile = null;
        Plugin.LocalPluginContext localContext = null;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // INFO: (hest) The entropy should be unique for each application. DON'T COPY THIS VALUE INTO A NEW PROJECT!!!!
        internal static byte[] Entropy = System.Text.Encoding.Unicode.GetBytes("RainDanceImport");

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePath);
            }
        }
        #endregion

        #region Constructors
        public RunBatch()
        {
            try
            {
                localContext = GenerateLocalContext();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Public Methods
        public bool Run()
        {
            try
            {
                string filepath = ConfigurationManager.AppSettings["FilesDir"];
                string filebackup = ConfigurationManager.AppSettings["Backup"];

                DirectoryInfo info = new DirectoryInfo(filepath);
                FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();

                if (files.Any())
                {
                    for (int x = 0; x <= (files.Length - 1); x++)
                    {
                        EntityCollection users = _getUsers();
                        string currentFile = string.Format("{0}\\{1}", filepath, files[x]);
                        _readFile(currentFile);
                        _checkUser(users);
                    }

                    for (int x = 0; x <= (files.Length - 1); x++)
                    {
                        string fromFile = string.Format("{0}\\{1}", filepath, files[x]);
                        string fromTo = string.Format("{0}\\{1}", filebackup, files[x]);
                        File.Move(fromFile, fromTo);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error Run(): " + ex.Message);
                throw new Exception("Error Run(): " + ex.Message);
            }

            return false;
        }
        #endregion

        #region Private Methods
        private static Plugin.LocalPluginContext GenerateLocalContext()
        {
            try
            {
                _log.Debug("Trying to get the Connection to Dynamics.");

                // Connect to the CRM web service using a connection string.
                CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(RunBatch.CredentialFilePath, RunBatch.Entropy));

                // Cast the proxy client to the IOrganizationService interface.
                IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                if (serviceProxy == null)
                    _log.Error("Connection to Dynamics failed.");
                else
                    _log.Error("Connection to Dynamics succeeded.");

                return new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());
            }
            catch (Exception e)
            {
                _log.Error("Error while initiating GenerateLocalContext. " + e.Message);
                throw new Exception("Error while initiating GenerateLocalContext. " + e.Message);
            }
        }

        private EntityCollection _getUsers()
        {
            EntityCollection returnvalue;

            try
            {
                string fetchXml = "";
                fetchXml += "<fetch version='1.0' mapping='logical' distinct='false'>";
                fetchXml += "  <entity name='systemuser'>";
                fetchXml += "    <attribute name='systemuserid' />";
                fetchXml += "    <attribute name='cgi_rsid' />";
                fetchXml += "    <attribute name='fullname' />";
                fetchXml += "    <filter type='and'>";
                fetchXml += "      <condition attribute='cgi_rsid' operator='not-null' />";
                fetchXml += "    </filter>";
                fetchXml += "  </entity>";
                fetchXml += "</fetch>";
                FetchExpression f = new FetchExpression(fetchXml);
                returnvalue = localContext.OrganizationService.RetrieveMultiple(f);
            }
            catch (Exception ex)
            {
                _log.Error("Error _getUsers(): " + ex.Message);
                throw new Exception("Error _getUsers(): " + ex.Message);
            }

            return returnvalue;
        }

        private void _readFile(string file)
        {
            try
            {
                _rsidFromFile = new List<string>();
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    string rsid = line.Substring(4, 6);
                    _rsidFromFile.Add(rsid);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error _readFile(): " + ex.Message);
                throw new Exception("Error _readFile(): " + ex.Message);
            }
        }

        private void _checkUser(EntityCollection users)
        {
            try
            {
                foreach (Entity user in users.Entities)
                {
                    string userrsid = user["cgi_rsid"].ToString();
                    string rsid = _rsidFromFile.FirstOrDefault(x => x == userrsid);
                    if (string.IsNullOrEmpty(rsid))
                    {
                        user.Attributes["cgi_rsid"] = null;
                        localContext.OrganizationService.Update(user);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error _checkUser(): " + ex.Message);
                throw new Exception("Error _checkUser(): " + ex.Message);
            }
        }
        #endregion
    }
}