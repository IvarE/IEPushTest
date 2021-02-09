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

namespace CRM.Travel.Information.Classes
{
    public class TransportType : XrmBaseEntity
    {

        private int _id;
        public int Id
        {
            get { return _id; }
            set 
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        private string _caption;
        public string Caption
        {
            get { return _caption; }
            set 
            {
                if (_caption != value)
                {
                    _caption = value;
                    OnPropertyChanged("Caption");
                }
            }
        }


    }
}
