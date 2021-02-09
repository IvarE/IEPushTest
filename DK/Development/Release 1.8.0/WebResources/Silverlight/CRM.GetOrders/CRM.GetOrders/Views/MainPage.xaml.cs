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
using CRM.GetOrders.ViewModel;
using CGIXrm;
using System.Collections.ObjectModel;
using System.Windows.Browser;

namespace CRM.GetOrders
{
    public partial class MainPage : UserControl
    {

        //CRM.GetOrdersTestPage.html?serveraddress=http://v-dkcrm-utv/Skanetrafiken&userlcid=1033&id=416758F3-5345-E411-80D1-0050569010AD

        private MainPageViewModel _mainPageViewModel;
        private WebParameters _webParameters;
        private CrmManager _crmManager;

        const string LocalizedLabelGroup = "SILVERLIGHT_GET_ORDERS";

        public MainPage(string querystring, string initparams)
        {
            InitializeComponent();
            _webParameters = new WebParameters();
            _crmManager = new CrmManager(_webParameters.ServerAddress);
            _mainPageViewModel = new MainPageViewModel(_webParameters, _crmManager, Dispatcher, this, querystring, initparams);
        }

        public string UserFullName { get { return _mainPageViewModel._userFullName; } private set { } }

        public void ShowError(Exception ex, string module)
        {
            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(ex.Message, module, MessageBoxButton.OK);
            });
        }

        public void ShowErrorMessage(string error, string module)
        {
            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(error, module, MessageBoxButton.OK);
            });
        }

        [ScriptableMember]
        public void SetEntityID(string entityid, string userFullName)
        {
            try
            {
                _mainPageViewModel._userFullName = userFullName;
                _mainPageViewModel.GetCreditRows_OnlyFromAccountOrContactForm();
            }
            catch (Exception ex)
            {
                ShowError(ex, "SetEntityID");
            }
        }

    }

}

