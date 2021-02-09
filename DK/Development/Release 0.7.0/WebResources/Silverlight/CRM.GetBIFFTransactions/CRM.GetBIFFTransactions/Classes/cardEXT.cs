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
using System.Reflection;
using System.Linq;

using CRM.GetBIFFTransactions.TravelCardService;
using CGIXrm;
using System.Text;
using System.Windows.Browser;
using System.Windows.Threading;

namespace CRM.GetBIFFTransactions {
  public class cardEXT : XrmBaseNotify {

	public cardEXT(CrmManager crmManager) {
	  _commandOpenEntity = new RelayCommand(_commandOpenEntityAction, true);
	  _crmManager = crmManager;
	}

	private CrmManager _crmManager;

	private Guid _travelCardID;
	public Guid TravelCardID {
	  get { return _travelCardID; }
	  set { _travelCardID = value; OnPropertyChanged("TravelCardID"); }
	}

	private string _travelCardName;
	public string TravelCardName {
	  get { return _travelCardName; }
	  set { _travelCardName = value; OnPropertyChanged("TravelCardName"); }
	}

	private Guid? _accountId;
	public Guid? AccountId {
	  get { return _accountId; }
	  set { _accountId = value; OnPropertyChanged("AccountId"); }
	}

	private string _accountIdName;
	public string AccountIdName {
	  get { return _accountIdName; }
	  set { _accountIdName = value; OnPropertyChanged("AccountIdName"); }
	}

	private Guid? _contactId;
	public Guid? ContactId {
	  get { return _contactId; }
	  set {
		_contactId = value;
		OnPropertyChanged("ContactId");
		//ContactUri = CreateContactUri();
	  }
	}


	private string _contactUri;
	public string ContactUri {
	  get { return _contactUri; }
	  set {
		_contactUri = value;
		OnPropertyChanged("ContactUri");
	  }
	}

	private string _contactIdName;
	public string ContactIdName {
	  get { return _contactIdName; }
	  set { _contactIdName = value; OnPropertyChanged("ContactIdName"); }
	}

	private int? _blocked;
	public int? Blocked {
	  get { return _blocked; }
	  set { _blocked = value; OnPropertyChanged("Blocked"); }
	}

	private Guid? _cardTypeId;
	public Guid? CardTypeId {
	  get { return _cardTypeId; }
	  set { _cardTypeId = value; OnPropertyChanged("CardTypeId"); }
	}

	private string _cardTypeIdName;
	public string CardTypeIdName {
	  get { return _cardTypeIdName; }
	  set { _cardTypeIdName = value; OnPropertyChanged("CardTypeIdName"); }
	}

	private string _numberOfZones;
	public string NumberOfZones {
	  get { return _numberOfZones; }
	  set { _numberOfZones = value; OnPropertyChanged("NumberOfZones"); }
	}

	private string _periodeicCardType;
	public string PeriodeicCardType {
	  get { return _periodeicCardType; }
	  set { _periodeicCardType = value; OnPropertyChanged("PeriodeicCardType"); }
	}

	private string _validFrom;
	public string ValidFrom {
	  get { return _validFrom; }
	  set { _validFrom = value; OnPropertyChanged("ValidFrom"); }
	}

	private string _validTo;
	public string ValidTo {
	  get { return _validTo; }
	  set { _validTo = value; OnPropertyChanged("ValidTo"); }
	}

	private string _value_card_type;
	public string Value_card_type {
	  get { return _value_card_type; }
	  set { _value_card_type = value; OnPropertyChanged("Value_card_type"); }
	}

	private int _stateCode;
	public int StateCode {
	  get { return _stateCode; }
	  set { _stateCode = value; OnPropertyChanged("StateCode"); }
	}

	//travelcard.cgi_autoloadstatus,
	private int _autoloadstatus;
	public int Autoloadstatus {
	  get { return _autoloadstatus; }
	  set { _autoloadstatus = value; OnPropertyChanged("Autoloadstatus"); }
	}


	//travelcard.cgi_autoloadstatusname,
	private string _autoloadstatusname;
	public string Autoloadstatusname {
	  get { return _autoloadstatusname; }
	  set { _autoloadstatusname = value; OnPropertyChanged("Autoloadstatusname"); }
	}


	//travelcard.cgi_autoloadconnectiondate,
	private string _autoloadconnectiondate;
	public string Autoloadconnectiondate {
	  get { return _autoloadconnectiondate; }
	  set { _autoloadconnectiondate = value; OnPropertyChanged("Autoloadconnectiondate"); }
	}


	//travelcard.cgi_autoloaddisconnectiondate
	private string _autoloaddisconnectiondate;
	public string Autoloaddisconnectiondate {
	  get { return _autoloaddisconnectiondate; }
	  set { _autoloaddisconnectiondate = value; OnPropertyChanged("Autoloaddisconnectiondate"); }
	}

	//travelcard.cgi_creditcardmask
	private string _creditcardmask;
	public string Creditcardmask {
	  get { return _creditcardmask; }
	  set { _creditcardmask = value; OnPropertyChanged("Creditcardmask"); }
	}

	//travelcard.cgi_creditcardmask
	private string _failedattemptstochargemoney;
	public string Failedattemptstochargemoney {
	  get { return _failedattemptstochargemoney; }
	  set { _failedattemptstochargemoney = value; OnPropertyChanged("Failedattemptstochargemoney "); }
	}

