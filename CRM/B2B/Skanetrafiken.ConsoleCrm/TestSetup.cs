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

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace Endeavor.Crm.UnitTest
{
    public static class PromptForPassword
    {
        public static Tuple<string, string> ShowDialog(string user, string password)
        {
            Form prompt = new Form() { FormBorderStyle = FormBorderStyle.FixedDialog, TopMost = true };
            prompt.Width = 300;
            prompt.Height = 180;
            prompt.Text = "Enter Credentials";
            System.Windows.Forms.Label textLabelUser = new System.Windows.Forms.Label() { Left = 10, Top = 30, Text = "User: "};
            TextBox textBoxUser = new TextBox() { Left = textLabelUser.Left + textLabelUser.Width + 10, Top = textLabelUser.Top, Width = 150, UseSystemPasswordChar = false, Text = user };

            System.Windows.Forms.Label textLabelPassword = new System.Windows.Forms.Label() { Left = 10, Top = 60, Text = "Password: " };
            TextBox textBoxPassword = new TextBox() { Left = textLabelPassword.Left + textLabelPassword.Width + 10, Top = textLabelPassword.Top, Width = 150, UseSystemPasswordChar = true, Text = password };

            Button confirmation = new Button() { Text = "Ok", Left = 170, Width = 50, Top = 100, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            Button cancellation = new Button() { Text = "Cancel", Left = 230, Width = 50, Top = 100, DialogResult = DialogResult.Cancel };
            cancellation.Click += (sender, e) => { prompt.Close(); };
            
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancellation);
            prompt.Controls.Add(textLabelUser);
            prompt.Controls.Add(textBoxUser);
            prompt.Controls.Add(textLabelPassword);
            prompt.Controls.Add(textBoxPassword);
            DialogResult result = prompt.ShowDialog();
            if (result == DialogResult.Cancel)
                throw new Exception("Credential entry cancelled.");
            return new Tuple<string, string>(textBoxUser.Text, textBoxPassword.Text);
        }
    }
    [SetUpFixture]
    public class TestSetup
    {
        private static ServerConnection.Configuration _config;

        public TestSetup()
        {
        }

        [SetUp, STAThread, RequiresSTA]
        public void SetupConfig()
        {
            string password = string.Empty;
            string username = Properties.Settings.Default.UserName;

            // Johan test.
            _config = null;

            string credentialFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Endeavor", "Endeavor.Crm.UnitTest.Internal.Credential.xml");



            if (File.Exists(credentialFile))
            {
                XDocument doc = XDocument.Load(credentialFile);
                username = doc.Root.Element("Username").Value;
                password = Configuration.ToInsecureString(Configuration.DecryptString(doc.Root.Element("Password").Value));
            }

            Tuple<string, string> user = new Tuple<string, string>(username, password);

            if (password.Length == 0)
            { 

                // INFO: (hest) Get password from user. This is not really a good idea for unit tests since the can't run without user interaction.
                user = PromptForPassword.ShowDialog(username, password);
                username = user.Item1;
                password = user.Item2;

                XDocument saveDoc = new XDocument(
                    new XElement("Root",
                         new XElement("Username", username),
                         new XElement("Password", Configuration.EncryptString(Configuration.ToSecureString(password)))
                    )
                );
                Directory.CreateDirectory(Path.GetDirectoryName(credentialFile));
                saveDoc.Save(credentialFile);
            }


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
                    System.Net.NetworkCredential cred = new System.Net.NetworkCredential(user.Item1, user.Item2, Properties.Settings.Default.Domain);
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

            System.Console.WriteLine(string.Format("Connecting to organization:{0} using user:{1}", _config.OrganizationName, _config.Credentials.UserName));

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
