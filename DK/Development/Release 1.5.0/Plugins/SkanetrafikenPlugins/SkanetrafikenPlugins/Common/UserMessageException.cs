using System;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    [Serializable]
    public class UserMessageException : Exception
    {
        #region Constructors
        public UserMessageException(string userMessage, Exception ex)
            : base(userMessage, ex)
        {
        }

        public UserMessageException(string userMessage)
            : base(userMessage)
        {
        }
        #endregion
    }
}
