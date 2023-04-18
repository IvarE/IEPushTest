using System;
using System.Linq;
using System.Web;
using CGIXrmHandler;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.Collections.ObjectModel;
using CGI.CRM2013.Skanetrafiken.CGIXrmLogger;
using CGIXrmHandler.CrmClasses;
using CGIXrmWin;

public partial class PASS_DoAction : System.Web.UI.Page
{
    /* Debug data.
    //?sFN=X 500&sLN=Testsson&sA=Kvarnstensg 8 &sPA=25227 Helsingborg&sPH=&sPW=&sPF=&sEM=&sSSN=5202023030&sPR=Helsingborgs kommun&sOP1=stmbcnv&sOP2=multibcbsa&sBid=9693322&iDC=1&sTTJ=FT&sTCN=Taxi Kurir i Malmö AB&iTCID=107&iTLID=0&iTJID=0&iTFID=0&sTFN=   Kvarnstensg 8  25227 Helsingborg&sTFD=20140919&sTFT=0950&iTTID=0&sTTN= Helsingborgs Stadsteater Karl Johans g 1  25221 Helsingborg&sTTD=20140919&sTTT=0958&sTCID=21012P
    //?sFN=X 500&sLN=Testsson&sA=Kvarnstensg 8 &sPA=25227 Helsingborg&sPH=&sPW=&sPF=&sEM=&sSSN=5202023030&sPR=Skånetrafiken&sOP1=stmbcnv&sOP2=multibcbsa&sBid=9693458&iDC=2&sTTJ=NÄR&sTCN=Maximilians Transporter | Josef 1&iTCID=82&iTLID=0 | 1000&iTJID=0 | 1000&iTFID=0 | 1000&sTFN= Lund Bankgatan [hpl] Lilla Tvärg 26  22352 Lund | Josef 3&sTFD=20140919 | Josef 4&sTFT=1719 | Josef 5&iTTID=0 | 1000&sTTN= V Hoby k:a [hpl] Västra Hoby 172  22591 Lund | Josef 6&sTTD=20140919 | Josef 7&sTTT=1740 | Josef 8&sTCID=3005P Lu | Josef 2
    //?sFN=SR r&sPA=25227 Helsiäängborg&sEM=agneta.h@bredband.net&sPH=Testar&sPW=&iDC=3&sTFN=||tätt&sTTT=jo|sef|&sTTD=sef|jo|ttt&sTCID=fes|jo|ttt
    //?sFN=X 500&sLN=Testsson&sA=Kvarnstensg 8 &sPA=25227 Helsingborg&sPH=&sPW=&sPF=&sEM=&sSSN=345&sPR=Skånetrafiken&sOP1=stmbcnv&sOP2=multibcbsa&sBid=9693453&iDC=1&sTTJ=NÄR&sTCN=Maximilians Transporter&iTCID=82&iTLID=0&iTJID=0&iTFID=0&sTFN= V Hoby k:a [hpl] Västra Hoby 172  22591 Lund&sTFD=20140919&sTFT=0819&iTTID=0&sTTN= Lund Bankgatan [hpl] Lilla Tvärg 26  22352 Lund&sTTD=20140919&sTTT=0840&sTCID=51001P

    Sekund-Acc
    http://localhost:4004/Pass/DoAction.aspx?sFN=Fyran&sLN=Fyrsson&sA=Fyrgatan 44 &sPA=44444 Fyran&sPH=123&sPW=0701556677&sPM=0555555555&sPF=789&sEM=fyran@mailinator.com&sSSN=20040404&sPR=Fyrans kommun&sOP1=stmbcnv&sOP2=9693292&iDC=1&sTTJ=FT&iBID=5555&sTCN=Taxi Fyrir i Malmö AB&iTCID=107&iTLID=89&iTJID=2323&sTRN=676767&sTFN=Fyrgatan 44 444444 Fyran&sTFD=20140919&sTFT=1559&iTTID=0&sTTN=Grand Hotel i Malmö 11 55555 Mölle    

    Kund med sekretesskydd
    http://sekund.skanetrafiken.se:4004/PASS/DoAction.aspx?sFN=Andrea&sLN=Breitholtz&sA=SEKRETESSKYDD%20%20&sPA=%20&sPH=&sPW=&sPF=&sEM=&sSSN=196810267523&sPR=&sOP1=&sOP2=&iDC=0 
    */

    private PASSHandler _passHandler;

