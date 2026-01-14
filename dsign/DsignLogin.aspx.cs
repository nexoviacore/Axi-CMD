using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class aspx_DsignLogin : System.Web.UI.Page
{

    public StringBuilder strParams = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Request.QueryString["rn"] != null)
        {
            strParams.Append("<input type=hidden name=rn value=" + Request.QueryString["rn"].ToString() + ">");
         }
        if (Request.QueryString["hash"] != null)
        {
            strParams.Append("<input type=hidden name=hash value=" + Request.QueryString["hash"].ToString() + ">");
        }
        if (Request.QueryString["rndh"] != null)
        {
            strParams.Append("<input type=hidden name=rndh value=" + Request.QueryString["rndh"].ToString() + ">");
        }
         if (Request.QueryString["dgiview"] != null)
        {
            strParams.Append("<input type=hidden name=dgiview value=" + Request.QueryString["dgiview"].ToString() + ">");
        }

    }
}