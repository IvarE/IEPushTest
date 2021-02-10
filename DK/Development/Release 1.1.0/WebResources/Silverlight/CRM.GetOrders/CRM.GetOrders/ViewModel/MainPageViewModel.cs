#define WCFDEBUG
#undef WCFDEBUG

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CGIXrm;
using CGIXrm.CrmSdk;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using CRM.GetOrders.GetOrdersServiceReference;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text.RegularExpressions;
using CRM.GetOrders.Views;
using System.Windows.Browser;
using System.Windows.Data;
using System.Globalization;

namespace CRM.GetOrders.ViewModel
{
    // Accountid =  F37B570B-65F0-E311-80CE-0050569010AD
    // CRM.GetOrdersTestPage.html?serveraddress=http://10.16.229.201/skanetrafiken&userlcid=1053&id=F37B570B-65F0-E311-80CE-0050569010AD&debug=true&data=ACCOUNT
    // From V-DKVS-UTV
    // CRM.GetOrdersTestPage.html?serveraddress=http://V-DKCRM-UTV/skanetrafiken&userlcid=1053&id=CC0B2EE0-227A-E411-80D4-0050569010AD&debug=true&data=CONTACT&email=kjerstilundemo@hotmail.com
    //



    //              F37B570B-65F0-E311-80CE-0050569010AD
    // Contactid = FF4DEEAB-E908-E411-80D1-0050569010AD
    // CRM.GetOrdersTestPage.html?serveraddress=http://10.16.229.201/skanetrafiken&userlcid=1053&id=FF4DEEAB-E908-E411-80D1-0050569010AD&debug=true&data=CONTACT


    // CRM.GetOrdersTestPage.html?serveraddress=http://10.16.229.201/skanetrafiken&userlcid=1053

    // CRM.GetOrdersTestPage.html?serveraddress=http://10.16.229.201/skanetrafiken&userlcid=1053&id=6C4BE848-31F6-E311-80CE-0050569010AD&debug=true&data=ACCOUNT

    // CRM.GetOrdersTestPage.html?serveraddress=http://10.16.229.201/skanetrafiken&userlcid=1053&id=6C4BE848-31F6-E311-80CE-0050569010AD&debug=true

    // marcus.malmestad@starrepublic.com CONTACT 019E0C6E-0382-E411-80D4-005056902292
    // CRM.GetOrdersTestPage.html?serveraddress=http://V-DKCRM-TST/skanetrafiken&userlcid=1053&id=019E0C6E-0382-E411-80D4-005056902292&debug=true&data=CONTACT&email=marcus.malmestad@starrepublic.com

    // marcus.malmestad@starrepublic.com
    // CRM.GetOrdersTestPage.html?serveraddress=http://V-DKCRM-TST/skanetrafiken&userlcid=1053&id=019E0C6E-0382-E411-80D4-005056902292&debug=true&data=CONTACT&email=marcus.malmestad@starrepublic.com

    //http://v-dkcrm-utv/Skanetrafiken/main.aspx?etc=112&extraqs=&histKey=207841729&id=%7b6C4BE848-31F6-E311-80CE-0050569010AD%7d&newWindow=true&pagetype=entityrecord

    //http://v-dkcrm-utv/Skanetrafiken/main.aspx?etc=2&extraqs=&histKey=645677701&id=%7b83683883-F158-E411-80D2-0050569010AD%7d&newWindow=true&pagetype=entityrecord


    // https://sekund.skanetrafiken.se/DKCRM/main.aspx#800395063
    // CRM.GetOrdersTestPage.html?serveraddress=http://v-dkcrm-utv/Skanetrafiken&userlcid=1053&id=095861DC-6DF2-E411-80DB-0050569010AD&debug=true&data=CONTACT&email=martinandreassondk@gmail.com

    // susanne.jonsson@skanetrafiken.se
    // C822DC25-2B3E-E411-80D1-0050569010AD
    // CRM.GetOrdersTestPage.html?serveraddress=http://v-dkcrm-utv/Skanetrafiken&userlcid=1053&id=C822DC25-2B3E-E411-80D1-0050569010AD&debug=true&data=CONTACT&email=susanne.jonsson@skanetrafiken.se

    // christian.ravantti@starrepublic.com
    // 92888631-2FFE-E411-80DB-0050569010AD
    // CRM.GetOrdersTestPage.html?serveraddress=http://v-dkcrm-utv/Skanetrafiken&userlcid=1053&id=92888631-2FFE-E411-80DB-0050569010AD&debug=true&data=CONTACT&email=christian.ravantti@starrepublic.com

    // CRM.GetOrdersTestPage.html?serveraddress=https://sekunduat.skanetrafiken.se/DKCRMUAT&&userlcid=1053&id=F0BAD711-D1E4-E411-80D7-005056906AE2&debug=true&data=CONTACT&email=dktestare@gmail.com

    // NEW TO LINKS. ADD typename=contact or typename=account



    /// <summary>
    /// Notes by A.J
    /// At debug, with debug param supplied in browser, plus id param, this program gets credit rows first. Then at search get credit rows again, then orders.
    /// If not debug, in CRM, not sure if it retrieves credit rows first, check in CRM.
    /// 
    /// _createNewOrder ( odd name, should be something like _createOrderRowForOrdersList ) is called once for each order.
    /// 
    /// What I can see, it looks like after a credit has been completed, and child window closed, credit rows are retrieved again to refresh. However I cannot see
    /// that orders are retrieved again. Which probably means that stale data regarding amount that can be credited exist in order rows? Investigate.
    /// 
    /// WARNING: logic to determine the form type is not by using the regular parameter that you could pass in by the form, but by adding a custom parameter in the 
    /// web resource ( html ) registration page, "acccount" or "contact". It will be availible in the data parameter. TODO refactor to use the regular one to avoid
    /// confusion. The debug feature in VS run, should have its urls changed to use the regular one also ( manually )
    /// 
    /// </summary>
    public class MainPageViewModel : XrmBaseNotify
    {
        private GetOrdersServiceClient _ordersClient;

        ObservableCollection<OrderHeader_ViewObject> _orderSearchResultCollection;

        private WebParameters _webParameters;
        private CrmManager _crmManager;
        private Dispatcher _dispatcher;
        private MainPage _mainpage;

        private string _querystring;
        private string _initparams;

        private ObservableCollection<CreditOrderRow> _creditOrderRows;
        private ObservableCollection<CreditOrderRow_ViewObject> _creditOrderRow_ViewObjects;
        private ObservableCollection<Language> _language;

        //private bool _canEditOrder = false;

        // =========================================================================    

        private bool _isWaiting;
        public bool IsWaiting
        {
            get { return _isWaiting; }
            set { _isWaiting = value; OnPropertyChanged("IsWaiting"); }
        }

