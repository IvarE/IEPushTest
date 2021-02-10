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

using CRM.Treeview.Category.ViewModel;
using System.Windows.Data;
using CRM.Treeview.Category.Views;
using System.Windows.Browser;
using CRM.Treeview.Category.Classes;

namespace CRM.Treeview.Category
{
    public partial class MainPage : UserControl
    {

        public MainPageViewModel _mainViewModel { get; set; }
        public string Debug { get; set; }

        public MainPage()
        {
            try
            {
                InitializeComponent();
                _mainViewModel = new MainPageViewModel(this.grdGrid, this.lstSelection, this.Dispatcher, this.version);
                _mainViewModel.Init();
            }
            catch (Exception ex)
            {
                _showError(ex);     
            }
        }

        [ScriptableMember]
        public void SetEntityID(string entityid)
        {
            try
            {
                _mainViewModel.StartGetSavedCaseCategories(entityid);
            }
            catch (Exception ex)
            {
                _showError(ex);
            }
        }

        private void _showError(Exception ex)
        {
            ErrorPage _errorPage = new ErrorPage("", ex);
            _errorPage.Show();
        }

        
    }
}
