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
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Dynamic;
using Crm.FormAssistantPanel.Utilities;
using System.Windows.Controls.Primitives;

namespace Crm.FormAssistantPanel.ViewModels
{
    public class FormAssistantPanelControlViewModel:ViewModelBase
    {
        DataGrid viewDataGrid = new DataGrid();

        public FormAssistantPanelControlViewModel()
        {
            
        }
        string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { _title = value;
            NotifyPropertyChanged("Title");
            }
        }
        int _pageSize=3;
        public int PageSize
        {
          get { return _pageSize; }
          set { _pageSize = value;
          //NotifyPropertyChanged("PageSize");
          //NotifyPropertyChanged("PagedView");
          }
        }

        ObservableCollection<View> _view;
        public ObservableCollection<View> View
        {
            get { return _view; }
            set { _view = value;
                //NotifyPropertyChanged("View");
            PagedView = new PagedCollectionView(value);
            }
        }

        PagedCollectionView _pagedView;
        public PagedCollectionView PagedView
        {
            get
            {
                return _pagedView;
            }
            set
            {
                _pagedView = value;
                NotifyPropertyChanged("PagedView");
            }
        }


        string _returnAttribute = string.Empty;

        public string ReturnAttribute
        {
            get { return _returnAttribute; }
            set { _returnAttribute = value; }
        }

        string _targetAttribute = string.Empty;
        public string TargetAttribute
        {
            get { return _targetAttribute; }
            set { _targetAttribute = value; }
        }

        bool _showDescriptionPanel;

        public bool ShowDescriptionPanel
        {
            get { return _showDescriptionPanel; }
            set { _showDescriptionPanel = value;
            NotifyPropertyChanged("ShowDescriptionPanel");
            }
        }

        string _descriptionAttribute = string.Empty;
        public string DescriptionAttribute
        {
            get { return _descriptionAttribute; }
            set { _descriptionAttribute = value; }
        }
        
        

        RelayCommand _copyDataCommand;
        public RelayCommand CopyDataCommand
        {
            get {
                if (_copyDataCommand == null)
                    _copyDataCommand = new RelayCommand(CopyDataAction, true);
                return _copyDataCommand; }
            set { _copyDataCommand = value; }
        }        
        private void CopyDataAction(object sender)
        {
            View selectedRow = (View)((DataGridRow)sender).DataContext;
            DoProcess(selectedRow, ReturnAttribute, TargetAttribute);
        }
        RelayCommand _showDescriptionCommand;
        public RelayCommand ShowDescriptionCommand
        {
            get {
                if (_showDescriptionCommand == null)
                    _showDescriptionCommand = new RelayCommand(ShowDescriptionAction, ShowDescriptionPanel);
                return _showDescriptionCommand; }
            set { _showDescriptionCommand = value; }
        }
        private void ShowDescriptionAction(object sender)
        {
            View selectedRow = (View)((DataGridRow)sender).DataContext;
            string strDescription = (string)selectedRow.GetPropertyValue(DescriptionAttribute);
            if (!string.IsNullOrEmpty(strDescription))
            {
                FormAssistantDescriptionPanel faDescriptionPanel = new FormAssistantDescriptionPanel();
                faDescriptionPanel.Description = strDescription;
                faDescriptionPanel.VerticalAlignment = VerticalAlignment.Center;
                faDescriptionPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                faDescriptionPanel.Show();

                //XRMUtility.ShowAlertDialog(strDescription);
            }

        }
        public void DoProcess(View selectedRow,string returnAttribute,string targetAttribute)
        {
            dynamic returnValue = null;
            switch (selectedRow.AttributeDetails[returnAttribute])
            {
                case "lookup":
                    var lookUpDetail = ((LookUpDetails)(selectedRow.LookUpDetails[returnAttribute]));
                    if (lookUpDetail != null)
                    {
                        
                        int entityType;
                        returnValue = new ExpandoObject();
                        returnValue.isTypeSpecified = false;
                        
                        if (int.TryParse(lookUpDetail.EntityType, out entityType))
                            returnValue.isTypeSpecified = true;
                        
                        
                        returnValue.id=Convert.ToString(lookUpDetail.Id);
                        returnValue.entityType=lookUpDetail.EntityType;
                        returnValue.name=lookUpDetail.Name;
                    }
                    else
                    {
                        

                    }
                    break;
                case "statecode":
                case "optionset":
                    //returnValue.Name = selectedRow.GetPropertyValue(returnAttribute);
                    returnValue = Convert.ToInt32(selectedRow.UnFormattedValues[returnAttribute]);
                    break;
                case "date":
                    returnValue = DateTime.Parse(selectedRow.UnFormattedValues[returnAttribute]);
                    break;
                case "text":
                default:
                    returnValue = (string)selectedRow.UnFormattedValues[returnAttribute];
                    break;
            }

            XRMUtility.SetAttributeValue(targetAttribute,returnValue);
        }

        public void OpenRecord(View selectedRow)
        {
            XRMUtility.OpenCrmRecord(selectedRow.ViewEntityName,new Guid((string)(selectedRow.GetPropertyValue(selectedRow.ViewEntityName + "id[H]"))));
        }
        

    }
}
