using System;
using System.Windows;
using System.Windows.Controls;
using CRM.GetBIFFTransactions.ViewModel;
using System.Windows.Browser;
using CGIXrm;
using System.Text.RegularExpressions;

namespace CRM.GetBIFFTransactions
{
    public partial class MainPage : UserControl
    {
        MainPageViewModel _mainPageViewModel;
        string _urlParameters;

        string _querystring = "";
        string _initparams = "";
        string _data = "";
        string _id = "";
        string _userlcid = "1053";
        string _debug = "";
        string _debugcard = "";

        public MainPage(string querystring, string initparams)
        {
            try
            {
                InitializeComponent();
                _querystring = querystring;
                _initparams = initparams;

                CrmManager _crmmanager = null;
                WebParameters _webparams = new WebParameters();

                string[] _params = Regex.Split(_querystring, "&");
                foreach (string _key in _params)
                {
                    if (!string.IsNullOrEmpty(_key))
                    {
                        string[] _values = Regex.Split(_key, "=");
                        if (_values[0].ToLower().ToString() == "data")
                            _data = _values[1].ToString();

                        if (_values[0].ToLower().ToString() == "userlcid")
                            _userlcid = _values[1].ToString();

                        if (_values[0].ToLower().ToString() == "id")
                            _id = _values[1].ToString();

                        if (_values[0].ToLower().ToString() == "debug")
                            _debug = _values[1].ToString();

                        if (_values[0].ToLower().ToString() == "travelcard")
                            _debugcard = _values[1].ToString();
                    }
                }

                txtData.Text = _data;
                txtid.Text = _id;
                txtUserlcdid.Text = _userlcid;


                string clientUrl = "";
                ScriptObject xrm = (ScriptObject)HtmlPage.Window.GetProperty("Xrm");
                ScriptObject page = (ScriptObject)xrm.GetProperty("Page");
                ScriptObject pageContext = (ScriptObject)page.GetProperty("context");
                clientUrl = (string)pageContext.Invoke("getClientUrl");

                _crmmanager = new CrmManager(clientUrl);

                _mainPageViewModel = new MainPageViewModel(this, _crmmanager, Dispatcher, _userlcid, _id, _data, _debug, _debugcard);

            }
            catch (Exception ex)
            {
                ShowError(ex, "MainPage");
            }
        }

        public void ShowError(Exception ex, string module)
        {
            Dispatcher.BeginInvoke(() =>
            {
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

        public void ShowMessage(string message, string module)
        {
            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(message, module, MessageBoxButton.OK);
            });
        }

        [ScriptableMember]
        public void SetEntityID(string entityid)
        {
            try
            {
                if (_data.ToUpper() == "TRAVELCARD")
                {
                    txtid.Text = entityid;
                    _mainPageViewModel.StartSearchTravelCardFromBIFF(entityid);
                }
                else
                {
                    txtid.Text = entityid;
                    _mainPageViewModel.LoadSavedBIFFTransactions(entityid);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "SetEntityID");
            }
        }
               
        [ScriptableMember]
        public void SetTravelCardNumber(string travelcardid)
        {
            try
            {
                _mainPageViewModel.SetTravelCardFromCase(travelcardid);
            }
            catch (Exception ex)
            {
                ShowError(ex, "SetTravelCardNumber");
            }
        }

		private void HyperlinkButton_Click(object sender, RoutedEventArgs e) {
		  HyperlinkButton btn = sender as HyperlinkButton;
		  MessageBox.Show(btn.NavigateUri.AbsoluteUri);
		  new Exception(btn.NavigateUri.AbsoluteUri.ToString());
		}

    }
}
