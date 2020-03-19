using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm.Extensions;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class SystemParameterEntity : Generated.ed_systemparameter
    {
        public static void HandlePreSystemParameterCreate(Plugin.LocalPluginContext localContext, SystemParameterEntity target)
        {
            if (!target.Contains(SystemParameterEntity.Fields.ed_Enabled))
                target.ed_Enabled = true;
            if (!target.Contains(SystemParameterEntity.Fields.ed_LeadValidityHours))
                target.ed_LeadValidityHours = 48;
        }

        // Get 
        public static EntityReference GetParameterLookupValue(Plugin.LocalPluginContext localContext, Guid currentUsrId, string parameterName)
        {
            // Get the appropriate parameter record
            SystemParameterEntity param = SystemParameterEntity.GetParameters(localContext, parameterName);

            if (param == null)
                return null;

            return (EntityReference)param[parameterName];
        }

        // Get a string value
        public static string GetParameterStringValue(Plugin.LocalPluginContext localContext, Guid currentUsrId, string parameterName)
        {
            // Get the appropriate parameter record
            SystemParameterEntity param = SystemParameterEntity.GetParameters(localContext, parameterName);

            if (param == null)
                return null;

            return (string)param[parameterName];
        }

        // Get a optionset value
        public static OptionSetValue GetParameterOptionSetValue(Plugin.LocalPluginContext localContext, Guid currentUsrId, string parameterName)
        {
            // Get the appropriate parameter record
            SystemParameterEntity param = SystemParameterEntity.GetParameters(localContext, parameterName);

            if (param == null)
                return null;

            return (OptionSetValue)param[parameterName];
        }

        // Get 
        public static int? GetParameterIntValue(Plugin.LocalPluginContext localContext, Guid currentUsrId, string parameterName)
        {
            // Get the appropriate parameter record
            SystemParameterEntity param = SystemParameterEntity.GetParameters(localContext, parameterName);

            if (param == null)
                return null;

            return (int)param[parameterName];
        }

        public static Money GetParameterMoneyValue(Plugin.LocalPluginContext localContext, Guid currentUsrId, string parameterName)
        {
            // Get the appropriate parameter record
            SystemParameterEntity param = SystemParameterEntity.GetParameters(localContext, parameterName);

            if (param == null)
                return null;

            return (Money)param[parameterName];
        }

        public static bool? GetParameterBoolValue(Plugin.LocalPluginContext localContext, Guid currentUsrId, string parameterName)
        {
            // Get the appropriate parameter record
            SystemParameterEntity param = SystemParameterEntity.GetParameters(localContext, parameterName);

            if (param == null)
                return null;

            return (bool)param[parameterName];
        }



        /// <summary>
        /// Get the appropriate parameter record
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        private static SystemParameterEntity GetParameters(Plugin.LocalPluginContext localContext, string parameterName)
        {
            // Get all parameter records
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(SystemParameterEntity.Fields.statecode, ConditionOperator.Equal, (int)Schema.Generated.ed_systemparameterState.Active);
            // Parameter is enabled
            filter.AddCondition(SystemParameterEntity.Fields.ed_Enabled, ConditionOperator.Equal, true);
            IList<SystemParameterEntity> records = XrmRetrieveHelper.RetrieveMultiple<SystemParameterEntity>(
                localContext,
                new ColumnSet(
                    parameterName),
                filter);

            // Did we get any result
            if (records.Count == 0)
            {
                records = new List<SystemParameterEntity>();
                SystemParameterEntity def = new SystemParameterEntity();
                def.Id = localContext.OrganizationService.Create(def);
                records.Add(def);
            }
            else if (records.Count > 1)
            {
                throw new NotImplementedException("Support for multiple parameters is not implemented.");

                // Get BU of user

                // Do we have parameter for user & BU

                // Do we have parameter for BU

                // Do we have parameter for parent bu (recursive)

            }

            // Is key in result, if not it's null
            if (records[0].Contains(parameterName))
                return records[0];
            else
                return null;
        }
    }

    
}