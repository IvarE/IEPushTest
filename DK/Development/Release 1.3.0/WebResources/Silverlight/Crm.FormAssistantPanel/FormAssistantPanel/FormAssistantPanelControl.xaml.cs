using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Crm.FormAssistantPanel.ViewModels;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Dynamic;

namespace Crm.FormAssistantPanel
{
    public partial class FormAssistantPanelControl : UserControl
    {
        FormAssistantPanelControlViewModel formAssistantPanelControlViewModel;
        public FormAssistantPanelControl()
        {
            formAssistantPanelControlViewModel = new FormAssistantPanelControlViewModel();
            this.DataContext = formAssistantPanelControlViewModel;
            InitializeComponent();

        }
        
        //public void FormAssistantPanelControl(string Columns)
        public void AddColumns(string Columns)
        {
            string strCols = string.Empty;
            strCols = Columns;
            
            //InitializeComponent();                  
            //this.DataContext = new FormAssistantPanelControlViewModel();

            string[] colHeadders = strCols.Split(',', '|');
            for (int iPos = 1; iPos <= colHeadders.Length; iPos += 2)
            {
                #region commendted code
                //if (iPos == 1)
                //{
                
                //    //DataGridCheckBoxColumn chkBoxColumn = new DataGridCheckBoxColumn();
                //    //chkBoxColumn.Binding = new Binding()
                //    //{
                //    //    Mode = BindingMode.TwoWay,
                //    //    Path = new PropertyPath("IsSelected"),
                //    //};
                //    //ViewDataGrid.Columns.Add(chkBoxColumn);
                //    //string templateDefaultColumnXAML = @"<DataTemplate xmlns='http://schemas.microsoft.com/client/2007' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'><StackPanel Orientation='Horizontal'><Image Margin='5,0,0,0'></Image><CheckBox Margin='5,0,5,0' IsChecked='{Binding Path=IsSelected, Mode=TwoWay)}'/></StackPanel></DataTemplate>";


                //    //string templateDefaultColumnXAML = @"<DataTemplate xmlns='http://schemas.microsoft.com/client/2007' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'><StackPanel Orientation='Horizontal'><CheckBox IsChecked='{Binding Path=IsSelected, Mode=TwoWay)}'></CheckBox><Image Margin='5,0,0,0'></Image></StackPanel></DataTemplate>";
                //    //DataGridTemplateColumn templateDefaultColumn = new DataGridTemplateColumn();
                //    //templateDefaultColumn.CellTemplate = (DataTemplate)XamlReader.Load(templateDefaultColumnXAML);
                //    //templateDefaultColumn.Header = colHeadders[iPos];
                //    //ViewDataGrid.Columns.Add(templateDefaultColumn);

                //    DataGridTemplateColumn templateDefaultColumn = new DataGridTemplateColumn();
                //    templateDefaultColumn.CellTemplate = (DataTemplate)Resources["firstColumnTemplate"];
                //    //templateDefaultColumn.Header = colHeadders[iPos];
                //    ViewDataGrid.Columns.Add(templateDefaultColumn);
                    
                    
                //}
                ////else
                ////{
                ////DataGridTextColumn textColumn = new DataGridTextColumn();
                ////textColumn.Header = colHeadders[iPos];
                ////textColumn.Binding = new Binding(colHeadders[iPos]);
                ////ViewDataGrid.Columns.Add(textColumn);
                #endregion

                //if (iPos == 1)
                //{
                //    DataGridTemplateColumn templateDefaultColumn = new DataGridTemplateColumn();
                //    templateDefaultColumn.CellTemplate = (DataTemplate)Resources["firstColumnTemplate"];
                //    //templateDefaultColumn.Header = colHeadders[iPos];
                //    templateDefaultColumn.HeaderStyle = (Style)App.Current.Resources["DataGridColumnStyle"];
                //    ViewDataGrid.Columns.Add(templateDefaultColumn);
                //}
                if ((colHeadders[iPos].ToLowerInvariant().Contains("[h]")))
                    break;

                string strHeadder = string.Empty;
                if (colHeadders[iPos].ToLowerInvariant().Contains("[p]"))
                    strHeadder=colHeadders[iPos].Replace("[P]", "").Replace("[p]", "");
                else
                    strHeadder=colHeadders[iPos];



                string templateColumnXAML = @"<DataTemplate xmlns='http://schemas.microsoft.com/client/2007' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'><StackPanel Orientation='Horizontal' Background='Transparent'><TextBlock VerticalAlignment='Center' Text='{Binding Path=" + strHeadder + ")}'/></StackPanel></DataTemplate>";
                DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
                templateColumn.CellTemplate = (DataTemplate)XamlReader.Load(templateColumnXAML);
                templateColumn.Header = strHeadder;
                templateColumn.HeaderStyle = (Style)App.Current.Resources["DataGridColumnStyle"];
                ViewDataGrid.Columns.Add(templateColumn);
                

            }
           
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(FormAssistantPanelControl), null);
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
                formAssistantPanelControlViewModel.Title = value;
            }
        }

        public static readonly DependencyProperty ViewProperty = DependencyProperty.Register("View", typeof(ObservableCollection<View>), typeof(FormAssistantPanelControl), null);
        public ObservableCollection<View> View
        {
            get { return(ObservableCollection<View>)GetValue(ViewProperty); }
            set
            {
                SetValue(ViewProperty, value);
                formAssistantPanelControlViewModel.View=value;
            }
        }

        public static readonly DependencyProperty ColumnHeaderProperty =DependencyProperty.Register("ColumnHeader", typeof(string), typeof(FormAssistantPanelControl), null);
        public string ColumnHeader
        {
            get { return (string)GetValue(ColumnHeaderProperty); }
            set { SetValue(ColumnHeaderProperty, value);
            AddColumns(value);
            }
        }

        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize", typeof(int), typeof(FormAssistantPanelControl), null);
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set
            {
                SetValue(PageSizeProperty, value);
                formAssistantPanelControlViewModel.PageSize = value; 
            }
        }
        public static readonly DependencyProperty ReturnAttributeProperty = DependencyProperty.Register("ReturnAttribute", typeof(string), typeof(FormAssistantPanelControl), null);
        public string ReturnAttribute
        {
            get { return (string)GetValue(ReturnAttributeProperty); }
            set
            {
                SetValue(ReturnAttributeProperty, value);
                formAssistantPanelControlViewModel.ReturnAttribute = value;
            }
        }

        public static readonly DependencyProperty TargetAttributeProperty = DependencyProperty.Register("TargetAttribute", typeof(string), typeof(FormAssistantPanelControl), null);
        public string TargetAttribute
        {
            get { return (string)GetValue(TargetAttributeProperty); }
            set
            {
                SetValue(TargetAttributeProperty, value);
                formAssistantPanelControlViewModel.TargetAttribute = value;
            }
        }

        public static readonly DependencyProperty DescriptionAttributeProperty = DependencyProperty.Register("DescriptionAttribute", typeof(string), typeof(FormAssistantPanelControl), null);
        public string DescriptionAttribute
        {
            get { return (string)GetValue(DescriptionAttributeProperty); }
            set
            {
                SetValue(DescriptionAttributeProperty, value);
                formAssistantPanelControlViewModel.DescriptionAttribute = value;
            }
        }

        public static readonly DependencyProperty ShowDescriptionPanelProperty = DependencyProperty.Register("ShowDescriptionPanel", typeof(bool), typeof(FormAssistantPanelControl), null);
        public bool ShowDescriptionPanel
        {
            get { return (bool)GetValue(ShowDescriptionPanelProperty); }
            set
            {
                SetValue(ShowDescriptionPanelProperty, value);
                formAssistantPanelControlViewModel.ShowDescriptionPanel = value;
            }
        }
        private void DataGridDoubleClickBehavior_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            //View selectedRow = (View)((DataGrid)sender).SelectedItem;

            View selectedRow = ((View)((DataGridRow)sender).DataContext);
            formAssistantPanelControlViewModel.OpenRecord(selectedRow);
        }

        

        private void ViewDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ReturnAttribute) && !string.IsNullOrEmpty(this.TargetAttribute))
            {
                ContextMenu contextMenu = new ContextMenu();
                MenuItem mnuCopyData = new MenuItem() { Header = "Copy Data" };
                mnuCopyData.Command = formAssistantPanelControlViewModel.CopyDataCommand;
                mnuCopyData.CommandParameter = e.Row;
                contextMenu.Items.Add(mnuCopyData);
                if (this.ShowDescriptionPanel)
                {
                    MenuItem mnuShowDescription = new MenuItem() { Header = "Show Description" };
                    mnuShowDescription.Command = formAssistantPanelControlViewModel.ShowDescriptionCommand;
                    mnuShowDescription.CommandParameter = e.Row;
                    contextMenu.Items.Add(mnuShowDescription);
                }
                ContextMenuService.SetContextMenu(e.Row, contextMenu);
            }
        }

        

    }
}