    readonly LogToCrm _log2Crm = new LogToCrm();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.HasKeys())
            {
                _passHandler = new PASSHandler();
                _passHandler.LogMessage("C:\\Temp\\PASSLog.txt", "***********************************************************************************************");
                //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Start");

                foreach (String key in Request.QueryString.AllKeys)
                {
                    if (key == "sFN" || key == "sLN" || key == "sPR")
                    {
                        //Only logg Firstname, Lastname and sPR
                        _passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Key: " + key + " Value: " + Request.QueryString[key]);
                    }
                }

                CGIXrmHandler.CrmClasses.Incident incident = new CGIXrmHandler.CrmClasses.Incident
                {
                    sFN = string.IsNullOrEmpty(Request.QueryString["sFN"]) ? null : Request.QueryString["sFN"],             // Förnamn
                    sLN = string.IsNullOrEmpty(Request.QueryString["sLN"]) ? null : Request.QueryString["sLN"],             // Efternamn
                    sA = string.IsNullOrEmpty(Request.QueryString["sA"]) ? null : Request.QueryString["sA"],                // Adress1
                    sPA = string.IsNullOrEmpty(Request.QueryString["sPA"]) ? null : Request.QueryString["sPA"],             // Postnummer och Ort
                    sPH = string.IsNullOrEmpty(Request.QueryString["sPH"]) ? null : Request.QueryString["sPH"],
                    sPW = string.IsNullOrEmpty(Request.QueryString["sPW"]) ? null : Request.QueryString["sPW"],             // Telefonnr
                    sPM = string.IsNullOrEmpty(Request.QueryString["sPM"]) ? null : Request.QueryString["sPM"],             // Mobilnr
                    sPF = string.IsNullOrEmpty(Request.QueryString["sPF"]) ? null : Request.QueryString["sPF"],
                    sEM = string.IsNullOrEmpty(Request.QueryString["sEM"]) ? string.Empty : Request.QueryString["sEM"],     // Epostadress
                    sSSN = string.IsNullOrEmpty(Request.QueryString["sSSN"]) ? string.Empty : Request.QueryString["sSSN"],   // Personnr
                    sPR = string.IsNullOrEmpty(Request.QueryString["sPR"]) ? null : Request.QueryString["sPR"],
                    sOP1 = string.IsNullOrEmpty(Request.QueryString["sOP1"]) ? null : Request.QueryString["sOP1"],
                    sOP2 = string.IsNullOrEmpty(Request.QueryString["sOP2"]) ? null : Request.QueryString["sOP2"],
                    sTTJ = string.IsNullOrEmpty(Request.QueryString["sTTJ"]) ? null : Request.QueryString["sTTJ"],
                    iBID =
                        string.IsNullOrEmpty(Request.QueryString["iBID"])
                            ? null
                            : (int?)Int32.Parse(Request.QueryString["iBID"]),
                    iDC =
                        string.IsNullOrEmpty(Request.QueryString["iDC"])
                            ? null
                            : (int?)Int32.Parse(Request.QueryString["iDC"]),
                    circulationNameInPass1 = TryGetQueryStringPart(Request, "sTRN", 0),
                    circulationNameInPass2 = TryGetQueryStringPart(Request, "sTRN", 1),
                    operatorPass1 = TryGetQueryStringPart(Request, "sTCN", 0),
                    operatorPass2 = TryGetQueryStringPart(Request, "sTCN", 1)
                };

                // AJ: Henrics code, uncomment when changeset 3716 should be in production


                //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Find customer");

                ObservableCollection<Contact> contacts = _passHandler.FetchContacts(incident);     //(incident.sSSN, incident.sEM);

                if (contacts.Count == 1)
                {
                    //CGIXrmHandler.CrmClasses.Contact contact = contacts.FirstOrDefault();
                    CGIXrmHandler.CrmClasses.Contact contact = contacts[0];
                    //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Found 1 customer : " + contact.ContactId);
                    incident.DefaultCustomer = new EntityReference(contact.LogicalName, contact.ContactId);
                    incident.Contact = new EntityReference(contact.LogicalName, contact.ContactId);
                }
                else
                {
                    //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Anonymous customer");
                    EntityReference anonymousCustomer = _passHandler.GetAnonymousCustomer();
                    if (anonymousCustomer != null)
                        incident.DefaultCustomer = anonymousCustomer;
                    else
                        throw new Exception("Could not find customer!");
                }

                PASSTravelInformation[] passTravelInformations = new PASSTravelInformation[(int)incident.iDC];
                PASSTravelInformation passTravelInformation = new PASSTravelInformation();

                //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Find travelinformation");

                if (incident.iDC >= 1)
                {
                    for (int i = 0; i < incident.iDC; i++)
                    {
                        passTravelInformation.PASSTravelInformationName = string.Format("{0} : {1}", "PASS", DateTime.Now.ToShortDateString());
                        passTravelInformation.sTCN = string.IsNullOrEmpty(Request.QueryString["sTCN"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["sTCN"].Split('|')[i]) ? null
                                : Request.QueryString["sTCN"].Split('|')[i];
                        passTravelInformation.sTCID = string.IsNullOrEmpty(Request.QueryString["sTCID"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["sTCID"].Split('|')[i]) ? null
                                : Request.QueryString["sTCID"].Split('|')[i];
                        passTravelInformation.iTLID = string.IsNullOrEmpty(Request.QueryString["iTLID"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["iTLID"].Split('|')[i]) ? null
                                : (int?)Int32.Parse(Request.QueryString["iTLID"].Split('|')[i]);
                        passTravelInformation.sTLN = string.IsNullOrEmpty(Request.QueryString["sTLN"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["sTLN"].Split('|')[i]) ? null
                                : Request.QueryString["sTLN"].Split('|')[i];
                        passTravelInformation.sTRN = string.IsNullOrEmpty(Request.QueryString["sTRN"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["sTRN"].Split('|')[i]) ? null
                                : Request.QueryString["sTRN"].Split('|')[i];
                        passTravelInformation.iTJID = string.IsNullOrEmpty(Request.QueryString["iTJID"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["iTJID"].Split('|')[i]) ? null
                                : (int?)Int32.Parse(Request.QueryString["iTJID"].Split('|')[i]);
                        passTravelInformation.iTFID = string.IsNullOrEmpty(Request.QueryString["iTFID"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["iTFID"].Split('|')[i]) ? null
                                : (int?)Int32.Parse(Request.QueryString["iTFID"].Split('|')[i]);
                        passTravelInformation.sTFN = string.IsNullOrEmpty(Request.QueryString["sTFN"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["sTFN"].Split('|')[i]) ? null
                                : Request.QueryString["sTFN"].Split('|')[i];
                        passTravelInformation.sTFD = string.IsNullOrEmpty(Request.QueryString["sTFD"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["sTFD"].Split('|')[i]) ? null
                                : Request.QueryString["sTFD"].Split('|')[i];
                        passTravelInformation.sTFT = string.IsNullOrEmpty(Request.QueryString["sTFT"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["sTFT"].Split('|')[i]) ? null
                                : Request.QueryString["sTFT"].Split('|')[i];
                        passTravelInformation.iTTID = string.IsNullOrEmpty(Request.QueryString["iTTID"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["iTTID"].Split('|')[i]) ? null
                                : (int?)Int32.Parse(Request.QueryString["iTTID"].Split('|')[i]);
                        passTravelInformation.sTTN = string.IsNullOrEmpty(Request.QueryString["sTTN"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["sTTN"].Split('|')[i]) ? null
                                : Request.QueryString["sTTN"].Split('|')[i];
                        passTravelInformation.sTTD = string.IsNullOrEmpty(Request.QueryString["sTTD"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["sTTD"].Split('|')[i]) ? null
                                : Request.QueryString["sTTD"].Split('|')[i];
                        passTravelInformation.sTTT = string.IsNullOrEmpty(Request.QueryString["sTTT"]) ? null
                            : string.IsNullOrEmpty(Request.QueryString["sTTT"].Split('|')[i]) ? null
                                : Request.QueryString["sTTT"].Split('|')[i];
                        passTravelInformations[i] = passTravelInformation;
                    }
                }

                //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Enter DoAction");
                DoAction(incident, passTravelInformations);

            }
        }
    }

    // AJ: Henrics code, uncomment when changeset 3716 should be in production
    // Removed some duplicate code, added some guardcode and logic, added try catch in case it doesnt work.
    private string TryGetQueryStringPart(HttpRequest request, string queryStringPart, int i)
    {
        try
        {
            string queryStringPartValue = Request.QueryString[queryStringPart];
            if (string.IsNullOrEmpty(queryStringPartValue))
            {
                return null;
            }
            string[] queryStringPartValueSplittedValues = queryStringPartValue.Split('|');
            if (queryStringPartValueSplittedValues.Length > i)
            {
                string queryStringPartSplittedValue = queryStringPartValueSplittedValues[i];
                return string.IsNullOrEmpty(queryStringPartSplittedValue) ? null : queryStringPartSplittedValue;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
    }

    private void DoAction(CGIXrmHandler.CrmClasses.Incident incident, PASSTravelInformation[] passTravelInformations)
    {
        try
        {
            //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Logtrace");
            _log2Crm.Trace("Entering Do Action Method", "PASS.DoAction", "CGIXrmPortal");
            //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Enter ExecutePASSRequest");
            Guid guid = _passHandler.ExecutePASSRequest(incident, passTravelInformations);
            //string URL = "http://v-dkcrm-utv/Skanetrafiken/main.aspx?etc=112&extraqs=&id=%7b{0}%7d&newWindow=true&pagetype=entityrecord";
            //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Redirect to guid = " + guid);
            Response.Redirect(GenerateUrl(guid));
            //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Stop");
            //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "***********************************************************************************************");
        }
        catch (Exception ex)
        {
            _log2Crm.Exception("Error Processing Request with Error Message :" + ex.Message, "PASS.DoAction", ex, "CGIXrmPortal");
        }
    }

    private string GenerateUrl(Guid recordId)
    {
        //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Find BaseUrl");
        string baseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
        //_passHandler.LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "BaseUrl = " + baseUrl);
        string objectTypeCode = "112";
        if (!string.IsNullOrEmpty(baseUrl))
            return string.Format(baseUrl, objectTypeCode, recordId);
        else
            throw new Exception("No Matching URL found!");
    }

}