using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PASS_Test : System.Web.UI.Page
{

    //?sTest=Kalle Kula
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.HasKeys())
            {
                string _test = string.IsNullOrEmpty(Request.QueryString["sTest"]) ? "<missing>" : Request.QueryString["sTest"];
                txtInfo.Text = _test;

                txtUrl.Text = Request.RawUrl.ToString();



            }
        }
    }
}