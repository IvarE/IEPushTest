using System;

namespace CGIXrmRainDanceExport.Classes
{
    public static class StringExtensions
    {
        #region Public Methods
        public static string SetToFixedLengthPadRight(this string value, int maxLength)
        {
            if (String.IsNullOrEmpty(value))
            {
                return new String(' ', maxLength);
            }

            return value.Length <= maxLength ? value.PadRight(maxLength) : value.Substring(0, maxLength);
        }
        public static string SetMaxLength(this string value, int maxLength)
        {
            if (String.IsNullOrEmpty(value))
            {
                return "";
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
        #endregion
    }
}
