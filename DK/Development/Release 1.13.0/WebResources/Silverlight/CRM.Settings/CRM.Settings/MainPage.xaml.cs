using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using CGIXrm;
using CGIXrm.CrmSdk;
using System.Collections.ObjectModel;
using System.Windows.Browser;

namespace CRM.Settings
{
    public partial class MainPage : UserControl
    {

        CrmManager _manager;

        public string Version
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1].Trim(); }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string _xml = "";
                _xml += "<fetch distinct='false' mapping='logical' version='1.0'>";
                _xml += "   <entity name='cgi_setting'>";
                _xml += "       <attribute name='cgi_settingid'/>";
                _xml += "       <attribute name='cgi_name'/>";
                _xml += "       <attribute name='cgi_data'/>";
                _xml += "       <filter type='and'>";
                _xml += "           <condition attribute='cgi_name' value='CASE_UNKNOWN_CUSTOMER' operator='eq'/>";
                _xml += "       </filter>";
                _xml += "   </entity>";
                _xml += "</fetch>";

                _manager.Fetch<Setting>(_xml, buttonClick_callback);
            }
            catch (Exception ex)
            {
                _showMessageBox(ex.Message);
            }
        }

        private void buttonClick_callback(ObservableCollection<Setting> settings)
        {
            try
            {
                if (settings.Count() > 0)
                {
                    _showMessageBox("Standardkund finns redan!");
                }
                else
                {
                    _createDefaultCustomerSetting();
                }
            }
            catch (Exception ex)
            {
                _showMessageBox(ex.Message);
            }
        }

        private void _createDefaultCustomerSetting()
        {
            try
            {
                Setting _s = new Setting();
                _s.Name = "CASE_UNKNOWN_CUSTOMER";
                _s.Data = "Okänd Kund";
                _manager.Create(_s, _createDefaultCustomerSetting_callback);
            }
            catch (Exception ex)
            {
                _showMessageBox(ex.Message);
            }
        }

        private void _createDefaultCustomerSetting_callback(Setting setting)
        {
            _showMessageBox("Standardkund tillagd!");
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            lblVersion.Text = Version;
            string _url = "";
            WebParameters _webparams = new WebParameters();

            try
            {
                if (_webparams.ServerAddress == null)
                {
                    string obj = System.Windows.Browser.HtmlPage.Window.Invoke("getParamsFromHTMLpage") as string;
                    int _char = obj.IndexOf("WebResources");
                    _url = obj.Substring(0, _char - 1);
                }
                else
                {
                    _url = _webparams.ServerAddress;
                }
            }
            catch (Exception ex)
            {
                _showMessageBox(ex.Message);
            }
            
            _manager = new CrmManager(_url);
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void _showMessageBox(string message)
        {
            Dispatcher.BeginInvoke(() => { MessageBox.Show(message, "Settings", MessageBoxButton.OK); });
        }
    }

    [XrmEntity("cgi_setting")]
    public class Setting : XrmBaseEntity
    {
        [XrmPrimaryKey]
        [Xrm("cgi_settingid")]
        public Guid SettingID { get; set; }

        [Xrm("cgi_name")]
        public string Name { get; set; }

        [Xrm("cgi_data")]
        public string Data { get; set; }
    
    }
}
