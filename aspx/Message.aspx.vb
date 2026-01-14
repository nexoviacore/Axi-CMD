Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Xml
Imports System.IO
Partial Class Message
    Inherits System.Web.UI.Page
    Public EnableOldTheme As String
    Public cac_order As String
    Public proj As String
    Public sid As String
    Public userName As String
    Public tst_Scripts As String = ""
    Public enableBackForwButton As String = ""
    Dim logobj As LogFile.Log = New LogFile.Log()
    Dim util As Util.Util = New Util.Util()
    Public direction As String = "ltr"
    Public lang As String = ""

    Public langType As String = "en"
    Protected Overrides Sub InitializeCulture()
        If Session("language") IsNot Nothing Then
            util = New Util.Util()
            Dim dirLang As String = String.Empty
            dirLang = util.SetCulture(Session("language").ToString().ToUpper())
            If Not String.IsNullOrEmpty(dirLang) Then
                direction = dirLang.Split("-")(0)
                langType = dirLang.Split("-")(1)
            End If
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AxEnableOldTheme") IsNot Nothing Then
            EnableOldTheme = Session("AxEnableOldTheme").ToString().ToLower()
        End If

        If Session("project") = "" Then
            Dim url As String
            url = util.SESSEXPIRYPATH
            Response.Write("<script>" & vbCrLf)
            Response.Write("parent.parent.location.href='" & url & "';")
            Response.Write(vbCrLf & "</script>")
        Else

            If Session("language") <> Nothing Then
                lang = Session("language").ToString()
            End If
            If Request.UrlReferrer IsNot Nothing Then
                If Not (Request.UrlReferrer.AbsolutePath.ToLower().Contains("message.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("mainnew.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("iview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("tstruct.aspx")) Then
                    Response.Redirect("../CusError/AxCustomError.aspx")
                End If
            End If

            Dim sid As String
            sid = Session("nsessionid")
            cac_order = sid & "msg"

            If Session("language").ToString() = "ARABIC" Then
                direction = "rtl"
            End If

            If (Not IsPostBack) Then
                sid = Session("nsessionid").ToString()
                logobj.CreateLog("Loaging Message.aspx", sid, "login", "")
                proj = Session("project").ToString()
                proj = CheckSpecialChars(proj)
                ViewState("proj") = proj

                sid = CheckSpecialChars(sid)
                ViewState("sid") = sid
                userName = Session("user").ToString()
                userName = CheckSpecialChars(userName)
                ViewState("User") = userName

                Dim ivname As String = String.Empty
                If (Not System.Configuration.ConfigurationManager.AppSettings("ToDoiview") Is Nothing) Then
                    ivname = System.Configuration.ConfigurationManager.AppSettings("ToDoiview")
                End If

                If (ivname <> "") Then
                    Response.Redirect("iview.aspx?ivname=" & ivname)
                End If

                Dim filename As String = "ToDoList"
                Dim iXml As String = ""
                Dim errlog As String = logobj.CreateLog("Call to GetChoices Web Service", sid, "ToDoList", "")
                iXml = iXml & "<sqlresultset axpapp=" & Chr(34) & proj & Chr(34) & " sessionid= " & Chr(34) & sid & Chr(34) & " trace=" & Chr(34) & errlog & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " transid="""">"
                Dim dbType As String = String.Empty
                Dim ToDosql As String = String.Empty

                If (Not System.Configuration.ConfigurationManager.AppSettings("ToDosql") Is Nothing) Then
                    ToDosql = System.Configuration.ConfigurationManager.AppSettings("ToDosql")
                End If

                If (ToDosql <> "") Then
                    iXml = iXml & "<sql>" & ToDosql & "</sql></sqlresultset>"
                Else
                    If (Not Session("axdb") Is Nothing) Then
                        dbType = Session("axdb")
                    End If

                    If ((dbType.ToLower() = "oracle") Or (dbType.ToLower() = "")) Then

                        iXml = iXml & "<sql>select fromwhom,TO_CHAR(message) MESSAGE,  to_char(to_date(upddatetime, 'DD/MM/YYYY HH24:MI:SS'), 'DD/MM/YYYY') upddatetime, sname, recordid,to_char(dueby, 'DD/MM/YYYY') DUEDATE from axtasks where towhom ='" & Session("user") & "' and status=1 order by upddatetime desc</sql>"
                    ElseIf (dbType.ToLower() = "my sql" OrElse dbType.ToLower() = "mysql") Then
                        iXml = iXml & "<sql>SELECT fromwhom,convert(message, char(1000)) MESSAGE,convert(date_format(upddatetime, '%d/%m/%Y %H:%i:%s'), char(50)) upddatetime,sname,recordid,convert(date_format(dueby, '%d/%m/%Y %H:%i:%s'), char(50)) DUEDATE FROM axtasks WHERE towhom = '" & Session("user") & "' AND status = 1 ORDER BY convert(date_format(upddatetime, '%d/%m/%Y %H:%i:%s'), char(50)) DESC</sql>"
                    ElseIf (dbType.ToLower() = "ms sql") Then
                        iXml = iXml + "<sql>select fromwhom, convert(nvarchar(70),(message)) MESSAGE, convert(varchar(10), upddatetime, 103) upddatetime, sname, recordid, convert(varchar(10), dueby, 103) duedate from axtasks where towhom = '" & Session("user") & "' AND status = 1 ORDER BY convert(datetime, upddatetime) DESC</sql>"
                    End If
                    iXml += Session("axApps").ToString() + Application("axProps").ToString() & Session("axGlobalVars").ToString() & Session("axUserVars").ToString() & "</sqlresultset>"
                End If

                Dim ires As String = String.Empty
                'Call service
                Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
                ires = objWebServiceExt.CallGetChoiceWS("message", iXml)

                Dim strErrMsg As String = String.Empty
                strErrMsg = util.ParseXmlErrorNode(ires)


                If strErrMsg <> String.Empty Then
                    strErrMsg = strErrMsg.Replace(vbCr, "").Replace(vbLf, "")
                    If strErrMsg = Constants.SESSIONERROR Then
                        Session.RemoveAll()
                        Session.Abandon()
                        Dim url1 As String
                        url1 = util.SESSEXPIRYPATH
                        Response.Write("<script>" & vbCrLf)
                        Response.Write("parent.parent.location.href='" & url1 & "';")
                        Response.Write(vbCrLf & "</script>")
                    Else
                        Response.Redirect(util.ERRPATH + strErrMsg)
                    End If
                Else

                    Session("Msgsearch") = ires
                    BindDataGrid(ires)
                End If

                Session("message") = Nothing
            Else
                proj = ViewState("proj")
                sid = ViewState("sid")
                userName = ViewState("User")

            End If

        End If
        Dim docHt As Integer = 100
        tst_Scripts = tst_Scripts & "<script language=""javascript"" type=""text/javascript"" >function setDocht(){ }</script>"
        If Session("backForwBtnPressed") IsNot Nothing AndAlso Not Convert.ToBoolean(Session("backForwBtnPressed")) Then
            util.UpdateNavigateUrl(HttpContext.Current.Request.Url.AbsoluteUri)
        End If
        Session("backForwBtnPressed") = False
        enableBackForwButton = "<script language=""javascript"" type=""text/javascript"" > enableBackButton=""" & Convert.ToBoolean(Session("enableBackButton")) & """; enableForwardButton=""" & Convert.ToBoolean(Session("enableForwardButton")) & """;</script>"
    End Sub
    Private Function CheckSpecialChars(ByVal str As String) As String
        If str <> Nothing Then
            str = Regex.Replace(str, "&", "&amp;")
            str = Regex.Replace(str, "<", "&lt;")
            str = Regex.Replace(str, ">", "&gt;")
            str = Regex.Replace(str, "'", "&apos;")
        End If
        Return str
    End Function
    Private Sub BindDataGrid(ByVal a As String)

        Session("message") = Nothing
        Dim value As String = ""
        Dim ds As New DataSet()

        'The result contains the trace message after the trace is ON, hence removing the trace message before binding the grid.
        'Here 15 is the no of characters in the last node i.e. </sqlresultset>
        If Session("trace") = "true" Then
            a = a.Substring(0, a.IndexOf("</sqlresultset>") + 15)
        End If
        Dim sr As New System.IO.StringReader(a)
        ds.ReadXml(sr)

        If (ds.Tables().Count <= 1) Then
            Session("message") = Nothing
            lblMsg.Visible = True
            lblMsg.Text = lblNodata.Text
            searDiv.Visible = False
        Else
            Dim loadxmlDoc As New XmlDocument
            loadxmlDoc.LoadXml(a)
            Dim WrproductNodes As XmlNodeList
            WrproductNodes = loadxmlDoc.GetElementsByTagName("response")
            If (WrproductNodes.Count = 0) Then

            Else
                For Each WrproductNode As XmlNode In WrproductNodes
                    If (Not WrproductNode.Attributes("value") Is Nothing) Then
                        value = WrproductNode.Attributes("value").Value
                    End If
                Next
            End If

            If (value <> "--") Then
                For Each docRow As DataRow In ds.Tables("row").Rows

                    Dim dt As New System.Data.DataTable()
                    If Session("message") IsNot Nothing Then
                        dt = DirectCast(Session("message"), System.Data.DataTable)
                    Else
                        dt.Columns.Add("From")
                        dt.Columns.Add("Message")
                        dt.Columns.Add("Time")
                        dt.Columns.Add("link")
                        dt.Columns.Add("Due")

                    End If
                    Dim drow As DataRow = dt.NewRow()
                    drow("From") = docRow("FROMWHOM").ToString()
                    drow("Message") = docRow("MESSAGE").ToString()
                    Dim nUpdtime As String = docRow("UPDDATETIME").ToString()
                    If (nUpdtime <> "") Then
                        'Dim nMon As String = nUpdtime.Substring(0, 2)
                        'Dim nDate As String = nUpdtime.Substring(3, 2)
                        'Dim nYr As String = nUpdtime.Substring(6, 4)
                        'drow("Time") = nDate & "/" & nMon & "/" & nYr
                        drow("Time") = docRow("UPDDATETIME").ToString()
                    Else
                        drow("Time") = ""
                    End If
                    If docRow("DUEDATE").ToString() = "*" Then
                        drow("Due") = ""
                    Else
                        drow("Due") = docRow("DUEDATE").ToString()
                    End If

                    Dim sname As String = docRow("sname").ToString()
                    Dim recordid As String = docRow("recordid").ToString()
                    Dim link As String = "./tstruct.aspx?transid=" & sname & "&recordid=" & recordid
                    drow("link") = link

                    dt.Rows.Add(drow)
                    Session("message") = dt
                Next
                Cache.Insert(cac_order, ds.Tables("row"), Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            Else
                Session("message") = Nothing
                lblMsg.Visible = True
                lblMsg.Text = lblNodata.Text
                searDiv.Visible = False
            End If
        End If


        If Session("message") IsNot Nothing Then
            Dim dt As System.Data.DataTable = DirectCast(Session("message"), System.Data.DataTable)
            If (dt IsNot Nothing) AndAlso (dt.Rows.Count > 0) Then
                lblMsg.Visible = False
                gvMessages.Visible = True
                gvMessages.DataSource = dt
                gvMessages.DataBind()
            Else
                gvMessages.Visible = False
            End If
        End If

    End Sub

    Protected Sub gvMessages_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMessages.RowCommand
        If (e.CommandName = "Select") Then
            Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).Parent.Parent, GridViewRow)
            Dim links As String = e.CommandArgument.ToString()
            Response.Redirect(links)
        End If
    End Sub

    Protected Sub gvMessages_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMessages.PageIndexChanging
        Dim dtorders As New System.Data.DataTable()
        dtorders = Cache.Get(cac_order)
        Dim dt As New System.Data.DataTable()
        dt.Columns.Add("From")
        dt.Columns.Add("Message")
        dt.Columns.Add("Time")
        dt.Columns.Add("link")
        dt.Columns.Add("Due")
        For Each docRow As DataRow In dtorders.Rows

            Dim drow As DataRow = dt.NewRow()
            drow("From") = docRow("FROMWHOM").ToString()
            drow("Message") = docRow("MESSAGE").ToString()
            'Dim nUpdtime As String = docRow("UPDDATETIME").ToString()  'Commenting these lines since we will be directly setting the values from the datatable as per 'BindDataGrid' method.
            'Dim nMon As String = nUpdtime.Substring(0, 2)
            'Dim nDate As String = nUpdtime.Substring(3, 2)
            'Dim nYr As String = nUpdtime.Substring(6, 4)
            'drow("Time") = nDate & "/" & nMon & "/" & nYr
            If docRow("UPDDATETIME").ToString() <> "" Then
                drow("Time") = docRow("UPDDATETIME").ToString()
            Else
                drow("Time") = ""
            End If

            If docRow("DUEDATE").ToString() = "*" Then
                drow("Due") = ""
            Else
                drow("Due") = docRow("DUEDATE").ToString()
            End If

            Dim sname As String = docRow("sname").ToString()
            Dim recordid As String = docRow("recordid").ToString()

            Dim link As String = "./tstruct.aspx?transid=" & sname & "&recordid=" & recordid
            drow("link") = link
            dt.Rows.Add(drow)
        Next
        gvMessages.PageIndex = e.NewPageIndex
        gvMessages.DataSource = dt.DefaultView
        gvMessages.DataBind()


    End Sub
    Protected Sub clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles clear.Click
        Dim sewith As String
        sewith = swith.Text
        swith.Text = " "
    End Sub

    Protected Sub sbut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles sbut.Click

        Dim rexml As String = ""

        Dim sefor As String
        sefor = sf_Cb.SelectedValue
        Dim sewith As String
        sewith = swith.Text
        Dim xmlsfor As String = ""
        If sefor = "From" Then
            xmlsfor = "FROMWHOM"
        ElseIf sefor = "Message" Then
            xmlsfor = "MESSAGE"
        ElseIf sefor = "Date" Then
            xmlsfor = "UPDDATETIME"
        ElseIf sefor = "Duedate" Then
            xmlsfor = "DUEDATE"
        End If

        If sewith = "" Then

        Else
            Dim xmlDoc As New XmlDocument
            Dim productNodes As XmlNodeList
            Dim productNode As XmlNode
            Dim baseDataNodes As XmlNodeList
            Dim sres As String = Session("Msgsearch")
            xmlDoc.LoadXml(sres)

            productNodes = xmlDoc.SelectNodes("//row")
            For Each productNode In productNodes
                baseDataNodes = productNode.ChildNodes
                For Each baseDataNode As XmlNode In baseDataNodes
                    'Response.Write(baseDataNode.Name & "<br>")
                    If xmlsfor = baseDataNode.Name Then
                        Dim nval As String = ""
                        nval = baseDataNode.InnerText
                        If nval.ToLower.Contains(sewith.ToLower) Then
                            rexml = rexml & "<row>" & baseDataNode.ParentNode.InnerXml & "</row>"
                            Exit For
                        End If
                    End If
                Next
            Next
        End If
        If rexml <> "" Then
            rexml = "<sqlresultset><sql></sql><response>" & rexml & "</response></sqlresultset>"
        Else
            rexml = "<sqlresultset><sql></sql><response><row> <FROMWHOM></FROMWHOM><MESSAGE></MESSAGE><UPDDATETIME></UPDDATETIME><SNAME></SNAME><RECORDID></RECORDID><DUEDATE></DUEDATE></row></response></sqlresultset>"
        End If

        BindDataGrid(rexml)

    End Sub

    Protected Sub gvMessages_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMessages.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Dim lbl As Label = CType(e.Row.FindControl("lblFrom"), Label)
            Dim lbl_ndt As Label = CType(e.Row.FindControl("lblTime"), Label)
            Dim lbl_ddt As Label = CType(e.Row.FindControl("lblDue"), Label)
            'Check for due date < current date
            'If lbl_ddt.Text <> "" Then
            '    Dim actdt As String = lbl_ndt.Text
            '    Dim dd As String
            '    Dim MM As String
            '    Dim yyyy As String
            '    dd = Mid(actdt, 1, 2)
            '    MM = Mid(actdt, 4, 2)
            '    yyyy = Mid(actdt, 7)
            '    Dim newDt As Date
            '    newDt = New Date(Integer.Parse(yyyy), Integer.Parse(MM), Integer.Parse(dd))
            '    Dim currdt As New Date
            '    currdt = DateTime.Today
            '    'Response.Write(newDt & "..." & currdt & "****")
            '    If currdt < newDt Then
            '        ''Response.Write("yes...")
            '        'e.Row.BackColor = Drawing.Color.Blue
            '        'e.Row.Font.Bold = True
            '        e.Row.ForeColor = Drawing.Color.Red
            '    Else

            '    End If
            'End If

        End If
    End Sub
End Class
