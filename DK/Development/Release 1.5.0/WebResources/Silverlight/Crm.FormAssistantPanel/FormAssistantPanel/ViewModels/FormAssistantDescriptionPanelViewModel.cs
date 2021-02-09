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

namespace Crm.FormAssistantPanel.ViewModels
{
    public class FormAssistantDescriptionPanelViewModel:ViewModelBase
    {
        string _windowTitle = "Description";

        public string WindowTitle
        {
            get { return _windowTitle; }
            set { _windowTitle = value;
            NotifyPropertyChanged("WindowTitle");
            }
        }
        string _title = "Description";

        public string Title
        {
            get { return _title; }
            set { _title = value;
            NotifyPropertyChanged("Title");
            }
        }
        string _description = string.Empty;

        public string Description
        {
            get { return _description; }
            set { _description = value;
            NotifyPropertyChanged("Description");
            }
        }

    }
}
