using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.ObjectModel;
using System.Linq;

using System.Text.RegularExpressions;
using System.IO;

using Microsoft.Crm.Sdk.Messages;

using CGIXrmHandler.CrmClasses;
using CGIXrmHandler.Shared;

namespace CGIXrmHandler
{
    public class PASSHandler : IDisposable
    {
        #region Declarations
        private readonly XrmManager _xrmManager;
        // Flag: Has Dispose already been called? 
        bool _disposed;
        #endregion

        #region Constructors
        public PASSHandler()
        {
            var xrmHelper = new XrmHelper();
            _xrmManager = xrmHelper.GetXrmManagerFromAppSettings();
        }

        #endregion

        #region Private Methods
        
        private EntityReference CreateIncident(Incident incident)
        {
            incident.CaseOrigin = new OptionSetValue((int)CaseOrgin.PhoneCall);
            Entity entity = _xrmManager.Create(incident);

            EntityReference team = GetDefaultTem();
            if (team != null)
            {
                AssignRequest assign = new AssignRequest
                {
                    Assignee = new EntityReference(team.LogicalName, team.Id),
                    Target = new EntityReference(entity.LogicalName, entity.Id)
                };
                _xrmManager.Service.Execute(assign);
                LogMessage("C:\\Temp\\PASSLog.txt", "Assign to team.");
            }

            return new EntityReference(entity.LogicalName, entity.Id);
        }

        private Guid CreatePASSTravelInformation(PASSTravelInformation passTravelInformation, EntityReference IncidentReference)
        {
            LogMessage("C:\\Temp\\PASSLog.txt", "CreatePASSTravelInformation");
            passTravelInformation.Case = IncidentReference;
            Entity entity = _xrmManager.Create<PASSTravelInformation>(passTravelInformation);
            return entity.Id;
        }

        private Guid _createContact(Incident incident)
        {
            Guid contactid;

            try
            {
                Entity ent = new Entity("contact");

                if (!string.IsNullOrEmpty(incident.sFN))
                    ent.Attributes.Add("firstname", incident.sFN);

                if (!string.IsNullOrEmpty(incident.sLN))
                    ent.Attributes.Add("lastname", incident.sLN);

                string fullname = _returnFullname(incident.sFN, incident.sLN);
                if (!string.IsNullOrEmpty(fullname))
                    ent.Attributes.Add("fullname", fullname);

                if (!string.IsNullOrEmpty(incident.sA))
                    ent.Attributes.Add("address1_line2", incident.sA);

                if (!string.IsNullOrEmpty(incident.sPA))
                {
                    string sPa = incident.sPA;

                    // Tar bort ev. mellanslag i postnr
                    if (incident.sPA.Length > 4)
                    {
                        if (incident.sPA[3].Equals(' '))
                            sPa = incident.sPA.Remove(3, 1);
                    }

                    string postaladdress = "";
                    string city = "";

                    if (sPa.Contains(" "))
                    {
                        string[] split = Regex.Split(sPa, " ");
                        if (split.Length == 1)
                        {
                            postaladdress = split[0];
                            city = "";
                        }

                        if (split.Length >= 2)
                        {
                            if (split[0].Length == 5)
                            {
                                postaladdress = string.Format("{0} {1}", split[0].Substring(0, 3), split[0].Substring(3, 2));
                            }
                            else
                            {
                                postaladdress = split[0];
                            }
                            foreach (string word in split.Skip(1))
                                city += word + " ";
                            city.Trim();
                        }
                        ent.Attributes.Add("address1_postalcode", postaladdress);
                        ent.Attributes.Add("address1_city", city);
                    }
                    else
                    {
                        ent.Attributes.Add("address1_postalcode", sPa);
                        ent.Attributes.Add("address1_city", "");
                    }
                }

                if (!string.IsNullOrEmpty(incident.sPH))
                    ent.Attributes.Add("telephone2", _formatPhoneNumber(incident.sPH));

                if (!string.IsNullOrEmpty(incident.sPW))
                    ent.Attributes.Add("telephone1", _formatPhoneNumber(incident.sPW));

                if (!string.IsNullOrEmpty(incident.sPM))
                    ent.Attributes.Add("telephone3", _formatPhoneNumber(incident.sPM));

                if (!string.IsNullOrEmpty(incident.sEM))
                    ent.Attributes.Add("emailaddress1", incident.sEM);

                if (!string.IsNullOrEmpty(incident.sSSN))
                {
                    ent.Attributes.Add("cgi_socialsecuritynumber", _formatSSN(incident.sSSN));
                }
                LogMessage("C:\\Temp\\PASSLog.txt", "Before create customer");
                contactid = _xrmManager.Create(ent);
                LogMessage("C:\\Temp\\PASSLog.txt", "After create customer " + contactid.ToString());
            }
            catch (Exception ex)
            {
                LogMessage("C:\\Temp\\PASSLog.txt", ex.Message);
                contactid = Guid.Empty;
            }

            return contactid;
        }

