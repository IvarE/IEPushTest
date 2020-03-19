namespace Crm.FormAssistantPanel.Utilities
{
    enum DataSource
    {
        CRM = 285050000
    }

    enum QueryType
    {
        SystemView = 285050000,
        SavedView = 285050001,
        FetchXML = 285050002
    }
    
    public class PanelConfiguration
    {
        string _recordGuid = string.Empty;

        public string RecordGuid
        {
            get { return _recordGuid; }
            set { _recordGuid = value; }
        }

        string _pageSize = string.Empty;
        public string PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        string _configurationid=string.Empty;
        public string ConfigurationID
        {
            get { return _configurationid; }
            set { _configurationid = value; }
        }

        string _columnHeader = string.Empty;
        public string ColumnHeader
        {
            get { return _columnHeader; }
            set { _columnHeader = value; }
        }

        string _entityScope;
        public string EntityScope
        {
            get { return _entityScope; }
            set { _entityScope = value; }
        }

        string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        int _dataSource;
        public int DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }
        string _dataSourceName;
        public string DataSourceName
        {
            get { return _dataSourceName; }
            set { _dataSourceName = value; }
        }
        int _queryType;
        public int QueryType
        {
            get { return _queryType; }
            set { _queryType = value; }
        }
        string _queryTypeName;
        public string QueryTypeName
        {
            get { return _queryTypeName; }
            set { _queryTypeName = value; }
        }
        string _controlName;
        public string ControlName
        {
            get { return _controlName; }
            set { _controlName = value; }
        }

        bool _visibleForCreate;
        public bool VisibleForCreate
        {
            get { return _visibleForCreate; }
            set { _visibleForCreate = value; }
        }

        bool _visibleForUpdate;
        public bool VisibleForUpdate
        {
            get { return _visibleForUpdate; }
            set { _visibleForUpdate = value; }
        }

        string _viewName;
        public string ViewName
        {
            get { return _viewName; }
            set { _viewName = value; }
        }

        string _viewGUID;
        public string ViewGUID
        {
            get { return _viewGUID; }
            set { _viewGUID = value; }
        }
        string _viewPrimaryEntity;
        public string ViewPrimaryEntity
        {
            get { return _viewPrimaryEntity; }
            set { _viewPrimaryEntity = value; }
        }

        string _filterAttribute;
        public string FilterAttribute
        {
            get { return _filterAttribute; }
            set { _filterAttribute = value; }
        }
        
        string _returnAttribute;
        public string ReturnAttribute
        {
            get { return _returnAttribute; }
            set { _returnAttribute = value; }
        }

        string _targetAttribute;
        public string TargetAttribute
        {
            get { return _targetAttribute; }
            set { _targetAttribute = value; }
        }

        string _fetchXML;
        public string FetchXML
        {
            get { return _fetchXML; }
            set { _fetchXML = value; }
        }

        bool _showDescriptionPanel;
        public bool ShowDescriptionPanel
        {
            get { return _showDescriptionPanel; }
            set { _showDescriptionPanel = value; }
        }

        string _descriptionAttribute = string.Empty;
        public string DescriptionAttribute
        {
            get { return _descriptionAttribute; }
            set { _descriptionAttribute = value; }
        }

    }
}
