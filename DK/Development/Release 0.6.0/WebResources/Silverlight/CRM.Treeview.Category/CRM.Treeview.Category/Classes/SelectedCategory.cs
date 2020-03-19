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
using CRM.Treeview.Category.ViewModel;

namespace CRM.Treeview.Category.Classes
{
    public class SelectedCategory
    {

        public SelectedCategory()
        {
            _deleteCommand = new RelayCommand(_deleteCommand_action);
        }

        public Guid CaseCategoryId { get; set; }
        public string Caption { get; set; }
        public Guid? Category1 { get; set; }
        public Guid? Category2 { get; set; }
        public Guid? Category3 { get; set; }

        public MainPageViewModel ParentViewModel { get; set; }
        public CaseCategory ParentCaseCategory { get; set; }
        public ItemRow ItemRow { get; set; }

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _deleteCommand; }
            set { _deleteCommand = value; }
        }

        private void _deleteCommand_action()
        {
            ParentViewModel.DeleteSelection(this);
        }

    }
}
