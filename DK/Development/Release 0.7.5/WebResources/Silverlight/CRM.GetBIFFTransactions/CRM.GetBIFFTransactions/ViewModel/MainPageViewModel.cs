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
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Threading;
using System.Windows.Data;
using System.ServiceModel;
using System.IO;
using System.Windows.Browser;
using System.Globalization;

using CRM.GetBIFFTransactions.TravelCardService;

// CRM.GetBIFFTransactionsPage.html?serveraddress=http://10.16.229.201/Skanetrafiken&userlcid=1053&id=BE3FEFCC-30F6-E311-80CE-0050569010AD
// DEBUGTestPage.html?serveraddress=http://10.16.229.201/Skanetrafiken&userlcid=1053&id=BE3FEFCC-30F6-E311-80CE-0050569010AD

// Case resolved
// DEBUGTestPage.html?serveraddress=http://10.16.229.201/Skanetrafiken&userlcid=1053&id=BE3FEFCC-30F6-E311-80CE-0050569010AD&data=CASE&debug=true

// Case not resolved
// DEBUGTestPage.html?serveraddress=http://10.16.229.201/Skanetrafiken&userlcid=1053&id=6C4BE848-31F6-E311-80CE-0050569010AD&data=CASE&debug=true&travelcard=2680642391
// DEBUGTestPage.html?serveraddress=http://V-DKCRM-UTV/Skanetrafiken&userlcid=1053&id=6C4BE848-31F6-E311-80CE-0050569010AD&data=CASE&debug=true&travelcard=2680642391

// Jojokort
// DEBUGTestPage.html?serveraddress=http://v-dkcrm-utv/Skanetrafiken&userlcid=1053&id=37656A24-D802-E411-80D1-0050569010AD&data=CASE&debug=true&travelcard=2680642391

// Från sitemap.
// DEBUGTestPage.html?serveraddress=http://10.16.229.201/Skanetrafiken&userlcid=1053


// CRM.GetBIFFTransactionsPage.html?serveraddress=http://10.16.229.201/Skanetrafiken&userlcid=1053&id=BE3FEFCC-30F6-E311-80CE-0050569010AD
// DEBUGTestPage.html?serveraddress=http://10.16.229.201/Skanetrafiken&userlcid=1053&id=BE3FEFCC-30F6-E311-80CE-0050569010AD

// http://v-dkcrm-utv/Skanetrafiken/main.aspx?etc=112&extraqs=&histKey=347357107&id=%7b6C4BE848-31F6-E311-80CE-0050569010AD%7d&newWindow=true&pagetype=entityrecord

// http://v-dkcrm-utv/Skanetrafiken/main.aspx?etc=112&extraqs=&histKey=614754393&id=%7b37656A24-D802-E411-80D1-0050569010AD%7d&newWindow=true&pagetype=entityrecord#473326458


// my case
// DEBUGTestPage.html?serveraddress=http://v-dkcrm-utv/Skanetrafiken&userlcid=1053&id=C7CC7E0F-AD06-E411-80D1-0050569010AD&data=CASE&debug=true&travelcard=2680642391

// readonly case
// DEBUGTestPage.html?serveraddress=http://V-DKCRM-UTV/Skanetrafiken&userlcid=1053&id=6C26A2BA-4507-E411-80D1-0050569010AD&data=CASE&debug=true&travelcard=2680642391

// solved case
// DEBUGTestPage.html?serveraddress=http://V-DKCRM-UTV/Skanetrafiken&userlcid=1053&id=6E71C836-B374-E411-80D4-0050569010AD&data=CASE&debug=true&travelcard=2680642391

// my case tst
// DEBUGTestPage.html?serveraddress=http://v-dkcrm-tst/Skanetrafiken&userlcid=1053&id=21827338-0085-E411-80D4-005056902292&data=CASE&debug=true&travelcard=0815648772
// DEBUGTestPage.html?serveraddress=http://v-dkcrm-tst/Skanetrafiken&userlcid=1053&id=21827338-0085-E411-80D4-005056902292&data=CASE&debug=true&travelcard=2680642391

// my case utv sitemmap
// DEBUGTestPage.html?serveraddress=http://v-dkcrm-utv/Skanetrafiken&userlcid=1053

// DEBUGTestPage.html?serveraddress=https://sekunduat.skanetrafiken.se/DKRMUAT&userlcid=1053&id=21827338-0085-E411-80D4-005056902292&data=CASE&debug=true&travelcard=2680642391



namespace CRM.GetBIFFTransactions.ViewModel
{
    public class MainPageViewModel : XrmBaseNotify
    {
        TravelCardClient _travelCardClient;
        CrmManager _crmManager;
        MainPage _mainpage;
        Dispatcher _dispatcher;
        setting _settings;
        SystemUserSettings _user;
        OutstandingCharges _outstandingCharges;
        RechargeCard _rechargeCard;

        ObservableCollection<localizedlabel> _localizedLabelsList;
        ObservableCollection<travelCardTransaction> _transactionList;
        ObservableCollection<Zone> _zoneNameList;
        ObservableCollection<travelCardTransaction> _saveList = new ObservableCollection<travelCardTransaction>();

        string _userlcid;
        string _data = "";
        string _id = "";
        string _debug = "";
        string _debugcard = "";

        private travelCard _travelcard;
        public travelCard Travelcard
        {
            get { return _travelcard; }
            set { _travelcard = value; OnPropertyChanged("Travelcard"); }
        }

        private cardEXT _cardExt;
        public cardEXT CardExt
        {
            get { return _cardExt; }
            set { _cardExt = value; OnPropertyChanged("CardExt"); }
        }

        private bool _isWaiting;
        public bool IsWaiting
        {
            get { return _isWaiting; }
            set 
            {
                if (_isWaiting != value)
                {
                    _isWaiting = value;
                    OnPropertyChanged("IsWaiting");
                }
            }
        }

        private bool _isLoadingWaiting;
        public bool IsLoadingWaiting
        {
            get { return _isLoadingWaiting; }
            set { _isLoadingWaiting = value; OnPropertyChanged("IsLoadingWaiting"); }
        }

        private bool _saveEnabled;
        public bool SaveEnabled
        {
            get { return _saveEnabled; }
            set { _saveEnabled = value; OnPropertyChanged("SaveEnabled"); }
        }

