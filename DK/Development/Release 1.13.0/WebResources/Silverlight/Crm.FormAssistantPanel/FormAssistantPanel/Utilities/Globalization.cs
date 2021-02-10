using System;
using System.Threading;

namespace Crm.FormAssistantPanel.Utilities
{
    public class Globalization
    {

        /// <summary>
        /// Author:Ponraj
        /// Description:This function used to set CurrentThread's UICulture based on LCID passed.
        /// </summary>
        /// <param name="LCID">Holds LCID of Logged in User</param>
        public static void SetUICulture(double LCID)
        {
            try
            {
                string LCIDName = "en-us";
                switch (Convert.ToInt32(LCID))
                {
                    case 1033:
                        LCIDName = "en-us";
                        break;
                    case 1053:
                        LCIDName = "sv-se";
                        break;
                    case 2077:
                        LCIDName = "sv-fi";
                        break;
                    case 1043:
                        LCIDName = "nl-nl";
                        break;
                    case 2067:
                        LCIDName = "nl-be";
                        break;
                    case 1044:
                        LCIDName = "nb-no";
                        break;
                    case 2068:
                        LCIDName = "nn-no";
                        break;
                    case 1031:
                        LCIDName = "de-de";
                        break;
                    case 1030:
                        LCIDName = "da";
                        break;
                    case 1049:
                        LCIDName = "ru";
                        break;
                    case 1035:
                        LCIDName = "fi";
                        break;

                }
                System.Globalization.CultureInfo oCultureInfo = new System.Globalization.CultureInfo(LCIDName);
                System.Threading.Thread.CurrentThread.CurrentCulture = oCultureInfo;
                System.Threading.Thread.CurrentThread.CurrentUICulture = oCultureInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Author:Renis
        /// Description:This function used to set give thread's Culture based on LCID passed.
        /// </summary>
        /// <param name="defaultLCID">Holds LCID of Logged in User</param>
        public static void SetUICulture(Thread thread, double LCID)
        {
            try
            {
                string LCIDName = "en-us";
                switch (Convert.ToInt32(LCID))
                {
                    case 1033:
                        LCIDName = "en-us";
                        break;
                    case 1053:
                        LCIDName = "sv-se";
                        break;
                    case 2077:
                        LCIDName = "sv-fi";
                        break;
                    case 1043:
                        LCIDName = "nl-nl";
                        break;
                    case 2067:
                        LCIDName = "nl-be";
                        break;
                    case 1044:
                        LCIDName = "nb-no";
                        break;
                    case 2068:
                        LCIDName = "nn-no";
                        break;
                    case 1031:
                        LCIDName = "de-de";
                        break;
                    case 1030:
                        LCIDName = "da";
                        break;
                    case 1049:
                        LCIDName = "ru";
                        break;
                    case 1035:
                        LCIDName = "fi";
                        break;

                }
                System.Globalization.CultureInfo oCultureInfo = new System.Globalization.CultureInfo(LCIDName);
                thread.CurrentCulture = oCultureInfo;
                thread.CurrentUICulture = oCultureInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
