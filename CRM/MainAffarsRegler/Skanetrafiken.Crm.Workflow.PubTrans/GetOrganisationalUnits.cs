using System;
using System.Data;
using System.Activities;
using System.Data.SqlClient;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

using Endeavor.Crm;
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.Crm
{
    public class GetOrganisationalUnits : CodeActivity
    {
        [Output("GetOrganisationalUnitsResponse")]
        public OutArgument<string> GetOrganisationalUnitsResponse { get; set; }

        private Plugin.LocalPluginContext GetLocalContext(CodeActivityContext activityContext)
        {
            IWorkflowContext workflowContext = activityContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService organizationService = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);
            ITracingService tracingService = activityContext.GetExtension<ITracingService>();

            return new Plugin.LocalPluginContext(null, organizationService, null, tracingService);
        }

        protected override void Execute(CodeActivityContext activityContext)
        {
            //GENERATE CONTEXT
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
            localContext.Trace($"GetOrganisationalUnits started.");

            //TRY EXECUTE
            try
            {
                string response = ExecuteCodeActivity(localContext);
                GetOrganisationalUnitsResponse.Set(activityContext, response);
            }
            catch (Exception ex)
            {
                GetOrganisationalUnitsResponse.Set(activityContext, ex.Message);
            }

            localContext.Trace($"GetOrganisationalUnits finished.");

        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext)
        {
            string query = "SELECT [Id], [Name], [Code], [ExistsFromDate],[ExistsUptoDate] " +
                           "FROM [OrganisationalUnit] " +
                           "FOR XML AUTO";

            DataTable dataTable = new DataTable();
            using (SqlConnection conn = CreateSqlConnectionDeltaDatabase(localContext))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dataTable);
                    conn.Close();
                    da.Dispose();
                }
            }

            string rowsxml = "";
            foreach (DataRow row in dataTable.Rows)
                rowsxml += row[0].ToString();

            return rowsxml;
        }

        private static SqlConnection CreateSqlConnectionDeltaDatabase(Plugin.LocalPluginContext localContext)
        {
            return new SqlConnection(CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_TravelInformationDBConnectionString));
        }
    }
}
