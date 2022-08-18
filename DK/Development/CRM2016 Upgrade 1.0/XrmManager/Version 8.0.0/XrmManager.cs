using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.CodeDom;
using System.Runtime.Serialization;
using System.Reflection;
using System.Data.SqlClient;
using System.Web.Services.Protocols;
using System.ComponentModel;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using System.Net;
using Microsoft.Xrm.Client.Services;
using System.Threading;
using CGI.CRM2013.Skanetrafiken.CGIXrmLogger;

namespace CGIXrmWin
{

    // ********************************************************************

    public partial class XrmManager
    {

        readonly LogToCrm _log2Crm = new LogToCrm();
        #region constructors

        // -------------------------------------------------------------------------

        public XrmManager()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public XrmManager(IOrganizationService service)
        {
            pService = InitService(service);
        }

        public XrmManager(string serverAddress = "", string domain = "", string username = "", string password = "")
        {
            
            pService = InitService(serverAddress, domain, username, password);
        }

        //public XrmManager(Uri uri, Uri homeRealmUri, System.ServiceModel.Description.ClientCredentials clientCredentials, System.ServiceModel.Description.ClientCredentials deviceCredential)
        //{
        //    pService = InitService(uri, homeRealmUri, clientCredentials, deviceCredential);
        //}

        #endregion constructors

        #region init crm service 

        // -------------------------------------------------------------------------

        public IOrganizationService InitService(IOrganizationService service)
        {
            pService = null;
            pService = new OrganizationService(service);
            return pService;
        }

        // -------------------------------------------------------------------------

        public IOrganizationService InitService(string serverAddress = "", string domain = "", string username = "", string password = "")
        {
            //pService = null;
            if (string.IsNullOrEmpty(serverAddress))
            {
                // try to get the address from web-config or appsettings (web based not exe)
                serverAddress = System.Configuration.ConfigurationManager.AppSettings["CrmServiceAddress"];
                if (string.IsNullOrEmpty(serverAddress))
                    throw new Exception("Service Address not provided. web appsetting [CrmServiceAddress] not found");
            }

            if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                try
                {
                    // try to get the domain from web-config or appsettings (web based not exe)
                    domain = System.Configuration.ConfigurationManager.AppSettings["CrmDomain"];

                    // try to get the domain from web-config or appsettings (web based not exe)
                    username = System.Configuration.ConfigurationManager.AppSettings["CrmUsername"];

                    // try to get the domain from web-config or appsettings (web based not exe)
                    password = System.Configuration.ConfigurationManager.AppSettings["CrmPassword"];
                }
                catch { }
            }

            // serverAddress has a value, get the CrmServiceAddress
            Uri _serviceAddress = _getServiceAddress(serverAddress);

            string _crmConnectionString = string.Empty;
            if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                _crmConnectionString = string.Format("Url={0};", _serviceAddress.ToString());
            }
            else if (!string.IsNullOrEmpty(domain) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                _crmConnectionString = string.Format("Url={0};Domain={1};Username={2};Password={3};", _serviceAddress.ToString(), domain, username, password);
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            CrmServiceClient conn = null;
            if (!string.IsNullOrWhiteSpace(_crmConnectionString))
            {
                conn = ConnectionCacheManager.GetConnectionFromConfig("sekundCrm", _crmConnectionString);
            }
            else 
            {
                conn = ConnectionCacheManager.GetConnectionFromConfig("sekundCrm", null);
            }

            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService pService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;
            return pService;
        }

        // -------------------------------------------------------------------------

        //public OrganizationService InitService(Uri uri, Uri homeRealmUri, System.ServiceModel.Description.ClientCredentials clientCredentials, System.ServiceModel.Description.ClientCredentials deviceCredential)
        //{
        //    pService = null;
        //    IOrganizationService service = new Microsoft.Xrm.Sdk.Client.OrganizationServiceProxy(uri, homeRealmUri, clientCredentials, deviceCredential);
        //    pService = new OrganizationService(service);
        //    return pService;
        //}


