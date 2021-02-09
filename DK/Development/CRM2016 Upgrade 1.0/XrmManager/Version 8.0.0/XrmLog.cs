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

    public class XrmLog
    {
        // 1. log folder via constructor
        // 2. registry 64bit  HKEY_LOCAL_MACHINE\SOFTWARE\Logica\XrmLog  Directory == string
        // 3. c:\Logica\

        private string pFileName = @"c:\Logica\LogicaPluginLog.txt";
        private string pDirName = string.Empty;

        #region constructor and desctructor 

        // -------------------------------------------------------------------------

        public XrmLog()
        {
            _createFile();
            XrmEventLog.Write(string.Format("XrmLog started, log path [{0}] ", pFileName), EventLogEntryType.Information, 100);
        }

        // -------------------------------------------------------------------------

        public XrmLog(string dirName)
        {
            pDirName = dirName;
            _createFile();
            XrmEventLog.Write(string.Format("XrmLog started, log path [{0}] ", pFileName), EventLogEntryType.Information, 100);
        }

        // -------------------------------------------------------------------------

        ~XrmLog()
        {
            XrmEventLog.Write(string.Format("XrmLog stopped, log path [{0}] ", pFileName), EventLogEntryType.Information, 300);
        }

        // -------------------------------------------------------------------------

        #endregion constructor

        #region private fields
        
        private int pEventId = 0;
        private const int LEVELFACTOR = 3;
        private bool pFileOk = false;
        private Dictionary<string, int> pSections = new Dictionary<string, int>();

        #endregion private fields

        #region file methods

        // -------------------------------------------------------------------------

        private void _setFileName()
        {
            if (string.IsNullOrEmpty(pDirName))
            {
                try
                {
                    RegistryKey _localMachine =   RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);                                        
                    RegistryKey _regKey = _localMachine.OpenSubKey(@"SOFTWARE\Logica\XrmLog");
                    pDirName = _regKey.GetValue("Directory").ToString();
                    _regKey.Close();
                    _localMachine.Close();
                }
                catch { }
            }

            if (!string.IsNullOrEmpty(pDirName))
                pFileName = Path.Combine(pDirName, string.Format("LogicaPluginLog[{0}].txt", DateTime.Now.ToString("yyyyMMdd")));
            else
                pFileName = string.Format(@"c:\Logica\XrmLog\LogicaPluginLog[{0}].txt", DateTime.Now.ToString("yyyyMMdd"));            
        }

        // -------------------------------------------------------------------------

        private void _createFile()
        {
            pEventId = 1001;
            _setFileName();

            pEventId = 1002;
            string _path = Path.GetDirectoryName(pFileName);
            
            pEventId = 1003;
            string _file = Path.GetFileName(pFileName);
            DirectoryInfo _di = null;
            try
            {
                pEventId = 1005;
                if (!Directory.Exists(_path))
                {
                    pEventId = 1006;
                    _di = Directory.CreateDirectory(_path);                    
                }
                pEventId = 1008;
                FileInfo _fi = new FileInfo(pFileName);

                if (!_fi.Exists)
                {
                    pEventId = 1010;
                    using (FileStream _fs = new FileStream(pFileName, FileMode.CreateNew, FileAccess.Write))
                    {
                        _fs.Close();
                    }

                    pEventId = 1012;
                    using (FileStream _fs = new FileStream(pFileName, FileMode.Open, FileAccess.Write))
                    {                        
                        _fs.Close();                        
                    }
                    
                    pEventId = 1014;
                    _writeToFile(string.Concat(_getRowPrefix(XrmLogMessageType.Information), "Log Start!"));
                }
                else
                {
                    // try to open file 
                    pEventId = 1020;
                    using (FileStream _fs = new FileStream(pFileName, FileMode.Open, FileAccess.Write))
                    {
                        _fs.Close();
                    }
                    
                    pEventId = 1024;
                    _writeToFile(string.Concat(_getRowPrefix(XrmLogMessageType.Information), "Log Start!"));
                }                
                pFileOk = true;
            }
            catch (IOException exc)
            {
                XrmEventLog.Write(exc, EventLogEntryType.Error, pEventId);
            }
            catch (Exception exc)
            {
                XrmEventLog.Write(exc, EventLogEntryType.Error, pEventId);
            }
        }

        // -------------------------------------------------------------------------

        private void _writeToFile(string message)
        {
            pEventId = 2000;
            try
            {
                pEventId = 2001;
                using (FileStream _fs = new FileStream(pFileName, FileMode.Append, FileAccess.Write))
                {
                    pEventId = 2002;
                    StreamWriter _sw = new StreamWriter(_fs);
                    
                    pEventId = 2003;
                    _sw.Write(message);
                    _sw.WriteLine(string.Empty);
                    _sw.Close();
                }
            }
            catch (IOException exc)
            {
                XrmEventLog.Write(exc, EventLogEntryType.Error, pEventId);
            }
            catch (Exception exc)
            {
                XrmEventLog.Write(exc, EventLogEntryType.Error, pEventId);
            }
        }

        // -------------------------------------------------------------------------

        #endregion file methods

        #region get error message method

        // -------------------------------------------------------------------------

        private StringBuilder _getErrorMessage(Exception exc)
        {            
            StringBuilder _sb = new StringBuilder();
            _sb.AppendLine("*** ERROR ***");
            while (exc != null)
            {                    
                _sb.AppendLine(exc.Source);
                _sb.AppendLine(exc.Message);

                if (exc is SoapException)
                {
                    SoapException _soap = exc as SoapException;
                    if (_soap.Detail != null)
                        _sb.AppendLine(_soap.Detail.InnerText);
                }
                else if (exc is SqlException)
                {
                    SqlException _sql = exc as SqlException;
                    _sb.AppendLine("SQL Exception");
                    _sb.AppendFormat("Server: {0}", _sql.Server);
                    _sb.AppendLine();
                    _sb.AppendFormat("Procedure: {0}", _sql.Procedure);
                    _sb.AppendLine();
                    _sb.AppendFormat("ErrorCode: {0}", _sql.ErrorCode.ToString());
                    _sb.AppendLine();
                    _sb.AppendFormat("LineNumber: {0}", _sql.LineNumber.ToString());
                    _sb.AppendLine();
                    _sb.AppendFormat("Number: {0}", _sql.Number.ToString());
                    _sb.AppendLine();
                    _sb.AppendLine("SQL Server Errors (begin):");
                    foreach (SqlError err in _sql.Errors)
                    {
                        _sb.AppendFormat("Source: {0}", err.Source);
                        _sb.AppendLine();
                        _sb.AppendFormat("Server: {0}", err.Server);
                        _sb.AppendLine();
                        _sb.AppendFormat("Procedure: {0}", err.Procedure);
                        _sb.AppendLine();
                        _sb.AppendFormat("Number: {0}", err.Number);
                        _sb.AppendLine();
                        _sb.AppendFormat("LineNumber: {0}", err.LineNumber);
                        _sb.AppendLine();
                        _sb.AppendFormat("Message: {0}", err.Message);
                        _sb.AppendLine();
                        _sb.AppendFormat("Class: {0}", err.Class);
                        _sb.AppendLine();
                        _sb.AppendFormat("State: {0}", err.State);
                        _sb.AppendLine();
                    }
                    _sb.AppendLine("SQL Server Errors (end).");
                    _sb.AppendLine();
                }
                _sb.AppendLine("StackTrace begin: ");
                _sb.AppendLine(exc.StackTrace);
                _sb.AppendLine("StackTrace end.");
                _sb.AppendLine();
                exc = exc.InnerException;
            }
            return _sb;
        }

        // -------------------------------------------------------------------------

        #endregion get error message method

        #region file section 

        // -------------------------------------------------------------------------

        public void StartSection(string sectionName)
        {
            pEventId = 3000;
            try
            {
                if (pSections.ContainsKey(sectionName))
                {
                    pEventId = 3001;
                    StartSection(sectionName, pSections[sectionName]);
                }
                else
                {
                    pEventId = 3002;
                    int _maxNumber = 0;
                    if (pSections.Count > 0)
                        _maxNumber = pSections.Max(x => x.Value);
                    pEventId = 3003;
                    StartSection(sectionName, _maxNumber + 1);
                }
            }
            catch (Exception exc)
            {
                XrmEventLog.Write(exc, EventLogEntryType.Error, pEventId);
            }
        }

        // -------------------------------------------------------------------------

        public void StartSection(string sectionName, int level)
        {
            pEventId = 3020;
            try
            {
                if (pSections.ContainsKey(sectionName))
                {
                    pSections[sectionName] = level;
                }
                else
                {
                    pSections.Add(sectionName, level);
                }
                
                pEventId = 3022;
                string _message = string.Concat(new string(' ', pSections[sectionName] * LEVELFACTOR), sectionName);
                pEventId = 3024;
                _writeToFile(string.Concat(_getRowPrefix(XrmLogMessageType.SectionStart), _message));
            }
            catch (Exception exc)
            {
                XrmEventLog.Write(exc, EventLogEntryType.Error, pEventId);
            }
        }

        // -------------------------------------------------------------------------

        public void EndSection(string sectionName)
        {
            pEventId = 3040;
            try
            {
                int _level = 0;

                if (pSections.ContainsKey(sectionName))
                {
                    pEventId = 3042;
                    _level = pSections[sectionName];
                    pEventId = 3044;
                    pSections.Remove(sectionName);
                }
                pEventId = 3046;
                string _message = string.Concat(new string(' ', _level * LEVELFACTOR), sectionName);
                pEventId = 3048;
                _writeToFile(string.Concat(_getRowPrefix(XrmLogMessageType.SectionEnd), _message));
            }
            catch (Exception exc)
            {
                XrmEventLog.Write(exc, EventLogEntryType.Error, pEventId);
            }
        }

        // -------------------------------------------------------------------------

        #endregion file section

        #region write exception 

        // -------------------------------------------------------------------------

        public void Write(Exception error)
        {
            Write(error, string.Empty, XrmLogMessageType.Error);
        }

        // -------------------------------------------------------------------------

        public void Write(Exception error, string sectionName)
        {
            Write(error, sectionName, XrmLogMessageType.Error);
        }

        // -------------------------------------------------------------------------

        public void Write(Exception error, string sectionName, XrmLogMessageType messageType)
        {
            pEventId = 5000;
            try
            {
                pEventId = 5001;
                StringBuilder _errMsg = _getErrorMessage(error);
                StringBuilder _sb = new StringBuilder();

                DateTime _dateTime = DateTime.Now;

                pEventId = 5002;
                string[] _split = _errMsg.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string s in _split)
                {
                    pEventId = 5004;
                    if (pSections.ContainsKey(sectionName))
                    {
                        pEventId = 5005;
                        _sb.AppendLine(string.Concat(_getRowPrefix(messageType, _dateTime), new string(' ', pSections[sectionName] * LEVELFACTOR), s));
                    }
                    else
                    {
                        pEventId = 5006;
                        _sb.AppendLine(string.Concat(_getRowPrefix(messageType, _dateTime), s));
                    }
                }
                pEventId = 5010;
                _writeToFile(_sb.ToString());
            }
            catch (Exception exc)
            {
                XrmEventLog.Write(exc, EventLogEntryType.Error, pEventId);
            }
        }

        // -------------------------------------------------------------------------

        #endregion write exception

        #region write string 

        // -------------------------------------------------------------------------

        /// <summary>
        /// Writes a information message to the log
        /// </summary>
        /// <param name="message">text</param>
        public void Write(string message)
        {
            Write(message, string.Empty, XrmLogMessageType.Information);
        }

        // -------------------------------------------------------------------------

        public void Write(string message, string sectionName)
        {
            Write(message, sectionName, XrmLogMessageType.Information);
        }

        // -------------------------------------------------------------------------

        public void Write(string message, XrmLogMessageType messageType)
        {
            Write(message, string.Empty, messageType);
        }

        // -------------------------------------------------------------------------

        public void Write(string message, string sectionName, XrmLogMessageType messageType)
        {
            pEventId = 5050;
            try
            {
                if (!pFileOk)
                    return;

                pEventId = 5051;
                if (pSections.ContainsKey(sectionName))
                {
                    pEventId = 5052;
                    message = string.Concat(new string(' ', pSections[sectionName] * 3), message);
                }
                pEventId = 5053;
                message = string.Concat(_getRowPrefix(messageType), message);

                pEventId = 5054;
                _writeToFile(message);
            }
            catch (Exception exc)
            {
                XrmEventLog.Write(exc, EventLogEntryType.Error, pEventId);
            }
        }

        // -------------------------------------------------------------------------

        #endregion write string

        #region get row prefix - date and message type

        // -------------------------------------------------------------------------

        private string _getRowPrefix(XrmLogMessageType messageType)
        {
            return _getRowPrefix(messageType, DateTime.Now);
        }

        // -------------------------------------------------------------------------

        private string _getRowPrefix(XrmLogMessageType messageType, DateTime dateTime)
        {
            pEventId = 8000;

            string _mstype = string.Empty;
            switch (messageType)
            {
                case XrmLogMessageType.SectionStart:
                    _mstype = "Section Start";
                    break;
                case XrmLogMessageType.SectionEnd:
                    _mstype = "Section End";
                    break;
                case XrmLogMessageType.SuccessAudit:
                    _mstype = "Success Audit";
                    break;
                case XrmLogMessageType.FailureAudit:
                    _mstype = "Failure Audit";
                    break;
                case XrmLogMessageType.Error:
                    _mstype = "Error";
                    break;
                case XrmLogMessageType.Warning:
                    _mstype = "Warning";
                    break;
                case XrmLogMessageType.Information:
                    _mstype = "Information";
                    break;
                default:
                    _mstype = string.Empty;
                    break;
            }
            
            pEventId = 8001;
            return string.Format("[{0}][{1}] ", dateTime.ToString("yyyyMMdd-HHmmss:ffffff"), _mstype.PadRight(13, '.'));                    
        }

        // -------------------------------------------------------------------------

        #endregion get row prefix - date and message type

    }

    // ********************************************************************

    public enum XrmLogMessageType
    { 
        None,
        SectionStart,
        SectionEnd,
        Information,
        SuccessAudit,
        FailureAudit,
        Error,
        Warning
    }

    // ********************************************************************

}
