using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSTDK003.Utility;

namespace INTSTDK003.Helper
{
    public class INTSTDK003Helper
    {
        public INTSTDK003Helper() { }

        public string ReadSSOValue(string AppName, string Value)
        {
            try { return INTSTDK003.Utility.SSOClientHelper.Read(AppName, Value); }
            catch { return string.Empty; }
        }
    }
}
