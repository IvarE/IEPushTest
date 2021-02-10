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

using CRM.Travel.Information.Classes;
using CRM.Travel.Information.Views;
using CGIXrm;
using CGIXrm.CrmSdk;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using System.Windows.Data;
using System.ServiceModel;

using CRM.Travel.Information.PubTrans;
using System.Windows.Browser;
using System.Text.RegularExpressions;

namespace CRM.Travel.Information.ViewModel {

  // address to server http://10.16.229.201:4001/ExtConnectorService.svc


  // CRM.Travel.InformationPage.html?serveraddress=http://dev03/Skanetrafiken&id=70BB8646-149E-E311-93F2-00155D0D0417&OrgLcId=1053

  // CRM.Travel.InformationPage.html?serveraddress=http://V-DKCRM-TST/Skanetrafiken&debug=true&userlcid=1053&ActionDate=2013-12-13T11:00:00&id=2DCD8D54-D27E-E411-80D4-005056902292

  // CRM.Travel.InformationPage.html?serveraddress=http://V-DKCRM-TST/Skanetrafiken&debug=true&userlcid=1053&ActionDate=2015-01-14T11:00:00&id=2DCD8D54-D27E-E411-80D4-005056902292

  // CRM.Travel.InformationPage.html?serveraddress=http://V-DKCRM-UTV/Skanetrafiken&debug=true&userlcid=1053&ActionDate=2015-01-14T11:00:00&id=C7CC7E0F-AD06-E411-80D1-0050569010AD

  // CRM.Travel.InformationPage.html?serveraddress=http://V-DKCRM-UTV/Skanetrafiken&debug=true&userlcid=1053&ActionDate=2014-12-29T07:00:00&id=C7CC7E0F-AD06-E411-80D1-0050569010AD

  // CRM.Travel.InformationPage.html?serveraddress=http://V-DKCRM-TST/Skanetrafiken&debug=true&userlcid=1053&ActionDate=2015-01-21T07:00:00&id=B45C9D80-ECB1-E411-80DA-005056902292

  // My incident
  // CRM.Travel.InformationPage.html?serveraddress=http://V-DKCRM-UTV/Skanetrafiken&debug=true&userlcid=1053&ActionDate=2015-01-21T07:00:00&id=C7CC7E0F-AD06-E411-80D1-0050569010AD

  //read only incident
  // CRM.Travel.InformationPage.html?serveraddress=http://V-DKCRM-UTV/Skanetrafiken&debug=true&userlcid=1053&ActionDate=2015-01-21T07:00:00&id=6C26A2BA-4507-E411-80D1-0050569010AD

  //closed incident
  // CRM.Travel.InformationPage.html?serveraddress=http://V-DKCRM-UTV/Skanetrafiken&debug=true&userlcid=1053&ActionDate=2015-01-21T07:00:00&id=6E71C836-B374-E411-80D4-0050569010AD



  /*
	  Train       = CBO_MODEOFTRANSPORT_TRAIN_CAPTION         : ID 1
	  CityBus     = CBO_MODEOFTRANSPORT_CITYBUS_CAPTION       : ID 2
	  RegionBus   = CBO_MODEOFTRANSPORT_REGIONBUS_CAPTION     : ID 3
  */


  // http://v-dkcrm-utv/Skanetrafiken/main.aspx?etc=112&extraqs=&histKey=476493428&
  // id=%7bBE3FEFCC-30F6-E311-80CE-0050569010AD
  // %7d&newWindow=true&pagetype=entityrecord

  // http://v-dkcrm-tst/Skanetrafiken/main.aspx?etc=112&extraqs=&histKey=918503041&
  // id=%7b2DCD8D54-D27E-E411-80D4-005056902292
  // %7d&newWindow=true&pagetype=entityrecord

  // http://v-dkcrm-tst/Skanetrafiken/main.aspx?etc=112&extraqs=&histKey=581805537&
  // id=%7b
  // 2DCD8D54-D27E-E411-80D4-005056902292
  // %7d&newWindow=true&pagetype=entityrecord

  // http://v-dkcrm-utv/Skanetrafiken/main.aspx?etc=112&extraqs=&histKey=788028444&
  // id=%7b
  // C7CC7E0F-AD06-E411-80D1-0050569010AD
  // %7d&newWindow=true&pagetype=entityrecord#878572119


  public class MainPageViewModel {

	const string LocalizedLabelGroup = "SILVERLIGHT_TRAVEL_INFO";
	string WaitCaption = "Wait...";

	WebParameters _webparams;
	CrmManager _crmManager;
	MainPage _mainpage;
	Dispatcher _theDispatcher;
	FetchQueries _fetch;
	Header _header;
	SystemUserSettings _user;

	webservice _webservice;
	CRM.Travel.Information.PubTrans.PubTransServiceClient _pubtransClient;

	string _id;

	ObservableCollection<CRM.Travel.Information.Classes.TransportType> _modeOfTransportTypeList;
	List<OrganisationalUnit> _organisationalUnitList = new List<OrganisationalUnit>();
	List<Contractor> _contractorList = new List<Contractor>();

	ObservableCollection<TravelInformation> _selectionList;

	string _MSGBOX_INFO_NO_SELECTION;

	string _MSGBOX_INFO_CAPTION;
	string _MSGBOX_INFO_NO_CITYSELECTION;
	string _MSGBOX_INFO_NO_TOURSELECTION;
	string _MSGBOX_INFO_NO_FROMSELECTION;
	string _MSGBOX_INFO_NO_TOSELECTION;

	string strActionDate;
	string _debug;

	public string Version {
	  get { return System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1].Trim(); }
	}

