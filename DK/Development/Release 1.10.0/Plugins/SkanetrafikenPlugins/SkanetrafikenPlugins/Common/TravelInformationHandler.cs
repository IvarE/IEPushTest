using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    /// <summary>
    /// Used by travelinformation_Post and travelinformation_Delete
    /// </summary>
    public static class TravelInformationHandler
    {
        #region Public Methods
        public static bool IsRelatedToCase(this Entity travelInformation)
        {
            return travelInformation.Attributes.ContainsKey("cgi_caseid");
        }

        public static DataCollection<Entity> GetRelatedTravelInfos(EntityReference crmCaseId, IOrganizationService service)
        {
            QueryExpression relatedTravelInfosQuery = CreateQueryForAllTravelInformationsRelatedToCase(crmCaseId.Id);

            EntityCollection relatedTravelInfos = service.RetrieveMultiple(relatedTravelInfosQuery);

            return relatedTravelInfos.Entities;
        }

        public static void UpdateCase(Entity crmCase, IOrganizationService service)
        {
            service.Update(crmCase);
        }

        public static Entity CreateCaseWithUpdatedTravelInformation(Entity travelInformation)
        {
            Entity crmCase = new Entity("incident")
            {
                Id = ((EntityReference) travelInformation.Attributes["cgi_caseid"]).Id
            };


            crmCase["cgi_travelinformationlookup"] = new EntityReference
            {
                LogicalName = "cgi_travelinformation",
                Id = (Guid)travelInformation.Attributes["cgi_travelinformationid"]
            };

            crmCase["cgi_travelinformation"] = travelInformation.GetValue<string>("cgi_travelinformation");
            crmCase["cgi_travelinformationtransport"] = travelInformation.GetValue<string>("cgi_transport");
            crmCase["cgi_travelinformationarrivalactual"] = travelInformation.GetValue<string>("cgi_arivalactual");
            crmCase["cgi_travelinformationcity"] = travelInformation.GetValue<string>("cgi_city");
            crmCase["cgi_travelinformationcompany"] = travelInformation.GetValue<string>("cgi_contractor");
            crmCase["cgi_travelinformationdeviationmessage"] = travelInformation.GetValue<string>("cgi_deviationmessage");
            crmCase["cgi_travelinformationdirectiontext"] = travelInformation.GetValue<string>("cgi_directiontext");
            crmCase["cgi_travelinformationline"] = travelInformation.GetValue<string>("cgi_line");
            crmCase["cgi_travelinformationstart"] = travelInformation.GetValue<string>("cgi_start");
            crmCase["cgi_travelinformationstartactual"] = travelInformation.GetValue<string>("cgi_startactual");
            crmCase["cgi_travelinformationstop"] = travelInformation.GetValue<string>("cgi_stop");
            crmCase["cgi_travelinformationtour"] = travelInformation.GetValue<string>("cgi_tour");
            crmCase["cgi_travelinformationdisplaytext"] = travelInformation.GetValue<string>("cgi_displaytext");
            crmCase["cgi_train"] = travelInformation.GetValue<string>("cgi_journeynumber");

            //Using GetValue when these dates are missing sets these dates to 00010101. We want them to be empty which is why we can't use GetValue here
            crmCase["cgi_travelinformationarrivalplanned"] = travelInformation.Attributes.ContainsKey("cgi_arivalplanned") ? (DateTime?)travelInformation.Attributes["cgi_arivalplanned"] : null;
            crmCase["cgi_travelinformationstartplanned"] = travelInformation.Attributes.ContainsKey("cgi_startplanned") ? (DateTime?)travelInformation.Attributes["cgi_startplanned"] : null;

            return crmCase;
        }

        public static Entity CreateCaseWithEmptyTravelInformation(Entity travelInformation)
        {
            Entity crmCase = new Entity("incident")
            {
                Id = ((EntityReference) travelInformation.Attributes["cgi_caseid"]).Id
            };


            crmCase["cgi_travelinformationlookup"] = null;
            crmCase["cgi_travelinformation"] = null;
            crmCase["cgi_travelinformationtransport"] = null;
            crmCase["cgi_travelinformationarrivalactual"] = null;
            crmCase["cgi_travelinformationcity"] = null;
            crmCase["cgi_travelinformationcompany"] = null;
            crmCase["cgi_travelinformationdeviationmessage"] = null;
            crmCase["cgi_travelinformationdirectiontext"] = null;
            crmCase["cgi_travelinformationline"] = null;
            crmCase["cgi_travelinformationstart"] = null;
            crmCase["cgi_travelinformationstartactual"] = null;
            crmCase["cgi_travelinformationstop"] = null;
            crmCase["cgi_travelinformationtour"] = null;
            crmCase["cgi_travelinformationdisplaytext"] = null;
            crmCase["cgi_travelinformationarrivalplanned"] = null;
            crmCase["cgi_travelinformationstartplanned"] = null;

            return crmCase;
        }

        public static bool canceledBeforeStart(this Entity travelInformation)
        {   
            return travelInformation.GetAttributeValue<bool>("cgi_failedtodepart");
        }

        public static bool canceledBeforeArrival(this Entity travelInformation)
        {
            return travelInformation.GetAttributeValue<bool>("cgi_failedtoarrive");
        }
        #endregion

        #region Private Methods
        private static QueryExpression CreateQueryForAllTravelInformationsRelatedToCase(Guid crmCaseId)
        {
            ConditionExpression condition = new ConditionExpression
            {
                AttributeName = "cgi_caseid",
                Operator = ConditionOperator.Equal,
            };
            condition.Values.Add(crmCaseId.ToString());

            QueryExpression relatedTravelInfosQuery = new QueryExpression
            {
                ColumnSet = new ColumnSet("cgi_travelinformation",
                    "cgi_caseid",
                    "cgi_travelinformationid",
                    "cgi_transport",
                    "cgi_arivalactual",
                    "cgi_city",
                    "cgi_contractor",
                    "cgi_deviationmessage",
                    "cgi_directiontext",
                    "cgi_line",
                    "cgi_start",
                    "cgi_startactual",
                    "cgi_stop",
                    "cgi_tour",
                    "cgi_displaytext",
                    "cgi_arivalplanned",
                    "cgi_startplanned"),
                EntityName = "cgi_travelinformation"
            };
            relatedTravelInfosQuery.Criteria.AddCondition(condition);

            return relatedTravelInfosQuery;
        }
        #endregion
    }
}
