using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmRainDanceExport.Classes
{
    public static class StringExtensions
    {
        public static string SetToFixedLength(this string value, int maxLength)
        {
            if (String.IsNullOrEmpty(value))
            {
                return new String(' ', maxLength);
            }

            return value.Length <= maxLength ? value.PadRight(maxLength) : value.Substring(0, maxLength);
        }
    }
}