        // -------------------------------------------------------------------------

        private Uri _getServiceAddress(string serverAddress)
        {
            if (serverAddress.EndsWith("/"))
                serverAddress += "XRMServices/2011/Organization.svc";
            else
                serverAddress += "/XRMServices/2011/Organization.svc";

            return new Uri(serverAddress, UriKind.Absolute);
        }

        // -------------------------------------------------------------------------

#endregion init crm service

        // ===================================================================================

#region private fields


#endregion private fields

        // ===================================================================================

#region Get (from CRM)

        // -----------------------------------------------------------------------------------

        public Entity Get(string entityName, Guid id, params string[] columns)
        {
            Entity _entity = null;
            try
            {
                if (columns == null || columns.Length == 0)
                    _entity = pService.Retrieve(entityName, id, new ColumnSet(true));
                else
                    _entity = pService.Retrieve(entityName, id, new ColumnSet(columns));
            }
            catch (Exception exc)
            {
                //pLog.StartSection("XrmManager_Get");
                //pLog.Write(exc, "XrmManager_Get");
                //pLog.EndSection("XrmManager_Get");
                if (pReThrowError) throw exc;
            }
            return _entity;
        }

        // -------------------------------------------------------------------------

        public Entity[] Get(QueryBase query)
        {
            XElement _baseQuery = null;
            int _pageNumber = 1;

            if (query is FetchExpression)
                _baseQuery = XElement.Parse(((FetchExpression)query).Query);

            List<Entity> _entities = new List<Entity>();

            EntityCollection _col = pService.RetrieveMultiple(query);
            _entities.AddRange(_col.Entities);

            while (_col.MoreRecords && !_col.TotalRecordCountLimitExceeded)
            {
                if (query is FetchExpression)
                {
                    ((FetchExpression)query).Query = _addNextPageCookie(_baseQuery.ToString(), _col.PagingCookie, ++_pageNumber).ToString();
                }
                else
                {
                    ((QueryExpression)query).PageInfo.PageNumber++;
                }
                _col = pService.RetrieveMultiple(query);
                _entities.AddRange(_col.Entities);
            }

            return _entities.ToArray();
        }

        // -------------------------------------------------------------------------

        private XElement _addNextPageCookie(string _baseQuery, string cookie, int pageNumber)
        {
            XElement _xFetch = XElement.Parse(_baseQuery);
            _xFetch.Add(new XAttribute("paging-cookie", ""));
            _xFetch.Add(new XAttribute("page", ""));

            XElement _xCookie = XElement.Parse(cookie);
            _xFetch.Attribute("paging-cookie").Value = _xCookie.ToString();
            _xFetch.Attribute("page").Value = pageNumber.ToString();

            return _xFetch;
        }

        // -----------------------------------------------------------------------------------

        public TEntity Get<TEntity>(Guid id, params string[] columns)
        {

            Type _t = typeof(TEntity);
            string _entityName = _t.Name;
            XrmEntityAttribute _entAtt = _t.GetCustomAttributes(typeof(XrmEntityAttribute), true).FirstOrDefault() as XrmEntityAttribute;
            if (_entAtt != null)
                _entityName = _entAtt.LogicalName;

            Entity _entity = Get(_entityName, id, columns);
            return _decode<TEntity>(_getObjectProperties<TEntity>(), _entity, default(TEntity));
        }

        // -----------------------------------------------------------------------------------

        public TEntity Get<TEntity>(TEntity objEntity)
        {
            Entity _entity = _encode<TEntity>(objEntity, null);
            _entity = Get(_entity);
            return _decode<TEntity>(_getObjectProperties<TEntity>(), _entity, objEntity);
        }

        // -----------------------------------------------------------------------------------

