using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRM2013.SkanetrafikenPlugins
{
    public class emailcheck_Pre : IPlugin
    {
        #region Public Methods
        // This plugin is executed on both account and contact.
        // If the user is trying to save an emailaddress that allready exists
        // it will display an error.
        // The plugin is using the same attribute (emailaddress1) on account and contact. 
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData data = new PluginData(serviceProvider);

            try
            {
                if (data.Context.InputParameters.Contains("Target") && data.Context.InputParameters["Target"] is Entity)
                {
                    data.Target = (Entity)data.Context.InputParameters["Target"];

                    if (data.Target.Attributes.Contains("emailaddress1"))
                    {
                        if(data.Target["emailaddress1"] != null)
                        {
                            string email = data.Target["emailaddress1"].ToString();

                            if (data.Target.LogicalName == "account")
                            {
                                _checkIfEmailExistsOnAccount(data, email);
                            }

                            // Funktions skall inte användas längre.
                            // Denna logik ligger i CRMPlus-Fasad
                            //if (data.Target.LogicalName == "contact")
                            //{
                            //    _checkIfEmailExistsOnContact(data, email);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        #endregion

        #region Private Methods
        private void _checkIfEmailExistsOnAccount(PluginData data, string email)
        {
            EntityCollection accounts = data.Service.RetrieveMultiple(_getAccount(email));
            if (accounts != null && accounts.Entities.Any())
            {
                Entity account = accounts[0];

                //current accountid
                Guid g = data.Target.Id;
                //accountid on found record.
                Guid accountid = account.Id;
                //if _g == _accountid then we have found the same record.
                if (g == accountid)
                    return;

                string name = "";
                if (account.Attributes.Contains("name"))
                    name = account.Attributes["name"].ToString();

                string emailStr = "";
                if (account.Attributes.Contains("emailaddress1"))
                    emailStr = account.Attributes["emailaddress1"].ToString();

                string errormessage = string.Format("Emailadresen finns redan på en annan organisation:\n{0}\n{1}", name, emailStr);
                throw new InvalidPluginExecutionException(errormessage);
            }
        }

        private void _checkIfEmailExistsOnContact(PluginData data, string email)
        {
            EntityCollection contacts = data.Service.RetrieveMultiple(_getContact(email));
            if (contacts != null && contacts.Entities.Any())
            {
                Entity contact = contacts[0];

                //current contactid
                Guid g = data.Target.Id;
                //accountid on found record.
                Guid contactid = contact.Id;
                //if _g == _contactid then we have found the same record.
                if (g == contactid)
                    return;

                string firstname = "";
                if (contact.Attributes.Contains("firstname"))
                    firstname = contact.Attributes["firstname"].ToString();

                string lastname = "";
                if (contact.Attributes.Contains("lastname"))
                    lastname = contact.Attributes["lastname"].ToString();
                
                string emailStr = "";
                if (contact.Attributes.Contains("emailaddress1"))
                    emailStr = contact.Attributes["emailaddress1"].ToString();

                string errormessage = string.Format("Emailadresen finns redan på en annan privatperson:\n{0} {1}\n{2}", firstname, lastname, emailStr);
                throw new InvalidPluginExecutionException(errormessage);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // queries

        private QueryByAttribute _getAccount(string email)
        {
            QueryByAttribute query = new QueryByAttribute("account")
            {
                ColumnSet = new ColumnSet("accountid", "emailaddress1", "name")
            };
            query.Attributes.Add("emailaddress1");
            query.Values.Add(email);
            query.Attributes.Add("statecode");
            query.Values.Add(0);

            return query;
        }

        private QueryByAttribute _getContact(string email)
        {
            QueryByAttribute query = new QueryByAttribute("contact")
            {
                ColumnSet = new ColumnSet("contactid", "emailaddress1", "firstname", "lastname")
            };
            query.Attributes.Add("emailaddress1");
            query.Values.Add(email);
            query.Attributes.Add("statecode");
            query.Values.Add(0);

            return query;
        }
        #endregion
    }
}
