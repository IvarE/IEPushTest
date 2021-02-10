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
    public class zoneEXT : XrmBaseNotify
    {

        private string _zoneListID;
        public string ZoneListID
        {
            get { return _zoneListID; }
            set { _zoneListID = value; OnPropertyChanged("ZoneListID"); }
        }
        
        private string _zone;
        public string Zone
        {
            get { return _zone; }
            set { _zone = value; OnPropertyChanged("Zone"); }
        }

        private string _zoneName;
        public string ZoneName
        {
            get { return _zoneName; }
            set { _zoneName = value; OnPropertyChanged("ZoneName"); }
        }

        private string _zoneCaption;
        public string ZoneCaption
        {
            get { return _zoneCaption; }
            set { _zoneCaption = value; OnPropertyChanged("ZoneCaption"); }
        }



    }
}