        public ObservableCollection<TEntity> Get<TEntity>(string fetchXml)
        {
            return Get<TEntity>(new FetchExpression(fetchXml));
        }

        // -----------------------------------------------------------------------------------

        public ObservableCollection<TEntity> Get<TEntity>(QueryBase query)
        {
            Entity[] _entities = Get(query);
            ObservableCollection<TEntity> _list = new ObservableCollection<TEntity>();
            foreach (Entity entity in _entities)
            {
                _list.Add(_decode<TEntity>(_getObjectProperties<TEntity>(), entity, default(TEntity)));
            }
            return _list;
        }

        // -----------------------------------------------------------------------------------

#endregion Get (from CRM)

        // ===================================================================================

#region WhoAmI and Current User

        public WhoAmIResponse WhoAmI()
        {
            WhoAmIResponse _whoAmIResponse = null;
            try
            {
                _whoAmIResponse = (WhoAmIResponse)pService.Execute(new WhoAmIRequest());
            }
            catch { }

            return _whoAmIResponse;
        }

        // -----------------------------------------------------------------------------------

        public SystemUser GetCurrentUser()
        {
            SystemUser _user = new SystemUser();
            _user.FullName = "<no no>";
            try
            {
                WhoAmIResponse _whoAmI = WhoAmI();
                if (_whoAmI == null)
                    throw new Exception("Who Am I is null");

                Entity _entity = Get("systemuser", _whoAmI.UserId, "fullname");
                _user.SystemUserId = _whoAmI.UserId;
                _user.FullName = _entity.GetAttributeValue<string>("fullname");
            }
            catch (Exception exc)
            {
                _user.FirstName = exc.Message;
            }

            return _user;
        }

#endregion WhoAmI and Current User

        // ===================================================================================

#region crm create

        // -----------------------------------------------------------------------------------

        public Guid Create(Entity entity)
        {
            return pService.Create(entity);
        }

        // -----------------------------------------------------------------------------------

        public Entity Create<TEntity>(TEntity objEntity)
        {
            Entity _entity = null;
            if (objEntity is ICrmEntity)
            {
                _entity = _encode<TEntity>(objEntity, ((ICrmEntity)objEntity).Entity);
                ((ICrmEntity)objEntity).Entity = _entity;
            }
            else
            {
                _entity = _encode<TEntity>(objEntity, null);
            }
            _entity.Id = Guid.Empty;

            _entity.Id = pService.Create(_entity);

            Dictionary<string, XrmProperty> _props = _getObjectProperties<TEntity>();
            XrmProperty _xprop = _props.FirstOrDefault(y => y.Value.IsPrimaryKey == true).Value;
            if (_xprop != null)
            {
                _xprop.PropInfo.SetValue(objEntity, _entity.Id, null);
            }
            return _entity;
        }

        // -----------------------------------------------------------------------------------

#endregion crm create

        // ===================================================================================

#region crm update

        // -----------------------------------------------------------------------------------

        public void Update(Entity entity)
        {
            pService.Update(entity);
        }

        // -----------------------------------------------------------------------------------

        public Entity Update<TEntity>(TEntity objEntity)
        {
            Entity _entity = null;
            if (objEntity is ICrmEntity)
            {
                _entity = _encode<TEntity>(objEntity, ((ICrmEntity)objEntity).Entity);
                ((ICrmEntity)objEntity).Entity = _entity;
            }
            else
            {
                _entity = _encode<TEntity>(objEntity, null);
            }
            pService.Update(_entity);
            return _entity;
        }

        // -----------------------------------------------------------------------------------

#endregion crm update

        // ===================================================================================

#region crm save

        // -----------------------------------------------------------------------------------

        public void Save(Entity entity)
        {
            if (entity.Id == Guid.Empty)
                pService.Create(entity);
            else
                pService.Update(entity);
        }

        // -----------------------------------------------------------------------------------

