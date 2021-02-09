using System;
using System.Collections.Generic;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;

using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Linq;

using System.Text.RegularExpressions;
using System.IO;

using Microsoft.Crm.Sdk.Messages;

namespace CGIXrmHandler
{
    public class PASSHandler : IDisposable
    {

        private XrmManager xrmManager;
        private XrmHelper xrmHelper;

        #region [Constructor]
        public PASSHandler()
        {
            xrmHelper = new XrmHelper();
            xrmManager = xrmHelper.GetXrmManagerFromAppSettings();
        }

        #endregion

        #region [Private Methods]
        
        private EntityReference CreateIncident(CGIXrmHandler.PASS.CrmClasses.Incident incident)
        {
            incident.CaseOrigin = new Microsoft.Xrm.Sdk.OptionSetValue((int)CaseOrgin.PhoneCall);
            Entity entity = xrmManager.Create<CGIXrmHandler.PASS.CrmClasses.Incident>(incident);

            EntityReference _team = GetDefaultTem();
            if (_team != null)
            {
                AssignRequest assign = new AssignRequest
                {
                    Assignee = new EntityReference(_team.LogicalName, _team.Id),
                    Target = new EntityReference(entity.LogicalName, entity.Id)
                };
                xrmManager.Service.Execute(assign);
                LogMessage("C:\\Temp\\PASSLog.txt", "Assign to team.");
            }

            return new EntityReference(entity.LogicalName, entity.Id);
        }

        private Guid CreatePASSTravelInformation(CGIXrmHandler.PASS.CrmClasses.PASSTravelInformation passTravelInformation, EntityReference IncidentReference)
        {
            LogMessage("C:\\Temp\\PASSLog.txt", "CreatePASSTravelInformation");
            passTravelInformation.Case = IncidentReference;
            Entity entity = xrmManager.Create<CGIXrmHandler.PASS.CrmClasses.PASSTravelInformation>(passTravelInformation);
            return entity.Id;
        }

        #endregion

        #region [Public Methods]

        public Guid ExecutePASSRequest(CGIXrmHandler.PASS.CrmClasses.Incident incident, CGIXrmHandler.PASS.CrmClasses.PASSTravelInformation[] passTravelInformations)
        {
            LogMessage("C:\\Temp\\PASSLog.txt", "Create incident");
            EntityReference incidentReference = CreateIncident(incident);

            foreach (CGIXrmHandler.PASS.CrmClasses.PASSTravelInformation passTravelInformation in passTravelInformations)
            {
                CreatePASSTravelInformation(passTravelInformation, incidentReference);
            }

            return incidentReference.Id;
        }

        /*public CGIXrmHandler.PASS.CrmClasses.Account TryFetchAccount(string ssn, string email, string name)
        {
            CGIXrmHandler.PASS.CrmClasses.Account[] accounts;
            ObservableCollection<CGIXrmHandler.PASS.CrmClasses.Account> retrievedAccounts = xrmManager.Get<CGIXrmHandler.PASS.CrmClasses.Account>(new FetchExpression(FetchAccountsXML(ssn, email, name)));
            accounts = retrievedAccounts.Where(x => ssn.Equals(x.SocialSecurityNumber) || email.Equals(x.Email)).ToArray();
            if (accounts.Length > 1)
                throw new Exception("Duplicate account was found!");
            else if (accounts.Length == 1)
                return accounts[0];
            else
            {
                accounts = retrievedAccounts.Where(x => name.Equals(x.Name)).ToArray();
                if (accounts.Length > 1)
                    throw new Exception("Duplicate account was found!");
                else if (accounts.Length == 1)
                    return accounts[0];
                else
                    return null;
            }
        }*/

