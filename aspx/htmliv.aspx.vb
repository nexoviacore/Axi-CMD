Imports System.Xml
Imports System.Text
Imports System.IO
Imports System.Drawing
Imports System.Data
'Imports System.Collections.Generic

Partial Class htmliv
    Inherits System.Web.UI.Page

    Public sb As StringBuilder = New StringBuilder()
    Dim util As Util.Util = New Util.Util()
    Dim iname As String
    Dim headerText As String = ""
    Dim user As String
    Dim sid As String
    Dim ivtype As String
    Dim proj As String
    Dim paramVal As String = ""
    Dim caption As String = ""
    Dim isPrint As String
    Dim isFromHome As Boolean = False
    Dim ivhead As New ArrayList()
    Dim arrLstParams As New ArrayList
    Dim arrLstParamVal As New ArrayList
    Dim _xmlString As String =
                   "<?xml version=""1.0"" encoding=""utf-8"" ?>"
    Dim logobj As LogFile.Log = New LogFile.Log()
    Dim utilObj As Util.Util = New Util.Util()
    Dim colHide As New ArrayList()
    Dim colFld As New ArrayList()
    Dim colHead As New ArrayList()
    Dim paramXml As String = ""
    Dim pivotGroupHeaderNames As New ArrayList()
    Dim pivotStartCol As New ArrayList()
    Dim pivotEndCol As New ArrayList()
    Dim htmlColumns As New ArrayList()
    Dim objIview
    Dim listres As String = String.Empty
    Dim isperf As String = "false"
    Dim AxPrintTitle As String = String.Empty
    Dim AxPrintTitleAlign As String = String.Empty
    Dim AdvancedConfig As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Ikey As String = String.Empty

        If Request.UrlReferrer IsNot Nothing Then
            'If Not (Request.UrlReferrer.AbsolutePath.ToLower().Contains("iview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("listiview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("saveas.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("htmliv.aspx")) Then
            '    Response.Redirect("../CusError/AxCustomError.aspx")
            'End If
        End If
        If Session("project") = "" Then
            Response.Redirect("sess.aspx")
        Else
            If Session("ivKey") <> Nothing Then
                Ikey = Session("ivKey").ToString()
            Else
                If Not Request.QueryString("ivKey") Is Nothing Then
                    Ikey = Request.QueryString("ivKey")
                End If
            End If
            If Session("AxPrintTitle") <> Nothing Then
                If Session("AxPrintTitle").ToString() <> "" Then
                    AxPrintTitle = Session("AxPrintTitle").ToString()
                End If
            End If
            If Session("AxPrintTitleAlign") <> Nothing Then
                If Session("AxPrintTitleAlign").ToString() <> "" Then
                    AxPrintTitleAlign = Session("AxPrintTitleAlign").ToString()
                End If
            End If
            If Application("advancedConfig") <> Nothing Then
                AdvancedConfig = Application("advancedConfig")
            End If
            If Session("AxShowAppTitle") <> Nothing And Session("AxShowAppTitle").ToString().ToLower() = "true" Then
                If Session("AxAppTitle") <> Nothing AndAlso Session("AxAppTitle") <> String.Empty Then
                    headerText = Session("AxAppTitle").ToString()
                ElseIf Session("projTitle") <> Nothing Then
                    headerText = Session("projTitle").ToString()
                End If
            End If

            ' Set Mime Type 
            ' Push the Data to the client. 
            If Not IsPostBack Then
                iname = Request.QueryString("ivname")
                isPrint = Request.QueryString("isPrint")
                user = Session("user")
                sid = Session("nsessionid")
                ivtype = Request.QueryString("ivtype")
                If Not Request.QueryString("axpCache") Is Nothing Then
                    paramVal = Session("AxIvExportParams-" + iname).ToString()
                    Session("AxIvExportParams-" + iname) = ""
                Else
                    paramVal = Request.QueryString("params")
                End If
                If Not Request.QueryString("AxFromhome") Is Nothing Then
                    isFromHome = Request.QueryString("AxFromhome")
                End If


                If Not iname Is Nothing Then
                    If Not utilObj.IsValidIvName(iname) Then
                        Response.Redirect(Constants.PARAMERR)
                    End If
                End If

                If Not ivtype Is Nothing Then
                    If Not ivtype Is Nothing And Not utilObj.IsChar(ivtype) Then
                        Response.Redirect(Constants.PARAMERR)
                    End If
                End If

                If Not isPrint Is Nothing Then
                    If Not utilObj.IsChar(isPrint) Then
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

                ' If ivtype = "listview" Or ivtype = "lview" Then
                '     objIview = New ListviewData()
                '     objIview = CType(Session(Ikey), ListviewData)
                ' Else
                objIview = New IviewData()
                objIview = CType(Session(Ikey), IviewData)
                ' End If

                proj = Session("project")
                ViewState("iname") = iname
                ViewState("ivtype") = ivtype
                ViewState("proj") = proj
                ViewState("user") = user
                ViewState("sid") = sid
                ViewState("params") = paramVal
                ViewState("isPrint") = isPrint
            Else
                user = ViewState("user")
                iname = ViewState("iname")
                ivtype = ViewState("ivtype")
                proj = ViewState("proj")
                sid = ViewState("sid")
                paramVal = ViewState("params")
                isPrint = ViewState("isPrint")
            End If

            Dim encoding As New System.Text.UTF8Encoding()
            If isPrint = "true" Then
                Dim buffer() As Byte = encoding.GetBytes(GetHtml())
                Response.BinaryWrite(buffer)
                Response.End()
            Else
                Dim buffer() As Byte = encoding.GetBytes(GetHtml())
                Response.Clear()
                Response.ContentType = "text/html"
                Response.AppendHeader("Content-Disposition", "inline;filename=filename.htm")
                Response.Charset = ""
                Response.BinaryWrite(buffer)
                Response.End()
            End If
        End If


    End Sub


    Private Function GetHtml() As String
        Dim isGrandTotal As Boolean = False
        Dim isList As String
        If ivtype = "listview" Or ivtype = "lview" Then
            isList = "true"
        Else
            isList = "false"
        End If
        GetParam(paramVal)
        Dim fileName As String = "GetLView" & iname
        Dim errorLog As String = logobj.CreateLog("GetLView.", sid, fileName, "new")

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

        Dim custXML As String = ""
        If isList = "true" Then
            'if custom view is selected then passing custom view name in the input xml
            Try
                If Not objIview.CustomView Is String.Empty Then
                    custXML = " custviewname =""" & objIview.CustomView & """"
                    'custviewname="testing1"
                End If
            Catch ex As Exception
            End Try
        End If

        Dim iXml As String = String.Empty
        iXml = "<root name=""" & iname & """" & custXML & " axpapp =""" & proj & """ sessionid= """ & sid & """ appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " trace=" & Chr(34) & errorLog & Chr(34) & " pageno=""0"" firsttime=""yes"" sorder=""" & sortOrd & """ scol=""" & sortCol & """ fcolopr=""" + filterOpr + """ fcolnm=""" & filterCol & """ fcolval1=""" + filterColVal + """ fcolval2=""" + filterValue1 + """ pforms=""true"" ><params> "
        iXml = iXml & paramXml
        iXml = iXml & "</params>"
        iXml &= Session("axApps").ToString() + Application("axProps").ToString() + Session("axGlobalVars").ToString() + Session("axUserVars").ToString() & "</root>"
        Dim ires As String = String.Empty
        If isFromHome Then
            objIview = New IviewData()
            objIview.IsDirectDBcall = False
            objIview.FilterXml = ""
            objIview.colFld = New ArrayList
            objIview.ColNoPrint = New ArrayList
            objIview.HiddenCols = New ArrayList
            objIview.ShowHiddengridCols = New ArrayList
            objIview.ReportHdrs = New ArrayList
            objIview.ReportFtrs = New ArrayList
            objIview.ColHide = New ArrayList
            objIview.HeadName = New ArrayList
        End If
        objIview.FilterXml = ""
        If objIview.IsDirectDBcall = True Then
            Dim ds As DataSet = New DataSet()
            Dim objdb As DBContext = New DBContext()
            ds = objdb.GetIviewDataDB(objIview.SqlQuery, 0, 0, objIview.IsGrandTotal)
            Dim gridPageSize As Integer
            If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then
                gridPageSize = ds.Tables(0).Rows.Count
            End If
            ds = objIview.ApplySubTotal(ds, True, gridPageSize)
            Return DataTable2String(ds.Tables(0))
        Else
            'Call service
            Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
            Dim resultTable As System.Data.DataTable = New System.Data.DataTable()
            If ivtype = "listview" Or ivtype = "lview" Then
                If objIview.FilterXml Is String.Empty Then                 'Checking for filtering in listview
                    ires = objWebServiceExt.CallGetLViewWS(iname, iXml, objIview.WebServiceTimeout)

                Else
                    If objIview.Listres IsNot Nothing Then
                        listres = objIview.Listres
                    End If
                    ires = objIview.FilterXml.ToString()
                    Dim index As Integer = -1
                    index = ires.IndexOf(">")
                    ires = ires.Insert(index + 1, listres)
                End If
            Else
                If Session("FilteredData") Is Nothing Then
                    objIview.requestJSON = False
                    ires = objWebServiceExt.CallGetIViewWS(iname, iXml, "", objIview)
                    If ires IsNot Nothing Then
                        ires = ires.Split("♠")(1)
                    End If
                    Dim xmlDoc As New XmlDocument
                    Dim compCheckPerf As XmlNode
                    xmlDoc.LoadXml(ires)
                    compCheckPerf = xmlDoc.SelectSingleNode("//data")
                    If Not compCheckPerf Is Nothing Then
                        If Not compCheckPerf.Attributes("perfxml") Is Nothing Then
                        End If
                    End If


                Else
                    Dim dt As System.Data.DataTable = New System.Data.DataTable()
                    dt = CType(Session("FilteredData"), System.Data.DataTable)

                    'If (objIview.IsPerfXml) Then
                    '    dt = GetSerialNoForPerfXml(dt, "column1")
                    'End If

                    Dim res As String = Session("res")
                    ires = ConvertDatatableToXML(dt)
                    Dim index As Integer = -1
                    index = ires.IndexOf(">")
                    ires = ires.Insert(index + 1, res)

                End If

            End If
        End If

        If Mid(ires, 1, 7) <> Constants.ERROR Then
            ires = _xmlString & ires
            Dim xmlDoc As New XmlDocument
            xmlDoc.LoadXml(ires)
            Dim compNodes As XmlNodeList
            Dim compNode As XmlNode
            Dim cbaseDataNodes As XmlNodeList
            compNodes = xmlDoc.SelectNodes("//data/comps")
            For Each compNode In compNodes
                cbaseDataNodes = compNode.ChildNodes
                Dim cnt1 As Integer
                cnt1 = 0
                Dim compNodeCnt As Integer = 0
                Dim ToolbarBtnCnt As Integer
                ToolbarBtnCnt = cbaseDataNodes.Count - 1
                For compNodeCnt = ToolbarBtnCnt To 0 Step -1
                    If Mid(cbaseDataNodes(compNodeCnt).Name, 1, 7) = "x__head" Then
                        caption = (cbaseDataNodes(compNodeCnt).Attributes("caption").Value)
                        Exit For
                    ElseIf Mid(cbaseDataNodes(compNodeCnt).Name, 1, 3) = "lbl" Then
                        ivhead.Add(cbaseDataNodes(compNodeCnt).Attributes("caption").Value)
                    End If
                Next
            Next
            pivotGroupHeaderNames.Clear()
            pivotStartCol.Clear()
            pivotEndCol.Clear()

            Dim productNodes As XmlNodeList
            Dim productNode As XmlNode
            Dim baseDataNodes As XmlNodeList
            productNodes = xmlDoc.SelectNodes("//headrow")
            Dim hcolNos As Integer
            hcolNos = 0
            Dim arrNumericFld As New ArrayList
            Dim checkRownofld As Boolean = False
            If objIview.colFld.Count > 0 Then
                If objIview.colFld(0) = "rowno" And isperf = "true" Then
                    colHide.Add("false")
                    checkRownofld = True
                    colFld.Add("rowno")
                End If
            End If
            For Each productNode In productNodes
                baseDataNodes = productNode.ChildNodes
                Dim iCount As Integer = -1
                For Each baseDataNode As XmlNode In baseDataNodes
                    iCount += 1
                    If baseDataNode.Name <> "rowno" Then
                        colFld.Add(baseDataNode.Name)
                        colHead.Add(baseDataNode.InnerText)
                    End If
                    If baseDataNode.Name = "rowno" Then
                        'colHide.Add("false")
                        'checkRownofld = True
                    Else
                        If baseDataNode.Name.StartsWith("html_") Then
                            htmlColumns.Add(baseDataNode.Name)
                        End If
                        If Not baseDataNode.Attributes("type") Is Nothing Then
                            If baseDataNode.Attributes("type").Value = "n" Then
                                arrNumericFld.Add(baseDataNode.Name)
                            End If
                        End If
                        If Not baseDataNode.Attributes("hide") Is Nothing Then
                            If baseDataNode.Name.StartsWith("hide_") Then
                                colHide.Add("true")
                            ElseIf ivtype <> "listview" And ivtype <> "lview" Then
                                If objIview.ColNoPrint.Count > iCount Then
                                    If objIview.ColNoPrint(iCount) = True Then
                                        colHide.Add("true")
                                    Else
                                        colHide.Add(baseDataNode.Attributes("hide").Value)
                                    End If
                                Else
                                    colHide.Add(baseDataNode.Attributes("hide").Value)
                                End If
                            ElseIf (objIview.HiddenCols.IndexOf(baseDataNode.Name) > -1) Then
                                colHide.Add("true")
                            ElseIf (objIview.ShowHiddengridCols) IsNot Nothing Then
                                If (objIview.ShowHiddengridCols.IndexOf(baseDataNode.Name) > -1) Then
                                    colHide.Add("false")
                                Else
                                    colHide.Add(baseDataNode.Attributes("hide").Value)
                                End If
                            Else
                                colHide.Add(baseDataNode.Attributes("hide").Value)
                            End If
                        End If
                        If baseDataNode.Name = "pivotghead" Then
                            Dim finalNodelist As XmlNodeList
                            For Each base2node As XmlNode In baseDataNode
                                finalNodelist = base2node.ChildNodes
                                For Each finalNode As XmlNode In finalNodelist
                                    If finalNode.Name = "sn" Then
                                        pivotStartCol.Add(finalNode.InnerText)
                                    ElseIf finalNode.Name = "ghead" Then
                                        pivotGroupHeaderNames.Add(finalNode.InnerText)
                                    ElseIf finalNode.Name = "en" Then
                                        pivotEndCol.Add(finalNode.InnerText)
                                    End If
                                Next
                            Next
                            colHide.Add("true")
                        End If
                        hcolNos = hcolNos + 1
                    End If
                Next
            Next

            Dim htIdx = 0
            Dim cIdx = 0
            For htIdx = 0 To htmlColumns.Count - 1
                For cIdx = 0 To colFld.Count - 1
                    Dim htmlColName As String = htmlColumns(htIdx)
                    If colFld(cIdx) = htmlColName Then
                        colHide(cIdx) = "false"
                    ElseIf colFld(cIdx) = htmlColName.Replace("html_", "") Then
                        colHide(cIdx) = "true"
                    End If
                Next
            Next
            'to hide column if col is hidden and to hide html_col and if html_col is hidden to hide col.
            If (objIview.HiddenCols.Count > 0) Then
                Dim hdncol = 0
                Dim hdnColumnName As String
                For hdncol = 0 To objIview.HiddenCols.Count - 1
                    hdnColumnName = objIview.HiddenCols(hdncol)
                    ''to hide the column which are hidden in hide columns
                    If (colFld.IndexOf(hdnColumnName) > -1) Then
                        colHide(colFld.IndexOf(hdnColumnName)) = "true"
                    End If
                    If (hdnColumnName.StartsWith("html_")) Then
                        hdnColumnName = hdnColumnName.Replace("html_", "")
                    Else
                        hdnColumnName = "html_" + hdnColumnName
                    End If
                    ''to hide the column after changing the column name 
                    If (colFld.IndexOf(hdnColumnName) > -1) Then
                        colHide(colFld.IndexOf(hdnColumnName)) = "true"
                    End If
                Next
            End If

            'to remove attributes from headrow
            Dim productNodes2 As XmlNodeList
            Dim productNode2 As XmlNode
            Dim baseDataNodes2 As XmlNodeList
            productNodes2 = xmlDoc.SelectNodes("//headrow")
            For Each productNode2 In productNodes2
                baseDataNodes2 = productNode2.ChildNodes
                For Each baseDataNode2 As XmlNode In baseDataNodes2
                    baseDataNode2.Attributes.RemoveAll()
                Next
            Next

            'Remove Comps
            Dim cNode As XmlNode
            cNode = xmlDoc.SelectSingleNode("//comps")
            cNode.ParentNode.RemoveChild(cNode)

            'add Header to xmldoc
            'remove hidden fields in headrow
            Dim hrep As Integer
            'change  later try new code to remove this for
            For hrep = 1 To hcolNos
                Dim productNodes3 As XmlNodeList
                Dim productNode3 As XmlNode
                Dim baseDataNodes3 As XmlNodeList
                productNodes3 = xmlDoc.SelectNodes("//headrow")
                Dim hidx As Integer
                For Each productNode3 In productNodes3
                    baseDataNodes3 = productNode3.ChildNodes
                    For Each baseDataNode3 As XmlNode In baseDataNodes3
                        For hidx = 0 To colFld.Count - 1
                            If colFld(hidx).ToString().ToLower() = baseDataNode3.Name.ToLower() Then
                                'If colHide(hidx) = "true" Or objIview.colHlink(hidx) <> "-" Then
                                If colHide(hidx) = "true" Then
                                    baseDataNode3.ParentNode.RemoveChild(baseDataNode3)
                                End If
                            End If
                        Next
                    Next
                Next
            Next

            'remove hidden fields
            Dim rep As Integer
            'change  later try new code to remove this for
            For rep = 1 To hcolNos

                Dim productNodes1 As XmlNodeList
                Dim productNode1 As XmlNode
                Dim baseDataNodes1 As XmlNodeList
                productNodes1 = xmlDoc.SelectNodes("//row")
                Dim idx As Integer
                For Each productNode1 In productNodes1
                    baseDataNodes1 = productNode1.ChildNodes
                    For Each baseDataNode1 As XmlNode In baseDataNodes1
                        For idx = 0 To colFld.Count - 1
                            If colFld(idx).ToString().ToLower() = baseDataNode1.Name.ToLower() Then
                                'If colHide(idx) = "true" And colFld(idx) <> "axrowtype" Or objIview.colHlink(idx) <> "-" Then
                                If colHide(idx) = "true" And colFld(idx) <> "axrowtype" Then
                                    baseDataNode1.ParentNode.RemoveChild(baseDataNode1)
                                End If
                            End If
                        Next
                    Next
                Next
            Next

            sb.Append("<meta http-equiv='Content-Type' content='text/html;charset=UTF-8'>")
            sb.Append("<html>")
            If isPrint = "true" Then
                sb.Append("<head><title>&nbsp;</title>")
            Else
                'sb.Append("<head><title>HTML</title>")
                sb.Append("<head><title>&nbsp;</title>")
            End If

            If isPrint = "true" Then
                sb.Append("<script type='text/javascript' language='javascript'>")
                sb.Append("window.onload=function(){ setTimeout('printIt()',1000); };")
                sb.Append("function printIt(){window.print();}")
                sb.Append("</script>")
            End If

            sb.Append("<script src=""../Js/thirdparty/jquery/3.1.1/jquery.min.js""></script><script src='../Js/HtmlPrint.js'></script><link href='../Css/htmlfont.css' rel='stylesheet' type='text/css' /></head>")
            Dim direction As String = "ltr"
            If (Session("language").ToString() = "ARABIC") Then
                direction = "rtl"
            End If

            sb.Append("<body dir=" + direction + ">")
            If (AxPrintTitle <> "") Then
                sb.Append("<table width='100%' class='tblhead'>")
                If (AxPrintTitleAlign <> "" And AdvancedConfig <> "") Then
                    sb.Append("<tr><td class='tdarrow' style='text-align: " & AxPrintTitleAlign & "'><font style='font-size : 22.5px; FONT-weight:normal;COLOR:#808080'>" & AxPrintTitle & "</font></td></tr></table>")

                Else

                    sb.Append("<tr><td class='tdarrow'><center><font style='font-size : 22.5px; FONT-weight:normal;COLOR:#808080'>" & AxPrintTitle & "</font></center></td></tr></table>")
                End If
            End If
            If Session("AxShowAppTitle") <> Nothing And Session("AxShowAppTitle").ToString().ToLower() = "true" Then
                'Create Heading/Title of the page
                sb.Append("<div style='width:100%;align:left;border:solid 0px #497D9E;' class='container3'>")
                sb.Append("<b class='rtop3'><b class='r1'></b><b class='r2'></b><b class='r3'></b><b class='r4'></b></b>")
                sb.Append("<table width='100%' class='tblhead'>")
                If Session("AxAppTitle") <> Nothing AndAlso Session("AxAppTitle") <> String.Empty Then
                    sb.Append("<tr><td class='tdarrow'><center><font style='font-size : 16px;'>" & Session("AxAppTitle").ToString() & "</font></center></td></tr>")
                ElseIf Session("projTitle") <> Nothing AndAlso Session("projTitle") <> String.Empty Then
                    sb.Append("<tr><td class='tdarrow'><center><font style='font-size : 16px;'>" & Session("projTitle").ToString() & "</font></center></td></tr>")
                End If

                'sb.Append("<tr><td class='tdarrow'><center><font style='font-family : Segoe UI;font-size : 22.5px; FONT-weight:normal;COLOR:#808080'>" & caption & "</font></center></td></tr>")
                sb.Append("</table></div>")
            End If
            sb.Append("<table width='100%' class='tblhead'>")
            'HEADERS
            If objIview.ReportHdrs.count > 0 Then
                Dim indx As Integer = 0
                For indx = 0 To objIview.ReportHdrs.Count - 1 Step 1
                    sb.Append("<tr><td class='tdarrow'><center><font style= " & objIview.ReportHdrStyles(indx).ToString() & ">" & objIview.ReportHdrs(indx).ToString() & "</font></center></td></tr></table></div>")
                Next
            End If
            'Else
            'create paramaters
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
                    ElseIf (arrParamDet(i).ToString() = String.Empty) Then
                        arrParamCaption.Insert(i, "")
                        arrParamIsHidden.Insert(i, "")
                    End If
                Next
            End If

            Dim paramCnt As Integer
            Dim cnt As Integer = 0
            Dim paramStr As String = String.Empty
            For paramCnt = 0 To arrLstParams.Count - 1
                If (arrParamIsHidden(paramCnt) = "false") Then
                    paramStr &= arrParamCaption(paramCnt) & " - " & arrLstParamVal(paramCnt) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                    cnt = cnt + 1
                End If
                If (cnt = 2) Or paramCnt = arrLstParams.Count Then
                    sb.Append("<div style='width:100%;align:left;border:solid 0px #497D9E;'>")
                    sb.Append("<font style='font-size : 16px; FONT-weight:normal;COLOR:#2F3E53'>" & paramStr & "</font></div>")
                    cnt = 0
                    paramStr = ""
                End If
                Dim lastRow As Integer = arrLstParams.Count
                If lastRow = paramCnt + 1 Then
                    If lastRow Mod 2 <> 0 Then
                        sb.Append("<div style='width:100%;align:left;border:solid 0px #497D9E;'>")
                        sb.Append("<font style='font-size : 16px; FONT-weight:normal;COLOR:#2F3E53'>" & paramStr & "</font></div>")
                    End If
                End If
            Next

            'Adding Filter string
            If objIview.FilterText <> String.Empty Then
                sb.Append("<div style='width:100%;align:center;'>")
                sb.Append("<center><font style='font-size : 12px; FONT-weight:normal;COLOR:#808080'> " & objIview.FilterText & "</font></center></div>")
            End If

            sb.Append("<br/>")
            'create pivot headers
            Dim pivotRow As String = String.Empty
            If pivotGroupHeaderNames.Count > 1 Then
                pivotRow = "<tr bgcolor=#ffffff>"
            End If

            Dim inc As Integer
            Dim val As String
            Dim colSpan As Integer
            For inc = 0 To pivotGroupHeaderNames.Count - 1
                colSpan = CInt(pivotEndCol(inc).ToString) - CInt(pivotStartCol(inc).ToString)
                val = pivotGroupHeaderNames(inc).ToString() & "," & colSpan & "," & 1
                pivotRow += "<td colspan=""" & colSpan & """><center><font color=black ><b>" & pivotGroupHeaderNames(inc).ToString() & "</b></font></center></td>"
            Next
            If pivotRow <> "" Then
                pivotRow += "</tr>"
            End If
            ' Create Table and Column Headers 
            sb.Append("<table id='tblhtmlPrint'cellpadding=0 cellspacing=0 border='1px solid #c4c4c4' width=90% style=""font-size: 14px;border-color: #f0f0f0;"">")
            sb.Append(pivotRow)
            sb.Append("<thead>")
            sb.Append("<tr bgcolor=#ffffff>")
            Dim hproductNodes As XmlNodeList
            Dim hproductNode As XmlNode
            Dim hbaseDataNodes As XmlNodeList
            Dim tmpColCount As Integer = 0
            Dim hn As String = ""
            Dim slnoIndex As Integer = -1
            Dim AlignStr As String = String.Empty
            hproductNodes = xmlDoc.SelectNodes("/data/headrow")
            For Each hproductNode In hproductNodes
                hbaseDataNodes = hproductNode.ChildNodes
                tmpColCount = hbaseDataNodes.Count
                For Each hbaseDataNode As XmlNode In hbaseDataNodes

                    If hbaseDataNode.InnerText = "axp_slno" Then
                        hn = "Sr. No."
                        slnoIndex = 1
                    Else
                        hn = hbaseDataNode.InnerText
                    End If

                    If (hn.IndexOf("~") <> "-1") Then
                        hn = hn.Replace("~", "<br/>")
                    End If

                    If hbaseDataNode.Name <> "rowno" Then
                        sb.Append("<td><center><font color=black ><b>")
                        sb.Append(hn)
                        sb.Append("</b></font></center></td>")
                    Else
                        If ivtype = "listview" Or ivtype = "lview" Then
                            sb.Append("<td><center><font color=black ><b>")
                            sb.Append("Sr. No.")
                            sb.Append("</b></font></center></td>")
                        End If
                    End If

                Next
            Next
            sb.Append("</tr>")
            sb.Append("</thead>")



            Dim oddrowclr As String
            Dim evenrowclr As String
            oddrowclr = "white"
            evenrowclr = "white"
            Dim rowno As Integer
            rowno = 1
            If checkRownofld = True Then
                tmpColCount = tmpColCount - 1
            End If
            ' Get Data
            Dim dproductNodes As XmlNodeList
            Dim dproductNode As XmlNode
            Dim dbaseDataNodes As XmlNodeList

            dproductNodes = xmlDoc.SelectNodes("/data/row")
            Dim addGrandTotal As Boolean = False
            For Each dproductNode In dproductNodes
                dbaseDataNodes = dproductNode.ChildNodes
                Dim i As Integer
                Dim trColor As String = ""

                For i = 0 To dbaseDataNodes.Count - 1
                    If dbaseDataNodes(1) IsNot Nothing Then
                        If dbaseDataNodes(1).InnerText = "subhead" Then
                            trColor = "color:DarkRed;font-weight:bold;"
                            dbaseDataNodes(1).ParentNode.RemoveChild(dbaseDataNodes(1))
                            Exit For
                        ElseIf dbaseDataNodes(1).InnerText = "gtot" Then
                            trColor = "color:DarkGreen;font-weight:bold;"
                            dbaseDataNodes(1).ParentNode.RemoveChild(dbaseDataNodes(1))
                            Exit For
                        ElseIf dbaseDataNodes(1).InnerText = "stot" Then
                            trColor = "color:DarkRed;font-weight:bold;"
                            dbaseDataNodes(1).ParentNode.RemoveChild(dbaseDataNodes(1))
                            Exit For
                        Else
                            If isperf IsNot Nothing Then
                                If (isperf = "false") Then
                                    dbaseDataNodes(1).ParentNode.RemoveChild(dbaseDataNodes(1))
                                    Exit For
                                End If
                            End If
                        End If
                    End If

                Next
                If rowno Mod 2 = 0 Then
                    sb.Append("<tr bgcolor=" & evenrowclr & " style=""" & trColor & """>")
                Else
                    sb.Append("<tr bgcolor=" & oddrowclr & " style=""" & trColor & """>")
                End If
                Dim k As Integer = 1
                Dim isFirstCol As Integer = 0

                For Each dbaseDataNode As XmlNode In dbaseDataNodes
                    Dim indxLoc = objIview.ColFld.IndexOf(dbaseDataNode.Name.ToLower())
                    'If objIview.colHlink(indxLoc) = "-" Then
                    If indxLoc > -1 Then
                        AlignStr = objIview.ColAlign(indxLoc).ToString()
                        If AlignStr = "" Then
                            AlignStr = "left"
                        End If
                    End If
                    If dbaseDataNode.Name = "rowno" Then
                        If ivtype = "listview" Or ivtype = "lview" Then
                            sb.Append("<td style=""text-align:center; vertical-align :top"">")
                            sb.Append(dbaseDataNode.InnerText)
                            sb.Append("</td>")
                        Else
                            Continue For
                        End If
                    Else
                        If addGrandTotal And dbaseDataNode.InnerText = "" And isFirstCol <> 0 And slnoIndex <> -1 Then
                            dbaseDataNode.InnerText = "Grand Total"
                        End If

                        If isFirstCol = 0 And slnoIndex <> -1 Then
                            sb.Append("<td style=""vertical-align:top; text-align:" & AlignStr & """>")
                        Else
                            sb.Append("<td style=""vertical-align:top; text-align:" & AlignStr & """>")
                        End If
                        If dbaseDataNode.InnerText = "Grand Total" And slnoIndex <> -1 Then
                            addGrandTotal = True
                            dbaseDataNode.InnerText = ""
                        Else
                            dbaseDataNode.InnerText = dbaseDataNode.InnerText.Replace("&nbsp;", " ")
                            dbaseDataNode.InnerText = util.CheckSpecialChars(dbaseDataNode.InnerText)
                            sb.Append(dbaseDataNode.InnerText)
                            sb.Append("</td>")
                        End If

                    End If
                    k = k + 1
                    isFirstCol = isFirstCol + 1
                    ' End If
                Next

                'Commented as one extra empty column was comnig in print, need to check in other iviews
                'For k = k To tmpColCount
                '    sb.Append("<td>")
                '    sb.Append("")
                '    sb.Append("</td>")
                'Next

                sb.Append("</tr>")
                rowno = rowno + 1
            Next
            'Commenting rowMerge since it was causing uneasy UI
            'sb.Append("</table><script>rowMerge();</script>")
            sb.Append("</table>")

            'FOOTERS
            If objIview.ReportFtrs.Count > 0 Then
                Dim indx As Integer
                For indx = 0 To objIview.ReportFtrs.Count - 1 Step 1
                    sb.Append("<div style='width:100%;align:center;'>")
                    sb.Append("<center><font " & objIview.ReportFtrStyles(indx).ToString() & ">" & objIview.ReportFtrs(indx).ToString() & "</font></center></div>")
                Next
            End If

            sb.Append("</body>")
            sb.Append("</html>")
            Return sb.ToString()
        Else
            Return "IView Result Error"
        End If
    End Function

    Public Function GetSerialNoForPerfXml(ByVal dt As System.Data.DataTable, ByVal columnName As String) As System.Data.DataTable

        If (columnName <> Nothing) Then

            Dim srNoColumnName As String = columnName
            If (dt.Columns.Contains(srNoColumnName)) Then

                Dim count As Integer
                For count = 0 To dt.Rows.Count - 1 Step 1

                    Dim sno As Integer = count + 1
                    dt.Rows(count)(srNoColumnName) = sno

                Next
            End If
        End If
        Return dt

    End Function

    Private Function GetStyle(ByVal font As String) As String
        Dim result As String = String.Empty
        Dim strColor As String = String.Empty
        Dim size As String = String.Empty
        Dim family As String = String.Empty
        Dim bold As String = String.Empty
        If font <> String.Empty Then
            'the format of style node will be as fontcolor,font-size,font-family,bold 
            result = "style="
            Dim strFont = font.Split(",")

            If Not strFont(0) Is Nothing Then
                Dim c As Color = Color.FromName(strFont(0).Substring(2))
                If c.IsKnownColor Then
                    strColor = c.Name
                Else
                    strColor = "black"
                End If
            End If

            If Not strFont(1) Is Nothing Then
                size = strFont(1)
                If size = String.Empty Then
                    size = "8"
                End If
            End If

            If Not strFont(2) Is Nothing Then
                family = strFont(2)
                If family = String.Empty Then
                    family = "Segoe UI"
                End If
            End If

            If Not strFont(3) Is Nothing Then
                If (strFont(3).ToLower() = "t") Then
                    bold = "bold"
                Else
                    bold = "normal"
                End If

            End If

            If result <> String.Empty Then
                result = "style='color:black;font-size:16px;font-family:" & family & ";font-weight:normal;'"
            End If

        End If
        Return result
    End Function
    Private Sub GetParam(ByVal param As String)
        Dim k As Integer
        Dim tem1 As String = String.Empty
        Dim arrParams As Array
        Dim arrNoOfParams As Array
        Dim params As String = String.Empty
        If Not paramVal Is Nothing Then
            If paramVal.Contains("~") Then
                arrNoOfParams = paramVal.Split("~")
                Dim i As Integer
                For i = 0 To arrNoOfParams.Length - 1
                    arrParams = arrNoOfParams(i).ToString().Split("♠")
                    arrLstParams.Insert(i, arrParams(0))
                    If arrParams(1).Contains("`") = True Then
                        arrParams(1) = arrParams(1).Replace("`", "~")
                        'arrParams(1) = arrParams(1).Replace("`", ",")
                    End If

                    arrLstParamVal.Insert(i, arrParams(1))
                Next
            Else
                arrParams = paramVal.Split("♠")
                For k = 0 To arrParams.Length - 1
                    If arrParams.Length > k + 1 Then
                        arrLstParams.Insert(k, arrParams(0))
                        If arrParams(1).Contains("`") = True Then
                            arrParams(1) = arrParams(1).Replace("`", ",")
                        End If

                        arrLstParamVal.Insert(k, arrParams(1))
                    End If
                Next
            End If
        End If

        Dim j As Integer
        For j = 0 To arrLstParams.Count - 1
            If arrLstParams.Count > 0 Then
                tem1 = arrLstParams(j).ToString()
                If arrLstParams.Count > 0 Then
                    paramVal = arrLstParamVal(j).ToString().Replace("&grave;", "~")
                    paramVal = arrLstParamVal(j).ToString().Replace("&amp;", "&")
                Else
                    paramVal = ""
                End If
            End If
            If param <> "" Then
                paramXml = paramXml & "<" & tem1 & ">"
                paramVal = paramVal.Replace("`", ",")
                paramXml = paramXml & utilObj.CheckSpecialCharsSaveAs(utilObj.ReplaceUrlSpecialChars(paramVal))
                paramXml = paramXml & "</" & tem1 & ">"
                params = params & "&" & tem1 & "="
                params = params & paramVal
            Else
                paramXml = "<params>"
            End If
        Next
    End Sub
    'By using this method we can convert datatable to xml
    Public Function ConvertDatatableToXML(ByVal dt As System.Data.DataTable) As String
        Dim str As New MemoryStream()
        dt.TableName = "row"
        dt.WriteXml(str, True)
        str.Seek(0, SeekOrigin.Begin)
        Dim sr As New StreamReader(str)
        Dim xmlstr As String
        xmlstr = sr.ReadToEnd().Replace("DocumentElement", "data")
        Return (xmlstr)
    End Function


    Public Function DataTable2String(dataTable As System.Data.DataTable) As String
        Dim sb As New StringBuilder()
        If dataTable IsNot Nothing Then

            sb.Append("<meta http-equiv='Content-Type' content='text/html;charset=UTF-8'>")
            sb.Append("<html>")
            If isPrint = "true" Then
                'sb.Append("<head><title>" & objIview.IviewCaption & "</title>")
                sb.Append("<head><title>&nbsp;</title>")
            Else
                'sb.Append("<head><title>HTML</title>")
                sb.Append("<head><title>&nbsp;</title>")
            End If

            If isPrint = "true" Then
                sb.Append("<script type='text/javascript' language='javascript'>")
                sb.Append("window.onload=function(){ setTimeout('printIt()',1000); };")
                sb.Append("function printIt(){window.print();}")
                sb.Append("</script>")
            End If

            sb.Append("<script src=""../Js/thirdparty/jquery/3.1.1/jquery.min.js""></script><script src='../Js/HtmlPrint.js'></script></head>")
            Dim direction As String = "ltr"
            If (Session("language").ToString() = "ARABIC") Then
                direction = "rtl"
            End If
            sb.Append("<body dir=" + direction + ">")
            If (AxPrintTitle <> "") Then
                sb.Append("<table width='100%' class='tblhead'>")
                If (AxPrintTitleAlign <> "" And AdvancedConfig <> "") Then
                    sb.Append("<tr><td class='tdarrow' style='text-align: " & AxPrintTitleAlign & "'><font style='font-size : 22.5px; FONT-weight:normal;COLOR:#808080'>" & AxPrintTitle & "</font></td></tr></table>")

                Else

                    sb.Append("<tr><td class='tdarrow'><center><font style='font-size : 22.5px; FONT-weight:normal;COLOR:#808080'>" & AxPrintTitle & "</font></center></td></tr></table>")
                End If
            End If
            If Session("AxShowAppTitle") <> Nothing And Session("AxShowAppTitle").ToString().ToLower() = "true" Then
                'Create Heading/Title of the page
                sb.Append("<div style='width:100%;align:left;border:solid 0px #497D9E;' class='container3'>")
                sb.Append("<b class='rtop3'><b class='r1'></b><b class='r2'></b><b class='r3'></b><b class='r4'></b></b>")
                sb.Append("<table width='100%' class='tblhead'>")

                If Session("AxAppTitle") <> Nothing AndAlso Session("AxAppTitle") <> String.Empty Then
                    sb.Append("<tr><td class='tdarrow'><center><font style='font-size : 22.5px; FONT-weight:normal;COLOR:#808080'>" & Session("AxAppTitle").ToString() & "</font></center></td></tr>")
                ElseIf Session("projTitle") <> Nothing Then
                    sb.Append("<tr><td class='tdarrow'><center><font style='font-size : 22.5px; FONT-weight:normal;COLOR:#808080'>" & Session("projTitle").ToString() & "</font></center></td></tr>")
                End If
                sb.Append("</table></div>")
            End If
            sb.Append("<table width='100%' class='tblhead'>")

            sb.Append("<tr><td class='tdarrow'><center><font style='font-size : 22.5px; FONT-weight:normal;COLOR:#808080'>" & objIview.IviewCaption & "</font></center></td></tr></table>")

            sb.Append("<table width='100%' class='tblhead'>")
            If objIview.ReportHdrs.count > 0 Then
                Dim indx As Integer = 0
                For indx = 0 To objIview.ReportHdrs.Count - 1 Step 1
                    sb.Append("<tr><td class='tdarrow'><center><font style= " & objIview.ReportHdrStyles(indx).ToString() & ">" & objIview.ReportHdrs(indx).ToString() & "</font></center></td></tr>")
                Next
            End If
            sb.Append("</table>")

            sb.Append("<table id='tblhtmlPrint'cellpadding=0 cellspacing=0 border='1px solid #c4c4c4' width=90% style=""font-size: 12px;border-color: #f0f0f0;"">")
            sb.Append("<thead>")
            sb.Append("<tr bgcolor=#ffffff>")
            For Each col As DataColumn In dataTable.Columns
                If objIview.ColHide(col.Ordinal) = False And objIview.ColNoPrint(col.Ordinal) = False Then
                    sb.Append("<th>")
                    sb.Append(objIview.HeadName(col.Ordinal))
                    'sb.Append(col.ColumnName)
                    sb.Append("</th>")
                End If
            Next
            sb.Append("</tr>")
            sb.Append("</thead>")
            For Each row As DataRow In dataTable.Rows
                sb.Append("<tr>")
                For Each column As DataColumn In dataTable.Columns
                    If objIview.ColHide(column.Ordinal) = False And objIview.ColNoPrint(column.Ordinal) = False Then
                        sb.Append("<td>")
                        sb.Append(If(IsDBNull(row(column.ColumnName)), String.Empty, util.CheckSpecialChars(row(column.ColumnName)))) 'util.CheckSpecialChars(dbaseDataNode.InnerText)
                        sb.Append("</td>")
                    End If
                Next
                sb.Append("</tr>")
            Next
            sb.Append("</table>")

            sb.Append("</table><script>rowMerge();</script>")

            'FOOTERS
            If objIview.ReportFtrs.Count > 0 Then
                Dim indx As Integer
                For indx = 0 To objIview.ReportFtrs.Count - 1 Step 1
                    sb.Append("<div style='width:100%;align:center;'>")
                    sb.Append("<center><font " & objIview.ReportFtrStyles(indx).ToString() & ">" & objIview.ReportFtrs(indx).ToString() & "</font></center></div>")
                Next
            End If

            sb.Append("</body>")
            sb.Append("</html>")
        Else
            Return "IView Result Error"
        End If
        Return sb.ToString()
    End Function
End Class