        public Entity Save<TEntity>(TEntity objEntity)
        {
            Entity _entity = null;
            if (objEntity.GetType() is ICrmEntity)
            {
                _entity = _encode<TEntity>(objEntity, ((ICrmEntity)objEntity).Entity);
                ((ICrmEntity)objEntity).Entity = _entity;
            }
            else
            {
                _entity = _encode<TEntity>(objEntity, null);
            }

            if (_entity.Id == Guid.Empty)
            {
                _entity.Id = pService.Create(_entity);
                Dictionary<string, XrmProperty> _props = _getObjectProperties<TEntity>();
                XrmProperty _xprop = _props.FirstOrDefault(y => y.Value.IsPrimaryKey == true).Value;
                if (_xprop != null)
                {
                    _xprop.PropInfo.SetValue(objEntity, _entity.Id, null);
                }
            }
            else
            {
                pService.Update(_entity);
            }

            return _entity;
        }

        // -----------------------------------------------------------------------------------

#endregion crm save

        // ===================================================================================

#region crm delete

        // -----------------------------------------------------------------------------------

        public void Delete(string entityName, Guid id)
        {
            pService.Delete(entityName, id);
        }

        // -----------------------------------------------------------------------------------

        public void Delete(Entity entity)
        {
            pService.Delete(entity.LogicalName, entity.Id);
        }

        public void Delete<TEntity>(TEntity objEntity)
        {
            if (typeof(TEntity) is ICrmEntity)
            {
                pService.Delete(((ICrmEntity)objEntity).Entity.LogicalName, ((ICrmEntity)objEntity).Entity.Id);
            }
        }

        // -----------------------------------------------------------------------------------

#endregion crm delete

        // ===================================================================================

#region decode to TEntity

        // -----------------------------------------------------------------------------------

        private TEntity _decode<TEntity>(Dictionary<string, XrmProperty> props, Entity entity, TEntity useObject)
        {
            TEntity _object = default(TEntity);

            if (useObject == null)
                _object = Activator.CreateInstance<TEntity>();
            else
                _object = useObject;


            Object _value = null;

            if (_object is ICrmEntity)
                ((ICrmEntity)_object).Entity = entity;

            foreach (XrmProperty xrmProp in props.Values)
            {
                _value = null;

                if (!entity.Attributes.ContainsKey(xrmProp.Path))
                    continue;

                if (!entity.Attributes.TryGetValue(xrmProp.Path, out _value))
                    continue;

                if (_value is AliasedValue)
                {
                    _value = ((AliasedValue)_value).Value;
                }

                if (_value is DateTime)
                {
                    _value = ((DateTime)_value).ToLocalTime();
                }

                // change decode part to formatted if type is optionset value
                if (_value is OptionSetValue && xrmProp.DecodePart == XrmDecode.Name)
                    xrmProp.DecodePart = XrmDecode.Formatted;

                if (xrmProp.DecodePart == XrmDecode.Value)
                {
                    if (_value is EntityReference)
                    {
                        _value = ((EntityReference)_value).Id;
                    }
                    else if (_value is OptionSetValue)
                    {
                        _value = ((OptionSetValue)_value).Value;
                    }
                }
                else if (xrmProp.DecodePart == XrmDecode.Name)
                {
                    if (_value is EntityReference)
                    {
                        _value = ((EntityReference)_value).Name;
                    }
                }
                else if (xrmProp.DecodePart == XrmDecode.Formatted)
                {
                    if (!entity.FormattedValues.ContainsKey(xrmProp.Path)) continue;

                    _value = entity.FormattedValues.FirstOrDefault(x => x.Key == xrmProp.Path);
                    if (_value != null)
                        _value = ((KeyValuePair<string, string>)_value).Value;
                }
                else if (xrmProp.DecodePart == XrmDecode.Type)
                {
                    if (_value is EntityReference)
                        _value = ((EntityReference)_value).LogicalName;
                }

                if (xrmProp.IsPrimaryKey)
                    xrmProp.PropInfo.SetValue(_object, entity.Id, null);
                else
                    xrmProp.PropInfo.SetValue(_object, _value, null);

            }

            return _object;
        }

