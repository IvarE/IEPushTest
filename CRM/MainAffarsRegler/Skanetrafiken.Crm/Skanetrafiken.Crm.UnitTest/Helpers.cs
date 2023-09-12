
// This file is maintained through Endeavor NuGet. Please do not modify it directly in your project.

using System;
using System.Linq;
using System.Security;

using Microsoft.Xrm.Sdk;

namespace Endeavor.Crm.IntegrationTests
{
    class Configuration
    {
        static byte[] entropy = System.Text.Encoding.Unicode.GetBytes("Salt Is Not A Password");

        public static string EncryptString(System.Security.SecureString input)
        {
            byte[] encryptedData = System.Security.Cryptography.ProtectedData.Protect(
                System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)),
                entropy,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    entropy,
                    System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
    }

    /// <summary>
    /// TracingService outputing trace to Trace, Debug or Console.
    /// Trace will show in Debug Window during Test.
    /// Console will show in Output in NUnit Test Explorer.
    /// </summary>
    class TracingService : ITracingService
    {
        public bool UseDebug { get; set; }
        public bool UseTrace { get; set; }
        public bool UseConsole { get; set; }
        
        /// <summary>
        /// Default constructor using Trace and Console output.
        /// </summary>
        public TracingService() : this(true, true, false)
        {
        }

        public TracingService(bool useTrace, bool useConsole, bool useDebug)
        {
            UseTrace = useTrace;
            UseConsole = useConsole;
            UseDebug = useDebug;
        }


        public void Trace(string format, params object[] args)
        {
            if (args.Count() > 0)
            {
                if(UseTrace)
                    System.Diagnostics.Trace.TraceInformation(format, args);
                if (UseDebug)
                    System.Diagnostics.Debug.Print(format, args);
                if (UseConsole)
                    System.Console.WriteLine(format, args);
            }
            else
            {
                if (UseTrace)
                    System.Diagnostics.Trace.TraceInformation(format);
                if (UseDebug)
                    System.Diagnostics.Debug.Print(format);
                if (UseConsole)
                    System.Console.WriteLine(format);
            }
        }
    }

    class ServiceProvider : IServiceProvider
    {
        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(ITracingService))
                return new TracingService();

            throw new NotImplementedException();
        }
    }
}