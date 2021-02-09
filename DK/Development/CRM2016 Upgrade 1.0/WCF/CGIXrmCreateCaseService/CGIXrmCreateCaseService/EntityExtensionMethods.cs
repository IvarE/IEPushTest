using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Endeavor.Extensions
{
    /// <summary>
    /// Extensions to the Entity Class.
    /// </summary>
    internal static class EntityExtension
    {
        /// <summary>
        /// Implementation of ToEntity which returns null if entity is null.
        /// </summary>
        public static T ToEntityNull<T>(this Entity entity) where T : Entity
        {
            if (entity != null)
                return entity.ToEntity<T>();
            return null;
        }


        ///
        /// <summary>
        /// Return message in content as string.
        /// </summary>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        public static string ContentToString(this HttpContent httpContent)
        {
            var readAsStringAsync = httpContent.ReadAsStringAsync();
            return readAsStringAsync.Result;
        }


    }
}