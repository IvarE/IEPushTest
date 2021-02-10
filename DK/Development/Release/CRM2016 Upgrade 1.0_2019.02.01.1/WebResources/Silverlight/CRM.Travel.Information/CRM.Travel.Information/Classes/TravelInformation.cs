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
using CRM.Travel.Information.ViewModel;

namespace CRM.Travel.Information.Classes
{
    public class TravelInformation : XrmBaseEntity
    {

        public TravelInformation()
        {
            _commandAdd = new RelayCommand(_commandAddAction, true);
            _commandDelete = new RelayCommand(_commandDeleteAction, true);
        }

        private string _travelInformationId;
        public string TravelInformationId
        {
            get { return _travelInformationId; }
            set { _travelInformationId = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //<DatedVehicleJourneyId>1120502298936317</DatedVehicleJourneyId>
        private string _datedVehicleJourneyId;
        public string DatedVehicleJourneyId
        {
            get { return _datedVehicleJourneyId; }
            set { _datedVehicleJourneyId = value; }
        }

        //<ServiceJourneyGid>9015012081801816</ServiceJourneyGid>
        private string _serviceJourneyGid;
        public string ServiceJourneyGid
        {
            get { return _serviceJourneyGid; }
            set { _serviceJourneyGid = value; }
        }

        //<OperatingDayDate>2014-12-29T00:00:00+01:00</OperatingDayDate>
        private DateTime _operatingDayDate;
        public DateTime OperatingDayDate
        {
            get { return _operatingDayDate; }
            set { _operatingDayDate = value; }
        }

        //<ContractorGid>9013012005300000</ContractorGid>
        private string _contractorGid;
        public string ContractorGid
        {
            get { return _contractorGid; }
            set { _contractorGid = value; }
        }

        //<LineDesignation>Pågatåg</LineDesignation>
        private string _lineDesignation;
        public string LineDesignation
        {
            get { return _lineDesignation; }
            set { _lineDesignation = value; }
        }

        //<JourneyNumber>1816</JourneyNumber>
        private string _journeyNumber;
        public string JourneyNumber
        {
            get { return _journeyNumber; }
            set { _journeyNumber = value; }
        }
        
        //Transport
        private string _transport;
        public string Transport
        {
            get { return _transport; }
            set
            {
                if (_transport != value)
                {
                    _transport = value;
                    OnPropertyChanged("Transport");
                }
            }
        }

        //City
        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        //Line
        private string _line;
        public string Line
        {
            get { return _line; }
            set
            {
                if (_line != value)
                {
                    _line = value;
                    OnPropertyChanged("Line");
                }
            }
        }

        private string _lineType;
        public string LineType
        {
            get { return _lineType; }
            set
            {
                if(_lineType != value)
                {
                    _lineType = value;
                    OnPropertyChanged("LineType");
                }
            }
        }

        //Tour
        private string _tour;
        public string Tour
        {
            get { return _tour; }
            set
            {
                if (_tour != value)
                {
                    _tour = value;
                    OnPropertyChanged("Tour");
                }
            }
        }

        //Contractor
        private string _contractor;
        public string Contractor
        {
            get { return _contractor; }
            set
            {
                if (_contractor != value)
                {
                    _contractor = value;
                    OnPropertyChanged("Contractor");
                }
            }
        }

        private string _trainNumber;
        public string TrainNumber
        {
            get { return _trainNumber; }
            set
            {
                if (_trainNumber != value)
                {
                    _trainNumber = value;
                    OnPropertyChanged("TrainNumber");
                }
            }
        }

        //Start
        private string _start;
        public string Start
        {
            get { return _start; }
            set
            {
                if (_start != value)
                {
                    _start = value;
                    OnPropertyChanged("Start");
                }
            }
        }

        //Stop
        private string _stop;
        public string Stop
        {
            get { return _stop; }
            set
            {
                if (_stop != value)
                {
                    _stop = value;
                    OnPropertyChanged("Stop");
                }
            }
        }

        //Startplanned
        private string _startPlanned;
        public string StartPlanned
        {
            get { return _startPlanned; }
            set {
                if (_startPlanned != value)
                {
                    _startPlanned = value;
                    OnPropertyChanged("StartPlanned");
                }
            }
        }

        //Startactual
        private string _startActual;
        public string StartActual
        {
            get { return _startActual; }
            set {
                if (_startActual != value)
                {
                    _startActual = value;
                    OnPropertyChanged("StartActual");
                }
            }
        }
        
        private string _startActualDateTime;
        public string StartActualDateTime
        {
            get { return _startActualDateTime; }
            set
            {
                if (_startActualDateTime != value)
                {
                    _startActualDateTime = value;
                    OnPropertyChanged("StartActualDateTime");
                }
            }
        }

        //Arivalplanned
        private string _arivalPlanned;
        public string ArivalPlanned
        {
            get { return _arivalPlanned; }
            set {
                if (_arivalPlanned != value)
                {
                    _arivalPlanned = value;
                    OnPropertyChanged("ArivalPlanned");
                }
            }
        }

        //Arivalactual
        private string _arivalActual;
        public string ArivalActual
        {
            get { return _arivalActual; }
            set {
                if (_arivalActual != value)
                {
                    _arivalActual = value;
                    OnPropertyChanged("ArivalActual");
                }
            }
        }
        
        private string _arrivalActualDateTime;
        public string ArrivalActualDateTime
        {
            get { return _arrivalActualDateTime; }
            set
            {
                if (_arrivalActualDateTime != value)
                {
                    _arrivalActualDateTime = value;
                    OnPropertyChanged("ArrivalActualDateTime");
                }
            }
        }

        private bool _failedToDepart;
        public bool FailedToDepart
        {
            get { return _failedToDepart; }
            set
            {
                if (_failedToDepart != value)
                {
                    _failedToDepart = value;
                    OnPropertyChanged("FailedToDepart");
                }
            }
        }

        private bool _failedToArrive;
        public bool FailedToArrive
        {
            get { return _failedToArrive; }
            set
            {
                if (_failedToArrive != value)
                {
                    _failedToArrive = value;
                    OnPropertyChanged("FailedToArrive");
                }
            }
        }

        private string _startDifference;
        public string StartDifference
        {
            get { return _startDifference; }
            set
            {
                if (_startDifference != value)
                {
                    _startDifference = value;
                    OnPropertyChanged("StartDifference");
                }
            }
        }

        private string _arrivalDifference;
        public string ArrivalDifference
        {
            get { return _arrivalDifference; }
            set
            {
                if (_arrivalDifference != value)
                {
                    _arrivalDifference = value;
                    OnPropertyChanged("ArrivalDifference");
                }
            }
        }

        //DirectionText
        private string _directionText;
        public string DirectionText
        {
            get { return _directionText; }
            set {
                if (_directionText != value)
                {
                    _directionText = value;
                    OnPropertyChanged("DirectionText");
                }
            }
        }

        //DirectionText orginal
        private string _directionTextOrg;
        public string DirectionTextOrg
        {
            get { return _directionTextOrg; }
            set
            {
                if (_directionTextOrg != value)
                {
                    _directionTextOrg = value;
                    OnPropertyChanged("DirectionTextOrg");
                }
            }
        }


        private string _displayText;
        public string DisplayText
        {
            get { return _displayText; }
            set { _displayText = value; }
        }

        private string _caseId;
        public string CaseId
        {
            get { return _caseId; }
            set { _caseId = value; }
        }

        private bool _hasArrivalDeviation;
        public bool HasArrivalDeviation
        {
            get { return _hasArrivalDeviation; }
            set { _hasArrivalDeviation = value; OnPropertyChanged("HasArrivalDeviation"); }
        }

        private bool _hasDepartureDeviation;
        public bool HasDepartureDeviation
        {
            get { return _hasDepartureDeviation; }
            set { _hasDepartureDeviation = value; OnPropertyChanged("HasDepartureDeviation"); }
        }

        private bool _hasServiceJourneyDeviation;
        public bool HasServiceJourneyDeviation
        {
            get { return _hasServiceJourneyDeviation; }
            set { _hasServiceJourneyDeviation = value; OnPropertyChanged("HasServiceJourneyDeviation"); }
        }

        private string _deviationMessage;
        public string DeviationMessage
        {
            get { return _deviationMessage; }
            set { _deviationMessage = value; }
        }

        private string _actualStartTimeDisplay;
        public string ActualStartTimeDisplay
        {
            get { return _actualStartTimeDisplay; }
            set { _actualStartTimeDisplay = value; }
        }

        private string _plannedStartTimeDisplay;
        public string PlannedStartTimeDisplay
        {
            get { return _plannedStartTimeDisplay; }
            set { _plannedStartTimeDisplay = value; }
        }

        private string _actualArivalTimeDisplay;
        public string ActualArivalTimeDisplay
        {
            get { return _actualArivalTimeDisplay; }
            set { _actualArivalTimeDisplay = value; }
        }

        private string _plannedArivalTimeDisplay;
        public string PlannedArivalTimeDisplay
        {
            get { return _plannedArivalTimeDisplay; }
            set { _plannedArivalTimeDisplay = value; }
        }

        public MainPageViewModel MainPageViewModel { get; set; }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Command

        private RelayCommand _commandAdd;
        public RelayCommand CommandAdd
        {
            get { return _commandAdd; }
            set { _commandAdd = value; }
        }

        private void _commandAddAction()
        {
            MainPageViewModel.AddToSelection(this);
        }

        private RelayCommand _commandDelete;
        public RelayCommand CommandDelete
        {
            get { return _commandDelete; }
            set { _commandDelete = value; }
        }

        private void _commandDeleteAction()
        {
            MainPageViewModel.DeleteFromSelection(this);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        


    }
}
