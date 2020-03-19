using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Xml.Linq;
using System.IO;
using System.Security;

namespace Endeavor.Crm
{
    /// <summary>
    /// Class handling connection strings and credentials to connect to Dynamics CRM.
    /// </summary>
    public class CrmConnection
    {
        private static string ExternalPasswordPlaceHolder = "[Encrypted]";

        /// <summary>
        /// Verifies if a connection string is valid for Microsoft Dynamics CRM.
        /// </summary>
        private static bool ValidConnectionString(string connectionString)
        {
            // At a minimum, a connection string must contain one of these arguments.
            if (connectionString.Contains("Url=") ||
                connectionString.Contains("Server=") ||
                connectionString.Contains("ServiceUri="))
                return true;
            return false;
        }

        /// <summary>
        /// Gets web service connection information from the app.config file.
        /// If there is more than one available, an exception is thrown.
        /// </summary>
        /// <returns>A string containing web service connection configuration information.</returns>
        public static string GetCrmConnectionString(string credentialFilePath, byte[] entropy)
        {
            // Create a filter list of connection strings so that we have a list of valid
            // connection strings for Microsoft Dynamics CRM only.
            List<KeyValuePair<String, String>> filteredConnectionStrings = new List<KeyValuePair<String, String>>();

            foreach (ConnectionStringSettings connectionStringSettings in ConfigurationManager.ConnectionStrings)
            {
                // Remove any connection string not applicable to CRM.
                if (ValidConnectionString(connectionStringSettings.ConnectionString))
                {
                    filteredConnectionStrings.Add(
                        new KeyValuePair<string, string>(
                            connectionStringSettings.Name,
                            connectionStringSettings.ConnectionString));
                }
            }

            if (filteredConnectionStrings.Count == 0)
            {
                throw new Exception(string.Format(CultureInfo.InvariantCulture, "No connection string found in the configuration file."));
            }
            else if (filteredConnectionStrings.Count > 1)
            {
                throw new Exception(string.Format(CultureInfo.InvariantCulture, "Multiple connection strings ({1}) found in the configuration file.", filteredConnectionStrings.Count));
            }

            string connectionString = filteredConnectionStrings[0].Value;

            SetCrmConnectionPassword(credentialFilePath, ref connectionString, entropy);

            return connectionString;
        }

        /// <summary>
        /// Update the connection string with the password from the credential file, if the Password tag is in the connection string.
        /// </summary>
        internal static void SetCrmConnectionPassword(string credentialFilePath, ref string connectionString, byte[] entropy)
        {
            // Do nothing for e.g. Integrated Authentication.
            if (!connectionString.Contains("Password"))
                return;

            int passwordStartAt = connectionString.IndexOf("Password=") + "Password=".Length;
            int passwordEndAt = connectionString.IndexOf(";", passwordStartAt);
            string password = connectionString.Substring(passwordStartAt, passwordEndAt - passwordStartAt);
            if (password != ExternalPasswordPlaceHolder)
            {
                throw new Exception(string.Format(CultureInfo.InvariantCulture, "The password in the connection string is illegal. Only the placeholder \"{0}\" is allowed as password. The password is encrypted in a separate file by running the application with the \"/Password\" parameter.", ExternalPasswordPlaceHolder));
            }

            // Remove Placeholder
            connectionString = connectionString.Remove(passwordStartAt, passwordEndAt - passwordStartAt);
            // Load password from credentials file.
            connectionString = connectionString.Insert(passwordStartAt, LoadCredentials(credentialFilePath, entropy));
        }

        /// <summary>
        /// Store password in the credential file.
        /// </summary>
        internal static void SaveCredentials(string filePath, string password, byte[] entropy)
        {
            XDocument saveDoc = new XDocument(
                new XElement("Root",
                        new XElement("Password", EncryptString(ToSecureString(password), entropy))
                )
            );
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            saveDoc.Save(filePath);
        }

        /// <summary>
        /// Read password from the credential file.
        /// </summary>
        internal static string LoadCredentials(string filePath, byte[] entropy)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The credentials file \"{0}\" cannot be found. Use the /Password: parameter to create the credential file.", filePath);
            }
            XDocument doc = XDocument.Load(filePath);
            return ToInsecureString(DecryptString(doc.Root.Element("Password").Value, entropy));
        }

        public static string EncryptString(System.Security.SecureString input, byte[] entropy)
        {
            byte[] encryptedData = System.Security.Cryptography.ProtectedData.Protect(
                System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)),
                entropy,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData, byte[] entropy)
        {
            try
            {
                byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    entropy,
                    System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData));
            }
            catch (Exception)
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
}
