using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Axpert_Object;
using System.Xml;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace Util
{
    /// <summary>
    /// Summary description for CommonUtils
    /// </summary>
    public class CommonUtils
    {
        public string HomeUrl = string.Empty;
        public string cloudFilePath = string.Empty;
        AxpertDotNet ads = new AxpertDotNet();
        public CommonUtils()
        {          
            HomeUrl = System.Configuration.ConfigurationManager.AppSettings["url"].ToString();
            cloudFilePath = System.Configuration.ConfigurationManager.AppSettings["cloudFilePath"].ToString();
        }

        #region BlsMethods
        
       


        /// <summary>
        /// GetData will return dataset for the input sql query, from the dataset select the datatable in the result page
        /// </summary>
        /// <param name="qry">fill the sql query you want to execute</param>
        public DataTable GetData(string qry)
        {
            string result = "";
            result = ads.GetChoices(qry, false);
            StringReader theReader = new StringReader(result);
            DataSet ds = new DataSet();
            ds.ReadXml(theReader);
            if (ds.Tables.Count > 2)
            {
                if (ds.Tables[2].Rows.Count > 0)
                    return ds.Tables[2];
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// UpdateData will return string as done if insert or update  for the input sql query, from the dataset select the datatable in the result page
        /// </summary>
        /// <param name="qry">fill the sql query you want to execute</param>
        public string UpdateData(string qry)
        {
            string result = ads.GetChoices(qry, false);
            return result;
        }

        /// <summary>
        /// This will bind the grdiview with provided datasource
        /// </summary>
        /// <param name="gv">gridview it self is the parameter</param>
        /// <param name="Source">any type of source is accepted(either of datatable or list<t>)</param>
        public void BindGrid(GridView gv, object Source)
        {
            gv.DataSource = Source;
            gv.DataBind();
        }

        /// <summary>
        /// This will bind the dropdownlist with provided datasource
        /// </summary>
        /// <param name="gv">dropdownlsit it self is the parameter</param>
        /// <param name="Source">any type of source is accepted(either of datatable or list<t>)</param>
        /// <param name="textField">string as the name of datatextfield </param>
        /// <param name="valueField">string as the name of datavaluefield</param>
        public void fillDDL(DropDownList ddl, string textField, string valueField, object Source)
        {
            try
            {
                ddl.DataSource = Source;
                ddl.DataTextField = textField;
                ddl.DataValueField = valueField;
                ddl.DataBind();
                ddl.Items.Insert(0, "Select");
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// This will select the dropdownlist item with the value 
        /// </summary>
        /// <param name="gv">dropdownlsit it self is the parameter</param>
        /// <param name="selval">string as value that to be selected</param>
        public void ddlSel(DropDownList ddl, string selval)
        {
            ddl.ClearSelection();
            ddl.Items.FindByValue(selval).Selected = true;
        }

        public void BindRepeater(Repeater rptr, object Source)
        {
            rptr.DataSource = Source;
            rptr.DataBind();
        }

        public void CheckMemberSession()
        {
            if (HttpContext.Current.Session["member"] == null)
            {
                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Response.Redirect("../Default.aspx");
            }
        }

        public void CheckPartnerSession()
        {
            if (HttpContext.Current.Session["partner"] == null)
            {
                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Response.Redirect("/Partners");
            }
        }
        //string cols = "";
        //foreach (DataColumn dc in dt.Columns)
        //{
        //    cols += dc.ColumnName + "$";
        //}


        /// <summary>
        /// Sends Email
        /// </summary>
        /// <param name="ToAddress"></param>
        /// <param name="bodyContent"></param>
        /// <param name="Subject"></param>
        public void SendMail(string ToAddress, string bodyContent, string Subject, string FromAddress)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.To.Add(ToAddress);
                msg.From = new MailAddress(FromAddress, FromAddress, System.Text.Encoding.UTF8);
                msg.Subject = Subject;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.Body = bodyContent;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = false;
                msg.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("onlinespokenenglish156@gmail.com", "partsofspeech");
                client.Port = 25;
                //or use 587
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                object userState = msg;
                client.Send(msg);
            }
            catch
            {
            }
        }
        public bool checkString(object obj)
        {
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(obj.ToString()) && obj.ToString() != "*")
                    return true;
            }
            return false;
        }

        public void BindDdls(DropDownList ddlFormsCount, DropDownList ddlUsersCount, string qry)
        {
            DataTable dt;
            dt = GetData(qry);
            DataView dv = new DataView(dt);
            DataTable dtForms = dv.ToTable(true, "formscount");
            dtForms = SortIntegerDT(dtForms, "formscount");
            DataTable dtUsers = dv.ToTable(true, "userscount");
            dtUsers = SortIntegerDT(dtUsers, "userscount");
            fillDDL(ddlFormsCount, "FORMSCOUNT", "FORMSCOUNT", dtForms);
            fillDDL(ddlUsersCount, "USERSCOUNT", "USERSCOUNT", dtUsers);
            dv.Dispose();
            dt.Dispose();
            dtForms.Dispose();
            dtUsers.Dispose();
        }

        private DataTable SortIntegerDT(DataTable dt, string Colname)
        {
            DataTable dt2 = dt.Clone();
            dt2.Columns[Colname].DataType = Type.GetType("System.Int32");

            foreach (DataRow dr in dt.Rows)
            {
                dt2.ImportRow(dr);
            }
            dt2.AcceptChanges();
            DataView dv = dt2.DefaultView;
            dv.Sort = Colname + " ASC";
            return dt2 = dv.ToTable();
        }
        #endregion

    }
}