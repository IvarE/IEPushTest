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

namespace Crm.FormAssistantPanel
{
    public partial class FormAssistantDescriptionPanel : ChildWindow
    //public partial class FormAssistantDescriptionPanel : UserControl
    {
        FormAssistantDescriptionPanelViewModel faDescriptionPanelViewModel;
        public FormAssistantDescriptionPanel()
        {
            faDescriptionPanelViewModel = new FormAssistantDescriptionPanelViewModel();
            this.DataContext = faDescriptionPanelViewModel;
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = false;
            //this.Close();
        }
        public static readonly DependencyProperty DescriptionPanelProperty = DependencyProperty.Register("Description", typeof(string), typeof(FormAssistantDescriptionPanel), null);
        public string Description
        {
            get { return (string)GetValue(DescriptionPanelProperty); }
            set
            {
                SetValue(DescriptionPanelProperty, value);
                faDescriptionPanelViewModel.Description = value;
            }
        }
    }
}

