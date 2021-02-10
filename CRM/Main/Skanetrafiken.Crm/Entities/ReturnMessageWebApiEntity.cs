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
    public class ReturnMessageWebApiEntity : Generated.ed_ReturnMessageWebAPI
    {
        /// <summary>
        /// Fetches value of given field as string.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public static string GetValueString(Plugin.LocalPluginContext localContext, string settingName)
        {
            ReturnMessageWebApiEntity settings = XrmRetrieveHelper.RetrieveFirst<ReturnMessageWebApiEntity>(localContext, new ColumnSet(settingName));
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
    }
}
