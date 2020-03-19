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
using Skanetrafiken.Crm.Properties;

namespace Skanetrafiken.Crm.Entities
{
    public class CgiSettingEntity : Generated.cgi_setting
    {

        public static int GetSettingInt(Plugin.LocalPluginContext localContext, string settingName)
        {
            // Set time valid from CgiSettings
            CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(settingName), CurrentTimeFilter());
            if (settings == null)
            {
                throw new MissingFieldException(Resources.ValidSettingsEntityMissing);
            }

            int? setting = settings.GetAttributeValue<int>(settingName);
            if (!setting.HasValue)
            {
                throw new MissingFieldException(string.Format(Resources.SettingsValueMissing, settingName));
            }
            return setting.Value;
        }

        public static decimal GetSettingDecimal(Plugin.LocalPluginContext localContext, string settingName)
        {
            // Set time valid from CgiSettings
            CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(settingName), CurrentTimeFilter());
            if (settings == null)
            {
                throw new MissingFieldException(Resources.ValidSettingsEntityMissing);
            }

            decimal? setting = settings.GetAttributeValue<decimal>(settingName);
            if (!setting.HasValue)
            {
                throw new MissingFieldException(string.Format(Resources.SettingsValueMissing, settingName));
            }
            return setting.Value;
        }


        public static string GetSettingString(Plugin.LocalPluginContext localContext, string settingName)
        {
            // Set time valid from CgiSettings
            CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(settingName), CurrentTimeFilter());
            if (settings == null)
            {
                throw new MissingFieldException(Resources.ValidSettingsEntityMissing);
            }

            string setting = settings.GetAttributeValue<string>(settingName);
            if (string.IsNullOrWhiteSpace(setting))
            {
                throw new MissingFieldException(string.Format(Resources.SettingsValueMissing, settingName));
            }
            return setting;
        }

        /// <summary>
        /// Returns a system parameter.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public static object GetSetting(Plugin.LocalPluginContext localContext, string settingName, bool allowNullValue = false)
        {
            var settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(settingName), CurrentTimeFilter());
            if (settings == null && !allowNullValue)
                throw new MissingFieldException(Resources.ValidSettingsEntityMissing);

            var setting = settings.GetAttributeValue<object>(settingName); //This will fetch correct datatype that matches the settingName.
            if (setting == null)
            {
                throw new MissingFieldException(string.Format(Resources.SettingsValueMissing, settingName));
            }
            return setting;
        }

        public static DateTime GetSettingDate(Plugin.LocalPluginContext localContext, string settingName, bool allowNullValue = false)
        {
            var settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(settingName), CurrentTimeFilter());
            if (settings == null && !allowNullValue)
                throw new MissingFieldException(Resources.ValidSettingsEntityMissing);

            var setting = settings.GetAttributeValue<DateTime>(settingName); //This will fetch correct datatype that matches the settingName.
            return setting;
        }

        public static EntityReference GetSettingEntRef(Plugin.LocalPluginContext localContext, string settingName)
        {
            CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(settingName), CurrentTimeFilter());
            if (settings == null)
            {
                throw new MissingFieldException(Resources.ValidSettingsEntityMissing);
            }
            if (settings.GetAttributeValue<EntityReference>(settingName) == null)
            {
                throw new MissingFieldException(string.Format(Resources.SettingsValueMissing, settingName));
            }
            return settings.GetAttributeValue<EntityReference>(settingName);
        }

        private static FilterExpression CurrentTimeFilter()
        {
            return new FilterExpression(LogicalOperator.And)
            {
                Conditions =
                {
                    new ConditionExpression(CgiSettingEntity.Fields.cgi_ValidFrom, ConditionOperator.OnOrBefore, DateTime.Now)
                },
                Filters =
                {
                    new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                        {
                            new ConditionExpression(CgiSettingEntity.Fields.cgi_ValidTo, ConditionOperator.Null),
                            new ConditionExpression(CgiSettingEntity.Fields.cgi_ValidTo, ConditionOperator.OnOrAfter, DateTime.Now)
                        }
                    }
                }
            };
        }
    }


}