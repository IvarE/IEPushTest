using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using System.Data.SqlClient;
using Microsoft.Xrm.Sdk.Query;
using System.Data;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    static class CustomerHandler
    {
        #region Public Methods
        // TODO flytta innehåll till extern metod likt de andra?
        public static void ExecuteCustomerSyncronization(IOrganizationService service, string customerId)
        {
            try
            {
                string syncCustomerServiceUrl = Utilities.GetSetting(service, "cgi_synccustomerservice");

                SyncCustomerService.SyncCustomerRequest request = new SyncCustomerService.SyncCustomerRequest
                {
                    SyncFromCrmtoEPiRequestParameters = new SyncCustomerService.SyncFromCrmtoEPiRequestParameters
                    {
                        CustomerId = customerId // Cyrus ACC contact id "e0f9c312-d2e4-e411-80d7-005056906ae2"
                    }
                };

                BasicHttpBinding myBinding = new BasicHttpBinding
                {
                    Name = "myBasicHttpBinding"
                };
                EndpointAddress endPointAddress = new EndpointAddress(syncCustomerServiceUrl);

                SyncCustomerService.SyncCustomerClient client = new SyncCustomerService.SyncCustomerClient(myBinding, endPointAddress);

                // TODO varför inte en "vanlig" request?

                SyncCustomerService.SyncFromCrmtoEPiResponseParameters responsParameters = client.SyncCustomer(request.SyncFromCrmtoEPiRequestParameters);

                uint statusCode = responsParameters.StatusCode;

                if (statusCode != 200)
                {
                    string errorMessage = "Ett fel uppstod vid synkronisering av kund. ";
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        errorMessage += " CustomerId: ";
                        errorMessage += customerId;
                    }
                    if (!string.IsNullOrEmpty(responsParameters.Message))
                    {
                        errorMessage += " Felinformation: ";
                        errorMessage += responsParameters.Message;
                    }
                    if (!string.IsNullOrEmpty(responsParameters.ErrorMessage))
                    {
                        errorMessage += " Detaljerat felmeddelande: " + responsParameters.ErrorMessage;
                    }
                    errorMessage += " Statuskod: " + statusCode.ToString();
                    throw new InvalidPluginExecutionException(errorMessage);
                }

            }
            catch (InvalidPluginExecutionException ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i ExecuteCustomerSyncronization, customerId = {0}. Detaljerat felmeddelande: {1}", customerId, ex));
            }
        }

        public static int getNextCustomerNumber(string CounterName, PluginData data)
        {

            Entity settings = GetSettings(data);
            if (settings.Attributes.Contains("cgi_integrationdbconstr") && settings.Attributes["cgi_integrationdbconstr"] != null && ((string)settings.Attributes["cgi_integrationdbconstr"]) != "")
            {
                using (SqlConnection con = new SqlConnection((string)settings.Attributes["cgi_integrationdbconstr"]))
                {
                    SqlCommand command = new SqlCommand("sp_GetNextCustomerNumber", con) { CommandType = CommandType.StoredProcedure };
                    command.Parameters.Add(new SqlParameter("@CounterName", CounterName));
                    SqlParameter NextCustomerNumberParam = new SqlParameter() { ParameterName = "@NextCustomerNumber", DbType = DbType.Int32, Direction = ParameterDirection.Output };
                    command.Parameters.Add(NextCustomerNumberParam);
                    con.Open();
                    command.ExecuteNonQuery();
                    if (NextCustomerNumberParam.Value == DBNull.Value)
                        throw new InvalidPluginExecutionException(string.Format("sp_GetNextCustomerNumber returns DBNull for @COUnterName={0}",CounterName));
                    int NextCustomerNumber =   (int)NextCustomerNumberParam.Value;
                    con.Close();
                    return NextCustomerNumber;
                }
            }
            else
            {
                throw new InvalidPluginExecutionException("Integration Database Conectionstring Setting is missing in CRM-database.");
            }
        }
        #endregion
        private static Entity GetSettings(PluginData data)
        {
            #region FetchXML

            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_integrationdbconstr' />";
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

            FetchExpression f = new FetchExpression(xml);
            EntityCollection settings = data.Service.RetrieveMultiple(f);

            return settings.Entities.First();
        }
    }
}
