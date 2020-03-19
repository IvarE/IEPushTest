using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CGIXrmWin;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Crm;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
using System.IO;
using CRM2013.SkanetrafikenPlugins;

namespace CGIXrmCreateCaseService
{
    public class CreateCaseManager
    {
        XrmManager _xrmManager;
        private object LockSql = new object();

        public CreateCaseManager() 
        {
            _xrmManager = _initCrmManager();
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
        
        //Create incident from web.
        internal void RequestCreateCase(CreateCaseRequest request)
        {
            CreateCaseResponse _returnValue = new CreateCaseResponse();
            
            try
            {
                _createLogfile(request);

                account _account = null;

                if (request.Customer != null && request.Customer.Value != null && request.Customer.Value != Guid.Empty)
                {
                    //_account = _getAccountFromId(request, out _emailCount);
                    _account = _getAccountFromId((Guid)request.Customer, request.CustomerType);
                    if (_account == null)
                        throw new Exception(string.Format("Kunden med id {0} hittas inte i databasen!", request.Customer.ToString()));

                    _account.EmailCount = _account.EmailCount;
                }
                else
                {
                    if (request.ContactCustomer == true)
                    {
                        account _acc = _getAccountFromEmail(request.EmailAddress, request.CustomerType);
                        if (_acc != null)
                        {
                            if (_acc.EmailCount == 1)
                            {
                                _account = new account();
                                _account.AccountId = _acc.AccountId;
                                _account.EmailCount = _acc.EmailCount;
                            }
                            else
                            {
                                setting _setting = _getDefaultCustomer();
                                if (_setting != null)
                                {
                                    _account = new account();
                                    _account.AccountId = new Guid(_setting.DefaultCustomer);
                                    _account.AccountName = "CGI_DEFAULT";
                                    _account.EmailCount = _acc.EmailCount;
                                }
                                else
                                {
                                    throw new Exception("Det finns ingen default kund definierad!");
                                }
                            }
                        }
                        else
                        {
                            //Create private customer (contact) from firstname, lastname and emailaddress
                            _account = new account();
                            Guid _g = _createCustomer(request.FirstName, request.LastName, request.EmailAddress, request.MobilePhoneNumber);
                            _account.AccountId = _g;
                            _account.EmailCount = 1;
                        }
                    }
                    else
                    {
                        setting _setting = _getDefaultCustomer();
                        if (_setting != null)
                        {
                            _account = new account();
                            _account.AccountId = new Guid(_setting.DefaultCustomer);
                            _account.AccountName = "CGI_DEFAULT";
                            _account.EmailCount = 1;
                        }
                        else
                            throw new Exception("Det finns ingen default kund definierad!");
                    }
                }

                //create case from request
                Guid _incidentid = _createIncident(request, _account);
                
                //add documents from collection
                if (request.DocumentList != null && request.DocumentList.Count() > 0)
                {
                    foreach (document _doc in request.DocumentList)
                    {
                        _createAnnotation(_doc, _incidentid);
                    }
                }
                //add filelinks from collection
                if (request.FileLinks!=null && request.FileLinks.Count()>0) {
                    foreach (FileLink fileLink in request.FileLinks)
                    {
                        createFileLink(fileLink, _incidentid);
                    } 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void createFileLink(FileLink fileLink, Guid incidentid)
        {
            try
            {
                if (string.IsNullOrEmpty(fileLink.URL) == false)
                {
                    Entity entity = new Entity("cgi_filelink");
                    entity.Attributes["cgi_incidentid"] = new EntityReference("incident", incidentid);
                    entity.Attributes["cgi_url"] = fileLink.URL.Replace(@"\", @"/");
                    Guid _g = _xrmManager.Create(entity);
                }
            }
            catch { /* Write to log? */ }
        }

        //Create incident from AutoRG
        internal CreateAutoRGCaseResponse RequestCreateAutoRGCase(CreateAutoRGCaseRequest request)
        {
            CreateAutoRGCaseResponse _response = new CreateAutoRGCaseResponse();
            _response.Success = false;
            _response.ErrorMessage = "";


            try
            {

                Guid CustomerIdGuid = Guid.Empty;
                if (request.Customer != null && request.Customer != "")
                {
                    if (!Guid.TryParse(request.Customer, out CustomerIdGuid))
                    {
                        _response.Success = false;
                        _response.ErrorMessage = string.Format("Unable to parse the CustomerId from : {0}", request.Customer);
                        return _response;
                    }
                }


                //sök upp eller skapa en kund, privat(contact) eller organisation (account)
                account _account = null;

                if (CustomerIdGuid != Guid.Empty)
                {
                    _account = _getAccountFromId(CustomerIdGuid, (CustomerType)request.CustomerType);
                    if (_account == null)
                        throw new Exception(string.Format("Kunden med id {0} hittas inte i databasen!", request.Customer.ToString()));

                }
                else
                {
                    account _acc = _getAccountFromEmail(request.EmailAddress, (CustomerType)request.CustomerType);
                    if (_acc != null)
                    {
                        if (_acc.EmailCount == 1)
                        {
                            _account = _acc;
                        }
                        else
                        {
                            setting _crmsetting = _getDefaultCustomer();
                            if (_crmsetting != null)
                            {
                                _account = new account();
                                _account.AccountId = new Guid(_crmsetting.DefaultCustomer);
                                _account.AccountName = "CGI_DEFAULT";
                                _account.EmailCount = _acc.EmailCount;
                            }
                            else
                            {
                                throw new Exception("Det finns ingen default kund definierad!");
                            }
                        }
                    }
                    else
                    {
                        //Create private customer (contact) from firstname, lastname and emailaddress
                        _account = new account();
                        Guid ContactId = _createCustomer(request.FirstName, request.LastName, request.EmailAddress, "");
                        _account.AccountId = ContactId;
                        _account.EmailCount = 1;
                    }
                }

                //create case from request
                _response.CaseID = _createIncidentRGOL(request, _account).ToString();
                _response.Success = true;
            }
            catch (FaultException faultex)
            {
                _response.CaseID = "";
                _response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                _response.CaseID = "";
                _response.ErrorMessage = ex.Message;
            }

            return _response;
        }

        internal UpdateAutoRGCaseResponse RequestCreateTravelInformationLinkedToCase(UpdateAutoRGCaseRequest request)
        {
            UpdateAutoRGCaseResponse _response = new UpdateAutoRGCaseResponse()
            {
                RefundID = "",
                Success = false,
                ErrorMessage = ""
            };

            try
            {

                Guid caseId;
                if (!Guid.TryParse(request.CaseID, out caseId))
                {
                    throw new Exception("Request is missing parameter CaseID.");
                }

                Entity travelInformation = new Entity("cgi_travelinformation");
                travelInformation.Attributes = new AttributeCollection();


                travelInformation.Attributes.Add("cgi_caseid", new EntityReference("incident", caseId));

                if (!string.IsNullOrEmpty(request.TravelInformationLine))
                {
                    travelInformation.Attributes.Add("cgi_line", request.TravelInformationLine);
                }

                if (!string.IsNullOrEmpty(request.TravelInformationCompany))
                {
                    travelInformation.Attributes.Add("cgi_contractor", request.TravelInformationCompany);
                }

                if (request.TravelInformationStartPlanned != null)
                {
                    travelInformation.Attributes.Add("cgi_startplanned", request.TravelInformationStartPlanned);
                }

                if (request.TravelInformationArrivalPlanned != null)
                {
                    travelInformation.Attributes.Add("cgi_arivalplanned", request.TravelInformationArrivalPlanned);
                }

                if (!string.IsNullOrEmpty(request.TravelInformationTitle))
                {
                    travelInformation.Attributes.Add("cgi_travelinformation", request.TravelInformationTitle);
                }

                if (!string.IsNullOrEmpty(request.TravelInformationArrivalActual))
                {
                    travelInformation.Attributes.Add("cgi_arivalactual", request.TravelInformationArrivalActual);
                }
                if (!string.IsNullOrEmpty(request.TravelInformationCity))
                {
                    travelInformation.Attributes.Add("cgi_city", request.TravelInformationCity);
                }

                if (!string.IsNullOrEmpty(request.TravelInformationDeviationMessage))
                {
                    travelInformation.Attributes.Add("cgi_deviationmessage", request.TravelInformationDeviationMessage);
                }
                if (!string.IsNullOrEmpty(request.TravelInformationDirectionText))
                {
                    travelInformation.Attributes.Add("cgi_directiontext", request.TravelInformationDirectionText);
                }

                if (!string.IsNullOrEmpty(request.TravelInformationDisplayText))
                {
                    //travelInformation.Attributes.Add("cgi_displaytext", String.Format("{0} {1}",  request.TravelInformationCompany, request.TravelInformationDisplayText));

                    //Start time

                    string plannedStartTime = "";
                    TimeSpan diffStartTimeSpan = new TimeSpan();
                    string actualStartTime;
                    string diffStartTime = "";
                    try
                    {
                        actualStartTime = Convert.ToDateTime(request.TravelInformationStartActual).ToString("H:mm:ss");
                        plannedStartTime = request.TravelInformationStartPlanned.Value.ToString("H:mm:ss");
                        diffStartTimeSpan = Convert.ToDateTime(request.TravelInformationStartActual) - request.TravelInformationStartPlanned.Value;

                        if (diffStartTimeSpan.TotalMinutes > 0)
                        {
                            diffStartTime += "+";
                        }

                        if(diffStartTimeSpan.TotalDays > 364 || diffStartTimeSpan.TotalDays < -364)
                        {
                            diffStartTime = "X";
                            actualStartTime = "";
                        }
                        else
                        {
                            diffStartTime += diffStartTimeSpan.TotalMinutes.ToString();
                        }
                    }
                    catch
                    {
                        diffStartTime = "X";
                        actualStartTime = "";
                    }


                    //Arrive time
                    string plannedEndTime = "";
                    TimeSpan diffEndTimeSpan = new TimeSpan();
                    string actualEndTime;
                    string diffEndTime = "";

                    try
                    {
                        plannedEndTime = request.TravelInformationArrivalPlanned.Value.ToString("H:mm:ss");
                        actualEndTime = Convert.ToDateTime(request.TravelInformationArrivalActual).ToString("H:mm:ss");
                        diffEndTimeSpan = Convert.ToDateTime(request.TravelInformationArrivalActual) - request.TravelInformationArrivalPlanned.Value;

                        if (diffEndTimeSpan.TotalMinutes > 0)
                        {
                            diffEndTime += "+";
                        }

                        if(diffEndTimeSpan.TotalDays > 364 || diffEndTimeSpan.TotalDays < -364)
                        {
                            diffEndTime = "X";
                            actualEndTime = "";
                        }
                        else
                        {
                            diffEndTime += diffEndTimeSpan.TotalMinutes.ToString();
                        }
                    }
                    catch
                    {
                        diffEndTime = "X";
                        actualEndTime = "";
                    }
                    

                    string travelCompany = "(ingen operatör)";
                    if (!string.IsNullOrEmpty(request.TravelInformationDisplayText))
                    {
                        travelCompany = request.TravelInformationCompany;
                    }

                    travelInformation.Attributes.Add("cgi_displaytext", String.Format("Linje: [{0}] Tur:{1}[{2}({3})] {4} - {5}[{6}({7})] {8} Entreprenör: {9}", request.TravelInformationLine, plannedStartTime, actualStartTime, diffStartTime, request.TravelInformationStart, plannedEndTime, actualEndTime, diffEndTime, request.TravelInformationStop, travelCompany));
                }
                else if (!string.IsNullOrEmpty(request.TravelInformationDisplayText))
                {
                    //travelInformation.Attributes.Add("cgi_displaytext", String.Format("{0} (ingen operatör)", request.TravelInformationDisplayText));
                }

                if (!string.IsNullOrEmpty(request.TravelInformationStart))
                {
                    travelInformation.Attributes.Add("cgi_start", request.TravelInformationStart);
                }

                if (!string.IsNullOrEmpty(request.TravelInformationStartActual))
                {
                    travelInformation.Attributes.Add("cgi_startactual", request.TravelInformationStartActual);
                }
                if (!string.IsNullOrEmpty(request.TravelInformationStop))
                {
                    travelInformation.Attributes.Add("cgi_stop", request.TravelInformationStop);
                }

                if (!string.IsNullOrEmpty(request.TravelInformationTour))
                {
                    travelInformation.Attributes.Add("cgi_tour", request.TravelInformationTour);
                }

                if (!string.IsNullOrEmpty(request.TravelInformationTransport))
                {
                    travelInformation.Attributes.Add("cgi_transport", request.TravelInformationTransport);
                }

                Guid travelInformationId = _xrmManager.Create(travelInformation);

                //_response.TravelInformationID = travelInformationId.ToString();

                _response.Success = true;

            }
            catch (FaultException faultex)
            {
                _response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
            }

            return _response;
        }


        //Create incident from AutoRG
        internal UpdateAutoRGCaseResponse RequestCreateAutoRGRefundDecision(UpdateAutoRGCaseRequest request)
        {
            UpdateAutoRGCaseResponse _response = new UpdateAutoRGCaseResponse();
            _response.Success = false;
            _response.ErrorMessage = "";


            try
            {
                Guid CaseIdGuid;
                if (!Guid.TryParse(request.CaseID, out CaseIdGuid))
                {
                    _response.Success = false;
                    _response.ErrorMessage = string.Format("Unable to parse the CaseID: {0}", request.CaseID);
                    return _response;
                }
                incident _case = _getIncidentFromID(CaseIdGuid);

                if (!request.Approved)
                    request.RefundType = 0; //ska ge avslag

                //create decision from request
                RGOLSetting _setting = _getRGOLSettingFromNo(request.RefundType);

                basecurrency _currency = _getCurrency();

                Guid RefundId = _createDecisionRGOL(request.Value, _setting, _case, _currency.BaseCurrencyId, request.InternalMessage, request.CustomerMessage);

                if (request.Approved)
                {
                    _SetRefundDescisionStatus(RefundId, 0, 285050005);//285050005 = Approved
                    ReimbursementHandler rh = new ReimbursementHandler();
                    rh.ExecuteRefundAndUpdatesStatus(RefundId, _xrmManager.Service);
                    //leave incident open
                }
                else
                {
                    _SetRefundDescisionStatus(RefundId, 0, 285050006);//0 = Active, Declined = 285 050 006
                    _closeIncident(_case.IncidentId);
                }
                _response.RefundID = RefundId.ToString();
                _response.Success = true;
            }
            catch (FaultException faultex)
            {
                _response.RefundID = "";
                _response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                _response.RefundID = "";
                _response.ErrorMessage = ex.Message;
            }

            return _response;
        }

        public void _SetRefundDescisionStatus(Guid RefundId, int state, int status)
        {
            //State value: 0 = active, corresponding Status values: 1 = New
            //State value: 1 = inactive, corresponding Status values: 2 = Declined, 285050000 = Approved, 285050001 =Approved -Transaction failed


            SetStateRequest statusrequest = new SetStateRequest();
            statusrequest.EntityMoniker = new EntityReference("cgi_refund", RefundId);
            statusrequest.State = new OptionSetValue(state);
            statusrequest.Status = new OptionSetValue(status);
            SetStateResponse response = (SetStateResponse)_xrmManager.Service.Execute(statusrequest);
        }

        public void _closeIncident(Guid IncidentId)
        {
            Entity _caseresolution = new Entity("incidentresolution");
            _caseresolution.Attributes.Add("incidentid", new EntityReference("incident", IncidentId));
            _caseresolution.Attributes.Add("subject", "Problemet löst.");

            CloseIncidentRequest _closerequest = new CloseIncidentRequest()
            {
                IncidentResolution = _caseresolution,
                //RequestName = "CloseIncident",
                Status = new OptionSetValue(5)
            };
            CloseIncidentResponse _closeresponse = (CloseIncidentResponse)_xrmManager.Service.Execute(_closerequest);

        }

        /*
        //Update incident from AutoRG
        internal UpdateAutoRGCaseResponse RequestUpdateAutoRGCase(UpdateAutoRGCaseRequest request)
        {
            UpdateAutoRGCaseResponse _response = new UpdateAutoRGCaseResponse();

            try
            {
                //find case from id
                incident _case = _getIncidentFromID(request);
                if (_case != null)
                {
                    //create decision from request
                    RGOLSetting _setting = _getRGOLSettingFromNo(request);
                    if (_setting != null)
                    {
                        basecurrency _currency = _getCurrency();
                        if (_currency != null)
                        {
                            string _errormessage = "";
                            Guid _g = _createDecisionRGOL(request, _setting, _case, _currency.BaseCurrencyId, out _errormessage);
                            if (_g != Guid.Empty)
                            {
                                string _closeIncidentErrormassge = "";
                                if (_closeIncident(request, out _closeIncidentErrormassge) == true)
                                {
                                    _response.UpdateCase = true;
                                    _response.ErrorMessage = "";
                                }
                                else
                                {
                                    _response.UpdateCase = false;
                                    _response.ErrorMessage = _closeIncidentErrormassge;
                                }
                            }
                            else
                            {
                                _response.UpdateCase = false;
                                _response.ErrorMessage = _errormessage;
                            }
                        }
                        else
                        {
                            _response.UpdateCase = false;
                            _response.ErrorMessage = "Default valuta hittas inte på organisationen!";
                        }
                    }
                    else
                    {
                        _response.UpdateCase = false;
                        _response.ErrorMessage = string.Format("RGOLsetting hittas inte för typen : {0}", request.RefundType.ToString());
                    }

                }
                else
                {
                    _response.UpdateCase = false;
                    _response.ErrorMessage = string.Format("Det angivna ärendet {0} hittas inte!", request.CaseID);
                }
            }
            catch (FaultException faultex)
            {
                _response.UpdateCase = false;
                _response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                _response.UpdateCase = false;
                _response.ErrorMessage = ex.Message;
            }
            
            return _response;
        }
        */

        // SQL
        private account _getAccountFromId(Guid customerid, CustomerType customerType)
        {
            account _returnValue = null;
            
            try
            {
                SqlConnection sqlCon = OpenSQL();
                AccountSqlList accounts = null;
                using (SqlCommand command = new SqlCommand("sp_GetAccountFromAccountId", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter { ParameterName = "@accountid", SqlDbType = System.Data.SqlDbType.UniqueIdentifier, SqlValue = customerid });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@type", SqlDbType = System.Data.SqlDbType.Int, SqlValue = (int)customerType });

                    XmlReader reader = command.ExecuteXmlReader();
                    if (reader != null)
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(AccountSqlList));
                        accounts = ser.Deserialize(reader) as AccountSqlList;
                    }
                    reader.Close();
                    CloseSQL(sqlCon);

                    if (accounts != null && accounts.Accounts != null && accounts.Accounts.Count() > 0)
                    {
                        _returnValue = accounts.Accounts[0];
                        _returnValue.EmailCount = accounts.Accounts.Count();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _returnValue;
        }

        private account _getAccountFromIdRGOL(Guid customerid, CustomerType customerType)
        {
            account _returnValue = null;
            
            try
            {
                SqlConnection sqlCon = OpenSQL();
                AccountSqlList accounts = null;
                using (SqlCommand command = new SqlCommand("sp_GetAccountFromAccountId", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter { ParameterName = "@accountid", SqlDbType = System.Data.SqlDbType.UniqueIdentifier, SqlValue = customerid });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@type", SqlDbType = System.Data.SqlDbType.Int, SqlValue = (int)customerType });

                    XmlReader reader = command.ExecuteXmlReader();
                    if (reader != null)
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(AccountSqlList));
                        accounts = ser.Deserialize(reader) as AccountSqlList;
                    }
                    reader.Close();
                    CloseSQL(sqlCon);

                    if (accounts != null && accounts.Accounts != null && accounts.Accounts.Count() > 0)
                    {
                        _returnValue = accounts.Accounts[0];
                        _returnValue.EmailCount = accounts.Accounts.Count();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _returnValue;
        }

        private account _getAccountFromEmail(string emailAddress, CustomerType customerType)
        {
            account _returnValue = null;
            
            try
            {
                SqlConnection sqlCon = OpenSQL();
                AccountSqlList accounts = null;
                using (SqlCommand command = new SqlCommand("sp_GetAccountFromEmail", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter { ParameterName = "@emailaddress", SqlDbType = System.Data.SqlDbType.VarChar, SqlValue = emailAddress });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@type", SqlDbType = System.Data.SqlDbType.Int, SqlValue = (int)customerType });

                    XmlReader reader = command.ExecuteXmlReader();
                    if (reader != null)
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(AccountSqlList));
                        accounts = ser.Deserialize(reader) as AccountSqlList;
                    }
                    reader.Close();
                    CloseSQL(sqlCon);

                    if (accounts != null && accounts.Accounts != null && accounts.Accounts.Count() > 0)
                    {
                        _returnValue = accounts.Accounts[0];
                        _returnValue.EmailCount = accounts.Accounts.Count();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _returnValue;
        }

        private account _getAccountFromEmailRGOL(CreateAutoRGCaseRequest request, out string emailcount)
        {
            account _returnValue = null;
            emailcount = "0";

            try
            {
                SqlConnection sqlCon = OpenSQL();
                AccountSqlList accounts = null;
                using (SqlCommand command = new SqlCommand("sp_GetAccountFromEmail", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter { ParameterName = "@emailaddress", SqlDbType = System.Data.SqlDbType.VarChar, SqlValue = request.EmailAddress });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@type", SqlDbType = System.Data.SqlDbType.Int, SqlValue = (int)request.CustomerType });

                    XmlReader reader = command.ExecuteXmlReader();
                    if (reader != null)
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(AccountSqlList));
                        accounts = ser.Deserialize(reader) as AccountSqlList;
                    }
                    reader.Close();
                    CloseSQL(sqlCon);

                    if (accounts != null && accounts.Accounts != null && accounts.Accounts.Count() > 0)
                    {
                        _returnValue = accounts.Accounts[0];
                        emailcount = accounts.Accounts.Count().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _returnValue;
        }

        private setting _getDefaultCustomer()
        {
            setting _returnValue = null;

            try
            {
                SqlConnection sqlCon = OpenSQL();
                SettingSqlList settings = null;
                using (SqlCommand command = new SqlCommand("sp_GetDefaultCustomer", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    XmlReader reader = command.ExecuteXmlReader();
                    if (reader != null)
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(SettingSqlList));
                        settings = ser.Deserialize(reader) as SettingSqlList;
                    }
                    reader.Close();
                    CloseSQL(sqlCon);

                    if (settings != null && settings.Settings != null && settings.Settings.Count() > 0)
                    {
                        _returnValue = settings.Settings[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _returnValue;
        
        }

        private incident _getIncidentFromID(Guid incidentid)
        {
            incident _returnValue = null;
            
            try
            {
                SqlConnection sqlCon = OpenSQL();
                IncidentSqlList incidents = null;
                using (SqlCommand command = new SqlCommand("sp_GetIncidentFromIncidentId", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter { ParameterName = "@incidentid", SqlDbType = System.Data.SqlDbType.UniqueIdentifier, SqlValue = incidentid });
                    
                    XmlReader reader = command.ExecuteXmlReader();
                    if (reader != null)
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(IncidentSqlList));
                        incidents = ser.Deserialize(reader) as IncidentSqlList;
                    }
                    reader.Close();
                    CloseSQL(sqlCon);

                    if (incidents != null && incidents.Incidents != null && incidents.Incidents.Count() > 0)
                    {
                        _returnValue = incidents.Incidents[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _returnValue;
        }

        private RGOLSetting _getRGOLSettingFromNo(int RefundType)
        {
            RGOLSetting _returnValue = null;

            try
            {
                SqlConnection sqlCon = OpenSQL();
                RGOLSettingSqlList rgolsettings = null;
                using (SqlCommand command = new SqlCommand("sp_GetRGOLSetting", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter { ParameterName = "@rgolsetting", SqlDbType = System.Data.SqlDbType.Int, SqlValue = RefundType });

                    XmlReader reader = command.ExecuteXmlReader();
                    if (reader != null)
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(RGOLSettingSqlList));
                        rgolsettings = ser.Deserialize(reader) as RGOLSettingSqlList;
                    }
                    reader.Close();
                    CloseSQL(sqlCon);

                    if (rgolsettings != null && rgolsettings.RGOLSettings != null && rgolsettings.RGOLSettings.Count() > 0)
                    {
                        _returnValue = rgolsettings.RGOLSettings[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _returnValue;
        }

        private string _getTravelCardIdFromCardNumber(string cardnumber)
        {
            string _returnValue = null;

            try
            {
                SqlConnection sqlCon = OpenSQL();
                CardSqlList _cards = null;
                using (SqlCommand command = new SqlCommand("sp_GetTravelCard", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter { ParameterName = "@cardnumber", SqlDbType = System.Data.SqlDbType.VarChar, SqlValue = cardnumber });

                    XmlReader reader = command.ExecuteXmlReader();
                    if (reader != null)
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(CardSqlList));
                        _cards = ser.Deserialize(reader) as CardSqlList;
                    }
                    reader.Close();
                    CloseSQL(sqlCon);

                    if (_cards != null && _cards.Cards != null && _cards.Cards.Count() > 0)
                    {
                        _returnValue = _cards.Cards[0].TravelCardId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _returnValue;
        }


        // SQL

        // Entities
            Entity incidentEntity = new Entity("incident");

        private Guid _createIncident(CreateCaseRequest request, account customer)
        {
            incidentEntity.Attributes = new AttributeCollection();

            incidentEntity.Attributes.Add("caseorigincode", new OptionSetValue(3));
            
            if (!string.IsNullOrEmpty(request.CustomersCategory))
                incidentEntity.Attributes.Add("cgi_customers_category", request.CustomersCategory);

            if (!string.IsNullOrEmpty(request.CustomersSubcategory))
                incidentEntity.Attributes.Add("cgi_customers_subcategory", request.CustomersSubcategory);

            int _sendtoqueue = _getSendToQueue(request.CustomersCategory, request.CustomersSubcategory);
            incidentEntity.Attributes.Add("cgi_sendtoqueue", new OptionSetValue(_sendtoqueue));
            
            string titleText = String.Empty;
            string descriptionText = String.Empty;

            if (!string.IsNullOrEmpty(request.Title))
            {
                titleText = request.Title;
            }
            if (_sendtoqueue == 285050008)
            {
                titleText = "Bestridan kontrollavgift";
            }
            
            incidentEntity.Attributes.Add("title", titleText);

            if (!string.IsNullOrEmpty(request.Description))
                descriptionText = String.Format("{0}{1}", request.Description, Environment.NewLine);

            if (customer.AccountName == "CGI_DEFAULT")
            {
                incidentEntity.Attributes.Add("customerid", new EntityReference("account", customer.AccountId));
            }
            else
            {
                if (request.CustomerType == CustomerType.Organisation)
                {
                    incidentEntity.Attributes.Add("cgi_accountid", new EntityReference("account", customer.AccountId));
                    incidentEntity.Attributes.Add("customerid", new EntityReference("account", customer.AccountId));
                }

                if (request.CustomerType == CustomerType.Private)
                {
                    incidentEntity.Attributes.Add("cgi_contactid", new EntityReference("contact", customer.AccountId));
                    incidentEntity.Attributes.Add("customerid", new EntityReference("contact", customer.AccountId));
                }
            }

            if (!string.IsNullOrEmpty(request.InvoiceNumber))
            {
                incidentEntity.Attributes.Add("cgi_invoiceno", request.InvoiceNumber);
                descriptionText += String.Format("{0}Fakturanummer: {1}", Environment.NewLine, request.InvoiceNumber);
            }

            if (!string.IsNullOrEmpty(request.ControlFeeNumber))
            {
                incidentEntity.Attributes.Add("cgi_controlfeeno", request.ControlFeeNumber);
                descriptionText += String.Format("{0}Kontrollavgiftsnummer: {1}", Environment.NewLine, request.ControlFeeNumber);
            }

            if (!string.IsNullOrEmpty(request.CardNumber))
            {
                incidentEntity.Attributes.Add("cgi_unregisterdtravelcard", request.CardNumber);

                //Rem Do not save travelcard in lookup on case.
                //string _travelCardId = _getTravelCardIdFromCardNumber(request.CardNumber);
                //if (!string.IsNullOrEmpty(_travelCardId))
                //    incidentEntity.Attributes.Add("cgi_travelcardid", new EntityReference("cgi_travelcard", new Guid(_travelCardId)));
                //else
                //    incidentEntity.Attributes.Add("cgi_unregisterdtravelcard", request.CardNumber);      
            }

            if (!string.IsNullOrEmpty(request.WayOfTravel))
                incidentEntity.Attributes.Add("cgi_way_of_transport", request.WayOfTravel);

            if (!string.IsNullOrEmpty(request.Line))
            {
                incidentEntity.Attributes.Add("cgi_line", request.Line);
                descriptionText += String.Format("{0}Linje: {1}", Environment.NewLine, request.Line);
            }

            if (!string.IsNullOrEmpty(request.Train))
            {
                incidentEntity.Attributes.Add("cgi_train", request.Train);
                descriptionText += String.Format("{0}Linje: {1}", Environment.NewLine, request.Train);
            }

            if (!string.IsNullOrEmpty(request.County))
            {
                incidentEntity.Attributes.Add("cgi_county", request.County);
                descriptionText += String.Format("{0}Kommun: {1}", Environment.NewLine, request.County);
            }

            if (!string.IsNullOrEmpty(request.City))
            {
                incidentEntity.Attributes.Add("cgi_city", request.City);
                descriptionText += String.Format("{0}Stad: {1}", Environment.NewLine, request.City);
            }

            if (!string.IsNullOrEmpty(request.EmailAddress))
                incidentEntity.Attributes.Add("cgi_emailaddress", request.EmailAddress);

            if (request.ContactCustomer == true)
                incidentEntity.Attributes.Add("cgi_contactcustomer", true);
            else
                incidentEntity.Attributes.Add("cgi_contactcustomer", false);

            if (customer.EmailCount != null)
                incidentEntity.Attributes.Add("cgi_emailcount", customer.EmailCount.ToString());

            incidentEntity.Attributes.Add("description", descriptionText);

            Guid _g = _xrmManager.Create(incidentEntity);
            return _g;
        }

        private Guid _createIncidentRGOL(CreateAutoRGCaseRequest request, account customer)
        {
            Entity incidentEntity = new Entity("incident");
            incidentEntity.Attributes = new AttributeCollection();

            incidentEntity.Attributes.Add("caseorigincode", new OptionSetValue(285050007));
            incidentEntity.Attributes.Add("casetypecode", new OptionSetValue(285050003));
            incidentEntity.Attributes.Add("cgi_contactcustomer", false);

            if (!string.IsNullOrEmpty(request.CustomersCategory))
                incidentEntity.Attributes.Add("cgi_customers_category", request.CustomersCategory);

            if (!string.IsNullOrEmpty(request.CustomersSubcategory))
                incidentEntity.Attributes.Add("cgi_customers_subcategory", request.CustomersSubcategory);

            if (!string.IsNullOrEmpty(request.Title))
                incidentEntity.Attributes.Add("title", request.Title);

            if (!string.IsNullOrEmpty(request.Title))
                incidentEntity.Attributes.Add("description", request.Description);
            else
                incidentEntity.Attributes.Add("description", "Ej angivet");

            if (customer.AccountName == "CGI_DEFAULT")
            {
                incidentEntity.Attributes.Add("customerid", new EntityReference("account", customer.AccountId));
            }
            else
            {
                if ((CustomerType)request.CustomerType == CustomerType.Organisation)
                {
                    incidentEntity.Attributes.Add("cgi_accountid", new EntityReference("account", customer.AccountId));
                    incidentEntity.Attributes.Add("customerid", new EntityReference("account", customer.AccountId));
                }

                if ((CustomerType)request.CustomerType == CustomerType.Private)
                {
                    incidentEntity.Attributes.Add("cgi_contactid", new EntityReference("contact", customer.AccountId));
                    incidentEntity.Attributes.Add("customerid", new EntityReference("contact", customer.AccountId));
                }
            }

            if (!string.IsNullOrEmpty(request.CardNumber))
            {
                incidentEntity.Attributes.Add("cgi_travelcardno", request.CardNumber);
            }

            if (!string.IsNullOrEmpty(request.WayOfTravel))
            {
                incidentEntity.Attributes.Add("cgi_way_of_transport", request.WayOfTravel);
            }

            if (!string.IsNullOrEmpty(request.Line))
            {
                incidentEntity.Attributes.Add("cgi_line", request.Line);                
            }

            if (!string.IsNullOrEmpty(request.EmailAddress))
            {
                incidentEntity.Attributes.Add("cgi_emailaddress", request.EmailAddress);
            }

            if (!string.IsNullOrEmpty(request.MobileNo))
            {
                incidentEntity.Attributes.Add("cgi_customer_telephonenumber_mobile", request.MobileNo);
            }

            if (!string.IsNullOrEmpty(request.MobileNo))
            {
                incidentEntity.Attributes.Add("cgi_telephonenumber", request.MobileNo);
            }

            if (!string.IsNullOrEmpty(request.RGOLIssueID))
            {
                incidentEntity.Attributes.Add("cgi_rgolissueid", request.RGOLIssueID);
            }

            if (request.DepartureDateTime != null)
            {
                incidentEntity.Attributes.Add("cgi_departuredatetime", request.DepartureDateTime);
            }

            if (request.DepartureDateTime != null)
            {
                incidentEntity.Attributes.Add("cgi_actiondate", request.DepartureDateTime);
            }


            if (customer.EmailCount != null)
            {
                incidentEntity.Attributes.Add("cgi_emailcount", customer.EmailCount.ToString());
            }

            Guid _g = _xrmManager.Create(incidentEntity);

            return _g;
        }

       
        private Guid _createDecisionRGOL(decimal RefundAmount, RGOLSetting rgolsetting, incident incident, string currencyid, string InternalMessage, string CustomerMessage)
        {
            Guid _returnValue = Guid.Empty;

            Entity refundEntity = new Entity("cgi_refund");
            refundEntity.Attributes = new AttributeCollection();

            if (!string.IsNullOrEmpty(rgolsetting.RefundTypeId))
                refundEntity.Attributes.Add("cgi_refundtypeid", new EntityReference("cgi_refundtype", new Guid(rgolsetting.RefundTypeId)));

            if (!string.IsNullOrEmpty(rgolsetting.ReImBursementFormId))
                refundEntity.Attributes.Add("cgi_reimbursementformid", new EntityReference("cgi_reimbursementform", new Guid(rgolsetting.ReImBursementFormId)));

            if (!string.IsNullOrEmpty(incident.TravelCardNo))
                refundEntity.Attributes.Add("cgi_travelcard_number", incident.TravelCardNo);

            if (!string.IsNullOrEmpty(incident.AccountId))
                refundEntity.Attributes.Add("cgi_accountid", new EntityReference("account", new Guid(incident.AccountId)));

            if (!string.IsNullOrEmpty(incident.ContactId))
                refundEntity.Attributes.Add("cgi_contactid", new EntityReference("contact", new Guid(incident.ContactId)));

            if (!string.IsNullOrEmpty(incident.TicketNumber))
                refundEntity.Attributes.Add("cgi_refundnumber", incident.TicketNumber);

            if (!string.IsNullOrEmpty(InternalMessage))
                refundEntity.Attributes.Add("cgi_comments", InternalMessage);
            
            if (!string.IsNullOrEmpty(CustomerMessage))
                refundEntity.Attributes.Add("cgi_customermessage", CustomerMessage);

            refundEntity.Attributes.Add("cgi_isautogenerated", true);

            refundEntity.Attributes.Add("cgi_caseid", new EntityReference("incident", incident.IncidentId));

            if (!string.IsNullOrEmpty(incident.MobileNumber))
                refundEntity.Attributes.Add("cgi_mobilenumber", incident.MobileNumber);

            if (RefundAmount > 0)
            {
                Money _money = new Money();
                _money.Value = RefundAmount;
                refundEntity.Attributes.Add("cgi_amount", _money);
            }

            if (!string.IsNullOrEmpty(currencyid))
                refundEntity.Attributes.Add("transactioncurrencyid", new EntityReference("transactioncurrency", new Guid(currencyid)));

            refundEntity.Attributes.Add("cgi_attestation", new OptionSetValue(285050004));

            _returnValue = _xrmManager.Create(refundEntity);

            return _returnValue;
        }

        private void _createAnnotation(document document, Guid incidentid)
        {
            string encodedData = System.Convert.ToBase64String(document.DocumentBody);

            Entity Annotation = new Entity("annotation");
            Annotation.Attributes["objectid"] = new EntityReference("incident", incidentid);
            Annotation.Attributes["objecttypecode"] = "incident";
            Annotation.Attributes["subject"] = document.Subject;
            Annotation.Attributes["documentbody"] = encodedData;
            Annotation.Attributes["mimetype"] = @"text/plain";
            Annotation.Attributes["notetext"] = document.NoteText;
            Annotation.Attributes["filename"] = document.FileName;
            Guid _g = _xrmManager.Create(Annotation);
        }

        private Guid _createCustomer(string FirstName, string LastName, string EmailAddress, string mobilephonenumber)
        {
            Entity customerEntity = new Entity("contact");
            customerEntity.Attributes = new AttributeCollection();

            if (!string.IsNullOrEmpty(FirstName))
                customerEntity.Attributes.Add("firstname", FirstName);

            if (!string.IsNullOrEmpty(LastName))
                customerEntity.Attributes.Add("lastname", LastName);

            if (!string.IsNullOrEmpty(EmailAddress))
                customerEntity.Attributes.Add("emailaddress1", EmailAddress);

            if (!string.IsNullOrEmpty(mobilephonenumber))
                customerEntity.Attributes.Add("telephone2", mobilephonenumber);

            Guid _g = _xrmManager.Create(customerEntity);
            return _g;
        }

        private Guid _createCustomerRGOL(CreateAutoRGCaseRequest request)
        {
            Entity customerEntity = new Entity("contact");
            customerEntity.Attributes = new AttributeCollection();

            if (!string.IsNullOrEmpty(request.FirstName))
                customerEntity.Attributes.Add("firstname", request.FirstName);

            if (!string.IsNullOrEmpty(request.LastName))
                customerEntity.Attributes.Add("lastname", request.LastName);

            if (!string.IsNullOrEmpty(request.EmailAddress))
                customerEntity.Attributes.Add("emailaddress1", request.EmailAddress);
            
            Guid _g = _xrmManager.Create(customerEntity);
            return _g;
        }

        // Entities

        private int _getSendToQueue(string customercategory, string custumersubcategory)
        {
            // 285,050,006  Kundtjänst 1 Linjen
            int _returnValue = 285050000;

            if (customercategory == "Synpunkter trafikinformation" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                _returnValue = 285050000;
            else if (customercategory == "Tankar och önskemål om ny trafik" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                _returnValue = 285050000;
            else if (customercategory == "Skador och vandalisering" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                _returnValue = 285050000;
            else if (customercategory == "Min resa" && custumersubcategory == "Regionbuss")
                _returnValue = 285050000;
            else if (customercategory == "Min resa" && custumersubcategory == "Stadsbuss")
                _returnValue = 285050000;
            else if (customercategory == "Min resa" && custumersubcategory == "Tåg")
                _returnValue = 285050000;
            else if (customercategory == "Övrigt" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                _returnValue = 285050000;
            else if (customercategory == "Överklaga kontroll avgift" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                _returnValue = 285050008;
            else if (customercategory == "JoJo kort och priser" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                _returnValue = 285050004;
            else if (customercategory == "Min resa" && custumersubcategory == "Närtrafik")
                _returnValue = 285050001;
            else if (customercategory == "Min resa" && custumersubcategory == "Färdtjänst/sjukresa")
                _returnValue = 285050001;
            else if (customercategory == "Boka gruppresa" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                _returnValue = 285050006;
            else if (customercategory == "Boka ledsagning" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                _returnValue = 285050006;
            else if (customercategory == "Kundtjänst Serviceresor" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                _returnValue = 285050001;
            else if ((customercategory == "" || string.IsNullOrEmpty(customercategory)) && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                _returnValue = 285050000;
            
            return _returnValue;
        }

        private basecurrency _getCurrency()
        {
            basecurrency _returnValue = null;
            
            try
            {
                SqlConnection sqlCon = OpenSQL();
                BaseCurrencySqlList currencies = null;
                using (SqlCommand command = new SqlCommand("sp_GetBaseCurrency", sqlCon))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    XmlReader reader = command.ExecuteXmlReader();
                    if (reader != null)
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(BaseCurrencySqlList));
                        currencies = ser.Deserialize(reader) as BaseCurrencySqlList;
                    }
                    reader.Close();
                    CloseSQL(sqlCon);

                    if (currencies != null && currencies.BaseCurrencies != null && currencies.BaseCurrencies.Count() > 0)
                    {
                        _returnValue = currencies.BaseCurrencies[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _returnValue;
        }

        private void _createLogfile(CreateCaseRequest request)
        { 
            try
            {
                if (request != null)
                {
                    StreamWriter _sw = new StreamWriter("C:\\Temp\\createcase.log", true);
                    _sw.WriteLine("======================================================================================================================");
                    _sw.WriteLine(DateTime.Now.ToString());

                    if (!string.IsNullOrEmpty(request.CustomersCategory))
                        _sw.WriteLine("CustomersCategory : " + request.CustomersCategory);
                    else
                        _sw.WriteLine("CustomersCategory is missing");

                    if (!string.IsNullOrEmpty(request.CustomersSubcategory))
                        _sw.WriteLine("CustomersSubcategory : " + request.CustomersSubcategory);
                    else
                        _sw.WriteLine("CustomersSubcategory is missing");

                    if (!string.IsNullOrEmpty(request.Title))
                        _sw.WriteLine("Title : " + request.Title);
                    else
                        _sw.WriteLine("Title is missing");

                    if (!string.IsNullOrEmpty(request.Description))
                        _sw.WriteLine("Description : " + request.Description);
                    else
                        _sw.WriteLine("Description is missing");

                    if (request.Customer != null && request.Customer != Guid.Empty)
                        _sw.WriteLine("Customer : " + request.Customer.ToString());
                    else
                        _sw.WriteLine("Customer is missing");

                    if (request.CustomerType != null && request.CustomerType == CustomerType.Private)
                        _sw.WriteLine("Customer : Private");
                    else if (request.CustomerType != null && request.CustomerType == CustomerType.Organisation)
                        _sw.WriteLine("Customer : Organisation");
                    else
                        _sw.WriteLine("Customer is missing");

                    if (!string.IsNullOrEmpty(request.InvoiceNumber))
                        _sw.WriteLine("InvoiceNumber : " + request.InvoiceNumber);
                    else
                        _sw.WriteLine("InvoiceNumber is missing");

                    if (!string.IsNullOrEmpty(request.ControlFeeNumber))
                        _sw.WriteLine("ControlFeeNumber : " + request.ControlFeeNumber);
                    else
                        _sw.WriteLine("ControlFeeNumber is missing");

                    if (!string.IsNullOrEmpty(request.CardNumber))
                        _sw.WriteLine("CardNumber : " + request.CardNumber);
                    else
                        _sw.WriteLine("CardNumber is missing");

                    if (!string.IsNullOrEmpty(request.WayOfTravel))
                        _sw.WriteLine("WayOfTravel : " + request.WayOfTravel);
                    else
                        _sw.WriteLine("WayOfTravel is missing");

                    if (!string.IsNullOrEmpty(request.Line))
                        _sw.WriteLine("Line : " + request.Line);
                    else
                        _sw.WriteLine("Line is missing");

                    if (!string.IsNullOrEmpty(request.City))
                        _sw.WriteLine("City : " + request.City);
                    else
                        _sw.WriteLine("City is missing");

                    if (!string.IsNullOrEmpty(request.Train))
                        _sw.WriteLine("Train : " + request.Train);
                    else
                        _sw.WriteLine("Train is missing");

                    if (!string.IsNullOrEmpty(request.County))
                        _sw.WriteLine("County : " + request.County);
                    else
                        _sw.WriteLine("County is missing");

                    if (!string.IsNullOrEmpty(request.FirstName))
                        _sw.WriteLine("FirstName : " + request.FirstName);
                    else
                        _sw.WriteLine("FirstName is missing");

                    if (!string.IsNullOrEmpty(request.LastName))
                        _sw.WriteLine("LastName : " + request.LastName);
                    else
                        _sw.WriteLine("LastName is missing");

                    if (!string.IsNullOrEmpty(request.EmailAddress))
                        _sw.WriteLine("EmailAddress : " + request.EmailAddress);
                    else
                        _sw.WriteLine("EmailAddress is missing");

                    if (request.ContactCustomer != null && request.ContactCustomer == true)
                        _sw.WriteLine("ContactCustomer : true");
                    else if (request.ContactCustomer != null && request.ContactCustomer == false)
                        _sw.WriteLine("ContactCustomer : false");
                    else
                        _sw.WriteLine("ContactCustomer is missing");

                    
                    _sw.Flush();
                    _sw.Close();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}