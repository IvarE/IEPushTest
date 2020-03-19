using System;
using System.Collections.Generic;
using System.Linq;
using CGIXrmWin;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using Microsoft.Xrm.Sdk;

namespace ImportContactsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        XrmManager _xrmManager;

        public bool CreateContacts()
        {
            try
            {
                _xrmManager = _initCrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand com = new SqlCommand("sp_CreateContactImportTable", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.ExecuteNonQuery();
                
                string sql = "";
                sql += "select";
                sql += " distinct c.customer_id, c.customer_type, c.one_time_customer, c.company, c.address, c.address2, c.ZIP, c.city, c.province_state, c.ccode,";
                sql += " c.language, c.FAX, c.orgnr, c.VATnumber, c.customer_no, c.comments, c.pricelist_id, c.warehouse_id, c.customer_category_id, c.preferred_shippinglocation_id,";
                sql += " c.discount, c.currency, c.credit_limit, c.credit_open, c.customer_keys, c.state, c.validated_name, c.validated_firstname, c.validated_lastname, c.validated_address,";
                sql += " c.validated_ZIP, c.validated_city, c.validation_date, c.validation_service, c.validation_state, c.validation_string, c.default_payment_id, c.default_shipping_id,";
                sql += " c.ctext1, c.ctext2, c.ctext3, c.ctext4, c.ctext5, c.ctext6, c.cselect1, c.cselect2, c.cselect3, c.cselect4, c.cselect5,";
                sql += " c.cselect6, c.cselect7, c.cselect8, c.cselect9, c.cselect10,";
                sql += " c.affiliate_id, c.affiliate_date, c.affiliate_member, c.surname, c.lastname, c.phone, c.cellphone, c.email, c.login, c.password, c.want_newsletter, c.have_sent_welcomemail,";
                sql += " c.creditworthy, c.admin_id, c.order_count, c.order_totals, c.last_order, c.changed, c.created, c.synced, c.upd_state, c.contactidx";
                sql += " from contacts c";
                sql += " order by c.customer_id";

                SqlCommand command = new SqlCommand(sql, con);
                SqlDataReader reader = command.ExecuteReader();

                List<OrganizationRequest> requestList = new List<OrganizationRequest>();
                OrganizationRequestCollection orgColl = new OrganizationRequestCollection();
                int orgCollCount = 1;

                try
                {
                    while (reader.Read())
                    {
                        // Create contact
                        string id = reader["customer_id"].ToString();
                        string firstname = reader["surname"].ToString().Replace(@"\N", "");
                        string lastname = reader["lastname"].ToString().Replace(@"\N", "");
                        string address = reader["address"].ToString().Replace(@"\N", "");
                        string zip = reader["ZIP"].ToString().Replace(@"\N", "");
                        string city = reader["city"].ToString().Replace(@"\N", "");
                        string comments = reader["comments"].ToString().Replace(@"\N", "");
                        string phone = reader["phone"].ToString().Replace(@"\N", "");
                        string cellphone = reader["cellphone"].ToString().Replace(@"\N", "");
                        string email = reader["email"].ToString().Replace(@"\N", "");
                        string fax = reader["FAX"].ToString().Replace(@"\N", "");

                        string contactCrmid = reader["contactidx"].ToString();

                        string fullname = string.Format("{0} {1}", firstname, lastname);

                        Console.WriteLine("{0}, {1} {2}", id, firstname, lastname);

                        Entity ent = new Entity("contact")
                        {
                            LogicalName = "contact"
                        };
                        ent.Attributes.Add("cgi_importid", id);
                        ent.Attributes.Add("fullname", fullname);
                        ent.Attributes.Add("firstname", firstname);
                        ent.Attributes.Add("lastname", lastname);
                        ent.Attributes.Add("address1_line1", address);
                        ent.Attributes.Add("address1_postalcode", zip);
                        ent.Attributes.Add("address1_city", city);
                        ent.Attributes.Add("description", comments);
                        ent.Attributes.Add("telephone2", phone);
                        ent.Attributes.Add("telephone1", cellphone);
                        ent.Attributes.Add("telephone3", fax);
                        ent.Attributes.Add("emailaddress1", email);

                        var requestName = "Create";
                        if (contactCrmid != Guid.Empty.ToString())
                        {
                            ent.Id = new Guid(contactCrmid);
                            requestName = "Update";
                        }

                        if (orgCollCount >= 1000)
                        {
                            OrganizationRequest request = new OrganizationRequest
                            {
                                RequestName = "ExecuteMultiple",
                                Parameters = new ParameterCollection()
                            };
                            ExecuteMultipleSettings b = new ExecuteMultipleSettings { ContinueOnError = false, ReturnResponses = true };
                            KeyValuePair<string, object> a = new KeyValuePair<string, object>("Settings", b);
                            request.Parameters.Add(a);
                            KeyValuePair<string, object> aa = new KeyValuePair<string, object>("Requests", orgColl);
                            request.Parameters.Add(aa);
                            requestList.Add(request);

                            orgCollCount = 1;
                            orgColl = new OrganizationRequestCollection();
                        }

                        OrganizationRequest createrequest = new OrganizationRequest()
                        {
                            RequestName = requestName
                        };
                        createrequest["Target"] = ent;
                        orgColl.Add(createrequest);
                        orgCollCount++;
                    }

                    if (orgColl.Any())
                    {
                        OrganizationRequest request = new OrganizationRequest
                        {
                            RequestName = "ExecuteMultiple",
                            Parameters = new ParameterCollection()
                        };
                        ExecuteMultipleSettings b = new ExecuteMultipleSettings { ContinueOnError = false, ReturnResponses = true };
                        KeyValuePair<string, object> a = new KeyValuePair<string, object>("Settings", b);
                        request.Parameters.Add(a);
                        KeyValuePair<string, object> aa = new KeyValuePair<string, object>("Requests", orgColl);
                        request.Parameters.Add(aa);
                        requestList.Add(request);
                    }

                    foreach (OrganizationRequest r in requestList)
                    {
                        _xrmManager.Service.Execute(r);
                    }
                }
                catch (Exception ex)
                {
                    throw new WebException(ex.Message);
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }

            return true;
        }

        public bool CreateTravelCards()
        {
            try
            {
                _xrmManager = _initCrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand com = new SqlCommand("sp_CreateTravelCardImportTable", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                com.ExecuteNonQuery();

                string sql = "";
                sql += "select";
                sql += " customer_id, cardnumber, travelcardid, contactidx";
                sql += " from travelcards";
                sql += " order by customer_id";

                SqlCommand command = new SqlCommand(sql, con);
                SqlDataReader reader = command.ExecuteReader();

                List<OrganizationRequest> requestList = new List<OrganizationRequest>();
                OrganizationRequestCollection orgColl = new OrganizationRequestCollection();
                int orgCollCount = 1;

                try
                {
                    while (reader.Read())
                    {
                        // Create contact
                        string id = reader["customer_id"].ToString();
                        string cardnumber = reader["cardnumber"].ToString().Replace(@"\N", "");
                        string travelcardid = reader["travelcardid"].ToString().Replace(@"\N", "");
                        
                        string contactCrmid = reader["contactidx"].ToString();

                        Entity ent = new Entity("cgi_travelcard")
                        {
                            LogicalName = "cgi_travelcard"
                        };
                        ent.Attributes.Add("cgi_importid", id);
                        ent.Attributes.Add("cgi_travelcardnumber", cardnumber);
                        ent.Attributes.Add("cgi_travelcardname", cardnumber);
                        ent.Attributes.Add("cgi_contactid", new EntityReference("contact", new Guid(contactCrmid)));
                        
                        var requestName = "Create";
                        if (travelcardid != Guid.Empty.ToString())
                        {
                            ent.Id = new Guid(travelcardid);
                            requestName = "Update";
                        }

                        if (orgCollCount >= 1000)
                        {
                            OrganizationRequest request = new OrganizationRequest
                            {
                                RequestName = "ExecuteMultiple",
                                Parameters = new ParameterCollection()
                            };
                            ExecuteMultipleSettings b = new ExecuteMultipleSettings { ContinueOnError = false, ReturnResponses = true };
                            KeyValuePair<string, object> a = new KeyValuePair<string, object>("Settings", b);
                            request.Parameters.Add(a);
                            KeyValuePair<string, object> aa = new KeyValuePair<string, object>("Requests", orgColl);
                            request.Parameters.Add(aa);
                            requestList.Add(request);

                            orgCollCount = 1;
                            orgColl = new OrganizationRequestCollection();
                        }

                        OrganizationRequest createrequest = new OrganizationRequest()
                        {
                            RequestName = requestName
                        };
                        createrequest["Target"] = ent;
                        orgColl.Add(createrequest);
                        orgCollCount++;
                    }

                    if (orgColl.Any())
                    {
                        OrganizationRequest request = new OrganizationRequest
                        {
                            RequestName = "ExecuteMultiple",
                            Parameters = new ParameterCollection()
                        };
                        ExecuteMultipleSettings b = new ExecuteMultipleSettings { ContinueOnError = false, ReturnResponses = true };
                        KeyValuePair<string, object> a = new KeyValuePair<string, object>("Settings", b);
                        request.Parameters.Add(a);
                        KeyValuePair<string, object> aa = new KeyValuePair<string, object>("Requests", orgColl);
                        request.Parameters.Add(aa);
                        requestList.Add(request);
                    }

                    foreach (OrganizationRequest r in requestList)
                    {
                        _xrmManager.Service.Execute(r);
                    }
                }
                catch (Exception ex)
                {
                    throw new WebException(ex.Message);
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
            return true;
        }

        private XrmManager _initCrmManager()
        {
            try
            {
                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                string domain = ConfigurationManager.AppSettings["Domain"];
                string username = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();

                XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);
                return xrmMgr;
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }
    }
}
