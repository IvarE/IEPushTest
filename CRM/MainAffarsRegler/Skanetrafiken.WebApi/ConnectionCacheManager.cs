﻿using Microsoft.Xrm.Tooling.Connector;
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
                bookingList.Add(new Tuple<int, string>(threadId, cacheName));
            }

            HttpContext httpContext = HttpContext.Current;

            //Special implementation for V.9 Connection Issues
            //string testConnectionString = @"Url=https://sekundtst.skanetrafiken.se/DKCRMTST" + ";" + "Domain=D1" + ";" + @"Username=D1\CRMAdmin" + ";" + "Password=uSEme2!nstal1" + ";" + "authtype=AD";
            //string testConnectionString = @"Url=https://sekundacc.skanetrafiken.se/DKCRMACC" + ";" + "Domain=D1" + ";" + @"Username=D1\CRMAdmin" + ";" + "Password=uSEme2!nstal1" + ";" + "authtype=AD";
            //string testConnectionString = @"Url=https://sekund.skanetrafiken.se/DKCRM" + ";" + "Domain=D1" + ";" + @"Username=D1\CRMAdmin" + ";" + "Password=uSEme2!nstal1" + ";" + "authtype=AD";
            //CrmServiceClient _conn = new CrmServiceClient(testConnectionString);

            //When Commented -> Using Special implementation for V.9 Connection Issues
            CrmServiceClient _conn = null;
            if (httpContext != null)
            {
                _conn = httpContext.Cache.Get(cacheName) as CrmServiceClient;
            }
            if (_conn == null)
            {
                string connectionString = CrmConnection.GetCrmConnectionString(CredentialFilePath);
                //  Connect to the CRM web service using a connection string.
                _conn = new CrmServiceClient(connectionString);

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
            using (var _logger = new AppInsightsLogger())
            {


                int i = 0;
                while (true)
                {

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