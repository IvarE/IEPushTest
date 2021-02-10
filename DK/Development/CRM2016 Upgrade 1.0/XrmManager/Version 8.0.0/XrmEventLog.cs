using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Data.SqlClient;
using System.Web.Services.Protocols;
using Microsoft.Win32;

namespace CGIXrmWin
{

    // ********************************************************************

    public class XrmEventLog
    {
        // ====================================================================

        #region Event log

        // --------------------------------------------------------------------

        private static string EVENTLOGNAME = "LogicaXrm";
        private static string EVENTSOURCENAME = "LogicaXrmService";

        // --------------------------------------------------------------------

        private static string _getErrorMessage(Exception error)
        {
            StringBuilder _sb = new StringBuilder();
            while (error != null)
            {
                _sb.AppendFormat("### {0} ###", error.Source);
                _sb.AppendLine();
                _sb.AppendFormat("[ {0} ]", error.Message);
                if (error is SoapException)
                {
                    SoapException _soap = error as SoapException;
                    if (_soap.Detail != null)
                    {
                        _sb.AppendLine(" =soap innertext(begin)= ");
                        _sb.AppendLine(_soap.Detail.InnerText);
                        _sb.AppendLine(" =soap innertext(end)= ");
                    }
                }
                _sb.AppendLine();
                _sb.AppendLine(" =StackTrace(begin)= ");
                _sb.AppendLine(error.StackTrace);
                _sb.AppendLine(" =StackTrace(end)= ");
                _sb.AppendLine("######");

                error = error.InnerException;
            }
            return _sb.ToString();
        }

        // --------------------------------------------------------------------

        private static bool _createEventLogIfNotExists()
        {
            try
            {
                if (!EventLog.SourceExists(EVENTSOURCENAME))
                {
                    EventLog.CreateEventSource(EVENTSOURCENAME, EVENTLOGNAME);
                    // wait a moment before use
                    System.Threading.Thread.Sleep(500);
                }
            }
            catch (Exception exc)
            {
                EventLog.WriteEntry(EVENTSOURCENAME, _getErrorMessage(exc), EventLogEntryType.Error, 9999);
            }

            return EventLog.SourceExists(EVENTSOURCENAME);
        }

        // --------------------------------------------------------------------

        public static void Write(Exception error, EventLogEntryType entryType, int eventId)
        {
            Write(_getErrorMessage(error), entryType, eventId);
        }

        // --------------------------------------------------------------------

        public static void Write(string eventMessage, EventLogEntryType entryType, int eventId)
        {
            if (!EventLog.SourceExists(EVENTSOURCENAME))
                _createEventLogIfNotExists();

            if (!EventLog.Exists(EVENTLOGNAME))
                System.Threading.Thread.Sleep(250);

            EventLog _evLog = new EventLog(EVENTLOGNAME);
            _evLog.Source = EVENTSOURCENAME;
            _evLog.WriteEntry(eventMessage, entryType, eventId);
            _evLog.Close();
        }

        // --------------------------------------------------------------------

        #endregion Event log

        // ====================================================================

    }

    // ********************************************************************

}