        private string _returnFullname(string fname, string lname)
        {
            string fullname = "okänd";

            if (!string.IsNullOrEmpty(fname))
                fullname = fname;

            if (!string.IsNullOrEmpty(lname))
                fullname = string.Format("{0} {1}", fullname, lname);

            return fullname;
        }

        private string _formatSSN(string ssn)
        {
            if (string.IsNullOrEmpty(ssn))
                return "";

            string socialsecuritynumber;

            try
            {
                socialsecuritynumber = ssn.Replace("-", "");
                string first;
                string last;

                if (socialsecuritynumber.Length == 10)
                {
                    first = socialsecuritynumber.Substring(0, 6);
                    last = socialsecuritynumber.Substring(6, 4);
                    socialsecuritynumber = string.Format("19{0}{1}", first, last);
                }

                if (socialsecuritynumber.Length == 12)
                {
                    first = socialsecuritynumber.Substring(0, 8);
                    last = socialsecuritynumber.Substring(8, 4);
                    socialsecuritynumber = string.Format("{0}{1}", first, last);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return socialsecuritynumber;
        }

        private string _formatPhoneNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
                return "";

            string phonenumber;

            try
            {
                phonenumber = number;

                if (number.Substring(0, 1) == "+")
                {
                    phonenumber = phonenumber.Substring(3, (number.Length - 3));
                }

                int first = 0;
                int.TryParse(phonenumber.Substring(0, 1), out first);
                if (first > 0)
                {
                    phonenumber = string.Format("0{0}", phonenumber);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return phonenumber;
        }


        #endregion

        #region Public Methods

        public Guid ExecutePASSRequest(Incident incident, PASSTravelInformation[] passTravelInformations)
        {
            LogMessage("C:\\Temp\\PASSLog.txt", "Create incident");
            EntityReference incidentReference = CreateIncident(incident);

            foreach (PASSTravelInformation passTravelInformation in passTravelInformations)
            {
                CreatePASSTravelInformation(passTravelInformation, incidentReference);
            }

            return incidentReference.Id;
        }


        public ObservableCollection<Contact> FetchContacts(Incident incident)
        {
            ObservableCollection<Contact> contacts;
            try
            {
                LogMessage("C:\\Temp\\PASSLog.txt", "Fetch contact 1");
                contacts = _xrmManager.Get<Contact>(new FetchExpression(FetchContactsXML(incident.sSSN, incident.sEM)));
                if (contacts == null || !contacts.Any())
                {
                    LogMessage("C:\\Temp\\PASSLog.txt", "Fetch contact 2");
                    Guid g = _createContact(incident);
                    LogMessage("C:\\Temp\\PASSLog.txt", "Fetch contact 3");
                    if (g != Guid.Empty)
                    {
                        LogMessage("C:\\Temp\\PASSLog.txt", "Fetch contact 4");
                        contacts = _xrmManager.Get<Contact>(new FetchExpression(FetchContactsXML(incident.sSSN, incident.sEM)));
                        LogMessage("C:\\Temp\\PASSLog.txt", "Fetch contact 5");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("C:\\Temp\\PASSLog.txt", "Fetch contact 6");
                LogMessage("C:\\Temp\\PASSLog.txt", ex.Message);
                throw new Exception(ex.Message);
            }

            return contacts;
        }

        public void LogMessage(string path, string message)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.UTF8);
                message = string.Format("{0} : {1}", DateTime.Now, message);
                sw.WriteLine(message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region GettSettings

        public EntityReference GetAnonymousCustomer()
        {
            #region FetchXML
           
            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_defaultcustomeroncase' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + now + "' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + now + "' />";
            xml += "               <condition attribute='cgi_validto' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            #endregion

            EntityCollection settings = _xrmManager.Service.RetrieveMultiple(new FetchExpression(xml));

            return settings.Entities.First().GetAttributeValue<EntityReference>("cgi_defaultcustomeroncase");
        }

        public EntityReference GetDefaultTem()
        {
            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_defaultteamonpasscase' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + now + "' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + now + "' />";
            xml += "               <condition attribute='cgi_validto' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";
                
            EntityCollection settings = _xrmManager.Service.RetrieveMultiple(new FetchExpression(xml));

            return settings.Entities.First().GetAttributeValue<EntityReference>("cgi_defaultteamonpasscase");

        }

        #endregion

        #region [FetchXML]

        // TODO : email parameter not used
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

        #endregion

        #region [IDispose]


        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }

        ~PASSHandler()
        {
            Dispose(false);
        }
        #endregion
    }
}
