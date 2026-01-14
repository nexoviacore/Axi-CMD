Imports System.IO
Imports System.Xml

Partial Class useracciview
    Inherits System.Web.UI.Page
    Dim logobj As LogFile.Log = New LogFile.Log()
    Dim util As Util.Util = New Util.Util()
    Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
    Public direction As String = "ltr"
    Public langType As String = "en"

    Public isCloudApp As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        util.IsValidSession()
        ResetSessionTime()
        If Session("project") = "" Then
            Dim url As String
            url = util.SESSEXPIRYPATH
            Response.Write("<script>" & vbCrLf)
            Response.Write("if(window.opener && !window.opener.closed){window.opener.parent.location.href='" + url + "';window.close();}else {parent.parent.location.href='" + url + "';}")
            Response.Write(vbCrLf & "</script>")
        Else

            If Not IsPostBack() Then
                SetFieldValues()
            End If
        End If
        If util.IsValidQueryString(Request.RawUrl) = False Then
            Response.Redirect(util.ERRPATH + Constants.INVALIDURL)
        End If
        If (ConfigurationManager.AppSettings("isCloudApp") <> Nothing) Then
            isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings("isCloudApp").ToString())
        End If

        Dim filcustom As FileInfo
        filcustom = New FileInfo(HttpContext.Current.Server.MapPath("~/Js/lang/content-" + langType + ".js"))
        If Not (filcustom.Exists) Then
            direction = "ltr"
            langType = "en"
        End If
    End Sub

    Protected Sub SetFieldValues()
        Dim iname As String
        iname = Request.QueryString("iname")

        Dim selrole As String
        selrole = Request.QueryString("role")

        If Session("language").ToString() = "ARABIC" Then
            direction = "rtl"
        End If
        Dim errlog As String = logobj.CreateLog("Calling GetUserAccessIviewDetails web service", Session("nsessionid").ToString(), "GetUserAccessIviewDetails", "new")
        Dim detiXml As String
        detiXml = "<root axpapp=""" & Session("project") & """ sessionid= " & Chr(34) & Session("nsessionid") & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " trace=""" & errlog & """ ivname=""" & iname & """ user=""" & Session("user") & """ role=""" & selrole & """>"
        detiXml &= Session("axApps").ToString() + Application("axProps").ToString() & Session("axGlobalVars").ToString() & Session("axUserVars").ToString() & "</root>"
        Dim res As String = String.Empty
        'Call service
        res = objWebServiceExt.CallGetUserAccessIviewDetailsWS(iname, detiXml)

        Dim strErrMsg As String = String.Empty
        strErrMsg = util.ParseXmlErrorNode(res)

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
                strErrMsg = Regex.Replace(strErrMsg, "'", "\""")
                ClientScript.RegisterStartupScript([GetType](), "name", "<script language=""javascript""> showAlertDialog(""error"", """ & strErrMsg & """);</script>")
            End If
        Else

            Dim xmldoc As New XmlDocument
            xmldoc.LoadXml(res)

            Dim butroots As XmlNodeList
            Dim prnodebuts As XmlNode
            Dim baseDataNodesButs As XmlNodeList
            butroots = xmldoc.SelectNodes("//root/buttons")

            Dim butRes As String = ""
            Dim butrs As Integer = 1
            Dim nbutrs As String = "001"
            For Each prnodebuts In butroots
                baseDataNodesButs = prnodebuts.ChildNodes
                For Each baseDataNodebut As XmlNode In baseDataNodesButs

                    Dim butname As String = ""
                    Dim butview As String = ""
                    Dim butenable As String = ""
                    Dim nodebutname As XmlNode = baseDataNodebut.Attributes("cap")
                    If (nodebutname Is Nothing) Then
                        butname = baseDataNodebut.Name
                    Else
                        butname = nodebutname.Value
                    End If

                    Dim nodebutview As XmlNode = baseDataNodebut.Attributes("view")
                    If (nodebutview Is Nothing) Then
                        butview = ""
                    Else
                        butview = nodebutview.Value
                    End If

                    Dim nodebutenable As XmlNode = baseDataNodebut.Attributes("enable")
                    If (nodebutenable Is Nothing) Then
                        butenable = ""
                    Else
                        butenable = nodebutenable.Value
                    End If

                    Dim butview_cb As String

                    butview_cb = "<input class=""w-15px h-15px"" id=""butview" & nbutrs & """ data-btn-view=" & nbutrs & """  type=""checkbox"" value=""" & baseDataNodebut.Name & """"
                    If butview = "true" Then
                        butview_cb = butview_cb & "  checked=""true""  />"
                    Else
                        butview_cb = butview_cb & "   />"
                    End If


                    Dim butenable_cb As String
                    butenable_cb = "<input class=""w-15px h-15px"" id=""butenable" & nbutrs & """ data-btn-enable=" & nbutrs & """  type=""checkbox"" value=""" & butname & """"
                    If butview = "true" And butenable = "true" Then
                        butenable_cb = butenable_cb & "  checked=""true""  />"
                    Else
                        butenable_cb = butenable_cb & "   />"
                    End If

                    butrs = butrs + 1
                    If butrs.ToString.Length = 1 Then
                        nbutrs = "00" & butrs
                    ElseIf butrs.ToString.Length = 2 Then
                        nbutrs = "0" & butrs
                    ElseIf butrs.ToString.Length = 3 Then
                        nbutrs = butrs
                    End If
                    butRes = butRes & "<tr class=""tab_row""><td>" & butname & "</td><td class=""text-center"">" & butview_cb & "</td><td class=""text-center"">" & butenable_cb & "</td></tr>"
                Next
            Next
            butRes = "<table class='table table-sm table-row-bordered w-100 gridData'><thead><tr class=""text-center""><th id='thButtonName'>Button Name</th><th class='view'>View</th><th class='enable'>Enable</th></tr></thead>" & butRes & "</table>"

            but_div.InnerHtml = butRes
            save.Attributes.Add("onclick", "javascript:setAccessValues();")
        End If
    End Sub

    Protected Sub save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save.Click
        Dim ixml As String

        ixml = "<buttons>" & but_xml.Value & "</buttons>"

        Dim iname As String
        iname = Request.QueryString("iname")
        Dim selrole As String
        selrole = Request.QueryString("role")
        Dim errlog As String = logobj.CreateLog("Calling SaveAccessIviewDetails web service", Session("nsessionid"), "SaveAccessIviewDetails", "")
        ixml = "<root axpapp=""" & Session("project") & """ user=""" & Session("user") & """ sessionid= " & Chr(34) & Session("nsessionid") & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " trace=""" & errlog & """ ivname=""" & iname & """ role=""" & selrole & """> " & ixml
        ixml &= Session("axApps").ToString() & Application("axProps").ToString() & Session("axGlobalVars").ToString() & Session("axUserVars").ToString() & "</root>"
        'setbutval()
        Dim res As String = String.Empty
        'Call service
        res = objWebServiceExt.CallSaveAccessIviewDetailsWS(iname, ixml)

        Dim strErrMsg As String = String.Empty
        strErrMsg = util.ParseXmlErrorNode(res)

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
                strErrMsg = Regex.Replace(strErrMsg, "'", "\""")
                ClientScript.RegisterStartupScript([GetType](), "name", "<script language=""javascript""> showAlertDialog(""error"", """ & strErrMsg & """);</script>")
            End If
        Else
            ClientScript.RegisterStartupScript([GetType](), "name", "<script language=""javascript"">showAlertDialog(""success"", eval(callParent('lcm[339]')));</script>")
        End If
        SetFieldValues()
    End Sub

    Private Sub setbutval()
        If but_xml.Value <> "" Then
            Dim xmldoc As New XmlDocument
            Dim res As String
            res = "<root><buttons>" & but_xml.Value & "</buttons></root>"
            xmldoc.LoadXml(res)


            Dim butroots As XmlNodeList
            Dim prnodebuts As XmlNode
            Dim baseDataNodesButs As XmlNodeList
            butroots = xmldoc.SelectNodes("//root/buttons")

            Dim butRes As String = ""
            Dim butrs As Integer = 1
            Dim nbutrs As String = "001"
            For Each prnodebuts In butroots
                baseDataNodesButs = prnodebuts.ChildNodes
                For Each baseDataNodebut As XmlNode In baseDataNodesButs

                    Dim butname As String = ""
                    Dim butview As String = ""
                    Dim butenable As String = ""
                    Dim nodebutname As XmlNode = baseDataNodebut.Attributes("cap")
                    If (nodebutname Is Nothing) Then
                        butname = baseDataNodebut.Name
                    Else
                        butname = nodebutname.Value
                    End If

                    Dim nodebutview As XmlNode = baseDataNodebut.Attributes("view")
                    If (nodebutview Is Nothing) Then
                        butview = ""
                    Else
                        butview = nodebutview.Value
                    End If

                    Dim nodebutenable As XmlNode = baseDataNodebut.Attributes("enable")
                    If (nodebutenable Is Nothing) Then
                        butenable = ""
                    Else
                        butenable = nodebutenable.Value
                    End If

                    Dim butview_cb As String

                    butview_cb = "<input class=""w-15px h-15px"" id=""butview" & nbutrs & """ data-btn-view=" & nbutrs & """  type=""checkbox"" value=""" & butname & """"
                    If butview = "true" Then
                        butview_cb = butview_cb & "  checked=""true""  />"
                    Else
                        butview_cb = butview_cb & "   />"
                    End If


                    Dim butenable_cb As String
                    butenable_cb = "<input class=""w-15px h-15px"" id=""butenable" & nbutrs & """ data-btn-enable=" & nbutrs & """  type=""checkbox"" value=""" & butname & """"
                    If butenable = "true" Then
                        butenable_cb = butenable_cb & "  checked=""true""  />"
                    Else
                        butenable_cb = butenable_cb & "   />"
                    End If

                    butrs = butrs + 1
                    If butrs.ToString.Length = 1 Then
                        nbutrs = "00" & butrs
                    ElseIf butrs.ToString.Length = 2 Then
                        nbutrs = "0" & butrs
                    ElseIf butrs.ToString.Length = 3 Then
                        nbutrs = butrs
                    End If
                    butRes = butRes & "<tr class=""tab_row""><td>" & butname & "</td><td align=""center"">" & butview_cb & "</td><td align=""center"">" & butenable_cb & "</td></tr>"
                Next
            Next
            butRes = "<table class='table table-sm table-row-bordered w-100 gridData'><thead><tr class=""text-center""><th id='thButtonName'>Button Name</th><th class='view'>View</th><th class='enable'>Enable</th></tr></thead>" & butRes & "</table>"

            but_div.InnerHtml = butRes
        End If

    End Sub

    Private Sub ResetSessionTime()
        If Session("AxSessionExtend") IsNot Nothing AndAlso Session("AxSessionExtend").ToString() = "true" Then
            ClientScript.RegisterStartupScript(Me.[GetType](), "SessionAlert", "eval(callParent('ResetSession()', 'function'));", True)
        End If
    End Sub
End Class
