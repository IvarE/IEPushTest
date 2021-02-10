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

namespace CRM.GetBIFFTransactions
{
    public class travelCard : XrmBaseNotify
    {
        //CardInformation
        //<ns0:CardIssuer>Skånetrafiken CSC</ns0:CardIssuer>
        private string _cardIssuer;
        public string CardIssuer
        {
            get { return _cardIssuer; }
            set { _cardIssuer = value; OnPropertyChanged("CardIssuer"); }
        }

        //<ns0:CardKind>4</ns0:CardKind>
        private string _cardKind;
        public string CardKind
        {
            get { return _cardKind; }
            set { _cardKind = value; OnPropertyChanged("CardKind"); }
        }

        //<ns0:CardHotlisted>true</ns0:CardHotlisted>
        private string _cardHotlisted;
        public string CardHotlisted
        {
            get { return _cardHotlisted; }
            set { _cardHotlisted = value; OnPropertyChanged("CardHotlisted"); }
        }

        //<ns0:CardTypePeriod>0</ns0:CardTypePeriod>
        private string _cardTypePeriod;
        public string CardTypePeriod
        {
            get { return _cardTypePeriod; }
            set { _cardTypePeriod = value; }
        }

        //<ns0:CardTypeValue>3161</ns0:CardTypeValue>
        private string _cardTypeValue;
        public string CardTypeValue
        {
            get { return _cardTypeValue; }
            set { _cardTypeValue = value; }
        }

