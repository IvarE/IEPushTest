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
using CRM.Treeview.Category.ViewModel;

namespace CRM.Treeview.Category.Classes
{
    public class ItemRow : XrmBaseEntity
    {

        public ItemRow() 
        {
            _Command1 = new RelayCommand(command1_action, false);
            _Command2 = new RelayCommand(command2_action, false);
            _Command3 = new RelayCommand(command3_action, false);
        }

        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }

        public string Name { get; set; }

        public Guid? Text1ID { get; set; }
        public Guid? Text2ID { get; set; }
        public Guid? Text3ID { get; set; }

        public System.Windows.TextDecorationCollection TextDec1 { get; set; }
        public System.Windows.TextDecorationCollection TextDec2 { get; set; }
        public System.Windows.TextDecorationCollection TextDec3 { get; set; }
 
        public Thickness BorderThickness1 { get; set; }
        public Thickness BorderThickness2 { get; set; }
        public Thickness BorderThickness3 { get; set; }

        public Brush BackgroundBrush1 { get; set; }
        public Brush BackgroundBrush2 { get; set; }
        public Brush BackgroundBrush3 { get; set; }

        public Visibility Visibility1 { get; set; }
        public Visibility Visibility2 { get; set; }
        public Visibility Visibility3 { get; set; }

        public int Sortorder1 { get; set; }
        public int Sortorder2 { get; set; }
        public int Sortorder3 { get; set; }

        public string CaseId { get; set; }
        public MainPageViewModel ParentViewModel { get; set; }
        
        // relay command

        private RelayCommand _Command1;
        public RelayCommand Command1
        {
            get
            {
                return _Command1;
            }
            set
            {
                _Command1 = value;
            }
        }

        private RelayCommand _Command2;
        public RelayCommand Command2
        {
            get
            {
                return _Command2;
            }
            set
            {
                _Command2 = value;
            }
        }

        private RelayCommand _Command3;
        public RelayCommand Command3
        {
            get
            {
                return _Command3;
            }
            set
            {
                _Command3 = value;
            }
        }

        // functions

        private void command1_action()
        {
            _saveCategoryOnCase();
        }

        private void command2_action()
        {
            _saveCategoryOnCase();
        }

        private void command3_action()
        {
            _saveCategoryOnCase();
        }

        private string _setName()
        {
            return string.Format("{0} {1} {2}", this.Text1, this.Text2, this.Text3);
        }

        private void _saveCategoryOnCase()
        {
            try
            {
                Name = _setName();
                if (ParentViewModel.CheckIfSelectionExists(this) == false)
                    ParentViewModel.AddToSelection(this);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
    }
}
