using ASBExt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net.Configuration;
using System.Net;

namespace ASBCustom
{

    /// <summary>
    /// Summary description for customwebservice
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CustomWebservice : System.Web.Services.WebService
    {
        Util.Util utilObj = new Util.Util();
        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        LogFile.Log logobj = new LogFile.Log();

        //This method is dummy method for the reference
        [WebMethod(EnableSession = true)]
        public string CustomFunction()
        {
            string result = string.Empty;
            result = Session["project"].ToString();
            return result;
        }
    }
}
