using Microsoft.Xrm.Sdk;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    public static class AttributeExtensions
    {
        #region Public Methods

        /// <summary>
        /// If attribute was updated, its availible at least as a key on update message.
        /// If value is null, then update was a delete of value,
        /// then try to get value from preimage for use in your logic.
        /// For use on update in a post event.
        /// Preimage must be registred.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="preImage"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetUpdatedOrPreImageAttributeValue(
            this AttributeCollection collection,
            Entity preImage,
            string key,
            out object value)
        {

            if (collection.TryGetValue(key, out value))
            {
                if (value == null)
                {
                    return (preImage != null && preImage.Attributes != null) && preImage.Attributes.TryGetValue(key, out value);
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}
