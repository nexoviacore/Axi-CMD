Imports System.Xml

Partial Class pdfparams
    Inherits System.Web.UI.Page

    Dim str As String
    Dim util As New Util.Util()
    Public optStr As String = String.Empty
    Public EnableOldTheme As String
    Public ptransid As String = String.Empty
    Public direction As String = "ltr"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.UrlReferrer IsNot Nothing Then
            If Not (Request.UrlReferrer.AbsolutePath.ToLower().Contains("iview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("tstruct.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("pdfparams.aspx")) Then
                Response.Redirect("../CusError/AxCustomError.aspx")
            End If
        End If
        If Session("AxEnableOldTheme") IsNot Nothing Then
            EnableOldTheme = Session("AxEnableOldTheme").ToString().ToLower()
        End If

        str = Application("ScriptsurlPath").ToString()
        hdnScriptspath.Value = str
        Dim _xmlString As String =
       "<?xml version=""1.0"" encoding=""utf-8"" ?>"

        Dim user As String
        Dim sid As String
        Dim proj As String
        proj = Session("project")
        user = Session("user")
        sid = Session("nsessionid")
        Dim ivname As String
        ivname = Request.QueryString("sname")

        If Not util.IsValidIvName(ivname) Then
            Response.Redirect(Constants.PARAMERR)
        End If

        Dim recid As String
        recid = Request.QueryString("recid")

        If Not util.IsNumber(recid) Then
            Response.Redirect(Constants.PARAMERR)
        End If

        Dim stype As String
        stype = Request.QueryString("stype")
        If Not util.IsAlphaNum(stype) Then
            Response.Redirect(Constants.PARAMERR)
        End If

        If Session("language").ToString() = "ARABIC" Then
            direction = "rtl"
        End If

        Response.Write("<Script language=javascript>")
        Response.Write("var proj = '" & Session("project") & "';var user='" & user & "';var sid='" & sid & "';var ivna='" & ivname & "';var rid='" & recid & "';var gl_language = '" & Session("language").ToString() & "';var trace='" & Session("trace") & "';")
        Response.Write("</script>")

        Dim logobj As LogFile.Log = New LogFile.Log()
        Dim ires As String = String.Empty
        Dim iXml As String = String.Empty
        Dim fileName As String = "GetPDFList" + ivname
        Dim errorLog As String = logobj.CreateLog("GetPDFList.", sid, fileName, "new")
        iXml = "<root sname =""" & ivname & """ axpapp = """ & Session("project") & """ sessionid = """ & sid & """ appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " stype =""" & stype & """  trace=""" & errorLog & """ >"
        iXml += Session("axApps").ToString() & Application("axProps").ToString() & Session("axGlobalVars").ToString() & Session("axUserVars").ToString() & "</root>"
        'Call service
        Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
        ires = objWebServiceExt.CallGetPDFListWS(ivname, iXml)

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
            ires = _xmlString & ires

            Dim xmlDoc As New XmlDocument
            xmlDoc.LoadXml(ires)
            ptransid = xmlDoc.SelectSingleNode("/root/transid").InnerText


            Dim gcxmlDoc As New XmlDocument
            Dim gcproductNodes As XmlNodeList
            Dim gcproductNode As XmlNode
            Dim gcbaseDataNodes As XmlNodeList


            gcxmlDoc.LoadXml(ires)

            gcproductNodes = gcxmlDoc.SelectNodes("/root")
            For Each gcproductNode In gcproductNodes
                gcbaseDataNodes = gcproductNode.ChildNodes
                'Response.Write(gcproductNode.Name & "<br>")
                Dim tNo As Integer
                tNo = 1
                For Each gcbaseDataNode As XmlNode In gcbaseDataNodes
                    '  Response.Write(gcbaseDataNode.InnerText & "<br>")
                    If tNo = 1 Then
                        optStr = optStr ' to eleiminate first child transid from ires xml
                    Else
                        optStr = optStr & "<option value=""" & gcbaseDataNode.Attributes("source").Value & """>" & gcbaseDataNode.InnerText & "</option>"
                    End If
                    tNo = tNo + 1
                Next
            Next
        End If

    End Sub

End Class
