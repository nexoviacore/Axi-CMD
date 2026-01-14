
Partial Class err
    Inherits System.Web.UI.Page
    Public isErred As Boolean
    Public errMsg As String = String.Empty
    Public enableBackForwButton As String = ""
    Public customError As String = String.Empty
    Public loginStr As String
    Public EnableOldTheme As String
    Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
    Public direction As String = "ltr"
    Public serverprocesstime As String = ""
    Public requestProcess_logtime As String = ""
    Dim util As New Util.Util()
    Dim ObjExecTr As ExecTrace = ExecTrace.Instance
    Public misflag As String
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
        If Request.QueryString("errmsg") <> Nothing Then
            Dim util1 = New Util.Util()
            If util1.CheckCrossScriptingInString(Request.QueryString("errmsg")) = "True" Or util1.ContainsXSS(Request.QueryString("errmsg")) = "True" Then
                misflag = Constants.MALICIOUSNPUTDETECTED
            End If
        End If

        If misflag <> Nothing Then
            errMsg = misflag
            Return
        End If
        If Request.QueryString("errmsg") <> Nothing Then
            Dim errExecMsg = Request.QueryString("errmsg")
            If errExecMsg.Contains("*♠*") Then
                errExecMsg = errExecMsg.Replace("*♠*", "♠")
                requestProcess_logtime = errExecMsg.Split("♠")(1)
            End If
        End If


        'Session("enableForwardButton") = True
        If Session("project") Is Nothing Or Session("project") = "" Then
            Dim url As String
            url = util.SESSEXPIRYPATH
            Response.Write("<script>" & vbCrLf)
            Response.Write("parent.parent.location.href='" & url & "';")
            Response.Write(vbCrLf & "</script>")
        End If

        If Session("language") IsNot Nothing Then
            If Session("language").ToString() = "ARABIC" Then
                direction = "rtl"
            End If
        End If
        If Session("AxEnableOldTheme") IsNot Nothing Then
            EnableOldTheme = Session("AxEnableOldTheme").ToString().ToLower()
        End If

        If Not IsPostBack Then
            enableBackForwButton = "<script type=""text/javascript"" > enableBackButton=""" & Convert.ToBoolean(Session("enableBackButton")) & """; enableForwardButton=""" & Convert.ToBoolean(Session("enableForwardButton")) & """;</script>"
            loginStr = Application("LoginPath").ToString()
            If Request.QueryString("errmsg") <> Nothing Then
                errMsg = Request.QueryString("errmsg")

                If errMsg.Contains("*♠*") Then
                    errMsg = errMsg.Replace("*♠*", "♠")
                    errMsg = errMsg.Split("♠")(0)
                End If

                If errMsg = "Invalid parameter" Then
                    errMsg = lblInvParams.Text
                ElseIf errMsg.Contains("Unknown error.") Then
                    errMsg = lblUnknownError.Text
                ElseIf errMsg.Contains("Error occurred(2)") Then
                    errMsg = lbleroccurred.Text
                Else
                    errMsg = errMsg
                End If
            End If
            If errMsg = String.Empty Then
                customError = lblCustomerror.Text
            End If
        End If

        If Session("backForwBtnPressed") IsNot Nothing AndAlso CBool(Session("backForwBtnPressed")) = False Then
            util.UpdateNavigateUrl(HttpContext.Current.Request.Url.AbsoluteUri)
        End If
        If requestProcess_logtime <> Nothing Then
            requestProcess_logtime += ObjExecTr.RequestProcessTime("Response")
            serverprocesstime = ObjExecTr.TotalServerElapsTime()
        End If
    End Sub
End Class
