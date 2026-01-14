Imports System.IO
Imports LogFile

Partial Class signout
    Inherits System.Web.UI.Page
    Public appName As String
    Public appTitle As String
    Public loginStr As String
    Public EnableOldTheme As String
    Dim util As New Util.Util()
    Public direction As String = "ltr"
    Public langType As String = "en"
    Public AxCloudDB As String
    Public strFileinfo As String = String.Empty

    Protected Overrides Sub InitializeCulture()
        If Session("language") IsNot Nothing Or Application("LangSess") IsNot Nothing Then
            Dim langProj As String = If(Session("language") Is Nothing, Application("LangSess").ToString(), Session("language"))
            Dim dirLang As String = String.Empty
            dirLang = util.SetCulture(langProj.ToUpper())
            If Not String.IsNullOrEmpty(dirLang) Then
                direction = dirLang.Split("-")(0)
                langType = dirLang.Split("-")(1)
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            main_body.Attributes.Add("class", direction + " signout_cls")
            If Not (Request.UrlReferrer Is Nothing) Then
                If Request.UrlReferrer.AbsolutePath.Contains("main.aspx") Or Request.UrlReferrer.AbsolutePath.Contains("mainnew.aspx") Or Request.UrlReferrer.AbsolutePath.Contains("dwb.aspx") Then
                    'lblMsg.Text = lblLoggedout.Text
                    lblMsg.Text = GetLocalResourceObject("lblMsg.Text").ToString()
                Else
                    Response.Redirect(lblCustomerror.Text)
                End If
            End If
            If Session("AxCloudDB") IsNot Nothing Then
                AxCloudDB = Session("AxCloudDB").ToString()
            End If
            If Application("AxCloudDB") IsNot Nothing And AxCloudDB Is Nothing Then
                AxCloudDB = Application("AxCloudDB").ToString()
                Application("AxCloudDB") = ""
            End If

            ''Remove session id cookie
            'Response.Cookies.Add(New System.Web.HttpCookie("ASP.NET_SessionId", ""))
            'Page.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache)
            'Response.Cache.SetNoStore()

            'If Request.Cookies("ASP.NET_SessionId") IsNot Nothing Then
            '    Response.Cookies("ASP.NET_SessionId").Value = String.Empty
            '    Response.Cookies("ASP.NET_SessionId").Expires = DateTime.Now.AddMonths(-20)
            'End If


            If Session("AxEnableOldTheme") IsNot Nothing Then
                EnableOldTheme = Session("AxEnableOldTheme").ToString()
            End If

            If Not IsPostBack Then
                loginStr = Application("LoginPath").ToString()
                If Session("projTitle") Is Nothing Then
                    If Session("AxAppTitle") IsNot Nothing Then
                        appTitle = Session("AxAppTitle").ToString()
                    End If
                Else
                    appTitle = Session("projTitle").ToString()

                End If
                appName = appTitle
            End If

            Dim _xmlString As String =
      "<?xml version=""1.0"" encoding=""utf-8"" ?>"

            If Session("project") Is Nothing Then
                'do nothing
            Else

                'Dim sXml As String
                Dim user As String
                Dim sid As String
                Dim proj As String
                Dim Svrlic_redis As String
                'sXml = ""
                user = Session("user")
                sid = Session("nsessionid")
                proj = Session("project")
                If Session("Svrlic_redis") IsNot Nothing Then
                    Svrlic_redis = Session("Svrlic_redis").ToString()
                End If
                'sXml = sXml & "<root axpapp=" & Chr(34) & Session("project") & Chr(34) & " " & Svrlic_redis & " sessionid= " & Chr(34) & sid & Chr(34) & " trace=" & Chr(34) & Session("trace") & Chr(34) & ">"

                'If Session("axApps") IsNot Nothing Then
                '    sXml &= Session("axApps").ToString()
                'End If
                util.DeleteKeepAliveWebKey()
                util.SaveExecutionText()
                util.DeleteUnwantedKeys()
                util.TempAttaServerFiles()
                util.ClearUserIviewData()
                'sXml &= Application("axProps").ToString() & "</root>"
                Dim res As String = String.Empty
                util.delALLNotificiationKeyfromRedis()
                util.RemoveLoggedUserDetails(proj, sid, user)
                'Call service
                'Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
                'res = objWebServiceExt.CallLogoutWS("Signout", sXml)
                Dim _axApps As String = String.Empty
                Dim _axProps As String = String.Empty
                If Session("axApps") IsNot Nothing Then
                    _axApps = Session("axApps").ToString()
                    _axProps = Application("axProps").ToString()
                ElseIf proj <> "" AndAlso Application(proj & "-axApps") IsNot Nothing Then
                    _axApps = Application(proj & "-axApps").ToString()
                    _axProps = Application("axProps").ToString()
                End If
                _axApps = _axApps.Replace("\", "\\")
                Dim _UserName As String = String.Empty
                If Session("username") IsNot Nothing Then
                    _UserName = Session("username").ToString()
                End If
                Dim signOutJson As String = "{""logout"":{""axpapp"":""" & proj & """,""username"":""" & _UserName & """,""sessionid"":""" & sid & """," & Svrlic_redis & " ""trace"":""" & Session("trace") & """},""axapps"":""" & _axApps & """,""axprops"":""" & _axProps & """}"
                res = ALCClient.CallALCClientLogout(_UserName, signOutJson)

                Dim scriptsPath As String = HttpContext.Current.Application("ScriptsPath").ToString()
                scriptsPath = scriptsPath + "Axpert\\" + sid
                If Directory.Exists(scriptsPath) And sid IsNot Nothing Then
                    Try
                        Directory.Delete(scriptsPath, True)
                    Catch
                        'Do Nothing
                    End Try
                End If

                Dim keys As New ArrayList
                Dim firstCac As String = sid & "order"
                keys.Add(firstCac)

                Dim k As Integer
                For k = 0 To k < keys.Count - 1
                    Cache.Remove(keys(k))
                Next
            End If
            Dim folderPath As String = Server.MapPath("~/images/Custom")
            Dim di As New IO.DirectoryInfo(folderPath)
            Dim diFileinfo As IO.FileInfo() = di.GetFiles()
            Dim drfile As IO.FileInfo
            Dim customlogoexist = "False"
            Dim custommoblogoexist = "False"
            Dim Ismobile = Request.Browser.IsMobileDevice
            'For Each drfile In diFileinfo
            '    If drfile.Length > 0 AndAlso drfile.Name.Contains("homelogo_mob") Then
            '        strFileinfo = drfile.Name
            '        Exit For
            '    End If
            'Next
            'If Ismobile Then
            '    For Each drfile In diFileinfo
            '        '  Dim filename As String = drfile.ToString
            '        If drfile.Length > 0 AndAlso drfile.Name.Contains("homelogo_mob") Then
            '            main_body.Attributes.CssStyle.Add("background", "url(./../images/Custom/" + drfile.Name + "?v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ") no-repeat center center fixed !important")
            '            main_body.Attributes.CssStyle.Add("background-size", "cover !important")
            '            main_body.Attributes.CssStyle.Add("height", "100vh !important")
            '            custommoblogoexist = "True"
            '            Exit For
            '        End If
            '    Next
            '    If custommoblogoexist = "False" AndAlso Ismobile Then
            '        'main_body.Attributes.CssStyle.Add("background", "url(./../AxpImages/homelogo_mob.jpg) no-repeat")
            '        main_body.Attributes.CssStyle.Add("background", "url(./../AxpImages/login-img.png) no-repeat")
            '        main_body.Attributes.CssStyle.Add("background-size", "cover !important")
            '        main_body.Attributes.CssStyle.Add("height", "100vh !important")
            '    End If
            'Else
            'For Each drfile In diFileinfo
            '    '  Dim filename As String = drfile.ToString
            '    If drfile.Length > 0 AndAlso drfile.Name.Contains("homelogo") Then
            '        If drfile.Name.Contains("mp4") Then
            '            main_body.Attributes.CssStyle.Add("background", "")
            '            bgvid.Attributes.CssStyle.Add("display", "block")
            '            bgvidsource.Attributes.Add("src", "./../images/Custom/homelogo.mp4?v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + "")
            '            customlogoexist = "True"
            '            Exit For
            '        Else
            '            main_body.Attributes.CssStyle.Add("background", "url(./../images/Custom/" + drfile.Name + "?v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ") no-repeat center center fixed !important")
            '            main_body.Attributes.CssStyle.Add("background-size", "cover !important")
            '            main_body.Attributes.CssStyle.Add("height", "100vh !important")
            '            customlogoexist = "True"
            '            Exit For
            '        End If
            '    End If
            'Next
            If customlogoexist = "False" Then
                'main_body.Attributes.CssStyle.Add("background", "url(./../AxpImages/homelogo.jpg) center center no-repeat fixed !important")
                'main_body.Attributes.CssStyle.Add("background-size", "cover !important")
                main_body.Attributes.CssStyle.Add("background", "url(./../AxpImages/login-img.png)")
                main_body.Attributes.CssStyle.Add("background-repeat", "no-repeat")
                main_body.Attributes.CssStyle.Add("background-attachment", "fixed")
                main_body.Attributes.CssStyle.Add("background-position", "bottom")
                main_body.Attributes.CssStyle.Add("background-size", "cover !important")
            End If
            'End If
            If AxCloudDB IsNot Nothing Then
                Dim CloudUrl As String = ConfigurationManager.AppSettings("HomeSessExpriesUrl")
                Page.ClientScript.RegisterStartupScript([GetType](), "myrest", "<script>window.parent.location = '" + CloudUrl + "'</script>")
            End If

            util.KillSession() 'Kill this page's session after load
        Catch ex As Exception
            Try
                util.KillSession() 'Kill this page's session after load
            Catch exe As Exception
                Dim sessionStateSection As System.Web.Configuration.SessionStateSection = CType(ConfigurationManager.GetSection("system.web/sessionState"), System.Web.Configuration.SessionStateSection)
                Dim cookieName As String = sessionStateSection.CookieName
                Dim mycookie As HttpCookie = New HttpCookie(cookieName)
                mycookie.Expires = DateTime.Now.AddDays(-1)
                HttpContext.Current.Response.Cookies.Add(mycookie)
                HttpContext.Current.Session.Abandon()
            End Try
        End Try
    End Sub

End Class
