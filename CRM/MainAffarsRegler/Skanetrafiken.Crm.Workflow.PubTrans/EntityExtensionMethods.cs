
// This file is maintained through Endeavor NuGet. Please do not modify it directly in your project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk;


namespace Endeavor.Crm.Extensions
{
    /// <summary>
    /// Extensions to the Entity Class.
    /// </summary>
    public static class EntityExtension
    {
        public const string AliasedSeparator = ".";

        /// <summary>
        /// Combine Entity Attributes. The entities must have the same logical names.
        /// If the same attribute exists the the passed entity attribute value overrides this attribute value.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="source"></param>
        public static void CombineAttributes(this Entity entity, Entity source)
        {
            if (entity.LogicalName != source.LogicalName)
                throw new InvalidPluginExecutionException(string.Format("The passed entity {0} must have the same logical name as this entity {1}.", entity.LogicalName, source.LogicalName));

            foreach (KeyValuePair<string, object> attribute in source.Attributes)
            {
                entity[attribute.Key] = attribute.Value;
            }
        }

        /// <summary>
        /// Check if an attribute is modified in this (target) entity compared with preImage entity
        /// </summary>
        /// <param name="target"></param>
        /// <param name="preImage"></param>
        /// <param name="attributeName"></param>
        public static bool IsAttributeModified(this Entity target, Entity preImage, string attributeName)
        {
            List<string> specificAttributesList = new List<string>();
            specificAttributesList.Add(attributeName);

            AttributeCollection modifiedAttributes = target.GetModifiedAttributes(preImage, specificAttributesList);
            return modifiedAttributes.Count > 0;
        }

        /// <summary>
        /// Check if any supplied attribute is modified in this (target) entity compared with preImage entity
        /// </summary>
        /// <param name="target"></param>
        /// <param name="preImage"></param>
        /// <param name="specificAttributes"></param>
        /// <returns></returns>
        public static bool IsAnyAttributeModified(this Entity target, Entity preImage, params string[] specificAttributes)
        {
            AttributeCollection modifiedAttributes = target.GetModifiedAttributes(preImage, specificAttributes);
            return modifiedAttributes.Count > 0;
        }

        /// <summary>
        /// /// Get modified attributes in this (target) entity compared with preImage entity
        /// </summary>
        /// <param name="target"></param>
        /// <param name="preImage"></param>
        /// <param name="specificAttributes">List of attributes to check, if empty all modified attributes will be returned</param>
        /// <returns></returns>
        public static AttributeCollection GetModifiedAttributes(this Entity target, Entity preImage, params string[] specificAttributes)
        {
            List<string> specificAttributesList = new List<string>();
            if (specificAttributes != null)
            {
                specificAttributesList = specificAttributes.ToList<string>();
            }
            return target.GetModifiedAttributes(preImage, specificAttributesList);
        }

