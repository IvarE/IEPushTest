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

using CRM.Travel.Information.ViewModel;
using CRM.Travel.Information.Classes;
using System.Collections.ObjectModel;
using System.Windows.Browser;
using System.Threading;
using System.Windows.Threading;

namespace CRM.Travel.Information.Views
{
    public partial class MainPage : UserControl
    {

        MainPageViewModel _mainPageViewModel;
        
        public MainPage(string querystring, string initparams)
        {
            try
            {
                InitializeComponent();

                string clientUrl = "";
                ScriptObject xrm = (ScriptObject)HtmlPage.Window.GetProperty("Xrm");
                ScriptObject page = (ScriptObject)xrm.GetProperty("Page");
                ScriptObject pageContext = (ScriptObject)page.GetProperty("context");
                clientUrl = (string)pageContext.Invoke("getClientUrl");

                string _querystring = querystring;
                string _initparams = initparams;
                _mainPageViewModel = new MainPageViewModel(this, Dispatcher, clientUrl, _querystring, _initparams);
                Version.Text = _mainPageViewModel.Version;
            }
            catch (Exception ex)
            {
                ShowError(ex, "MainPage");
            }
        }

        public void ShowError(Exception ex, string module)
        {
            Dispatcher.BeginInvoke(() => {

                string message = string.Empty;
                Exception innerException = ex;

                do
                {
                    message = message + (string.IsNullOrEmpty(innerException.Message) ? string.Empty : innerException.Message);
                    innerException = innerException.InnerException;
                }
                while (innerException != null);

                string _message = string.Format("{0}\n{1}", module, message);
                MessageBox.Show(_message, "Error", MessageBoxButton.OK);  
            });
        }
        
        public void ShowMessage(string error, string module)
        {
            Dispatcher.BeginInvoke(() =>
            {
                string _message = string.Format("{0}\n{1}", module, error);
                MessageBox.Show(_message, "Error", MessageBoxButton.OK);
            });
        }

        [ScriptableMember]
        public void SetEntityID(string entityid)
        {
            try
            {
                _mainPageViewModel.GetSavedLines(entityid);
            }
            catch (Exception ex)
            {
                ShowError(ex, "SetEntityID");
            }
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //set focus on first combobox
                cboTransport.Focus();
            }
            catch (Exception ex)
            {
                ShowError(ex, "SetEntityID");
            }
        }

        
        
    }
}
