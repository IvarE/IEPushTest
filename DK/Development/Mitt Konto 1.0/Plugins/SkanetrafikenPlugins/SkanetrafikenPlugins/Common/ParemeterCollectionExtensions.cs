using Microsoft.Xrm.Sdk;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    public static class ParemeterCollectionExtensions
    {
        #region Public Methods
        public static bool TryGetTargetEntity(this ParameterCollection collection, out Entity value)
        {
            value = null;
            if (collection.Contains("Target") && collection["Target"] is Entity)
            {
                value = (Entity)collection["Target"];
                return true;
            }
            return false;
        }
        #endregion
        /* For future reference: PlugindataBase is less accessible so it doesnt work if access level is not changed
        public static bool SetTargetEntity(this ParameterCollection collection, PlugindataBase data)
        {}
        */
    }
}