        // TODO: rename Version, check so not in use in xaml
        public string version
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1].Trim(); }
        }

        private string _orderNrCaption;
        public string OrderNrCaption
        {
            get { return _orderNrCaption; }
            set { _orderNrCaption = value; OnPropertyChanged("OrderNrCaption"); }
        }

        private string _dateFromCaption;
        public string DateFromCaption
        {
            get { return _dateFromCaption; }
            set { _dateFromCaption = value; OnPropertyChanged("DateFromCaption"); }
        }

        private string _dateToCaption;
        public string DateToCaption
        {
            get { return _dateToCaption; }
            set { _dateToCaption = value; OnPropertyChanged("DateToCaption"); }
        }

        private string _searchCaption;
        public string SearchCaption
        {
            get { return _searchCaption; }
            set { _searchCaption = value; OnPropertyChanged("SearchCaption"); }
        }

        private string _saveCaption;
        public string SaveCaption
        {
            get { return _saveCaption; }
            set { _saveCaption = value; OnPropertyChanged("SaveCaption"); }
        }

        private bool _isExecutingInStandaloneSearchOrderArea;
        public bool IsExecutingInStandaloneSearchOrderArea
        {
            get { return _isExecutingInStandaloneSearchOrderArea; }
            set
            {
                _isExecutingInStandaloneSearchOrderArea = value;
                OnPropertyChanged("IsExecutingInStandaloneSearchOrderArea");
            }
        }

        private string linkToContactOrAccount;
        public string LinkToContactOrAccount
        {
            get
            {
                return linkToContactOrAccount;
            }
            set
            {
                linkToContactOrAccount = value;
                OnPropertyChanged("LinkToContactOrAccount");
            }
        }

        private string linkName;
        public string LinkName
        {
            get
            {
                return linkName;
            }
            set
            {
                linkName = value;
                OnPropertyChanged("LinkName");
            }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _accountid;
        public string Accountid
        {
            get { return _accountid; }
            set
            {
                _accountid = value;
                _contactid = string.Empty;
                // TODO: possible set _id
            }
        }

        private string _contactid;
        public string Contactid
        {
            get { return _contactid; }
            set
            {
                _contactid = value;
                _accountid = string.Empty;
                // TODO: possible set _id
            }
        }

        private string _emailAddress;
        public string Emailaddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        private string _cardNumber;
        public string CardNumber
        {
            get { return _cardNumber; }
            set { _cardNumber = value; }
        }

        private string _debug;
        public string Debug
        {
            get { return _debug; }
            set { _debug = value; }
        }
        
        public string _userFullName { get; set; }

        public string _formTypeName { get; set; }

        public setting Setting { get; set; }

        #region constructor

        public MainPageViewModel(WebParameters webParameters, CrmManager crmManager, Dispatcher dispatcher, MainPage mainpage, string querystring, string initparams)
        {
            try
            {
                //_canEditOrder = false;
                IsWaiting = true;
                _querystring = querystring;
                _initparams = initparams;

                ButtonSearchCommand = new RelayCommand(SearchCommand, true);
                ButtonOpenContactOrAccountCommand = new RelayCommand(OpenContactOrAccountCommand, true);

                _webParameters = webParameters;
                _crmManager = crmManager;
                _dispatcher = dispatcher;
                _mainpage = mainpage;

                _debug = GetQueryParametersKey(_querystring, "debug");
                //_data = _getKey(_querystring, "data");
                _id = GetQueryParametersKey(_querystring, "id");
                _id = _id.Replace("{", string.Empty).Replace("}", string.Empty);

                _formTypeName = GetQueryParametersKey(_querystring, "typename");
                _emailAddress = GetQueryParametersKey(_querystring, "email");

                // TODO: in HTML we pass a null, that could be checked otherwise that should be refactored.
                if (string.IsNullOrEmpty(_debug) && string.IsNullOrEmpty(_formTypeName))
                {
                    IsExecutingInStandaloneSearchOrderArea = true;
                }
                else
                {
                    IsExecutingInStandaloneSearchOrderArea = false;
                }
                
                if (!string.IsNullOrEmpty(_debug) && _formTypeName.ToUpper() == "ACCOUNT")
                {
                    Accountid = _id;
                    _userFullName = "DKTESTARE DKTESTSSON";
                }

                if (!string.IsNullOrEmpty(_debug) && _formTypeName.ToUpper() == "CONTACT")
                {
                    Contactid = _id;
                    _userFullName = "DKTESTARE DKTESTSSON";
                }

                if (_formTypeName.ToUpper() == "ACCOUNT")
                {
                    Accountid = _id;
                }

                if (_formTypeName.ToUpper() == "CONTACT")
                {
                    Contactid = _id;
                }

                _mainpage.DataContext = this;
                
                // INFO: In standalonemode sometimes the initial waiting popup loads at start
                // and never goes away. This stops that. 
                // UPDATE: proberbly not needed since _ordersClient_GetOrdersCompleted had it finally IsWaiting uncommented.
                // Needs to be tested though.
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });

                InitOrderClient();
                //_checkIfUserCanEditOrder();

                DateTime baseDate = DateTime.Today;
                var thisMonthStart = baseDate.AddDays(-45);         // baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                var thisDay = baseDate;

                _mainpage.dtTimeFrom.Text = thisMonthStart.ToShortDateString();
                _mainpage.dtTimeTo.Text = thisDay.ToShortDateString();

                _mainpage.dg1.MouseLeftButtonUp += dg1_MouseLeftButtonUp;
                _mainpage.dtTimeFrom.LostFocus += dtTimeFrom_LostFocus;
                _mainpage.dtTimeTo.LostFocus += dtTimeTo_LostFocus;
                _mainpage.dtTimeFrom.GotFocus += dtTimeFrom_GotFocus;
                _mainpage.dtTimeTo.GotFocus += dtTimeTo_GotFocus;

            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "MainPageViewModel");
            }
        }

        void dtTimeTo_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_mainpage.dtTimeTo.Text))
                {
                    _mainpage.dtTimeTo.SelectAll();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "dtTimeTo_GotFocus");
            }
        }

        void dtTimeFrom_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_mainpage.dtTimeFrom.Text))
                {
                    _mainpage.dtTimeFrom.SelectAll();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "dtTimeFrom_GotFocus");
            }
        }

        private void dtTimeTo_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_mainpage.dtTimeTo.Text))
                {
                    string date = CheckFormatOfDate(_mainpage.dtTimeTo.Text);
                    DateTime _dt;
                    bool _ok = DateTime.TryParse(date, out _dt);
                    if (_ok == false)
                        _mainpage.dtTimeTo.Text = "";
                    else
                        _mainpage.dtTimeTo.Text = _dt.ToShortDateString();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "dtTimeTo_LostFocus");
            }
        }

        private void dtTimeFrom_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_mainpage.dtTimeFrom.Text))
                {
                    string date = CheckFormatOfDate(_mainpage.dtTimeFrom.Text);
                    DateTime _dt;
                    bool _ok = DateTime.TryParse(date, out _dt);
                    if (_ok == false)
                        _mainpage.dtTimeFrom.Text = "";
                    else if (_ok == true)
                        _mainpage.dtTimeFrom.Text = _dt.ToShortDateString();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "dtTimeFrom_LostFocus");
            }
        }

        private string CheckFormatOfDate(string input)
        {
            string returnValue = "";

            if (string.IsNullOrEmpty(input))
                return "";

            if (input.Length < 6)
                return "";

            try
            {
                if (input.Length == 6 && input.IndexOf("-") < 0)
                {
                    string _year = input.Substring(0, 2);
                    string _month = input.Substring(2, 2);
                    string _day = input.Substring(4, 2);
                    returnValue = string.Format("{0}-{1}-{2}", _year, _month, _day);
                }

                if (input.Length == 8 && input.IndexOf("-") < 0)
                {
                    string _year = input.Substring(0, 4);
                    string _month = input.Substring(4, 2);
                    string _day = input.Substring(6, 2);
                    returnValue = string.Format("{0}-{1}-{2}", _year, _month, _day);
                }

                if (input.Length == 8 && input.IndexOf("-") >= 0)
                {
                    string _year = input.Substring(0, 2);
                    string _month = input.Substring(3, 2);
                    string _day = input.Substring(6, 2);
                    returnValue = string.Format("{0}-{1}-{2}", _year, _month, _day);
                }

                if (input.Length == 10 && input.IndexOf("-") >= 0)
                {
                    string _year = input.Substring(0, 4);
                    string _month = input.Substring(5, 2);
                    string _day = input.Substring(8, 2);
                    returnValue = string.Format("{0}-{1}-{2}", _year, _month, _day);
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_checkFormatOfDate");
            }

            return returnValue;
        }

        private void dg1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // *********************************************************
                // Removed this.
                //if (_canEditOrder == false)
                //    return;
                // *********************************************************

                DependencyObject dep = (DependencyObject)e.OriginalSource;
                while ((dep != null) && !(dep is DataGridCell))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }
                if (dep == null) return;

                if (dep is DataGridCell)
                {
                    DataGridCell cell = dep as DataGridCell;

                    while ((dep != null) && !(dep is DataGridRow))
                    {
                        dep = VisualTreeHelper.GetParent(dep);
                    }
                    DataGridRow row = dep as DataGridRow;
                    OrderHeader_ViewObject _o = row.DataContext as OrderHeader_ViewObject;
                    if (_o != null)
                    {
                        OrderRowPage _window = new OrderRowPage(_mainpage, _o, _ordersClient, _accountid, _contactid, _creditOrderRows, _language);
                        _window.Closed += Window_Closed;
                        _window.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "dg1_MouseLeftButtonUp");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                IsWaiting = true;
                if (IsExecutingInStandaloneSearchOrderArea)
                {
                    GetCreditRows();
                }
                else
                {
                    // do we not need to also renew the data in order rows in dg1?
                    GetCreditRows();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_window_Closed");
            }
        }

        #endregion constructor

        private string GetQueryParametersKey(string keys, string key)
        {
            string _returnValue = "";
            string[] _params = Regex.Split(keys, "&");
            foreach (string _key in _params)
            {
                if (!string.IsNullOrEmpty(_key))
                {
                    string[] _values = Regex.Split(_key, "=");
                    if (_values[0].ToLower().ToString() == key)
                        _returnValue = _values[1].ToString();
                }
            }

            return _returnValue;
        }

        private void RefreshDataContext()
        {
            _dispatcher.BeginInvoke(() =>
            {
                _mainpage.DataContext = null;
                _mainpage.DataContext = this;
            });
        }

        private void InitOrderClient()
        {
            try
            {
                _crmManager.Fetch<setting>(XMLinitOrderClient(), InitOrderClient_callback);
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_initOrderClient");
            }

        }

        private void InitOrderClient_callback(ObservableCollection<setting> result)
        {
            try
            {
                if (result != null)
                {

                    if (result.Count() > 0)
                    {
                        Setting = result[0] as setting;
                        string _address = Setting.ServiceAddress;
                        CreateOrderClient(_address);
                    }
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_initOrderClient_callback");
            }
        }

        private string XMLinitOrderClient()
        {
            string _xml = "";

            string _now = DateTime.Now.ToString("s");
            _xml += "<fetch distinct='false' mapping='logical' version='1.0'>";
            _xml += " <entity name='cgi_setting'>";
            _xml += "  <attribute name='cgi_ehandelorderservice'/>";
            _xml += "  <attribute name='cgi_crmuri'/>";
            _xml += "  <filter type='and'>";
            _xml += "   <condition attribute='statecode' value='0' operator='eq'/>";
            _xml += "   <condition attribute='cgi_validfrom' value='" + _now + "' operator='on-or-before'/>";
            _xml += "   <filter type='or'>";
            _xml += "    <condition attribute='cgi_validto' value='" + _now + "' operator='on-or-after'/>";
            _xml += "    <condition attribute='cgi_validto' operator='null'/>";
            _xml += "   </filter>";
            _xml += "  </filter>";
            _xml += " </entity>";
            _xml += "</fetch>";

            return _xml;
        }

        private void CreateOrderClient(string serviceaddress)
        {
            try
            {
#if WCFDEBUG
                string _url = "http://localhost:57774/GetOrdersService.svc";
                BasicHttpBinding _webserviceBindning = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
                _webserviceBindning.MaxReceivedMessageSize = int.MaxValue;
                _webserviceBindning.MaxBufferSize = int.MaxValue;
                _webserviceBindning.OpenTimeout = TimeSpan.FromMinutes(10);
                _webserviceBindning.CloseTimeout = TimeSpan.FromMinutes(10);
                _webserviceBindning.ReceiveTimeout = TimeSpan.FromMinutes(10);
                _webserviceBindning.SendTimeout = TimeSpan.FromMinutes(10);
                _webserviceBindning.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                EndpointAddress _webserviceEndpoint = new EndpointAddress(_url);
#else
                BasicHttpBinding _webserviceBindning = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
                _webserviceBindning.MaxReceivedMessageSize = int.MaxValue;
                _webserviceBindning.MaxBufferSize = int.MaxValue;
                _webserviceBindning.OpenTimeout = TimeSpan.FromMinutes(10);
                _webserviceBindning.CloseTimeout = TimeSpan.FromMinutes(10);
                _webserviceBindning.ReceiveTimeout = TimeSpan.FromMinutes(10);
                _webserviceBindning.SendTimeout = TimeSpan.FromMinutes(10);
                _webserviceBindning.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                EndpointAddress _webserviceEndpoint = new EndpointAddress(serviceaddress);
#endif
                _ordersClient = new GetOrdersServiceClient(_webserviceBindning, _webserviceEndpoint);
                InitTranslation(); // TODO: move out
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_createOrderClient");
            }
        }

        private void InitTranslation()
        {
            try
            {
                _crmManager.Fetch<Language>(XMLinitTranslation(), InitTranslation_callback);
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_initTranslation");
            }
        }

        private void InitTranslation_callback(ObservableCollection<Language> result)
        {
            try
            {
                if (result != null && result.Count() > 0)
                {
                    _dispatcher.BeginInvoke(() =>
                    {
                        
                        _mainpage.dg1.Columns[1].Header = GetTranslation("grd1OrderNumber", ".error.", result);
                        _mainpage.dg1.Columns[2].Header = GetTranslation("grd1OrderDate", ".error.", result);
                        _mainpage.dg1.Columns[3].Header = GetTranslation("grd1Total", ".error.", result);
                        _mainpage.dg1.Columns[4].Header = GetTranslation("grd1VAT", ".error.", result);
                        //_mainpage.dg1.Columns[5].Header = GetTranslation("grd1Code", ".error.", result);
                        _mainpage.dg1.Columns[5].Header = "Ej krediterbart";  // GetTranslation("grd1OtherPayment", ".error.", result);
                        _mainpage.dg1.Columns[6].Header = "Kan krediteras";   // GetTranslation("grd1OtherPayment", ".error.", result);
                        _mainpage.dg1.Columns[7].Header = GetTranslation("grd1OrderStatus", ".error.", result);
                        _mainpage.dg1.Columns[8].Header = GetTranslation("grd1OrderType", ".error.", result);
                        
                        _mainpage.dg2.Columns[1].Header = GetTranslation("grd2OrderNumber", ".error.", result);
                        _mainpage.dg2.Columns[2].Header = GetTranslation("grd2Date", ".error.", result);
                        _mainpage.dg2.Columns[3].Header = GetTranslation("grd2Time", ".error.", result);
                        _mainpage.dg2.Columns[4].Header = GetTranslation("grd2Product", ".error.", result);
                        _mainpage.dg2.Columns[5].Header = GetTranslation("grd2ReferenceNr", ".error.", result);
                        _mainpage.dg2.Columns[6].Header = "Skapad av";//;GetTranslation("grd2CreatedBy", ".error.", result);
                        _mainpage.dg2.Columns[7].Header = "Orsak";
                        _mainpage.dg2.Columns[8].Header = GetTranslation("grd2Sum", ".error.", result);
                        
                        OrderNrCaption = GetTranslation("lblOrderNr", ".error.", result);
                        DateFromCaption = GetTranslation("lblDateFrom", ".error.", result);
                        DateToCaption = GetTranslation("lblDateTo", ".error.", result);
                        SearchCaption = GetTranslation("btnSearch", ".error.", result);
                        
                        _language = new ObservableCollection<Language>();
                        _language = result;
                    });

                    if (_debug == "true")
                    {
                        if (!string.IsNullOrEmpty(_id))
                        {
                            GetCreditRows();
                        }
                    }
                    else
                    {
                        if (IsExecutingInStandaloneSearchOrderArea == false)
                        {
                            // TODO anything?
                            GetCreditRows_OnlyFromAccountOrContactForm();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_initTranslation_callback");
            }
        }

        private string GetTranslation(string tag, string error, ObservableCollection<Language> localizedList)
        {
            if (localizedList == null)
                return "error";

            Language found = localizedList.Where<Language>(x => x.Tag == tag).FirstOrDefault();

            if (found != null)
                return found.Name;
            else
                return error;
        }

        private string XMLinitTranslation()
        {
            string _xml = "";

            _xml += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
            _xml += "    <entity name='cgi_localizedlabel'>";
            _xml += "        <attribute name='cgi_localizedlabelid' />";
            _xml += "        <attribute name='cgi_localizedlabelname' />";
            _xml += "        <attribute name='cgi_localizedcontrolid' />";
            _xml += "        <link-entity name='cgi_localizationlanguage' from='cgi_localizationlanguageid' to='cgi_localizationlanguageid' alias='ae'>";
            _xml += "            <filter type='and'>";
            _xml += "                <condition attribute='cgi_localizationlanguagenumber' operator='eq' value='" + _webParameters.UserLcId + "' />";
            _xml += "            </filter>";
            _xml += "        </link-entity>";
            _xml += "        <link-entity name='cgi_localizedlabelgroup' from='cgi_localizedlabelgroupid' to='cgi_localizedlabelgroupid' alias='af'>";
            _xml += "            <filter type='and'>";
            _xml += "                <condition attribute='cgi_localizedlabelgroupname' operator='eq' value='SILVERLIGHT_GET_ORDERS' />";
            _xml += "            </filter>";
            _xml += "        </link-entity>";
            _xml += "    </entity>";
            _xml += "</fetch>";

            return _xml;
        }

        #region Commands

        private RelayCommand pButtonSaveCommand;
        public RelayCommand ButtonSaveCommand
        {
            get { return pButtonSaveCommand; }
            set
            {
                if (pButtonSaveCommand != value)
                {
                    pButtonSaveCommand = value;
                    OnPropertyChanged("ButtonSaveCommand");
                }
            }
        }

        private RelayCommand pButtonSearchCommand;
        public RelayCommand ButtonSearchCommand
        {
            get { return pButtonSearchCommand; }
            set
            {
                if (pButtonSearchCommand != value)
                {
                    pButtonSearchCommand = value;
                    OnPropertyChanged("ButtonSearchCommand");
                }
            }
        }

        private RelayCommand pButtonOpenContactOrAccountCommand;
        public RelayCommand ButtonOpenContactOrAccountCommand
        {
            get { return pButtonOpenContactOrAccountCommand; }
            set
            {
                if (pButtonOpenContactOrAccountCommand != value)
                {
                    pButtonOpenContactOrAccountCommand = value;
                    OnPropertyChanged("ButtonOpenContactOrAccountCommand");
                }
            }
        }

        #endregion

        #region Command functions

        private void OpenContactOrAccountCommand()
        {
            try
            {
                _dispatcher.BeginInvoke(() =>
                {
                    //System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(this.LinkToContactOrAccount), "_newWindow", "toolbar=no, status=no, resizable=yes");
                    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(this.LinkToContactOrAccount), "_parent");                    
                });
            }
            catch (Exception ex)
            {
                IsWaiting = false;
                _mainpage.ShowError(ex, "OpenContactOrAccountCommand");
            }
        }

        private void SearchCommand()
        {
            try
            {

                _dispatcher.BeginInvoke(() =>
                {
                    ResetLinkToAccountOrContact();
                    _mainpage.dg1.ItemsSource = null;
                    _mainpage.dg2.ItemsSource = null;
                    IsWaiting = true;
                });

                if (IsExecutingInStandaloneSearchOrderArea)
                {
                    if (string.IsNullOrEmpty(OrderNumberText) == false || string.IsNullOrEmpty(CardNumber) == false)
                    {
                        _ordersClient.GetOrdersCompleted += OrdersClient_GetOrdersCompleted;
                        _ordersClient.GetOrdersAsync(null, OrderNumberText, CardNumber, null, null, null);
                    }
                    else
                    {
                        _dispatcher.BeginInvoke(() =>
                        {
                            IsWaiting = false;
                        });
                        _mainpage.ShowErrorMessage("Vänligen ange ett order nummer eller kortnummer!", "Information");
                    }
                }
                else
                {
                    // AJ: this seems to be wrong since earlier, can make a search on ordernb if email adress is missing
                    if (!string.IsNullOrEmpty(_emailAddress) || !string.IsNullOrEmpty(OrderNumberText) || !string.IsNullOrEmpty(CardNumber))
                    {
                        IsWaiting = true;

                        string _searchFrom = "";
                        try
                        {
                            DateTime _dtFrom;
                            DateTime.TryParse(_mainpage.dtTimeFrom.Text, out _dtFrom);
                            _searchFrom = _dtFrom.ToString();
                        }
                        catch
                        {
                            _searchFrom = DateTime.Now.ToShortDateString();
                            _mainpage.dtTimeFrom.Text = DateTime.Now.ToShortDateString();
                        }

                        string _searchTo = "";
                        try
                        {
                            DateTime _dtTo;
                            DateTime.TryParse(_mainpage.dtTimeTo.Text, out _dtTo);
                            string _tempdt = _dtTo.ToShortDateString() + "T23:59:59";
                            DateTime.TryParse(_tempdt, out _dtTo);
                            _searchTo = _dtTo.ToString();
                        }
                        catch
                        {
                            _searchFrom = DateTime.Now.ToShortDateString();
                            _mainpage.dtTimeFrom.Text = DateTime.Now.ToShortDateString();
                        }

                        _ordersClient.GetOrdersCompleted += OrdersClient_GetOrdersCompleted;
                        _ordersClient.GetOrdersAsync(_id, OrderNumberText, CardNumber, _searchFrom, _searchTo, _mainpage.txtEmail.Text);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(_emailAddress))
                        {
                            _mainpage.ShowErrorMessage("Vänligen ange en email adress!", "Information");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                IsWaiting = false;
                _mainpage.ShowError(ex, "SearchCommand");
            }
        }

        private void OrdersClient_GetOrdersCompleted(object sender, GetOrdersCompletedEventArgs e)
        {
            try
            {
                
                if (e.Result != null)
                {
                    GetOrderResponse _response = e.Result as GetOrderResponse;

                    if (!string.IsNullOrEmpty(_response.ErrorMessage))
                    {
                        IsWaiting = false;
                        _mainpage.ShowErrorMessage(_response.ErrorMessage, "_ordersClient_GetOrdersCompleted");
                        return;
                    }

                    if (_response.Orders != null)
                    {
                        _orderSearchResultCollection = new ObservableCollection<OrderHeader_ViewObject>();
                        _orderHeadCollection = _response.Orders;


                        if (IsExecutingInStandaloneSearchOrderArea == false/* && _creditOrderRows == null*/)
                            // AJ: Made by previous developer. This doesnt really work as its async and _savedCreditOrderRows is used below I would say.
                            GetCreditRows();

                        else if (IsExecutingInStandaloneSearchOrderArea == true /*&& _savedCreditOrderRows == null*/)
                        {
                            // INFO: We always fetch new/or reset credit rows.
                            /* INFO: This is for new standalone area, where credit rows are not loaded first when page loads, as they are in contact and account form.
                             * TODO: Project should have async/await functionality which would make this much more easier then async callbacks.
                            */

                            // TODO: we might get a nullref error here?
                            OrderHeader orderHeader = _orderHeadCollection.Where(x => x.Customer != null).FirstOrDefault(x => x.Customer.AccountNumber != null);

                            if (orderHeader != null)
                            {
                                string accountNumber = orderHeader.Customer.AccountNumber;
                                bool? isCompany = orderHeader.Customer.IsCompany;

                                if (isCompany != null)
                                {
                                    string customerName = orderHeader.Customer.Name;
                                    if (isCompany == true)
                                    {
                                        Id = accountNumber;
                                        Accountid = accountNumber;

                                        if (string.IsNullOrEmpty(customerName) == false)
                                        {
                                            SetLinkToAccountOrContact(Contactid, Accountid, customerName);
                                        }

                                        GetCreditRows();
                                    }
                                    else
                                    {
                                        Id = accountNumber;
                                        Contactid = accountNumber;

                                        if (string.IsNullOrEmpty(customerName) == false)
                                        {
                                            SetLinkToAccountOrContact(Contactid, Accountid, customerName);
                                        }

                                        GetCreditRows();
                                    }
                                }

                            }
                           
                        }
                        else
                        {
                            IsWaiting = false;
                        }

                        CreateGUIOrderRowsForOrderList1();
                    }
                    else
                    {
                        _dispatcher.BeginInvoke(() =>
                        {
                            _mainpage.dg1.ItemsSource = null;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_ordersClient_GetOrdersCompleted");
            }
            finally
            {
                IsWaiting = false;
                _ordersClient.GetOrdersCompleted -= OrdersClient_GetOrdersCompleted;
            }
        }

        private void SetLinkToAccountOrContact(string contactId, string accountId, string customerName)
        {
            string[] windowParams = new string[] { };
            if (string.IsNullOrEmpty(customerName) == false)
            {
                if (string.IsNullOrEmpty(contactId) == false || string.IsNullOrEmpty(accountId) == false)
                {
                    this.LinkToContactOrAccount = Utilities.CreateCustomerUri(Setting.CrmUri, contactId, accountId, windowParams);
                    this.LinkName = customerName;
                }
            }
        }

        private void ResetLinkToAccountOrContact()
        {
            this.LinkToContactOrAccount = null;
            this.LinkName = null;
        }

        private void CreateGUIOrderRowsForOrderList1()
        {

            foreach (OrderHeader orderHeader_DataTransferObject in _orderHeadCollection)
            {
                OrderHeader_ViewObject orderHeader_ViewObject = PopulateOrderHeaderViewObject(orderHeader_DataTransferObject);

                //check saved order
                if (_creditOrderRows != null)
                {
                    CreditOrderRow _savedorder = _creditOrderRows.FirstOrDefault(x => x.OrderNumber == orderHeader_DataTransferObject.OrderNumber);
                    if (_savedorder != null)
                        orderHeader_ViewObject.FontStyle = "X";
                    else
                        orderHeader_ViewObject.FontStyle = "";
                }

                _orderSearchResultCollection.Add(orderHeader_ViewObject);
            }

            _dispatcher.BeginInvoke(() =>
            {
                _mainpage.dg1.ItemsSource = null;
                _mainpage.dg1.ItemsSource = _orderSearchResultCollection.OrderBy(x => x.OrderNumber);
            });
        }

        /*
        private void OrdersClient_GetSavedCreditOrderRowsCompleted_In_ExecutingInStandAloneMode(object sender, GetSavedCreditOrderRowsCompletedEventArgs e)
        {
            try
            {
                CreateGUIOrderRowsForOrderList1();
            }
            finally
            {
                _ordersClient.GetSavedCreditOrderRowsCompleted -= OrdersClient_GetSavedCreditOrderRowsCompleted_In_ExecutingInStandAloneMode;
            }
        }
        */

        // AJ: orderheader contains OrderItems, those are displayed in child window.
        private OrderHeader_ViewObject PopulateOrderHeaderViewObject(OrderHeader order_DataTransferObject)
        {
            OrderHeader_ViewObject order_ViewObject = new OrderHeader_ViewObject();
            decimal notCreditableSum_credits_done = 0;
            decimal creditableSum_creditCard_payments = 0;

            try
            {
                if (order_DataTransferObject != null)
                {
                    //orderhead
                    PopulateOrderHeaderViewObject(order_DataTransferObject, order_ViewObject);

                    //payments
                    order_ViewObject.Payments = new ObservableCollection<Payment_ViewObject>();
                    PopulateOrderHeaderPaymentsViewObject(order_DataTransferObject, order_ViewObject);

                    //sum of all credits done so far
                    if (order_DataTransferObject.Payments != null && order_DataTransferObject.Payments.Count > 0)
                    {
                        notCreditableSum_credits_done = AccumulateNotCreditableSum_CreditsDone(order_DataTransferObject);
                    }

                    //shippingAddress
                    order_ViewObject.ShippingAddress = new ObservableCollection<ShippingAddress_ViewObject>();
                    PopulateOrderHeaderShippingAdressViewObject(order_DataTransferObject, order_ViewObject);

                    if (order_ViewObject != null && order_ViewObject.Payments != null && order_ViewObject.Payments.Count() > 0)
                    {
                        creditableSum_creditCard_payments = AccumulateCreditableSum_CreditCardPayments(order_ViewObject);

                        //Bugg #2297. This is a fix for it.
                        decimal amountThatCanBeCredit = 0;
                        amountThatCanBeCredit = creditableSum_creditCard_payments - notCreditableSum_credits_done;//order_ViewObject.AmountThatCanNotBeCredit;
                        if (amountThatCanBeCredit < 0)
                        {
                            amountThatCanBeCredit = 0;
                        }
                        order_ViewObject.AmountThatCanBeCredit = amountThatCanBeCredit;//creditableSum1;
                        // ******************************************************************************** 

                        //test
                        order_ViewObject.AmountThatCanNotBeCredit = order_ViewObject.OrderTotal - order_ViewObject.AmountThatCanBeCredit;
                    }

                    //orderitems
                    order_ViewObject.OrderItems = new ObservableCollection<OrderRow_ViewObject>();
                    PopulateOrderHeaderOrderRowsVIewObject(order_DataTransferObject, order_ViewObject);
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_createNewOrder");
            }
            return order_ViewObject;
        }

        private static void PopulateOrderHeaderOrderRowsVIewObject(OrderHeader order_DataTransferObject, OrderHeader_ViewObject order_ViewObject)
        {
            foreach (OrderRow orderRow_DataTransferObject in order_DataTransferObject.OrderItems)
            {
                // AJ : These seems to be the lines displayed in child window when you click a order row
                OrderRow_ViewObject orderRow_detailsView_ViewObject = new OrderRow_ViewObject();
                orderRow_detailsView_ViewObject.Code = orderRow_DataTransferObject.Code;
                orderRow_detailsView_ViewObject.Discount = orderRow_DataTransferObject.Discount;
                orderRow_detailsView_ViewObject.Name = orderRow_DataTransferObject.Name;
                orderRow_detailsView_ViewObject.Price = orderRow_DataTransferObject.Price;
                orderRow_detailsView_ViewObject.Quantity = orderRow_DataTransferObject.Quantity;
                orderRow_detailsView_ViewObject.CalculateRowPrice();
                order_ViewObject.OrderItems.Add(orderRow_detailsView_ViewObject);
            }
        }

        private static decimal AccumulateCreditableSum_CreditCardPayments(OrderHeader_ViewObject order_ViewObject)
        {
            decimal creditableSum_creditCard_payments = 0;

            string[] paymentMethodCodes = new string[] { "DIBSDIRECTNORDEA", "DIBSDIRECTHANDELSBANKEN", "DIBSDIRECTSWEDBANK", "DIBSDIRECTSEB", "DIBSCREDITCARDAUTH" };

            IEnumerable<Payment_ViewObject> _valueList2 = order_ViewObject.Payments.
                Where(x => x.Code != null).
                Where(x => x.TransactionType != null).
                Where(x => x.TransactionType.ToUpper() == "DRAGNING").
                Where(x => paymentMethodCodes.Contains(x.Code.ToUpper()));

            foreach (Payment_ViewObject _list in _valueList2)
            {
                creditableSum_creditCard_payments += _list.Sum;
            }
            return creditableSum_creditCard_payments;
        }

        private static void PopulateOrderHeaderShippingAdressViewObject(OrderHeader order_DataTransferObject, OrderHeader_ViewObject order_ViewObject)
        {
            foreach (ShippingAddress shippingAddress_DataTransferObject in order_DataTransferObject.ShippingAddress)
            {
                ShippingAddress_ViewObject shippingAddress_ViewObject = new ShippingAddress_ViewObject();
                shippingAddress_ViewObject.Address = shippingAddress_DataTransferObject.Address;
                shippingAddress_ViewObject.CellPhoneNumber = shippingAddress_DataTransferObject.CellPhoneNumber;
                shippingAddress_ViewObject.City = shippingAddress_DataTransferObject.City;
                shippingAddress_ViewObject.Co = shippingAddress_DataTransferObject.Co;
                shippingAddress_ViewObject.CompanyName = shippingAddress_DataTransferObject.CompanyName;
                shippingAddress_ViewObject.Country = shippingAddress_DataTransferObject.Country;
                shippingAddress_ViewObject.Email = shippingAddress_DataTransferObject.Email;
                shippingAddress_ViewObject.ExtraInfo = shippingAddress_DataTransferObject.ExtraInfo;
                shippingAddress_ViewObject.FirstName = shippingAddress_DataTransferObject.FirstName;
                shippingAddress_ViewObject.LastName = shippingAddress_DataTransferObject.LastName;
                shippingAddress_ViewObject.PostalCode = shippingAddress_DataTransferObject.PostalCode;

                order_ViewObject.ShippingAddress.Add(shippingAddress_ViewObject);
            }
        }

        private static decimal AccumulateNotCreditableSum_CreditsDone(OrderHeader order_DataTransferObject)
        {
            decimal notCreditableSum_credits_done = 0;

            IEnumerable<Payment> query = order_DataTransferObject.Payments.
                Where(x => x.TransactionType != null).
                Where(x => x.TransactionType.ToUpper() == "KREDITERING");

            foreach (Payment payment_DataTransferObject in query)
            {
                notCreditableSum_credits_done += payment_DataTransferObject.Sum;
            }
            return notCreditableSum_credits_done;
        }

        private static void PopulateOrderHeaderPaymentsViewObject(OrderHeader order_DataTransferObject, OrderHeader_ViewObject order_ViewObject)
        {
            foreach (Payment payment_DataTransferObject in order_DataTransferObject.Payments)
            {
                Payment_ViewObject payment_ViewObject = new Payment_ViewObject();
                payment_ViewObject.Code = payment_DataTransferObject.Code;
                payment_ViewObject.Name = payment_DataTransferObject.Name;
                payment_ViewObject.ReferenceNumber = payment_DataTransferObject.ReferenceNumber;
                payment_ViewObject.Sum = payment_DataTransferObject.Sum;
                payment_ViewObject.TransactionType = payment_DataTransferObject.TransactionType;
                payment_ViewObject.TransactionId = payment_DataTransferObject.TransactionId;
                payment_ViewObject.GiftCardCode = payment_DataTransferObject.GiftCardCode;
                order_ViewObject.Payments.Add(payment_ViewObject);
            }
        }

        private static void PopulateOrderHeaderViewObject(OrderHeader order_DataTransferObject, OrderHeader_ViewObject order_ViewObject)
        {
            order_ViewObject.OrderDate = order_DataTransferObject.OrderDate;
            order_ViewObject.OrderNumber = order_DataTransferObject.OrderNumber;
            order_ViewObject.OrderStatus = order_DataTransferObject.OrderStatus;
            order_ViewObject.OrderTotal = order_DataTransferObject.OrderTotal;
            order_ViewObject.OrderTotalVat = order_DataTransferObject.OrderTotalVat;
            order_ViewObject.OrderType = order_DataTransferObject.OrderType;
        }


        #endregion

        #region form properties


        public string OrderNumberText { get; set; }

        private ObservableCollection<CustomerOrder> _pOrders;
        public ObservableCollection<CustomerOrder> Orders
        {
            get { return _pOrders; }
            set
            {
                if (_pOrders != value)
                {
                    _pOrders = value;
                    OnPropertyChanged("Orders");
                }
            }
        }

        private ObservableCollection<CustomerOrder> _pOrderRows;
        private ObservableCollection<OrderHeader> _orderHeadCollection;

        public ObservableCollection<CustomerOrder> OrderRows
        {
            get { return _pOrderRows; }
            set
            {
                if (_pOrderRows != value)
                {
                    _pOrderRows = value;
                    OnPropertyChanged("OrderRows");
                }
            }
        }

        #endregion

        private void GetCreditRows()
        {

            try
            {
                if (_ordersClient != null)
                {

                    if (!string.IsNullOrEmpty(_accountid) || !string.IsNullOrEmpty(_contactid))
                    {
                        _dispatcher.BeginInvoke(() => { IsWaiting = true; });

                        _ordersClient.GetSavedCreditOrderRowsCompleted += OrdersClient_GetCreditOrderRowsCompleted;

                        _ordersClient.GetSavedCreditOrderRowsAsync(_accountid, _contactid);
                    }
                    else
                    {
                        _dispatcher.BeginInvoke(() => { IsWaiting = false; });

                        // INFO: in standalone mode, new search can result in new customer, we have to reset credit rows.
                        if (IsExecutingInStandaloneSearchOrderArea)
                        {
                            // AJ: TODO all lines like this one should work with the view object list instead
                            _dispatcher.BeginInvoke(() => { _mainpage.dg2.ItemsSource = null; });
                        }
                    }
                }
                else
                {

                    _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                }
            }
            catch (Exception ex)
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_getSavedCreditRows");
            }
        }

        private void GetCreditRows(EventHandler<GetSavedCreditOrderRowsCompletedEventArgs> eventhandler)
        {
            try
            {
                if (_ordersClient != null)
                {
                    _dispatcher.BeginInvoke(() => { IsWaiting = true; });
                    _ordersClient.GetSavedCreditOrderRowsCompleted += OrdersClient_GetCreditOrderRowsCompleted;
                    _ordersClient.GetSavedCreditOrderRowsAsync(_accountid, _contactid);
                }
                else
                {
                    _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                }
            }
            catch (Exception ex)
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_getSavedCreditRows");
            }
        }

        private void OrdersClient_GetCreditOrderRowsCompleted(object sender, GetSavedCreditOrderRowsCompletedEventArgs e)
        {
            try
            {
                _dispatcher.BeginInvoke(() => { _mainpage.dg2.ItemsSource = null; });

                _creditOrderRows = new ObservableCollection<CreditOrderRow>();
                _creditOrderRow_ViewObjects = new ObservableCollection<CreditOrderRow_ViewObject>();

                if (e.Result != null)
                {
                    SavedCreditOrderRowsResponse response = e.Result as SavedCreditOrderRowsResponse;

                    if (!string.IsNullOrEmpty(response.ErrorMessage))
                    {
                        _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                        _mainpage.ShowErrorMessage(response.ErrorMessage, "_ordersClient_GetSavedCreditOrderRowsCompleted 1");
                        return;
                    }

                    if (response.OrderList != null && response.OrderList.Count() > 0)
                    {
                        _creditOrderRows = response.OrderList;

                        foreach (CreditOrderRow creditOrderRow in _creditOrderRows)
                        {
                            CreditOrderRow_ViewObject creditOrderRow_ViewObject = PopulateCreditOrderRow_ViewObject(creditOrderRow);
                            _creditOrderRow_ViewObjects.Add(creditOrderRow_ViewObject);

                            if (_orderSearchResultCollection != null && _orderSearchResultCollection.Count() > 0)
                            {

                                OrderHeader_ViewObject orderHeader_ViewObjects = _orderSearchResultCollection.FirstOrDefault(x => x.OrderNumber == creditOrderRow.OrderNumber);
                                if (orderHeader_ViewObjects != null)
                                    orderHeader_ViewObjects.FontStyle = "X";
                                /*else
                                    _o.FontStyle = " ";*/
                            }
                        }

                        _dispatcher.BeginInvoke(() =>
                        {
                            _mainpage.dg2.ItemsSource = null;
                            
                            IEnumerable<CreditOrderRow_ViewObject> filteredAndOrderedCreditOrderRow_ViewObjects = new List<CreditOrderRow_ViewObject>();
                            filteredAndOrderedCreditOrderRow_ViewObjects = FilterAndOrderCreditOrderRow_ViewObjects();
                            
                            _mainpage.dg2.ItemsSource = filteredAndOrderedCreditOrderRow_ViewObjects; //_creditOrderRow_ViewObjects.Where(x => x.OrderNumber == OrderNumberText);
                        });
                    }
                    else
                    {
                        _dispatcher.BeginInvoke(() =>
                        {
                            _mainpage.dg2.ItemsSource = null;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_ordersClient_GetSavedCreditOrderRowsCompleted 2");
            }
            finally
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _ordersClient.GetSavedCreditOrderRowsCompleted -= OrdersClient_GetCreditOrderRowsCompleted;
            }
        }

        private IEnumerable<CreditOrderRow_ViewObject> FilterAndOrderCreditOrderRow_ViewObjects()
        {
            IEnumerable<CreditOrderRow_ViewObject> filteredCreditOrderRow_ViewObjects;
            filteredCreditOrderRow_ViewObjects = _creditOrderRow_ViewObjects;
            if (string.IsNullOrEmpty(OrderNumberText) == false)
            {
                filteredCreditOrderRow_ViewObjects = filteredCreditOrderRow_ViewObjects.Where(x => x.OrderNumber.ToUpper() == OrderNumberText.ToUpper());//.OrderByDescending(x => x.OrderNumber); 
            }
            if (string.IsNullOrEmpty(CardNumber) == false)
            {
                IEnumerable<string> uniqueOrderNumbers = _orderSearchResultCollection.Select(x => x.OrderNumber.ToUpper()).Distinct();
                filteredCreditOrderRow_ViewObjects = filteredCreditOrderRow_ViewObjects.Where(x => uniqueOrderNumbers.Contains(x.OrderNumber.ToUpper()));//.OrderByDescending(x => x.OrderNumber); 
            }
            return filteredCreditOrderRow_ViewObjects.OrderByDescending(x => x.OrderNumber); 
        }

        private static CreditOrderRow_ViewObject PopulateCreditOrderRow_ViewObject(CreditOrderRow creditOrderRow)
        {
            CreditOrderRow_ViewObject creditOrderRow_ViewObject = new CreditOrderRow_ViewObject();
            creditOrderRow_ViewObject.Row = "";
            creditOrderRow_ViewObject.OrderNumber = creditOrderRow.OrderNumber;
            creditOrderRow_ViewObject.Date = creditOrderRow.Date;
            creditOrderRow_ViewObject.Time = creditOrderRow.Time;
            creditOrderRow_ViewObject.Product = creditOrderRow.Productnumber;
            creditOrderRow_ViewObject.ReferenceNumber = creditOrderRow.ReferenceNumber;
            creditOrderRow_ViewObject.CreatedBy = creditOrderRow.CreatedBy;
            creditOrderRow_ViewObject.Reason = creditOrderRow.Reason;
            creditOrderRow_ViewObject.Sum = creditOrderRow.Sum;
            return creditOrderRow_ViewObject;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void GetCreditRows_OnlyFromAccountOrContactForm()
        {
            try
            {
                GetObjectID();
                _dispatcher.BeginInvoke(() => { IsWaiting = true; });
                GetCreditRows();
            }
            catch (Exception ex)
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "GetSavedOrderRows");
            }
        }

        private void GetObjectID()
        {
            try
            {
                _dispatcher.BeginInvoke(() =>
                {
                    if (IsExecutingInStandaloneSearchOrderArea)
                    {
                        // TODO anything?
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(_debug) && _formTypeName.ToUpper() == "ACCOUNT")
                        {
                            string _id = HtmlPage.Window.Invoke("GetCustomerID") as string;
                            _id = RemoveBracketsFromString(_id);
                            Accountid = _id;
                        }

                        if (string.IsNullOrEmpty(_debug) && _formTypeName.ToUpper() == "CONTACT")
                        {
                            string _id = HtmlPage.Window.Invoke("GetCustomerID") as string;
                            _id = RemoveBracketsFromString(_id);
                            Contactid = _id;

                        }

                        if (string.IsNullOrEmpty(_debug))
                        {
                            string obj = HtmlPage.Window.Invoke("GetEmailAddress") as string;
                            Emailaddress = obj;
                            if (_mainpage.txtEmail != null)
                                _mainpage.txtEmail.Text = Emailaddress != null ? Emailaddress : String.Empty;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_getObjectID");
            }
        }

        private string RemoveBracketsFromString(string value)
        {
            value = value.Replace("{", "");
             value = value.Replace("}", "");
             return value;
        }

        /*
        private void _checkIfUserCanEditOrder()
        {
            try
            {
                _crmManager.Fetch<sequrityroles>(_xmlcheckIfUserCanEditOrder(), _checkIfUserCanEditOrder_callback);
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_checkIfUserCanEditOrder");
            }
        }
        */

        /*
        private void _checkIfUserCanEditOrder_callback(ObservableCollection<sequrityroles> result)
        {
            try
            {
                if (result != null && result.Count() > 0)
                {
                    _canEditOrder = true;
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_checkIfUserCanEditOrder_callback");
            }
        }
        */

        /*
        private string _xmlcheckIfUserCanEditOrder()
        {
            string _xml = "";

            _xml += "<fetch version='1.0' mapping='logical' distinct='true'>";
            _xml += "<entity name='role'>";
            _xml += "<attribute name='name' />";
            _xml += "<attribute name='businessunitid' />";
            _xml += "<attribute name='roleid' />";
            _xml += "<filter type='and'>";
            _xml += "<condition attribute='name' operator='eq' value='Skånetrafiken Kreditera Order' />";
            _xml += "</filter>";
            _xml += "<link-entity name='systemuserroles' from='roleid' to='roleid' visible='false' intersect='true'>";
            _xml += "<link-entity name='systemuser' from='systemuserid' to='systemuserid' alias='ad'>";
            _xml += "<filter type='and'>";
            _xml += "<condition attribute='systemuserid' operator='eq-userid' />";
            _xml += "</filter>";
            _xml += "</link-entity>";
            _xml += "</link-entity>";
            _xml += "</entity>";
            _xml += "</fetch>";

            return _xml;
        }
        */
    }

    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string _returnValue = "";

            if (value != null)
            {
                if (value.ToString() == "0")
                {
                    _returnValue = "0,00";
                }
                else
                {
                    string _svalue = value.ToString();
                    decimal _dvalue = 0.0M;
                    decimal.TryParse(_svalue, out _dvalue);
                    _returnValue = _dvalue.ToString("0.00");
                }
            }

            return _returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    // INFO: generic with true/false param possible instead of two converters

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as bool? == true) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
                return (Visibility)value == Visibility.Visible;
            else
                return false;
        }
    }

    public class ReversedBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as bool? == false) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
                return (Visibility)value != Visibility.Visible;
            else
                return true;
        }
    }

    public class Log
    {
        public static void Write(object message/*, params object[] values*/)
        {
            HtmlWindow window = HtmlPage.Window;
            var isConsoleAvailable = (bool)window.Eval("typeof(console) != 'undefined' && typeof(console.log) != 'undefined'");
            if (isConsoleAvailable)
            {
                var createLogFunction = (bool)window.Eval("typeof(sllog) == 'undefined'");
                if (createLogFunction)
                {
                    // Load the logging function into global scope:
                    string logFunction = "function sllog(msg) { console.log(msg); }";
                    string code = string.Format(@"if(window.execScript) {{ window.execScript('{0}'); }} else {{ eval.call(null, '{0}'); }}", logFunction);
                    window.Eval(code);
                }

                // Prepare the message
                DateTime dateTime = DateTime.Now;
                string output = string.Format("{0} - {1}", dateTime.ToString("u"), string.Format(message.ToString()/*, values*/));

                // Invoke the logging function:
                var logger = window.Eval("sllog") as ScriptObject;
                if (logger != null)
                {
                    // Workaround: Cannot call InvokeSelf outside of UI thread, without dispatcher
                    Dispatcher d = Deployment.Current.Dispatcher;
                    if (!d.CheckAccess())
                    {
                        d.BeginInvoke((System.Threading.ThreadStart)(() => logger.InvokeSelf(output)));
                    }
                    else
                    {
                        logger.InvokeSelf(output);
                    }
                }
            }
        }
    }

}