        // -----------------------------------------------------------------------------------

        private Dictionary<string, XrmProperty> _getObjectProperties<TEntity>()
        {
            XrmAttribute _xrmAtt; // C# attribute class type
            XrmPrimaryKeyAttribute _primaryKey;
            Dictionary<string, XrmProperty> _dicProps = new Dictionary<string, XrmProperty>();

            TEntity _entityObject = Activator.CreateInstance<TEntity>();
            Type _entityObjectType = _entityObject.GetType();

            List<PropertyInfo> _props = _entityObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            foreach (PropertyInfo prop in _props)
            {
                _xrmAtt = prop.GetCustomAttributes(typeof(XrmAttribute), true).FirstOrDefault() as XrmAttribute;
                if (_xrmAtt == null) continue;
                _primaryKey = prop.GetCustomAttributes(typeof(XrmPrimaryKeyAttribute), true).FirstOrDefault() as XrmPrimaryKeyAttribute;
                XrmProperty _p = new XrmProperty();
                _p.IsPrimaryKey = _primaryKey != null;
                _p.PropInfo = prop;
                _p.DecodePart = _xrmAtt.DecodePart; // == XrmDecode.Default ? XrmDecode.Value : _xrmAtt.DecodePart;
                _p.Path = string.IsNullOrEmpty(_xrmAtt.Path) ? prop.Name : _xrmAtt.Path;
                _dicProps.Add(prop.Name, _p);
            }
            return _dicProps;
        }

        // -----------------------------------------------------------------------------------

        private void _findPrimaryProperty<TEntity>(TEntity obj)
        {

        }

        // -----------------------------------------------------------------------------------

#endregion decode to TEntity

        // ===================================================================================

#region Encode to Entity

        // -----------------------------------------------------------------------------------

        private Entity _encode<TEntity>(TEntity objEntity, Entity entity)
        {
            XrmReadOnlyAttribute _readOnlyAttrib;
            XrmAttribute _xrmAttrib;
            XrmPrimaryKeyAttribute _primaryKey;

            if (entity == null)
                entity = new Entity();

            Type _typeOfObject = objEntity.GetType();
            if (string.IsNullOrEmpty(entity.LogicalName))
            {
                entity.LogicalName = _typeOfObject.Name;
                XrmEntityAttribute _entAtt = _typeOfObject.GetCustomAttributes(typeof(XrmEntityAttribute), true).FirstOrDefault() as XrmEntityAttribute;
                if (_entAtt != null)
                    entity.LogicalName = _entAtt.LogicalName;
            }

            List<PropertyInfo> _props = objEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            foreach (PropertyInfo prop in _props)
            {
                _readOnlyAttrib = prop.GetCustomAttributes(typeof(XrmReadOnlyAttribute), true).FirstOrDefault() as XrmReadOnlyAttribute;
                if (_readOnlyAttrib != null)
                    continue;

                _xrmAttrib = prop.GetCustomAttributes(typeof(XrmAttribute), true).FirstOrDefault() as XrmAttribute;
                if (_xrmAttrib == null)
                    continue;

                if (_xrmAttrib != null && _xrmAttrib.DecodePart != XrmDecode.Default)
                    continue;

                if (string.IsNullOrEmpty(_xrmAttrib.Path) || _xrmAttrib.Path.Contains("."))
                    continue;

                _primaryKey = prop.GetCustomAttributes(typeof(XrmPrimaryKeyAttribute), true).FirstOrDefault() as XrmPrimaryKeyAttribute;
                if (_primaryKey != null)
                {
                    Guid _id;
                    if (Guid.TryParse(prop.GetValue(objEntity, null).ToString(), out _id))
                        entity.Id = _id;
                    continue;
                }

                if (!entity.Attributes.ContainsKey(_xrmAttrib.Path))
                {
                    entity.Attributes.Add(_xrmAttrib.Path, prop.GetValue(objEntity, null));
                }
                else
                {
                    entity.Attributes[_xrmAttrib.Path] = prop.GetValue(objEntity, null);
                }
            }
            return entity;
        }

