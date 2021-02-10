using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Crm.FormAssistantPanel.ViewModels;
using System.Windows.Browser;

namespace Crm.FormAssistantPanel
{
    public partial class FormAssistantPanel : UserControl
    {
        

        public FormAssistantPanel()
        {
            
            InitializeComponent();
            FormAssistantPanelViewModel fapVM = new FormAssistantPanelViewModel(FormAssistPanel);
            this.DataContext = fapVM;
            fapVM.InitStart();
        }
        [ScriptableMember]
        public void LoadData(string parameters)
        {

        }
    }
}
