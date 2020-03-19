using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace Skanetrafiken.Crm
{
    public static class ConnectionCacheManager
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string cacheNamePrefix = "conn";

        //public static string globalString;
        private static List<Tuple<int, string>> bookingList = new List<Tuple<int, string>>();
        private static int lockedByThreadId = -1;

        public static CrmServiceClient GetAvailableConnection(int threadId, bool print)
        {
            string cacheName = "placeholderString";
            lock (bookingList)
            {
                cacheName = getFreeCacheName();
                if (print)
                    _log.Debug("Using Connection " + cacheName);
                bookingList.Add(new Tuple<int, string>(threadId, cacheName));
            }

            HttpContext httpContext = HttpContext.Current;
            CrmServiceClient _conn = null;
            if (httpContext != null)
            {
                _conn = httpContext.Cache.Get(cacheName) as CrmServiceClient;
            }
            if (_conn == null)
            {
                if (print)
                    _log.Debug("Creating new connection from CRM (no cache found)");
                string connectionString = CrmConnection.GetCrmConnectionString(CredentialFilePath);
                if (print)
                    _log.DebugFormat("connectionString = {0}", connectionString);
                //  Connect to the CRM web service using a connection string.
                _conn = new CrmServiceClient(connectionString);
                
                if (print)
                    _log.DebugFormat("Connection state: _conn.IsReady = {0}", _conn.IsReady);
                if (_conn.IsReady)
                {
                    httpContext.Cache.Insert(cacheName, _conn, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
                }
                else
                {

                }
            }
            return _conn;
        }

        private static string getFreeCacheName()
        {
            int i = 0;
            while (true)
            {
                if (i > 4)
                    _log.Warn("CAUTION!! More than 5 connections are booked in the Manager. Possible bug.");

                bool isBooked = false;
                foreach (Tuple<int, string> booking in bookingList)
                {
                    if ((cacheNamePrefix + i).Equals(booking.Item2))
                        isBooked = true;
                }
                if (!isBooked)
                {
                    return (cacheNamePrefix + i);
                }
                else
                {
                    i++;
                }
            }
        }

        public static void ReleaseConnection(int threadId)
        {
            if (bookingList.Count > 0)
            {
                bookingList.RemoveAll(p => threadId == p.Item1);
            }
        }

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePath);
            }
        }
    }
}