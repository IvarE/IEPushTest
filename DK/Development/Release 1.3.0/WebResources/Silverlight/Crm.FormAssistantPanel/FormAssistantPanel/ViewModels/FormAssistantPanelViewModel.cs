using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Crm.FormAssistantPanel.Utilities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Browser;
using System.Windows.Data;
using System.Text;

namespace Crm.FormAssistantPanel.ViewModels
{
    public class FormAssistantPanelViewModel:ViewModelBase
    {
        StackPanel formAssistPanel = new StackPanel();
        
        public FormAssistantPanelViewModel()
        {
            
        }

        public FormAssistantPanelViewModel(StackPanel formAssistPanelControl)
        {
            formAssistPanel = formAssistPanelControl;
            //FormType = 1;
        }


        //double _panelHeight = 0;
        //public double PanelHeight
        //{
        //    get {
        //        if (_panelHeight == 0)
        //        {
        //            string strHeight = (string)HtmlPage.Window.Invoke("GetPanelHeight", SilverlightUtility.CustomParameters["panelname"]);
        //            if (strHeight.EndsWith("px"))
        //            {
        //                _panelHeight = Convert.ToDouble(strHeight.Replace("px", ""));
        //            }
        //            else
        //            {
        //                _panelHeight = Convert.ToDouble(strHeight);
        //            }

        //        }
        //        return _panelHeight; }
        //    set { _panelHeight = value; }
        //}


        double _controlWidth = 0;
        public double ControlWidth
        {
            get {
                if (_controlWidth == 0)
                {
                    if(XRMUtility.CustomParameters.ContainsKey("width")&&string.IsNullOrEmpty(XRMUtility.CustomParameters["width"]))
                    {
                        string strHeight = XRMUtility.CustomParameters["width"];
                        if (strHeight.EndsWith("px"))
                        {
                            _controlWidth = Convert.ToDouble(strHeight.Replace("px", ""));
                        }
                        else
                        {
                            _controlWidth = Convert.ToDouble(strHeight);
                        }
                    }
                    else
                    {
                        _controlWidth = 300;
                    }

                }
               
                return _controlWidth;
            }
            set { _controlWidth = value; }
        }

        Orientation _controlOrientation = new Orientation();
        public Orientation ControlOrientation
        {
            get { 
                    switch(XRMUtility.GetOrientation())
                    {
                        case "horizantal":
                            _controlOrientation= Orientation.Horizontal;
                            break;
                        case "vertical":
                            _controlOrientation= Orientation.Vertical;
                            break;
                        default:
                            _controlOrientation = Orientation.Horizontal;
                            break;
                    }
                
                return _controlOrientation; }
            set { _controlOrientation = value;
            NotifyPropertyChanged("ControlOrientation");
            }
        }

        ObservableCollection<View> _viewCollection = new ObservableCollection<View>();
        public ObservableCollection<View> ViewCollection
        {
            get { return _viewCollection; }
            set
            {
                _viewCollection = value;
                NotifyPropertyChanged("ViewCollection");
            }
        }

        int _formType = 0;
        public int FormType
        {
            get { 
                
                if(_formType==0)
                    _formType=XRMUtility.GetFormType();

                return _formType; 
            
            }
            set { _formType = value; }
        }
        

        List<PanelConfiguration> _panelConfigurations;
        public List<PanelConfiguration> PanelConfigurations
        {
            get { return _panelConfigurations; }
            set { _panelConfigurations = value; }
        }

        public void InitStart()
        {
            CrmHelper crmHelper = new CrmHelper();
            CrmHelper.GetPanelConfigurationCallbackHandler getPanelConfigurationCallbackHandler = new CrmHelper.GetPanelConfigurationCallbackHandler(ProcessGetPanelConfigurationCallBack);
            crmHelper.GetPanelConfiguration(XRMUtility.GetEntityName(),getPanelConfigurationCallbackHandler);
        }

