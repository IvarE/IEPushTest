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

        //internal ObservableCollection<Language> _localized;

        ObservableCollection<orderheader> _orderSearchResultCollection;

        private WebParameters _webParameters;
        private CrmManager _crmManager;
        private Dispatcher _dispatcher;
        private MainPage _mainpage;
        //private GetOrdersServiceClient _serviceclient;

        private string _querystring;
        private string _initparams;
        //private string _data;

        private ObservableCollection<CreditOrderRow> _savedCreditOrderRows;
        private ObservableCollection<savedOrderRow> _showRows;
        private ObservableCollection<Language> _language;

        private bool _canEditOrder = false;

        // =========================================================================    

        private bool _isWaiting;
        public bool IsWaiting
        {
            get { return _isWaiting; }
            set { _isWaiting = value; OnPropertyChanged("IsWaiting"); }
        }

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
            set { _accountid = value; }
        }

        private string _contactid;
        public string Contactid
        {
            get { return _contactid; }
            set { _contactid = value; }
        }

        private string _emailAddress;
        public string Emailaddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        private string _debug;
        public string Debug
        {
            get { return _debug; }
            set { _debug = value; }
        }

        public string _userFullName { get; set; }

        #region constructor

        public MainPageViewModel(WebParameters webParameters, CrmManager crmManager, Dispatcher dispatcher, MainPage mainpage, string querystring, string initparams)
        {
            try
            {

                _canEditOrder = false;
                IsWaiting = true;
                _querystring = querystring;
                _initparams = initparams;

                ButtonSearchCommand = new RelayCommand(SearchCommand, true);

                _webParameters = webParameters;
                _crmManager = crmManager;
                _dispatcher = dispatcher;
                _mainpage = mainpage;

                _debug = _getKey(_querystring, "debug");
                //_data = _getKey(_querystring, "data");
                _id = _getKey(_querystring, "id");
                _id = _id.Replace("{", string.Empty).Replace("}", string.Empty);

                _typename = _getKey(_querystring, "typename");
                _emailAddress = _getKey(_querystring, "email");

                // TODO: in HTML we pass a null, that could be checked otherwise that should be refactored.
                if (string.IsNullOrEmpty(_debug) && string.IsNullOrEmpty(_typename))
                {
                    IsExecutingInStandaloneSearchOrderArea = true;

                }
                else
                {
                    IsExecutingInStandaloneSearchOrderArea = false;

                }

                if (!string.IsNullOrEmpty(_debug) && _typename.ToUpper() == "ACCOUNT")
                {
                    _accountid = _id;
                    _contactid = "";
                    _userFullName = "DKTESTARE DKTESTSSON";
                }

                if (!string.IsNullOrEmpty(_debug) && _typename.ToUpper() == "CONTACT")
                {
                    _accountid = "";
                    _contactid = _id;
                    _userFullName = "DKTESTARE DKTESTSSON";
                }

                if (_typename.ToUpper() == "ACCOUNT")
                {
                    _accountid = _id;
                    _contactid = "";
                }

                if (_typename.ToUpper() == "CONTACT")
                {
                    _accountid = "";
                    _contactid = _id;
                }
                //_mainpage.ShowErrorMessage(_contactid + " * " + _id + " * " + _data.ToUpper(), "id contact test");

                _mainpage.DataContext = this;

                // INFO: In standalonemode sometimes the initial waiting popup loads at start
                // and never goes away. This stops that. 
                // UPDATE: proberbly not needed since _ordersClient_GetOrdersCompleted had it finally IsWaiting uncommented.
                // Needs to be tested though.
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });

                _initOrderClient();
                _checkIfUserCanEditOrder();

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
                    string _date = _checkFormatOfDate(_mainpage.dtTimeTo.Text);
                    DateTime _dt;
                    bool _ok = DateTime.TryParse(_date, out _dt);
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
                    string _date = _checkFormatOfDate(_mainpage.dtTimeFrom.Text);
                    DateTime _dt;
                    bool _ok = DateTime.TryParse(_date, out _dt);
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

        private string _checkFormatOfDate(string input)
        {
            string _returnValue = "";

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
                    _returnValue = string.Format("{0}-{1}-{2}", _year, _month, _day);
                }

                if (input.Length == 8 && input.IndexOf("-") < 0)
                {
                    string _year = input.Substring(0, 4);
                    string _month = input.Substring(4, 2);
                    string _day = input.Substring(6, 2);
                    _returnValue = string.Format("{0}-{1}-{2}", _year, _month, _day);
                }

                if (input.Length == 8 && input.IndexOf("-") >= 0)
                {
                    string _year = input.Substring(0, 2);
                    string _month = input.Substring(3, 2);
                    string _day = input.Substring(6, 2);
                    _returnValue = string.Format("{0}-{1}-{2}", _year, _month, _day);
                }

                if (input.Length == 10 && input.IndexOf("-") >= 0)
                {
                    string _year = input.Substring(0, 4);
                    string _month = input.Substring(5, 2);
                    string _day = input.Substring(8, 2);
                    _returnValue = string.Format("{0}-{1}-{2}", _year, _month, _day);
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_checkFormatOfDate");
            }

            return _returnValue;
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
                    orderheader _o = row.DataContext as orderheader;
                    if (_o != null)
                    {
                        OrderRowPage _window = new OrderRowPage(_mainpage, _o, _ordersClient, _accountid, _contactid, _savedCreditOrderRows, _language);
                        _window.Closed += _window_Closed;
                        _window.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "dg1_MouseLeftButtonUp");
            }
        }

        private void _window_Closed(object sender, EventArgs e)
        {
            try
            {
                IsWaiting = true;
                if (IsExecutingInStandaloneSearchOrderArea)
                {
                    _getCreditRows();
                }
                else
                {
                    // do we not need to also renew the data in order rows in dg1?
                    _getCreditRows();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_window_Closed");
            }
        }

        #endregion constructor

        private string _getKey(string keys, string key)
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

        private void _refreshDataContext()
        {
            _dispatcher.BeginInvoke(() =>
            {
                _mainpage.DataContext = null;
                _mainpage.DataContext = this;
            });
        }

        private void _initOrderClient()
        {
            try
            {
                _crmManager.Fetch<setting>(_xmlinitOrderClient(), _initOrderClient_callback);
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_initOrderClient");
            }

        }

        private void _initOrderClient_callback(ObservableCollection<setting> result)
        {

            try
            {

                if (result != null)
                {

                    if (result.Count() > 0)
                    {

                        setting _setting = result[0] as setting;
                        string _address = _setting.ServiceAddress;
                        _createOrderClient(_address);
                    }
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_initOrderClient_callback");
            }
        }

        private string _xmlinitOrderClient()
        {
            string _xml = "";

            string _now = DateTime.Now.ToString("s");
            _xml += "<fetch distinct='false' mapping='logical' version='1.0'>";
            _xml += "<entity name='cgi_setting'>";
            _xml += "<attribute name='cgi_ehandelorderservice'/>";
            _xml += "<filter type='and'>";
            _xml += "<condition attribute='statecode' value='0' operator='eq'/>";
            _xml += "<condition attribute='cgi_validfrom' value='" + _now + "' operator='on-or-before'/>";
            _xml += "<filter type='or'>";
            _xml += "<condition attribute='cgi_validto' value='" + _now + "' operator='on-or-after'/>";
            _xml += "<condition attribute='cgi_validto' operator='null'/>";
            _xml += "</filter>";
            _xml += "</filter>";
            _xml += "</entity>";
            _xml += "</fetch>";

            return _xml;
        }

        private void _createOrderClient(string serviceaddress)
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
                //_mainpage.ShowErrorMessage("_initOrderClient COMPLETED"+DateTime.Now.ToString("hh:mm:ss.fff tt"),"test");
                _initTranslation(); // TODO: move out
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_createOrderClient");
            }
        }

        private void _initTranslation()
        {
            try
            {
                _crmManager.Fetch<Language>(_xmlinitTranslation(), _initTranslation_callback);
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_initTranslation");
            }
        }

        private void _initTranslation_callback(ObservableCollection<Language> result)
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
                        _mainpage.dg1.Columns[5].Header = GetTranslation("grd1Code", ".error.", result);
                        _mainpage.dg1.Columns[6].Header = "Ej krediterbart";  // GetTranslation("grd1OtherPayment", ".error.", result);
                        _mainpage.dg1.Columns[7].Header = "Kan krediteras";   // GetTranslation("grd1OtherPayment", ".error.", result);
                        _mainpage.dg1.Columns[8].Header = GetTranslation("grd1OrderStatus", ".error.", result);
                        _mainpage.dg1.Columns[9].Header = GetTranslation("grd1OrderType", ".error.", result);

                        _mainpage.dg2.Columns[1].Header = GetTranslation("grd2OrderNumber", ".error.", result);
                        _mainpage.dg2.Columns[2].Header = GetTranslation("grd2Date", ".error.", result);
                        _mainpage.dg2.Columns[3].Header = GetTranslation("grd2Time", ".error.", result);
                        _mainpage.dg2.Columns[4].Header = GetTranslation("grd2Product", ".error.", result);
                        _mainpage.dg2.Columns[5].Header = GetTranslation("grd2ReferenceNr", ".error.", result);
                        _mainpage.dg2.Columns[6].Header = "Skapad av";//;GetTranslation("grd2CreatedBy", ".error.", result);
                        _mainpage.dg2.Columns[7].Header = GetTranslation("grd2Sum", ".error.", result);

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
                            _getCreditRows();
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

        private string _xmlinitTranslation()
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

        #endregion

        #region Command functions

        private void SearchCommand()
        {
            try
            {
                _dispatcher.BeginInvoke(() =>
                {
                    _mainpage.dg1.ItemsSource = null;
                    IsWaiting = true;
                });

                if (IsExecutingInStandaloneSearchOrderArea)
                {
                    if (string.IsNullOrEmpty(OrderNumberText) == false)
                    {
                        _ordersClient.GetOrdersCompleted += _ordersClient_GetOrdersCompleted;
                        _ordersClient.GetOrdersAsync(null, OrderNumberText, null, null, null);
                    }
                    else
                    {
                        _dispatcher.BeginInvoke(() =>
                        {
                            IsWaiting = false;
                        });
                        _mainpage.ShowErrorMessage("Vänligen ange ett order nummer!", "Information");
                    }
                }
                else
                {
                    // AJ: this seems to be wrong since earlier, can make a search on ordernb if email adress is missing
                    if (!string.IsNullOrEmpty(_emailAddress) || !string.IsNullOrEmpty(OrderNumberText))
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

                        _ordersClient.GetOrdersCompleted += _ordersClient_GetOrdersCompleted;
                        _ordersClient.GetOrdersAsync(_id, OrderNumberText, _searchFrom, _searchTo, _mainpage.txtEmail.Text);
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

        private void _ordersClient_GetOrdersCompleted(object sender, GetOrdersCompletedEventArgs e)
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
                        _orderSearchResultCollection = new ObservableCollection<orderheader>();
                        /*ObservableCollection<OrderHeader>*/
                        _orderHeadCollection = _response.Orders;


                        if (IsExecutingInStandaloneSearchOrderArea == false && _savedCreditOrderRows == null)
                            // AJ: Made by previous developer. This doesnt really work as its async and _savedCreditOrderRows is used below I would say.
                            _getCreditRows();

                        else if (IsExecutingInStandaloneSearchOrderArea == true /*&& _savedCreditOrderRows == null*/)
                        {
                            // INFO: We always fetch new/or reset credit rows.
                            /* INFO: This is for new standalone area, where credit rows are not loaded first when page loads, as they are in contact and account form.
                             * TODO: Project should have async/await functionality which would make this much more easier then async callbacks.
                            */

                            //if (IsExecutingInStandaloneSearchOrderArea == true)
                            //{
                            OrderHeader orderHeader = _orderHeadCollection.FirstOrDefault(x => x.Customer.AccountNumber != null);

                            if (orderHeader != null)
                            {

                                string accountNumber = orderHeader.Customer.AccountNumber;
                                bool? isCompany = orderHeader.Customer.IsCompany;
                                if (isCompany != null)
                                {

                                    if (isCompany == true)
                                    {

                                        //Guid accountNumberGuid;
                                        //if (Guid.TryParse(accountNumber, out accountNumberGuid))
                                        //{
                                        _id = accountNumber;
                                        _accountid = accountNumber;

                                        _getCreditRows(/*_ordersClient_GetSavedCreditOrderRowsCompleted_In_ExecutingInStandAloneMode/*, _ohlist*/);
                                        //return;
                                        //}
                                    }
                                    else
                                    {

                                        _id = accountNumber;
                                        _contactid = accountNumber;

                                        _getCreditRows(/*_ordersClient_GetSavedCreditOrderRowsCompleted_In_ExecutingInStandAloneMode/*, _ohlist*/);
                                        //return;
                                    }
                                }

                            }
                            //}
                        }
                        else
                        {

                            IsWaiting = false;

                        }

                        CreateGUIOrderRowsForOrderList1(/*_ohlist*/);




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
                _ordersClient.GetOrdersCompleted -= _ordersClient_GetOrdersCompleted;
            }
        }

        private void CreateGUIOrderRowsForOrderList1(/*ObservableCollection<OrderHeader> _ohlist*/)
        {

            foreach (OrderHeader orderHeader_DataTransferObject in _orderHeadCollection)
            {
                orderheader orderHeader_ViewObject = _createNewOrder(orderHeader_DataTransferObject);

                //check saved order
                if (_savedCreditOrderRows != null)
                {
                    CreditOrderRow _savedorder = _savedCreditOrderRows.FirstOrDefault(x => x.OrderNumber == orderHeader_DataTransferObject.OrderNumber);
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

        private void _ordersClient_GetSavedCreditOrderRowsCompleted_In_ExecutingInStandAloneMode(object sender, GetSavedCreditOrderRowsCompletedEventArgs e/*, ObservableCollection<OrderHeader> _ohlist*/)
        {
            try
            {
                CreateGUIOrderRowsForOrderList1(/*_ohlist*/);
            }
            finally
            {
                _ordersClient.GetSavedCreditOrderRowsCompleted -= _ordersClient_GetSavedCreditOrderRowsCompleted_In_ExecutingInStandAloneMode;
            }
        }

        // AJ: orderheader contains OrderItems, those are displayed in child window.
        private orderheader _createNewOrder(OrderHeader order_DataTransferObject)
        {
            orderheader order_ViewObject = new orderheader();

            try
            {
                if (order_DataTransferObject != null)
                {

                    //orderhead
                    order_ViewObject.OrderDate = order_DataTransferObject.OrderDate;
                    order_ViewObject.OrderNumber = order_DataTransferObject.OrderNumber;
                    order_ViewObject.OrderStatus = order_DataTransferObject.OrderStatus;
                    order_ViewObject.OrderTotal = order_DataTransferObject.OrderTotal;
                    order_ViewObject.OrderTotalVat = order_DataTransferObject.OrderTotalVat;
                    order_ViewObject.OrderType = order_DataTransferObject.OrderType;
                    order_ViewObject.ValueCodeAmount = 0;
                    order_ViewObject.OtherAmount = 0;

                    //payments
                    order_ViewObject.Payments = new ObservableCollection<payment>();
                    foreach (Payment payment_DataTransferObject in order_DataTransferObject.Payments)
                    {
                        payment payment_ViewObject = new payment();
                        payment_ViewObject.Code = payment_DataTransferObject.Code;
                        payment_ViewObject.Name = payment_DataTransferObject.Name;
                        payment_ViewObject.ReferenceNumber = payment_DataTransferObject.ReferenceNumber;
                        payment_ViewObject.Sum = payment_DataTransferObject.Sum;
                        payment_ViewObject.TransactionType = payment_DataTransferObject.TransactionType;
                        payment_ViewObject.TransactionId = payment_DataTransferObject.TransactionId;
                        payment_ViewObject.GiftCardCode = payment_DataTransferObject.GiftCardCode;
                        order_ViewObject.Payments.Add(payment_ViewObject);
                    }

                    decimal notCreditableSum1 = 0;
                    decimal notCreditableSum2 = 0;
                    decimal notCreditableSum3 = 0;
                    decimal creditableSum1 = 0;

                    //credits
                    if (order_DataTransferObject.Payments != null && order_DataTransferObject.Payments.Count > 0)
                    {
                        IEnumerable<Payment> query = order_DataTransferObject.Payments.
                            Where(x => x.TransactionType != null).
                            Where(x => x.TransactionType.ToUpper() == "KREDITERING");

                        foreach (Payment payment_DataTransferObject in query)
                        {
                            notCreditableSum2 += payment_DataTransferObject.Sum;
                        }
                    }

                    //ShippingAddress
                    order_ViewObject.ShippingAddress = new ObservableCollection<ShippingAddress_ViewObject>();

                    foreach (ShippingAddress sa in order_DataTransferObject.ShippingAddress)
                    {
                        ShippingAddress_ViewObject shippingAddress_ViewObject = new ShippingAddress_ViewObject();
                        shippingAddress_ViewObject.Address = sa.Address;
                        shippingAddress_ViewObject.CellPhoneNumber = sa.CellPhoneNumber;
                        shippingAddress_ViewObject.City = sa.City;
                        shippingAddress_ViewObject.Co = sa.Co;
                        shippingAddress_ViewObject.CompanyName = sa.CompanyName;
                        shippingAddress_ViewObject.Country = sa.Country;
                        shippingAddress_ViewObject.Email = sa.Email;
                        shippingAddress_ViewObject.ExtraInfo = sa.ExtraInfo;
                        shippingAddress_ViewObject.FirstName = sa.FirstName;
                        shippingAddress_ViewObject.LastName = sa.LastName;
                        shippingAddress_ViewObject.PostalCode = sa.PostalCode;

                        order_ViewObject.ShippingAddress.Add(shippingAddress_ViewObject);
                    }

                    if (order_ViewObject != null && order_ViewObject.Payments != null && order_ViewObject.Payments.Count() > 0)
                    {
                        // Can not creadit this amount.
                        IEnumerable<payment> notCreditableTypes1 = order_ViewObject.Payments.Where(x =>
                            //x.Name.ToUpper() == "VÄRDEKOD - CAPTURE - PROCESSED"
                            x.Name.ToUpper() == "VÄRDEKOD"
                            );
                        foreach (payment _list in notCreditableTypes1)
                        {
                            notCreditableSum1 += _list.Sum;
                        }

                        // Can not creadit this amount.
                        IEnumerable<payment> notCreditableTypes2 = order_ViewObject.Payments.Where(x =>
                            //x.Name.ToUpper() == "KREDITKORT - AUTHORIZATION - PROCESSED" ||
                            //x.Name.ToUpper() == "ERSÄTTNING - CAPTURE - PROCESSED" ||
                            x.Name.ToUpper() == "ERSÄTTNING" ||
                                //x.Name.ToUpper() == "PRESENTKORT - AUTHORIZATION - PROCESSED" ||
                                //x.Name.ToUpper() == "PRESENTKORT - CAPTURE - PROCESSED" ||
                            x.Name.ToUpper() == "PRESENTKORT" ||
                                //x.Name.ToUpper() == "FAKTURA - CAPTURE - PROCESSED"
                            x.Name.ToUpper() == "FAKTURA"
                            );

                        foreach (payment _list in notCreditableTypes2)
                        {
                            notCreditableSum3 += _list.Sum;
                        }

                        /*
                        // Can creadit this amount.
                        IEnumerable<payment> _valueList2 = _newOrder.Payments.Where(x =>
                            //x.Name.ToUpper() == "KREDITKORT - CAPTURE - PROCESSED" ||
                            x.Name.ToUpper() == "KREDITKORT" ||
                            //x.Name.ToUpper() == "SWEDBANK - CAPTURE - PROCESSED" ||
                            x.Name.ToUpper() == "SWEDBANK" ||
                            //x.Name.ToUpper() == "SEB - CAPTURE - PROCESSED" ||
                            x.Name.ToUpper() == "SEB" ||
                            //x.Name.ToUpper() == "HANDELSBANKEN - CAPTURE - PROCESSED"
                            x.Name.ToUpper() == "HANDELSBANKEN"
                            );
                        */

                        string[] paymentMethodCodes = new string[] { "DIBSDIRECTNORDEA", "DIBSDIRECTHANDELSBANKEN", "DIBSDIRECTSWEDBANK", "DIBSDIRECTSEB", "DIBSCREDITCARDAUTH" };

                        IEnumerable<payment> _valueList2 = order_ViewObject.Payments.
                            Where(x => x.Code != null).
                            Where(x => x.TransactionType != null).
                            Where(x => x.TransactionType.ToUpper() == "DRAGNING").
                            Where(x => paymentMethodCodes.Contains(x.Code.ToUpper()));

                        foreach (payment _list in _valueList2)
                        {
                            creditableSum1 += _list.Sum;
                        }

                        order_ViewObject.AmountThatCanNotBeCredit = notCreditableSum3 + notCreditableSum1;
                        /*Log.Write("order_ViewObject.AmountThatCanNotBeCredit " + order_ViewObject.AmountThatCanNotBeCredit);
                        if (_savedCreditOrderRows != null && _savedCreditOrderRows.Count > 0)
                        {
                            order_ViewObject.AmountThatCanNotBeCredit += _savedCreditOrderRows.Where(x => x.OrderNumber == order_ViewObject.OrderNumber)
                                                                                        .Where(x => !string.IsNullOrEmpty(x.Sum))
                                                                                        .Sum(x => Convert.ToDecimal(x.Sum));
                        }*/

                        //if (IsExecutingInStandaloneSearchOrderArea)
                        //{
                        if (notCreditableSum2 != 0)
                        {
                            order_ViewObject.AmountThatCanNotBeCredit += notCreditableSum2;
                        }
                        //}

                        //_newOrder.AmountThatCanBeCredit = (_newOrder.OrderTotal - _cardValue - _nonrefundvalue1) + _nonrefundvalue2;
                        order_ViewObject.AmountThatCanBeCredit = creditableSum1;
                        // ******************************************************************************** 
                    }

                    order_ViewObject.OtherAmount = (order_ViewObject.OrderTotal - order_ViewObject.AmountThatCanNotBeCredit) + order_ViewObject.AmountThatCanBeCredit;
                    order_ViewObject.ValueCodeAmount = notCreditableSum1;

                    if (order_ViewObject.OrderTotal == notCreditableSum1)
                    {
                        order_ViewObject.IsCellEnabled = false;
                    }
                    else
                    {
                        order_ViewObject.IsCellEnabled = true;
                    }

                    /*
                    // enable/disable row in order child window details view
                    bool shouldRowBeEnabled = false;
                    if ( order_DataTransferObject!=null && order_DataTransferObject.Customer!=null) {
                            shouldRowBeEnabled = string.IsNullOrEmpty(order_DataTransferObject.Customer.AccountNumber)==false;
                            Log.Write("order_DataTransferObject.Customer.AccountNumber " + order_DataTransferObject.Customer.AccountNumber.ToString());
                    }
                    Log.Write("enableRow " + shouldRowBeEnabled);
                    */

                    //orderitems
                    order_ViewObject.OrderItems = new ObservableCollection<orderrow>();
                    foreach (OrderRow orderRow_DataTransferObject in order_DataTransferObject.OrderItems)
                    {
                        // AJ : These seems to be the lines displayed in child window when you click a order row
                        orderrow orderRow_detailsView_ViewObject = new orderrow();
                        orderRow_detailsView_ViewObject.Code = orderRow_DataTransferObject.Code;
                        orderRow_detailsView_ViewObject.Discount = orderRow_DataTransferObject.Discount;
                        orderRow_detailsView_ViewObject.Name = orderRow_DataTransferObject.Name;
                        orderRow_detailsView_ViewObject.Price = orderRow_DataTransferObject.Price;
                        orderRow_detailsView_ViewObject.Quantity = orderRow_DataTransferObject.Quantity;
                        orderRow_detailsView_ViewObject.CalculateRowPrice();
                        //orderRow_detailsView_ViewObject.IsRowEnabled = shouldRowBeEnabled;
                        order_ViewObject.OrderItems.Add(orderRow_detailsView_ViewObject);
                    }
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_createNewOrder");
            }
            return order_ViewObject;
        }


        #endregion

        #region form properties


        public string OrderNumberText { get; set; }

        private ObservableCollection<CustomerOrder> pOrders;
        public ObservableCollection<CustomerOrder> Orders
        {
            get { return pOrders; }
            set
            {
                if (pOrders != value)
                {
                    pOrders = value;
                    OnPropertyChanged("Orders");
                }
            }
        }

        private ObservableCollection<CustomerOrder> pOrderRows;
        private ObservableCollection<OrderHeader> _orderHeadCollection;

        public ObservableCollection<CustomerOrder> OrderRows
        {
            get { return pOrderRows; }
            set
            {
                if (pOrderRows != value)
                {
                    pOrderRows = value;
                    OnPropertyChanged("OrderRows");
                }
            }
        }

        #endregion

        private void _getCreditRows()
        {

            try
            {
                if (_ordersClient != null)
                {

                    if (!string.IsNullOrEmpty(_accountid) || !string.IsNullOrEmpty(_contactid))
                    {
                        _dispatcher.BeginInvoke(() => { IsWaiting = true; });

                        _ordersClient.GetSavedCreditOrderRowsCompleted += _ordersClient_GetSavedCreditOrderRowsCompleted;

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

        private void _getCreditRows(EventHandler<GetSavedCreditOrderRowsCompletedEventArgs> eventhandler/*, ObservableCollection<OrderHeader> _ohlist*/)
        {

            try
            {
                if (_ordersClient != null)
                {

                    //if (!string.IsNullOrEmpty(_accountid) || !string.IsNullOrEmpty(_contactid))
                    //{

                    _dispatcher.BeginInvoke(() => { IsWaiting = true; });

                    _ordersClient.GetSavedCreditOrderRowsCompleted += _ordersClient_GetSavedCreditOrderRowsCompleted;

                    // INFO: 
                    /*if (eventhandler != null)
                    {
                        _ordersClient.GetSavedCreditOrderRowsCompleted += eventhandler;
                    }*/


                    _ordersClient.GetSavedCreditOrderRowsAsync(_accountid, _contactid);
                    //}
                    //else
                    //{
                    //_dispatcher.BeginInvoke(() => { IsWaiting = false; });
                    //}
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

        private void _ordersClient_GetSavedCreditOrderRowsCompleted(object sender, GetSavedCreditOrderRowsCompletedEventArgs e)
        {
            try
            {
                _dispatcher.BeginInvoke(() => { _mainpage.dg2.ItemsSource = null; });

                _savedCreditOrderRows = new ObservableCollection<CreditOrderRow>();
                _showRows = new ObservableCollection<savedOrderRow>();

                if (e.Result != null)
                {
                    SavedCreditOrderRowsResponse _response = e.Result as SavedCreditOrderRowsResponse;

                    if (!string.IsNullOrEmpty(_response.ErrorMessage))
                    {
                        _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                        _mainpage.ShowErrorMessage(_response.ErrorMessage, "_ordersClient_GetSavedCreditOrderRowsCompleted 1");
                        return;
                    }

                    if (_response.OrderList != null && _response.OrderList.Count() > 0)
                    {

                        _savedCreditOrderRows = _response.OrderList;

                        foreach (CreditOrderRow _row in _savedCreditOrderRows)
                        {

                            savedOrderRow _sor = new savedOrderRow();
                            _sor.Row = "";
                            _sor.OrderNumber = _row.OrderNumber;
                            _sor.Date = _row.Date;
                            _sor.Time = _row.Time;
                            _sor.Product = _row.Productnumber;
                            _sor.ReferenceNumber = _row.ReferenceNumber;
                            _sor.CreatedBy = _row.CreatedBy;
                            _sor.Sum = _row.Sum;
                            _showRows.Add(_sor);

                            if (_orderSearchResultCollection != null && _orderSearchResultCollection.Count() > 0)
                            {

                                orderheader _o = _orderSearchResultCollection.FirstOrDefault(x => x.OrderNumber == _row.OrderNumber);
                                if (_o != null)
                                    _o.FontStyle = "X";
                                /*else
                                    _o.FontStyle = " ";*/
                            }
                        }

                        _dispatcher.BeginInvoke(() =>
                        {

                            _mainpage.dg2.ItemsSource = null;
                            _mainpage.dg2.ItemsSource = _showRows.OrderByDescending(x => x.OrderNumber);
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
                _ordersClient.GetSavedCreditOrderRowsCompleted -= _ordersClient_GetSavedCreditOrderRowsCompleted;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void GetCreditRows_OnlyFromAccountOrContactForm()
        {
            try
            {
                _getObjectID();
                _dispatcher.BeginInvoke(() => { IsWaiting = true; });
                _getCreditRows();
            }
            catch (Exception ex)
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "GetSavedOrderRows");
            }
        }

        private void _getObjectID()
        {
            try
            {

                _dispatcher.BeginInvoke(() =>
                {

                    //_data = _getKey(_querystring, "data");
                    if (IsExecutingInStandaloneSearchOrderArea)
                    {
                        // TODO anything?

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(_debug) && _typename.ToUpper() == "ACCOUNT")
                        {
                            string obj = HtmlPage.Window.Invoke("GetCustomerID") as string;
                            _id = obj;
                            _id = _id.Replace("{", "");
                            _id = _id.Replace("}", "");
                            _accountid = _id;
                            _contactid = "";
                        }

                        if (string.IsNullOrEmpty(_debug) && _typename.ToUpper() == "CONTACT")
                        {
                            string obj = HtmlPage.Window.Invoke("GetCustomerID") as string;
                            _id = obj;
                            _id = _id.Replace("{", "");
                            _id = _id.Replace("}", "");
                            _accountid = "";
                            _contactid = _id;

                        }

                        if (string.IsNullOrEmpty(_debug))
                        {
                            string obj = HtmlPage.Window.Invoke("GetEmailAddress") as string;
                            Emailaddress = obj;
                            _mainpage.txtEmail.Text = Emailaddress;
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

        private string _formatAmount(string input)
        {
            string _returnValue = "";

            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    input = input.Replace(".", ",");
                    decimal _value = 0.0M;
                    decimal.TryParse(input, out _value);
                    _returnValue = _value.ToString("# ### ### ###.00");
                }
                catch (Exception ex)
                {
                    _mainpage.ShowError(ex, "_formatAmount");
                }
            }

            return _returnValue;
        }

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


        public string _typename { get; set; }
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

    /*
    public class BoolToValueConverter<T> : System.Windows.Data.IValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return FalseValue;
            else
                return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? value.Equals(TrueValue) : false;
        }
    }
    */

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




