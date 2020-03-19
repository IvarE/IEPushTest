using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace Endeavor.Crm
{
    /// <summary>
    /// The XrmMetadataHelper class simplifies the retrieval of metadata from Crm.
    /// </summary>
    internal static class XrmMetaDataHelper
    {
        /// <summary>
        /// Get the value for a specific option label.
        /// </summary>
        public static int GetOptionValue(Plugin.LocalPluginContext localContext, string entity, string attribute, string label)
        {
            return GetOption(localContext, entity, attribute, label).Value.Value;
        }
        /// <summary>
        /// Get the label for a specific option value.
        /// </summary>
        public static string GetOptionLabel(Plugin.LocalPluginContext localContext, string entity, string attribute, int value)
        {
            OptionMetadata[] optionList = GetOptions(localContext, entity, attribute);
            return GetOption(localContext, optionList, value).Label.UserLocalizedLabel.Label.ToString();
        }
        /// <summary>
        /// Get the metadata for a specific option label.
        /// </summary>
        public static OptionMetadata GetOption(Plugin.LocalPluginContext localContext, string entity, string attribute, string label)
        {
            OptionMetadata[] optionList = GetOptions(localContext, entity, attribute);
            return GetOption(localContext, optionList, label);
        }
        /// <summary>
        /// Get the metadata for a specific option label.
        /// </summary>
        public static OptionMetadata GetOption(Plugin.LocalPluginContext localContext, OptionMetadata[] optionList, string label)
        {
            foreach (OptionMetadata omd in optionList)
            {
                if (omd.Label.UserLocalizedLabel.Label == label)
                {
                    return omd;
                }
            }
            throw new Exception(string.Format("Option label {0} not found.", label));
        }
        /// <summary>
        /// Get the metadata for a specific option value.
        /// </summary>
        public static OptionMetadata GetOption(Plugin.LocalPluginContext localContext, OptionMetadata[] optionList, int value)
        {
            foreach (OptionMetadata omd in optionList)
            {
                if (omd.Value == value)
                {
                    return omd;
                }
            }
            throw new Exception(string.Format("Option value {0} not found.", value));
        }
        /// <summary>
        /// Get Metadata for option set.
        /// </summary>
        public static OptionMetadata[] GetOptions(Plugin.LocalPluginContext localContext, string entity, string attribute)
        {
            RetrieveAttributeRequest retrieveAttributeRequest = new
            RetrieveAttributeRequest
            {
                EntityLogicalName = entity,
                LogicalName = attribute,
                RetrieveAsIfPublished = true
            };

            // Execute the request.
            RetrieveAttributeResponse retrieveAttributeResponse = (RetrieveAttributeResponse)localContext.OrganizationService.Execute(retrieveAttributeRequest);

            // Access the retrieved attribute.
            // Get the current options list for the retrieved attribute.
            if (retrieveAttributeResponse.AttributeMetadata is Microsoft.Xrm.Sdk.Metadata.PicklistAttributeMetadata ||
                retrieveAttributeResponse.AttributeMetadata is Microsoft.Xrm.Sdk.Metadata.StateAttributeMetadata ||
                retrieveAttributeResponse.AttributeMetadata is Microsoft.Xrm.Sdk.Metadata.StatusAttributeMetadata)
            {
                Microsoft.Xrm.Sdk.Metadata.EnumAttributeMetadata retrievedPicklistAttributeMetadata =
                    (Microsoft.Xrm.Sdk.Metadata.EnumAttributeMetadata)retrieveAttributeResponse.AttributeMetadata;

                return retrievedPicklistAttributeMetadata.OptionSet.Options.ToArray();
            }
            else if (retrieveAttributeResponse.AttributeMetadata is Microsoft.Xrm.Sdk.Metadata.BooleanAttributeMetadata)
            {
                Microsoft.Xrm.Sdk.Metadata.BooleanAttributeMetadata retrievedPicklistAttributeMetadata =
                    (Microsoft.Xrm.Sdk.Metadata.BooleanAttributeMetadata)retrieveAttributeResponse.AttributeMetadata;

                OptionMetadata[] list = new OptionMetadata[2];
                list[0] = retrievedPicklistAttributeMetadata.OptionSet.TrueOption;
                list[1] = retrievedPicklistAttributeMetadata.OptionSet.FalseOption;
                return list;
            }

            throw new Exception(string.Format("Options not found for entity {0} and field {1}.", entity, attribute));
        }

        /// <summary>
        /// Get Metadata for status option set.
        /// </summary>
        public static StatusOptionMetadata[] GetStatusOptions(Plugin.LocalPluginContext localContext, string entity, string attribute)
        {
            RetrieveAttributeRequest retrieveAttributeRequest = new
            RetrieveAttributeRequest
            {
                EntityLogicalName = entity,
                LogicalName = attribute,
                RetrieveAsIfPublished = true
            };

            // Execute the request.
            RetrieveAttributeResponse retrieveAttributeResponse = (RetrieveAttributeResponse)localContext.OrganizationService.Execute(retrieveAttributeRequest);

            // Access the retrieved attribute.
            // Get the current options list for the retrieved attribute.
            if (retrieveAttributeResponse.AttributeMetadata is Microsoft.Xrm.Sdk.Metadata.StatusAttributeMetadata)
            {
                AttributeMetadata attrMetadata = (AttributeMetadata)retrieveAttributeResponse.AttributeMetadata;
                StatusAttributeMetadata statusMetadata = (StatusAttributeMetadata)attrMetadata;
                List<StatusOptionMetadata> list = new List<StatusOptionMetadata>(statusMetadata.OptionSet.Options.Count);
                foreach (StatusOptionMetadata optionMeta in statusMetadata.OptionSet.Options)
                {
                    list.Add(optionMeta);
                }
                return list.ToArray();
            }

            throw new Exception(string.Format("Status options not found for entity {0} and field {1}.", entity, attribute));
        }

        public static EntityMetadata GetEntity(Plugin.LocalPluginContext localContext, string entity, EntityFilters typeOfMetadata)
        {
            RetrieveEntityRequest retrieveRequest = new RetrieveEntityRequest
            {
                EntityFilters = typeOfMetadata,
                LogicalName = entity
            };
            RetrieveEntityResponse retrieveEntityResponse = (RetrieveEntityResponse)localContext.OrganizationService.Execute(retrieveRequest);
            return retrieveEntityResponse.EntityMetadata;
        }

        public static string GetEntityPrimaryIdAttribute(Plugin.LocalPluginContext localContext, string entity)
        {
            return GetEntity(localContext, entity, EntityFilters.Entity).PrimaryIdAttribute;
        }

        /// <summary>
        /// Retrieves the logical attribute names of an entity
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="entity">The logical name</param>
        /// <param name="onlyValidForAdvancedFind">Only return the attributes that is valid for advanced find (optional, true by default)</param>
        /// <returns>A list of attributes</returns>
        public static List<string> GetAttributeNames(Plugin.LocalPluginContext localContext, string entity, bool onlyValidForAdvancedFind = true)
        {
            EntityMetadata entityMetadata = GetEntity(localContext, entity, EntityFilters.Attributes);

            List<string> attributes = new List<string>();
            foreach (AttributeMetadata attribute in entityMetadata.Attributes)
            {
                if (attribute.IsValidForAdvancedFind.Value)
                {
                    attributes.Add(attribute.LogicalName);
                }
                else if (!onlyValidForAdvancedFind)
                {
                    attributes.Add(attribute.LogicalName);
                }
            }
            return attributes;
        }
    }
}
