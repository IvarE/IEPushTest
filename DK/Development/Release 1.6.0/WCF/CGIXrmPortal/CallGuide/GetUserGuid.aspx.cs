using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CGIXrmHandler;
using CGIXrmHandler.CrmClasses;

public partial class GetUserGuid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.HasKeys())
            {
                if (!string.IsNullOrEmpty(Request.QueryString["agentname"]))
                    GetAgentUserGuid(Request.QueryString["agentname"]);
            }
        }
    }
    private void GetAgentUserGuid(string agentName)
    {
        CallGuideHandler callguideHandler = new CallGuideHandler(Guid.Empty, (Settings)Application["XrmSettings"]);
        Guid callerId = callguideHandler.GetAgentUserId(agentName);
        Response.Write(callerId);
    }
}