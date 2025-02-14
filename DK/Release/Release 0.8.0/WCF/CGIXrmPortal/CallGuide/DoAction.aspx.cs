﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CGIXrmHandler;
using System.Configuration;
using System.Text;
using System.IO;

public partial class doaction : System.Web.UI.Page
{
    CGIXrmLogger.LogToCrm log2Crm = new CGIXrmLogger.LogToCrm();
    protected void Page_Load(object sender, EventArgs e)
    {
        //Call:CallGuide/DoAction.aspx?contactid=040123456&queuetime=15&aphonenumber=040 123456&bphonenumber=0771-777777&errand=909&screenpopchoice=&contactsourcetype=ivr&cid=&agentname=reine&contacttime=
        //Chat:CallGuide/DoAction.aspx?contactid=040123450&queuetime=15&aphonenumber=&bphonenumber=&errand=702&screenpopchoice=&contactsourcetype=chat&cid=&agentname=reine&contacttime=
        //Response.Write("Enter Application");
        if (!IsPostBack)
        {
           
            if(Request.QueryString.HasKeys())
            {
                LogURL("C:\\Temp\\CallGuide.txt", Request.RawUrl);
                LogURL("C:\\Temp\\CallGuide.txt", Server.HtmlEncode(Request.RawUrl));
                
                //Response.Write("Has QueryString");
                CallGuideRequest callguideRequest = new CallGuideRequest();
                callguideRequest.CallGuideSessionId= string.IsNullOrEmpty(Request.QueryString["contactid"]) ? string.Empty : Request.QueryString["contactid"];
                callguideRequest.QueueTime= string.IsNullOrEmpty(Request.QueryString["queuetime"]) ? string.Empty : Request.QueryString["queuetime"];
                callguideRequest.APhoneNumber=string.IsNullOrEmpty(Request.QueryString["aphonenumber"]) ? string.Empty : Request.QueryString["aphonenumber"];
                callguideRequest.BPhoneNumber=string.IsNullOrEmpty(Request.QueryString["bphonenumber"]) ? string.Empty : Request.QueryString["bphonenumber"];
                callguideRequest.ErrandTaskType=string.IsNullOrEmpty(Request.QueryString["errand"]) ? string.Empty : Request.QueryString["errand"];
                callguideRequest.ScreenPopChoice=string.IsNullOrEmpty(Request.QueryString["screenpopchoice"]) ? string.Empty : Request.QueryString["screenpopchoice"];
                callguideRequest.ContactSourceType = string.IsNullOrEmpty(Request.QueryString["contactsourcetype"]) ? string.Empty : Request.QueryString["contactsourcetype"]; // TODO .ToUpperInvariant()? its used in several places after this ( actually lower, but you should use upper )
                callguideRequest.CId=string.IsNullOrEmpty(Request.QueryString["cid"]) ? string.Empty : Request.QueryString["cid"];
                callguideRequest.AgentName=string.IsNullOrEmpty(Request.QueryString["agentname"]) ? string.Empty : Request.QueryString["agentname"];
                callguideRequest.CallDuration=string.IsNullOrEmpty(Request.QueryString["contacttime"]) ? string.Empty : Request.QueryString["contacttime"];
                callguideRequest.ChatCustomerAlias = string.IsNullOrEmpty(Request.QueryString["chatcustomeralias"]) ? string.Empty : Request.QueryString["chatcustomeralias"];

                Guid callerId = Guid.Empty;

                if (!string.IsNullOrEmpty(Request.QueryString["agentid"]))
                {
                    Guid.TryParse(Request.QueryString["agentid"],out callerId); // refactor to agentId?
                }
                
                if(IsValidRequest(callguideRequest))
                    DoAction(callguideRequest, callerId);
            }
            
        }
    
    }

    public void LogURL(string path, string message)
    {
        try
        {
            StreamWriter _sw = new StreamWriter(path, true, System.Text.Encoding.UTF8);
            string _message = string.Format("{0} : {1}", DateTime.Now, message);
            _sw.WriteLine(_message);
            _sw.Flush();
            _sw.Close();
        }
        catch
        {
            throw;
        }
    }

    private bool IsValidRequest(CallGuideRequest callguideRequest)
    {
        
        if (string.IsNullOrEmpty(callguideRequest.CallGuideSessionId))
            return false;
        
        switch (callguideRequest.ContactSourceType.ToLowerInvariant())
        {
            case "ivr":
                //If callerid is hidden let it pass. this is handled later.
                //if (string.IsNullOrEmpty(callguideRequest.APhoneNumber))
                //    return false;
                break;
            case "chat":
                break;
            case "emailserver":
                break;
            default:
                break;


        }
        return true;
    }

