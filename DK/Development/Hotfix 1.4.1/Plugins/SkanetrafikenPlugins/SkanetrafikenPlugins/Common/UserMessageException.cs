using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    public class UserMessageException : Exception
    {
        #region Constructors
        public UserMessageException(string UserMessage, Exception ex)
            : base(UserMessage, ex)
        {
        }
        public UserMessageException(string UserMessage)
            : base(UserMessage)
        {
        }
        #endregion
    }
}
