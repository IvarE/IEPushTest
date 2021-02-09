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
using System.Collections.ObjectModel;
using CRM.GetBIFFTransactions.ViewModel;

namespace CRM.GetBIFFTransactions
{
    public class travelCardTransaction : XrmBaseEntity
    {

        public travelCardTransaction()
        {
            _commandAdd = new RelayCommand(_commandAddAction, true);
            _commandDelete = new RelayCommand(_commandDeleteAction, true);
        }

        private Guid? _transactionid;
        public Guid? Transactionid
        {
            get { return _transactionid; }
            set { _transactionid = value; }
        }

        //<ns0:Date>2014-10-29T10:57:37+01:00</ns0:Date> 
        private string _date;
        public string Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged("Date"); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private string _showDate;
        public string ShowDate
        {
            get { return _showDate; }
            set { _showDate = value; OnPropertyChanged("ShowDate"); }
        }

        private string _showTime;

        public string ShowTime
        {
            get { return _showTime; }
            set { _showTime = value; OnPropertyChanged("ShowTime"); }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //<ns0:DeviceID>6518</ns0:DeviceID> 
        private string _deviceID;
        public string DeviceID
        {
            get { return _deviceID; }
            set { _deviceID = value; OnPropertyChanged("DeviceID"); }
        }

        //<ns0:CardSect>1</ns0:CardSect> 
        private string _cardSect;
        public string CardSect
        {
            get { return _cardSect; }
            set { _cardSect = value; OnPropertyChanged("CardSect"); }
        }

        //<ns0:RecType>72</ns0:RecType> 
        private string _recType;
        public string RecType
        {
            get { return _recType; }
            set { _recType = value; OnPropertyChanged("RecType"); }
        }

        //<ns0:TxnType>0</ns0:TxnType> 
        private string _txnType;
        public string TxnType
        {
            get { return _txnType; }
            set { _txnType = value; OnPropertyChanged("TxnType"); }
        }

        //<ns0:Route>Pendeln Lund - Åkarp - Malmö</ns0:Route> 
        private string _route;
        public string Route
        {
            get { return _route; }
            set { _route = value; OnPropertyChanged("Route"); }
        }

        //<ns0:Balance>17.2</ns0:Balance>
        private string _balance;
        public string Balance
        {
            get { return _balance; }
            set { _balance = value; OnPropertyChanged("Balance"); }
        }
        
        //<ns0:Amount>17.6</ns0:Amount>
        private string _amount;
        public string Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged("Amount"); }
        }

        //<ns0:OrigZone>2041</ns0:OrigZone> 
        private string _origZone;
        public string OrigZone
        {
            get { return _origZone; }
            set { _origZone = value; OnPropertyChanged("OrigZone"); }
        }

        private string _origZoneName;
        public string OrigZoneName
        {
            get { return _origZoneName; }
            set { _origZoneName = value; OnPropertyChanged("OrigZoneName"); }
        }

        //<ns0:DestZone>2041</ns0:DestZone>
        private string _destZone;
        public string DestZone
        {
            get { return _destZone; }
            set { _destZone = value; OnPropertyChanged("DestZone"); }
        }

        private string _destZonName;
        public string DestZonName
        {
            get { return _destZonName; }
            set { _destZonName = value; OnPropertyChanged("DestZonName"); }
        }

        private string _travelCard;
        public string TravelCard
        {
            get { return _travelCard; }
            set { _travelCard = value; OnPropertyChanged("TravelCard"); }
        }

        public MainPageViewModel MainPageViewModel { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string GetDate
        {
            get
            {
                if (!string.IsNullOrEmpty(_date))
                {
                    DateTime _dateValue;
                    if (DateTime.TryParse(_date, out _dateValue))
                    {
                        DateTime _dateTime = Convert.ToDateTime(_date);
                        return _dateTime.ToShortDateString();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public string GetTime
        {
            get
            {
                if (!string.IsNullOrEmpty(_date))
                {
                    DateTime _dateValue;
                    if (DateTime.TryParse(_date, out _dateValue))
                    {
                        DateTime _dateTime = Convert.ToDateTime(_date);
                        return _dateTime.ToShortTimeString();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public string GetName
        {
            get
            {
                string _cgi_travelcardtransaction = "<missing>";
                if (!string.IsNullOrEmpty(_date))
                    _cgi_travelcardtransaction = string.Format("{0}:{1}", GetDate, GetTime);

                if (!string.IsNullOrEmpty(_route))
                    _cgi_travelcardtransaction += string.Format(":{0}", _route);
                
                return _cgi_travelcardtransaction;
            }
        }




        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private RelayCommand _commandAdd;
        public RelayCommand CommandAdd
        {
            get { return _commandAdd; }
            set { _commandAdd = value; }
        }

        private void _commandAddAction()
        {
            MainPageViewModel.SaveTravelInformation(this);
        }

        private RelayCommand _commandDelete;
        public RelayCommand CommandDelete
        {
            get { return _commandDelete; }
            set { _commandDelete = value; }
        }

        private void _commandDeleteAction()
        {
            MainPageViewModel.DeleteTravelInformation(this);
        }

    }
}
