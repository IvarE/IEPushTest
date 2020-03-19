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
using System.Text.RegularExpressions;

namespace Skanetrafiken.Crm.Entities
{
    public class EmailEntity : Generated.Email
    {
        public static ColumnSet CgiSettingColumnSet = new ColumnSet(
            CgiSettingEntity.Fields.ed_EmailValidationSite
            );

        /// <summary>
        /// Replace PlaceHolders in text with attributes from entity.
        /// </summary>
        public static string ReplacePlaceHolders(Plugin.LocalPluginContext localContext, string text, Entity entity)
        {
            localContext.TracingService.Trace("Entered function ReplacePlaceHolders");
            string prefix = entity.LogicalName;


            foreach (KeyValuePair<string, object> attribute in entity.Attributes)
            {
                string textToFind = "[!" + prefix + ":" + attribute.Key + ";]";

                if (text.Contains(textToFind))
                {
                    string value = "";
                    Type attributeType = attribute.Value.GetType();

                    if (attributeType == typeof(Microsoft.Xrm.Sdk.Money))
                    {
                        value = ((Money)attribute.Value).Value.ToString();
                    }
                    else if (attributeType == typeof(Microsoft.Xrm.Sdk.OptionSetValue))
                    {
                        value = ((OptionSetValue)attribute.Value).Value.ToString();
                    }
                    else if (attributeType == typeof(Microsoft.Xrm.Sdk.EntityReference))
                    {
                        value = ((EntityReference)attribute.Value).Name.ToString();
                    }
                    else
                    {
                        value = attribute.Value.ToString();
                    }

                    text = text.Replace(textToFind, value);
                }
            }
            text = Regex.Replace(text, @"\[!" + prefix + @":[^}]+\]", "");

            return text;
        }

        /// <summary>
        /// Replace PlaceHolders in subject and body/description with attributes from entity.
        /// </summary>
        public void ReplacePlaceHolders(Plugin.LocalPluginContext localContext, Entity entity)
        {
            //LanguageEntity language = null;
            //if (refLanguage != null && refLanguage.Id != Guid.Empty)
            //{
            //    language = XrmRetrieveHelper.Retrieve<LanguageEntity>(localContext, refLanguage.Id
            //                                                , new ColumnSet(LanguageEntity.Fields.ed_MicrosoftLocale));
            //    int.TryParse(language.ed_MicrosoftLocale.ToString(), out languageCode);
            //}

            this.Subject = ReplacePlaceHolders(localContext, this.Subject, entity);
            this.Description = ReplacePlaceHolders(localContext, this.Description, entity);
            //this.Description = ReplacePlaceHoldersFixed(localContext, this.Description, entity, languageCode);
        }

        /// <summary>
        /// Create Email from template. CRM replaces placeholders for entityReference and we manually replace placeholders for placeHolderEntities
        /// </summary>
        public static EmailEntity CreateEmailFromTemplate(Plugin.LocalPluginContext localContext, TemplateEntity template, EntityReference to, IList<Entity> placeHolderEntities = null)
        {
            localContext.TracingService.Trace("CreateEmailFromTemplate");
            
            EmailEntity email = template.InstantiateTemplate(localContext, to);

            // Fill email with data
            if (placeHolderEntities != null)
            {
                foreach (Entity placeHolderEntity in placeHolderEntities)
                {
                    email.ReplacePlaceHolders(localContext, placeHolderEntity);
                }
            }

            //email.Subject = "testmail:" + email.Subject;
            //email.Id = XrmHelper.Create(localContext.OrganizationService, email);

            return email;
        }

        ///// <summary>
        ///// Create Email from template. CRM replaces placeholders for entityReference and we manually replace placeholders for placeHolderEntities
        ///// </summary>
        //public static EmailEntity CreateEmailFromTemplate(Plugin.LocalPluginContext localContext, TemplateEntity template, EntityReference entityReference, EntityReference refLanguage, IList<Entity> placeHolderEntities = null)
        //{
        //    localContext.TracingService.Trace("CreateEmailFromTemplate");

        //    TemplateEntity newTemplate = null;

        //    int languageCode = 1033;        //english
        //    LanguageEntity language = null;
        //    if (refLanguage != null && refLanguage.Id != Guid.Empty)
        //    {
        //        language = XrmRetrieveHelper.Retrieve<LanguageEntity>(localContext, refLanguage.Id
        //                                                    , new ColumnSet(LanguageEntity.Fields.ed_MicrosoftLocale));
        //        int.TryParse(language.ed_MicrosoftLocale.ToString(), out languageCode);
        //        if (template.LanguageCode != language.ed_MicrosoftLocale)
        //        {
        //            FilterExpression filter = new FilterExpression(LogicalOperator.And);
        //            filter.AddCondition(TemplateEntity.Fields.Title, ConditionOperator.Equal, template.Title);
        //            filter.AddCondition(TemplateEntity.Fields.LanguageCode, ConditionOperator.Equal, language.ed_MicrosoftLocale);
        //            newTemplate = XrmRetrieveHelper.RetrieveFirst<TemplateEntity>(localContext, new ColumnSet(true), filter);
        //        }
        //    }
        //    if (newTemplate == null)
        //        newTemplate = template;


        //    EmailEntity email = newTemplate.InstantiateTemplate(localContext, entityReference);

        //    // Fill email with data
        //    if (placeHolderEntities != null)
        //    {
        //        foreach (Entity placeHolderEntity in placeHolderEntities)
        //        {
        //            email.ReplacePlaceHolders(localContext, placeHolderEntity, refLanguage);
        //        }
        //    }

        //    //email.Subject = "testmail:" + email.Subject;
        //    //email.Id = XrmHelper.Create(localContext.OrganizationService, email);

        //    return email;
        //}
    }

    
}