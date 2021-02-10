using System;
using Microsoft.Xrm.Sdk;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    public static class EntityExtensions
    {
        #region Public Methods
        ///<summary> 
        ///Extension method to get an attribute value from the entity or image 
        ///</summary> 
        ///<typeparam name="T">The attribute type</typeparam> 
        ///<param name="entity">The primary entity</param> 
        ///<param name="attributeLogicalName">Logical name of the attribute</param> 
        ///<param name="image">Image (pre/post) of the primary entity</param> 
        ///<param name="defaultValue">The default value to use</param> 
        ///<returns>The attribute value of type T</returns> 
        public static T GetAttributeValue<T>(this Entity entity, string attributeLogicalName, Entity image, T defaultValue)
        {
            return entity.Contains(attributeLogicalName)
                ? entity.GetAttributeValue<T>(attributeLogicalName)
                : image != null && image.Contains(attributeLogicalName)
                    ? image.GetAttributeValue<T>(attributeLogicalName)
                    : defaultValue;
        }

        public static T GetValue<T>(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Attributes.ContainsKey(attributeLogicalName))
                return default(T);

            var aliasedValue = entity[attributeLogicalName] as AliasedValue;
            if (aliasedValue != null)
            {
                if (aliasedValue.Value == null)
                {
                    return default(T);
                }

                T value;
                try
                {
                    value = (T) aliasedValue.Value;
                }
                catch (Exception ex)
                {
                    throw new InvalidPluginExecutionException(ex.Message);
                }

                return value;
            }
            return entity.GetAttributeValue<T>(attributeLogicalName);
        }
        #endregion
    }
}
