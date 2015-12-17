using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppIIS
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // Response.Write("<script>alert(1);</script>");

            if (Page.IsPostBack)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ScriptDescription", "$(document).ready(function(){ alert('Login Failed. Please try again " + DateTime.Now.ToString() + "'); });", true);
            }
        }
    }
}
