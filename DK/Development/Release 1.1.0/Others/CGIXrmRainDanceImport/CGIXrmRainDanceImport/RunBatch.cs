using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CGIXrmWin;

using Microsoft.Crm;
using Microsoft.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System.Configuration;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.IO;

namespace CGIXrmRainDanceImport
{
    public class RunBatch
    {
        XrmManager _xrmManager;
        string _fileName = "";
        List<string> _rsidFromFile;

        public RunBatch()
        {
            try
            {
                _xrmManager = _initManager();
            }
            catch
            {
                throw;
            }
        }

        
        public bool Run()
        {
            bool _ok = false;

            try
            {
                string _filepath = ConfigurationManager.AppSettings["FilesDir"].ToString();
                string _filebackup = ConfigurationManager.AppSettings["Backup"].ToString();
                
                DirectoryInfo _info = new DirectoryInfo(_filepath);
                FileInfo[] _files = _info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                
                if (_files != null && _files.Count() > 0)
                {
                    for (int x = 0; x <= (_files.Count() - 1); x++)
                    {
                        EntityCollection _users = _getUsers();
                        string _currentFile = string.Format("{0}\\{1}", _filepath, _files[x].ToString());
                        _readFile(_currentFile);
                        _checkUser(_users);
                    }

                    for (int x = 0; x <= (_files.Count() - 1); x++)
                    {
                        string _fromFile = string.Format("{0}\\{1}", _filepath, _files[x].ToString());
                        string _fromTo = string.Format("{0}\\{1}", _filebackup, _files[x].ToString());
                        File.Move(_fromFile, _fromTo);
                    }
                }
            }
            catch
            {
                throw;
            }

            return _ok;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private XrmManager _initManager()
        {
            try
            {

                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"].ToString();
                string domain = ConfigurationManager.AppSettings["Domain"].ToString();
                string username = ConfigurationManager.AppSettings["Username"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                else
                {
                    XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);
                    return xrmMgr;
                }
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }

        private EntityCollection _getUsers()
        {
            EntityCollection _returnvalue = null;

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
                FetchExpression _f = new FetchExpression(fetchXml);
                _returnvalue = _xrmManager.Service.RetrieveMultiple(_f);
            }
            catch
            {
                throw;
            }

            return _returnvalue;
        }

        private void _readFile(string file)
        {
            try
            {
                _rsidFromFile = new List<string>();
                string[] lines = System.IO.File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    string _rsid = line.Substring(4, 6);
                    _rsidFromFile.Add(_rsid);
                }
            }
            catch
            {
                throw;
            }
        }

        private void _checkUser(EntityCollection users)
        {
            try
            {
                foreach (Entity user in users.Entities)
                {
                    string _userrsid = user["cgi_rsid"].ToString();
                    string _rsid = _rsidFromFile.FirstOrDefault(x => x == _userrsid);
                    if (string.IsNullOrEmpty(_rsid))
                    {
                        user.Attributes["cgi_rsid"] = null;
                        _xrmManager.Service.Update(user);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
