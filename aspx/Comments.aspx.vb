Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Xml
Imports System.IO
Partial Class Comments
    Inherits System.Web.UI.Page
    Dim sid As String = String.Empty
    Public EnableOldTheme As String
    Dim tid As String = String.Empty
    Dim rid As String = String.Empty
    Dim replaceamp As String = "amp;"
    Dim logobj As LogFile.Log = New LogFile.Log()
    Dim util As Util.Util = New Util.Util()
    Public direction As String = "ltr"
    ''' <summary>
    ''' onload get the comments for a particular transaction which is attached with workflow.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("AxEnableOldTheme") IsNot Nothing Then
            EnableOldTheme = Session("AxEnableOldTheme").ToString().ToLower()
        End If
        If Request.UrlReferrer IsNot Nothing Then
            If Not (Request.UrlReferrer.AbsolutePath.ToLower().Contains("tstruct.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("comments.aspx")) Then
                Response.Redirect("../CusError/AxCustomError.aspx")
            End If
        End If
        If (Not IsPostBack) Then
            Session("comments") = Nothing
            sid = Session("nsessionid")
            tid = Request.QueryString("transid")

            If Not util.IsTransIdValid(tid) Then
                Response.Redirect(Constants.PARAMERR)
            End If

            rid = Request.QueryString("recordid")
            If Not util.IsNumber(rid) Then
                Response.Redirect(Constants.PARAMERR)
            End If


            Dim fileName As String = "ViewComments" & tid
            Dim errorLog As String = logobj.CreateLog("Getting comments.", sid, fileName, "new")
            If Session("language").ToString() = "ARABIC" Then
                direction = "rtl"
            End If
            Dim comments As String = "<root axpapp=" & Chr(34) & Session("project") & Chr(34) & " sessionid=" & Chr(34) & sid & Chr(34) & " trace=" & Chr(34) & errorLog & Chr(34) & " transid=" & Chr(34) & tid & Chr(34) & " recordid=" & Chr(34) & rid & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & ">"
            comments &= Session("axApps").ToString() & Application("axProps").ToString() & Session("axGlobalVars").ToString() & Session("axUserVars").ToString() & "</root>"
            Dim loadComments As String = String.Empty

            Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()

            'Call service
            loadComments = objWebServiceExt.CallViewCommentsWS(tid, comments)

            If loadComments = "" Then

            Else

                Dim strErrMsg As String = String.Empty
                strErrMsg = util.ParseXmlErrorNode(loadComments)

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
                    BindDataGrid(loadComments)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' bind the data from the web service to the gridview control.
    ''' </summary>
    ''' <param name="a"></param>
    ''' <remarks></remarks>
    Private Sub BindDataGrid(ByVal a As String)
        Dim ds As New DataSet()
        Dim sr As New System.IO.StringReader(a)
        ds.ReadXml(sr)
        If (ds.Tables().Count = 0) Then
            Session("comments") = Nothing
            lblMsg.Visible = True
            lblMsg.Text = "No Comments Found"
        Else
            For Each docRow As DataRow In ds.Tables(0).Rows
                Dim dt As New System.Data.DataTable()
                If Session("comments") IsNot Nothing Then
                    dt = DirectCast(Session("comments"), System.Data.DataTable)
                Else
                    dt.Columns.Add("From")
                    dt.Columns.Add("Comments")
                    dt.Columns.Add("Time")
                    dt.Columns.Add("Status")
                End If
                Dim drow As DataRow = dt.NewRow()
                drow("From") = docRow("USERNAME").ToString()
                Dim comm As String = docRow("COMMENTS").ToString()
                comm = comm.Replace(replaceamp, "&")
                drow("Comments") = comm
                drow("Time") = docRow("DATETIME").ToString()
                drow("Status") = docRow("STATUS").ToString()
                dt.Rows.Add(drow)
                Session("comments") = dt
            Next
        End If
        If Session("comments") IsNot Nothing Then
            Dim dt As System.Data.DataTable = DirectCast(Session("comments"), System.Data.DataTable)
            If (dt IsNot Nothing) AndAlso (dt.Rows.Count > 0) Then
                lblMsg.Visible = False
                gvComments.Visible = True
                gvComments.DataSource = dt
                gvComments.DataBind()
            Else
                gvComments.Visible = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' gridview sorting 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvComments_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvComments.Sorting
        Dim dtorders As New System.Data.DataTable()
        dtorders = DirectCast(Session("comments"), System.Data.DataTable)
        Dim DefaultView As New DataView(dtorders)
        dtorders.DefaultView.Sort = ""
        dtorders = Session("comments")
        If ViewState("SortDir") = "ASC" Then
            dtorders.DefaultView.Sort = "Time" + " " + "DESC"
            ViewState("SortDir") = "DESC"
        Else
            dtorders.DefaultView.Sort = "Time" + " " + "ASC"
            ViewState("SortDir") = "ASC"
        End If
        gvComments.DataSource = dtorders.DefaultView
        Session("comments") = dtorders
        gvComments.DataBind()

    End Sub

    ''' <summary>
    ''' gridview pagination.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvComments_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvComments.PageIndexChanging
        Dim dtOrders As New System.Data.DataTable()
        gvComments.Columns(1).Visible = True
        dtOrders = Session("comments")
        gvComments.PageIndex = e.NewPageIndex
        gvComments.DataSource = dtOrders.DefaultView
        gvComments.DataBind()
    End Sub
End Class
