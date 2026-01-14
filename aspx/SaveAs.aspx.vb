Imports System.Xml
Imports System.Text
Imports System.IO

Partial Class SaveAs
    Inherits System.Web.UI.Page
    Public sb As StringBuilder = New StringBuilder()
    Dim iname As String
    Public user As String
    Public sid As String
    Dim ivtype As String
    Public proj As String
    Dim paramVal As String = ""
    Dim ivKey As String = ""
    Dim caption As String = ""
    Dim isPrint As String
    Public param1 As String
    Public trace As String
    Public EnableOldTheme As String
    Public direction As String = "ltr"

    Dim _xmlString As String = _
                   "<?xml version=""1.0"" encoding=""utf-8"" ?>"
    Dim colHide As New ArrayList()
    Dim colFld As New ArrayList()
    Dim colHead As New ArrayList()
    Dim paramXml As String = ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.UrlReferrer IsNot Nothing Then
            If Not (Request.UrlReferrer.AbsolutePath.ToLower().Contains("iview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("listiview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("saveas.aspx")) Then
                Response.Redirect("../CusError/AxCustomError.aspx")
            End If
        End If
        If Session("AxEnableOldTheme") IsNot Nothing Then
            EnableOldTheme = Session("AxEnableOldTheme").ToString().ToLower()
        End If

        If Not IsPostBack Then

            Dim utilObj As New Util.Util()

            iname = Request.QueryString("tid")
            If Not (utilObj.IsValidIvName(iname)) Then
                Response.Redirect(Constants.PARAMERR)
            End If

            ivtype = Request.QueryString("ivtype")
            If Not utilObj.IsChar(ivtype) Or ivtype.Length > 5 Then
                Response.Redirect(Constants.PARAMERR)
            End If
            paramVal = Request.QueryString("param")
            If Not Request.QueryString("ivKey") Is Nothing Then
                ivKey = Request.QueryString("ivKey")
            End If

            ViewState("iname") = iname

            ViewState("params") = paramVal
            ViewState("ivKey") = ivKey

            If Session("language").ToString() = "ARABIC" Then
                direction = "rtl"
            End If

            user = Session("user")
            sid = Session("nsessionid")
            proj = Session("project")
            trace = Session("trace")
            ViewState("user") = user
            ViewState("sid") = sid
            ViewState("proj") = proj
            ViewState("trace") = trace
            ViewState("ivtype") = ivtype
        Else

            iname = ViewState("iname")
            paramVal = ViewState("params")
            ivKey = ViewState("ivKey")
            user = ViewState("user")
            sid = ViewState("sid")
            proj = ViewState("proj")
            trace = ViewState("trace")
            ivtype = ViewState("ivtype")
        End If
    End Sub

    Protected Sub btnSaveAs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAs.Click


        Session("ivname") = iname
        Session("params") = param.Value
        Session("ivtype") = ivtype
        Session("ivKey") = ivKey
        lblErr.Text = ""
        Select Case ddloptions.SelectedValue
            Case "HTML"
                ClientScript.RegisterStartupScript(Me.GetType(), "Htmlfile", "OpenHtml()", True)
            Case "PDF"
                Server.Execute("pdfiview.aspx")
            Case "WORD"
                Server.Execute("WordView.aspx")
            Case "CSV"
                Server.Execute("csviview.aspx")
            Case "XML"
                Server.Execute("xmliview.aspx")
            Case "Excel"
                Server.Execute("excel.aspx")
            Case "Save As"
                lblErr.Text = "Please select a type to save as."
            Case Else

        End Select


        'End If


    End Sub




End Class
