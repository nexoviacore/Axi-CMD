using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using System.Web.Services;
using CacheMgr;
using ClosedXML.Excel;
using System.Data.OleDb;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using Npgsql;


public partial class Wizardsetting : System.Web.UI.Page
{
    LogFile.Log logobj = new LogFile.Log();
    Util.Util util = new Util.Util();
    ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    ASB.WebService asbWebService = new ASB.WebService();
    public string strFillDepPName = string.Empty;
    public static String lsProj = "dwb";
    public ArrayList arrFillList = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["project"] == null)
        //{
        //    //SessionExpired();
        //    return;
        //}
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        ScriptManager1.RegisterAsyncPostBackControl(this.cboType);
        //ScriptManager1.RegisterAsyncPostBackControl(this.cboCont1);
        //ScriptManager1.RegisterAsyncPostBackControl(this.cboCont2);
        btnSave.Click += new EventHandler(btnSave_Click);
        btnRemoveall.Click += new EventHandler(btnRemoveall_Click);
        btnRemove.Click += new EventHandler(btnRemove_Click);
        cboType.SelectedIndexChanged += new EventHandler(cboType_SelectedIndexChanged);
        cboCont1.SelectedIndexChanged += CboCont1_SelectedIndexChanged;
        cboCont2.SelectedIndexChanged += CboCont2_SelectedIndexChanged;
        btnUpload.Click += BtnUpload_Click;
        btnInactive.Click += BtnInactive_Click;
        btnDeleteTab.Click += BtnDeleteTab_Click;
        btnPublish.Click += BtnPublish_Click;
        //   btnCheck.Click += BtnCheck_Click;
        if (IsPostBack) return;
        fillType();
        //fillTabs();
        //fillContainerCbo();
        filldefault();
        //divupload.Visible = false;
        //divParamConfig.Visible = false;
    }

    private void BtnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            String filename = "save-AxPages";
            string errorLog = logobj.CreateLog("Call SaveDate", Session["nsessionid"].ToString(), "CallSaveDate-AxPages", "new", "true");
            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "insert into axpages (name,caption, props, blobno,visible,type,ordno,levelno,updatedon, createdon, createdby,updatedby, pagetype) values ('" + tbxPageTitle.Text.Trim() + "','" + tbxPageTitle.Text + "', 'wizardpage.aspx?wizard_id=1', 1, 'T', 'p',61,0,'" + DateTime.Now + "','" + DateTime.Now + "','admin','admin','web')";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            inputXML = query;
            logobj.CreateLog("Call to SaveData Web Service" + inputXML, Session["nsessionid"].ToString(), filename, "");
            result = asbExt.CallGetChoiceWS("", inputXML);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Page Published Successfully." + result + "');", true);
            //Response.Write(result);
            //Response.End();
        }
        catch (Exception ex)
        { Response.Write(ex.Message); }
        finally
        { }
    }

    private void CboCont2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboCont2.SelectedItem.ToString() != cboCont1.SelectedItem.ToString())
        {
            if (fillTstructFields(cboCont2.SelectedValue.ToString()) != String.Empty)
            {
                divCont4.InnerHtml = fillTstructFields(cboCont2.SelectedValue.ToString());
            }
            else if (fillPaymentGatewayFields(cboCont2.SelectedItem.ToString()) != String.Empty)
            {
                divCont4.InnerHtml = fillPaymentGatewayFields(cboCont2.SelectedItem.ToString());
            }
            else if (ConstructParamsHtml1(getIviewParam(cboCont2.SelectedValue.ToString())) != String.Empty)
            {
                divCont4.InnerHtml = ConstructParamsHtml1(getIviewParam(cboCont2.SelectedValue.ToString()));
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('both container not same please select different one!');", true);
        }
    }
    private void CboCont1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (fillTstructFields(cboCont1.SelectedValue.ToString()) != String.Empty)
        {
            divCont1.InnerHtml = fillTstructFields(cboCont1.SelectedValue.ToString());
        }
        else if (fillPaymentGatewayFields(cboCont1.SelectedItem.ToString()) != String.Empty)
        {
            divCont1.InnerHtml = fillPaymentGatewayFields(cboCont1.SelectedItem.ToString());
        }
        else if (ConstructParamsHtml1(getIviewParam(cboCont1.SelectedValue.ToString())) != String.Empty)
        {
            divCont1.InnerHtml = ConstructParamsHtml1(getIviewParam(cboCont1.SelectedValue.ToString()));
        }
    }

    private void BtnDeleteTab_Click(object sender, EventArgs e)
    {
        if (Session["cFile"] != null && Session["cFile"].ToString() != String.Empty)
            Session.Remove("cFile");
        String divSess = tbxTitleDel.Text;
        String divSess1 = tbxTitleDel.Text;
        if (Session["divContent"] != null && Session["divContent"].ToString() != String.Empty)
        {

            String[] strSess = Session["divContent"].ToString().Split(Convert.ToChar(","));
            String[] strSess1 = Session["divCbType"].ToString().Split(Convert.ToChar(","));
            String[] strSess2 = Session["divtStruct"].ToString().Split(Convert.ToChar(","));
            String[] strSess3 = Session["tbActive"].ToString().Split(Convert.ToChar(","));
            Session.Remove("divContent");
            if (strSess.Length > 0)
            {
                for (Int16 iCount = 0; iCount <= Convert.ToInt16(strSess.Length - 1); iCount++)
                {
                    divSess = strSess[iCount].ToString();
                    divSess1 = strSess[iCount].ToString() + "cont_1";
                    if (Session[divSess] != null && Session[divSess].ToString() != String.Empty)
                    {
                        //Response.Write(cboTabName.SelectedItem.ToString()+"-"+ Session[divSess].ToString());
                        //Response.End();
                        if (tbxTitleDel.Text == divSess)
                        {
                            Session.Remove(divSess);
                            Session.Remove(divSess1);
                            Session.Remove(strSess1[iCount].ToString());
                            Session.Remove(strSess2[iCount].ToString());
                            Session.Remove(strSess3[iCount].ToString());
                        }
                        else
                        {
                            if (Session["divContent"] != null && Session["divContent"].ToString() != String.Empty)
                            {

                                if (Session["divContent"].ToString().Contains(divSess) == false)
                                {
                                    Session["divContent"] = Session["divContent"].ToString() + "," + divSess;
                                    if (Session["tbActive"] != null && Session["tbActive"].ToString() != String.Empty)
                                    {
                                        Session["tbActive"] = Session["tbActive"].ToString() + ",Active-" + divSess;
                                    }
                                }

                            }
                            else
                            {
                                Session["divContent"] = divSess;
                                Session["tbActive"] = "Active-" + divSess;
                            }

                            if (Session["divCbType"] != null && Session["divCbType"].ToString() != String.Empty)
                            {

                                if (Session["divCbType"].ToString().Contains(strSess1[iCount].ToString()) == false)
                                {
                                    Session["divCbType"] = Session["divCbType"].ToString() + "," + strSess1[iCount].ToString();
                                }

                            }
                            else
                            {
                                Session["divCbType"] = strSess1[iCount].ToString();
                            }

                            if (Session["divtStruct"] != null && Session["divtStruct"].ToString() != String.Empty)
                            {

                                if (Session["divtStruct"].ToString().Contains(strSess2[iCount].ToString()) == false)
                                {
                                    Session["divtStruct"] = Session["divtStruct"].ToString() + "," + strSess2[iCount].ToString();
                                }
                            }
                            else
                            {
                                Session["divtStruct"] = strSess2[iCount].ToString();
                            }
                        }
                    }
                }
            }
        }
        filldefault();
    }

    private void BtnInactive_Click(object sender, EventArgs e)
    {
        if (Session["tbActive"] != null && Session["tbActive"].ToString() != String.Empty)
        {
            //Response.Write(Session["tbActive"].ToString());
            //Response.End();
            if (Session["tbActive"].ToString().Contains("Active-" + tbxTitleInc.Text) == true)
            {
                String lssesVal = Session["tbActive"].ToString();
                Session.Remove("tbActive");
                lssesVal = lssesVal.Replace("Active-" + tbxTitleInc.Text, "Inactive-" + tbxTitleInc.Text);
                Session["tbActive"] = lssesVal;
            }
            else
            {
                String lssesVal = Session["tbActive"].ToString();
                Session.Remove("tbActive");
                lssesVal = lssesVal.Replace("Inactive-" + tbxTitleInc.Text, "Active-" + tbxTitleInc.Text);
                Session["tbActive"] = lssesVal;
            }
        }
        Response.Redirect(Request.RawUrl);
    }

    private string ConstructParamsHtml1(string result)
    {

        StringBuilder strJsArrays = new StringBuilder();
        StringBuilder strParamDetails = new StringBuilder();
        ArrayList iviewParamValues = new ArrayList();
        ArrayList iviewParams = new ArrayList();
        bool isSqlFld = false;
        string clientCulture = null;
        IviewData objIview = new IviewData();
        string _xmlString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
        StringBuilder paramHtml = new StringBuilder();
        result = _xmlString + result;
        // logobj.CreateLog("Loading and setting parameters components", sid, fileName, string.Empty);
        string parameterName = string.Empty;
        string paramCaption = string.Empty;
        string paramType = string.Empty;
        string paramHidden = string.Empty;
        string paramMOE = string.Empty;
        string paramValue = string.Empty;
        string paramSql = string.Empty;
        string paramDepStr = string.Empty;
        Boolean unHideParams = false;
        string expr = string.Empty;
        string vExpr = string.Empty;
        int tabIndex = 0;

        XmlDocument xmlDoc = new XmlDocument();
        XmlNodeList productNodes = default(XmlNodeList);
        XmlNodeList baseDataNodes = default(XmlNodeList);
        //  StringBuilder strJsArrays = new StringBuilder();
        int iCnt = 0;
        int fldNo = 0;
        int dpCnt = 0;
        try
        {
            xmlDoc.LoadXml(result);
        }
        catch (XmlException ex)
        {
            // Response.Redirect(util.ERRPATH + ex.Message);
        }

        Regex reg = new Regex("[*'\",_&#^@]");
        productNodes = xmlDoc.SelectNodes("//root");
        bool showParam = false;
        if (productNodes[0].Attributes["showparams"] != null && productNodes[0].Attributes["showparams"].Value != string.Empty)
            showParam = Convert.ToBoolean(productNodes[0].Attributes["showparams"].Value);


        Int16 tCount = 0;
        foreach (XmlNode productNode in productNodes)
        {
            baseDataNodes = productNode.ChildNodes;
            foreach (XmlNode baseDataNode in baseDataNodes)
            {
                tCount++;
                if (baseDataNode.Attributes["cat"].Value == "params")
                {
                    paramValue = string.Empty;
                    if (baseDataNode.Attributes["value"] != null)
                        paramValue = baseDataNode.Attributes["value"].Value;

                    iviewParamValues.Add(paramValue);

                    foreach (XmlNode tstNode in baseDataNode)
                    {
                        if (tstNode.Name == "a0")
                        {
                            parameterName = tstNode.InnerText;
                        }
                        else if (tstNode.Name == "a2")
                        {
                            paramCaption = tstNode.InnerText;
                        }
                        else if (tstNode.Name == "a4")
                        {
                            paramType = tstNode.InnerText;
                        }
                    }
                }

                paramHtml.Append("<div itemid= 'itmiv-" + tCount.ToString() + "' class='btn btn-default box-item ui-draggable ui-draggable-handle' style='width: 100%;'>" + paramCaption + "</div>");
                //paramHtml.Append("Pram name: " + parameterName + " Param Cap: " + paramCaption + " Param type: " + paramType);
            }

        }
        return paramHtml.ToString();
    }

    private void BtnCheck_Click(object sender, EventArgs e)
    {
        try
        {
            //getIviewParam();
            //string inputXML = string.Empty;
            //ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            //String result = "";
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //DataSet ds1 = new DataSet();
            //DataTable dt1 = new DataTable();
            //string sqlQuery = string.Empty;
            //String filename = "save-AxPages" ;
            //string errorLog = logobj.CreateLog("Call SaveDate", Session["nsessionid"].ToString(), "CallSaveDate-AxPages", "new", "true");
            //string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            //sqlQuery = "insert into axpages (name,caption, props, blobno,visible,type,ordno,levelno,updatedon, createdon, createdby,updatedby, pagetype) values ('"+tbxTitle.Text+"','"+tbxContent.Text+"', 'wizardpage.aspx?wizard_id=1', 1, 'T', 'p',61,0,'"+DateTime.Now+ "','" + DateTime.Now + "','admin','admin','web')";
            //sqlQuery = util.CheckSpecialChars(sqlQuery);
            //query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            //inputXML = query;
            //logobj.CreateLog("Call to SaveData Web Service" + inputXML, Session["nsessionid"].ToString(), filename, "");
            //result = asbExt.CallGetChoiceWS("", inputXML);
            //Response.Write(result);
            //Response.End();
        }
        catch (Exception ex)
        { Response.Write(ex.Message); }
        finally
        { }
    }

    private void BtnUpload_Click(object sender, EventArgs e)
    {
        if (cboType.SelectedItem.ToString() == "CUSTOM PAGE")
        {
            if (cboTstruct.SelectedValue.ToString() == String.Empty)
            {
                string strFilePath = string.Empty;
                if (fileUploadCustom.HasFile)
                {
                    // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + fileUploadCustom.FileName.ToString() + "');", true);
                    string extension = System.IO.Path.GetExtension(fileUploadCustom.FileName);
                    //if (extension.ToUpper() != ".JPG" && extension.ToUpper() != ".GIF" && extension.ToUpper() != ".PNG" && extension.ToUpper() != ".JPEG" && extension.ToUpper() != ".BMP")
                    //{
                    //    lblMsg.Text = "Upload JPG/PNG/JPEG images only.";
                    //    lblMsg.ForeColor = System.Drawing.Color.Red;
                    //    return;
                    //}
                    strFilePath = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + fileUploadCustom.PostedFile.FileName;
                    fileUploadCustom.PostedFile.SaveAs(Server.MapPath(@"~/downloads/" + strFilePath.Trim()));
                    Session["cFile"] = "../downloads/" + strFilePath.Trim();
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Session["cFile"].ToString() + "');", true);
                }


                //if (fileUploadCustom.FileName.ToString() != String.Empty)
                //{
                //    fileUploadCustom.SaveAs(fullpath + "\\" + fileUploadCustom.PostedFile.FileName);
                //    lnk = fullpath + fileUploadCustom.PostedFile.FileName;
                //}
                if (Session["divtStruct"] != null && Session["divtStruct"].ToString() != String.Empty)
                {

                    if (Session["divtStruct"].ToString().Contains(Session["cFile"].ToString()) == false)
                    {
                        Session["divtStruct"] = Session["divtStruct"].ToString() + "," + Session["cFile"].ToString();
                    }

                }
                else
                {
                    Session["divtStruct"] = Session["cFile"].ToString();
                }
            }
            else
            {
                if (cboTstruct.SelectedItem.ToString() == "Import Page")
                    Session["cFile"] = "ImportNew.aspx";
                else if (cboTstruct.SelectedItem.ToString() == "Export Page")
                    Session["cFile"] = "ExportNew.aspx";

                if (Session["divtStruct"] != null && Session["divtStruct"].ToString() != String.Empty)
                {

                    if (Session["divtStruct"].ToString().Contains(cboTstruct.SelectedValue.ToString()) == false)
                    {
                        Session["divtStruct"] = Session["divtStruct"].ToString() + "," + cboTstruct.SelectedValue.ToString();
                    }

                }
                else
                {
                    Session["divtStruct"] = cboTstruct.SelectedValue.ToString();
                }
            }

            if (Session["divContent"] != null && Session["divContent"].ToString() != String.Empty)
            {

                if (Session["divContent"].ToString().Contains(tbxTitle.Text) == false)
                {
                    Session["divContent"] = Session["divContent"].ToString() + "," + tbxTitle.Text;
                }

            }
            else
            {
                Session["divContent"] = tbxTitle.Text;
            }
            if (Session["divCbType"] != null && Session["divCbType"].ToString() != String.Empty)
            {

                if (Session["divCbType"].ToString().Contains(cboType.SelectedItem.ToString()) == false)
                {
                    Session["divCbType"] = Session["divCbType"].ToString() + "," + cboType.SelectedItem.ToString();
                }

            }
            else
            {
                Session["divCbType"] = cboType.SelectedItem.ToString();
            }
            filldefault();
        }
    }

    private void filldefault()
    {

        String divSess = "";
        String divSess1 = "";
        System.Text.StringBuilder obSb = new System.Text.StringBuilder();
        System.Text.StringBuilder obSb1 = new System.Text.StringBuilder();
        try
        {
            if (Session["divContent"] != null && Session["divContent"].ToString() != String.Empty)
            {
                String[] strSess = Session["divContent"].ToString().Split(Convert.ToChar(","));
                String[] strSess1 = Session["divCbType"].ToString().Split(Convert.ToChar(","));
                String[] strSess2 = Session["divtStruct"].ToString().Split(Convert.ToChar(","));
                if (strSess.Length > 0)
                {
                    for (Int16 iCount = 0; iCount <= Convert.ToInt16(strSess.Length - 1); iCount++)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "showdiv('divmd1', '" + strSess.Length.ToString() + "');", true);
                        divSess = strSess[iCount].ToString();
                        divSess1 = strSess[iCount].ToString() + "cont_1";

                        if (Session[divSess] != null && Session[divSess].ToString() != String.Empty)
                        {
                            if (Session["tbActive"] != null && Session["tbActive"].ToString() != String.Empty)
                            {
                                if (Session["tbActive"].ToString().Contains("Active-" + divSess) == true)
                                {
                                    obSb.Append(Session[divSess].ToString().Replace("showdiv('divmd1', '1')", "showdiv('divmd1', '" + strSess.Length.ToString() + "')"));
                                }
                                else
                                {
                                    obSb.Append(Session[divSess].ToString().Replace("showdiv('divmd1', '1')", "showdiv('divmd1', '" + strSess.Length.ToString() + "')").Replace("<a ", "<a style='color:Red;' ").Replace("</i>Inactive", "</i>Active"));
                                }
                            }

                            if (Session[divSess1] != null && Session[divSess1].ToString() != String.Empty)
                            {

                                if (strSess1[iCount].ToString() == "TSTRUCT")
                                {
                                    if (iCount == 0)
                                        divCont1.InnerHtml = fillTstructFields(strSess2[iCount].ToString());
                                    else
                                        divCont4.InnerHtml = fillTstructFields(strSess2[iCount].ToString());
                                }
                                else if (strSess1[iCount].ToString() == "IVIEW")
                                {
                                    //Response.Write(ConstructParamsHtml1(getIviewParam(strSess2[iCount].ToString())));
                                    //Response.End();
                                    if (iCount == 0)
                                        divCont1.InnerHtml = ConstructParamsHtml1(getIviewParam(strSess2[iCount].ToString()));
                                    else
                                        divCont4.InnerHtml = ConstructParamsHtml1(getIviewParam(strSess2[iCount].ToString()));
                                }
                                else if (strSess1[iCount].ToString() == "PAYMENT GATEWAY")
                                {
                                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + fillPaymentGatewaysFldName(strSess2[iCount].ToString()) + "');", true);

                                    if (iCount == 0)
                                        divCont1.InnerHtml = fillPaymentGatewayFields(fillPaymentGatewaysFldName(strSess2[iCount].ToString()));
                                    else
                                        divCont4.InnerHtml = fillPaymentGatewayFields(fillPaymentGatewaysFldName(strSess2[iCount].ToString()));

                                }
                                obSb1.Append(Session[divSess1].ToString());
                            }
                        }
                        else
                        {
                            String paymentgDtl = "";
                            String lnk = "";

                            if (cboType.SelectedItem.ToString() == "TSTRUCT")
                            {
                                lnk = "tstruct.aspx?transid=" + cboTstruct.SelectedValue.ToString();
                                //if (iCount == 0)
                                //    divCont1.InnerHtml = fillTstructFields(cboTstruct.SelectedValue.ToString());
                                //else
                                //    divCont4.InnerHtml = ConstructParamsHtml1(getIviewParam(cboTstruct.SelectedValue.ToString()));
                            }
                            else if (cboType.SelectedItem.ToString() == "IVIEW")
                            {
                                lnk = "iview.aspx?ivname=" + cboTstruct.SelectedValue.ToString();
                                //if (iCount == 0)
                                //    divCont1.InnerHtml = ConstructParamsHtml1(getIviewParam(cboTstruct.SelectedValue.ToString()));
                                //else
                                //    divCont4.InnerHtml = fillTstructFields(cboTstruct.SelectedValue.ToString());
                            }
                            else if (cboType.SelectedItem.ToString() == "CUSTOM PAGE")
                            {
                                if (cboTstruct.SelectedValue.ToString() == String.Empty)
                                {
                                    if (Session["cFile"] != null && Session["cFile"].ToString() != String.Empty)
                                        lnk = Session["cFile"].ToString();
                                    Session.Remove("cFile");
                                }
                                else
                                {
                                    if (cboTstruct.SelectedItem.ToString() == "Import Page")
                                        lnk = "ImportNew.aspx";
                                    else if (cboTstruct.SelectedItem.ToString() == "Export Page")
                                        lnk = "ExportNew.aspx";
                                }
                            }
                            else if (cboType.SelectedItem.ToString() == "PAYMENT GATEWAY")
                            {
                                string inputXML = string.Empty;
                                ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
                                String result = "";
                                DataSet ds = new DataSet();
                                DataTable dt = new DataTable();
                                DataSet ds1 = new DataSet();
                                DataTable dt1 = new DataTable();
                                string sqlQuery = string.Empty;
                                string errorLog = logobj.CreateLog("GetLoginActivity.", Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
                                string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
                                sqlQuery = "select a1.field_name, a1.field_value from mst_gatway_values a1, mst_paymentgateway_config b1 where a1.mst_paymentgateway_configid = b1.mst_paymentgateway_configid and  b1.payment_gateway = " + cboTstruct.SelectedValue.ToString();
                                sqlQuery = util.CheckSpecialChars(sqlQuery);
                                query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";



                                //inputXML = "<sqlresultset axpapp='" + lsProj + "' sessionid='" + Session["nsessionid"].ToString() + "' direct='true' trace='" + "true" + "' appsessionkey='' username='" + ConfigurationManager.AppSettings["axUsername"].ToString() + "' password='" + ConfigurationManager.AppSettings["axPassword"].ToString() + "'>";
                                //inputXML += "<sql>select caption,ntransid from axpdef_tstruct</sql>";
                                //inputXML += "</sqlresultset>";
                                inputXML = query;
                                result = asbExt.CallGetChoiceWS("", inputXML);
                                DataSet dsPages = new DataSet();
                                System.IO.StringReader sr = new System.IO.StringReader(result);
                                dsPages.ReadXml(sr);
                                ds = dsPages;

                                if (ds.Tables.Count > 1)
                                {
                                    dt = ds.Tables["row"];
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    paymentgDtl += "<div style='padding-top:10px;' class='card col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened'><table  cellpadding='2' cellspacing='0' border='0' width='100%' style='padding-top:10px;' class='gridData ivirMainGrid stripe row-border hover order-column table dataTable'>";
                                    for (Int16 cCount = 0; cCount <= Convert.ToInt16(dt.Rows.Count - 1); cCount++)
                                    {
                                        paymentgDtl += "<tr><td>" + dt.Rows[cCount][0].ToString() + "</td><td>" + dt.Rows[cCount][1].ToString() + "</td></tr>";
                                    }
                                    paymentgDtl += "</table></div>";
                                }
                            }
                            String ifrm = "";
                            if (cboType.SelectedItem.ToString() == "PAYMENT GATEWAY")
                            {
                                ifrm = "<div style='padding-top:10px;' id='divmd" + (iCount + 1).ToString() + "'>" + paymentgDtl + "</div>";
                            }
                            else
                            {
                                //wizMiddle1.Src = lnk;
                                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "showAlertDialog('error','" + lnk + "');", true);
                                //ifrm = "<iframe src='" + lnk + "' title='Wizards Dashboard' width='100%' height='800px' style='height:800px;'></iframe>";
                                //ifrm = "<div id='divmd" + (iCount + 1).ToString() + "'>" + ifrm + "</div>";
                                ifrm = "<iframe src='" + lnk + "' title='Wizards Dashboard' class='card col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened' style='padding: 0px;' frameborder='0' scrolling='no' allowtransparency='True' width='100%'></iframe>";
                            }
                            //Session[divSess] = "<div class='aiia-wizard-step'><h1>" + tbxTitle.Text + "</h1><div class='step-content'><p>" + ifrm + " </p></div></div>";

                            Session[divSess] = "<li itemid='itm-1" + (iCount + 1).ToString() + "' class='btn btn-default box-item' style='width: 100%;'><a href='javascript:void();' onclick=\"showdiv('divmd" + (iCount + 1).ToString() + "', '" + strSess.Length.ToString() + "')\">" + tbxTitle.Text + "</a><div class='btn-group dropleft wizard-drop'><button type = 'button' class='btn btn-secondary dropdown-toggle' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'><i class='fa fa-ellipsis-h'></i></button><div class='dropdown-menu'><a class='dropdown-item' href='javascript:void();' onclick=\"filltypesofvalues('" + cboType.SelectedValue.ToString() + "');showeditModal('Y', '" + tbxTitle.Text + "', '" + cboType.SelectedValue.ToString() + "', '" + cboTstruct.SelectedValue.ToString() + "');\"><i class='fa fa-edit'></i>Edit</a><a class='dropdown-item' href='javascript:void();' onclick=\"showIncModal('" + tbxTitle.Text + "');\"><i class='fa fa-ban'></i>Inactive</a><a class='dropdown-item' href='javascript:void();' onclick=\"showDelModal('" + tbxTitle.Text + "');\"><i class='fa fa-trash'></i>Delete</a></div></div></li>";
                            Session[divSess1] = "<div id='divmd" + (iCount + 1).ToString() + "'>" + ifrm + "</div>";
                            obSb.Append(Session[divSess].ToString());
                            obSb1.Append(Session[divSess1].ToString());
                        }
                    }
                }
            }
            lstWizardLeftMenu.InnerHtml = obSb.ToString();//"<div class='aiia-wizard-step'><h1>" + tbxTitle.Text + "</h1><div class='step-content'><p>test</p></div></div>";
            divmiddle.InnerHtml = obSb1.ToString();//"<div class='aiia-wizard-step'><h1>" + tbxTitle.Text + "</h1><div class='step-content'><p>test</p></div></div>";
            //wizard.InnerHtml = obSb.ToString();
            //fillTabs();
            fillContainerCbo();

            //Response.Redirect(Request.RawUrl);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "');", true);
            Response.Write(ex.Message);
        }
        finally
        {

        }
    }
    void cboType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboType.SelectedItem.ToString() == "TSTRUCT")
        {
            cboTstruct.Items.Clear();
            fillTstruct();
            divcbo.Visible = true;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "hideupload();", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "hideupload();", true);
            //divupload.Visible = false;
            //mainbtn.Visible = true;
        }
        else if (cboType.SelectedItem.ToString() == "IVIEW")
        {
            cboTstruct.Items.Clear();
            fillIviews();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "hideupload();", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "hideupload();", true);
            divcbo.Visible = true;
            //divupload.Visible = false;
            //mainbtn.Visible = true;
        }
        else if (cboType.SelectedItem.ToString() == "CUSTOM PAGE")
        {
            cboTstruct.Items.Clear();
            //divupload.Visible = true;
            divcbo.Visible = true;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "showupload();", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "showupload();", true);
            //mainbtn.Visible = false;
            cboTstruct.Items.Clear();
            cboTstruct.Items.Insert(0, "Select");
            cboTstruct.Items[0].Value = "";
            cboTstruct.Items.Insert(1, "Import Page");
            cboTstruct.Items[1].Value = "Import Page";
            cboTstruct.Items.Insert(2, "Export Page");
            cboTstruct.Items[2].Value = "Export Page";

        }
        else if (cboType.SelectedItem.ToString() == "PAYMENT GATEWAY")
        {
            cboTstruct.Items.Clear();
            divcbo.Visible = true;
            //divupload.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "hideupload();", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "hideupload();", true);
            fillPaymentGateways();
            //mainbtn.Visible = true;
        }
    }


    private String getIviewParam(String lsIview)
    {
        try
        {
            string ires = string.Empty;
            //if (cboType.SelectedItem.ToString() == "IVIEW")
            {
                String iViewname = lsIview;
                if (iViewname != "")
                {
                    string fileName = "openiview-" + iViewname;
                    string errLog = logobj.CreateLog("Call to GetParams Web Service", Session["nsessionid"].ToString(), fileName, "");
                    string iXml = string.Empty;
                    iXml = "<root name =\"" + iViewname + "\" axpapp = \"" + Session["project"].ToString() + "\" sessionid = \"" + Session["nsessionid"].ToString() + "\" appsessionkey=\"" + HttpContext.Current.Session["AppSessionKey"].ToString() + "\" username=\"" + HttpContext.Current.Session["username"].ToString() + "\" trace = \"" + errLog + "\"  >";
                    iXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
                    IviewData objIview = new IviewData();
                    objIview.WebServiceTimeout = objWebServiceExt.asbIview.Timeout;
                    ires = objWebServiceExt.CallGetParamsWS(iViewname, iXml, objIview);
                    if (ires != string.Empty)
                    {
                        ires = ires.Split('♠')[1];
                    }
                    //Response.Write(ConstructParamsHtml1(ires));
                    //Response.End();
                }
            }
            return ires;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    /// Get All user defined Tsturct captions
    /// </summary>
    public void fillTstruct()
    {
        try
        {
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select caption,ntransid from axpdef_tstruct";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";



            //inputXML = "<sqlresultset axpapp='" + lsProj + "' sessionid='" + Session["nsessionid"].ToString() + "' direct='true' trace='" + "true" + "' appsessionkey='' username='" + ConfigurationManager.AppSettings["axUsername"].ToString() + "' password='" + ConfigurationManager.AppSettings["axPassword"].ToString() + "'>";
            //inputXML += "<sql>select caption,ntransid from axpdef_tstruct</sql>";
            //inputXML += "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
            }
            cboTstruct.DataSource = dt;
            cboTstruct.DataTextField = "caption";
            cboTstruct.DataValueField = "ntransid";
            cboTstruct.DataBind();
            cboTstruct.Items.Insert(0, "Select");
            cboTstruct.Items[0].Value = "";
        }
        catch (Exception ex)
        { Response.Write(ex.Message); }
        finally
        { }
    }

    /// <summary>
    /// Get All user defined Tsturct captions
    /// </summary>
    public String getTstructName(String lstsname)
    {
        try
        {
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select caption,ntransid from axpdef_tstruct where ntransid='" + lstsname + "'";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
            }
            return dt.Rows[0][0].ToString();
        }
        catch (Exception ex)
        { return ex.Message; }
        finally
        { }
    }


    /// <summary>
    /// Get All user defined Tsturct captions
    /// </summary>
    public String fillTstruct1()
    {
        try
        {
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select caption,ntransid from axpdef_tstruct";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";



            //inputXML = "<sqlresultset axpapp='" + lsProj + "' sessionid='" + Session["nsessionid"].ToString() + "' direct='true' trace='" + "true" + "' appsessionkey='' username='" + ConfigurationManager.AppSettings["axUsername"].ToString() + "' password='" + ConfigurationManager.AppSettings["axPassword"].ToString() + "'>";
            //inputXML += "<sql>select caption,ntransid from axpdef_tstruct</sql>";
            //inputXML += "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
            }
            cboTstruct.DataSource = dt;
            cboTstruct.DataTextField = "caption";
            cboTstruct.DataValueField = "ntransid";
            cboTstruct.DataBind();
            cboTstruct.Items.Insert(0, "Select");
            cboTstruct.Items[0].Value = "";

            return "";
        }
        catch (Exception ex)
        { return ex.Message; }
        finally
        { }
    }


    /// <summary>
    /// Get All user defined Tsturct captions
    /// </summary>
    public String fillTstructFields(String lsTstruct)
    {
        try
        {
            System.Text.StringBuilder obSb = new StringBuilder();
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select fname, datatype from axpflds where tstruct='" + lsTstruct + "'";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";



            //inputXML = "<sqlresultset axpapp='" + lsProj + "' sessionid='" + Session["nsessionid"].ToString() + "' direct='true' trace='" + "true" + "' appsessionkey='' username='" + ConfigurationManager.AppSettings["axUsername"].ToString() + "' password='" + ConfigurationManager.AppSettings["axPassword"].ToString() + "'>";
            //inputXML += "<sql>select caption,ntransid from axpdef_tstruct</sql>";
            //inputXML += "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
            }
            if (dt.Rows.Count > 0)
            {
                for (Int16 iCount = 0; iCount <= Convert.ToInt16(dt.Rows.Count - 1); iCount++)
                {
                    obSb.Append("<div itemid= 'itmts-" + (iCount + 1).ToString() + "' class='btn btn-default box-item' style='width: 100%;'>" + dt.Rows[iCount][0].ToString() + "</div>");
                }
            }
            return obSb.ToString();
        }
        catch (Exception ex)
        { return ex.Message; }
        finally
        { }
    }

    /// <summary>
    /// Get All user defined Tsturct captions
    /// </summary>
    public String fillPaymentGatewayFields(String lsTstruct)
    {
        try
        {
            StringBuilder obSb = new StringBuilder();
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select a1.mst_payment_fieldid, a1.field_name from mst_payment_field a1, mst_paymentgateway b1 where a1.mst_paymentgatewayid = b1.mst_paymentgatewayid and lsFixed = 'No' and lsrequired = 'Yes' and b1.Paymentgateway_name = '" + lsTstruct + "'";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";



            //inputXML = "<sqlresultset axpapp='" + lsProj + "' sessionid='" + Session["nsessionid"].ToString() + "' direct='true' trace='" + "true" + "' appsessionkey='' username='" + ConfigurationManager.AppSettings["axUsername"].ToString() + "' password='" + ConfigurationManager.AppSettings["axPassword"].ToString() + "'>";
            //inputXML += "<sql>select caption,ntransid from axpdef_tstruct</sql>";
            //inputXML += "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
            }
            if (dt.Rows.Count > 0)
            {
                for (Int16 iCount = 0; iCount <= Convert.ToInt16(dt.Rows.Count - 1); iCount++)
                {
                    obSb.Append("<div itemid= 'itmpg-" + (iCount + 1).ToString() + "' class='btn btn-default box-item' style='width: 100%;'>" + dt.Rows[iCount][1].ToString() + "</div>");
                }
            }
            return obSb.ToString();
        }
        catch (Exception ex)
        { return ex.Message; }
        finally
        { }
    }
    /// <summary>
    /// Get All user defined Views captions
    /// </summary>
    private void fillIviews()
    {
        try
        {
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select name, caption from iviews";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";



            //inputXML = "<sqlresultset axpapp='" + lsProj + "' sessionid='" + Session["nsessionid"].ToString() + "' direct='true' trace='" + "true" + "' appsessionkey='' username='" + ConfigurationManager.AppSettings["axUsername"].ToString() + "' password='" + ConfigurationManager.AppSettings["axPassword"].ToString() + "'>";
            //inputXML += "<sql>select caption,ntransid from axpdef_tstruct</sql>";
            //inputXML += "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
            }
            cboTstruct.DataSource = dt;
            cboTstruct.DataTextField = "caption";
            cboTstruct.DataValueField = "name";
            cboTstruct.DataBind();
            cboTstruct.Items.Insert(0, "Select");
            cboTstruct.Items[0].Value = "";
        }
        catch (Exception ex)
        { Response.Write(ex.Message); }
        finally
        { }
    }

    /// <summary>
    /// Get All user defined Views captions
    /// </summary>
    private String getIviewsName(String ivName)
    {
        try
        {
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select name, caption from iviews where name='" + ivName + "'";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
            }
            return dt.Rows[0][1].ToString();
        }
        catch (Exception ex)
        { return ex.Message; }
        finally
        { }
    }

    public String fillIviews1()
    {
        try
        {
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select name, caption from iviews";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";
            //inputXML = "<sqlresultset axpapp='" + lsProj + "' sessionid='" + Session["nsessionid"].ToString() + "' direct='true' trace='" + "true" + "' appsessionkey='' username='" + ConfigurationManager.AppSettings["axUsername"].ToString() + "' password='" + ConfigurationManager.AppSettings["axPassword"].ToString() + "'>";
            //inputXML += "<sql>select caption,ntransid from axpdef_tstruct</sql>";
            //inputXML += "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
            }
            cboTstruct.DataSource = dt;
            cboTstruct.DataTextField = "caption";
            cboTstruct.DataValueField = "name";
            cboTstruct.DataBind();
            cboTstruct.Items.Insert(0, "Select");
            cboTstruct.Items[0].Value = "";
            return "";
        }
        catch (Exception ex)
        { return ex.Message; }
        finally
        { }
    }

    /// <summary>
    /// Get All Payment gateways 
    /// </summary>
    private void fillPaymentGateways()
    {
        try
        {
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select Paymentgateway_Name as caption, mst_paymentgatewayid from mst_paymentgateway";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";



            //inputXML = "<sqlresultset axpapp='" + lsProj + "' sessionid='" + Session["nsessionid"].ToString() + "' direct='true' trace='" + "true" + "' appsessionkey='' username='" + ConfigurationManager.AppSettings["axUsername"].ToString() + "' password='" + ConfigurationManager.AppSettings["axPassword"].ToString() + "'>";
            //inputXML += "<sql>select caption,ntransid from axpdef_tstruct</sql>";
            //inputXML += "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
            }
            cboTstruct.DataSource = dt;
            cboTstruct.DataTextField = "caption";
            cboTstruct.DataValueField = "mst_paymentgatewayid";
            cboTstruct.DataBind();
            cboTstruct.Items.Insert(0, "Select");
            cboTstruct.Items[0].Value = "";
        }
        catch (Exception ex)
        { Response.Write(ex.Message); }
        finally
        { }
    }

    public String fillPaymentGateways1()
    {
        try
        {
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select Paymentgateway_Name as caption, mst_paymentgatewayid from mst_paymentgateway";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";



            //inputXML = "<sqlresultset axpapp='" + lsProj + "' sessionid='" + Session["nsessionid"].ToString() + "' direct='true' trace='" + "true" + "' appsessionkey='' username='" + ConfigurationManager.AppSettings["axUsername"].ToString() + "' password='" + ConfigurationManager.AppSettings["axPassword"].ToString() + "'>";
            //inputXML += "<sql>select caption,ntransid from axpdef_tstruct</sql>";
            //inputXML += "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
            }
            cboTstruct.DataSource = dt;
            cboTstruct.DataTextField = "caption";
            cboTstruct.DataValueField = "mst_paymentgatewayid";
            cboTstruct.DataBind();
            cboTstruct.Items.Insert(0, "Select");
            cboTstruct.Items[0].Value = "";
            return "";
        }
        catch (Exception ex)
        { return ex.Message; }
        finally
        { }
    }

    public String fillPaymentGatewaysFldName(String lsfname)
    {
        try
        {
            string inputXML = string.Empty;
            ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
            String result = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sqlQuery = string.Empty;
            string errorLog = logobj.CreateLog("GetLoginActivity.", Session["nsessionid"].ToString(), "GetLoginAct-Wizardsetting" + string.Empty, "new");
            string query = "<sqlresultset axpapp='" + Session["project"].ToString() + "' sessionid='" + Session["nsessionid"].ToString() + "' trace='" + errorLog + "' appsessionkey='" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' ><sql>";
            sqlQuery = "select Paymentgateway_Name as caption, mst_paymentgatewayid from mst_paymentgateway where mst_paymentgatewayid='" + lsfname + "'";
            sqlQuery = util.CheckSpecialChars(sqlQuery);
            query += sqlQuery + " </sql>" + Session["axApps"].ToString() + Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</sqlresultset>";



            //inputXML = "<sqlresultset axpapp='" + lsProj + "' sessionid='" + Session["nsessionid"].ToString() + "' direct='true' trace='" + "true" + "' appsessionkey='' username='" + ConfigurationManager.AppSettings["axUsername"].ToString() + "' password='" + ConfigurationManager.AppSettings["axPassword"].ToString() + "'>";
            //inputXML += "<sql>select caption,ntransid from axpdef_tstruct</sql>";
            //inputXML += "</sqlresultset>";
            inputXML = query;
            result = asbExt.CallGetChoiceWS("", inputXML);
            DataSet dsPages = new DataSet();
            System.IO.StringReader sr = new System.IO.StringReader(result);
            dsPages.ReadXml(sr);
            ds = dsPages;

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables["row"];
            }
            return dt.Rows[0][0].ToString();
        }
        catch (Exception ex)
        { return ex.Message; }
        finally
        { }
    }


    void btnRemove_Click(object sender, EventArgs e)
    {
        if (Session["cFile"] != null && Session["cFile"].ToString() != String.Empty)
            Session.Remove("cFile");
        String divSess = cboTabName.SelectedItem.ToString();
        if (Session["divContent"] != null && Session["divContent"].ToString() != String.Empty)
        {

            String[] strSess = Session["divContent"].ToString().Split(Convert.ToChar(","));
            Session.Remove("divContent");
            if (strSess.Length > 0)
            {
                for (Int16 iCount = 0; iCount <= Convert.ToInt16(strSess.Length - 1); iCount++)
                {
                    divSess = strSess[iCount].ToString();

                    if (Session[divSess] != null && Session[divSess].ToString() != String.Empty)
                    {
                        //Response.Write(cboTabName.SelectedItem.ToString()+"-"+ Session[divSess].ToString());
                        //Response.End();
                        if (cboTabName.SelectedItem.ToString() == divSess)
                        {
                            Session.Remove(divSess);
                        }
                        else
                        {
                            if (Session["divContent"] != null && Session["divContent"].ToString() != String.Empty)
                            {

                                if (Session["divContent"].ToString().Contains(divSess) == false)
                                {
                                    Session["divContent"] = Session["divContent"].ToString() + "," + divSess;
                                }

                            }
                            else
                            {
                                Session["divContent"] = divSess;
                            }
                        }
                    }
                }
            }
        }
        filldefault();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void btnRemoveall_Click(object sender, EventArgs e)
    {
        if (Session["cFile"] != null && Session["cFile"].ToString() != String.Empty)
            Session.Remove("cFile");
        if (Session["divCbType"] != null && Session["divCbType"].ToString() != String.Empty)
        {
            Session.Remove("divCbType");
        }
        if (Session["divtStruct"] != null && Session["divtStruct"].ToString() != String.Empty)
        {
            Session.Remove("divtStruct");
        }
        if (Session["divContent"] != null && Session["divContent"].ToString() != String.Empty)
        {
            String divSess = "";
            String divSess1 = "";
            String[] strSess = Session["divContent"].ToString().Split(Convert.ToChar(","));
            if (strSess.Length > 0)
            {
                for (Int16 iCount = 0; iCount <= Convert.ToInt16(strSess.Length - 1); iCount++)
                {
                    divSess = strSess[iCount].ToString();
                    divSess1 = strSess[iCount].ToString() + "cont_1";
                    if (Session[divSess] != null && Session[divSess].ToString() != String.Empty)
                    {
                        Session.Remove(divSess);
                    }
                    if (Session[divSess1] != null && Session[divSess1].ToString() != String.Empty)
                    {
                        Session.Remove(divSess1);
                    }
                }
            }
            Session.Remove("divContent");
        }
        Response.Redirect(Request.RawUrl);
        //ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "document.getElementById('FrameID').contentDocument.location.reload(true);", true);

        //Response.Redirect("wizardsetting.aspx");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void btnSave_Click(object sender, EventArgs e)
    {
        String divSess = "";
        String divSess1 = "";
        try
        {

            if (tbxEdit.Text == "Y")
            {
                if (Session["tbActive"] != null && Session["tbActive"].ToString() != String.Empty)
                {
                    if (Session["tbActive"].ToString().Contains(tbTitle.Text) == true)
                    {
                        String lssesVal = Session["tbActive"].ToString();
                        Session.Remove("tbActive");
                        lssesVal = lssesVal.Replace(tbTitle.Text, tbxTitle.Text);
                        Session["tbActive"] = lssesVal;
                    }
                }

                if (Session["divContent"] != null && Session["divContent"].ToString() != String.Empty)
                {
                    if (Session["divContent"].ToString().Contains(tbTitle.Text) == true)
                    {
                        String lssesVal = Session["divContent"].ToString();
                        Session.Remove("divContent");
                        lssesVal = lssesVal.Replace(tbTitle.Text, tbxTitle.Text);
                        Session["divContent"] = lssesVal;
                        String[] strSess = Session["divContent"].ToString().Split(Convert.ToChar(","));
                        if (strSess.Length > 0)
                        {
                            for (Int16 iCount = 0; iCount <= Convert.ToInt16(strSess.Length - 1); iCount++)
                            {
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallShowAlertDialog", "showdiv('divmd1', '" + strSess.Length.ToString() + "');", true);
                                divSess = strSess[iCount].ToString();
                                divSess1 = strSess[iCount].ToString() + "cont_1";
                                if (Session[divSess] != null && Session[divSess].ToString() != String.Empty)
                                {
                                    if (tbTitle.Text == divSess)
                                    {
                                        Session.Remove(divSess);
                                    }
                                }
                                if (Session[divSess1] != null && Session[divSess1].ToString() != String.Empty)
                                {
                                    if (divSess1.Contains(tbTitle.Text))
                                        Session.Remove(divSess1);
                                }
                            }
                        }
                    }
                }
                if (Session["divCbType"] != null && Session["divCbType"].ToString() != String.Empty)
                {

                    if (Session["divCbType"].ToString().Contains(tbType.Text) == true)
                    {
                        String lssesVal = Session["divCbType"].ToString();
                        Session.Remove("divCbType");
                        lssesVal = lssesVal.Replace(tbType.Text, cboType.SelectedItem.ToString());
                        Session["divCbType"] = lssesVal;
                    }
                }
                if (Session["divtStruct"] != null && Session["divtStruct"].ToString() != String.Empty)
                {
                    if (Session["divtStruct"].ToString().Contains(tbst.Text) == true)
                    {
                        String lssesVal = Session["divtStruct"].ToString();
                        Session.Remove("divtStruct");
                        lssesVal = lssesVal.Replace(tbst.Text, cboTstruct.SelectedValue.ToString());
                        Session["divtStruct"] = lssesVal;
                    }
                }
            }
            else
            {
                if (Session["divContent"] != null && Session["divContent"].ToString() != String.Empty)
                {

                    if (Session["divContent"].ToString().Contains(tbxTitle.Text) == false)
                    {
                        Session["divContent"] = Session["divContent"].ToString() + "," + tbxTitle.Text;
                        if (Session["tbActive"] != null && Session["tbActive"].ToString() != String.Empty)
                        {
                            Session["tbActive"] = Session["tbActive"].ToString() + ",Active-" + tbxTitle.Text;
                        }
                    }

                }
                else
                {
                    Session["divContent"] = tbxTitle.Text;
                    Session["tbActive"] = "Active-" + tbxTitle.Text;
                }

                if (Session["divCbType"] != null && Session["divCbType"].ToString() != String.Empty)
                {

                    if (Session["divCbType"].ToString().Contains(cboType.SelectedItem.ToString()) == false)
                    {
                        Session["divCbType"] = Session["divCbType"].ToString() + "," + cboType.SelectedItem.ToString();
                    }

                }
                else
                {
                    Session["divCbType"] = cboType.SelectedItem.ToString();
                }

                if (Session["divtStruct"] != null && Session["divtStruct"].ToString() != String.Empty)
                {

                    if (Session["divtStruct"].ToString().Contains(cboTstruct.SelectedValue.ToString()) == false)
                    {
                        Session["divtStruct"] = Session["divtStruct"].ToString() + "," + cboTstruct.SelectedValue.ToString();
                    }
                }
                else
                {
                    Session["divtStruct"] = cboTstruct.SelectedValue.ToString();
                }
            }

            filldefault();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "location.reload();", true);
            Response.Redirect(Request.RawUrl);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "');", true);
            Response.Write(ex.Message);
        }
        finally
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void fillType()
    {
        cboType.Items.Insert(0, "Select");
        cboType.Items[0].Value = "";
        cboType.Items.Insert(1, "TSTRUCT");
        cboType.Items[1].Value = "TSTRUCT";
        cboType.Items.Insert(2, "IVIEW");
        cboType.Items[2].Value = "IVIEW";
        cboType.Items.Insert(3, "CUSTOM PAGE");
        cboType.Items[3].Value = "CUSTOM PAGE";
        cboType.Items.Insert(4, "PAYMENT GATEWAY");
        cboType.Items[4].Value = "PAYMENT GATEWAY";
    }

    private void fillTabs()
    {
        cboTabName.Items.Clear();
        if (Session["divContent"] != null && Session["divContent"].ToString() != String.Empty)
        {
            String divSess = "";
            String[] strSess = Session["divContent"].ToString().Split(Convert.ToChar(","));
            if (strSess.Length > 0)
            {
                //divParamConfig.Visible = true;
                for (Int16 iCount = 0; iCount <= Convert.ToInt16(strSess.Length - 1); iCount++)
                {
                    divSess = strSess[iCount].ToString();
                    if (Session[divSess] != null && Session[divSess].ToString() != String.Empty)
                    {
                        cboTabName.Items.Insert(iCount, divSess);
                        cboTabName.Items[iCount].Value = divSess;
                    }
                }
            }
        }
    }

    private void fillContainerCbo()
    {
        cboTabName.Items.Clear();
        if (Session["divtStruct"] != null && Session["divtStruct"].ToString() != String.Empty)
        {
            String divSess = "";
            String fName = "";
            String divSess1 = "";
            String[] strSess = Session["divtStruct"].ToString().Split(Convert.ToChar(","));
            String[] strSess1 = Session["divCbType"].ToString().Split(Convert.ToChar(","));
            if (strSess.Length > 0)
            {
                cboCont1.Items.Insert(0, "Select");
                cboCont1.Items[0].Value = "";
                cboCont2.Items.Insert(0, "Select");
                cboCont2.Items[0].Value = "";
                for (Int16 iCount = 0; iCount <= Convert.ToInt16(strSess.Length - 1); iCount++)
                {
                    divSess = strSess[iCount].ToString();
                    divSess1 = strSess1[iCount].ToString();
                    if (divSess1 == "TSTRUCT")
                    {
                        fName = getTstructName(divSess);

                    }
                    else if (divSess1 == "IVIEW")
                    {
                        fName = getIviewsName(divSess);
                    }
                    else if (divSess1 == "PAYMENT GATEWAY")
                    {
                        fName = fillPaymentGatewaysFldName(divSess);
                    }
                    cboCont1.Items.Insert(iCount + 1, fName);
                    cboCont1.Items[iCount + 1].Value = divSess;
                    cboCont2.Items.Insert(iCount + 1, fName);
                    cboCont2.Items[iCount + 1].Value = divSess;
                }
            }
        }
    }
}