        // -----------------------------------------------------------------------------------

#endregion Encode        

        // ===================================================================================

#region private class XrmProperty

        // -----------------------------------------------------------------------------------

        private class XrmProperty
        {
            public PropertyInfo PropInfo { get; set; }
            public string Path { get; set; }
            public bool IsAlias { get; set; }
            public XrmDecode DecodePart { get; set; }
            public bool IsPrimaryKey { get; set; }
        }

        // -----------------------------------------------------------------------------------

#endregion private class XrmProperty

        // ===================================================================================

#region properties

        // -------------------------------------------------------------------------

        private IOrganizationService pService;
        public IOrganizationService Service
        {
            get
            {
                return pService;
            }
            set
            {
                if (pService != value)
                {
                    pService = value;
                }
            }
        }

        private bool pReThrowError;
        public bool ReThrowError
        {
            get { return pReThrowError; }
            set
            {
                if (pReThrowError != value)
                {
                    pReThrowError = value;
                }
            }
        }

        // -------------------------------------------------------------------------

#endregion properties

        // ===================================================================================
    }

    // ********************************************************************


    // ***********************************************************************************

#region class OptionSet and options

    // ===================================================================================

    public class OptionSet : XrmBaseNotify
    {
        private ObservableCollection<OptionSetOption> pOptions = new ObservableCollection<OptionSetOption>();
        public ObservableCollection<OptionSetOption> Options
        {
            get { return pOptions; }
            set
            {
                if (pOptions != value)
                {
                    pOptions = value;
                    OnPropertyChanged("Options");
                }
            }
        }

        private string pEntityLogicalName;
        public string EntityLogicalName
        {
            get { return pEntityLogicalName; }
            set
            {
                if (pEntityLogicalName != value)
                {
                    pEntityLogicalName = value;
                    OnPropertyChanged("EntityLogicalName");
                }
            }
        }

        private string pOptionSetName;
        public string OptionSetName
        {
            get { return pOptionSetName; }
            set
            {
                if (pOptionSetName != value)
                {
                    pOptionSetName = value;
                    OnPropertyChanged("OptionSetName");
                }
            }
        }

        private bool pIsGlobal;
        public bool IsGlobal
        {
            get { return pIsGlobal; }
            set
            {
                if (pIsGlobal != value)
                {
                    pIsGlobal = value;
                    OnPropertyChanged("IsGlobal");
                }
            }
        }
    }

    // ===================================================================================

    public class OptionSetOption : XrmBaseNotify
    {
        private OptionSet pParentOptionSet;
        public OptionSet ParentOptionSet
        {
            get { return pParentOptionSet; }
            set
            {
                if (pParentOptionSet != value)
                {
                    pParentOptionSet = value;
                    OnPropertyChanged("ParentOptionSet");
                }
            }
        }

        private string pName;
        public string Name
        {
            get { return pName; }
            set
            {
                if (pName != value)
                {
                    pName = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private int pLanguageCode;
        public int LanguageCode
        {
            get { return pLanguageCode; }
            set
            {
                if (pLanguageCode != value)
                {
                    pLanguageCode = value;
                    OnPropertyChanged("LanguageCode");
                }
            }
        }

        private int pValue;
        public int Value
        {
            get { return pValue; }
            set
            {
                if (pValue != value)
                {
                    pValue = value;
                    OnPropertyChanged("Value");
                }
            }
        }
    }

    // ===================================================================================

#endregion class OptionSet and options

    // ***********************************************************************************

#region class EntityMetadata

    // ===================================================================================

    public class EntityDef
    {
        public int Language { get; set; }
        public string DisplayName { get; set; }
        public int ObjectTypeCode { get; set; }
        public string LogicalName { get; set; }
        public string SchemaName { get; set; }
    }

    // ===================================================================================

#endregion class EntityMetadata

    // ***********************************************************************************

#region C# Attributes

    // ===================================================================================

    [AttributeUsage(AttributeTargets.Class)]
    public class XrmEntityAttribute : Attribute
    {
        public XrmEntityAttribute() { }

        public XrmEntityAttribute(string logicalName)
        {
            this.LogicalName = logicalName;
        }

        public string LogicalName { get; set; }
    }

    // ===================================================================================

    [AttributeUsage(AttributeTargets.Property)]
    public class XrmAttribute : Attribute
    {
        public XrmAttribute() { }
        public XrmAttribute(string path) { Path = path; }
        public string Path { get; set; }
        public XrmDecode DecodePart { get; set; }
    }

    // ===================================================================================

    [AttributeUsage(AttributeTargets.Property)]
    public class XrmReadOnlyAttribute : Attribute
    { }

    // ===================================================================================

    [AttributeUsage(AttributeTargets.Property)]
    public class XrmPrimaryKeyAttribute : Attribute
    { }

    // ===================================================================================

    public enum XrmDecode
    {
        Default,
        Value,
        Formatted,
        Type,
        Name
    }

    // ===================================================================================

#endregion C# Attributes

    // ***********************************************************************************

#region CRM base classes

    // ===================================================================================

    [Serializable]
    [DataContract]
    public abstract class XrmBaseNotify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        virtual protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler prop = PropertyChanged;
            if (prop != null)
                prop(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // ===================================================================================

    public interface ICrmEntity
    {
        Entity Entity { get; set; }
    }

    // ===================================================================================

    [Serializable]
    [DataContract]
    public abstract class XrmBaseEntity : XrmBaseNotify, ICrmEntity
    {
        public Entity Entity { get; set; }

#region crm attributes

#region created by/on
        protected EntityReference pCreatedBy;
        [DataMember]
        [XrmReadOnly]
        [Xrm("createdby")]
        public EntityReference CreatedBy
        {
            get { return pCreatedBy; }
            set
            {
                if (pCreatedBy != value)
                {
                    pCreatedBy = value;
                    OnPropertyChanged("CreatedBy");
                }
            }
        }

        protected DateTime pCreatedOn;
        [DataMember]
        [XrmReadOnly]
        [Xrm("createdon")]
        public DateTime CreatedOn
        {
            get { return pCreatedOn; }
            set
            {
                if (pCreatedOn != value)
                {
                    pCreatedOn = value;
                    OnPropertyChanged("CreatedOn");
                }
            }
        }
#endregion created by/on

#region modified by/on
        protected EntityReference pModifiedBy;
        [DataMember]
        [XrmReadOnly]
        [Xrm("modifiedby")]
        public EntityReference ModifiedBy
        {
            get { return pModifiedBy; }
            set
            {
                if (pModifiedBy != value)
                {
                    pModifiedBy = value;
                    OnPropertyChanged("ModifiedBy");
                }
            }
        }

        protected DateTime pModifiedOn;
        [DataMember]
        [XrmReadOnly]
        [Xrm("modifiedon")]
        public DateTime ModifiedOn
        {
            get { return pModifiedOn; }
            set
            {
                if (pModifiedOn != value)
                {
                    pModifiedOn = value;
                    OnPropertyChanged("ModifiedOn");
                }
            }
        }
#endregion modified by/on

#region state code
        protected OptionSetValue pStateCode;
        [DataMember]
        [XrmReadOnly]
        [Xrm("statecode")]
        public OptionSetValue StateCode
        {
            get { return pStateCode; }
            set
            {
                if (pStateCode != value)
                {
                    pStateCode = value;
                    OnPropertyChanged("StateCode");
                }
            }
        }

        protected string pStateCodeName;
        [DataMember]
        [XrmReadOnly]
        [Xrm("statecode", DecodePart = XrmDecode.Formatted)]
        public string StateCodeName
        {
            get { return pStateCodeName; }
            set
            {
                if (pStateCodeName != value)
                {
                    pStateCodeName = value;
                    OnPropertyChanged("StateCodeName");
                }
            }
        }

#endregion state code

#region status code
        protected OptionSetValue pStatusCode;
        [DataMember]
        [Xrm("statuscode")]
        public OptionSetValue StatusCode
        {
            get { return pStatusCode; }
            set
            {
                if (pStatusCode != value)
                {
                    pStatusCode = value;
                    OnPropertyChanged("StatusCode");
                }
            }
        }

        protected string pStatusCodeName;
        [DataMember]
        [XrmReadOnly]
        [Xrm("statuscode", DecodePart = XrmDecode.Formatted)]
        public string StatusCodeName
        {
            get { return pStatusCodeName; }
            set
            {
                if (pStatusCodeName != value)
                {
                    pStatusCodeName = value;
                    OnPropertyChanged("StatusCodeName");
                }
            }
        }


#endregion status code

#region owner
        protected EntityReference pOwner;
        [DataMember]
        [XrmReadOnly]
        [Xrm("ownerid")]
        public EntityReference Owner
        {
            get { return pOwner; }
            set
            {
                if (pOwner != value)
                {
                    pOwner = value;
                    OnPropertyChanged("Owner");
                }
            }
        }

        protected string pOwnerType;
        [DataMember]
        [XrmReadOnly]
        [Xrm("ownerid", DecodePart = XrmDecode.Type)]
        public string OwnerType
        {
            get { return pOwnerType; }
            set
            {
                if (pOwnerType != value)
                {
                    pOwnerType = value;
                    OnPropertyChanged("OwnerType");
                }
            }
        }

        protected EntityReference pOwningUser;
        [DataMember]
        [XrmReadOnly]
        [Xrm("owninguser")]
        public EntityReference OwningUser
        {
            get { return pOwningUser; }
            set
            {
                if (pOwningUser != value)
                {
                    pOwningUser = value;
                    OnPropertyChanged("OwningUser");
                }
            }
        }

        protected string pOwningUserType;
        [DataMember]
        [XrmReadOnly]
        [Xrm("owninguser", DecodePart = XrmDecode.Type)]
        public string OwningUserType
        {
            get { return pOwningUserType; }
            set
            {
                if (pOwningUserType != value)
                {
                    pOwningUserType = value;
                    OnPropertyChanged("OwningUserType");
                }
            }
        }

        protected EntityReference pOwningBusinessUnit;
        [DataMember]
        [XrmReadOnly]
        [Xrm("owningbusinessunit")]
        public EntityReference OwningBusinessUnit
        {
            get { return pOwningBusinessUnit; }
            set
            {
                if (pOwningBusinessUnit != value)
                {
                    pOwningBusinessUnit = value;
                    OnPropertyChanged("OwningBusinessUnit");
                }
            }
        }

        protected string pOwningBusinessUnitType;
        [DataMember]
        [XrmReadOnly]
        [Xrm("owningbusinessunit", DecodePart = XrmDecode.Type)]
        public string OwningBusinessUnitType
        {
            get { return pOwningBusinessUnitType; }
            set
            {
                if (pOwningBusinessUnitType != value)
                {
                    pOwningBusinessUnitType = value;
                    OnPropertyChanged("OwningBusinessUnitType");
                }
            }
        }

#endregion owner

#endregion crm attributes
    }

    // ===================================================================================

#endregion CRM base classes

    // ***********************************************************************************

}
