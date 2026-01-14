Imports System.Data
Imports System.Xml
Imports System.Xml.Xsl
Imports System.IO
Imports Constants

Partial Class excel
    Inherits System.Web.UI.Page

    Dim _xmlString As String =
   "<?xml version=""1.0"" encoding=""utf-8"" ?>"
    Dim colHide As New ArrayList()
    Dim colFld As New ArrayList()
    Dim colHead As New ArrayList()
    Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
    Dim iname As String
    Dim user As String = String.Empty
    Dim sid As String
    Dim ivtype As String
    Dim proj As String
    Dim paramVal As String
    Dim paramXml As String = ""
    Dim logobj As LogFile.Log = New LogFile.Log()
    Dim util As Util.Util = New Util.Util()
    Dim filename As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.UrlReferrer IsNot Nothing Then
            If Not (Request.UrlReferrer.AbsolutePath.ToLower().Contains("iview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("listiview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("saveas.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("excel.aspx")) Then
                Response.Redirect("../CusError/AxCustomError.aspx")
            End If
        End If
        If Not IsPostBack Then
            iname = Request.QueryString("ivname")
            user = Session("user")
            sid = Session("nsessionid")
            ivtype = Request.QueryString("ivtype")
            ivtype = Request.QueryString("ivtype")
            paramVal = Request.QueryString("params")

            If Not iname Is Nothing Then
                If Not util.IsValidIvName(iname) Then
                    Response.Redirect(Constants.PARAMERR)
                End If
            End If


            If Not ivtype Is Nothing Then
                If Not ivtype Is Nothing And Not util.IsChar(ivtype) Then
                    Response.Redirect(Constants.PARAMERR)
                End If
            End If

            If iname = Nothing And Session("ivname") <> Nothing Then
                iname = Session("ivname").ToString()
            End If
            If paramVal = Nothing And Session("params") <> Nothing Then
                paramVal = Session("params").ToString()
            End If
            If ivtype = Nothing And Session("ivtype") <> Nothing Then
                ivtype = Session("ivtype").ToString()
            End If
            ViewState("ivtype") = ivtype
            proj = Session("project")
            ViewState("iname") = iname
            ViewState("ivtype") = ivtype
            ViewState("proj") = proj
            ViewState("user") = user
            ViewState("sid") = sid
            ViewState("params") = paramVal
        Else
            user = ViewState("user")
            iname = ViewState("iname")
            ivtype = ViewState("ivtype")
            proj = ViewState("proj")
            sid = ViewState("sid")
            paramVal = ViewState("params")
            ivtype = ViewState("ivtype")
        End If


        Dim isList As String
        If ivtype = "listview" Or ivtype = "lview" Then
            isList = "true"
        Else
            isList = "false"
        End If

        If (isList = "false") Then
            GetParam(paramVal)
        End If

        Dim sortCol As String = String.Empty
        Dim sortOrd As String = String.Empty
        Dim filterCol As String = String.Empty
        Dim filterColVal As String = String.Empty
        Dim filterValue1 As String = String.Empty
        Dim filterOpr As String = String.Empty

        If Session("sOrder") <> Nothing Then
            If Session("sOrder").ToString() <> "" Then
                sortOrd = Session("sOrder").ToString()
            End If
        End If

        If Session("sColumn") <> Nothing Then
            If Session("sColumn").ToString() <> "" Then
                sortCol = Session("sColumn").ToString()
            End If
        End If

        If Session("fcolopr") <> Nothing Then
            If Session("fcolopr").ToString() <> "" Then
                filterOpr = Session("fcolopr").ToString()
            End If
        End If

        If Session("fCol") <> Nothing Then
            If Session("fCol").ToString() <> "" Then
                filterCol = Session("fCol").ToString()
            End If
        End If

        If Session("fColVal") <> Nothing Then
            If Session("fColVal").ToString() <> "" Then
                filterColVal = Session("fColVal").ToString()
            End If
        End If

        If Session("fColVal2") <> Nothing Then
            If Session("fColVal2").ToString() <> "" Then
                filterValue1 = Session("fColVal2").ToString()
            End If
        End If

        Dim colorNode As String = ""
        Dim fname As String = "ExcelColors.xml"
        If File.Exists(Server.MapPath("./ivxml/" & fname)) Then
            Dim ad As String = File.ReadAllText(Server.MapPath("./ivxml/" & fname))
            Dim xmlDoc As New XmlDocument
            Dim productNodes As XmlNodeList
            Dim productNode As XmlNode
            xmlDoc.LoadXml(ad)
            productNodes = xmlDoc.SelectNodes("//root")
            For Each productNode In productNodes
                colorNode = productNode.InnerXml()
            Next
        End If

        'colorNode = "#3B5999"
        Dim ivxml As String = ""
        ivxml = ViewState("ivxml")
        If (ivxml <> "") Then
            ivxml = ivxml.Replace("<data>", "<data>" & colorNode)
        End If

        Dim errorLog As String = logobj.CreateLog("calling SaveAsExcel", sid, filename, "new")
        filename = "SaveAsExcel"
        errorLog = logobj.CreateLog("SaveAsExcel", sid, filename, "new")
        logobj.CreateLog("Call to SaveAsExcel Web Service ", sid, filename, "")
        logobj.CreateLog("Start Time " & Now.ToString(), sid, filename, "")
        logobj.CreateLog("", sid, filename, "")

        Dim finalexcelxml As String = ""
        finalexcelxml = "<root stype=" & Chr(34) & ivtype & Chr(34) & " sname=" & Chr(34) & ViewState("iname") & Chr(34) & " fname=" & Chr(34) & ViewState("iname") & ".xls" & Chr(34) & " name =" & Chr(34) & ViewState("iname") & Chr(34) & " axpapp =" & Chr(34) & Session("project") & Chr(34) & " sessionid =" & Chr(34) & Session("nsessionid") & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " trace = " & Chr(34) & errorLog & Chr(34) & " pageno=""0"" firsttime=""yes"" sorder=""" & sortOrd & """ scol=""" & sortCol & """ fcolopr=""" + filterOpr + """ fcolnm=""" & filterCol & """ fcolval1=""" + filterColVal + """ fcolval2=""" + filterValue1 + """><params>" & paramXml & "</params>"
        finalexcelxml &= Session("axApps").ToString() + Application("axProps").ToString() + Session("axGlobalVars").ToString() + Session("axUserVars").ToString() & "</root>"
        Dim result As String = String.Empty
        'Call service 
        result = objWebServiceExt.CallSaveAsExcelWS(ivtype, finalexcelxml)

        Dim strErr = String.Empty
        strErr = util.ParseXmlErrorNode(result)

        If strErr <> String.Empty Then
            If strErr = Constants.SESSIONERROR Then
                Session.RemoveAll()
                Session.Abandon()
                Dim url1 As String
                url1 = util.SESSEXPIRYPATH
                Response.Write("<script>" & vbCrLf)
                Response.Write("parent.parent.location.href='" & url1 & "';")
                Response.Write(vbCrLf & "</script>")
            Else
                Response.Redirect(util.ERRPATH + strErr)
            End If

        Else

            Dim index As Integer = result.IndexOf("Axpert\")
            result = result.Substring(index + 7)
            Dim ScriptsPath As String = Application("ScriptsPath")
            Dim flpath As String = ScriptsPath & "Axpert\" & result
            Dim files As System.IO.FileInfo = New System.IO.FileInfo(flpath) '-- if the file exists on the server
            If files.Exists Then 'set appropriate headers
                Response.Clear()
                Response.AddHeader("Content-Disposition", "attachment; filename=" & files.Name)
                Response.AddHeader("Content-Length", files.Length.ToString())
                Response.ContentType = "application/x-download"
                Response.WriteFile(files.FullName)
                Response.End() 'if file does not exist
            Else
                Response.Write("This file does not exist.")
            End If 'nothing in the URL as HTTP GET
        End If

    End Sub

    Private Sub GetParam(ByVal param As String)
        Dim k As Integer

        Dim arrParamCaption As New ArrayList
        Dim arrParamIsHidden As New ArrayList
        If Session("paramDetails") <> Nothing Then
            Dim strParamdetails = Session("paramDetails").ToString()
            Dim arrParamDet As Array
            arrParamDet = strParamdetails.Split("~")
            Dim arrTemp As Array
            Dim i As Integer
            For i = 0 To arrParamDet.Length - 1
                If (arrParamDet(i).ToString() <> String.Empty) Then
                    arrTemp = arrParamDet(i).ToString().Split(",")
                    arrParamCaption.Insert(i, arrTemp(0).ToString())
                    arrParamIsHidden.Insert(i, arrTemp(1).ToString())
                End If
            Next
        End If

        Dim tem1 As String
        Dim arrParams As Array
        Dim arrNoOfParams As Array
        Dim params As String
        Dim arrLstParams As New ArrayList
        Dim arrLstParamVal As New ArrayList
        If Not paramVal Is Nothing Then
            If paramVal.Contains("~") Then
                arrNoOfParams = paramVal.Split("~")
                Dim i As Integer
                For i = 0 To arrNoOfParams.Length - 1
                    arrParams = arrNoOfParams(i).ToString().Split(",")
                    arrLstParams.Insert(i, arrParams(0))
                    Dim appStr = arrParams(1)
                    appStr = appStr.Replace(Chr(10), " ")
                    If appStr.Contains("`") = True Then
                        appStr = appStr.Replace("`", ",")
                    End If

                    arrLstParamVal.Insert(i, appStr)
                Next
            Else
                arrParams = paramVal.Split(",")
                For k = 0 To arrParams.Length - 1
                    If arrParams.Length > k + 1 Then
                        arrLstParams.Insert(k, arrParams(0))
                        Dim appStr = arrParams(1)
                        If appStr.Contains("`") = True Then
                            appStr = appStr.Replace("`", ",")
                        End If
                        arrLstParamVal.Insert(k, appStr)
                    End If
                Next
            End If
        End If

        Dim j As Integer
        For j = 0 To arrLstParams.Count - 1
            If arrLstParams.Count > 0 Then
                tem1 = arrLstParams(j).ToString()
                If arrLstParams.Count > 0 Then
                    paramVal = arrLstParamVal(j).ToString()
                Else
                    paramVal = ""
                End If
            End If

            If param <> "" Then
                paramXml = paramXml & "<" & tem1 & " caption='" & arrParamCaption(j) & "' hide='" & arrParamIsHidden(j) & "' >"
                paramXml = paramXml & paramVal
                paramXml = paramXml & "</" & tem1 & ">"

                params = params & "&" & tem1 & "="
                params = params & paramVal
            Else
                paramXml = "<params>"
            End If

        Next
    End Sub

End Class
