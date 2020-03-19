using System.Windows;
using System.Resources;
using System.ComponentModel;
using System.Windows.Browser;
using Crm.FormAssistantPanel.Utilities;
using CGIXrm.CrmSdk;

namespace Crm.FormAssistantPanel.ViewModels
{
    public class ViewModelBase :INotifyPropertyChanged
    {

        public ViewModelBase()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(PropertyChanged, this, new PropertyChangedEventArgs(propertyName));

            }
        }

        string waitMessage = "Please wait...";
        public string WaitMessage
        {
            get { return waitMessage; }
            set { waitMessage = value; }
        }

        bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; }
        }

        public void StartBusyIndicator(string message)
        {
            WaitMessage = message;
            IsBusy = true;
            NotifyPropertyChanged("WaitMessage");
            NotifyPropertyChanged("IsBusy");
        }

        public void StopBusyIndicator()
        {
            WaitMessage = "Please wait...";
            IsBusy = false;            
            NotifyPropertyChanged("WaitMessage");
            NotifyPropertyChanged("IsBusy");
        }

        public void HandleCrmException()
        {
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                //HtmlPage.Window.Alert(Resources.Generic.CrmException);
            });
        }
    }
}
