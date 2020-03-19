using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRM2013.SkanetrafikenPlugins
{
    public class email_Pre : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData data = new PluginData(serviceProvider);
            bool needUpdate = false;

            try
            {
                if (data.Context.InputParameters.Contains("Target") && data.Context.InputParameters["Target"] is Entity)
                {
                    data.Target = (Entity)data.Context.InputParameters["Target"];

                    if (data.Target.Attributes.Contains("from"))
                    {
                        EntityCollection from = data.Target.GetAttributeValue<EntityCollection>("from");
                        if (from != null)
                        {
                            from.Entities.ToList().ForEach(party =>
                            {
                                EntityReference partyId = party.GetAttributeValue<EntityReference>("partyid");

                                if (partyId.LogicalName == "cgi_emailrecipient")
                                {
                                    var emailrecipientRec = data.Service.Retrieve("cgi_emailrecipient", partyId.Id, new ColumnSet(true));

                                    QueryByAttribute query = new QueryByAttribute("contact")
                                    {
                                        ColumnSet = new ColumnSet(true)
                                    };
                                    query.Attributes.Add("emailaddress1");
                                    query.Values.Add(emailrecipientRec["cgi_EmailAddress"].ToString());
                                    Entity contact = data.Service.RetrieveMultiple(query).Entities.FirstOrDefault();
                           
                                    if (contact == null)
                                    {
                                        contact = new Entity("contact");
                                        contact["firstname"] = emailrecipientRec["cgi_emailrecipientname"].ToString();
                                        contact["emailaddress1"] = emailrecipientRec["cgi_EmailAddress"].ToString();
                                        data.Service.Create(contact);
                                    }

                                    needUpdate = true;
                                    from.Entities.Remove(party);
                                    from.Entities.Add(contact);
                                }
                            });
                        }

                        if (needUpdate)
                           data.Target.Attributes["from"] = from;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        #endregion
    }
}