        private void ProcessGetPanelConfigurationCallBack(List<PanelConfiguration> panelConfigurations,bool requestStatus)
        {
            if (requestStatus)
            {

                if (panelConfigurations != null && panelConfigurations.Count > 0)
                {

                    PanelConfigurations = panelConfigurations;

                    CrmHelper crmHelper = new CrmHelper();
                    CrmHelper.ProcessViewDataRequestCallbackHandler processViewDataRequestCallbackHandler = new CrmHelper.ProcessViewDataRequestCallbackHandler(ProcessViewDataCallBack);

                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        foreach (PanelConfiguration panelConfig in PanelConfigurations)
                        {

                            //if (panelConfig.VisibleForCreate)
                            if (IsValidForCurrentForm(panelConfig.VisibleForCreate, panelConfig.VisibleForUpdate))
                            {
                                switch (panelConfig.QueryTypeName.ToLowerInvariant())
                                {
                                    case "systemview":
                                        crmHelper.ProcessCrmViewRequest(panelConfig.ViewGUID, panelConfig.ViewPrimaryEntity, panelConfig.ColumnHeader, true, panelConfig.RecordGuid, processViewDataRequestCallbackHandler);
                                        break;
                                    case "savedview":
                                        crmHelper.ProcessCrmViewRequest(panelConfig.ViewGUID, panelConfig.ViewPrimaryEntity, panelConfig.ColumnHeader, false, panelConfig.RecordGuid, processViewDataRequestCallbackHandler);
                                        break;
                                    case "fetchxml":

                                        StringBuilder fetchXML = new StringBuilder(panelConfig.FetchXML);

                                        if (!string.IsNullOrEmpty(panelConfig.FilterAttribute))
                                        {
                                            string[] filterAttributes = panelConfig.FilterAttribute.Split(',');
                                            if (filterAttributes.Length > 0)
                                            {
                                                for (int iNos = 0; iNos < filterAttributes.Length; iNos += 1)
                                                {
                                                    //L:Look Up ; C:Constant; S:String ; D:DateTime
                                                    string filterAttribute = filterAttributes[iNos].Substring(1, (filterAttributes[iNos].Length - 2));                                                    
                                                    string filterAttributeType = XRMUtility.GetAttributeType(filterAttribute);
                                                    if (string.IsNullOrEmpty(filterAttributeType))
                                                        continue;
                                                    var retValue = XRMUtility.GetAttributeValue(filterAttribute);
                                                    switch (filterAttributeType.ToLowerInvariant())
                                                    {
                                                        case "lookup":
                                                               fetchXML=fetchXML
                                                                        .Replace("{" + filterAttribute + ":id}", retValue[0].id)
                                                                        .Replace("{" + filterAttribute + ":type}", retValue[0].typename)
                                                                        .Replace("{" + filterAttribute + ":name}", retValue[0].name);
                                                            
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                   
                                                }
                                            }

                                        }
                                        crmHelper.ProcessFetchXMLRequest(fetchXML.ToString(), panelConfig.ColumnHeader, panelConfig.RecordGuid, processViewDataRequestCallbackHandler);

                                        break;
                                    default:
                                        break;
                                }
                            }

                        }
                    });
                }
            }
        }

        private bool IsValidForCurrentForm(bool visibleForCreate,bool visibleForUpdate)
        {
            switch (FormType)
            {
                case 1:
                    return visibleForCreate;                    
                case 2:
                    return visibleForUpdate;
            }

            return false;
        }
        
        [ScriptableMember]
        public void ProcessNewView(string panelConfigName,string fetchXmlAttributeValue=null)
        {

            PanelConfiguration panelConfig = (PanelConfiguration)PanelConfigurations.Where(pc => pc.ConfigurationID == panelConfigName);

            CrmHelper crmHelper = new CrmHelper();
            CrmHelper.ProcessViewDataRequestCallbackHandler processViewDataRequestCallbackHandler = new CrmHelper.ProcessViewDataRequestCallbackHandler(ProcessViewDataCallBack);
            if(panelConfig.VisibleForUpdate)
            {
                switch (panelConfig.QueryTypeName.ToLowerInvariant())
                {
                    case "systemview":                        
                        crmHelper.ProcessCrmViewRequest(panelConfig.ViewGUID,panelConfig.ViewPrimaryEntity, panelConfig.ColumnHeader, true, panelConfig.RecordGuid, processViewDataRequestCallbackHandler);
                        break;
                    case "savedview":
                        crmHelper.ProcessCrmViewRequest(panelConfig.ViewGUID,panelConfig.ViewPrimaryEntity, panelConfig.ColumnHeader, false, panelConfig.RecordGuid, processViewDataRequestCallbackHandler);
                        break;
                    case "fetchxml":
                        if (panelConfig.FetchXML.Contains("#########"))
                        {
                            string modFetchXml = panelConfig.FetchXML.Replace("#########", fetchXmlAttributeValue);
                            crmHelper.ProcessFetchXMLRequest(modFetchXml, panelConfig.ColumnHeader, panelConfig.RecordGuid, processViewDataRequestCallbackHandler);
                        }
                        else
                        {
                            crmHelper.ProcessFetchXMLRequest(panelConfig.FetchXML, panelConfig.ColumnHeader, panelConfig.RecordGuid, processViewDataRequestCallbackHandler);
                        }
                        break;
                    default:
                        break;
                }
            }

        }
        private void ProcessViewDataCallBack(ObservableCollection<View> ViewData, string panelConfigRecordGuid, bool requestStatus)
        { 
            if (!string.IsNullOrEmpty(panelConfigRecordGuid))
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    PanelConfiguration panelConfig= PanelConfigurations.Where(pc => pc.RecordGuid == panelConfigRecordGuid).SingleOrDefault();
                    FormAssistantPanelControl fapControl = new FormAssistantPanelControl();
                    fapControl.ColumnHeader = panelConfig.ColumnHeader;
                    fapControl.Title = panelConfig.Title;
                    fapControl.ReturnAttribute = panelConfig.ReturnAttribute;
                    fapControl.TargetAttribute = panelConfig.TargetAttribute;
                    fapControl.DescriptionAttribute = panelConfig.DescriptionAttribute;
                    fapControl.ShowDescriptionPanel = panelConfig.ShowDescriptionPanel;
                    fapControl.View = ViewData;
                    fapControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    fapControl.VerticalAlignment = VerticalAlignment.Stretch;
                    fapControl.Height = 200;
                    fapControl.Width = ControlWidth;
                    if(!string.IsNullOrEmpty(panelConfig.PageSize))
                    {
                        fapControl.PageSize = Convert.ToInt32(panelConfig.PageSize);
                    }

                    if (ControlOrientation == Orientation.Horizontal)
                    {
                        fapControl.Margin = new Thickness(5, 0, 0, 0);
                    }
                    else
                    {
                        fapControl.Margin = new Thickness(0, 5, 0, 0);
                    }                    
                    formAssistPanel.Children.Add(fapControl);

                });
            }
        }


        



    }
}