        //<ns0:CardValueProductType>RESKASSA SKÅNE</ns0:CardValueProductType> 
        private string _cardValueProductType;
        public string CardValueProductType
        {
            get { return _cardValueProductType; }
            set { _cardValueProductType = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //PurseDetails

        //<ns0:CardCategory>4</ns0:CardCategory>
        private string _pursecardCategory;
        public string PurseCardCategory
        {
            get { return _pursecardCategory; }
            set { _pursecardCategory = value; OnPropertyChanged("PurseCardCategory"); }
        }

        //<ns0:Balance>200</ns0:Balance>
        private string _pursebalance;
        public string PurseBalance
        {
            get { return _pursebalance; }
            set { _pursebalance = value; OnPropertyChanged("PurseBalance"); }
        }

        //<ns0:Currency>SEK</ns0:Currency> 
        private string _pursecurrency;
        public string PurseCurrency
        {
            get { return _pursecurrency; }
            set { _pursecurrency = value; OnPropertyChanged("PurseCurrency"); }
        }

        //<ns0:OutstandingDirectedAutoload>true</ns0:OutstandingDirectedAutoload>
        private string _purseoutstandingDirectedAutoload;
        public string PurseOutstandingDirectedAutoload
        {
            get { return _purseoutstandingDirectedAutoload; }
            set { _purseoutstandingDirectedAutoload = value; OnPropertyChanged("PurseOutstandingDirectedAutoload"); }
        }

        //<ns0:OutstandingEnableThresholdAutoload>false</ns0:OutstandingEnableThresholdAutoload>
        private string _purseoutstandingEnableThresholdAutoload;
        public string PurseOutstandingEnableThresholdAutoload
        {
            get { return _purseoutstandingEnableThresholdAutoload; }
            set { _purseoutstandingEnableThresholdAutoload = value; OnPropertyChanged("PurseOutstandingEnableThresholdAutoload"); }
        }

        //<ns0:Hotlisted>false</ns0:Hotlisted>
        private string _pursehotlisted;
        public string PurseHotlisted
        {
            get { return _pursehotlisted; }
            set { _pursehotlisted = value; OnPropertyChanged("PurseHotlisted"); }
        }

        //<ns0:HotlistReason>Lost</ns0:HotlistReason>
        //HotlistReason
        private string _pursehotlistReason;
        public string PurseHotlistReason
        {
            get { return _pursehotlistReason; }
            set { _pursehotlistReason = value; OnPropertyChanged("PurseHotlistReason"); }
        }

        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //PeriodDetails

        //<ns0:CardCategory>2</ns0:CardCategory> 
        private string _periodCardCategory;
        public string PeriodCardCategory
        {
            get { return _periodCardCategory; }
            set { _periodCardCategory = value; OnPropertyChanged("PeriodCardCategory"); }
        }
        
        //<ns0:ProductType>SKÅNEKORT 3 ZONER</ns0:ProductType> 
        private string _periodProductType;
        public string PeriodProductType
        {
            get { return _periodProductType; }
            set { _periodProductType = value; OnPropertyChanged("PeriodProductType"); }
        }
        
        //<ns0:PeriodStart>2014-10-24T00:00:00+02:00</ns0:PeriodStart> 
        private string _periodPeriodStart;
        public string PeriodPeriodStart
        {
            get { return _periodPeriodStart; }
            set { _periodPeriodStart = value; OnPropertyChanged("PeriodPeriodStart"); }
        }
        
        //<ns0:PeriodEnd>2014-11-22T00:00:00+01:00</ns0:PeriodEnd> 
        private string _periodPeriodEnd;
        public string PeriodPeriodEnd
        {
            get { return _periodPeriodEnd; }
            set { _periodPeriodEnd = value; OnPropertyChanged("PeriodPeriodEnd"); }
        }
        
        //<ns0:WaitingPeriods>0</ns0:WaitingPeriods> 
        private string _periodWaitingPeriods;
        public string PeriodWaitingPeriods
        {
            get { return _periodWaitingPeriods; }
            set { _periodWaitingPeriods = value; OnPropertyChanged("PeriodWaitingPeriods"); }
        }
        
        //<ns0:ZoneListID>1</ns0:ZoneListID> 
        private string _periodZoneListID;
        public string PeriodZoneListID
        {
            get { return _periodZoneListID; }
            set { _periodZoneListID = value; OnPropertyChanged("PeriodZoneListID"); }
        }
        
        //<ns0:PricePaid>58000</ns0:PricePaid> 
        private string _periodPricePaid;
        public string PeriodPricePaid
        {
            get { return _periodPricePaid; }
            set { _periodPricePaid = value; OnPropertyChanged("PeriodPricePaid"); }
        }
        
        //<ns0:Currency>SEK</ns0:Currency> 
        private string _periodCurrency;
        public string PeriodCurrency
        {
            get { return _periodCurrency; }
            set { _periodCurrency = value; OnPropertyChanged("PeriodCurrency"); }
        }
        
        //<ns0:OutstandingDirectedAutoload>false</ns0:OutstandingDirectedAutoload> 
        private string _periodOutstandingDirectedAutoload;
        public string PeriodOutstandingDirectedAutoload
        {
            get { return _periodOutstandingDirectedAutoload; }
            set { _periodOutstandingDirectedAutoload = value; OnPropertyChanged("PeriodOutstandingDirectedAutoload"); }
        }
        
        //<ns0:OutstandingEnableThresholdAutoload>false</ns0:OutstandingEnableThresholdAutoload> 
        private string _periodOutstandingEnableThresholdAutoload;
        public string PeriodOutstandingEnableThresholdAutoload
        {
            get { return _periodOutstandingEnableThresholdAutoload; }
            set { _periodOutstandingEnableThresholdAutoload = value; OnPropertyChanged("PeriodOutstandingEnableThresholdAutoload"); }
        }
        
        //<ns0:Hotlisted>false</ns0:Hotlisted> 
        private string _periodHotlisted;
        public string PeriodHotlisted
        {
            get { return _periodHotlisted; }
            set { _periodHotlisted = value; OnPropertyChanged("PeriodHotlisted"); }
        }
        
        //<ns0:ContractSerialNumber>1</ns0:ContractSerialNumber> 
        private string _periodContractSerialNumber;
        public string PeriodContractSerialNumber
        {
            get { return _periodContractSerialNumber; }
            set { _periodContractSerialNumber = value; OnPropertyChanged("PeriodContractSerialNumber"); }
        }

        private ObservableCollection<zoneEXT> _zoneList = new ObservableCollection<zoneEXT>();
        public ObservableCollection<zoneEXT> ZoneList
        {
            get { return _zoneList; }
            set { _zoneList = value; OnPropertyChanged("ZoneList"); }
        }

        private ObservableCollection<routeEXT> _routeList = new ObservableCollection<routeEXT>();
        public ObservableCollection<routeEXT> RouteList
        {
            get { return _routeList; }
            set { _routeList = value; OnPropertyChanged("RouteList"); }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string GetStartDate
        {
            get
            {
                if (!string.IsNullOrEmpty(_periodPeriodStart))
                {
                    DateTime _dateValue;
                    if (DateTime.TryParse(_periodPeriodStart, out _dateValue))
                    {
                        DateTime _dateTime = Convert.ToDateTime(_periodPeriodStart);
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

        public string GetEndDate
        {
            get
            {
                if (!string.IsNullOrEmpty(_periodPeriodEnd))
                {
                    DateTime _dateValue;
                    if (DateTime.TryParse(_periodPeriodEnd, out _dateValue))
                    {
                        DateTime _dateTime = Convert.ToDateTime(_periodPeriodEnd);
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

        public string GetPurceBalance
        {
            get { 
                if (!string.IsNullOrEmpty(_pursebalance))
                {
                    decimal _outValue;
                    string _value = _pursebalance.Replace(".", ",");
                    if (decimal.TryParse(_value, out _outValue))
                    {
                        _outValue = Convert.ToDecimal(_value);
                        return _outValue.ToString("0.00");
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

        public string GetPeriodPricePaid
        {
            get
            {
                if (!string.IsNullOrEmpty(_periodPricePaid))
                {
                    decimal _outValue;
                    string _value = _periodPricePaid.Replace(".", ",");
                    if (decimal.TryParse(_value, out _outValue))
                    {
                        _outValue = Convert.ToDecimal(_value);
                        return _outValue.ToString("0.00");
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

        public string GetCardTypeValue
        {
            get
            {
                if (!string.IsNullOrEmpty(_cardTypeValue))
                {
                    decimal _outValue;
                    string _value = _cardTypeValue.Replace(".", ",");
                    if (decimal.TryParse(_value, out _outValue))
                    {
                        _outValue = Convert.ToDecimal(_value);
                        return _outValue.ToString("0.00");
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


    }
}