	public void GetSavedLines(string id) {
	  try {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = true; });
		_id = id;
		_crmManager.Fetch<crmtravel>(_xmlGetSavedLines(id), GetSavedLines_callback);
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "GetSavedLines");
	  }
	}

	private void GetSavedLines_callback(ObservableCollection<crmtravel> result) {
	  try {
		_selectionList = new ObservableCollection<TravelInformation>();
		foreach (crmtravel _t in result) {
		  TravelInformation _travel = new TravelInformation() {
			TravelInformationId = _t.TravelInformationId.ToString(),
			Name = _t.Name,
			Transport = _t.Transport,
			Tour = _t.Tour,
			Stop = _t.Stop,
			Start = _t.Start,
			StartPlanned = _t.GetDateStartplanned,
			StartActual = _t.Startactual,
			Line = _t.Line,
			DirectionText = _t.Directiontext,
			Contractor = _t.Contractor,
			City = _t.City,
			CaseId = _t.Caseid.ToString(),
			ArivalPlanned = _t.GetDateArivalplanned,
			ArivalActual = _t.Arivalactual,
			DeviationMessage = _t.Deviationmessage,
			DisplayText = _t.Displaytext,
			MainPageViewModel = this
		  };


		  _selectionList.Add(_travel);
		}

		_refreshSelectionBinding();
		_getUser();
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "GetSavedLines_callback");
	  }
	  finally {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
	  }
	}

	private string _xmlGetSavedLines(string id) {
	  string _xml = "";

	  _xml += "<fetch distinct='false' mapping='logical' version='1.0'>";
	  _xml += "   <entity name='cgi_travelinformation'>";
	  _xml += "       <attribute name='cgi_travelinformationid'/>";
	  _xml += "       <attribute name='cgi_travelinformation'/>";
	  _xml += "       <attribute name='cgi_transport'/>";
	  _xml += "       <attribute name='cgi_tour'/>";
	  _xml += "       <attribute name='cgi_stop'/>";
	  _xml += "       <attribute name='cgi_start'/>";
	  _xml += "       <attribute name='cgi_startplanned'/>";
	  _xml += "       <attribute name='cgi_startactual'/>";
	  _xml += "       <attribute name='cgi_line'/>";
	  _xml += "       <attribute name='cgi_directiontext'/>";
	  _xml += "       <attribute name='cgi_contractor'/>";
	  _xml += "       <attribute name='cgi_city'/>";
	  _xml += "       <attribute name='cgi_caseid'/>";
	  _xml += "       <attribute name='cgi_arivalplanned'/>";
	  _xml += "       <attribute name='cgi_arivalactual'/>";
	  _xml += "       <attribute name='cgi_deviationmessage'/>";
	  _xml += "       <attribute name='cgi_displaytext'/>";
	  _xml += "       <filter type='and'>";
	  _xml += "           <condition attribute='cgi_caseid' value='" + id + "' operator='eq'/>";
	  _xml += "       </filter>";
	  _xml += "   </entity>";
	  _xml += "</fetch>";

	  return _xml;
	}

	private void _getUser() {
	  try {
		_crmManager.GetCurrentUser(_getUser_callback);
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_getUser");
	  }
	}

	private void _getUser_callback(ObservableCollection<SystemUserSettings> result) {
	  try {
		_user = result[0] as SystemUserSettings;

		if (!string.IsNullOrEmpty(_id)) {
		  _getStatusOfIncident(_id);
		}
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_getUser_callback");
	  }
	}

	private void _getStatusOfIncident(string id) {
	  try {
		_crmManager.Fetch<incident>(_xmlgetStatusOfIncident(id), _getStatusOfIncident_callback);
	  }
	  catch (Exception ex) {
		_setLoadingFalse();
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "_getStatusOfIncident");
	  }
	}

	private void _getStatusOfIncident_callback(ObservableCollection<incident> result) {
	  try {
		if (result != null && result.Count > 0) {
		  incident _case = result[0] as incident;

		  bool _readonly = false;
		  //Close incident
		  if (_case.Statecode == 1 && _case.Statuscode == 5)
			_readonly = true;

		  if (_case.Statecode == 0 && _case.Statuscode == 1) {
			if (_user.SystemUserId != _case.Ownerid)
			  _readonly = true;
		  }

		  if (_readonly == true) {
			if (_selectionList.Count > 0) {
			  foreach (TravelInformation _t in _selectionList) {
				_theDispatcher.BeginInvoke(() => { _t.CommandDelete.IsEnabled = false; });
			  }
			}

			_theDispatcher.BeginInvoke(() => {
			  _mainpage.btnSearch.IsEnabled = false;
			  _mainpage.btnAfter.IsEnabled = false;
			  _mainpage.btnBefore.IsEnabled = false;
			  _mainpage.cboTransport.IsEnabled = false;
			});
		  }
		}
	  }
	  catch (Exception ex) {
		_setLoadingFalse();
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "_getStatusOfIncident_callback");
	  }
	}

	private string _xmlgetStatusOfIncident(string id) {
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

	public MainPageViewModel(MainPage mainpage, Dispatcher theDispatcher, string querystring, string initparams) {
	  try {
		strActionDate = _getKey(querystring, "ActionDate");
		_debug = _getKey(querystring, "debug");

		_selectionList = new ObservableCollection<TravelInformation>();

		_theDispatcher = theDispatcher;
		_header = new Header();
		_mainpage = mainpage;
		_mainpage.DataContext = _header;
		_header.IsWaiting = true;
		_header.IsLoadingWaiting = true;

		_mainpage.Version.Text = this.Version;
		_fetch = new FetchQueries();
		_webparams = new WebParameters();
		_id = _webparams.Id;

		_crmManager = new CrmManager(_webparams.ServerAddress);

		if (!string.IsNullOrEmpty(_debug) || _debug == "true") {
		  if (!string.IsNullOrEmpty(_id))
			GetSavedLines(_id);
		}

		_webservice = new webservice();
		_webservice.Callback_completed = new Action(_setSettings_completed);
		_webservice.CrmManager = _crmManager;
		_webservice.Init();

		_mainpage.cboTransport.SelectionChanged += cboTransport_SelectionChanged;



		_mainpage.cboCity.SelectionChanged += cboCity_SelectionChanged;
		_mainpage.cboTour.SelectionChanged += cboTour_SelectionChanged;
		_mainpage.cboFrom.SelectionChanged += cboFrom_SelectionChanged;

		_mainpage.btnSearch.Click += btnSearch_Click;
		_mainpage.btnBefore.Click += btnBefore_Click;
		_mainpage.btnAfter.Click += btnAfter_Click;

		_mainpage.grdSearch.KeyDown += grdSearch_KeyDown;


		if (!string.IsNullOrEmpty(strActionDate))
		  _mainpage.txtActionDate.Text = strActionDate.Replace("T", " ");

	  }
	  catch (Exception ex) {
		_setLoadingFalse();
		_mainpage.ShowError(ex, "MainPageViewModel");
	  }
	}

	private void grdSearch_KeyDown(object sender, KeyEventArgs e) {
	  try {
		if (e.Key == Key.A) {
		  _theDispatcher.BeginInvoke(() => { _mainpage.grdSearch.IsEnabled = false; });
		  TravelInformation _t = ((DataGrid)sender).SelectedItem as TravelInformation;
		  AddToSelection(_t);
		}

		if (e.Key == Key.Q) {
		  _doSearch(-1);
		}

		if (e.Key == Key.P) {
		  _doSearch(1);
		}

		if (e.Key == Key.R) {
		  if (_mainpage.cboTransport.Items.Count() > 0) {
			_mainpage.cboTransport.SelectedIndex = 0;
			_mainpage.cboTransport.Focus();
		  }
		}
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _mainpage.grdSearch.IsEnabled = true; });
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "MainPageViewModel");
	  }
	}

	private void _setLoadingFalse() {
	  _theDispatcher.BeginInvoke(() => {
		_header.IsLoadingWaiting = false;
		_mainpage.LoadingRoot.Visibility = Visibility.Collapsed;
	  });
	}

	private string _getKey(string queryparams, string key) {
	  string _returnValue = "";

	  string[] _keys = Regex.Split(queryparams, "&");
	  foreach (string _key in _keys) {
		string[] _values = Regex.Split(_key, "=");
		if (_values[0].ToString() == key)
		  _returnValue = _values[1].ToString();
	  }

	  return _returnValue;
	}

	private void btnAfter_Click(object sender, RoutedEventArgs e) {
	  try {
		_doSearch(1);
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "btnAfter_Click");
	  }
	}

	private void btnBefore_Click(object sender, RoutedEventArgs e) {
	  try {
		_doSearch(-1);
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "btnBefore_Click");
	  }
	}

	private void btnSearch_Click(object sender, RoutedEventArgs e) {
	  try {
		_mainpage.txtActionDate.Text = _getActionDate("cgi_actiondate");
		if (!string.IsNullOrEmpty(_mainpage.txtActionDate.Text))
		  _mainpage.txtActionDate.Text = _mainpage.txtActionDate.Text.Replace("T", " ");

		strActionDate = _getActionDate("cgi_actiondate");
		_doSearch(0);
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "btnSearch_Click");
	  }
	}

	private string _getActionDate(string attributeName) {
	  string _returnValue = "";

	  _returnValue = string.Format("{0}-{1}-{2}T{3}:{4}:00",
		  DateTime.Now.Year.ToString("0000"), DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"),
		  DateTime.Now.Hour.ToString("00"), DateTime.Now.Minute.ToString("00"));

	  try {
		if (_debug == "false" || string.IsNullOrEmpty(_debug))
		  _returnValue = (string)System.Windows.Browser.HtmlPage.Window.Invoke("GetDateValue", attributeName);
		else
		  _returnValue = strActionDate;
	  }
	  catch {
		_returnValue = string.Format("{0}-{1}-{2}T{3}:{4}:00",
			DateTime.Now.Year.ToString("0000"), DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"),
			DateTime.Now.Hour.ToString("00"), DateTime.Now.Minute.ToString("00"));
	  }

	  return _returnValue;
	}

	private void btnCommandAdd_Click(object sender, RoutedEventArgs e) {
	  try {
		MessageBox.Show("CommandAdd");
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "btnCommandAdd_Click");
	  }
	}

	private void _doSearch(int hours) {
	  try {
		bool _onlyGetContractor = false;

		if (_header != null && _header.SelectedTransport != null && _header.SelectedTransport.Id > 0) {

		  string _forLineGids = string.Empty;
		  PubTrans.TransportType _transportType = PubTrans.TransportType.TRAIN;

		  if (_header.SelectedTransport.Id == 1) {
			if (_header.SelectedStopAreaFrom == null || _header.SelectedStopAreaFrom.StopAreaName == null) {
			  MessageBox.Show(_MSGBOX_INFO_NO_FROMSELECTION, _MSGBOX_INFO_CAPTION, MessageBoxButton.OK);
			  return;
			}

			if (_header.SelectedStopAreaTo == null || _header.SelectedStopAreaTo.StopAreaName == null) {
			  MessageBox.Show(_MSGBOX_INFO_NO_TOSELECTION, _MSGBOX_INFO_CAPTION, MessageBoxButton.OK);
			  return;
			}

			_forLineGids = string.Empty;
			_transportType = PubTrans.TransportType.TRAIN;
		  }

		  if (_header.SelectedTransport.Id == 2) {
			if (_header.SelectedCity == null || _header.SelectedCity.ZoneId == null) {
			  MessageBox.Show(_MSGBOX_INFO_NO_CITYSELECTION, _MSGBOX_INFO_CAPTION, MessageBoxButton.OK);
			  return;
			}

			if (_header.SelectedTour == null || _header.SelectedTour.LineGid == null) {
			  MessageBox.Show(_MSGBOX_INFO_NO_TOURSELECTION, _MSGBOX_INFO_CAPTION, MessageBoxButton.OK);
			  return;
			}

			if (_header.SelectedStopAreaFrom == null || _header.SelectedStopAreaFrom.StopAreaName == null || _header.SelectedStopAreaFrom.StopAreaGid == null) {
			  //MessageBox.Show(_MSGBOX_INFO_NO_FROMSELECTION, _MSGBOX_INFO_CAPTION, MessageBoxButton.OK);
			  //return;
			  _onlyGetContractor = true;
			}

			if (_header.SelectedStopAreaTo == null || _header.SelectedStopAreaTo.StopAreaName == null) {
			  //MessageBox.Show(_MSGBOX_INFO_NO_TOSELECTION, _MSGBOX_INFO_CAPTION, MessageBoxButton.OK);
			  //return;
			  _onlyGetContractor = true;
			}

			_forLineGids = _header.SelectedTour.LineGid;
			_transportType = PubTrans.TransportType.CITYBUS;
		  }

		  if (_header.SelectedTransport.Id == 3) {
			if (_header.SelectedTour == null || _header.SelectedTour.LineGid == null) {
			  MessageBox.Show(_MSGBOX_INFO_NO_TOURSELECTION, _MSGBOX_INFO_CAPTION, MessageBoxButton.OK);
			  return;
			}

			if (_header.SelectedStopAreaFrom == null || _header.SelectedStopAreaFrom.StopAreaName == null) {
			  //MessageBox.Show(_MSGBOX_INFO_NO_FROMSELECTION, _MSGBOX_INFO_CAPTION, MessageBoxButton.OK);
			  //return;
			  _onlyGetContractor = true;
			}

			if (_header.SelectedStopAreaTo == null || _header.SelectedStopAreaTo.StopAreaName == null) {
			  //MessageBox.Show(_MSGBOX_INFO_NO_TOSELECTION, _MSGBOX_INFO_CAPTION, MessageBoxButton.OK);
			  //return;
			  _onlyGetContractor = true;
			}

			_forLineGids = _header.SelectedTour.LineGid;
			_transportType = PubTrans.TransportType.REGIONBUS;
		  }

		  _theDispatcher.BeginInvoke(() => { _header.IsWaiting = true; });

		  if (_onlyGetContractor == false) {
			CRM.Travel.Information.PubTrans.PubTransServiceClient _pubtransClient = new PubTransServiceClient(_webservice.WebserviceBinding, _webservice.WebserviceEndpointAddress);
			_pubtransClient.GetDirectJourneysCompleted += new EventHandler<GetDirectJourneysCompletedEventArgs>(_pubtransClient_GetDirectJourneysCompleted);

			DateTime dtTo;
			dtTo = !string.IsNullOrEmpty(strActionDate) ? DateTime.Parse(strActionDate.ToString()) : DateTime.Now;

			DateTime _middle = dtTo.AddHours(hours);
			DateTime _searchFrom = _middle.AddHours(-1);
			strActionDate = _middle.ToString();

			_mainpage.txtActionDate.Text = strActionDate;
			_pubtransClient.GetDirectJourneysAsync(_header.SelectedStopAreaFrom.StopAreaGid, _header.SelectedStopAreaTo.StopAreaGid, _searchFrom, _forLineGids, _transportType);
		  }
		  else if (_onlyGetContractor == true) {
			_setContractor(_findContractor(""));
		  }
		}
		else {
		  _theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		  MessageBox.Show(_MSGBOX_INFO_NO_SELECTION, _MSGBOX_INFO_CAPTION, MessageBoxButton.OK);
		}
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "_doSearch");
	  }
	}

	private string _findContractor(string contractorGid) {
	  string _returnValue = "";
	  try {
		string _operatorcode = "";
		if (_header.SelectedTransport.Id == 1) {
		  _operatorcode = "";
		  Contractor _contractor = _contractorList.FirstOrDefault(x => x.Gid == contractorGid);
		  if (_contractor != null) {
			OrganisationalUnit _orgunit = _organisationalUnitList.FirstOrDefault(x => x.ID == _contractor.IsOrganisationId);
			if (_orgunit != null) {
			  _returnValue = _orgunit.Name;
			}
		  }
		}
		else if (_header.SelectedTransport.Id == 2) {
		  _operatorcode = _header.SelectedTour.LineOperatorCode;
		  OrganisationalUnit _orgunit = _organisationalUnitList.FirstOrDefault(x => x.Code == _operatorcode);
		  if (_orgunit != null)
			_returnValue = _orgunit.Name;
		}
		else if (_header.SelectedTransport.Id == 3) {
		  Contractor _c = _contractorList.FirstOrDefault(x => x.Gid == contractorGid);
		  if (_c != null) {
			string _id = _c.IsOrganisationId;
			OrganisationalUnit _orgunit = _organisationalUnitList.FirstOrDefault(x => x.ID == _id);
			if (_orgunit != null)
			  _returnValue = _orgunit.Name;
			else
			  _returnValue = "";
		  }
		  else {
			_returnValue = "";
		  }
		}
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "_findContractor");
	  }
	  return _returnValue;
	}

	private void _setContractor(string contractor) {
	  try {
		string _transport = "";
		if (_header != null && _header.SelectedTransport != null && _header.SelectedTransport.Caption != null)
		  _transport = _header.SelectedTransport.Caption;

		string _city = "";
		if (_header != null && _header.SelectedCity != null && _header.SelectedCity.ZoneShortName != null)
		  _city = _header.SelectedCity.ZoneShortName;

		string _tour = "";
		if (_header != null && _header.SelectedTour != null && _header.SelectedTour.LineNumber != null)
		  _tour = _header.SelectedTour.LineNumber;

		ObservableCollection<TravelInformation> _result = new ObservableCollection<TravelInformation>();
		TravelInformation _t = new TravelInformation {
		  Contractor = contractor,
		  Transport = _transport,
		  City = _city,
		  Tour = _tour,
		  Start = "",
		  Stop = "",
		  ArivalActual = "",
		  ArivalPlanned = "",
		  DirectionText = "",
		  StartActual = "",
		  StartPlanned = "",
		  HasArrivalDeviation = false,
		  HasDepartureDeviation = false,
		  HasServiceJourneyDeviation = false,
		};
		_t.MainPageViewModel = this;
		_t.CommandAdd.IsEnabled = true;
		_result.Add(_t);

		_mainpage.grdSearch.ItemsSource = null;
		_mainpage.grdSearch.ItemsSource = _result;

		if (_result != null && _result.Count() > 0) {
		  _mainpage.grdSearch.SelectedIndex = 0;
		  _mainpage.grdSearch.Focus();
		}

		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "_setContractor");
	  }
	}

	private void _pubtransClient_GetDirectJourneysCompleted(object sender, GetDirectJourneysCompletedEventArgs e) {
	  try {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		PopulateGrid(e);
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "_pubtransClient_GetDirectJourneysCompleted");
	  }
	}

	private void PopulateGrid(GetDirectJourneysCompletedEventArgs e) {
	  try {
		ObservableCollection<TravelInformation> _result = new ObservableCollection<TravelInformation>();

		// GetCallsForServiceJourney(string serviceJourneyId, DateTime operatingDate, string atStopGid);

		if (e.Result.DirectJourneysBetweenStops != null) {
		  int journeyCount = e.Result.DirectJourneysBetweenStops.Count;
		  for (int i = 0; i < journeyCount; i++) {
			TravelInformation _t = new TravelInformation();

			_t.Transport = _header.SelectedTransport.Caption; //Henning - Behöver delas upp så att vi kan veta vilket tåg det är...

			if (_header.SelectedCity == null)
			  _t.City = "";
			else
			  _t.City = _header.SelectedCity.ZoneShortName;


            /***********Start change 2017-05-15 by Henning Freudenthaler***********/
            /*
            if (_header.SelectedTour != null)
                _t.Tour = _header.SelectedTour.LineNumber;
            else
                _t.Tour = "";
            */

            if (_header.SelectedTransport.Id == 1) //Train
            {
                _t.Transport = _header.SelectedTour.LineName;

                _t.Line = string.IsNullOrEmpty(e.Result.DirectJourneysBetweenStops[i].JourneyNumber) ? "" : e.Result.DirectJourneysBetweenStops[i].JourneyNumber;
                //s_t.

                _t.Tour = string.IsNullOrEmpty(e.Result.DirectJourneysBetweenStops[i].JourneyNumber) ? "" : e.Result.DirectJourneysBetweenStops[i].JourneyNumber;
                _t.JourneyNumber = string.IsNullOrEmpty(e.Result.DirectJourneysBetweenStops[i].JourneyNumber) ? "" : e.Result.DirectJourneysBetweenStops[i].JourneyNumber;
            }
            if (_header.SelectedTransport.Id == 2 || _header.SelectedTransport.Id == 3) //City Bus or Bus
            {
                _t.Transport = _header.SelectedTransport.Caption;

                if (_header.SelectedTour != null)
                    _t.Line = _header.SelectedTour.LineNumber;
                else
                    _t.Line = "";

                _t.Tour = string.IsNullOrEmpty(e.Result.DirectJourneysBetweenStops[i].JourneyNumber) ? "" : e.Result.DirectJourneysBetweenStops[i].JourneyNumber;
                _t.JourneyNumber = string.IsNullOrEmpty(e.Result.DirectJourneysBetweenStops[i].JourneyNumber) ? "" : e.Result.DirectJourneysBetweenStops[i].JourneyNumber;

            }
            /***********End change 2017-05-15 by Henning Freudenthaler***********/


            string _contractor = string.IsNullOrEmpty(e.Result.DirectJourneysBetweenStops[i].ContractorGid) ? "" : e.Result.DirectJourneysBetweenStops[i].ContractorGid;
			_t.Contractor = _findContractor(_contractor);
			_t.Start = _header.SelectedStopAreaFrom.StopAreaShortName;
			_t.Stop = _header.SelectedStopAreaTo.StopAreaShortName;

			DateTime _ArivalActual;
			DateTime _ArivalPlanned;
			if (e.Result.DirectJourneysBetweenStops[i].ObservedArrivalDateTime != DateTime.MinValue) {
			  string _actual = e.Result.DirectJourneysBetweenStops[i].ObservedArrivalDateTime.ToString("HH:mm");
			  string _planned = e.Result.DirectJourneysBetweenStops[i].PlannedArrivalDateTime.ToString("HH:mm");

			  _ArivalActual = Convert.ToDateTime(_actual);
			  _ArivalPlanned = Convert.ToDateTime(_planned);

			  TimeSpan _a = _ArivalActual - _ArivalPlanned;
			  string _minutes = "";
			  if (_ArivalActual == _ArivalPlanned)
				_minutes = string.Format("{0}", _a.Minutes.ToString());
			  else if (_ArivalActual < _ArivalPlanned)
				_minutes = string.Format("{0}", _a.Minutes.ToString());
			  else if (_ArivalActual > _ArivalPlanned)
				_minutes = string.Format("+{0}", _a.Minutes.ToString());

			  _t.ArivalActual = e.Result.DirectJourneysBetweenStops[i].ObservedArrivalDateTime.ToString("HH:mm") + " (" + _minutes + ")";
			  _t.PlannedArivalTimeDisplay = string.Format("{0}", e.Result.DirectJourneysBetweenStops[i].PlannedArrivalDateTime.ToString("HH:mm"));
			}
			else {
			  _t.ArivalActual = "X";
			  _t.PlannedArivalTimeDisplay = "";
			}

			_t.ArivalPlanned = e.Result.DirectJourneysBetweenStops[i].PlannedArrivalDateTime.ToString("yyyy-MM-dd HH:mm");
			_t.DirectionTextOrg = e.Result.DirectJourneysBetweenStops[i].DirectionOfLineDescription;

			if (_header.SelectedTransport.Id == 1) {
			  _t.DirectionText = string.Format("{0} {1}", e.Result.DirectJourneysBetweenStops[i].JourneyNumber, e.Result.DirectJourneysBetweenStops[i].DirectionOfLineDescription);
			}
			else {
			  _t.DirectionText = string.Format("{0}, {1}, {2}", _header.SelectedTour.LineName, e.Result.DirectJourneysBetweenStops[i].JourneyNumber, e.Result.DirectJourneysBetweenStops[i].DirectionOfLineDescription);
			}



			DateTime _StartActual;
			DateTime _StartPlanned;
			if (e.Result.DirectJourneysBetweenStops[i].ObservedDepartureDateTime != DateTime.MinValue) {
			  string _actual = e.Result.DirectJourneysBetweenStops[i].ObservedDepartureDateTime.ToString("HH:mm");
			  string _planned = e.Result.DirectJourneysBetweenStops[i].PlannedDepartureDateTime.ToString("HH:mm");

			  _StartActual = Convert.ToDateTime(_actual);
			  _StartPlanned = Convert.ToDateTime(_planned);
			  TimeSpan _a = _StartActual - _StartPlanned;
			  string _minutes = "";
			  if (_StartActual == _StartPlanned)
				_minutes = string.Format("{0}", _a.Minutes.ToString());
			  else if (_StartActual < _StartPlanned)
				_minutes = string.Format("{0}", _a.Minutes.ToString());
			  else if (_StartActual > _StartPlanned)
				_minutes = string.Format("+{0}", _a.Minutes.ToString());

			  _t.StartActual = e.Result.DirectJourneysBetweenStops[i].ObservedDepartureDateTime.ToString("HH:mm") + " (" + _minutes + ")";
			  _t.PlannedStartTimeDisplay = string.Format("{0}", e.Result.DirectJourneysBetweenStops[i].PlannedDepartureDateTime.ToString("HH:mm"));
			}
			else {
			  _t.StartActual = "X";
			  _t.PlannedStartTimeDisplay = "";
			}

            //Tour number - Henning
			//_t.JourneyNumber = string.IsNullOrEmpty(e.Result.DirectJourneysBetweenStops[i].JourneyNumber) ? "" : e.Result.DirectJourneysBetweenStops[i].JourneyNumber; //Changed 2017-05-15 by Henning Freudnethaler
			_t.StartPlanned = e.Result.DirectJourneysBetweenStops[i].PlannedDepartureDateTime.ToString("yyyy-MM-dd HH:mm");

			_t.HasArrivalDeviation = e.Result.DirectJourneysBetweenStops[i].HasArrivalDeviation ? true : false;
			_t.HasDepartureDeviation = e.Result.DirectJourneysBetweenStops[i].HasDepartureDeviation ? true : false;
			_t.HasServiceJourneyDeviation = e.Result.DirectJourneysBetweenStops[i].HasServiceJourneyDeviation ? true : false;

			_t.DatedVehicleJourneyId = string.IsNullOrEmpty(e.Result.DirectJourneysBetweenStops[i].DatedVehicleJourneyId) ? "" : e.Result.DirectJourneysBetweenStops[i].DatedVehicleJourneyId;
			_t.ServiceJourneyGid = string.IsNullOrEmpty(e.Result.DirectJourneysBetweenStops[i].ServiceJourneyGid) ? "" : e.Result.DirectJourneysBetweenStops[i].ServiceJourneyGid;
			_t.OperatingDayDate = e.Result.DirectJourneysBetweenStops[i].OperatingDayDate != DateTime.MinValue ? e.Result.DirectJourneysBetweenStops[i].OperatingDayDate : new DateTime(1, 0, 0, 0, 0, 0);
			_t.ContractorGid = string.IsNullOrEmpty(e.Result.DirectJourneysBetweenStops[i].ContractorGid) ? e.Result.DirectJourneysBetweenStops[i].ContractorGid : "";
			_t.LineDesignation = string.IsNullOrEmpty(e.Result.DirectJourneysBetweenStops[i].LineDesignation) ? e.Result.DirectJourneysBetweenStops[i].LineDesignation : "";

			_t.MainPageViewModel = this;
			_t.CommandAdd.IsEnabled = true;
			_result.Add(_t);
		  }
		}
		_mainpage.grdSearch.ItemsSource = null;
		_mainpage.grdSearch.ItemsSource = _result;

		if (_result != null && _result.Count() > 0) {
		  _mainpage.grdSearch.SelectedIndex = 0;
		  _mainpage.grdSearch.Focus();
		}
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "PopulateGrid");
	  }
	}

	private void cboCity_SelectionChanged(object sender, SelectionChangedEventArgs e) {
	  try {
		_header.LineSelection = new ObservableCollection<PubTrans.Line>();

		if (_header.SelectedCity == null) {
		  _mainpage.Cursor = Cursors.Arrow;
		  return;
		}

		_header.SelectedCity = _mainpage.cboCity.SelectedItem as Zone;
		List<PubTrans.Line> _sorted = _header.SelectedCity.Lines.OrderBy(x => Convert.ToInt32(x.LineDisplayOrder)).ToList();

		foreach (PubTrans.Line _l in _sorted) {
		  _header.LineSelection.Add(_l);
		}

		_header.StopAreaSelection = new ObservableCollection<StopArea>();
		//_refreshFromComboBox();
		//_refreshToComboBox();
		_refreshLineComboBox();
		_mainpage.cboTour.SelectionChanged -= cboTour_SelectionChanged;
		_mainpage.cboTour.ItemsSource = null;
		if (_header.LineSelection != null) {
		  _mainpage.cboTour.ItemsSource = _header.LineSelection;
		  _mainpage.cboTour.DisplayMemberPath = "LineDesignation";
		}
		_mainpage.cboTour.SelectionChanged += cboTour_SelectionChanged;
	  }
	  catch (Exception ex) {
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "cboCity_SelectionChanged");
	  }
	}

	private void cboFrom_SelectionChanged(object sender, SelectionChangedEventArgs e) {
	  try {
		if (_header.SelectedTransport.Id == 1) {
		  _mainpage.cboTo.ItemsSource = null;
		  if (_header.SelectedStopAreaFrom != null) {
			if (_header.SelectedStopAreaFrom.UptoStopAreas != null) {
			  _mainpage.cboTo.ItemsSource = _header.SelectedStopAreaFrom.UptoStopAreas.OrderBy(x => x.StopAreaName);
			}
		  }
		  _mainpage.cboTo.DisplayMemberPath = "StopAreaName";
		}
	  }
	  catch (Exception ex) {
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "cboFrom_SelectionChanged");
	  }
	}

	private void cboTour_SelectionChanged(object sender, SelectionChangedEventArgs e) {
	  try {
		_mainpage.Cursor = Cursors.Wait;
		Header _h = ((ComboBox)sender).DataContext as Header;

		if (_h.SelectedTour == null) {
		  _mainpage.Cursor = Cursors.Arrow;
		  return;
		}

		_h.SelectedTour = _mainpage.cboTour.SelectedItem as PubTrans.Line;
		PubTrans.Line _l = _h.SelectedTour;
		List<PubTrans.StopArea> _stopareas = _l.StopAreas.OrderBy(x => x.StopAreaName).ToList();

		_header.StopAreaSelection = new ObservableCollection<StopArea>();
		foreach (PubTrans.StopArea _s1 in _stopareas) {
		  _header.StopAreaSelection.Add(_s1);
		}

		_refreshFromComboBox();
		_refreshToComboBox();
		_mainpage.Cursor = Cursors.Arrow;
	  }
	  catch (Exception ex) {
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "cboTour_SelectionChanged");
	  }
	}

	private void cboTransport_SelectionChanged(object sender, SelectionChangedEventArgs e) {
	  /*
		  Train       : ID 1
		  CityBus     : ID 2
		  RegionBus   : ID 3
	  */

	  try {
		Header _h = ((ComboBox)sender).DataContext as Header;
		CRM.Travel.Information.Classes.TransportType _t = _h.SelectedTransport;

		// *************************************************************************************
		// Search for new type of transport.
		_header.CityBusSelection = new ObservableCollection<Zone>();
		_header.LineSelection = new ObservableCollection<PubTrans.Line>();
		_header.SelectedCity = new Zone();
		_header.SelectedStopAreaFrom = new StopArea();
		_header.SelectedStopAreaTo = new StopArea();
		_header.SelectedTour = new PubTrans.Line();
		_header.StopAreaSelection = new ObservableCollection<StopArea>();
		_header.TrainStopSelection = new ObservableCollection<StopArea>();

		_refreshCityComboBox();
		_refreshFromComboBox();
		_refreshFromTrainStationComboBox();
		_refreshLineComboBox();
		_refreshToComboBox();
		_refreshToTrainStationComboBox();
		_refreshSearchResult();

		_setControlsVisibility();
		// *************************************************************************************

		_pubtransClient = new PubTransServiceClient(_webservice.WebserviceBinding, _webservice.WebserviceEndpointAddress);

		if (_t.Id == 0) {
		  _mainpage.Cursor = Cursors.Arrow;
		}

		//Train
		if (_t.Id == 1) {
		  _mainpage.Cursor = Cursors.Wait;
		  _theDispatcher.BeginInvoke(() => { _header.IsWaiting = true; });
		  //_header.IsWaiting = true;
		  _pubtransClient.GetTrainDetailsCompleted += _pubtransClient_GetTrainDetailsCompleted;
		  _pubtransClient.GetTrainDetailsAsync();
		}

		//Citybus
		if (_t.Id == 2) {
		  _mainpage.Cursor = Cursors.Wait;
		  _theDispatcher.BeginInvoke(() => { _header.IsWaiting = true; });
		  //_header.IsWaiting = true;//
		  _pubtransClient.GetStradBusDetailsCompleted += _pubtransClient_GetStradBusDetailsCompleted;
		  _pubtransClient.GetStradBusDetailsAsync();
		}

		//Regionbus
		if (_t.Id == 3) {
		  _mainpage.Cursor = Cursors.Wait;
		  _theDispatcher.BeginInvoke(() => { _header.IsWaiting = true; });
		  //_header.IsWaiting = true;
		  _pubtransClient.GetRegionBusDetailsCompleted += _pubtransClient_GetRegionBusDetailsCompleted;
		  _pubtransClient.GetRegionBusDetailsAsync();
		}


	  }
	  catch (Exception ex) {
		_mainpage.Cursor = Cursors.Arrow;
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "cboTransport_SelectionChanged");
	  }
	}

	private void _pubtransClient_GetRegionBusDetailsCompleted(object sender, GetRegionBusDetailsCompletedEventArgs e) {
	  try {
		_header.LineSelection = new ObservableCollection<PubTrans.Line>();

		List<PubTrans.Line> _rawlist = e.Result.ToList();
		List<PubTrans.Line> _list = new List<PubTrans.Line>();
		foreach (PubTrans.Line _l in _rawlist) {
		  int _linedesignation = 0;
		  int.TryParse(_l.LineDesignation, out _linedesignation);
		  if (_linedesignation > 0) {
			_list.Add(_l);
		  }
		}

		List<PubTrans.Line> _sorted = _list.OrderBy(x => Convert.ToInt32(x.LineDesignation)).ToList();

		foreach (PubTrans.Line _l in _sorted) {
		  _header.LineSelection.Add(_l);
		}

		_refreshLineComboBox();
		_mainpage.Cursor = Cursors.Arrow;
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "_pubtransClient_GetRegionBusDetailsCompleted");
	  }
	  finally {
		_pubtransClient.GetRegionBusDetailsCompleted -= _pubtransClient_GetRegionBusDetailsCompleted;
	  }
	}

	private void _pubtransClient_GetStradBusDetailsCompleted(object sender, GetStradBusDetailsCompletedEventArgs e) {
	  try {
		_header.CityBusSelection = new ObservableCollection<Zone>();
		List<PubTrans.Zone> _sorted = e.Result.OrderBy(x => x.ZoneName).ToList();

		foreach (PubTrans.Zone _z in _sorted) {
		  _header.CityBusSelection.Add(_z);
		}

		_refreshCityComboBox();
		_mainpage.Cursor = Cursors.Arrow;
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "_pubtransClient_GetStradBusDetailsCompleted");
	  }
	  finally {
		_pubtransClient.GetStradBusDetailsCompleted -= _pubtransClient_GetStradBusDetailsCompleted;
	  }
	}

	private void _pubtransClient_GetTrainDetailsCompleted(object sender, GetTrainDetailsCompletedEventArgs e) {
	  try {
		_header.TrainStopSelection = new ObservableCollection<StopArea>();
		List<PubTrans.StopArea> _sorted = e.Result.OrderBy(x => x.StopAreaName).ToList();

		foreach (PubTrans.StopArea _s1 in _sorted) {
		  _header.TrainStopSelection.Add(_s1);
		}

		_refreshFromTrainStationComboBox();
		//_refreshToTrainStationComboBox();
		_mainpage.Cursor = Cursors.Arrow;
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "_pubtransClient_GetTrainDetailsCompleted");
	  }
	  finally {
		_pubtransClient.GetTrainDetailsCompleted -= _pubtransClient_GetTrainDetailsCompleted;
	  }
	}

	private void _refreshLineComboBox() {
	  try {
		_theDispatcher.BeginInvoke(() => {
		  _mainpage.cboTour.ItemsSource = null;
		  if (_header.LineSelection != null)
			_mainpage.cboTour.ItemsSource = _header.LineSelection;

		  _mainpage.cboTour.DisplayMemberPath = "LineDesignation";
		  _header.IsWaiting = false;
		});
	  }
	  catch (Exception ex) {
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "_refreshLineComboBox");
	  }
	}

	private void _refreshFromComboBox() {
	  try {
		_theDispatcher.BeginInvoke(() => {
		  _mainpage.cboFrom.ItemsSource = null;
		  if (_header.StopAreaSelection != null)
			_mainpage.cboFrom.ItemsSource = _header.StopAreaSelection;
		  _mainpage.cboFrom.DisplayMemberPath = "StopAreaName";
		  _header.IsWaiting = false;
		});
	  }
	  catch (Exception ex) {
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "_refreshFromComboBox");
	  }
	}

	private void _refreshFromTrainStationComboBox() {
	  try {
		_theDispatcher.BeginInvoke(() => {
		  _mainpage.cboFrom.ItemsSource = null;
		  if (_header.TrainStopSelection != null)
			_mainpage.cboFrom.ItemsSource = _header.TrainStopSelection;
		  _mainpage.cboFrom.DisplayMemberPath = "StopAreaName";
		  _header.IsWaiting = false;
		});
	  }
	  catch (Exception ex) {
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "_refreshFromTrainStationComboBox");
	  }
	}

	private void _refreshToComboBox() {
	  try {
		_theDispatcher.BeginInvoke(() => {
		  _mainpage.cboTo.ItemsSource = null;
		  if (_header.StopAreaSelection != null)
			_mainpage.cboTo.ItemsSource = _header.StopAreaSelection;
		  _mainpage.cboTo.DisplayMemberPath = "StopAreaName";
		  _header.IsWaiting = false;
		});
	  }
	  catch (Exception ex) {
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "_refreshToComboBox");
	  }
	}

	private void _refreshToTrainStationComboBox() {
	  try {
		_theDispatcher.BeginInvoke(() => {
		  _mainpage.cboTo.ItemsSource = null;
		  if (_header.TrainStopSelection != null)
			_mainpage.cboTo.ItemsSource = _header.TrainStopSelection;
		  _mainpage.cboTo.DisplayMemberPath = "StopAreaName";
		  _header.IsWaiting = false;
		});
	  }
	  catch (Exception ex) {
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "_refreshToTrainStationComboBox");
	  }
	}

	private void _refreshCityComboBox() {
	  try {
		_theDispatcher.BeginInvoke(() => {
		  _mainpage.cboCity.ItemsSource = null;
		  if (_header.CityBusSelection != null)
			_mainpage.cboCity.ItemsSource = _header.CityBusSelection;
		  _mainpage.cboCity.DisplayMemberPath = "ZoneName";
		  _header.IsWaiting = false;
		});
	  }
	  catch (Exception ex) {
		_mainpage.Cursor = Cursors.Arrow;
		_mainpage.ShowError(ex, "_refreshCityComboBox");
	  }
	}

	private void _refreshSearchResult() {
	  try {
		_theDispatcher.BeginInvoke(() => {
		  _mainpage.grdSearch.ItemsSource = null;
		});
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_refreshSearchResult");
	  }
	}

	private void _setControlsVisibility() {
	  try {
		if (_header.SelectedTransport.Id == 0) {
		  _mainpage.CityCaption.Visibility = Visibility.Collapsed;
		  _mainpage.cboCity.Visibility = Visibility.Collapsed;
		  _mainpage.TourCaption.Visibility = Visibility.Collapsed;
		  _mainpage.cboTour.Visibility = Visibility.Collapsed;
		  _mainpage.FromCaption.Visibility = Visibility.Collapsed;
		  _mainpage.cboFrom.Visibility = Visibility.Collapsed;
		  _mainpage.ToCaption.Visibility = Visibility.Collapsed;
		  _mainpage.cboTo.Visibility = Visibility.Collapsed;
		}

		if (_header.SelectedTransport.Id == 1) {
		  _mainpage.CityCaption.Visibility = Visibility.Collapsed;
		  _mainpage.cboCity.Visibility = Visibility.Collapsed;
		  _mainpage.TourCaption.Visibility = Visibility.Collapsed;
		  _mainpage.cboTour.Visibility = Visibility.Collapsed;
		  _mainpage.FromCaption.Visibility = Visibility.Visible;
		  _mainpage.cboFrom.Visibility = Visibility.Visible;
		  _mainpage.ToCaption.Visibility = Visibility.Visible;
		  _mainpage.cboTo.Visibility = Visibility.Visible;
		}

		if (_header.SelectedTransport.Id == 2) {
		  _mainpage.CityCaption.Visibility = Visibility.Visible;
		  _mainpage.cboCity.Visibility = Visibility.Visible;
		  _mainpage.TourCaption.Visibility = Visibility.Visible;
		  _mainpage.cboTour.Visibility = Visibility.Visible;
		  _mainpage.FromCaption.Visibility = Visibility.Visible;
		  _mainpage.cboFrom.Visibility = Visibility.Visible;
		  _mainpage.ToCaption.Visibility = Visibility.Visible;
		  _mainpage.cboTo.Visibility = Visibility.Visible;
		}

		if (_header.SelectedTransport.Id == 3) {
		  _mainpage.CityCaption.Visibility = Visibility.Collapsed;
		  _mainpage.cboCity.Visibility = Visibility.Collapsed;
		  _mainpage.TourCaption.Visibility = Visibility.Visible;
		  _mainpage.cboTour.Visibility = Visibility.Visible;
		  _mainpage.FromCaption.Visibility = Visibility.Visible;
		  _mainpage.cboFrom.Visibility = Visibility.Visible;
		  _mainpage.ToCaption.Visibility = Visibility.Visible;
		  _mainpage.cboTo.Visibility = Visibility.Visible;
		}
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_setControlsVisibility");
	  }
	}

	private void _setSettings_completed() {
	  try {
		_loadLanguageLabels();
		_getOrganisationalUnits();
		_getContractors();
	  }
	  catch (Exception ex) {
		_setLoadingFalse();
		_mainpage.ShowError(ex, "_setSettings_completed");
	  }
	}

	private void _loadLanguageLabels() {
	  try {
		_crmManager.Fetch<Language>(_fetch.XmlGetLanguageLabels(LocalizedLabelGroup), _loadLanguageLabels_callback);
	  }
	  catch (Exception ex) {
		_setLoadingFalse();
		_mainpage.ShowError(ex, "_loadLanguageLabels");
	  }
	}

	private void _loadLanguageLabels_callback(ObservableCollection<Language> labelslist) {
	  try {
		_setButtonTranslation(_mainpage.btnSearch, _getTranslation(labelslist, "BUTTON_SEARCH"));
		_setTextBlockTranslation(_mainpage.TransportCaption, _getTranslation(labelslist, "LABEL_TRANSPORT"));
		_setTextBlockTranslation(_mainpage.CityCaption, _getTranslation(labelslist, "LABEL_CITY"));
		_setTextBlockTranslation(_mainpage.TourCaption, _getTranslation(labelslist, "LABEL_TOUR"));
		_setTextBlockTranslation(_mainpage.FromCaption, _getTranslation(labelslist, "LABEL_FROM"));
		_setTextBlockTranslation(_mainpage.ToCaption, _getTranslation(labelslist, "LABEL_TO"));

		_setGridColTranslation(_mainpage.grdSearch, 1, _getTranslation(labelslist, "GRID_HEADER_COL1"));
		_setGridColTranslation(_mainpage.grdSearch, 2, _getTranslation(labelslist, "GRID_HEADER_COL2"));
		_setGridColTranslation(_mainpage.grdSearch, 3, _getTranslation(labelslist, "GRID_HEADER_COL3"));
		_setGridColTranslation(_mainpage.grdSearch, 4, _getTranslation(labelslist, "GRID_HEADER_COL4"));
		_setGridColTranslation(_mainpage.grdSearch, 5, _getTranslation(labelslist, "GRID_HEADER_COL6"));
		_setGridColTranslation(_mainpage.grdSearch, 6, _getTranslation(labelslist, "GRID_HEADER_COL5"));

		WaitCaption = _getTranslation(labelslist, "MSG_SEARCH_WAIT");

		_MSGBOX_INFO_CAPTION = _getTranslation(labelslist, "MSGBOX_INFO_CAPTION");
		_MSGBOX_INFO_NO_SELECTION = _getTranslation(labelslist, "MSGBOX_INFO_NO_SELECTION");
		_MSGBOX_INFO_NO_CITYSELECTION = _getTranslation(labelslist, "MSGBOX_INFO_NO_CITYSELECTION");
		_MSGBOX_INFO_NO_TOURSELECTION = _getTranslation(labelslist, "MSGBOX_INFO_NO_TOURSELECTION");
		_MSGBOX_INFO_NO_FROMSELECTION = _getTranslation(labelslist, "MSGBOX_INFO_NO_FROMSELECTION");
		_MSGBOX_INFO_NO_TOSELECTION = _getTranslation(labelslist, "MSGBOX_INFO_NO_TOSELECTION");

		//_setTextBlockTranslation(_mainpage.enabledCaption, _getTranslation(labelslist, "MSG_ENABLE_CONTENT"));

		//Load transporttype combo
		_setModeOfTransport(labelslist);
	  }
	  catch (Exception ex) {
		_setLoadingFalse();
		_mainpage.ShowError(ex, "_loadLanguageLabels_callback");
	  }
	}

	private void _setModeOfTransport(ObservableCollection<Language> labelslist) {
	  try {
		CRM.Travel.Information.Classes.TransportType _tt;

		_modeOfTransportTypeList = new ObservableCollection<CRM.Travel.Information.Classes.TransportType>();

		_tt = new CRM.Travel.Information.Classes.TransportType();
		_tt.Caption = _getTranslation(labelslist, "CBO_MODEOFTRANSPORT_CHOOSE_CAPTION");
		_tt.Id = 0;
		_modeOfTransportTypeList.Add(_tt);

		_tt = new CRM.Travel.Information.Classes.TransportType();
		_tt.Caption = _getTranslation(labelslist, "CBO_MODEOFTRANSPORT_TRAIN_CAPTION");
		_tt.Id = 1;
		_modeOfTransportTypeList.Add(_tt);

		_tt = new CRM.Travel.Information.Classes.TransportType();
		_tt.Caption = _getTranslation(labelslist, "CBO_MODEOFTRANSPORT_CITYBUS_CAPTION");
		_tt.Id = 2;
		_modeOfTransportTypeList.Add(_tt);

		_tt = new CRM.Travel.Information.Classes.TransportType();
		_tt.Caption = _getTranslation(labelslist, "CBO_MODEOFTRANSPORT_REGIONBUS_CAPTION");
		_tt.Id = 3;
		_modeOfTransportTypeList.Add(_tt);

		_theDispatcher.BeginInvoke(() => {
		  _mainpage.cboTransport.ItemsSource = null;
		  _mainpage.cboTransport.ItemsSource = _modeOfTransportTypeList;

		  _mainpage.cboTransport.SelectionChanged -= new SelectionChangedEventHandler(cboTransport_SelectionChanged);
		  _mainpage.cboTransport.SelectedIndex = 0;
		  _setControlsVisibility();
		  _mainpage.cboTransport.SelectionChanged += new SelectionChangedEventHandler(cboTransport_SelectionChanged);

		});

		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_setLoadingFalse();
	  }
	  catch (Exception ex) {
		_setLoadingFalse();
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "_setModeOfTransport");
	  }
	}

	private string _getTranslation(ObservableCollection<Language> labelslist, string controlid) {
	  string _returnvalue = "";
	  try {
		int _lcid = Convert.ToInt32(_webparams.UserLcId);
		Language _translation = labelslist.FirstOrDefault(x => x.LocalizedControlId == controlid && x.LocalizationLanguageNumber == _lcid);
		if (_translation != null)
		  _returnvalue = _translation.LocalizedlabelName;
		else
		  _returnvalue = "<missing>";
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_getTranslation");
	  }

	  return _returnvalue;
	}

	private void _setButtonTranslation(Button control, string translation) {
	  try {
		_theDispatcher.BeginInvoke(() => { control.Content = translation; });
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_setButtonTranslation");
	  }
	}

	private void _setTextBlockTranslation(TextBlock control, string translation) {
	  try {
		_theDispatcher.BeginInvoke(() => { control.Text = translation; });
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_setTextBlockTranslation");
	  }
	}

	private void _setGridColTranslation(DataGrid grdGrid, int colidx, string translation) {
	  try {
		_theDispatcher.BeginInvoke(() => { grdGrid.Columns[colidx].Header = translation; });
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_setGridColTranslation");
	  }
	}

	public void AddToSelection(TravelInformation travelinformation) {
	  try {
		if (string.IsNullOrEmpty(_id))
		  return;

		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = true; });
		_getDiviationMessage(travelinformation);
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _mainpage.grdSearch.IsEnabled = true; });
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "AddToSelection");
	  }
	}

	private TravelInformation _currentTravelInformation;

	private void _getDiviationMessage(TravelInformation travelinformation) {
	  try {
		if (_checkIfItemExistInSelectionList(travelinformation) == false) {
		  if (!string.IsNullOrEmpty(travelinformation.ServiceJourneyGid)) {
			_currentTravelInformation = travelinformation;
			_pubtransClient = new PubTransServiceClient(_webservice.WebserviceBinding, _webservice.WebserviceEndpointAddress);
			_pubtransClient.GetCallsForServiceJourneyCompleted += _pubtransClient_GetCallsForServiceJourneyCompleted;
			_pubtransClient.GetCallsForServiceJourneyAsync(travelinformation.ServiceJourneyGid, travelinformation.OperatingDayDate, "");
		  }
		  else {
			_currentTravelInformation = travelinformation;
			_saveAndDisplayTravelInformation(travelinformation);
		  }
		}
		else {
		  _theDispatcher.BeginInvoke(() => { _mainpage.grdSearch.IsEnabled = true; });
		  _theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		}
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _mainpage.grdSearch.IsEnabled = true; });
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "_getDiviationMessage");
	  }
	}

	private void _pubtransClient_GetCallsForServiceJourneyCompleted(object sender, GetCallsForServiceJourneyCompletedEventArgs e) {
	  try {
		if (!string.IsNullOrEmpty(e.Result.ErrorMessage)) {
		  _theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		  _mainpage.ShowMessage(e.Result.ErrorMessage, "_pubtransClient_GetCallsForServiceJourneyCompleted");
		}

		string _deviationMessage = "";
		if (e.Result != null && e.Result.DeviationMessageVariants != null) {
		  DeviationMessageVariant1 _message = e.Result.DeviationMessageVariants.FirstOrDefault(x => x.UsageTypeLongCode == "DETAILS");
		  if (_message != null) {
			_deviationMessage = _message.Content.Replace("\n", " ");
		  }
		}

		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = true; });

		/*
		_currentTravelInformation.DisplayText = string.Format("{0} : {1} : {2} : {3} : {4} : {5} : {6} : {7} : {8}",
			_currentTravelInformation.Contractor,
			_currentTravelInformation.DirectionText,
			_currentTravelInformation.StartActual,
			_currentTravelInformation.ArivalActual,
			_currentTravelInformation.Transport,
			_currentTravelInformation.City,
			_currentTravelInformation.Tour,
			_currentTravelInformation.Start,
			_currentTravelInformation.Stop
			);
		*/

		_currentTravelInformation.DisplayText = _setSaveMessage(_currentTravelInformation); //Henning

		_currentTravelInformation.DeviationMessage = _deviationMessage;
		_selectionList.Add(_currentTravelInformation);
		_saveSelectionToCRM(_currentTravelInformation);
		_refreshSelectionBinding();
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _mainpage.grdSearch.IsEnabled = true; });
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "_pubtransClient_GetCallsForServiceJourneyCompleted");
	  }
	  finally {
		_pubtransClient.GetCallsForServiceJourneyCompleted -= _pubtransClient_GetCallsForServiceJourneyCompleted;
	  }
	}

	private string _setSaveMessage(TravelInformation travelinfo) {
	  string _returnMessage = "";

	  string _trafik;
	  string _city;
	  string _tour;
	  string _line;
	  string _contractor;


	  if (_header.SelectedTransport.Id == 1) {
		//_trafik = string.Format("Trafikslag: {0}", _header.SelectedTour.LineName);
		_tour = string.Format("Linje: [{0}]", travelinfo.JourneyNumber);
		_line = string.Format("Tur: {0} [{1}] {2} - {3} [{4}] {5}",
			travelinfo.PlannedStartTimeDisplay,
			travelinfo.StartActual,
			_header.SelectedStopAreaFrom.StopAreaName,
			travelinfo.PlannedArivalTimeDisplay,
			travelinfo.ArivalActual,
			_header.SelectedStopAreaTo.StopAreaName);
		_contractor = string.Format("Entreprenör: {0}", travelinfo.Contractor);

		_returnMessage = string.Format("{0} {1} {2}", _tour, _line, _contractor);
	  }


	  if (_header.SelectedTransport.Id == 2) {
		_trafik = string.Format("Trafikslag: {0}", _header.SelectedTour.LineName);
		_city = string.Format("Stad: {0}", _header.SelectedCity.ZoneName);
		_tour = string.Format("Linje: {0} [{1}] ({2})", _header.SelectedTour.LineNumber, travelinfo.JourneyNumber, travelinfo.DirectionTextOrg);
		_line = string.Format("Tur: {0} [{1}] {2} - {3} [{4}] {5}",
			travelinfo.PlannedStartTimeDisplay,
			travelinfo.StartActual,
			_header.SelectedStopAreaFrom.StopAreaName,
			travelinfo.PlannedArivalTimeDisplay,
			travelinfo.ArivalActual,
			_header.SelectedStopAreaTo.StopAreaName);
		_contractor = string.Format("Entreprenör: {0}", travelinfo.Contractor);

		_returnMessage = string.Format("{0} {1} {2} {3} {4}", _trafik, _city, _tour, _line, _contractor);
	  }

	  if (_header.SelectedTransport.Id == 3) {
		_trafik = string.Format("Trafikslag: {0}", _header.SelectedTour.LineName);
		_tour = string.Format("Linje: {0} [{1}] ({2})", _header.SelectedTour.LineDesignation, travelinfo.JourneyNumber, travelinfo.DirectionTextOrg);
		_line = string.Format("Tur: {0} [{1}] {2} - {3} [{4}] {5}",
			travelinfo.PlannedStartTimeDisplay,
			travelinfo.StartActual,
			_header.SelectedStopAreaFrom.StopAreaName,
			travelinfo.PlannedArivalTimeDisplay,
			travelinfo.ArivalActual,
			_header.SelectedStopAreaTo.StopAreaName);
		_contractor = string.Format("Entreprenör: {0}", travelinfo.Contractor);

		_returnMessage = string.Format("{0} {1} {2} {3}", _trafik, _tour, _line, _contractor);
	  }

	  return _returnMessage;
	}

	private void _saveAndDisplayTravelInformation(TravelInformation travelinformation) {
	  try {

		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = true; });

		string _displaytext = "";
		if (!string.IsNullOrEmpty(_currentTravelInformation.Contractor))
		  _displaytext = _currentTravelInformation.Contractor;

		if (!string.IsNullOrEmpty(_currentTravelInformation.City))
		  _displaytext += " " + _currentTravelInformation.City;

		if (!string.IsNullOrEmpty(_currentTravelInformation.Tour))
		  _displaytext += " " + _currentTravelInformation.Tour;

		_currentTravelInformation.DisplayText = _displaytext;
		_selectionList.Add(_currentTravelInformation);
		_saveSelectionToCRM(_currentTravelInformation);
		_refreshSelectionBinding();
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _mainpage.grdSearch.IsEnabled = true; });
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "_saveAndDisplayTravelInformation");
	  }
	}

	public void DeleteFromSelection(TravelInformation travelinformation) {
	  try {
		_deleteSelectionFromCRM(travelinformation);
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "DeleteFromSelection");
	  }
	}

	private bool _checkIfItemExistInSelectionList(TravelInformation travelinformation) {
	  if (string.IsNullOrEmpty(travelinformation.ServiceJourneyGid))
		return false;

	  TravelInformation _information = _selectionList.FirstOrDefault(x => x.ServiceJourneyGid == travelinformation.ServiceJourneyGid);
	  if (_information == null)
		return false;
	  else
		return true;

	}

	private void _refreshSelectionBinding() {
	  _theDispatcher.BeginInvoke(() => {
		try {
		  _mainpage.lstResult.ItemsSource = null;
		  _mainpage.lstResult.ItemsSource = _selectionList;
		}
		catch (Exception ex) {
		  _mainpage.ShowError(ex, "_refreshSelectionBinding");
		}
		finally {
		  _theDispatcher.BeginInvoke(() => {
			_mainpage.grdSearch.IsEnabled = true;
			_mainpage.grdSearch.Focus();
		  });
		}
	  });
	}

	private void _saveSelectionToCRM(TravelInformation travelinformation) {

            /*Henning Freudenthaler: -Need change here*/

        try {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = true; });

		Entity entity = new Entity();
		entity.LogicalName = "cgi_travelinformation";

		string _name = string.Format("{0} : {1}", _header.SelectedTransport.Caption, strActionDate);
		entity["cgi_travelinformation"] = _name;


		//if (string.Equals(_header.SelectedTransport.Caption, PubTrans.TransportType.TRAIN.ToString(), StringComparison.CurrentCultureIgnoreCase)
		if(!string.IsNullOrEmpty(travelinformation.JourneyNumber))
		  entity["cgi_journeynumber"] = travelinformation.JourneyNumber;

		if (travelinformation.Transport != null && !string.IsNullOrEmpty(travelinformation.Transport))
		  entity["cgi_transport"] = travelinformation.Transport;

		if (travelinformation.City != null && !string.IsNullOrEmpty(travelinformation.City))
		  entity["cgi_city"] = travelinformation.City;

        /* Add ["cgi_line"] */

        /* Add ["cgi_trainnumber"] */

		if (travelinformation.Tour != null && !string.IsNullOrEmpty(travelinformation.Tour))
		  entity["cgi_tour"] = travelinformation.Tour;

		if (travelinformation.StartPlanned != null && !string.IsNullOrEmpty(travelinformation.StartPlanned))
		  entity["cgi_startplanned"] = SplitDateTime(travelinformation.StartPlanned);

		if (travelinformation.StartActual != null && !string.IsNullOrEmpty(travelinformation.StartActual))
		  entity["cgi_startactual"] = travelinformation.StartActual;

		if (travelinformation.ArivalPlanned != null && !string.IsNullOrEmpty(travelinformation.ArivalPlanned))
		  entity["cgi_arivalplanned"] = SplitDateTime(travelinformation.ArivalPlanned);

		if (travelinformation.ArivalActual != null && !string.IsNullOrEmpty(travelinformation.ArivalActual))
		  entity["cgi_arivalactual"] = travelinformation.ArivalActual;

		if (travelinformation.DirectionText != null && !string.IsNullOrEmpty(travelinformation.DirectionText))
		  entity["cgi_directiontext"] = travelinformation.DirectionText;

		if (travelinformation.Start != null && !string.IsNullOrEmpty(travelinformation.Start))
		  entity["cgi_start"] = travelinformation.Start;

		if (travelinformation.Stop != null && !string.IsNullOrEmpty(travelinformation.Stop))
		  entity["cgi_stop"] = travelinformation.Stop;

		if (travelinformation.Contractor != null && !string.IsNullOrEmpty(travelinformation.Contractor))
		  entity["cgi_contractor"] = travelinformation.Contractor;

		if (travelinformation.DeviationMessage != null && !string.IsNullOrEmpty(travelinformation.DeviationMessage))
		  entity["cgi_deviationmessage"] = travelinformation.DeviationMessage;

		if (travelinformation.DisplayText != null && !string.IsNullOrEmpty(travelinformation.DisplayText))
		  entity["cgi_displaytext"] = travelinformation.DisplayText;

		if (!string.IsNullOrEmpty(_id)) {
		  EntityReference _refcase = new EntityReference();
		  _refcase.Name = "incident";
		  _refcase.Id = new Guid(_id);
		  entity["cgi_caseid"] = _refcase;
		}

		_crmManager.Service.BeginCreate(entity, new AsyncCallback(_saveSelectionToCRMCallback), travelinformation);
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "_saveSelectionToCRM");
	  }
	}

	private void _deleteSelectionFromCRM(TravelInformation travelinformation) {
	  try {
		_header.IsWaiting = true;
		Guid _informationid = new Guid(travelinformation.TravelInformationId);
		_crmManager.Service.BeginDelete("cgi_travelinformation", _informationid, _deleteSelectionFromCRM_callback, travelinformation);
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_deleteSelectionFromCRM");
	  }
	}

	public DateTime? SplitDateTime(string strPlannedDateTime) {
	  if (string.IsNullOrEmpty(strPlannedDateTime))
		return null;

	  DateTime _dt = new DateTime();
	  DateTime.TryParse(strPlannedDateTime, out _dt);
	  return _dt;
	}

	private void _saveSelectionToCRMCallback(IAsyncResult result) {
	  try {
		TravelInformation _travel = result.AsyncState as TravelInformation;
		Guid travelInformationID = _crmManager.Service.EndCreate(result);
		_travel.TravelInformationId = travelInformationID.ToString();
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
	  }
	  catch (Exception ex) {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
		_mainpage.ShowError(ex, "_saveSelectionToCRMCallback");
	  }
	}

	private void _deleteSelectionFromCRM_callback(IAsyncResult result) {
	  try {
		TravelInformation _info = result.AsyncState as TravelInformation;
		_crmManager.Service.EndDelete(result);
		_theDispatcher.BeginInvoke(() => { _selectionList.Remove(_info); });
		_refreshSelectionBinding();
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_deleteSelectionFromCRM_callback");
	  }
	  finally {
		_theDispatcher.BeginInvoke(() => { _header.IsWaiting = false; });
	  }
	}

	//////////////////////////////////////////////////////////////////

	private void _getOrganisationalUnits() {
	  try {
		_pubtransClient = new PubTransServiceClient(_webservice.WebserviceBinding, _webservice.WebserviceEndpointAddress);
		_pubtransClient.GetOrganisationalUnitsCompleted += _pubtransClient_GetOrganisationalUnitsCompleted;
		_pubtransClient.GetOrganisationalUnitsAsync();
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_getOrganisationalUnits");
	  }
	}

	private void _pubtransClient_GetOrganisationalUnitsCompleted(object sender, GetOrganisationalUnitsCompletedEventArgs e) {
	  try {
		_organisationalUnitList = new List<OrganisationalUnit>();
		if (e.Result != null && e.Result.OrganisationalUnitList != null && e.Result.OrganisationalUnitList.Count() > 0)
		  _organisationalUnitList = e.Result.OrganisationalUnitList.ToList();
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_pubtransClient_GetOrganisationalUnitsCompleted");
	  }
	  finally {
		_pubtransClient.GetOrganisationalUnitsCompleted -= _pubtransClient_GetOrganisationalUnitsCompleted;
	  }
	}

	private void _getContractors() {
	  try {
		_pubtransClient = new PubTransServiceClient(_webservice.WebserviceBinding, _webservice.WebserviceEndpointAddress);
		_pubtransClient.GetContractorsCompleted += _pubtransClient_GetContractorsCompleted;
		_pubtransClient.GetContractorsAsync();
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_getOrganisationalUnits");
	  }
	}

	private void _pubtransClient_GetContractorsCompleted(object sender, GetContractorsCompletedEventArgs e) {
	  try {
		_contractorList = new List<Contractor>();
		if (e.Result != null && e.Result.ContractorList != null && e.Result.ContractorList.Count() > 0)
		  _contractorList = e.Result.ContractorList.ToList();
	  }
	  catch (Exception ex) {
		_mainpage.ShowError(ex, "_pubtransClient_GetContractorsCompleted");
	  }
	  finally {
		_pubtransClient.GetContractorsCompleted -= _pubtransClient_GetContractorsCompleted;
	  }
	}

  }
}


