using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using CGIXrmWin;

namespace CGIXrmCreateCaseService.Entities
{
    public class CountryEntity : CGIXrmCreateCaseService.Schema.Generated.edp_Country
    {
        /// <summary>
        /// Get countrycode from string.
        /// If no match, return null.
        /// </summary>
        /// <param name="_xrmManager"></param>
        /// <param name="customerAddress1Country"></param>
        /// <returns></returns>
        internal static string GetCountryCodeFromName(XrmManager _xrmManager, string customerAddress1Country)
        {
            // Default to SE.
            if (string.IsNullOrEmpty(customerAddress1Country))
                return "SE";

            if (customerAddress1Country.ToUpper() == "SE")
                return "SE";

            PagingInfo page = new PagingInfo();
            page.Count = 1;
            page.PageNumber = 1;

            FilterExpression filter = new FilterExpression();
            filter.AddCondition(CountryEntity.Fields.edp_Name, ConditionOperator.Equal, customerAddress1Country);
            filter.AddCondition(CountryEntity.Fields.statecode, ConditionOperator.Equal, (int)Schema.Generated.edp_CountryState.Active);

            QueryExpression query = new QueryExpression(CountryEntity.EntityLogicalName);
            query.ColumnSet = new ColumnSet(CountryEntity.Fields.edp_CountryCode);
            query.Criteria = filter;
            query.PageInfo = page;

            EntityCollection settingscollection = _xrmManager.Service.RetrieveMultiple(query);

            if (settingscollection.Entities.Count == 0 ||
                !settingscollection[0].Contains(CountryEntity.Fields.edp_CountryCode))
            {
                return null;
            }
            else
                return (string)settingscollection[0].Attributes[CountryEntity.Fields.edp_CountryCode];

        }
    }
}