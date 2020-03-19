using System;
using System.Text;

namespace Crm.FormAssistantPanel.Utilities
{
    public class ExceptionHandling
    {
        public static string ConvertToString(Exception exception)
        {
            string prefix = "";

            StringBuilder sb = new StringBuilder();
            while (null != exception)
            {
                sb.Append(prefix);
                sb.AppendLine(exception.Message);
                sb.AppendLine(exception.StackTrace);

                prefix = "Innner Exception: ";
                exception = exception.InnerException;
            }

            return sb.ToString();
        }

    }
}
