using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using CRM_Connection;
using Microsoft.Crm;
using Microsoft.Crm.Sdk;
using Microsoft.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Query;

namespace Import_Customers
{
    class Program
    {

        static void Main(string[] args)
        {

            xRMConnection _concrm;
            OrganizationServiceProxy _service;

            try
            {
                _concrm = new xRMConnection("http://v-dkcrm-tst/Skanetrafiken");
                _concrm.Authenticationtype = AuthenticationType.Default;
                _concrm.Crmtype = CRMType.OnPremis;
                _concrm.Username = @"D1\CRMAdmin";
                _concrm.Password = "uSEme2!nstal1";
                _concrm.Domain = "D1";
                _service = _concrm.GetService();
                
                SqlConnection _con = new SqlConnection("Server=v-dksql-utv; Database=import; Trusted_Connection=True;");
                _con.Open();

                string _sql = "";
                _sql += "select";
                _sql += " distinct c.customer_id, c.customer_type, c.one_time_customer, c.company, c.address, c.address2, c.ZIP, c.city, c.province_state, c.ccode,";
                _sql += " c.language, c.FAX, c.orgnr, c.VATnumber, c.customer_no, c.comments, c.pricelist_id, c.warehouse_id, c.customer_category_id, c.preferred_shippinglocation_id,";
                _sql += " c.discount, c.currency, c.credit_limit, c.credit_open, c.customer_keys, c.state, c.validated_name, c.validated_firstname, c.validated_lastname, c.validated_address,";
                _sql += " c.validated_ZIP, c.validated_city, c.validation_date, c.validation_service, c.validation_state, c.validation_string, c.default_payment_id, c.default_shipping_id,";
                _sql += " c.ctext1, c.ctext2, c.ctext3, c.ctext4, c.ctext5, c.ctext6, c.cselect1, c.cselect2, c.cselect3, c.cselect4, c.cselect5,";
                _sql += " c.cselect6, c.cselect7, c.cselect8, c.cselect9, c.cselect10,";
                _sql += " c.affiliate_id, c.affiliate_date, c.affiliate_member, c.surname, c.lastname, c.phone, c.cellphone, c.email, c.login, c.password, c.want_newsletter, c.have_sent_welcomemail,";
                _sql += " c.creditworthy, c.admin_id, c.order_count, c.order_totals, c.last_order, c.changed, c.created, c.synced, c.upd_state";
                _sql += " from Customer c";
                _sql += " order by c.customer_id";

                SqlCommand _command = new SqlCommand(_sql, _con);
                SqlDataReader _reader = _command.ExecuteReader();

                try
                {
                    while (_reader.Read())
                    {

                        // Create account
                        string _id = _reader["customer_id"].ToString();
                        string _firstname = _reader["surname"].ToString().Replace(@"\N", "");
                        string _lastname = _reader["lastname"].ToString().Replace(@"\N", "");
                        string _coaddress = _reader["address2"].ToString().Replace(@"\N", "");
                        string _address = _reader["address"].ToString().Replace(@"\N", "");
                        string _zip = _reader["ZIP"].ToString().Replace(@"\N", "");
                        string _city = _reader["city"].ToString().Replace(@"\N", "");
                        string _comments = _reader["comments"].ToString().Replace(@"\N", "");
                        string _state = _reader["state"].ToString().Replace(@"\N", "");
                        string _phone = _reader["phone"].ToString().Replace(@"\N", "");
                        string _cellphone = _reader["cellphone"].ToString().Replace(@"\N", "");
                        string _email = _reader["email"].ToString().Replace(@"\N", "");
                        string _newsletter = _reader["want_newsletter"].ToString().Replace(@"\N", "");
                        string _FAX = _reader["FAX"].ToString().Replace(@"\N", "");

                        string _fullname = string.Format("{0} {1}", _firstname, _lastname);
                        
                        Entity _ent = new Entity("account");
                        _ent.Attributes.Add("cgi_importid", _id);
                        _ent.Attributes.Add("name", _fullname);
                        _ent.Attributes.Add("cgi_firstname", _firstname);
                        _ent.Attributes.Add("cgi_lastname", _lastname);

                        OptionSetValue _o = new OptionSetValue(1);
                        _ent.Attributes.Add("accountcategorycode", _o);
                        _ent.Attributes.Add("address1_line1", _coaddress);
                        _ent.Attributes.Add("address1_line2", _address);
                        _ent.Attributes.Add("address1_postalcode", _zip);
                        _ent.Attributes.Add("address1_city", _city);
                        _ent.Attributes.Add("description", _comments);

                        int _statecode = 1;
                        if (_state.ToUpper() == "ACTIVE")
                            _statecode = 0;

                        _ent.Attributes.Add("statecode", _statecode);
                        _ent.Attributes.Add("telephone2", _phone);
                        _ent.Attributes.Add("telephone1", _cellphone);
                        _ent.Attributes.Add("telephone3", _FAX);
                        _ent.Attributes.Add("emailaddress1", _email);

                        bool _letternews = false;
                        if (!string.IsNullOrEmpty(_newsletter))
                        {
                            if (_newsletter == "1")
                                _letternews = true;
                        }
                        _ent.Attributes.Add("cgi_newsletter", _letternews);

                        string _fetchacc = "";
                        _fetchacc += "<fetch version='1.0' mapping='logical' >";
                        _fetchacc += "    <entity name='account'>";
                        _fetchacc += "        <attribute name='accountid' />";
                        _fetchacc += "        <filter type='and'>";
                        _fetchacc += "            <condition attribute='cgi_importid' operator='eq' value='" + _id + "' />";
                        _fetchacc += "        </filter>";
                        _fetchacc += "    </entity>";
                        _fetchacc += "</fetch>";
                        
                        FetchExpression _f = new FetchExpression(_fetchacc);
                        EntityCollection _ents = new EntityCollection();
                        _ents = _service.RetrieveMultiple(_f);
                        if (_ents != null && _ents.Entities.Count() > 0)
                        {
                            Console.WriteLine(String.Format("{0}, {1} {2} : Update", _id, _firstname, _lastname));
                            _ent.Id = new Guid(_ents[0].Id.ToString());
                            _service.Update(_ent);
                        }
                        else
                        {
                            Console.WriteLine(String.Format("{0}, {1} {2} : Create", _id, _firstname, _lastname));
                            _service.Create(_ent);
                        }
                        
                        //end account

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    // Always call Close when done reading.
                    _reader.Close();
                }


                //travelcard

                string _sqlcard = "";
                _sqlcard += " select";
                _sqlcard += " c.customer_id, c.cardnumber";
                _sqlcard += " from Customer c";
                _sqlcard += " order by c.customer_id";
                SqlCommand _commandCard = new SqlCommand(_sqlcard, _con);
                SqlDataReader _readerCard = _commandCard.ExecuteReader();

                try
                {
                    while (_readerCard.Read())
                    {
                        string _idC = _readerCard["customer_id"].ToString();
                        string _cardnumber = _readerCard["cardnumber"].ToString().Replace(@"\N", "");

                        string _fetch = "";
                        _fetch += "<fetch version='1.0' mapping='logical' >";
                        _fetch += "    <entity name='account'>";
                        _fetch += "        <attribute name='name' />";
                        _fetch += "        <attribute name='accountid' />";
                        _fetch += "        <filter type='and'>";
                        _fetch += "            <condition attribute='cgi_importid' operator='eq' value='" + _idC + "' />";
                        _fetch += "        </filter>";
                        _fetch += "    </entity>";
                        _fetch += "</fetch>";

                        FetchExpression _f = new FetchExpression(_fetch);
                        EntityCollection _ents = new EntityCollection();
                        _ents = _service.RetrieveMultiple(_f);
                        if (_ents != null && _ents.Entities.Count() > 0)
                        {
                            Entity _ent = _ents[0];
                            string _accountid = _ent["accountid"].ToString();
                            string _name = _ent["name"].ToString();
                            EntityReference _accref = new EntityReference("account", new Guid(_accountid));

                            Entity _entCard = new Entity("cgi_travelcard");
                            _entCard.Attributes.Add("cgi_importid", _idC);
                            _entCard.Attributes.Add("cgi_travelcardnumber", _cardnumber);
                            _entCard.Attributes.Add("cgi_travelcardname", _cardnumber);
                            _entCard.Attributes.Add("cgi_accountid", _accref);

                            string _fetchExists = "";
                            _fetchExists += " <fetch version='1.0' mapping='logical'>";
                            _fetchExists += "  <entity name='cgi_travelcard'>";
                            _fetchExists += "    <attribute name='cgi_travelcardid' />";
                            _fetchExists += "    <filter type='and'>";
                            _fetchExists += "      <condition attribute='cgi_importid' operator='eq' value='" + _idC + "' />";
                            _fetchExists += "    </filter>";
                            _fetchExists += "  </entity>";
                            _fetchExists += " </fetch>";

                            FetchExpression _ft = new FetchExpression(_fetchExists);
                            EntityCollection _entsCards = new EntityCollection();
                            _entsCards = _service.RetrieveMultiple(_ft);
                            if (_entsCards != null && _entsCards.Entities.Count() > 0)
                            {
                                Console.WriteLine(String.Format("{0}, {1} : Update", _accountid, _name));
                                _entCard.Id = new Guid(_entsCards[0].Id.ToString());
                                _service.Update(_entCard);
                            }
                            else
                            {
                                Console.WriteLine(String.Format("{0}, {1} : Create", _accountid, _name));
                                _service.Create(_entCard);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    // Always call Close when done reading.
                    _readerCard.Close();
                    _con.Close();
                }

                //end travelcard

                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("");
            Console.WriteLine("Tryck enter för att avsluta!");
            Console.ReadLine();

        }

    }
}
