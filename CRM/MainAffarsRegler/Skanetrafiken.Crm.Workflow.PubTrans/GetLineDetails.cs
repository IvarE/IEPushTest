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
    public class GetLineDetails : CodeActivity
    {
        // TRAIN, STADSBUSS or REGIONBUS
        [Input("LineType")]
        [RequiredArgument()]
        public InArgument<string> LineType { get; set; }

        [Output("GetLineDetailsResponse")]
        public OutArgument<string> GetLineDetailsResponse { get; set; }

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
            localContext.Trace($"GetLineDetails started.");

            //GET VALUE(S)
            string lineType = LineType.Get(activityContext);

            //TRY EXECUTE
            try
            {
                string response = ExecuteCodeActivity(localContext, lineType);
                GetLineDetailsResponse.Set(activityContext, response);
            }
            catch (Exception ex)
            {
                GetLineDetailsResponse.Set(activityContext, ex.Message);
            }

            localContext.Trace($"GetLineDetails finished.");

        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string lineType)
        {
            string query = "SELECT TOP 1 [LineDetails], [LineType], [CreatedOn]" +
                           "FROM [LineDetails] " +
                           "WHERE [LineType] = '" + lineType + "' " +
                           "ORDER BY [CreatedOn] DESC";

            DataTable dataTable = new DataTable();
            using (SqlConnection conn = CreateSqlConnection(localContext))
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

            string str = "";
            foreach (DataRow row in dataTable.Rows)
                str = row["LineDetails"] as string;

            return str;
        }

        private static SqlConnection CreateSqlConnection(Plugin.LocalPluginContext localContext)
        {
            return new SqlConnection(CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_TravelInformationDBConnectionString));
        }
    }
}
