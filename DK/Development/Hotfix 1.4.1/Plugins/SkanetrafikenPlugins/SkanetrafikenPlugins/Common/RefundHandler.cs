using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    /// <summary>
    /// Used by refund_Post
    /// </summary>
    public static class RefundHandler
    {
        #region Public Methods
        public static bool IsRelatedToCase(this Entity refund)
        {
            return refund.Attributes.ContainsKey("cgi_caseid");
        }

        public static DataCollection<Entity> GetRelatedRefundInfos(EntityReference crmCaseId, IOrganizationService service)
        {
            QueryExpression relatedRefundInfosQuery = CreateQueryForAllRefundInformationsRelatedToCase(crmCaseId.Id);

            EntityCollection relatedRefundInfos = service.RetrieveMultiple(relatedRefundInfosQuery);

            return relatedRefundInfos.Entities;
        }

        public static void UpdateCase(Entity crmCase, IOrganizationService service)
        {
            service.Update(crmCase);
        }

        public static Entity CreateCaseWithUpdatedRefundInformation(Entity refund)
        {
            Entity crmCase = new Entity("incident");

            crmCase.Id = ((EntityReference)refund.Attributes["cgi_caseid"]).Id;

            //RefundTypes, RefundTransportCompanyId and RefundReimbursementForm are lookups and requires more than GetValue
            crmCase["cgi_refundtypes"] = new EntityReference
            {
                LogicalName = "cgi_refundtype",
                Id = ((EntityReference)refund.Attributes["cgi_refundtypeid"]).Id
            };

            if (refund.Attributes.Contains("cgi_reimbursementformid"))
            {
                crmCase["cgi_refundreimbursementform"] = new EntityReference
                {
                    LogicalName = "cgi_reimbursementform",
                    Id = ((EntityReference)refund.Attributes["cgi_reimbursementformid"]).Id
                };
            }

            if (refund.Attributes.Contains("cgi_transportcompanyid"))
            {
                crmCase["cgi_refundtransportcompanyid"] = new EntityReference
                {
                    LogicalName = "cgi_refundtransportcompanyid",
                    Id = ((EntityReference)refund.Attributes["cgi_refundtransportcompanyid"]).Id
                };
            }

            //Test if new refund contains attributes, then copy
            if (refund.Attributes.Contains("cgi_milage"))
            {
                crmCase["cgi_refundmilagekm"] = refund.GetValue<decimal>("cgi_milage");
            }
            if (refund.Attributes.Contains("cgi_milage_compensation"))
            {
                crmCase["cgi_refundmilagecompensation"] = refund.GetValue<decimal>("cgi_milage_compensation");
            }
            if (refund.Attributes.Contains("cgi_quantity"))
            {
                crmCase["cgi_refundquantity"] = refund.GetValue<int>("cgi_quantity");
            }
            if (refund.Attributes.Contains("cgi_calculated_amount"))
            {
                crmCase["cgi_refundcalculatedamount"] = refund.GetValue<decimal>("cgi_calculated_amount");
            }
            if (refund.Attributes.Contains("cgi_amount"))
            {
                crmCase["cgi_refundamount"] = refund.GetValue<Money>("cgi_amount");
            }
            if (refund.Attributes.Contains("cgi_value_code"))
            {
                crmCase["cgi_refundvaluecode"] = refund.GetValue<string>("cgi_value_code");
            }
            if (refund.Attributes.Contains("cgi_travelcard_number"))
            {
                crmCase["cgi_refundtravelcardno"] = refund.GetValue<string>("cgi_travelcard_number");
            }
            if (refund.Attributes.Contains("cgi_checknumber"))
            {
                crmCase["cgi_refundcheckno"] = refund.GetValue<string>("cgi_checknumber");
            }
            if (refund.Attributes.Contains("cgi_comments"))
            {
                crmCase["cgi_refundcomments"] = refund.GetValue<string>("cgi_comments");
            }
            if (refund.Attributes.Contains("cgi_last_valid"))
            {
                crmCase["cgi_refundlastvalid"] = refund.GetValue<DateTime>("cgi_last_valid");
            }
            if (refund.Attributes.Contains("cgi_accountno"))
            {
                crmCase["cgi_refundaccountno"] = refund.GetValue<string>("cgi_accountno");
            }

            return crmCase;
        }

        public static Entity CreateCaseWithEmptyRefundInformation(Entity travelInformation)
        {
            Entity crmCase = new Entity("incident");

            crmCase.Id = ((EntityReference)travelInformation.Attributes["cgi_caseid"]).Id;

            crmCase["cgi_RefundTypes"] = null;
            crmCase["cgi_RefundReimbursementForm"] = null;
            crmCase["cgi_RefundMilagekm"] = null;
            crmCase["cgi_RefundMilageCompensation"] = null;
            crmCase["cgi_RefundQuantity"] = null;
            crmCase["cgi_RefundCalculatedAmount"] = null;
            crmCase["cgi_RefundAmount"] = null;
            crmCase["cgi_RefundValueCode"] = null;
            crmCase["cgi_RefundTravelCardNo"] = null;
            crmCase["cgi_RefundLastValid"] = null;
            crmCase["cgi_RefundAccountNo"] = null;

            return crmCase;
        }
        #endregion

        #region Private Methods
        private static QueryExpression CreateQueryForAllRefundInformationsRelatedToCase(Guid crmCaseId)
        {
            ConditionExpression condition = new ConditionExpression
            {
                AttributeName = "cgi_caseid",
                Operator = ConditionOperator.Equal,
            };
            condition.Values.Add(crmCaseId.ToString());

            QueryExpression relatedRefundInfosQuery = new QueryExpression
            {
                ColumnSet = new ColumnSet("cgi_refund",
                    "cgi_refundtypeid",
                    "cgi_reimbursementformid",
                    "cgi_milage",
                    "cgi_milage_compensation",
                    "cgi_quantity",
                    "cgi_calculated_amount",
                    "cgi_amount",
                    "cgi_value_code",
                    "cgi_travelcard_number",
                    "cgi_last_valid",
                    "cgi_accountno"),


                EntityName = "cgi_refund"
            };
            relatedRefundInfosQuery.Criteria.AddCondition(condition);

            return relatedRefundInfosQuery;
        }
        #endregion
    }
}
