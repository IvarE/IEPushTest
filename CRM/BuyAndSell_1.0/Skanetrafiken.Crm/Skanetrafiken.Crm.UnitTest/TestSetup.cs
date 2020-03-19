using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Reflection;

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using System.Net;

namespace Endeavor.Crm.UnitTest
{
    [SetUpFixture]
    public class TestSetup
    {
        private static ServerConnection.Configuration _config;

        public TestSetup()
        {
        }

        [STAThread, OneTimeSetUp(), Apartment(System.Threading.ApartmentState.STA)]
        public void SetupConfig()
        {

            string password = string.Empty;
            string username = Properties.Settings.Default.UserName;

            if (!Properties.Settings.Default.UseDefaultCredentials)
            {
                string credentialFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Endeavor", Path.ChangeExtension(Assembly.GetAssembly(typeof(TestSetup)).GetName().Name, "xml"));

                if (File.Exists(credentialFile))
                {
                    XDocument doc = XDocument.Load(credentialFile);
                    username = doc.Root.Element("Username").Value;
                    password = Configuration.ToInsecureString(Configuration.DecryptString(doc.Root.Element("Password").Value));
                }

                if (password.Length == 0)
                {
                    throw new Exception(string.Format("The credential file {0} is missing. Please use Endeavor nUnit Credentials Manager to create the credential file.", credentialFile));

                    //// INFO: (hest) Get password from user. This is not really a good idea for unit tests since the can't run without user interaction.
                    //user = PromptForPassword.ShowDialog(username, password);
                    //username = user.Item1;
                    //password = user.Item2;

                    //XDocument saveDoc = new XDocument(
                    //    new XElement("Root",
                    //         new XElement("Username", username),
                    //         new XElement("Password", Configuration.EncryptString(Configuration.ToSecureString(password)))
                    //    )
                    //);
                    //Directory.CreateDirectory(Path.GetDirectoryName(credentialFile));
                    //saveDoc.Save(credentialFile);
                }
            }

            Tuple<string, string> user = new Tuple<string, string>(username, password);

            if (_config == null)
            {
                _config = new ServerConnection.Configuration();
                _config.DiscoveryUri = new Uri(Properties.Settings.Default.DiscoveryUri);
                _config.EndpointType = (AuthenticationProviderType)Enum.Parse(typeof(AuthenticationProviderType), Properties.Settings.Default.EndpointType);
                if (!string.IsNullOrEmpty(Properties.Settings.Default.HomeRealmUri))
                    _config.HomeRealmUri = new Uri(Properties.Settings.Default.HomeRealmUri);
                _config.OrganizationName = Properties.Settings.Default.OrganizationName;
                _config.OrganizationUri = new Uri(Properties.Settings.Default.OrganizationUri);
                _config.ServerAddress = Properties.Settings.Default.ServerAddress;


                if (_config.EndpointType == AuthenticationProviderType.ActiveDirectory)
                {
                    _config.Credentials = new ClientCredentials();
                    System.Net.NetworkCredential cred = System.Net.CredentialCache.DefaultNetworkCredentials;
                    if (!Properties.Settings.Default.UseDefaultCredentials)
                        cred = new System.Net.NetworkCredential(user.Item1, user.Item2, Properties.Settings.Default.Domain);
                    _config.Credentials.Windows.ClientCredential = cred;
                }
                else if (_config.EndpointType == AuthenticationProviderType.Federation)
                {
                    _config.Credentials = new ClientCredentials();
                    _config.Credentials.UserName.UserName = user.Item1;
                    _config.Credentials.UserName.Password = user.Item2;

                }
                else if (_config.EndpointType == AuthenticationProviderType.OnlineFederation)
                {
                    _config.Credentials = new ClientCredentials();
                    _config.Credentials.UserName.UserName = user.Item1;
                    _config.Credentials.UserName.Password = user.Item2;
                }
                else
                    throw new NotImplementedException(string.Format("AuthenticationProviderType {0} is supported for now.", _config.EndpointType.ToString()));

            }

            System.Console.WriteLine(string.Format("Connecting to organization:{0} using user:{1}", _config.OrganizationName, user.Item1));

        }

        internal static ServerConnection.Configuration Config
        {
            get
            {
                return _config;
            }
        }

    }
}
