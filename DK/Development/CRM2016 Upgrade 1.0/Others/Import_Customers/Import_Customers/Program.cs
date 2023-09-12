using System;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Import_Customers
{
    class Program
    {
        #region Main
        static void Main(string[] args)
        {
            try
            {
                var concrm = new xRMConnection("http://v-dkcrm-tst/Skanetrafiken")
                {
                    Authenticationtype = AuthenticationType.Default,
                    Crmtype = CRMType.OnPremis,
                    Username = @"D1\ ",
                    Password = " ",
                    Domain = "D1"
                };
                var service = concrm.GetService();
                
                SqlConnection con = new SqlConnection("Server=v-dksql-utv; Database=import; Trusted_Connection=True;");
                con.Open();

                string sql = "";
                sql += "select";
                sql += " distinct c.customer_id, c.customer_type, c.one_time_customer, c.company, c.address, c.address2, c.ZIP, c.city, c.province_state, c.ccode,";
                sql += " c.language, c.FAX, c.orgnr, c.VATnumber, c.customer_no, c.comments, c.pricelist_id, c.warehouse_id, c.customer_category_id, c.preferred_shippinglocation_id,";
                sql += " c.discount, c.currency, c.credit_limit, c.credit_open, c.customer_keys, c.state, c.validated_name, c.validated_firstname, c.validated_lastname, c.validated_address,";
                sql += " c.validated_ZIP, c.validated_city, c.validation_date, c.validation_service, c.validation_state, c.validation_string, c.default_payment_id, c.default_shipping_id,";
                sql += " c.ctext1, c.ctext2, c.ctext3, c.ctext4, c.ctext5, c.ctext6, c.cselect1, c.cselect2, c.cselect3, c.cselect4, c.cselect5,";
                sql += " c.cselect6, c.cselect7, c.cselect8, c.cselect9, c.cselect10,";
                sql += " c.affiliate_id, c.affiliate_date, c.affiliate_member, c.surname, c.lastname, c.phone, c.cellphone, c.email, c.login, c.password, c.want_newsletter, c.have_sent_welcomemail,";
                sql += " c.creditworthy, c.admin_id, c.order_count, c.order_totals, c.last_order, c.changed, c.created, c.synced, c.upd_state";
                sql += " from Customer c";
                sql += " order by c.customer_id";

                SqlCommand command = new SqlCommand(sql, con);
                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {

                        // Create account
                        string id = reader["customer_id"].ToString();
                        string firstname = reader["surname"].ToString().Replace(@"\N", "");
                        string lastname = reader["lastname"].ToString().Replace(@"\N", "");
                        string coaddress = reader["address2"].ToString().Replace(@"\N", "");
                        string address = reader["address"].ToString().Replace(@"\N", "");
                        string zip = reader["ZIP"].ToString().Replace(@"\N", "");
                        string city = reader["city"].ToString().Replace(@"\N", "");
                        string comments = reader["comments"].ToString().Replace(@"\N", "");
                        string state = reader["state"].ToString().Replace(@"\N", "");
                        string phone = reader["phone"].ToString().Replace(@"\N", "");
                        string cellphone = reader["cellphone"].ToString().Replace(@"\N", "");
                        string email = reader["email"].ToString().Replace(@"\N", "");
                        string newsletter = reader["want_newsletter"].ToString().Replace(@"\N", "");
                        string fax = reader["FAX"].ToString().Replace(@"\N", "");

                        string fullname = string.Format("{0} {1}", firstname, lastname);
                        
                        Entity ent = new Entity("account");
                        ent.Attributes.Add("cgi_importid", id);
                        ent.Attributes.Add("name", fullname);
                        ent.Attributes.Add("cgi_firstname", firstname);
                        ent.Attributes.Add("cgi_lastname", lastname);

                        OptionSetValue o = new OptionSetValue(1);
                        ent.Attributes.Add("accountcategorycode", o);
                        ent.Attributes.Add("address1_line1", coaddress);
                        ent.Attributes.Add("address1_line2", address);
                        ent.Attributes.Add("address1_postalcode", zip);
                        ent.Attributes.Add("address1_city", city);
                        ent.Attributes.Add("description", comments);

                        int statecode = 1;
                        if (state.ToUpper() == "ACTIVE")
                            statecode = 0;

                        ent.Attributes.Add("statecode", statecode);
                        ent.Attributes.Add("telephone2", phone);
                        ent.Attributes.Add("telephone1", cellphone);
                        ent.Attributes.Add("telephone3", fax);
                        ent.Attributes.Add("emailaddress1", email);

                        bool letternews = false;
                        if (!string.IsNullOrEmpty(newsletter))
                        {
                            if (newsletter == "1")
                                letternews = true;
                        }
                        ent.Attributes.Add("cgi_newsletter", letternews);

                        string fetchacc = "";
                        fetchacc += "<fetch version='1.0' mapping='logical' >";
                        fetchacc += "    <entity name='account'>";
                        fetchacc += "        <attribute name='accountid' />";
                        fetchacc += "        <filter type='and'>";
                        fetchacc += "            <condition attribute='cgi_importid' operator='eq' value='" + id + "' />";
                        fetchacc += "        </filter>";
                        fetchacc += "    </entity>";
                        fetchacc += "</fetch>";
                        
                        FetchExpression f = new FetchExpression(fetchacc);
                        var ents = service.RetrieveMultiple(f);
                        if (ents != null && ents.Entities.Any())
                        {
                            Console.WriteLine("{0}, {1} {2} : Update", id, firstname, lastname);
                            ent.Id = new Guid(ents[0].Id.ToString());
                            service.Update(ent);
                        }
                        else
                        {
                            Console.WriteLine("{0}, {1} {2} : Create", id, firstname, lastname);
                            service.Create(ent);
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
                    reader.Close();
                }


                //travelcard

                string sqlcard = "";
                sqlcard += " select";
                sqlcard += " c.customer_id, c.cardnumber";
                sqlcard += " from Customer c";
                sqlcard += " order by c.customer_id";
                SqlCommand commandCard = new SqlCommand(sqlcard, con);
                SqlDataReader readerCard = commandCard.ExecuteReader();

                try
                {
                    while (readerCard.Read())
                    {
                        string idC = readerCard["customer_id"].ToString();
                        string cardnumber = readerCard["cardnumber"].ToString().Replace(@"\N", "");

                        string fetch = "";
                        fetch += "<fetch version='1.0' mapping='logical' >";
                        fetch += "    <entity name='account'>";
                        fetch += "        <attribute name='name' />";
                        fetch += "        <attribute name='accountid' />";
                        fetch += "        <filter type='and'>";
                        fetch += "            <condition attribute='cgi_importid' operator='eq' value='" + idC + "' />";
                        fetch += "        </filter>";
                        fetch += "    </entity>";
                        fetch += "</fetch>";

                        FetchExpression f = new FetchExpression(fetch);
                        var ents = service.RetrieveMultiple(f);
                        if (ents != null && ents.Entities.Any())
                        {
                            Entity ent = ents[0];
                            string accountid = ent["accountid"].ToString();
                            string name = ent["name"].ToString();
                            EntityReference accref = new EntityReference("account", new Guid(accountid));

                            Entity entCard = new Entity("cgi_travelcard");
                            entCard.Attributes.Add("cgi_importid", idC);
                            entCard.Attributes.Add("cgi_travelcardnumber", cardnumber);
                            entCard.Attributes.Add("cgi_travelcardname", cardnumber);
                            entCard.Attributes.Add("cgi_accountid", accref);

                            string fetchExists = "";
                            fetchExists += " <fetch version='1.0' mapping='logical'>";
                            fetchExists += "  <entity name='cgi_travelcard'>";
                            fetchExists += "    <attribute name='cgi_travelcardid' />";
                            fetchExists += "    <filter type='and'>";
                            fetchExists += "      <condition attribute='cgi_importid' operator='eq' value='" + idC + "' />";
                            fetchExists += "    </filter>";
                            fetchExists += "  </entity>";
                            fetchExists += " </fetch>";

                            FetchExpression ft = new FetchExpression(fetchExists);
                            var entsCards = service.RetrieveMultiple(ft);
                            if (entsCards != null && entsCards.Entities.Any())
                            {
                                Console.WriteLine("{0}, {1} : Update", accountid, name);
                                entCard.Id = new Guid(entsCards[0].Id.ToString());
                                service.Update(entCard);
                            }
                            else
                            {
                                Console.WriteLine("{0}, {1} : Create", accountid, name);
                                service.Create(entCard);
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
                    readerCard.Close();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("");
            Console.WriteLine("Tryck enter för att avsluta!");
            Console.ReadLine();
        }
        #endregion
    }
}