        public ObservableCollection<PASS.CrmClasses.Contact> FetchContacts(PASS.CrmClasses.Incident incident)
        {
            ObservableCollection<PASS.CrmClasses.Contact> _contacts = new ObservableCollection<PASS.CrmClasses.Contact>();
            try
            {
                LogMessage("C:\\Temp\\PASSLog.txt", "Fetch contact 1");
                _contacts = xrmManager.Get<PASS.CrmClasses.Contact>(new FetchExpression(FetchContactsXML(incident.sSSN, incident.sEM)));
                if (_contacts == null || _contacts.Count() == 0)
                {
                    LogMessage("C:\\Temp\\PASSLog.txt", "Fetch contact 2");
                    Guid _g = _createContact(incident);
                    LogMessage("C:\\Temp\\PASSLog.txt", "Fetch contact 3");
                    if (_g != Guid.Empty)
                    {
                        LogMessage("C:\\Temp\\PASSLog.txt", "Fetch contact 4");
                        _contacts = xrmManager.Get<PASS.CrmClasses.Contact>(new FetchExpression(FetchContactsXML(incident.sSSN, incident.sEM)));
                        LogMessage("C:\\Temp\\PASSLog.txt", "Fetch contact 5");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("C:\\Temp\\PASSLog.txt", "Fetch contact 6");
                LogMessage("C:\\Temp\\PASSLog.txt", ex.Message);
                throw ex;
            }

            return _contacts;
        }

        private Guid _createContact(PASS.CrmClasses.Incident incident)
        {
            Guid _contactid = Guid.Empty;

            try
            {
                Entity _ent = new Entity("contact");
                
                if (!string.IsNullOrEmpty(incident.sFN))
                    _ent.Attributes.Add("firstname", incident.sFN);
                
                if (!string.IsNullOrEmpty(incident.sLN))
                    _ent.Attributes.Add("lastname", incident.sLN);

                string _fullname = _returnFullname(incident.sFN, incident.sLN);
                if (!string.IsNullOrEmpty(_fullname))
                    _ent.Attributes.Add("fullname", _fullname);

                if (!string.IsNullOrEmpty(incident.sA))
                    _ent.Attributes.Add("address1_line2", incident.sA);

                if (!string.IsNullOrEmpty(incident.sPA))
                {
                    string _postaladdress = "";
                    string _city = "";

                    if (incident.sPA.Contains(" "))
                    {
                        string[] _split = Regex.Split(incident.sPA, " ");
                        if (_split.Count() == 1)
                        {
                            _postaladdress = _split[0];
                            _city = "";
                        }

                        if (_split.Count() == 2)
                        {
                            if (_split[0].Length == 5)
                            {
                                _postaladdress = string.Format("{0} {1}", _split[0].Substring(0, 3), _split[0].Substring(3, 2));
                            }
                            else
                            {
                                _postaladdress = _split[0];
                            }
                            _city = _split[1];
                        }
                        _ent.Attributes.Add("address1_postalcode", _postaladdress);
                        _ent.Attributes.Add("address1_city", _city);
                    }
                    else
                    {
                        _ent.Attributes.Add("address1_postalcode", incident.sPA);
                        _ent.Attributes.Add("address1_city", "");
                    }
                }

                if (!string.IsNullOrEmpty(incident.sPH))
                    _ent.Attributes.Add("telephone2", _formatPhoneNumber(incident.sPH));

                if (!string.IsNullOrEmpty(incident.sPW))
                    _ent.Attributes.Add("telephone1", _formatPhoneNumber(incident.sPW));

                if (!string.IsNullOrEmpty(incident.sPM))
                    _ent.Attributes.Add("telephone3", _formatPhoneNumber(incident.sPM));

                if (!string.IsNullOrEmpty(incident.sEM))
                    _ent.Attributes.Add("emailaddress1", incident.sEM);

                if (!string.IsNullOrEmpty(incident.sSSN))
                {
                    _ent.Attributes.Add("cgi_socialsecuritynumber", _formatSSN(incident.sSSN));
                }
                LogMessage("C:\\Temp\\PASSLog.txt", "Before create customer");
                _contactid = xrmManager.Create(_ent);
                LogMessage("C:\\Temp\\PASSLog.txt", "After create customer " + _contactid.ToString());
            }
            catch (Exception ex)
            {
                LogMessage("C:\\Temp\\PASSLog.txt", ex.Message);
                _contactid = Guid.Empty;
            }

            return _contactid;
        }

        private string _returnFullname(string fname, string lname)
        {
            string _fullname = "okänd";

            if (!string.IsNullOrEmpty(fname))
                _fullname = fname;

            if (!string.IsNullOrEmpty(lname))
                _fullname = string.Format("{0} {1}", _fullname, lname);

            return _fullname;
        }

        /*public PASS.CrmClasses.Account FetchAccount(string name)
        {
            return xrmManager.Get<PASS.CrmClasses.Account>(new FetchExpression(FetchAccountXML(name))).FirstOrDefault<PASS.CrmClasses.Account>();
        }*/

        public void LogMessage(string path, string message)
        {
            try
            {
                StreamWriter _sw = new StreamWriter(path, true, System.Text.Encoding.UTF8);
                string _message = string.Format("{0} : {1}", DateTime.Now, message);
                _sw.WriteLine(_message);
                _sw.Flush();
                _sw.Close();
            }
            catch
            {
                throw;
            }
        }

        private string _formatSSN(string ssn)
        {
            if (string.IsNullOrEmpty(ssn))
                return "";

            string _socialsecuritynumber = "";

            try
            {
                _socialsecuritynumber = ssn.Replace("-", "");
                string _first = "";
                string _last = "";

                if (_socialsecuritynumber.Length == 10)
                {
                    _first = _socialsecuritynumber.Substring(0, 6);
                    _last = _socialsecuritynumber.Substring(6, 4);
                    _socialsecuritynumber = string.Format("19{0}{1}", _first, _last);
                }

                if (_socialsecuritynumber.Length == 12)
                {
                    _first = _socialsecuritynumber.Substring(0, 8);
                    _last = _socialsecuritynumber.Substring(8, 4);
                    _socialsecuritynumber = string.Format("{0}{1}", _first, _last);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return _socialsecuritynumber;
        }

        private string _formatPhoneNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
                return "";

            string _phonenumber = "";

            try
            {
                _phonenumber = number;

                if (number.Substring(0, 1) == "+")
                {
                    _phonenumber = _phonenumber.Substring(3, (number.Length - 3));
                }

                int _first = 0;
                int.TryParse(_phonenumber.Substring(0, 1), out _first);
                if (_first > 0)
                {
                    _phonenumber = string.Format("0{0}", _phonenumber);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return _phonenumber;
        }


        #endregion

        #region GettSettings

        public EntityReference GetAnonymousCustomer()
        {
            #region FetchXML
            
            /*
            string FetchXML = String.Format(
            "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
              "<entity name='cgi_setting'>" +
                "<attribute name='cgi_defaultcustomeroncase' />" +
                "<filter type='and'>" +
                  "<condition attribute='cgi_name' operator='eq' value='{0}' />" +
                "</filter>" +
              "</entity>" +
            "</fetch>", "Skånetrafiken 101");
            */

            string _now = DateTime.Now.ToString("s");
            string _xml = "";
            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_defaultcustomeroncase' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _now + "' />";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _now + "' />";
            _xml += "               <condition attribute='cgi_validto' operator='null' />";
            _xml += "           </filter>";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            #endregion

            EntityCollection settings = xrmManager.Service.RetrieveMultiple(new FetchExpression(_xml));

            return settings.Entities.First().GetAttributeValue<EntityReference>("cgi_defaultcustomeroncase");
        }

        public EntityReference GetDefaultTem()
        {
            string _now = DateTime.Now.ToString("s");
            string _xml = "";
            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_defaultteamonpasscase' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _now + "' />";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _now + "' />";
            _xml += "               <condition attribute='cgi_validto' operator='null' />";
            _xml += "           </filter>";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";
                
            EntityCollection settings = xrmManager.Service.RetrieveMultiple(new FetchExpression(_xml));

            return settings.Entities.First().GetAttributeValue<EntityReference>("cgi_defaultteamonpasscase");

        }

        #endregion

        #region [FetchXML]

        private string FetchContactsXML(string ssn, string email)
        {
            return string.Format(
            "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
              "<entity name='contact'>" +
                "<attribute name='contactid' />" +
                "<attribute name='cgi_socialsecuritynumber' />" +
                "<attribute name='emailaddress1' />" +
                "<filter type='and'>" +
                  "<filter type='or'>" +
                    "<condition attribute='cgi_socialsecuritynumber' operator='eq' value='{0}' />" +
                  "</filter>" +
                "</filter>" +
              "</entity>" +
            "</fetch>", ssn);
        }

       /* private string FetchAccountXML(string name)
        {
            return string.Format(
            "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
              "<entity name='account'>" +
                "<attribute name='accountid' />" +
                "<filter type='and'>" +
                  "<condition attribute='name' operator='eq' value='{0}' />" +
                "</filter>" +
              "</entity>" +
            "</fetch>", name);
        }*/

        /*private string FetchAccountsXML(string ssn, string email, string name)
        {
            return string.Format(
            "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
              "<entity name='account'>" +
                "<attribute name='accountid' />" +
                "<attribute name='cgi_socialsecuritynumber' />" +
                "<attribute name='emailaddress1' />" +
                "<attribute name='name' />" +
                "<filter type='and'>" +
                  "<filter type='or'>" +
                    "<filter type='or'>" +
                      "<condition attribute='cgi_socialsecuritynumber' operator='eq' value='{0}' />" +
                      "<condition attribute='emailaddress1' operator='eq' value='{1}' />" +
                    "</filter>" +
                    "<condition attribute='name' operator='eq' value='{2}' />" +
                  "</filter>" +
                "</filter>" +
              "</entity>" +
            "</fetch>", ssn, email, name);
        }*/

        #endregion

        #region [IDispose]
        // Flag: Has Dispose already been called? 
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
            }

            // Free any unmanaged objects here. 
            //
            disposed = true;
        }

        ~PASSHandler()
        {
            Dispose(false);
        }
        #endregion



    }
}