        /// <summary>
        /// Get modified attributes in this (target) entity compared with preImage entity
        /// </summary>
        /// <param name="target"></param>
        /// <param name="preImage"></param>
        /// <param name="specificAttributesList">List of attributes to check, if empty all modified attributes will be returned</param>
        /// <returns></returns>
        public static AttributeCollection GetModifiedAttributes(this Entity target, Entity preImage, List<string> specificAttributesList)
        {
            if (preImage == null)
                throw new InvalidPluginExecutionException(string.Format("The passed preImage entity is null."));

            if (target.LogicalName != preImage.LogicalName)
                throw new InvalidPluginExecutionException(string.Format("The passed preImage entity {0} must have the same logical name as this target entity {1}.", preImage.LogicalName, target.LogicalName));

            AttributeCollection modifiedAttributes = new AttributeCollection();

            foreach (KeyValuePair<string, object> targetAttribute in target.Attributes)
            {
                if (specificAttributesList != null && specificAttributesList.Count > 0 && !specificAttributesList.Contains(targetAttribute.Key))
                    continue;

                bool attributeIsChanged = false;

                // Target value is null
                if (targetAttribute.Value == null)
                {
                    if (preImage.Contains(targetAttribute.Key) &&
                        preImage[targetAttribute.Key] != null)
                    {
                        attributeIsChanged = true;
                    }
                }
                // Target value is not null
                else
                {
                    if (!preImage.Contains(targetAttribute.Key) ||
                        preImage[targetAttribute.Key] == null)
                    {
                        attributeIsChanged = true;
                    }
                    else
                    {
                        // Both target and preImage value is not null
                        if (targetAttribute.Value is EntityReference)
                        {
                            EntityReference t = (EntityReference)targetAttribute.Value;
                            EntityReference p = (EntityReference)preImage[targetAttribute.Key];
                            if (!t.Equals(p))
                            {
                                attributeIsChanged = true;
                            }
                        }
                        else if (targetAttribute.Value is OptionSetValue)
                        {
                            OptionSetValue t = (OptionSetValue)targetAttribute.Value;
                            OptionSetValue p = (OptionSetValue)preImage[targetAttribute.Key];
                            if (!t.Equals(p))
                            {
                                attributeIsChanged = true;
                            }
                        }
                        else if (targetAttribute.Value is Money)
                        {
                            Money t = (Money)targetAttribute.Value;
                            Money p = (Money)preImage[targetAttribute.Key];
                            if (!t.Equals(p))
                            {
                                attributeIsChanged = true;
                            }
                        }
                        else if (targetAttribute.Value is Boolean)
                        {
                            Boolean t = (Boolean)targetAttribute.Value;
                            Boolean p = (Boolean)preImage[targetAttribute.Key];
                            if (!t.Equals(p))
                            {
                                attributeIsChanged = true;
                            }
                        }
                        else if (targetAttribute.Value is DateTime)
                        {
                            DateTime t = (DateTime)targetAttribute.Value;
                            DateTime p = (DateTime)preImage[targetAttribute.Key];
                            if (t.Kind == p.Kind)
                            {
                                if (!t.Equals(p))
                                {
                                    attributeIsChanged = true;
                                }
                            }
                            else
                            {
                                if (!t.ToUniversalTime().Equals(p.ToUniversalTime()))
                                {
                                    attributeIsChanged = true;
                                }
                            }
                        }
                        else if (targetAttribute.Value is Decimal)
                        {
                            Decimal t = (Decimal)targetAttribute.Value;
                            Decimal p = (Decimal)preImage[targetAttribute.Key];
                            if (!t.Equals(p))
                            {
                                attributeIsChanged = true;
                            }
                        }
                        else if (targetAttribute.Value is Int32)
                        {
                            Int32 t = (Int32)targetAttribute.Value;
                            Int32 p = (Int32)preImage[targetAttribute.Key];
                            if (!t.Equals(p))
                            {
                                attributeIsChanged = true;
                            }
                        }
                        else if (targetAttribute.Value is Guid)
                        {
                            Guid t = (Guid)targetAttribute.Value;
                            Guid p = (Guid)preImage[targetAttribute.Key];
                            if (!t.Equals(p))
                            {
                                attributeIsChanged = true;
                            }
                        }
                        else if (targetAttribute.Value is String)
                        {
                            String t = (String)targetAttribute.Value;
                            String p = (String)preImage[targetAttribute.Key];
                            if (!t.Equals(p))
                            {
                                attributeIsChanged = true;
                            }
                        }
                        else if (targetAttribute.Value is EntityCollection)
                        {
                            // Do we need to compare each entity in collection?
                            EntityCollection tColl = (EntityCollection)targetAttribute.Value;
                            EntityCollection pColl = (EntityCollection)preImage[targetAttribute.Key];

                            if (tColl.Entities.Count != pColl.Entities.Count)
                            {
                                attributeIsChanged = true;
                            }
                            else
                            {
                                foreach (Entity t in tColl.Entities)
                                {
                                    if (!pColl.Entities.Any(p => p.Id == t.Id))
                                    {
                                        attributeIsChanged = true;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            String t = targetAttribute.Value.ToString();
                            String p = preImage[targetAttribute.Key].ToString();
                            if (!t.Equals(p))
                            {
                                attributeIsChanged = true;
                            }
                        }
                    }
                }

                if (attributeIsChanged)
                {
                    modifiedAttributes.Add(targetAttribute);
                }
            }

            return modifiedAttributes;
        }

        /// <summary>
        /// Trace entity properties.
        /// </summary>
        public static void Trace(this Entity entity, ITracingService tracingService)
        {
            try
            {
                if (tracingService == null)
                {
                    return;
                }

                if (entity == null)
                {
                    tracingService.Trace("\nTrace()\nEntity is null");
                    return;
                }

                if (entity.Attributes == null)
                {
                    tracingService.Trace("\nTrace()\nEntity has no attributes");
                    return;
                }

                StringBuilder traceString = new StringBuilder();
                traceString.AppendLine();
                traceString.AppendLine("----------" + entity.LogicalName + "----------");
                foreach (KeyValuePair<string, object> attribute in entity.Attributes)
                {
                    try
                    {
                        traceString.Append("- " + attribute.Key + " = ");
                        if (attribute.Value == null)
                            traceString.Append("null;");
                        else if (attribute.Value is Microsoft.Xrm.Sdk.EntityReference)
                            traceString.Append("(EntityReference) " + ((EntityReference)attribute.Value).LogicalName + " (" + ((EntityReference)attribute.Value).Id + ")");
                        else if (attribute.Value is Microsoft.Xrm.Sdk.OptionSetValue)
                            traceString.Append("(OptionSetValue) " + ((OptionSetValue)attribute.Value).ExtensionData + " (" + ((OptionSetValue)attribute.Value).Value.ToString() + ")");
                        else if (attribute.Value is Microsoft.Xrm.Sdk.Money)
                            traceString.Append("(Money) " + "(" + ((Money)attribute.Value).Value.ToString() + ")");
                        else if (attribute.Value is Microsoft.Xrm.Sdk.OptionSetValue)
                            traceString.Append("(Entity) " + ((OptionSetValue)attribute.Value).ExtensionData + " (" + ((OptionSetValue)attribute.Value).Value.ToString() + ")");
                        else if (attribute.Value is EntityCollection)
                            traceString.Append("(EntityCollection) " + ((EntityCollection)attribute.Value).EntityName + " (" + ((EntityCollection)attribute.Value).Entities.Count() + ")");
                        else
                            traceString.Append("(" + attribute.Value.GetType() + ") " + attribute.Value.ToString());
                        traceString.AppendLine(string.Empty);
                    }
                    catch (Exception ex)
                    {
                        traceString.AppendLine(ex.Message);
                    }
                }
                traceString.AppendLine("--------------------------------------");
                tracingService.Trace(traceString.ToString());
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("Failed to trace entity: " + ex.Message);
            }
        }
        /// <summary>
        /// Throw exception if property is missing.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tracingService">null to avoid tracing entity</param>
        /// <param name="propertyName">Name of Property</param>
        public static void ThrowMissingProperty(this Entity entity, ITracingService tracingService, string propertyName)
        {
            if (!entity.Contains(propertyName))
            {
                if (tracingService != null)
                {
                    entity.Trace(tracingService);
                }
                throw new MissingFieldException(entity.LogicalName, propertyName);
            }
        }
        /// <summary>
        /// Throw exception if any of the properties are missing. The exception message will list all properties that are missing.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tracingService">null to avoid tracing entity</param>
        /// <param name="propertyNames">Name of Property</param>
        public static void ThrowMissingProperties(this Entity entity, ITracingService tracingService, params string[] propertyNames)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string propertyName in propertyNames)
            {
                if (!entity.Contains(propertyName))
                {
                    builder.AppendLine(string.Format("Field '{0}{2}{1}' not found.", entity.LogicalName, propertyName, AliasedSeparator));
                }
            }
            if (builder.Length > 0)
            {
                if (tracingService != null)
                {
                    entity.Trace(tracingService);
                }
                throw new MissingFieldException(builder.ToString());
            }
        }

        /// <summary>
        /// Get the property's logical name (from attribute).
        /// </summary>
        public static string GetPropertyLogicalName<T>(this Entity entity, Expression<Func<T>> propertyExpression)
        {
            MemberInfo member = (propertyExpression.Body as MemberExpression).Member;
            AttributeLogicalNameAttribute att = (AttributeLogicalNameAttribute)Attribute.GetCustomAttribute(member, typeof(AttributeLogicalNameAttribute));
            return att.LogicalName;
        }

        /// <summary>
        /// Implementation of ToEntity which returns null if entity is null.
        /// </summary>
        public static T ToEntityNull<T>(this Entity entity) where T : Entity
        {
            if (entity != null)
                return entity.ToEntity<T>();
            return null;
        }
        /// <summary>
        /// Returns the Aliased Value for a column specified in a Linked entity
        /// </summary>
        /// <typeparam name="T">The type of the aliased attribute form the linked entity</typeparam>
        /// <param name="entity"></param>
        /// <param name="entityName">Entity Logical Name</param>
        /// <param name="attributeName">The aliased attribute from the linked entity.</param>
        /// <returns></returns>
        public static T GetAliasedValue<T>(this Entity entity, string entityName, string attributeName)
        {
            return GetAliasedValue<T>(entity, string.Format("{0}{2}{1}", entityName, attributeName, AliasedSeparator));
        }

        /// <summary>
        /// Returns the Aliased Value for a column specified in a Linked entity
        /// </summary>
        /// <typeparam name="T">The type of the aliased attribute form the linked entity</typeparam>
        /// <param name="entity"></param>
        /// <param name="entityName">Entity Logical Name</param>
        /// <param name="attributeName">The aliased attribute from the linked entity.</param>
        /// <returns></returns>
        public static T GetAliasedValueOrDefault<T>(this Entity entity, string entityName, string attributeName)
        {
            return GetAliasedValueOrDefault<T>(entity, string.Format("{0}{2}{1}", entityName, attributeName, AliasedSeparator));
        }

        /// <summary>
        /// Returns the Aliased Value for a column specified in a Linked entity
        /// </summary>
        /// <typeparam name="T">The type of the aliased attribute form the linked entity</typeparam>
        /// <param name="entity"></param>
        /// <param name="attributeName">The aliased attribute from the linked entity.  Can be preappeneded with the
        /// linked entities logical name and a period. ie "Contact.LastName"</param>
        /// <returns></returns>
        public static T GetAliasedValue<T>(this Entity entity, string attributeName)
        {
            string aliasedEntityName = SplitAliasedAttributeEntityName(ref attributeName);
            foreach (var attribute in entity.Attributes.Values)
            {
                AliasedValue aliased = attribute as AliasedValue;
                if (entity.IsAttributeAliasedValue(attributeName, aliasedEntityName, aliased))
                {
                    try
                    {
                        return (T)aliased.Value;
                    }
                    catch (InvalidCastException)
                    {
                        throw new InvalidCastException(
                            String.Format("Unable to cast attribute {0}{4}{1} from type {2} to type {3}",
                                    aliased.EntityLogicalName, aliased.AttributeLogicalName,
                                    typeof(T).Name, aliased.Value.GetType().Name, AliasedSeparator));
                    }
                }
            }
            throw new Exception("Aliased value with attribute " + attributeName +
                " was not found!  Only these attributes were found: " + String.Join(", ", entity.Attributes.Keys));
        }
        /// <summary>
        /// Handles spliting the attributeName if it is formated as "EntityAliasedName.AttributeName",
        /// updating the attribute name and returning the aliased EntityName
        /// </summary>
        /// <param name="attributeName">Attribute Name</param>
        private static string SplitAliasedAttributeEntityName(ref string attributeName)
        {
            string aliasedEntityName = null;
            if (attributeName.Contains(AliasedSeparator))
            {
                var split = attributeName.Split(AliasedSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (split.Length != 2)
                {
                    throw new Exception("Attribute Name was specified for an Alaised Value with " + split.Length +
                    " split parts, and two were expected.  Attribute Name = " + attributeName);
                }
                aliasedEntityName = split[0];
                attributeName = split[1];
            }
            return aliasedEntityName;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsAttributeAliasedValue(this Entity entity, string attributeName, string aliasedEntityName, AliasedValue aliased)
        {
            bool value =
           (aliased != null &&
                (aliasedEntityName == null || aliasedEntityName == aliased.EntityLogicalName) &&
                aliased.AttributeLogicalName == attributeName);


            // I believe there is a bug in CRM 2011 when dealing with aggregate values of a linked entity in FetchXML.
            // Even though it is marked with an alias, the AliasedValue in the Attribute collection will use the 
            // actual CRM name, rather than the aliased one, even though the AttributeCollection's key will correctly
            // use the aliased name.  So if the aliased Attribute Logical Name doesn't match the assumed attribute name
            // value, check to see if the entity contains an AliasedValue with that key whose attribute logical name 
            // doesn't match the key (the assumed bug), and mark it as being the aliased attribute
            if (!value && aliased != null && entity.Contains(attributeName))
            {
                var aliasedByKey = entity[attributeName] as AliasedValue;
                if (aliasedByKey != null && aliasedByKey.AttributeLogicalName != attributeName &&
                     Object.ReferenceEquals(aliased, aliasedByKey))
                {
                    value = true;
                }
            }
            return value;
        }

        /// <summary>
        /// Returns the Aliased Value for a column specified in a Linked entity, returning the default value for 
        /// the type if it wasn't found
        /// </summary>
        /// <typeparam name="T">The type of the aliased attribute form the linked entity</typeparam>
        /// <param name="entity"></param>
        /// <param name="attributeName">The aliased attribute from the linked entity.  Can be preappeneded with the
        /// linked entities logical name and a period. ie "Contact.LastName"</param>
        /// <returns></returns>
        public static T GetAliasedValueOrDefault<T>(this Entity entity, string attributeName)
        {
            T value;
            if (entity.HasAliasedAttribute(attributeName))
            {
                value = entity.GetAliasedValue<T>(attributeName);
            }
            else
            {
                value = default(T);
            }
            return value;
        }

        /// <summary>
        /// Check if the Aliased Value for a column specified in a Linked entity exists.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeName">The aliased attribute from the linked entity.  Can be preappeneded with the
        /// linked entities logical name and a period. ie "Contact.LastName"</param>
        /// <returns></returns>
        public static bool HasAliasedAttribute(this Entity entity, string attributeName)
        {
            string aliasedEntityName = SplitAliasedAttributeEntityName(ref attributeName);
            return entity.Attributes.Values.Any(a =>
                entity.IsAttributeAliasedValue(attributeName, aliasedEntityName, a as AliasedValue));
        }

        /// <summary>
        /// Returns the Aliased Value for a column specified in a Linked entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityName">Entity Logical Name</param>
        /// <param name="attributeName">The aliased attribute from the linked entity.</param>
        /// <returns></returns>
        public static bool HasAliasedAttribute(this Entity entity, string entityName, string attributeName)
        {
            return entity.Attributes.Values.Any(a =>
                entity.IsAttributeAliasedValue(attributeName, entityName, a as AliasedValue));
        }

        /// <summary>
        /// Copy aliased attributes from this entity to target.
        /// </summary>
        public static void FillAliasedAttributes(this Entity entity, Entity target)
        {
            entity.FillAliasedAttributes(target, target.LogicalName);
        }

        /// <summary>
        /// Copy aliased attributes from this entity to target.
        /// </summary>
        public static void FillAliasedAttributes(this Entity entity, Entity target, string alias)
        {
            string aliasDot = alias + AliasedSeparator;

            var aliasedAccountAttributesQuery =
                    from KeyValuePair<string, object> p in entity.Attributes
                    where p.Key.StartsWith(aliasDot)
                    && p.Value is AliasedValue
                    select p;

            foreach (KeyValuePair<string, object> attribute in aliasedAccountAttributesQuery)
            {
                target[attribute.Key.Replace(aliasDot, string.Empty)] = ((AliasedValue)attribute.Value).Value;
            }
        }

        /// <summary>
        /// Returns an Entity filled with all Aliased attributes for that Entitiy from the Source.
        /// </summary>
        /// <typeparam name="T">The Entity type to return</typeparam>
        /// <param name="source">The Entity to get Aliased attributes from</param>
        /// <returns>A new instance of the Entity T filled with all Aliased Values from the Source Entity</returns>
        public static T GetAliasedEntity<T>(this Entity source) where T : Entity
        {
            T target = Activator.CreateInstance<T>();

            if (source != null)
                source.FillAliasedAttributes(target);

            return target;
        }

    }
}
