using System;
using System.Windows;
using System.Windows.Input;

namespace Crm.FormAssistantPanel.Utilities
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> pParamHandler;
        private readonly Action pHandler;
        private bool pIsEnabled;

        public RelayCommand(Action<object> handler):this(handler, false)
        { }

        public RelayCommand(Action<object> handler, bool isEnabled)
        {
            pParamHandler = handler;
            IsEnabled = isEnabled;
        }

        public RelayCommand(Action handler):this(handler, false)
        { }

        public RelayCommand(Action handler, bool isEnabled)
        {
            pHandler = handler;
            IsEnabled = isEnabled;
        }


        public bool IsEnabled
        {
            get { return pIsEnabled; }
            set
            {
                if (value != pIsEnabled)
                {
                    pIsEnabled = value;
                    if (CanExecuteChanged != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            CanExecuteChanged(this, EventArgs.Empty);
                        });
                    }
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (this.pParamHandler != null)
                pParamHandler(parameter);
            else if (this.pHandler != null)
                pHandler();
        }
    }
}
