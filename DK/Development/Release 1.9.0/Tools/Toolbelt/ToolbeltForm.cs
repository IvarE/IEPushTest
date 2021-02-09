using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Web.Administration;
using Toolbelt.Classes;

namespace Toolbelt
{
    public partial class ToolbeltForm : Form
    {
        #region Declarations
        ServerManager _remote;
        #endregion

        #region Constructors
        public ToolbeltForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void fetchAppPoolsBtn_Click(object sender, EventArgs e)
        {
            appPoolsLogTextBox.Text += "Fetching app pools... ";

            AppPoolsOnServer.Items.Clear();

            var checkedButton = serversGroupBox.Controls.OfType<RadioButton>()
                .FirstOrDefault(r => r.Checked);

            if (checkedButton != null)
            {
                var server = checkedButton.Text;

                _remote = ServerManager.OpenRemote(server);
            }

            foreach (var appPool in _remote.ApplicationPools)
            {
                AppPoolsOnServer.Items.Add(appPool.Name);
            }

            appPoolsLogTextBox.Text += "done" + Environment.NewLine + Environment.NewLine;
        }

        private void restartAppPoolsBtn_Click(object sender, EventArgs e)
        {
            appPoolsLogTextBox.Text += "Recycling app pools..." + Environment.NewLine;

            var items = AppPoolsOnServer.CheckedItems;

            var checkedAppPools = _remote.ApplicationPools.Where(x => items.Contains(x.Name));

            foreach (var checkedAppPool in checkedAppPools)
            {
                appPoolsLogTextBox.Text += checkedAppPool.Name + ": recycling, ";
                checkedAppPool.Recycle();
                appPoolsLogTextBox.Text += "done" + Environment.NewLine;
            }

            appPoolsLogTextBox.Text += "Finished recycling app pools" + Environment.NewLine + Environment.NewLine;
        }

        private void fetchSitesBtn_Click(object sender, EventArgs e)
        {
            sitesLogTextBox.Text += "Fetching sites... ";

            sitesList.Items.Clear();

            var checkedButton = serversGroupBox2.Controls.OfType<RadioButton>()
                .FirstOrDefault(r => r.Checked);

            if (checkedButton != null)
            {
                var server = checkedButton.Text;

                _remote = ServerManager.OpenRemote(server);
            }

            foreach (var site in _remote.Sites)
            {
                sitesList.Items.Add(site.Name);
            }

            sitesLogTextBox.Text += "done" + Environment.NewLine + Environment.NewLine;
        }

        private void restartSitesBtn_Click(object sender, EventArgs e)
        {
            sitesLogTextBox.Text += "Restarting sites..." + Environment.NewLine;

            var sitesToRestart = sitesList.CheckedItems;

            var checkedSites = _remote.Sites.Where(x => sitesToRestart.Contains(x.Name));

            foreach (var site in checkedSites)
            {
                sitesLogTextBox.Text += site.Name + ": stopping,";
                site.Stop();
                sitesLogTextBox.Text += " restarting, ";
                site.Start();
                sitesLogTextBox.Text += "done" + Environment.NewLine;
            }

            sitesLogTextBox.Text += "Finished restarting sites" + Environment.NewLine + Environment.NewLine;
        }

        private void sitesLogClearBtn_Click(object sender, EventArgs e)
        {
            sitesLogTextBox.Text = string.Empty;
        }

        private void appPoolsLogClearBtn_Click(object sender, EventArgs e)
        {
            appPoolsLogTextBox.Text = string.Empty;
        }
        #endregion
    }
}
