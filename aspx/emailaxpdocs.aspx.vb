Imports System.Xml

Partial Class emailaxpdocs
    Inherits System.Web.UI.Page
    Public chkVal As String = String.Empty
    Dim _xmlString As String =
               "<?xml version=""1.0"" encoding=""utf-8"" ?>"
    Dim sname As String
    Dim stype As String
    Dim recid As String
    Public direction As String = "ltr"
    Dim util As New Util.Util()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("project") = "" Then
            Dim url As String
            url = util.SESSEXPIRYPATH
            Response.Write("<script>" & vbCrLf)
            Response.Write("parent.parent.location.href='" & url & "';")
            Response.Write(vbCrLf & "</script>")
        Else
            Dim user As String
            Dim sid As String
            user = Session("user")
            sid = Session("nsessionid")
            If Session("language").ToString() = "ARABIC" Then
                direction = "rtl"
            End If
            sname = Request.QueryString("sname")
            stype = Request.QueryString("stype")
            recid = Request.QueryString("recordid")

            ' For taking related documents
            Dim rdiXml As String
            rdiXml = ""

            rdiXml = "<root sname =""" & sname & """ axpapp = """ & Session("project") & """ sessionid = """ & sid & """ appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " stype =""" & stype & """  trace=" & Chr(34) & Session("trace") & Chr(34) & " recordid=""" & recid & """ >"
            rdiXml += Session("axGlobalVars").ToString() + Session("axUserVars").ToString() + "</root>"

            Dim rdires As String = String.Empty
            Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()

            'Call service 
            rdires = objWebServiceExt.CallGetDocListWS(sname, rdiXml)

            Dim errMsg As String = String.Empty
            errMsg = util.ParseXmlErrorNode(rdires)

            If errMsg <> String.Empty Then
                If errMsg = Constants.SESSIONERROR Then
                    Session.RemoveAll()
                    Session.Abandon()
                    Dim url1 As String
                    url1 = util.SESSEXPIRYPATH
                    Response.Write("<script>" & vbCrLf)
                    Response.Write("parent.parent.location.href='" & url1 & "';")
                    Response.Write(vbCrLf & "</script>")
                Else
                    If errMsg.ToLower().Contains("microsoft") Then
                        Response.Redirect(util.ERRPATH + "Required driver not found")
                    End If
                    Response.Redirect(util.ERRPATH + errMsg)
                End If
            Else
                rdires = _xmlString & rdires


                Dim gcxmlDoc As New XmlDocument
                Dim gcproductNodes As XmlNodeList
                Dim gcproductNode As XmlNode
                Dim gcbaseDataNodes As XmlNodeList

                gcxmlDoc.LoadXml(rdires)

                gcproductNodes = gcxmlDoc.SelectNodes("/root")
                For Each gcproductNode In gcproductNodes
                    gcbaseDataNodes = gcproductNode.ChildNodes
                    For Each gcbaseDataNode As XmlNode In gcbaseDataNodes
                        chkVal = chkVal & "<tr><td class=""rowbor""><input type=checkbox value=""" & gcbaseDataNode.InnerText & """>" & gcbaseDataNode.InnerText & "</td></tr>"
                    Next
                Next
            End If
        End If
    End Sub
End Class