	//<cgi_contactnumber>5</cgi_contactnumber>
	private string _contactnumber;
	public string Contactnumber {
	  get { return _contactnumber; }
	  set { _contactnumber = value; OnPropertyChanged("Contactnumber"); }
	}

	//Accountnumber

	//<cgi_accountNumber>5</cgi_accountNumber>
	private string _accountNumber;
	public string AccountNumber {
	  get { return _accountNumber; }
	  set { _accountNumber = value; OnPropertyChanged("AccountNumber"); }
	}

	private RelayCommand _commandOpenEntity;
	public RelayCommand
	  CommandOpenEntity {
	  get { return _commandOpenEntity; }
	  set { _commandOpenEntity = value; }
	}

	public string Contact {
	  get {
		if (ContactId != null)
		  return string.Format("{0} : {1}", Contactnumber, ContactIdName);
		else
		  return string.Format("{0} : {1}", AccountNumber, AccountIdName);
	  }
	}
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public string GetDateFrom {
	  get {
		if (!string.IsNullOrEmpty(_validFrom)) {
		  DateTime _date;
		  if (DateTime.TryParse(_validFrom, out _date)) {
			_date = Convert.ToDateTime(_validFrom);
			return _date.ToShortDateString();
		  }
		  else {
			return null;
		  }
		}
		else {
		  return null;
		}
	  }
	}

	public string GetDateTo {
	  get {
		if (!string.IsNullOrEmpty(_validTo)) {
		  DateTime _date;
		  if (DateTime.TryParse(_validTo, out _date)) {
			_date = Convert.ToDateTime(_validTo);
			return _date.ToShortDateString();
		  }
		  else {
			return null;
		  }
		}
		else {
		  return null;
		}
	  }
	}

	public string GetBlocked {
	  get {
		if (_blocked != null) {
		  if (_blocked == 0)
			return "Nej";
		  else
			return "Ja";
		}
		else {
		  return "Nej";
		}
	  }
	}

	public string GetAutoLoadStatus {
	  get {
		if (_autoloadstatus == 1)
		  return "Ja";
		else
		  return "Nej";
	  }
	}

	public string GetAutoLoadStartDate {
	  get {
		if (!string.IsNullOrEmpty(_autoloadconnectiondate)) {
		  DateTime _date;
		  if (DateTime.TryParse(_autoloadconnectiondate, out _date)) {
			_date = Convert.ToDateTime(_autoloadconnectiondate);
			return _date.ToShortDateString();
		  }
		  else {
			return null;
		  }
		}
		else {
		  return null;
		}
	  }
	}

	public string GetAutoLoadEndDate {
	  get {
		if (!string.IsNullOrEmpty(_autoloaddisconnectiondate)) {
		  DateTime _date;
		  if (DateTime.TryParse(_autoloaddisconnectiondate, out _date)) {
			_date = Convert.ToDateTime(_autoloaddisconnectiondate);
			return _date.ToShortDateString();
		  }
		  else {
			return null;
		  }
		}
		else {
		  return null;
		}
	  }
	}

	public string GetValidStartDate {
	  get {
		if (!string.IsNullOrEmpty(_validFrom)) {
		  DateTime _date;
		  if (DateTime.TryParse(_validFrom, out _date)) {
			_date = Convert.ToDateTime(_validFrom);
			return _date.ToShortDateString();
		  }
		  else {
			return null;
		  }
		}
		else {
		  return null;
		}
	  }
	}

	public void Clone(Card card) {
	  Type _t = typeof(Card);
	  PropertyInfo[] _props = _t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

	  Type _tme = typeof(cardEXT);
	  PropertyInfo[] _propsme = _tme.GetProperties(BindingFlags.Public | BindingFlags.Instance);

	  foreach (PropertyInfo p in _props) {
		PropertyInfo _found = _propsme.FirstOrDefault(x => x.Name == p.Name);
		if (_found != null) {
		  _found.SetValue(this, p.GetValue(card, null), null);
		}
	  }
	}

	private void _commandOpenEntityAction(object obj) {
	  //MessageBox.Show(CreateContactUri() + " " + ContactId);
	  CreateContactUri();
	}

	private void CreateContactUri() {
	  _crmManager.Fetch<setting>(CRM.GetBIFFTransactions.ViewModel.MainPageViewModel._xmlSettings(), fetchCallback);
	}

	private void fetchCallback(System.Collections.ObjectModel.ObservableCollection<setting> settingCollection) {
	  if (settingCollection.Count > 0) {
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(settingCollection.First().CrmUri);

		System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() => {
		  stringBuilder.Append("/main.aspx?etn=");
		  stringBuilder.Append(ContactId != null ? "contact" : "account");
		  stringBuilder.Append("&pagetype=entityrecord&");
		  stringBuilder.Append("id=%7B");
		  stringBuilder.Append(ContactId != null ? ContactId.ToString() : AccountId.ToString());
		  stringBuilder.Append("%7D");
		  stringBuilder.Append("&cmdbar=false");
		  stringBuilder.Append("&navbar=off");
		  System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(stringBuilder.ToString()));
		  //System.Windows.Browser.HtmlPage.
		});
	  }
	}

	//private void callback(System.Collections.ObjectModel.ObservableCollection<TEntity> obj) {
	//  throw new NotImplementedException();
	//}
  }
}