        public string Version
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1].Trim(); }
        }

        public MainPageViewModel(MainPage mainpage, CrmManager crmmanager, Dispatcher theDispatcher, string userlcid, string entityid, string data, string debug, string debugcard)
        {
            try
            {
                _searchTravelCardCommand = new RelayCommand(_searchTravelCard, true);
                _searchTravelCardTransactionsCommand = new RelayCommand(_searchTravelCardTransactions, true);

                _mainpage = mainpage;
                _dispatcher = theDispatcher;
                _crmManager = crmmanager;
                _userlcid = userlcid;
                _data = data;
                _id = "";
                _debug = debug;
                _debugcard = debugcard;

                if (!string.IsNullOrEmpty(_data))
                    IsWaiting = true;

                if (entityid != null)
                    _id = entityid;

                if (!string.IsNullOrEmpty(_debugcard))
                {
                    _mainpage.txtTravelCard.Text = _debugcard;
                    _mainpage.txtTravelCard.Focus();
                }

                _mainpage.DataContext = this;
                
                DateTime baseDate = DateTime.Today;
                var thisMonthStart = baseDate.AddDays(-30);     // (1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                var thisDay = baseDate;

                _mainpage.txtSearchTransactionsFrom.Text = thisMonthStart.ToShortDateString();
                _mainpage.txtSearchTransactionsTo.Text = thisDay.ToShortDateString();

                _mainpage.txtSearchTransactionsFrom.LostFocus += txtSearchTransactionsFrom_LostFocus;
                _mainpage.txtSearchTransactionsTo.LostFocus += txtSearchTransactionsTo_LostFocus;

                _mainpage.txtSearchTransactionsFrom.GotFocus += txtSearchTransactionsFrom_GotFocus;
                _mainpage.txtSearchTransactionsTo.GotFocus += txtSearchTransactionsTo_GotFocus;

                _mainpage.txtTravelCard.GotFocus += txtTravelCard_GotFocus;
                _mainpage.txtTravelCard.KeyDown += txtTravelCard_KeyDown;

                _mainpage.btnoutstandingCharges.Click += btnoutstandingCharges_Click;

                _mainpage.LayoutRoot.Loaded += LayoutRoot_Loaded;
             
                _mainpage.btnoutstandingCharges.IsEnabled = false;
             
            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "MainPageViewModel");
            }
        }

        private void btnoutstandingCharges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_mainpage.txtTravelCard.Text))
                {
                    IsWaiting = true;
                    string _cardNumber = _mainpage.txtTravelCard.Text.Trim();
                    _travelCardClient.RechargeCardCompleted += _travelCardClient_RechargeCardCompleted;
                    _travelCardClient.RechargeCardAsync(_cardNumber);
                }
            }
            catch (Exception ex)
            {
                _travelCardClient.RechargeCardCompleted -= _travelCardClient_RechargeCardCompleted;
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "btnoutstandingCharges_Click");
            }
        }

        private void _travelCardClient_RechargeCardCompleted(object sender, RechargeCardCompletedEventArgs e)
        {
            try
            {
                RechargeCardResponse _response = e.Result as RechargeCardResponse;

                if (!string.IsNullOrEmpty(_response.ErrorMessage))
                {
                    IsWaiting = false;
                    _mainpage.ShowMessage(_response.ErrorMessage, "_travelCardClient_RechargeCardCompleted");
                    return;
                }

                if (e.Result != null)
                {
                    _rechargeCard = new RechargeCard();
                    string _result = _response.RechargeCard;
                    XDocument _xdoc = XDocument.Parse(_result);
                    XElement _rechargeCardResponse = _getInformationNodeFromXML(_xdoc, "RechargeCardResponse");
                    _rechargeCard.Success = Convert.ToBoolean(_getValueFromXML(_rechargeCardResponse, "Success"));
                    _rechargeCard.Message = _getValueFromXML(_rechargeCardResponse, "Message");
                    _rechargeCard.ErrorMessage = _getValueFromXML(_rechargeCardResponse, "ErrorMessage");
                    _rechargeCard.StatusCode = Convert.ToInt32(_getValueFromXML(_rechargeCardResponse, "StatusCode"));

                    _mainpage.ShowMessage(_rechargeCard.Message, "Återladda");
                }
            }
            catch (Exception ex)
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_travelCardClient_RechargeCardCompleted");
            }
            finally
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _travelCardClient.RechargeCardCompleted -= _travelCardClient_RechargeCardCompleted;
            }
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //If silverlight control is placed on travelcard entity. try to find travelcardnumber
                _getTravelCardNumber();
                //_getCRMSettings();
                _getUser();
            }
            catch (Exception ex)
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "LayoutRoot_Loaded");
            }
        }

        private void txtTravelCard_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                    _searchTravelCard();
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "txtTravelCard_KeyDown");
            }
        }

        private void txtTravelCard_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_mainpage.txtTravelCard.Text))
                {
                    _mainpage.txtTravelCard.SelectAll();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "txtTravelCard_GotFocus");
            }
        }

        private void txtSearchTransactionsTo_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_mainpage.txtSearchTransactionsTo.Text))
                {
                    _mainpage.txtSearchTransactionsTo.SelectAll();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "txtSearchTransactionsTo_GotFocus");
            }
        }

        private void txtSearchTransactionsFrom_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_mainpage.txtSearchTransactionsFrom.Text))
                {
                    _mainpage.txtSearchTransactionsFrom.SelectAll();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "txtSearchTransactionsFrom_GotFocus");
            }
        }

        private void txtSearchTransactionsTo_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_mainpage.txtSearchTransactionsTo.Text))
                {
                    string _date = _checkFormatOfDate(_mainpage.txtSearchTransactionsTo.Text);
                    DateTime _dt;
                    bool _ok = DateTime.TryParse(_date, out _dt);
                    if (_ok == false)
                        _mainpage.txtSearchTransactionsTo.Text = "";
                    else
                        _mainpage.txtSearchTransactionsTo.Text = _dt.ToShortDateString();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "dtTimeTo_LostFocus");
            }
        }

        private void txtSearchTransactionsFrom_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_mainpage.txtSearchTransactionsFrom.Text))
                {
                    string _date = _checkFormatOfDate(_mainpage.txtSearchTransactionsFrom.Text);
                    DateTime _dt;
                    bool _ok = DateTime.TryParse(_date, out _dt);
                    if (_ok == false)
                        _mainpage.txtSearchTransactionsFrom.Text = "";
                    else
                        _mainpage.txtSearchTransactionsFrom.Text = _dt.ToShortDateString();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "dtTimeTo_LostFocus");
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

        private void _setLoadingFalse()
        {
            _dispatcher.BeginInvoke(() => {
                IsWaiting = false;
                IsLoadingWaiting = false;
                //_mainpage.LoadingRoot.Visibility = Visibility.Collapsed;
                _mainpage.LayoutRoot.Visibility = Visibility.Visible;
            });
        }

        public void LoadSavedBIFFTransactions(string entityid)
        {
            try
            {
                if (_data.ToUpper() == "CASE")
                {
                    _id = entityid;
                    _mainpage.txtid.Text = _id;
                    _getSavedTravelCardTransactions();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "LoadSavedBIFFTransactions");
            }
        }

        public void StartSearchTravelCardFromBIFF(string entityid)
        {
            try
            {
                if (_data.ToUpper() == "TRAVELCARD")
                {
                    _searchTravelCard();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "StartSearchTravelCardFromBIFF");
            }
        }

        private void _getUser()
        {
            try
            {
                _crmManager.GetCurrentUser(_getUser_callback);
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_getUser");
            }
        }

        private void _getUser_callback(ObservableCollection<CGIXrm.SystemUserSettings> result)
        {
            try
            {
                _user = result[0] as CGIXrm.SystemUserSettings;
                _getCRMSettings();
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_gerUser_callback");
            }
        }

        private void _getCRMSettings()
        {
            try
            {
                _crmManager.Fetch<setting>(_xmlSettings(), _getCRMSettings_completed);
            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_getCRMSettings");
            }
        }

        private void _getCRMSettings_completed(ObservableCollection<setting> result)
        {
            try
            {
                if (result != null && result.Count() > 0)
                {
                    _settings = new setting();
                    _settings.Crmcardservice = result[0].Crmcardservice;
                    _initTravelCardService();
                }
                else
                {
                    _setLoadingFalse();
                    _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                    throw new Exception("Fel i hämta CRMCardService!");
                }
            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_getCRMSettings_completed");
            }
        }

        internal static string _xmlSettings()
        {
            string _xml = "";

            string _now = DateTime.Now.ToShortDateString();

            _xml += "<fetch distinct='false' mapping='logical' version='1.0'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_crmcardservice'/>";
			_xml += "       <attribute name='cgi_crmuri'/>";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' value='0' operator='eq'/>";
            _xml += "           <condition attribute='cgi_validfrom' value='" + _now + "' operator='on-or-before'/>";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_validto' value='" + _now + "' operator='on-or-after'/>";
            _xml += "               <condition attribute='cgi_validto' operator='null'/>";
            _xml += "           </filter>";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";
            
            return _xml;
        }

        private void _initTravelCardService()
        {
            try
            {
#if WCFDEBUG
                string _url = "http://localhost:63137/TravelCard.svc";
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
                EndpointAddress _webserviceEndpoint = new EndpointAddress(_settings.Crmcardservice);

#endif

                _travelCardClient = new TravelCardClient(_webserviceBindning, _webserviceEndpoint);
                
                _getLocalizedLabels();
            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_initTravelCardService");
            }
        }

        private void _getLocalizedLabels()
        {
            try
            {
                _crmManager.Fetch<CRM.GetBIFFTransactions.localizedlabel>(_xmlLanguage(), _getLocalizedLabels_completed);
            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_getLocalizedLabels");
            }
        }

        private void _getLocalizedLabels_completed(ObservableCollection<CRM.GetBIFFTransactions.localizedlabel> result)
        {
            try
            {
                if (result != null && result.Count() > 0)
                {
                    _localizedLabelsList = new ObservableCollection<CRM.GetBIFFTransactions.localizedlabel>();
                    _localizedLabelsList = result as ObservableCollection<CRM.GetBIFFTransactions.localizedlabel>;
                }

                _loadTranslations();
            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_getLocalizedLabels_completed");
            }
        }

        private string _xmlLanguage()
        {
            string _xml = "";

            if (string.IsNullOrEmpty(_userlcid))
                _userlcid = "1053";

            _xml += "<fetch distinct='false' mapping='logical' version='1.0'>";
            _xml += "   <entity name='cgi_localizedlabel'>";
            _xml += "       <attribute name='cgi_localizedlabelname'/>";
            _xml += "       <attribute name='cgi_localizedlabelgroupid'/>";
            _xml += "       <attribute name='cgi_localizedcontrolid'/>";
            _xml += "       <link-entity name='cgi_localizedlabelgroup' alias='ag' to='cgi_localizedlabelgroupid' from='cgi_localizedlabelgroupid'>";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='cgi_localizedlabelgroupname' value='SILVERLIGHT_GET_BIFFTRANSACTIONS' operator='eq'/>";
            _xml += "       </filter>";
            _xml += "       </link-entity>";
            _xml += "       <link-entity name='cgi_localizationlanguage' alias='ah' to='cgi_localizationlanguageid' from='cgi_localizationlanguageid'>";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='cgi_localizationlanguagenumber' value='" + _userlcid + "' operator='eq'/>";
            _xml += "       </filter>";
            _xml += "       </link-entity>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            return _xml;
        }

        private void _loadTranslations()
        {
            try
            {
                _dispatcher.BeginInvoke(() => {
                    _mainpage.lblSearchCard.Text = _getLocalizedLabel("LABEL_SEARCH_CARD");

                    _mainpage.lblCRMCardInformation.Text = _getLocalizedLabel("LABEL_CRMCARD_INFO");
                    _mainpage.lblContactName.Text = _getLocalizedLabel("LABEL_ContactName");
                    _mainpage.lblCardName.Text = _getLocalizedLabel("LABEL_CardName");
                    //_mainpage.lblCardBlocked.Text = _getLocalizedLabel("LABEL_Blocked");
                    _mainpage.lblCardAutoLoad.Text = _getLocalizedLabel("LABEL_CardAutoLoadStatus");
                    _mainpage.lblCardAutoLoadStartDate.Text = _getLocalizedLabel("LABEL_CardAutoLoadStartDate");
                    _mainpage.lblOutstandingCharges.Text = _getLocalizedLabel("LABEL_OutstandingCharges");
                    _mainpage.btnoutstandingCharges.Content = _getLocalizedLabel("BTN_reload");
					_mainpage.lblCreditCardMask.Text = _getLocalizedLabel("LABEL_CreditCardMask");
					_mainpage.lblFailedAttemptsToChargeMoney.Text = _getLocalizedLabel("LABEL_FailedAttemptsToChargeMoney");
                    //_mainpage.lblCardAutoLoadEndDate.Text = _getLocalizedLabel("LABEL_CardAutoLoadEndDate");
                    //_mainpage.lblCardValidStartDate.Text = _getLocalizedLabel("LABEL_ValidStartDate");

                    _mainpage.lblCardInformation.Text = _getLocalizedLabel("LABEL_CARD_INFORMATION");
                    _mainpage.lblHotListed.Text = _getLocalizedLabel("LABEL_CardHotlistedStatus");

                    _mainpage.lblPurseInformation.Text = _getLocalizedLabel("LABEL_PURSE_INFORMATION");
                    _mainpage.lblPurseBalance.Text = _getLocalizedLabel("LABEL_Pursebalance");
                    _mainpage.lblPurseOutstandingDirectedAutoload.Text = _getLocalizedLabel("LABEL_PurseOutstandingDirectedAutoload");
                    _mainpage.lblPurseHotlisted.Text = _getLocalizedLabel("LABEL_PurseHotlisted");
                    _mainpage.lblPurseHotlistReason.Text = _getLocalizedLabel("LABEL_PurseHotListReason");

                    _mainpage.lblPeriodCardInformation.Text = _getLocalizedLabel("LABEL_PERIOD_INFORMATION");

                    //_mainpage.lblPeriodCardCategory.Text = _getLocalizedLabel("LABEL_PeriodCardCategory");
                    _mainpage.lblPeriodProductType.Text = _getLocalizedLabel("LABEL_PeriodProductType");
                    _mainpage.lblPeriodPeriodStart.Text = _getLocalizedLabel("LABEL_PeriodPeriodStart");
                    _mainpage.lblPeriodPeriodEnd.Text = _getLocalizedLabel("LABEL_PeriodPeriodEnd");
                    _mainpage.lblPeriodWaitingPeriods.Text = _getLocalizedLabel("LABEL_NumberOfWaitingPeriods");
                    _mainpage.lblPeriodPricePaid.Text = _getLocalizedLabel("LABEL_PricePaid");
                    _mainpage.lblPeriodOutstandingEnableThresholdAutoload.Text = _getLocalizedLabel("LABEL_PeriodOutstandingEnableThresholdAutoload");
                    _mainpage.lblPeriodHotlisted.Text = _getLocalizedLabel("LABEL_PeriodHotlisted");

                    _mainpage.lblZoneList.Text = _getLocalizedLabel("LABEL_ZoneList");
                    _mainpage.lblRouteList.Text = _getLocalizedLabel("LABEL_RouteList");
                    _mainpage.lblSearchTransactionsFrom.Text = _getLocalizedLabel("LABEL_SEARCHFROM");
                    _mainpage.lblSearchTransactionsTo.Text = _getLocalizedLabel("LABEL_SEARCHTO");
                    
                    _setColumnHeader(1, _getLocalizedLabel("LABEL_TRAN_TransactionDate"));
                    _setColumnHeader(2, _getLocalizedLabel("LABEL_TRAN_TransactionTime"));
                    _setColumnHeader(3, _getLocalizedLabel("LABEL_TRAN_CardSection"));
                    _setColumnHeader(4, _getLocalizedLabel("LABEL_TRAN_TRT"));
                    _setColumnHeader(5, _getLocalizedLabel("LABEL_TRAN_TT"));
                    _setColumnHeader(10, _getLocalizedLabel("LABEL_TRAN_RouteName"));
                    _setColumnHeader(6, _getLocalizedLabel("LABEL_TRAN_NewBalanceInPurce"));
                    _setColumnHeader(7, _getLocalizedLabel("LABEL_TRAN_Amount"));
                    _setColumnHeader(8, _getLocalizedLabel("LABEL_TRAN_OrigZone"));
                    _setColumnHeader(9, _getLocalizedLabel("LABEL_TRAN_DestZone"));
                    _setColumnHeader(11, _getLocalizedLabel("LABEL_TRAN_DeviceID"));

                    _mainpage.btnSearchCard.Content = "Sök";
                    _mainpage.btnSearchCardTransactions.Content = "Sök";

                });

                _getZoneNames();
            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_loadTranslations");
            }
        }

        private void _setColumnHeader(int idx, string headertext)
        {
            try
            {
                foreach (DataGridTemplateColumn _c in _mainpage.grdGridSearch.Columns)
                {
                    if (_c.DisplayIndex == idx)
                        _c.Header = headertext;
                }
            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_loadTranslations");
            }
        }

        private void _getZoneNames()
        {
            try
            {
                _travelCardClient.GetZoneNamesCompleted += _travelCardClient_GetZoneNamesCompleted;
                _travelCardClient.GetZoneNamesAsync();
            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_getZoneNames");
            }
        }

        private void _travelCardClient_GetZoneNamesCompleted(object sender, GetZoneNamesCompletedEventArgs e)
        {
            try
            {
                GetZoneNamesResponse _response = e.Result as GetZoneNamesResponse;

                if (!string.IsNullOrEmpty(_response.ErrorMessage))
                {
                    _setLoadingFalse();
                    _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                    _mainpage.ShowMessage(_response.ErrorMessage, "_travelCardClient_GetZoneNamesCompleted");
                    return;
                }

                if (_response.Zones != null)
                {
                    _zoneNameList = new ObservableCollection<Zone>();
                    foreach (Zone _zone in _response.Zones)
                    {
                        _zoneNameList.Add(_zone);
                    }
                }

                if (!string.IsNullOrEmpty(_data) && _data.ToUpper().ToString() == "CASE")
                    _getSavedTravelCardTransactions();

                if (!string.IsNullOrEmpty(_data) && _data.ToUpper().ToString() == "TRAVELCARD")
                {
                    _dispatcher.BeginInvoke(() =>
                    {
                        if (!string.IsNullOrEmpty(_mainpage.txtTravelCard.Text))
                            _searchTravelCard();
                    });
                }


            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_travelCardClient_GetZoneNamesCompleted");
            }
            finally
            {
                _travelCardClient.GetZoneNamesCompleted -= _travelCardClient_GetZoneNamesCompleted;
            }
        }

        private string _getLocalizedLabel(string labelname)
        {
            string _returnValue = "";

            try
            {
                if (_localizedLabelsList != null && _localizedLabelsList.Count() > 0)
                {
                    localizedlabel _label = _localizedLabelsList.FirstOrDefault(x => x.Localizedcontrolid == labelname);
                    if (_label != null)
                    {
                        _returnValue = _label.Localizedlabelname;
                    }
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_getLocalizedLabel");
            }

            return _returnValue;
        }

        private void _getTravelCardNumber()
        {
            try
            {
                if (!string.IsNullOrEmpty(_debug))
                    return;

                if (_data.ToUpper() == "TRAVELCARD")
                {
                    string obj = HtmlPage.Window.Invoke("getTravelCardNumber") as string;
                    _mainpage.txtTravelCard.Text = obj;
                    _id = "";
                    _mainpage.txtTravelCard.Focus();
                }
                else if (_data.ToUpper() == "CASE")
                {
                    string obj = HtmlPage.Window.Invoke("getTravelCardNumberFromCase") as string;
                    _findTravelCardNumber(obj);      
                }
                else if (string.IsNullOrEmpty(_data))
                {
                    System.Windows.Browser.HtmlPage.Plugin.Focus();
                    _mainpage.txtTravelCard.Focus();
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_getTravelCardNumber");
            }
        }

        public void SetTravelCardFromCase(string travelcardid)
        {
            try
            {
                _findTravelCardNumber(travelcardid);
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "SetTravelCardFromCase");
            }
        }

        private void _findTravelCardNumber(string travelcardid)
        {
            try
            {
                if (!string.IsNullOrEmpty(travelcardid))
                {
                    _dispatcher.BeginInvoke(() =>
                    {
                        _mainpage.txtTravelCard.Text = travelcardid;
                    });
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_findTravelCardNumber");
            }
        }

        private void _findTravelCardNumber_callback(ObservableCollection<cardCRM> result)
        {
            try
            {
                if (result != null && result.Count() > 0)
                {
                    _dispatcher.BeginInvoke(() => { 
                        cardCRM _card = result[0] as cardCRM;
                        string _cardnumber = _card.CardNumber;
                        _mainpage.txtTravelCard.Text = _cardnumber;
                    });
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_findTravelCardNumber_callback");
            }
        }

        private string _xmlfindTravelCardNumber(string id)
        {
            string _xml = "";

            _xml += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_travelcard'>";
            _xml += "       <attribute name='cgi_travelcardnumber' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='cgi_travelcardid' operator='eq' value='" + id + "' />";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            return _xml;
        }


        // *** Commands ***

        public void SaveTravelInformation(travelCardTransaction transaction)
        {
            try
            {
                //TravelCard, GetDate, GetTime, Route
                if (_checkIfRowExists(transaction) == true)
                    return;
                
                IsWaiting = true;

                Entity _transactionEnt = _createTransactionEntity(transaction);
                if (_transactionEnt != null)
                {
                    _crmManager.Service.BeginCreate(_transactionEnt, SaveTravelInformation_completed, transaction);
                }               
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "SaveTravelInformation");
            }
        }

        private bool _checkIfRowExists(travelCardTransaction transaction)
        {
            bool _returnValue = false;

            string _travelcard = transaction.TravelCard;
            string _date = transaction.GetDate;
            string _time = transaction.GetTime;
            string _route = transaction.Route;

            travelCardTransaction _t = _saveList.FirstOrDefault(
                x => x.TravelCard == _travelcard &&
                x.GetDate == _date &&
                x.GetTime == _time &&
                x.Route == _route);
            if (_t != null)
                _returnValue = true;

            return _returnValue;
        }

        public void DeleteTravelInformation(travelCardTransaction transaction)
        {
            try
            {
                IsWaiting = true;
                _crmManager.Service.BeginDelete("cgi_travelcardtransaction", transaction.Transactionid.Value, DeleteTravelInformation_completed, transaction);
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "DeleteTravelInformation");
            }
        }

        private void SaveTravelInformation_completed(IAsyncResult result)
        {
            try
            {
                travelCardTransaction _transaction = result.AsyncState as travelCardTransaction;
                Guid _transactionid = _crmManager.Service.EndCreate(result);
                _transaction.Transactionid = _transactionid;
                
                _dispatcher.BeginInvoke(() => {
                    _saveList.Add(_transaction);
                    _mainpage.lstSelection.ItemsSource = null;
                    _mainpage.lstSelection.ItemsSource = _saveList;

                    IsWaiting = false;
                });
            }
            catch (Exception ex)
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "SaveTravelInformation_completed");
            }
        }

        private void DeleteTravelInformation_completed(IAsyncResult result)
        {
            try
            {
                _crmManager.Service.EndDelete(result);
                travelCardTransaction _transaction = result.AsyncState as travelCardTransaction;
                
                _dispatcher.BeginInvoke(() =>
                {
                    travelCardTransaction _saveItem = _saveList.FirstOrDefault(x => x.Transactionid == _transaction.Transactionid);
                    if (_saveItem != null)
                    {
                        _saveList.Remove(_saveItem);
                    }
                    
                    _mainpage.lstSelection.ItemsSource = null;
                    _mainpage.lstSelection.ItemsSource = _saveList;

                    IsWaiting = false;
                });
            }
            catch (Exception ex)
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "DeleteTravelInformation_completed");
            }
        }

        private Entity _createTransactionEntity(travelCardTransaction transaction)
        {
            Entity _transactionEntity = null;

            try
            {
                _transactionEntity = new Entity();
                _transactionEntity.LogicalName = "cgi_travelcardtransaction";
                _transactionEntity.Attributes = new AttributeCollection();

                if (!string.IsNullOrEmpty(CardExt.TravelCardID.ToString()))
                {
                    if (CardExt.TravelCardID != Guid.Empty)
                    {
                        CGIXrm.CrmSdk.KeyValuePair<string, object> _keyID = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                        _keyID.Key = "cgi_travelcardid";
                        EntityReference _refcardid = new EntityReference();
                        _refcardid.Id = CardExt.TravelCardID;
                        _refcardid.LogicalName = "cgi_travelcard";
                        _keyID.Value = _refcardid;
                        _transactionEntity.Attributes.Add(_keyID);
                    }
                }

                if (!string.IsNullOrEmpty(transaction.CardSect))
                {
                    CGIXrm.CrmSdk.KeyValuePair<string, object> _keycgi_cardsect = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                    _keycgi_cardsect.Key = "cgi_cardsect";
                    _keycgi_cardsect.Value = transaction.CardSect;
                    _transactionEntity.Attributes.Add(_keycgi_cardsect);
                }

                if (!string.IsNullOrEmpty(transaction.Date))
                {
                    CGIXrm.CrmSdk.KeyValuePair<string, object> _keydate = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                    _keydate.Key = "cgi_date";
                    _keydate.Value = transaction.GetDate;
                    _transactionEntity.Attributes.Add(_keydate);
                }

                if (!string.IsNullOrEmpty(transaction.Date))
                {
                    CGIXrm.CrmSdk.KeyValuePair<string, object> _keytime = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                    _keytime.Key = "cgi_time";
                    _keytime.Value = transaction.GetTime;
                    _transactionEntity.Attributes.Add(_keytime);
                }

                if (!string.IsNullOrEmpty(transaction.Amount))
                {
                    CGIXrm.CrmSdk.KeyValuePair<string, object> _amount = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                    _amount.Key = "cgi_amount";
                    _amount.Value = transaction.Amount;
                    _transactionEntity.Attributes.Add(_amount);
                }

                if (!string.IsNullOrEmpty(transaction.DeviceID))
                {
                    CGIXrm.CrmSdk.KeyValuePair<string, object> _keycgi_deviceid = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                    _keycgi_deviceid.Key = "cgi_deviceid";
                    _keycgi_deviceid.Value = transaction.DeviceID;
                    _transactionEntity.Attributes.Add(_keycgi_deviceid);
                }

                if (!string.IsNullOrEmpty(transaction.OrigZone))
                {
                    CGIXrm.CrmSdk.KeyValuePair<string, object> _keycgi_origzone = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                    _keycgi_origzone.Key = "cgi_origzone";
                    _keycgi_origzone.Value = transaction.OrigZone;
                    _transactionEntity.Attributes.Add(_keycgi_origzone);
                }

                if (!string.IsNullOrEmpty(transaction.OrigZoneName))
                {
                    CGIXrm.CrmSdk.KeyValuePair<string, object> _keycgi_origzonename = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                    _keycgi_origzonename.Key = "cgi_origzonename";
                    _keycgi_origzonename.Value = transaction.OrigZoneName;
                    _transactionEntity.Attributes.Add(_keycgi_origzonename);
                }

                if (!string.IsNullOrEmpty(transaction.RecType))
                {
                    CGIXrm.CrmSdk.KeyValuePair<string, object> _keycgi_rectype = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                    _keycgi_rectype.Key = "cgi_rectype";
                    _keycgi_rectype.Value = transaction.RecType;
                    _transactionEntity.Attributes.Add(_keycgi_rectype);
                }

                if (!string.IsNullOrEmpty(transaction.Route))
                {
                    CGIXrm.CrmSdk.KeyValuePair<string, object> _keycgi_route = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                    _keycgi_route.Key = "cgi_route";
                    _keycgi_route.Value = transaction.Route;
                    _transactionEntity.Attributes.Add(_keycgi_route);
                }

                if (!string.IsNullOrEmpty(transaction.TxnType))
                {
                    CGIXrm.CrmSdk.KeyValuePair<string, object> _keycgi_txntype = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                    _keycgi_txntype.Key = "cgi_txntype";
                    _keycgi_txntype.Value = transaction.TxnType;
                    _transactionEntity.Attributes.Add(_keycgi_txntype);
                }

                if (!string.IsNullOrEmpty(_id))
                {
                    CGIXrm.CrmSdk.KeyValuePair<string, object> _keycgi_caseid = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                    _keycgi_caseid.Key = "cgi_caseid";
                    EntityReference _refcaseid = new EntityReference();
                    _refcaseid.Id = new Guid(_id);
                    _refcaseid.LogicalName = "incident";
                    _keycgi_caseid.Value = _refcaseid;
                    _transactionEntity.Attributes.Add(_keycgi_caseid);
                }

                if (!string.IsNullOrEmpty(transaction.TravelCard))
                {
                    CGIXrm.CrmSdk.KeyValuePair<string, object> _keycgi_travelcard = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                    _keycgi_travelcard.Key = "cgi_travelcard";
                    _keycgi_travelcard.Value = transaction.TravelCard;
                    _transactionEntity.Attributes.Add(_keycgi_travelcard);
                }

                CGIXrm.CrmSdk.KeyValuePair<string, object> _keytravelcardtransaction = new CGIXrm.CrmSdk.KeyValuePair<string, object>();
                _keytravelcardtransaction.Key = "cgi_travelcardtransaction";
                _keytravelcardtransaction.Value = transaction.GetName;
                _transactionEntity.Attributes.Add(_keytravelcardtransaction);
                
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_createTransactionEntity");
            }

            return _transactionEntity;
        }

        private RelayCommand _searchTravelCardCommand;
        public RelayCommand SearchTravelCardCommand
        {
            get { return _searchTravelCardCommand; }
            set { _searchTravelCardCommand = value; OnPropertyChanged("SearchTravelCardCommand"); }
        }
        
        private void _searchTravelCard()
        {
            try
            {
                try
                {
                    if (!string.IsNullOrEmpty(_mainpage.txtTravelCard.Text))
                    {
                        _mainpage.CRMCardViewer.DataContext = null;
                        _mainpage.CardViewer.DataContext = null;
                        _mainpage.PurseViewer.DataContext = null;
                        _mainpage.PeriodViewer.DataContext = null;
                        _mainpage.ZoneViewer.DataContext = null;
                        _mainpage.grdGridSearch.ItemsSource = null;
                        _mainpage.btnoutstandingCharges.IsEnabled = false;
                        
                        CardExt = new cardEXT(_crmManager);

                        if (_travelCardClient != null)
                        {
                            IsWaiting = true;
                            string _card = _mainpage.txtTravelCard.Text;
                            _travelCardClient.GetCardDetailsCompleted += _travelCardClient_GetCardDetailsCompleted;
                            _travelCardClient.GetCardDetailsAsync(_card);
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsWaiting = false;
                    _mainpage.ShowError(ex, "_searchTravelCard");
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_searchTravelCard");
            }
        }

        private RelayCommand _searchTravelCardTransactionsCommand;
        public RelayCommand SearchTravelCardTransactionsCommand
        {
            get { return _searchTravelCardTransactionsCommand; }
            set { _searchTravelCardTransactionsCommand = value; OnPropertyChanged("SearchTravelCardTransactionsCommand"); }
        }

        private void _searchTravelCardTransactions()
        {
            try
            {
                try
                {
                    if (!string.IsNullOrEmpty(_mainpage.txtTravelCard.Text) && !string.IsNullOrEmpty(_mainpage.txtSearchTransactionsFrom.Text) && !string.IsNullOrEmpty(_mainpage.txtSearchTransactionsTo.Text))
                    {
                        IsWaiting = true;
                        string _card = _mainpage.txtTravelCard.Text;
                        string _from = _mainpage.txtSearchTransactionsFrom.Text;
                        string _to = _mainpage.txtSearchTransactionsTo.Text;
                        string _maxtransactions = "100";

                        DateTime _datefrom;
                        DateTime _dateto;

                        if (!DateTime.TryParse(_from, out _datefrom))
                        {
                            IsWaiting = false;
                            return;
                        }

                        if (!DateTime.TryParse(_to, out _dateto))
                        {
                            IsWaiting = false;
                            return;
                        }

                        _travelCardClient.GetCardTransactionsCompleted += _travelCardClient_GetCardTransactionsCompleted;
                        _travelCardClient.GetCardTransactionsAsync(_card, _maxtransactions, _from, _to);

                    }
                }
                catch (Exception ex)
                {
                    _mainpage.ShowError(ex,"_searchTravelCardTransactions");
                }
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_searchTravelCardTransactions");
            }
        }

        private IEnumerable<XElement> _getCardTransactionsNodesFromXML(XDocument xdoc)
        {
            IEnumerable<XElement> _returnValue = null;
            try
            {
                _returnValue = xdoc.Descendants().Where(p => p.Name.LocalName == "Transactions");
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_getCardTransactionsNodesFromXML");
            }
            return _returnValue;
        }

        private IEnumerable<XElement> _getZonesListNodesFromXML(XDocument xdoc, string nodename)
        {
            IEnumerable<XElement> _returnValue = null;
            try
            {
                _returnValue = xdoc.Descendants().Where(p => p.Name.LocalName == nodename);
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_getZonesListNodesFromXML");
            }
            return _returnValue;
        }

        private void _travelCardClient_GetCardTransactionsCompleted(object sender, GetCardTransactionsCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    GetCardTransactionsResponse result = e.Result as GetCardTransactionsResponse;

                    if (!string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        IsWaiting = false;
                        _mainpage.ShowMessage(result.ErrorMessage, "_travelCardClient_GetCardTransactionsCompleted");
                        return;
                    }

                    _transactionList = new ObservableCollection<travelCardTransaction>();

                    if (e.Result != null)
                    {
                        string _result = result.Transactions;
                        XDocument _xdoc = XDocument.Parse(_result);

                        IEnumerable<XElement> _zonesList = _getZonesListNodesFromXML(_xdoc, "ZoneList");

                        IEnumerable<XElement> _cardTransactions = _getCardTransactionsNodesFromXML(_xdoc);
                        if (_cardTransactions != null)
                        {
                            foreach (XElement _element in _cardTransactions)
                            {
                                travelCardTransaction _t = new travelCardTransaction();
                                _t.Date = _getValueFromXML(_element, "Date");
                                _t.DeviceID = _getValueFromXML(_element, "DeviceID");
                                _t.CardSect = _convertCardSect(_getValueFromXML(_element, "CardSect"));
                                _t.RecType = _convertTransactionRecordType(_getValueFromXML(_element, "RecType"));
                                _t.TxnType = _convertTransactionType(_getValueFromXML(_element, "TxnType"));
                                _t.Route = _getValueFromXML(_element, "Route");
                                _t.Balance = _convertToDecimal(_getValueFromXML(_element, "Balance"));
                                _t.Amount = _convertToDecimal(_getValueFromXML(_element, "Amount"));
                                _t.OrigZone = _getValueFromXML(_element, "OrigZone");
                                _t.OrigZoneName = _convertZoneToName(_t.OrigZone);
                                _t.DestZone = _getValueFromXML(_element, "DestZone");
                                _t.DestZonName = _convertZoneToName(_t.DestZone);
                                _t.TravelCard = _mainpage.txtTravelCard.Text;
                                _t.MainPageViewModel = this;

                                if (_data.ToUpper() != "CASE")
                                {
                                    _t.CommandAdd.IsEnabled = false;
                                    _t.CommandDelete.IsEnabled = false;
                                }
                                else
                                {
                                    _t.CommandAdd.IsEnabled = true;
                                    _t.CommandDelete.IsEnabled = true;
                                }
                                _transactionList.Add(_t);
                            }
                        }
                    }

                    _mainpage.grdGridSearch.ItemsSource = null;
                    _mainpage.grdGridSearch.ItemsSource = _transactionList.OrderByDescending(x => x.Date);
                }
                else
                {
                    _mainpage.grdGridSearch.ItemsSource = null;
                }

                IsWaiting = false;
            }
            catch (Exception ex)
            {
                IsWaiting = false;
                _mainpage.ShowError(ex, "_travelCardClient_GetCardTransactionsCompleted");
            }
            finally
            {
                _travelCardClient.GetCardTransactionsCompleted -= _travelCardClient_GetCardTransactionsCompleted;
            }
        }

        // *** Commands ***

        private XElement _getInformationNodeFromXML(XDocument xdoc, string localeName)
        {
            XElement _returnValue = null;
            try
            {
                XElement _pursedetails = xdoc.Descendants().Where(p => p.Name.LocalName == localeName).FirstOrDefault();
                _returnValue = _pursedetails;
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_getInformationNodeFromXML");
            }
            return _returnValue;
        }

        private string _getValueFromXML(XElement xmldoc, string node)
        {
            string _returnValue = "";

            try
            {
                var _value = from _c in xmldoc.Descendants() where _c.Name.LocalName == node select _c.Value;
                if (_value != null)
                    _returnValue = _value.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "_getValueFromXML");
            }

            return _returnValue;
        }

        private void _travelCardClient_GetCardDetailsCompleted(object sender, GetCardDetailsCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {

                    GetCardDetailsResponse _response = e.Result as GetCardDetailsResponse;

                    if (!string.IsNullOrEmpty(_response.ErrorMessage))
                    {
                        IsWaiting = false;
                        _mainpage.ShowMessage(_response.ErrorMessage, "_travelCardClient_GetCardDetailsCompleted");
                        return;
                    }

                    _travelcard = new travelCard();

                    if (e.Result != null)
                    {
                        string _result = _response.CardDetails;
                        XDocument _xdoc = XDocument.Parse(_result);

                        XElement _cardinfo = _getInformationNodeFromXML(_xdoc, "CardInformation");
                        if (_cardinfo != null)
                        {
                            Travelcard.CardHotlisted = _convertTrueFalse(_getValueFromXML(_cardinfo, "CardHotlisted"));
                        }

                        XElement _pursedetails = _getInformationNodeFromXML(_xdoc, "PurseDetails");
                        if (_pursedetails != null)
                        {
                            Travelcard.PurseBalance = _getValueFromXML(_pursedetails, "Balance");
                            Travelcard.PurseOutstandingDirectedAutoload = _convertTrueFalse(_getValueFromXML(_pursedetails, "OutstandingDirectedAutoload"));
                            Travelcard.PurseHotlisted = _convertTrueFalse(_getValueFromXML(_pursedetails, "Hotlisted"));
                            Travelcard.PurseHotlistReason = _getValueFromXML(_pursedetails, "HotlistReason");
                        }

                        XElement _perioddetails = _getInformationNodeFromXML(_xdoc, "PeriodDetails");
                        if (_perioddetails != null)
                        {
                            Travelcard.PeriodCardCategory = _convertPeriodCardCategory(_getValueFromXML(_perioddetails, "CardCategory"));
                            Travelcard.PeriodProductType = _getValueFromXML(_perioddetails, "ProductType");
                            Travelcard.PeriodPeriodStart = _getValueFromXML(_perioddetails, "PeriodStart");
                            Travelcard.PeriodPeriodEnd = _getValueFromXML(_perioddetails, "PeriodEnd");
                            Travelcard.PeriodWaitingPeriods = _getValueFromXML(_perioddetails, "WaitingPeriods");
                            Travelcard.PeriodPricePaid = _convertAmount(_getValueFromXML(_perioddetails, "PricePaid"));
                            Travelcard.PeriodOutstandingEnableThresholdAutoload = _convertTrueFalse(_getValueFromXML(_perioddetails, "OutstandingEnableThresholdAutoload"));
                            Travelcard.PeriodHotlisted = _convertTrueFalse(_getValueFromXML(_perioddetails, "Hotlisted"));
                        }

                        IEnumerable<XElement> _zoneList = _getZonesListNodesFromXML(_xdoc, "ZoneLists");
                        if (_zoneList != null)
                        {
                            Travelcard.ZoneList = new ObservableCollection<zoneEXT>();

                            foreach (XElement _element in _zoneList)
                            {
                                zoneEXT _zonename = new zoneEXT();
                                _zonename.ZoneListID = _getValueFromXML(_element, "ZoneListID");
                                _zonename.Zone = _getValueFromXML(_element, "Zone");
                                _zonename.ZoneCaption = _convertZoneToName(_zonename.Zone);

                                Travelcard.ZoneList.Add(_zonename);
                            }
                        }

                        IEnumerable<XElement> _routeList = _getZonesListNodesFromXML(_xdoc, "RouteLists");
                        if (_routeList != null)
                        {
                            Travelcard.RouteList = new ObservableCollection<routeEXT>();
                            foreach (XElement _element in _routeList)
                            {
                                routeEXT _routeName = new routeEXT();
                                _routeName.RouteListID = _getValueFromXML(_element, "RouteListID");
                                _routeName.Route = _getValueFromXML(_element, "Route");
                                _routeName.FromZone = _getValueFromXML(_element, "FromZone");
                                _routeName.ToZone = _getValueFromXML(_element, "ToZone");
                                _routeName.RouteCaption = _convertRouteToName(_routeName.Route);
                                Travelcard.RouteList.Add(_routeName);
                            }
                        }
                    }

                    _mainpage.CardViewer.DataContext = null;
                    _mainpage.CardViewer.DataContext = Travelcard;
                    _mainpage.PurseViewer.DataContext = null;
                    _mainpage.PurseViewer.DataContext = Travelcard;
                    _mainpage.PeriodViewer.DataContext = null;
                    _mainpage.PeriodViewer.DataContext = Travelcard;
                    _mainpage.ZoneViewer.DataContext = null;
                    _mainpage.ZoneViewer.DataContext = Travelcard;
                    _mainpage.RouteViewer.DataContext = null;
                    _mainpage.RouteViewer.DataContext = Travelcard;

                    //Search for travelcard in crm.
                    if (!string.IsNullOrEmpty(_mainpage.txtTravelCard.Text))
                    {
                        string _card = _mainpage.txtTravelCard.Text;
                        _travelCardClient.GetCardFromCRMExtendedCompleted += _travelCardClient_GetCardFromCRMExtendedCompleted;
                        _travelCardClient.GetCardFromCRMExtendedAsync(_card);
                    }
                    else
                    {
                        IsWaiting = false;
                    }
                }
                else
                {
                    _mainpage.CardViewer.DataContext = null;
                    _mainpage.PurseViewer.DataContext = null;
                    _mainpage.PeriodViewer.DataContext = null;
                    _mainpage.ZoneViewer.DataContext = null;
                    IsWaiting = false;
                }
            }
            catch (Exception ex)
            {
                IsWaiting = false;
                _mainpage.ShowError(ex, "_travelCardClient_GetCardDetailsCompleted");
            }
            finally
            {
                _travelCardClient.GetCardDetailsCompleted -= _travelCardClient_GetCardDetailsCompleted;
            }
        }

        private void _travelCardClient_GetCardFromCRMExtendedCompleted(object sender, GetCardFromCRMExtendedCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    GetCRMCardDetailsResponse _response = e.Result as GetCRMCardDetailsResponse;

                    if (!string.IsNullOrEmpty(_response.ErrorMessage))
                    {
                        IsWaiting = false;
                        _mainpage.ShowMessage(_response.ErrorMessage, "_travelCardClient_GetCardFromCRMExtendedCompleted");
                        return;
                    }

                    if (_response.Card == null)
                    {
                        _mainpage.CRMCardViewer.DataContext = null;
                        GetOutstandingCharges();
                        return;
                    }

                    if (e.Result != null)
                    {
                        CardExt = new cardEXT(_crmManager);
                        CardExt.Clone(_response.Card);

                        _mainpage.CRMCardViewer.DataContext = null;
                        _mainpage.CRMCardViewer.DataContext = CardExt;
                        GetOutstandingCharges();
                    }
                }
                else
                {
                    _mainpage.CRMCardViewer.DataContext = null;
                }

                if (!string.IsNullOrEmpty(_data) && _data.ToUpper().ToString() == "CASE")
                    _getSavedTravelCardTransactions();

                if (!string.IsNullOrEmpty(_data) && _data.ToUpper().ToString() == "TRAVELCARD")
                    _dispatcher.BeginInvoke(() => { IsWaiting = false; });

            }
            catch (Exception ex)
            {
                IsWaiting = false;
                _mainpage.ShowError(ex, "_travelCardClient_GetCardFromCRMExtendedCompleted");
            }
            finally
            {
                _travelCardClient.GetCardFromCRMExtendedCompleted -= _travelCardClient_GetCardFromCRMExtendedCompleted;
            }
        }

        private void GetOutstandingCharges()
        {
            try
            {
                if (!string.IsNullOrEmpty(_mainpage.txtTravelCard.Text))
                {
                    string _card = _mainpage.txtTravelCard.Text;
                    _travelCardClient.GetGetOutstandingChargesCompleted += _travelCardClient_GetGetOutstandingChargesCompleted;
                    _travelCardClient.GetGetOutstandingChargesAsync(_card);
                }
                else
                {
                    _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                }
            }
            catch (Exception ex)
            {
                IsWaiting = false;
                _mainpage.ShowError(ex, "GetOutstandingCharges");
            }
        }

        private void _travelCardClient_GetGetOutstandingChargesCompleted(object sender, GetGetOutstandingChargesCompletedEventArgs e)
        {
            try
            {
                GetOutstandingChargesResponse _response = e.Result as GetOutstandingChargesResponse;

                if (!string.IsNullOrEmpty(_response.ErrorMessage))
                {
                    IsWaiting = false;
                    _mainpage.ShowMessage(_response.ErrorMessage, "_travelCardClient_GetGetOutstandingChargesCompleted");
                    return;
                }

                if (e.Result != null)
                {
                    _outstandingCharges = new OutstandingCharges();
                    string _result = _response.OutstandingCharges;
                    XDocument _xdoc = XDocument.Parse(_result);
                    XElement _outstandingChargesResponse = _getInformationNodeFromXML(_xdoc, "OutstandingChargesResponse");
                    _outstandingCharges.Message = _getValueFromXML(_outstandingChargesResponse, "Message");
                    if (_getValueFromXML(_outstandingChargesResponse, "HasOutstandingCharge") == "" || _getValueFromXML(_outstandingChargesResponse, "HasOutstandingCharge") == null)
                        _outstandingCharges.HasOutstandingCharge = false;
                    else
                        _outstandingCharges.HasOutstandingCharge = Convert.ToBoolean(_getValueFromXML(_outstandingChargesResponse, "HasOutstandingCharge"));

                    if (_getValueFromXML(_outstandingChargesResponse, "HasExpiredCharge") == "" || _getValueFromXML(_outstandingChargesResponse, "HasExpiredCharge") == null)
                        _outstandingCharges.HasExpiredCharge = false;
                    else
                        _outstandingCharges.HasExpiredCharge = Convert.ToBoolean(_getValueFromXML(_outstandingChargesResponse, "HasExpiredCharge"));
                    
                    _outstandingCharges.Amount = _getValueFromXML(_outstandingChargesResponse, "Amount");
                    _outstandingCharges.ErrorMessage = _getValueFromXML(_outstandingChargesResponse, "ErrorMessage");

                    if (_getValueFromXML(_outstandingChargesResponse, "StatusCode") == "" || _getValueFromXML(_outstandingChargesResponse, "StatusCode") == null)
                        _outstandingCharges.StatusCode = 0;
                    else
                        _outstandingCharges.StatusCode = Convert.ToInt32(_getValueFromXML(_outstandingChargesResponse, "StatusCode"));

                    _mainpage.OutstandingChargesViewer.DataContext = null;
                    _mainpage.OutstandingChargesViewer.DataContext = _outstandingCharges;

                    if (_outstandingCharges.HasExpiredCharge == true)
                        _mainpage.btnoutstandingCharges.IsEnabled = true;
                    else
                        _mainpage.btnoutstandingCharges.IsEnabled = false;

                }
                else
                {
                    _mainpage.OutstandingChargesViewer.DataContext = null;
                }
            }
            catch (Exception ex)
            {
                IsWaiting = false;
                _mainpage.ShowError(ex, "_travelCardClient_GetGetOutstandingChargesCompleted");
            }
            finally
            {
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _travelCardClient.GetGetOutstandingChargesCompleted -= _travelCardClient_GetGetOutstandingChargesCompleted;
            }
        }

        private void _getSavedTravelCardTransactions()
        {
            try
            {
                if (!string.IsNullOrEmpty(_id) && _data.ToUpper() == "CASE")
                {
                    _dispatcher.BeginInvoke(() => { IsWaiting = true; });
                    if (_travelCardClient != null)
                    {
                        _dispatcher.BeginInvoke(() => { 
                            _travelCardClient.GetTravelCardTransactionsCompleted += _travelCardClient_GetTravelCardTransactionsCompleted;
                            _travelCardClient.GetTravelCardTransactionsAsync(_id);
                        });
                    }
                    else
                    {
                        _setLoadingFalse();
                        _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                    }
                }
                else
                {
                    _setLoadingFalse();
                    _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                }
            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _dispatcher.BeginInvoke(() => { IsWaiting = false; });
                _mainpage.ShowError(ex, "_getSavedTravelCardTransactions");
            }

        }

        private void _travelCardClient_GetTravelCardTransactionsCompleted(object sender, GetTravelCardTransactionsCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    GetTravelCardTransactionsResponse _response = e.Result as GetTravelCardTransactionsResponse;

                    if (!string.IsNullOrEmpty(_response.ErrorMessage))
                    {
                        _setLoadingFalse();
                        IsWaiting = false;
                        _mainpage.ShowMessage(_response.ErrorMessage, "_travelCardClient_GetTravelCardTransactionsCompleted");
                        return;
                    }

                    if (_response.TravelCardTransactions == null)
                    {
                        _mainpage.lstSelection.ItemsSource = null;
                        _getIncidentStatus(_id);
                        return;
                    }

                    if (e.Result != null)
                    {
                        _saveList = new ObservableCollection<travelCardTransaction>();
                        foreach (TravelCardTransaction _t in _response.TravelCardTransactions)
                        {
                            travelCardTransaction _tran = new travelCardTransaction();
                            _tran.CardSect = _t.CardSect;

                            try
                            {
                                string _d = string.Format("{0}T{1}:00", _t.Date, _t.Time);
                                DateTime _dt;
                                DateTime.TryParse(_d, out _dt);
                                _tran.Date = _dt.ToString();
                            }
                            catch
                            {
                                _tran.Date = "";
                            }

                            _tran.ShowDate = _t.Date;
                            _tran.ShowTime = _t.Time;
                            _tran.DeviceID = _t.DeviceID;
                            _tran.MainPageViewModel = this;
                            _tran.OrigZone = _t.OrigZone;
                            _tran.RecType = _t.RecType;
                            _tran.Route = _t.Route;
                            _tran.Transactionid = _t.Transactionid;
                            _tran.TxnType = _t.TxnType;
                            _tran.TravelCard = _t.TravelCard;
                            _tran.Amount = _t.Amount;
                            _tran.OrigZoneName = _t.OrigZoneName;


                            if (_data.ToUpper() != "CASE")
                            {
                                _tran.CommandAdd.IsEnabled = false;
                                _tran.CommandDelete.IsEnabled = false;
                            }
                            else
                            {
                                _tran.CommandAdd.IsEnabled = true;
                                _tran.CommandDelete.IsEnabled = true;
                            }

                            _saveList.Add(_tran);
                        }

                        _mainpage.lstSelection.ItemsSource = null;
                        _mainpage.lstSelection.ItemsSource = _saveList.OrderBy(x => x.ShowDate).ThenBy(x => x.ShowTime);
                        _getIncidentStatus(_id);
                    }
                    else
                    {
                        _getIncidentStatus(_id);
                    }
                }
                else
                {
                    _mainpage.lstSelection.ItemsSource = null;
                    _getIncidentStatus(_id);
                }

            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _mainpage.ShowError(ex, "_travelCardClient_GetTravelCardTransactionsCompleted");
            }
            finally
            {
                _travelCardClient.GetTravelCardTransactionsCompleted -= _travelCardClient_GetTravelCardTransactionsCompleted;
            }
        }

        private void _getIncidentStatus(string id)
        {
            try
            {
                _crmManager.Fetch<incident>(_xmlgetIncidentStatus(id), _getIncidentStatus_callback);
            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _mainpage.ShowError(ex, "_getIncidentStatus");
            }
        }

        private void _getIncidentStatus_callback(ObservableCollection<incident> result)
        {
            try
            {
                if (result != null && result.Count > 0)
                {
                    incident _case = result[0] as incident;

                    _dispatcher.BeginInvoke(() => {
                        _mainpage.txtData.Text = string.Format("Statecode : {0}, Statuscode : {1}", _case.Statecode, _case.Statuscode);
                    });

                    bool _readonly = false;
                    if (_case.Statecode == 1 && _case.Statuscode == 5)
                        _readonly = true;

                    if (_case.Statecode == 0 && _case.Statuscode == 1)
                    {
                        if (_case.Ownerid != _user.SystemUserId)
                            _readonly = true;
                    }
                    
                    if (_readonly == true)
                    {
                        if (_saveList != null && _saveList.Count > 0)
                        {
                            foreach (travelCardTransaction _tran in _saveList)
                            {
                                _dispatcher.BeginInvoke(() => { _tran.CommandDelete.IsEnabled = false; });
                            }
                        }

                        _dispatcher.BeginInvoke(() =>
                        {
                            _mainpage.btnSearchCard.IsEnabled = false;
                            _mainpage.btnSearchCardTransactions.IsEnabled = false;
                            _mainpage.txtSearchTransactionsFrom.IsEnabled = false;
                            _mainpage.txtSearchTransactionsTo.IsEnabled = false;
                            _mainpage.txtTravelCard.IsEnabled = false;
                        });
                    }
                }

                _setLoadingFalse();

            }
            catch (Exception ex)
            {
                _setLoadingFalse();
                _mainpage.ShowError(ex, "_getIncidentStatus_callback");
            }
            finally
            {
                _setLoadingFalse();
            }
        }

        private string _xmlgetIncidentStatus(string id)
        {
            string _xml = "";

            _xml += "<fetch distinct='false' mapping='logical' version='1.0'>";
            _xml += "<entity name='incident'>";
            _xml += "<attribute name='statecode'/>";
            _xml += "<attribute name='statuscode'/>";
            _xml += "<attribute name='ownerid'/>";
            _xml += "<filter type='and'>";
            _xml += "<condition attribute='incidentid' value='" + id + "' operator='eq'/>";
            _xml += "</filter>";
            _xml += "</entity>";
            _xml += "</fetch>";

            return _xml;
        }

        private string _convertCardKInd(string kind)
        {
            string _returnValue = "";

            if (string.IsNullOrEmpty(kind))
                return _returnValue;

            if (kind == "0")
                _returnValue = "Odefinierat";
            else if (kind == "1")
                _returnValue = "Magnetkort";
            else if (kind == "4")
                _returnValue = "Oregistrerat kort";
            else
                _returnValue = "Odefinierat";

            return _returnValue;
        }

        private string _convertPurseCardCategory(string category)
        {
            string _returnValue = "";

            if (string.IsNullOrEmpty(category))
                return _returnValue;

            if (category == "0")
                _returnValue = "Odefinierat";
            else if (category == "4")
                _returnValue = "Värdekort";
            else
                _returnValue = "Odefinierat";

            return _returnValue;
        }

        private string _convertPeriodCardCategory(string category)
        {
            string _returnValue = "";

            if (string.IsNullOrEmpty(category))
                return _returnValue;

            if (category == "0")
                _returnValue = "Odefinierat";
            else if (category == "2")
                _returnValue = "Värdekort";
            else if (category == "5")
                _returnValue = "Limit period cards";
            else if (category == "6")
                _returnValue = "Period cards with waiting periods";
            else if (category == "7")
                _returnValue = "Limited period cards with waiting periods";
            else
                _returnValue = "Odefinierat";

            return _returnValue;
        }

        private string _convertTrueFalse(string truefalse)
        {
            string _returnValue = "";

            if (string.IsNullOrEmpty(truefalse))
                return _returnValue;

            if (truefalse.ToLower() == "true")
                _returnValue = "Ja";
            else
                _returnValue = "Nej";

            return _returnValue;
        }

        private string _convertCardSect(string cardsect)
        {
            string _returnValue = "";

            if (string.IsNullOrEmpty(cardsect))
                return _returnValue;

            if (cardsect == "0")
                _returnValue = "Hela kortet";
            else if (cardsect == "1")
                _returnValue = "Periodkort";
            else if (cardsect == "2")
                _returnValue = "Reskassa";
            else
                _returnValue = "Hela kortet";

            return _returnValue;
        }

        private string _convertTransactionRecordType(string recordtype)
        {
            string _returnValue = "";

            if (string.IsNullOrEmpty(recordtype))
                return _returnValue;
            if (recordtype == "70")
                _returnValue = "Reskassa";
            else if (recordtype == "71")
                _returnValue = "Value Payment";
            else if (recordtype == "72")
                _returnValue = "Periodkort";
            else if (recordtype == "74")
                _returnValue = "Övergång";
            else if (recordtype == "75")
                _returnValue = "Övergång";
            else if (recordtype == "78")
                _returnValue = "Inspektion";
            else if (recordtype == "79")
                _returnValue = "Activating of Waiting Period";
            else if (recordtype == "80")
                _returnValue = "Laddning Reskassa";
            else if (recordtype == "81")
                _returnValue = "Laddning Periodkort";
            else if (recordtype == "91")
                _returnValue = "Use of Card on hotlist";
            else
                _returnValue = "Reskassa";

            return _returnValue;
        }

        private string _convertTransactionType(string type)
        {
            string _returnValue = "";

            if (string.IsNullOrEmpty(type))
                return _returnValue;

            if (type == "0")
                _returnValue = "Ordinarie";
            else if (type == "1")
                _returnValue = "Utställning";
            else if (type == "2")
                _returnValue = "Refund";
            else if (type == "3")
                _returnValue = "Hämtad laddning Reskassa";
            else if (type == "4")
                _returnValue = "Hämtad Autoladda Period";
            else if (type == "+16")
                _returnValue = "Cancellation";
            else if (type == "+32")
                _returnValue = "Card Write Error";
            else if (type == "+64")
                _returnValue = "Walk Away";
            else if (type == "+128")
                _returnValue = "Info";
            else
                _returnValue = "Ordinarie";

            return _returnValue;
        }

        private string _convertZoneToName(string zonename)
        {
            string _returnValue = "";

            try
            {
                Zone _caption = _zoneNameList.FirstOrDefault(x => x.ZoneId == zonename);
                if (_caption != null)
                    _returnValue = _caption.ZoneName;
                else
                    _returnValue = zonename;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _returnValue;
        }

        private string _convertRouteToName(string routename)
        {
            return routename;
        }

        private string _convertAmount(string amount)
        {
            string _returnValue = "";

            if (!string.IsNullOrEmpty(amount))
            {
                string _last = amount.Substring(amount.Length - 2, 2);
                string _first = amount.Substring(0, amount.Length - 2);
                _returnValue = string.Format("{0}.{1}", _first, _last);
            }

            return _returnValue;
        }

        private string _convertToDecimal(string amount)
        {
            string _returnValue = "";

            if (!string.IsNullOrEmpty(amount))
            {
                if (amount.IndexOf(",") > 0 || amount.IndexOf(".") > 0)
                {
                    _returnValue = amount;
                }
                else
                {
                    _returnValue = string.Format("{0}.00", amount);
                }
            }

            return _returnValue;
        }



    }

    public class DecimailConverter : IValueConverter
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
                    _svalue = _svalue.Replace(".", ",");
                    decimal _dvalue = 0.0M;
                    decimal.TryParse(_svalue, out _dvalue);
                    _returnValue = _dvalue.ToString("0.00");
                }
            }

            return _returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
