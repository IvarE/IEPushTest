using System;
using System.Collections.Generic;
using System.Linq;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Configuration;
using System.IO;

namespace CGIXrmRainDanceImport
{
    public class RunBatch
    {
        #region Declarations
        readonly XrmManager _xrmManager;
        List<string> _rsidFromFile;
        #endregion

        #region Constructors
        public RunBatch()
        {
            try
            {
                _xrmManager = _initManager();
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return false;
        }
        #endregion

        #region Private Methods
        private XrmManager _initManager()
        {
            try
            {

                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                string domain = ConfigurationManager.AppSettings["Domain"];
                string username = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);
                return xrmMgr;
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
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
                returnvalue = _xrmManager.Service.RetrieveMultiple(f);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
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
                        _xrmManager.Service.Update(user);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