    private void DoAction(CallGuideRequest callguideRequest,Guid callerId)
    {
        try
        {
            log2Crm.Trace("Entering Do Action Method", "DoAction", "CGIXrmPortal");

            CallGuideHandler callguideHandler = new CallGuideHandler(callerId, (Settings)Application["XrmSettings"]);

            if (callerId == Guid.Empty)
            {
                callerId = callguideHandler.GetAgentUserId(callguideRequest.AgentName);
                log2Crm.Trace("Processed callerid since callerid supplied was empty", "DoAction", "CGIXrmPortal");

            }


            if (callguideRequest.ScreenPopChoice.ToLowerInvariant() != "endofcontact")
            {
                Guid crmRecordid = Guid.Empty;

                if (callerId != Guid.Empty)
                {
                    switch (callguideRequest.ContactSourceType.ToLowerInvariant())
                    {
                        case "ivr":
                            callguideRequest.CallDirection = CallDirection.Incoming;
                            log2Crm.Trace("IVR Request", "DoAction", "CGIXrmPortal");
                            //if(!callguideHandler.IsDuplicateRequest(callguideRequest))
                            //{
                            //callguideRequest.CallRouteAction = callguideHandler.GetCallRouteAction(callguideRequest.BPhoneNumber);
                            if (callguideRequest.ScreenPopChoice.ToLowerInvariant() == "manual")
                            {
                                callguideRequest.CallRouteAction = CallGuideRouteAction.Case;
                                crmRecordid = callguideHandler.ExecuteManualRequest(callguideRequest, callerId);
                            }
                            //else if (callguideRequest.ScreenPopChoice.ToLowerInvariant() == "afternormalaccept")
                            //{
                            //    //callguideRequest.CallRouteAction = CallGuideRouteAction.Activity;
                            //    //crmRecordid = callguideHandler.ExecuteNewCallRequest(callguideRequest, callerId);
                            //    //Response.ClearContent();
                            //    Response.Write("");
                            //    return;
                            //}
                            else
                            {
                                //callguideRequest.CallRouteAction = CallGuideRouteAction.Activity;
                                //crmRecordid = callguideHandler.ExecuteNewCallRequest(callguideRequest, callerId);
                                //Response.ClearContent();
                                Response.Write("Invalid ScreenPopChoice : " + callguideRequest.ScreenPopChoice);
                                return;
                            }
                            //}
                            break;

                        case "chat":
                            log2Crm.Trace("Chart Request", "DoAction", "CGIXrmPortal");
                            if (callguideRequest.ScreenPopChoice.ToLowerInvariant() == "manual")
                            {
                                callguideRequest.CallDirection = CallDirection.Incoming;
                                callguideRequest.CallRouteAction = CallGuideRouteAction.Chat;
                                //crmRecordid = callguideHandler.ExecuteChatRequest(callguideRequest, callerId);
                                crmRecordid = callguideHandler.ExecuteManualRequest(callguideRequest, callerId);
                            }
                            else
                            {
                                //callguideRequest.CallRouteAction = CallGuideRouteAction.Activity;
                                //crmRecordid = callguideHandler.ExecuteNewCallRequest(callguideRequest, callerId);
                                //Response.ClearContent();
                                Response.Write("Invalid ScreenPopChoice : " + callguideRequest.ScreenPopChoice);
                                return;
                            }
                            break;
                        case "emailserver":
                            log2Crm.Trace("Email Request", "DoAction", "CGIXrmPortal");
                            if (callguideRequest.ScreenPopChoice.ToLowerInvariant() == "manual")
                            {
                                callguideRequest.CallDirection = CallDirection.Incoming;
                                callguideRequest.CallRouteAction = CallGuideRouteAction.FaceBook;
                                //crmRecordid = callguideHandler.ExecuteFBRequest(callguideRequest, callerId);
                                crmRecordid = callguideHandler.ExecuteManualRequest(callguideRequest, callerId);
                            }
                            else
                            {
                                //callguideRequest.CallRouteAction = CallGuideRouteAction.Activity;
                                //crmRecordid = callguideHandler.ExecuteNewCallRequest(callguideRequest, callerId);
                                //Response.ClearContent();
                                Response.Write("Invalid ScreenPopChoice : " + callguideRequest.ScreenPopChoice);
                                return;
                            }
                            break;
                        default:
                            break;


                    }

                    //Response.Write("Process Completed");
                    if (crmRecordid != Guid.Empty)
                    {
                        Uri responseUrl = GenerateActionUrl(crmRecordid, callguideRequest.CallRouteAction);
                        Response.ClearContent();
                        Response.Write(responseUrl);
                    }
                    else
                    {
                        Response.ClearContent();
                        Response.Write("");
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(callguideRequest.CallDuration))
                {
                    if (callguideHandler.UpdateDuration(callguideRequest, callerId))
                    {
                        Response.Write(string.Empty);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            
            log2Crm.Exception("Error Processing Request with Error Message :"+ex.Message,"DoAction",ex, "CGIXrmPortal");
        }



    }
   
    private Uri GenerateActionUrl(Guid recordId,CallGuideRouteAction callRouteAction)
    {
        string baseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
        string objectTypeCode = string.Empty;
        if(!string.IsNullOrEmpty(baseUrl))
        {
            switch (callRouteAction)
            {
                case CallGuideRouteAction.Chat:
                    //objectTypeCode = ConfigurationManager.AppSettings["Chat"].ToString();
                    objectTypeCode = "112";
                    break;
                case CallGuideRouteAction.FaceBook:
                    //objectTypeCode = ConfigurationManager.AppSettings["FaceBook"].ToString();
                    objectTypeCode = "112";
                    break;
                case CallGuideRouteAction.Activity:
                    objectTypeCode = "4201";
                    break;
                case CallGuideRouteAction.Case:
                    objectTypeCode = "112";
                    break;
                case CallGuideRouteAction.Account:
                    objectTypeCode = "1";
                    break;
                default:
            
                break;
	        }
            return new Uri(string.Format(baseUrl,objectTypeCode,recordId));
        }
        
            
        else
            throw new Exception("No Matching URL found for Action") ;
            

    }
}