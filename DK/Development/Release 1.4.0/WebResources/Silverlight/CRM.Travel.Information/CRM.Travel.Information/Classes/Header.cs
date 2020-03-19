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

using System.Collections.ObjectModel;
using CRM.Travel.Information.Classes;
using CRM.Travel.Information.ViewModel;
using CGIXrm;
using CGIXrm.CrmSdk;

using CRM.Travel.Information.PubTrans;

namespace CRM.Travel.Information.Classes
{
    public class Header : XrmBaseNotify
    {

        public Header() { }

        private bool _isWaiting;
        public bool IsWaiting
        {
            get { return _isWaiting; }
            set { _isWaiting = value; OnPropertyChanged("IsWaiting"); }
        }

        private bool _isLoadingWaiting;
        public bool IsLoadingWaiting
        {
            get { return _isLoadingWaiting; }
            set { _isLoadingWaiting = value; OnPropertyChanged("IsLoadingWaiting"); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private ObservableCollection<TransportType> _transportTypeList = new ObservableCollection<TransportType>();
        public ObservableCollection<TransportType> TransportTypeList
        {
            get { return _transportTypeList; }
            set {
                if (_transportTypeList != value)
                {
                    _transportTypeList = value;
                    OnPropertyChanged("TransportTypeList");
                }
            }
        }

        private TransportType _selectedTransport;
        public TransportType SelectedTransport
        {
            get { return _selectedTransport; }
            set { _selectedTransport = value; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private ObservableCollection<PubTrans.Line> _lineSelection = new ObservableCollection<PubTrans.Line>();
        public ObservableCollection<PubTrans.Line> LineSelection
        {
            get { return _lineSelection; }
            set 
            {
                if (_lineSelection != value)
                {
                    _lineSelection = value;
                    OnPropertyChanged("RegionbusSelection");
                }
            }
        }

        private ObservableCollection<PubTrans.StopArea> _trainStopSelection = new ObservableCollection<StopArea>();
        public ObservableCollection<PubTrans.StopArea> TrainStopSelection
        {
            get { return _trainStopSelection; }
            set 
            {
                if (_trainStopSelection != value)
                {
                    _trainStopSelection = value;
                    OnPropertyChanged("TrainStopSelection");
                }
            }
        }

        private ObservableCollection<PubTrans.Zone> _cityBusSelection = new ObservableCollection<Zone>();
        public ObservableCollection<PubTrans.Zone> CityBusSelection
        {
            get { return _cityBusSelection; }
            set 
            {
                if (_cityBusSelection != null)
                {
                    _cityBusSelection = value;
                    OnPropertyChanged("CityBusSelection");
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private PubTrans.Zone _selectedCity = new Zone();
        public PubTrans.Zone SelectedCity
        {
            get { return _selectedCity; }
            set { _selectedCity = value; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private PubTrans.Line _selectedTour = new PubTrans.Line();
        public PubTrans.Line SelectedTour
        {
            get { return _selectedTour; }
            set { _selectedTour = value; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private ObservableCollection<PubTrans.StopArea> _stopAreaSelection = new ObservableCollection<StopArea>();
        public ObservableCollection<PubTrans.StopArea> StopAreaSelection
        {
            get { return _stopAreaSelection; }
            set 
            {
                if (_stopAreaSelection != value)
                {
                    _stopAreaSelection = value;
                    OnPropertyChanged("StopAreaSelection");
                }
            }
        }

        private PubTrans.StopArea _selectedStopAreaFrom;
        public PubTrans.StopArea SelectedStopAreaFrom
        {
            get { return _selectedStopAreaFrom; }
            set { _selectedStopAreaFrom = value; }
        }

        private PubTrans.StopArea _selectedStopAreaTo;
        public PubTrans.StopArea SelectedStopAreaTo
        {
            get { return _selectedStopAreaTo; }
            set { _selectedStopAreaTo = value; }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private ObservableCollection<TravelInformation> _travelInformationList = new ObservableCollection<TravelInformation>();
        public ObservableCollection<TravelInformation> TravelInformationList
        {
            get { return _travelInformationList; }
            set {
                if (_travelInformationList != value)
                {
                    _travelInformationList = value;
                    OnPropertyChanged("TravelInformationList");
                }
            }
        }

        
    }
}
