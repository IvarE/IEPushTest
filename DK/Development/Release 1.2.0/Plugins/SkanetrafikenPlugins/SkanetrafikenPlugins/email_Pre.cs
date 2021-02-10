using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Net;

namespace CRM2013.SkanetrafikenPlugins
{
    public class email_Pre : IPlugin
    {

        private class plugindata : PlugindataBase
        {
            public plugindata(IServiceProvider serviceProvider) : base(serviceProvider) { }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            plugindata _data = new plugindata(serviceProvider);
            bool needUpdate = false;

            try
            {
                if (_data.Context.InputParameters.Contains("Target") && _data.Context.InputParameters["Target"] is Entity)
                {
                    _data.Target = (Entity)_data.Context.InputParameters["Target"];

                    if (_data.Target.Attributes.Contains("from"))
                    {
                        EntityCollection from = _data.Target.GetAttributeValue<EntityCollection>("from");
                        if (from != null)
                        {
                            from.Entities.ToList().ForEach(party =>
                            {
                                EntityReference partyId = party.GetAttributeValue<EntityReference>("partyid");

                                if (partyId.LogicalName == "cgi_emailrecipient")
                                {
                                    var emailrecipientRec = _data.Service.Retrieve("cgi_emailrecipient", partyId.Id, new ColumnSet(true));
                                    String email = emailrecipientRec["cgi_EmailAddress"].ToString();

                                    QueryByAttribute _query = new QueryByAttribute("contact");
                                    _query.ColumnSet = new ColumnSet(true);
                                    _query.Attributes.Add("emailaddress1");
                                    _query.Values.Add(emailrecipientRec["cgi_EmailAddress"].ToString());
                                    Entity contact = _data.Service.RetrieveMultiple(_query).Entities.FirstOrDefault();
                           
                                    if (contact == null)
                                    {
                                        contact = new Entity("contact");
                                        contact["firstname"] = emailrecipientRec["cgi_emailrecipientname"].ToString();
                                        contact["emailaddress1"] = emailrecipientRec["cgi_EmailAddress"].ToString();
                                        _data.Service.Create(contact);
                                    }

                                    needUpdate = true;
                                    from.Entities.Remove(party);
                                    from.Entities.Add(contact);
                                }
                            });
                        }

                        if (needUpdate)
                           _data.Target.Attributes["from"] = from;
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
    }
}
