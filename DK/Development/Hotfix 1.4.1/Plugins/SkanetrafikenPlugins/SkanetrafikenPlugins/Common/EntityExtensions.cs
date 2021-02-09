using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

using System.ServiceModel;
using CRM2013.SkanetrafikenPlugins.CreateGiftcardService;
using Microsoft.Crm.Sdk.Messages;

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

            if (entity[attributeLogicalName] is AliasedValue)
            {
                if (((AliasedValue)entity[attributeLogicalName]).Value == null)
                {
                    return default(T);
                }

                T _value = default(T);
                try
                {
                    _value = (T)((AliasedValue)entity[attributeLogicalName]).Value;
                }
                catch { }

                return _value;
            }
            else
            {
                return entity.GetAttributeValue<T>(attributeLogicalName);
            }
        }
        #endregion
    }
}
