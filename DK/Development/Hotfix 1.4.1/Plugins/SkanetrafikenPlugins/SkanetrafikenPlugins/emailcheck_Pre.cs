using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;


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
            PluginData _data = new PluginData(serviceProvider);

            try
            {
                if (_data.Context.InputParameters.Contains("Target") && _data.Context.InputParameters["Target"] is Entity)
                {
                    _data.Target = (Entity)_data.Context.InputParameters["Target"];

                    if (_data.Target.Attributes.Contains("emailaddress1"))
                    {
                        string _email = _data.Target["emailaddress1"].ToString();

                        if (_data.Target.LogicalName == "account")
                        {
                            _checkIfEmailExistsOnAccount(_data, _email);
                        }

                        if (_data.Target.LogicalName == "contact")
                        {
                            _checkIfEmailExistsOnContact(_data, _email);
                        }
                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Private Methods
        private void _checkIfEmailExistsOnAccount(PluginData data, string email)
        {
            EntityCollection _accounts = data.Service.RetrieveMultiple(_getAccount(email));
            if (_accounts != null && _accounts.Entities.Count() > 0)
            {
                Entity _account = _accounts[0] as Entity;

                //current accountid
                Guid _g = data.Target.Id;
                //accountid on found record.
                Guid _accountid = _account.Id;
                //if _g == _accountid then we have found the same record.
                if (_g == _accountid)
                    return;

                string _name = "";
                if (_account.Attributes.Contains("name"))
                    _name = _account.Attributes["name"].ToString();

                string _email = "";
                if (_account.Attributes.Contains("emailaddress1"))
                    _email = _account.Attributes["emailaddress1"].ToString();

                string _errormessage = string.Format("Emailadresen finns redan på en annan organisation:\n{0}\n{1}", _name, _email);
                throw new InvalidPluginExecutionException(_errormessage);
            }
        }

        private void _checkIfEmailExistsOnContact(PluginData data, string email)
        {
            EntityCollection _contacts = data.Service.RetrieveMultiple(_getContact(email));
            if (_contacts != null && _contacts.Entities.Count() > 0)
            {
                Entity _contact = _contacts[0] as Entity;

                //current contactid
                Guid _g = data.Target.Id;
                //accountid on found record.
                Guid _contactid = _contact.Id;
                //if _g == _contactid then we have found the same record.
                if (_g == _contactid)
                    return;

                string _firstname = "";
                if (_contact.Attributes.Contains("firstname"))
                    _firstname = _contact.Attributes["firstname"].ToString();

                string _lastname = "";
                if (_contact.Attributes.Contains("lastname"))
                    _lastname = _contact.Attributes["lastname"].ToString();
                
                string _email = "";
                if (_contact.Attributes.Contains("emailaddress1"))
                    _email = _contact.Attributes["emailaddress1"].ToString();

                string _errormessage = string.Format("Emailadresen finns redan på en annan privatperson:\n{0} {1}\n{2}", _firstname, _lastname, _email);
                throw new InvalidPluginExecutionException(_errormessage);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // queries

        private QueryByAttribute _getAccount(string email)
        {
            QueryByAttribute _query = new QueryByAttribute("account");
            _query.ColumnSet = new ColumnSet("accountid", "emailaddress1", "name");
            _query.Attributes.Add("emailaddress1");
            _query.Values.Add(email);
            _query.Attributes.Add("statecode");
            _query.Values.Add(0);

            return _query;
        }

        private QueryByAttribute _getContact(string email)
        {
            QueryByAttribute _query = new QueryByAttribute("contact");
            _query.ColumnSet = new ColumnSet("contactid", "emailaddress1", "firstname", "lastname");
            _query.Attributes.Add("emailaddress1");
            _query.Values.Add(email);
            _query.Attributes.Add("statecode");
            _query.Values.Add(0);

            return _query;
        }
        #endregion
    }
}
