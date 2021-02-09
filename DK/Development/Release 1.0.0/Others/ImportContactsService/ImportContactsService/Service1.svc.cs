using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using CGIXrmWin;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Crm;
using Microsoft.Crm.Sdk;
using Microsoft.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Query;

namespace ImportContactsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        XrmManager _xrmManager;
        private object LockSql = new object();

        public bool CreateContacts()
        {
            bool _returnValue = false;

            try
            {
                _xrmManager = _initCrmManager();
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
                SqlConnection _con = new SqlConnection(connectionString);
                _con.Open();

                SqlCommand _com = new SqlCommand("sp_CreateContactImportTable", _con);
                _com.CommandType = CommandType.StoredProcedure;
                _com.ExecuteNonQuery();
                
                string _sql = "";
                _sql += "select";
                _sql += " distinct c.customer_id, c.customer_type, c.one_time_customer, c.company, c.address, c.address2, c.ZIP, c.city, c.province_state, c.ccode,";
                _sql += " c.language, c.FAX, c.orgnr, c.VATnumber, c.customer_no, c.comments, c.pricelist_id, c.warehouse_id, c.customer_category_id, c.preferred_shippinglocation_id,";
                _sql += " c.discount, c.currency, c.credit_limit, c.credit_open, c.customer_keys, c.state, c.validated_name, c.validated_firstname, c.validated_lastname, c.validated_address,";
                _sql += " c.validated_ZIP, c.validated_city, c.validation_date, c.validation_service, c.validation_state, c.validation_string, c.default_payment_id, c.default_shipping_id,";
                _sql += " c.ctext1, c.ctext2, c.ctext3, c.ctext4, c.ctext5, c.ctext6, c.cselect1, c.cselect2, c.cselect3, c.cselect4, c.cselect5,";
                _sql += " c.cselect6, c.cselect7, c.cselect8, c.cselect9, c.cselect10,";
                _sql += " c.affiliate_id, c.affiliate_date, c.affiliate_member, c.surname, c.lastname, c.phone, c.cellphone, c.email, c.login, c.password, c.want_newsletter, c.have_sent_welcomemail,";
                _sql += " c.creditworthy, c.admin_id, c.order_count, c.order_totals, c.last_order, c.changed, c.created, c.synced, c.upd_state, c.contactidx";
                _sql += " from contacts c";
                _sql += " order by c.customer_id";

                SqlCommand _command = new SqlCommand(_sql, _con);
                SqlDataReader _reader = _command.ExecuteReader();

                List<OrganizationRequest> _requestList = new List<OrganizationRequest>();
                List<OrganizationRequestCollection> _orgColls = new List<OrganizationRequestCollection>();
                OrganizationRequestCollection _orgColl = new OrganizationRequestCollection();
                int _orgCollCount = 1;
                string _requestName;

                try
                {
                    while (_reader.Read())
                    {
                        // Create contact
                        string _id = _reader["customer_id"].ToString();
                        string _firstname = _reader["surname"].ToString().Replace(@"\N", "");
                        string _lastname = _reader["lastname"].ToString().Replace(@"\N", "");
                        string _address = _reader["address"].ToString().Replace(@"\N", "");
                        string _zip = _reader["ZIP"].ToString().Replace(@"\N", "");
                        string _city = _reader["city"].ToString().Replace(@"\N", "");
                        string _comments = _reader["comments"].ToString().Replace(@"\N", "");
                        string _phone = _reader["phone"].ToString().Replace(@"\N", "");
                        string _cellphone = _reader["cellphone"].ToString().Replace(@"\N", "");
                        string _email = _reader["email"].ToString().Replace(@"\N", "");
                        string _FAX = _reader["FAX"].ToString().Replace(@"\N", "");

                        string _contactCRMID = _reader["contactidx"].ToString();

                        string _fullname = string.Format("{0} {1}", _firstname, _lastname);

                        Console.WriteLine(String.Format("{0}, {1} {2}", _id, _firstname, _lastname));

                        Entity _ent = new Entity("contact");
                        _ent.LogicalName = "contact";
                        _ent.Attributes.Add("cgi_importid", _id);
                        _ent.Attributes.Add("fullname", _fullname);
                        _ent.Attributes.Add("firstname", _firstname);
                        _ent.Attributes.Add("lastname", _lastname);
                        _ent.Attributes.Add("address1_line1", _address);
                        _ent.Attributes.Add("address1_postalcode", _zip);
                        _ent.Attributes.Add("address1_city", _city);
                        _ent.Attributes.Add("description", _comments);
                        _ent.Attributes.Add("telephone2", _phone);
                        _ent.Attributes.Add("telephone1", _cellphone);
                        _ent.Attributes.Add("telephone3", _FAX);
                        _ent.Attributes.Add("emailaddress1", _email);

                        _requestName = "Create";
                        if (_contactCRMID != Guid.Empty.ToString())
                        {
                            _ent.Id = new Guid(_contactCRMID);
                            _requestName = "Update";
                        }

                        if (_orgCollCount >= 1000)
                        {
                            OrganizationRequest _request = new OrganizationRequest();
                            _request.RequestName = "ExecuteMultiple";
                            _request.Parameters = new ParameterCollection();
                            ExecuteMultipleSettings _b = new ExecuteMultipleSettings { ContinueOnError = false, ReturnResponses = true };
                            KeyValuePair<string, object> _a = new KeyValuePair<string, object>("Settings", _b);
                            _request.Parameters.Add(_a);
                            KeyValuePair<string, object> _aa = new KeyValuePair<string, object>("Requests", _orgColl);
                            _request.Parameters.Add(_aa);
                            _requestList.Add(_request);

                            _orgCollCount = 1;
                            _orgColl = new OrganizationRequestCollection();
                        }

                        OrganizationRequest _createrequest = new OrganizationRequest() { RequestName = _requestName };
                        _createrequest["Target"] = _ent;
                        _orgColl.Add(_createrequest);
                        _orgCollCount++;
                    }

                    if (_orgColl != null && _orgColl.Count() > 0)
                    {
                        OrganizationRequest _request = new OrganizationRequest();
                        _request.RequestName = "ExecuteMultiple";
                        _request.Parameters = new ParameterCollection();
                        ExecuteMultipleSettings _b = new ExecuteMultipleSettings { ContinueOnError = false, ReturnResponses = true };
                        KeyValuePair<string, object> _a = new KeyValuePair<string, object>("Settings", _b);
                        _request.Parameters.Add(_a);
                        KeyValuePair<string, object> _aa = new KeyValuePair<string, object>("Requests", _orgColl);
                        _request.Parameters.Add(_aa);
                        _requestList.Add(_request);
                    }

                    foreach (OrganizationRequest _r in _requestList)
                    {
                        _xrmManager.Service.Execute(_r);
                    }

                    _returnValue = true;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    // Always call Close when done reading.
                    _reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _returnValue;
        }

        public bool CreateTravelCards()
        {
            bool _returnValue = false;

            try
            {
                _xrmManager = _initCrmManager();
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
                SqlConnection _con = new SqlConnection(connectionString);
                _con.Open();

                SqlCommand _com = new SqlCommand("sp_CreateTravelCardImportTable", _con);
                _com.CommandType = CommandType.StoredProcedure;
                _com.ExecuteNonQuery();

                string _sql = "";
                _sql += "select";
                _sql += " customer_id, cardnumber, travelcardid, contactidx";
                _sql += " from travelcards";
                _sql += " order by customer_id";

                SqlCommand _command = new SqlCommand(_sql, _con);
                SqlDataReader _reader = _command.ExecuteReader();

                List<OrganizationRequest> _requestList = new List<OrganizationRequest>();
                List<OrganizationRequestCollection> _orgColls = new List<OrganizationRequestCollection>();
                OrganizationRequestCollection _orgColl = new OrganizationRequestCollection();
                int _orgCollCount = 1;
                string _requestName;

                try
                {
                    while (_reader.Read())
                    {
                        // Create contact
                        string _id = _reader["customer_id"].ToString();
                        string _cardnumber = _reader["cardnumber"].ToString().Replace(@"\N", "");
                        string _travelcardid = _reader["travelcardid"].ToString().Replace(@"\N", "");
                        
                        string _contactCRMID = _reader["contactidx"].ToString();

                        Entity _ent = new Entity("cgi_travelcard");
                        _ent.LogicalName = "cgi_travelcard";
                        _ent.Attributes.Add("cgi_importid", _id);
                        _ent.Attributes.Add("cgi_travelcardnumber", _cardnumber);
                        _ent.Attributes.Add("cgi_travelcardname", _cardnumber);
                        _ent.Attributes.Add("cgi_contactid", new EntityReference("contact", new Guid(_contactCRMID)));
                        
                        _requestName = "Create";
                        if (_travelcardid != Guid.Empty.ToString())
                        {
                            _ent.Id = new Guid(_travelcardid);
                            _requestName = "Update";
                        }

                        if (_orgCollCount >= 1000)
                        {
                            OrganizationRequest _request = new OrganizationRequest();
                            _request.RequestName = "ExecuteMultiple";
                            _request.Parameters = new ParameterCollection();
                            ExecuteMultipleSettings _b = new ExecuteMultipleSettings { ContinueOnError = false, ReturnResponses = true };
                            KeyValuePair<string, object> _a = new KeyValuePair<string, object>("Settings", _b);
                            _request.Parameters.Add(_a);
                            KeyValuePair<string, object> _aa = new KeyValuePair<string, object>("Requests", _orgColl);
                            _request.Parameters.Add(_aa);
                            _requestList.Add(_request);

                            _orgCollCount = 1;
                            _orgColl = new OrganizationRequestCollection();
                        }

                        OrganizationRequest _createrequest = new OrganizationRequest() { RequestName = _requestName };
                        _createrequest["Target"] = _ent;
                        _orgColl.Add(_createrequest);
                        _orgCollCount++;
                    }

                    if (_orgColl != null && _orgColl.Count() > 0)
                    {
                        OrganizationRequest _request = new OrganizationRequest();
                        _request.RequestName = "ExecuteMultiple";
                        _request.Parameters = new ParameterCollection();
                        ExecuteMultipleSettings _b = new ExecuteMultipleSettings { ContinueOnError = false, ReturnResponses = true };
                        KeyValuePair<string, object> _a = new KeyValuePair<string, object>("Settings", _b);
                        _request.Parameters.Add(_a);
                        KeyValuePair<string, object> _aa = new KeyValuePair<string, object>("Requests", _orgColl);
                        _request.Parameters.Add(_aa);
                        _requestList.Add(_request);
                    }

                    foreach (OrganizationRequest _r in _requestList)
                    {
                        _xrmManager.Service.Execute(_r);
                    }

                    _returnValue = true;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    // Always call Close when done reading.
                    _reader.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _returnValue;
        }

        private XrmManager _initCrmManager()
        {
            try
            {
                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"].ToString();
                string domain = ConfigurationManager.AppSettings["Domain"].ToString();
                string username = ConfigurationManager.AppSettings["Username"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                else
                {
                    XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);
                    return xrmMgr;
                }
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }

        private SqlConnection OpenSQL()
        {
            lock (LockSql)
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
        }

        private void CloseSQL(SqlConnection connection)
        {
            lock (LockSql)
            {
                if (connection != null)
                    connection.Close();
            }
        }
        
    }
}
