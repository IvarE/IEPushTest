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

namespace CRM.GetBIFFTransactions
{
    public class routeEXT : XrmBaseNotify
    {

        //<ns0:RouteListID>1</ns0:RouteListID>
        private string _routeListID;
        public string RouteListID
        {
            get { return _routeListID; }
            set { _routeListID = value; OnPropertyChanged("RouteListID"); }
        }

        //<ns0:Route>4010</ns0:Route>
        private string _route;
        public string Route
        {
            get { return _route; }
            set { _route = value; OnPropertyChanged("Route"); }
        }

        //<ns0:FromZone>0</ns0:FromZone>
        private string _fromZone;
        public string FromZone
        {
            get { return _fromZone; }
            set { _fromZone = value; OnPropertyChanged("FromZone"); }
        }

        //<ns0:ToZone>0</ns0:ToZone>
        private string _toZone;
        public string ToZone
        {
            get { return _toZone; }
            set { _toZone = value; OnPropertyChanged("ToZone"); }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private string _routeCaption;
        public string RouteCaption
        {
            get { return _routeCaption; }
            set { _routeCaption = value; OnPropertyChanged("RouteCaption"); }
        }

    }
}
