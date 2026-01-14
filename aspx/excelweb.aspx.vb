Imports System.Data
Imports System.Xml
Imports System.Xml.Xsl
Imports System.Globalization
Imports System.IO
Imports System.Configuration
Imports System.Web



Partial Class excelweb
    Inherits System.Web.UI.Page

    Public sb As StringBuilder = New StringBuilder()
    Dim _xmlString As String =
                   "<?xml version=""1.0"" encoding=""utf-8"" ?>"
    Dim util As Util.Util = New Util.Util()
    Dim logobj As LogFile.Log = New LogFile.Log()
    Dim colHide As New ArrayList()
    Dim colFld As New ArrayList()
    Dim colHead As New ArrayList()
    Dim colType As New ArrayList()
    Dim colDec As New ArrayList()
    Dim paName As New ArrayList()
    Dim paCaption As New ArrayList()
    Dim paHidden As New ArrayList()
    Dim iname As String
    Dim uname As String
    Dim sid As String
    Dim ivtype As String
    Dim proj As String
    Dim paramVal As String
    Dim paramXml As String = ""
    Dim resparamVal As String
    Dim htmlColumns As New ArrayList()
    Dim objIview
    Dim Ikey As String = String.Empty
    Dim listres As String = String.Empty
    Dim isFromHome As Boolean = False
    Dim exportVerticalAlignStyle As String = "vertical-align: middle"



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Request.UrlReferrer Is Nothing) Then
            Dim strUrl As String = Request.UrlReferrer.AbsolutePath
            If strUrl.Contains("iview.aspx") Or strUrl.Contains("listIview.aspx") Then
                'continue execution
            Else
                'Response.Redirect("axcustomerror.aspx")
            End If
        Else
            ' Response.Redirect("axcustomerror.aspx")
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
            If Not Request.QueryString("AxFromhome") Is Nothing Then
                isFromHome = Request.QueryString("AxFromhome")
            End If
            Response.Clear()
            Response.ContentType = "application/vnd.ms-excel"
            Response.Charset = ""
            Dim result As String = GetExcel()
            Dim htmlEncodedResult As String = String.Empty
            htmlEncodedResult = HttpUtility.HtmlDecode(result)
            'htmlEncodedResult = htmlEncodedResult.Replace("<font", "<br/><font")
            'htmlEncodedResult = htmlEncodedResult.Replace("</font>", "</font> <br/>")


            Dim strFileName As String = iname & ".xls"
            Response.AppendHeader("Content-Disposition", "inline;filename=" & strFileName)
            Response.ContentEncoding = System.Text.Encoding.Unicode
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
            Response.Write(htmlEncodedResult)
            Response.End()
        End If
    End Sub
    'Private Sub GetExcel()
    Private Function GetExcel() As String
        If Not IsPostBack Then
            iname = Request.QueryString("ivname")
            uname = Session("user")
            sid = Session("nsessionid")
            ivtype = Request.QueryString("ivtype")
            If Not Request.QueryString("axpCache") Is Nothing Then
                paramVal = Session("AxIvExportParams-" + iname).ToString()
                Session("AxIvExportParams-" + iname) = ""
            Else
                paramVal = Request.QueryString("params")
            End If

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


            Dim stime As Long = DateTime.Now.Ticks
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
            ViewState("params") = paramVal
            proj = Session("project")
            ViewState("iname") = iname
            ViewState("ivtype") = ivtype
            ViewState("proj") = proj
            ViewState("user") = uname
            ViewState("sid") = sid
        Else
            uname = ViewState("user")
            iname = ViewState("iname")
            ivtype = ViewState("ivtype")
            proj = ViewState("proj")
            sid = ViewState("sid")
            paramVal = ViewState("params")
            ivtype = ViewState("ivtype")
        End If

        ' If ivtype = "listview" Or ivtype = "lview" Then
        '     objIview = New ListviewData()
        '     objIview = CType(Session(Ikey), ListviewData)
        ' Else
        objIview = New IviewData()
        objIview = CType(Session(Ikey), IviewData)
        ' End If
        If isFromHome Then
            objIview = New IviewData()
            objIview.FilterXml = ""
            objIview.colFld = New ArrayList
            objIview.ColNoPrint = New ArrayList
            objIview.ReportHdrs = New ArrayList
            objIview.ReportFtrs = New ArrayList
            objIview.ColHide = New ArrayList
            objIview.ColFld = New ArrayList
            objIview.ColAlign = New ArrayList
            objIview.IviewCaption = ""
            objIview.IsDirectDBcall = False

        End If

        Dim isList As String
        If ivtype = "listview" Or ivtype = "lview" Then
            isList = "true"
        Else
            isList = "false"
            resparamVal = paramVal
            GetParamcaption()
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
        Try
            Select Case objIview.exportVerticalAlign
                Case "top"
                    exportVerticalAlignStyle = "vertical-align: top"
                Case "middle"
                    exportVerticalAlignStyle = "vertical-align: middle"
                Case "bottom"
                    exportVerticalAlignStyle = "vertical-align: bottom"
                Case Else
                    exportVerticalAlignStyle = "vertical-align: middle"
            End Select

        Catch ex As Exception
            exportVerticalAlignStyle = "vertical-align: middle"
        End Try
        Dim fileName As String = "GetLView" & iname
        Dim errorLog As String = logobj.CreateLog("Getting CsvIview.", sid, fileName, "new")
        Dim iXml As String
        If ivtype = "listview" Or ivtype = "lview" Then
            Dim viewName = iname
            Dim CustomViewXml As String = String.Empty
            If Convert.ToString(Session("CustomView")) <> String.Empty AndAlso Convert.ToString(Session("CustViewTransId")) = iname Then
                viewName = Convert.ToString(Session("CustomView"))
            End If
            iXml = "<root name=""" & iname & """" & custXML & " axpapp =""" & Session("project") & """ sessionid = """ & sid & """ appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " trace =" & Chr(34) & errorLog & Chr(34) & " pageno=""0"" firsttime=""yes"" sorder=""" & sortOrd & """ scol=""" & sortCol & """ fcolopr=""" + filterOpr + """ fcolnm=""" & filterCol & """ fcolval1=""" + filterColVal + """ fcolval2=""" + filterValue1 + """ pforms=""true""><params> "
            iXml = iXml & paramXml
            iXml = iXml & "</params>"
            CustomViewXml = "<customizeview name='" + viewName + "'/>"
            iXml = iXml & CustomViewXml
        Else
            iXml = "<root name =""" & iname & """ axpapp =""" & Session("project") & """ sessionid = """ & sid & """ appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " trace =" & Chr(34) & errorLog & Chr(34) & " pageno=""0"" firsttime=""yes"" sorder=""" & sortOrd & """ scol=""" & sortCol & """ fcolopr=""" + filterOpr + """ fcolnm=""" & filterCol & """ fcolval1=""" + filterColVal + """ fcolval2=""" + filterValue1 + """  pforms=""true""><params> "
            iXml = iXml & paramXml
            iXml = iXml & "</params>"
        End If

        iXml &= Session("axApps").ToString() + Application("axProps").ToString() + Session("axGlobalVars").ToString() + Session("axUserVars").ToString() & "</root>"
        Dim ires As String = ""
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
            Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()

            'Call service
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
                    Dim isperf As String
                    xmlDoc.LoadXml(ires)

                    compCheckPerf = xmlDoc.SelectSingleNode("//data")
                    If Not compCheckPerf.Attributes("perfxml") Is Nothing Then
                    End If

                Else

                    Dim dt As System.Data.DataTable = New System.Data.DataTable()
                    dt = CType(Session("FilteredData"), System.Data.DataTable)
                    Dim res As String = Session("res")
                    ires = ConvertDatatableToXML(dt)
                    Dim index As Integer = -1
                    index = ires.IndexOf(">")
                    ires = ires.Insert(index + 1, res)

                End If
            End If
        End If

        If ires <> Constants.ERROR Or Mid(ires, 1, 7) <> Constants.ERROR Then
            ires = _xmlString & ires
            Dim xmlDoc As New XmlDocument
            xmlDoc.LoadXml(ires)
            Dim productNodes As XmlNodeList
            Dim productNode As XmlNode
            Dim baseDataNodes As XmlNodeList
            productNodes = xmlDoc.SelectNodes("//headrow")
            Dim hcolNos As Integer
            hcolNos = 0

            For Each productNode In productNodes
                baseDataNodes = productNode.ChildNodes
                Dim iCount As Integer = -1
                For Each baseDataNode As XmlNode In baseDataNodes
                    iCount += 1
                    If baseDataNode.Name = "pivotghead" Then
                    ElseIf baseDataNode.Name = "rowno" Then
                        baseDataNode.InnerText = "Sr. No."
                        colFld.Add(baseDataNode.Name)
                        If (ivtype = "lview") Then
                            colHide.Add("false")
                            colHead.Add("Sr. No.")
                            colType.Add("c")
                        Else
                            colHide.Add("true")
                            colHead.Add(baseDataNode.InnerText)
                            colType.Add("")
                        End If


                        colDec.Add("")
                    Else
                        If baseDataNode.Name.StartsWith("html_") Then
                            htmlColumns.Add(baseDataNode.Name)
                        End If
                        colFld.Add(baseDataNode.Name)
                        colHead.Add(baseDataNode.InnerText)

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
                        Else
                            colHide.Add(baseDataNode.Attributes("hide").Value)
                        End If
                        colType.Add(baseDataNode.Attributes("type").Value)
                        colDec.Add(baseDataNode.Attributes("dec").Value)
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

            'Get name from comps
            Dim iviewcap As String = ""
            Dim iviewAddCaption As String = ""
            Dim iviewAddFooter As String = ""
            Dim xmlDoc1 As New XmlDocument
            Dim compNodes As XmlNodeList
            Dim compNode As XmlNode
            Dim cbaseDataNodes As XmlNodeList

            xmlDoc1.LoadXml(ires)
            compNodes = xmlDoc1.SelectNodes("//data/comps")
            Dim lblcnt As Integer = 0
            Dim lblfcnt As Integer = 0
            For Each compNode In compNodes
                cbaseDataNodes = compNode.ChildNodes
                For Each cbaseDataNode As XmlNode In cbaseDataNodes
                    If Mid(cbaseDataNode.Name, 1, 7) = "x__head" Then
                        iviewcap = cbaseDataNode.Attributes("caption").Value
                    ElseIf Mid(cbaseDataNode.Name, 1, 3) = "lbl" Then
                        lblcnt += 1
                        If iviewAddCaption = "" Then
                            iviewAddCaption = cbaseDataNode.Attributes("caption").Value
                        Else
                            iviewAddCaption = iviewAddCaption & "*,*" & cbaseDataNode.Attributes("caption").Value
                        End If
                    ElseIf cbaseDataNode.Name = "header" Then
                        lblcnt += 1
                        For Each childHeaders As XmlNode In cbaseDataNode
                            If iviewAddCaption = "" Then
                                iviewAddCaption = childHeaders.InnerText
                            Else
                                iviewAddCaption = iviewAddCaption & "*,*" & childHeaders.InnerText
                            End If
                        Next
                    End If
                    If cbaseDataNode.Name = "footer" Then
                        lblfcnt += 1
                        For Each childFooters As XmlNode In cbaseDataNode
                            If iviewAddFooter = "" Then
                                iviewAddFooter = childFooters.InnerText
                            Else
                                iviewAddFooter = iviewAddFooter & "~" & childFooters.InnerText
                            End If
                        Next
                    End If
                Next
            Next
            If lblcnt <= 3 Then
                iviewAddCaption &= "*,*"
            End If
            'to remove attributes from headrow
            Dim productNodes2 As XmlNodeList
            Dim productNode2 As XmlNode
            Dim baseDataNodes2 As XmlNodeList
            productNodes2 = xmlDoc.SelectNodes("//headrow")

            Dim projectName As String = String.Empty
            If Session("AxAppTitle") <> Nothing AndAlso Session("AxAppTitle") <> String.Empty Then
                projectName = Session("AxAppTitle").ToString()
            ElseIf Session("projTitle") <> Nothing Then
                projectName = Session("projTitle").ToString()
            End If


            Dim nAppTitle As XmlAttribute
            nAppTitle = xmlDoc.CreateAttribute("projectName")
            nAppTitle.Value = projectName

            Dim nCaption As XmlAttribute
            nCaption = xmlDoc.CreateAttribute("caption")
            nCaption.Value = iviewcap

            'Dim addlCaption As XmlAttribute
            'addlCaption = xmlDoc.CreateAttribute("addlcaption")
            'addlCaption.Value = iviewAddCaption

            Dim crdate As XmlAttribute
            crdate = xmlDoc.CreateAttribute("crdate")
            crdate.Value = DateAndTime.Now.ToString()
            Dim time As DateTime = DateTime.Now
            Dim format As String = "dd/MM/yyyy"

            If Session("ClientLocale") = "en-us" Then
                format = "MM/dd/yyyy"
            End If

            'crdate.Value = util.GetClientDateString(clientCulture, DateString.Replace("-", "/"))
            'crdate.Value = time.ToString(format)
            crdate.Value = time.ToString(format, CultureInfo.InvariantCulture)

            Dim paramnode As XmlAttribute
            paramnode = xmlDoc.CreateAttribute("params")
            paramnode.Value = resparamVal

            Dim footNode As XmlAttribute
            footNode = xmlDoc.CreateAttribute("footer")
            footNode.Value = iviewAddFooter

            For Each productNode2 In productNodes2
                If Session("AxShowAppTitle") <> Nothing AndAlso Session("AxShowAppTitle").ToString().ToLower() = "true" AndAlso projectName <> String.Empty Then
                    productNode2.Attributes.Append(nAppTitle)
                End If

                productNode2.Attributes.Append(nCaption)
                'productNode2.Attributes.Append(addlCaption)
                productNode2.Attributes.Append(crdate)
                productNode2.Attributes.Append(paramnode)
                productNode2.Attributes.Append(footNode)
                baseDataNodes2 = productNode2.ChildNodes
                For Each baseDataNode2 As XmlNode In baseDataNodes2
                    baseDataNode2.Attributes.RemoveAll()
                Next
            Next

            'Add Headers
            'If Not objIview.ReportHdrs Is Nothing And objIview.ReportHdrs.Count > 0 Then
            Dim printHeaders As XmlElement = xmlDoc.CreateElement("printHeaders")
            Dim printHeadersString As StringBuilder = New StringBuilder()

            Dim indx As Integer = 0
            If objIview.ReportHdrs Is Nothing And objIview.ReportHdrs.Count > 0 Then
                For indx = 0 To objIview.ReportHdrs.Count - 1 Step 1
                    'printHeaders.A
                    printHeadersString.Append("<headerData>" & objIview.ReportHdrs(indx).ToString().Trim() & "</headerData>")
                    'sb.Append("<tr><td class='tdarrow'><center><font style= " & objIview.ReportHdrStyles(indx).ToString() & ">" & objIview.ReportHdrs(indx).ToString() & "</font></center></td></tr></table></div>")
                Next
            Else
                printHeadersString.Append("<headerData></headerData>")
            End If
            printHeaders.InnerXml = printHeadersString.ToString()
            xmlDoc.DocumentElement.PrependChild(printHeaders)
            'End If


            'If newPXml <> "" Then
            '    pivotNode.InnerXml = newPXml
            'Else
            '    pivotNode.InnerXml = "<head><ghead class="""" /></head>"

            'Remove Comps
            Dim cNode As XmlNode
            cNode = xmlDoc.SelectSingleNode("//comps")
            cNode.ParentNode.RemoveChild(cNode)
            If (ivtype = "listview" Or ivtype = "lview") And objIview.FilterXml Is String.Empty Then
                'Remove Customize view node
                Dim customNode As XmlNode
                customNode = xmlDoc.SelectSingleNode("//customizeview")
                customNode.ParentNode.RemoveChild(customNode)
            End If

            Dim pivotXml As String = ""
            'add Header to xmldoc
            'remove hidden fields in headrow
            Dim hrep As Integer
            For hrep = 1 To hcolNos
                Dim productNodes3 As XmlNodeList
                Dim productNode3 As XmlNode
                Dim baseDataNodes3 As XmlNodeList
                productNodes3 = xmlDoc.SelectNodes("//headrow")
                Dim hidx As Integer

                For Each productNode3 In productNodes3
                    baseDataNodes3 = productNode3.ChildNodes
                    For Each baseDataNode3 As XmlNode In baseDataNodes3

                        Dim nodeString As String = baseDataNode3.InnerText.Replace("~", "<br/>")
                        If baseDataNode3.Name <> "pivotghead" Then
                            baseDataNode3.InnerText = nodeString
                        End If

                        If baseDataNode3.Name = "pivotghead" Then
                            pivotXml = baseDataNode3.InnerXml
                            baseDataNode3.ParentNode.RemoveChild(baseDataNode3)
                        Else
                            For hidx = 0 To colFld.Count - 1
                                If colFld(hidx) = baseDataNode3.Name Then
                                    If colHide(hidx) = "true" Then
                                        baseDataNode3.ParentNode.RemoveChild(baseDataNode3)
                                    End If
                                End If
                            Next
                        End If
                    Next
                Next
            Next

            'Process Pivot
            pivotXml = "<pivot>" & pivotXml & "</pivot>"
            Dim pxmlDoc As New XmlDocument
            pxmlDoc.LoadXml(pivotXml)

            Dim ptproductNodes As XmlNodeList
            Dim ptproductNode As XmlNode
            Dim ptbaseDataNodes As XmlNodeList
            ptproductNodes = pxmlDoc.SelectNodes("//head")

            Dim newPXml As String = ""
            For Each ptproductNode In ptproductNodes
                Dim snno As String = ptproductNode.Attributes("snno").Value()
                Dim enno As String = ptproductNode.Attributes("enno").Value()
                Dim cspan As Integer = CInt(enno) - CInt(snno) + 1
                newPXml = newPXml & "<head>"
                ptbaseDataNodes = ptproductNode.ChildNodes
                For Each ptbaseDataNode As XmlNode In ptbaseDataNodes
                    If ptbaseDataNode.Name = "ghead" Then
                        ptbaseDataNode.InnerText = util.CheckSpecialChars(ptbaseDataNode.InnerText)
                        ptbaseDataNode.InnerText = ptbaseDataNode.InnerText.Replace("~", "<br/>")
                        newPXml = newPXml & "<ghead class=""gridHeader""  colspan=""" & cspan & """>" & ptbaseDataNode.InnerText & "</ghead>"
                    End If
                Next
                newPXml = newPXml & "</head>"
            Next
            'Add pivot node
            Dim pivotNode As XmlElement = xmlDoc.CreateElement("pivot")
            If newPXml <> "" Then
                pivotNode.InnerXml = newPXml
            Else
                pivotNode.InnerXml = "<head><ghead class="""" /></head>"
            End If

            xmlDoc.DocumentElement.PrependChild(pivotNode)


            'To set class
            Dim productNodes5 As XmlNodeList
            Dim productNode5 As XmlNode
            Dim baseDataNodes5 As XmlNodeList
            productNodes5 = xmlDoc.SelectNodes("//row")
            For Each productNode5 In productNodes5
                'productNode2.Attributes.Append(paramnode)
                baseDataNodes5 = productNode5.ChildNodes
                Dim rowclass As String = "searchresultitem"
                Dim colalignvalue As String = ""
                For Each baseDataNode5 As XmlNode In baseDataNodes5
                    Dim axrtype As String = ""
                    Dim dsa1 As String = baseDataNode5.Name.ToString()
                    Dim dfs1 As String = baseDataNode5.InnerText.ToString()
                    If (baseDataNode5.Name.ToString().ToLower() = "axrowtype" And baseDataNode5.InnerText.ToString() <> "") Then
                        axrtype = baseDataNode5.InnerText.ToString()
                    End If
                    If axrtype <> "" Then
                        rowclass = axrtype
                    End If

                    Dim indxLoc = objIview.ColFld.IndexOf(baseDataNode5.Name)
                    If indxLoc > -1 Then
                        colalignvalue = objIview.ColAlign(indxLoc).ToString()
                        If colalignvalue = "" Then
                            colalignvalue = "left"
                        End If
                    End If
                    Dim classnode As XmlAttribute
                    classnode = xmlDoc.CreateAttribute("class")
                    classnode.Value = (rowclass + "-" + colalignvalue).ToLower()
                    baseDataNode5.Attributes.Append(classnode)
                Next
            Next
            'Format numeric fields
            Dim productNodesNum As XmlNodeList
            Dim productNodeNum As XmlNode
            Dim baseDataNodesNum As XmlNodeList
            productNodesNum = xmlDoc.SelectNodes("//row")

            Dim idxx As Integer

            For Each productNodeNum In productNodesNum
                baseDataNodesNum = productNodeNum.ChildNodes
                For Each baseDataNodeNum As XmlNode In baseDataNodesNum
                    For idxx = 0 To colFld.Count - 1
                        If colFld(idxx) = baseDataNodeNum.Name.ToLower() Then
                            If colType(idxx) = "n" And baseDataNodeNum.InnerText <> "" And colDec(idxx) <> "0" Then
                                Dim flvalue As String = baseDataNodeNum.InnerText.ToString()
                                Dim val As Double
                                Dim firstChar As Char = Convert.ToChar(flvalue.Substring(0, 1))
                                Dim lastChar As Char = Convert.ToChar(flvalue.Substring(flvalue.Length - 1, 1))
                                Dim isNumber As Boolean = Char.IsDigit(firstChar)
                                If Not isNumber And firstChar = "(" And lastChar = ")" Then
                                    flvalue = flvalue.Substring(1, flvalue.Length - 2)
                                    val = Double.Parse(flvalue)
                                ElseIf flvalue = "&nbsp;" Then
                                    baseDataNodeNum.InnerText = ""
                                ElseIf Not isNumber Then
                                    flvalue = flvalue.Substring(1)
                                    val = Double.Parse(flvalue)
                                Else
                                    val = Double.Parse(flvalue)
                                End If
                                If Not isNumber And firstChar = "(" And lastChar = ")" Then
                                    baseDataNodeNum.InnerText = Convert.ToString(firstChar) + val.ToString("N" + colDec(idxx), CultureInfo.InvariantCulture) + Convert.ToString(lastChar)
                                Else
                                    baseDataNodeNum.InnerText = (If(isNumber, "", Convert.ToString(firstChar))) + val.ToString("N" + colDec(idxx), CultureInfo.InvariantCulture)
                                End If

                                'If baseDataNodeNum.InnerText.IndexOf("$") > -1 Then
                                'baseDataNodeNum.InnerText = "$" & String.Format("{0:n" & colDec(idxx) & "}", Convert.ToDouble(baseDataNodeNum.InnerText.Substring(1)))
                                'Else
                                'baseDataNodeNum.InnerText = String.Format("{0:n" & colDec(idxx) & "}", Convert.ToDouble(baseDataNodeNum.InnerText))
                                'End If
                            ElseIf colType(idxx) = "c" And baseDataNodeNum.InnerText <> "" Then
                                Dim nodeValue As String = baseDataNodeNum.InnerText
                                If IsNumeric(nodeValue) Then
                                    Dim indexOfDecimalPoint As Integer = nodeValue.IndexOf(".")
                                    Dim regex As Regex = New Regex("[0-9.-]*(e|E)[0-9.-]*")
                                    Dim match As Match = regex.Match(baseDataNodeNum.InnerText)
                                    If indexOfDecimalPoint > -1 Then
                                        Dim numberOfDecimals As Integer = nodeValue.Substring(indexOfDecimalPoint + 1).Length
                                        Dim decimalFormat As String = ""
                                        For j As Integer = 1 To numberOfDecimals
                                            decimalFormat += "0"
                                        Next
                                        baseDataNodeNum.InnerText = "=TEXT(""" & baseDataNodeNum.InnerText & """,""0." & decimalFormat & """)"
                                    ElseIf match.Success And match.Index = 0 Or baseDataNodeNum.InnerText.StartsWith("0") Or baseDataNodeNum.InnerText.Length > 15 Then
                                        baseDataNodeNum.InnerText = "&nbsp;" & baseDataNodeNum.InnerText & ""
                                    Else
                                        baseDataNodeNum.InnerText = "=TEXT(""" & baseDataNodeNum.InnerText & """,""0"")"
                                    End If
                                Else
                                    baseDataNodeNum.InnerText = baseDataNodeNum.InnerText.Replace("~", "<br/>")
                                End If
                            End If
                        End If
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
                                If colHide(idx) = "true" Then
                                    baseDataNode1.ParentNode.RemoveChild(baseDataNode1)
                                End If
                            End If
                        Next

                    Next
                Next
            Next

            'Code to add new nodes in xml which aws missed
            Dim headSinglenode As XmlNode
            headSinglenode = xmlDoc.SelectSingleNode("//headrow")

            Try
                Dim vAlign As XmlAttribute
                vAlign = xmlDoc.CreateAttribute("valign")
                vAlign.Value = exportVerticalAlignStyle
                headSinglenode.Attributes.Append(vAlign)

            Catch ex As Exception

            End Try

            Dim headNodecount As Integer = headSinglenode.ChildNodes.Count

            Dim productNodesRow As XmlNodeList
            Dim productNodeRow As XmlNode
            Dim baseDataNodesRow As XmlNodeList
            productNodesRow = xmlDoc.SelectNodes("//row")
            For Each productNodeRow In productNodesRow
                baseDataNodesRow = productNodeRow.ChildNodes
                Dim rowCounta As Integer = baseDataNodesRow.Count
                Dim rowclass As String = "searchresultitem"
                If rowCounta < headNodecount Then
                    Dim d As Integer = headNodecount - rowCounta
                    Dim b As Integer
                    For b = 1 To d
                        Dim tempNode As XmlElement = xmlDoc.CreateElement("refill")
                        tempNode.InnerText = ""
                        Dim classnode As XmlAttribute
                        classnode = xmlDoc.CreateAttribute("class")
                        classnode.Value = rowclass.ToLower()
                        tempNode.Attributes.Append(classnode)
                        productNodeRow.AppendChild(tempNode)
                    Next
                End If
            Next

            Dim sw As New StringWriter()
            Dim xw As New XmlTextWriter(sw)
            xmlDoc.WriteTo(xw)

            Dim nXml As String
            nXml = sw.ToString()

            Dim sr As New StringReader(nXml)

            Dim ds As New DataSet()
            ds.ReadXml(sr)
            For Each tables As System.Data.DataTable In ds.Tables
                If (tables.TableName.Equals("headrow")) Then
                    For Each columns As DataColumn In tables.Columns
                        If (tables.Rows(0)(columns).ToString().Contains("axp_slno")) Then
                            tables.Rows(0)(columns) = "Sr. No."
                            Exit For
                        End If
                        If (ivtype = "lview") Then
                            If (tables.Rows(0)(columns).ToString().Contains("rowno")) Then
                                tables.Rows(0)(columns) = "Sr. No."
                                Exit For
                            End If
                        End If
                    Next
                End If
            Next

            Dim xdd As XmlDataDocument
            xdd = New XmlDataDocument(ds)

            Dim xt As XslCompiledTransform = New XslCompiledTransform()
            xt.Load(Server.MapPath("excel.xsl"))
            Dim swr As StringWriter = New StringWriter(sb)
            Dim writer As XmlTextWriter = New XmlTextWriter(swr)
            xt.Transform(xdd, writer)
            sw.Close()
            writer.Close()
            'Response.End()
            Return sb.ToString()
        Else
            'Response.Write("IView Result Error")
            Return "IView Result Error"
        End If
    End Function

    Private Sub GetParam(ByVal param As String)
        Dim k As Integer

        Dim tem1 As String = ""
        Dim arrParams As Array
        Dim arrNoOfParams As Array
        Dim params As String = ""
        Dim arrLstParams As New ArrayList
        Dim arrLstParamVal As New ArrayList
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
                            'arrParams(1) = arrParams(1).Replace("`", ",")
                            arrParams(1) = arrParams(1).Replace("`", "~")
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
                Else
                    paramVal = ""
                End If
            End If

            If param <> "" Then
                paramXml = paramXml & "<" & tem1 & ">"
                paramXml = paramXml & paramVal
                paramXml = paramXml & "</" & tem1 & ">"

                params = params & "&" & tem1 & "="
                params = params & paramVal
            Else
                paramXml = "<params>"
            End If

        Next
    End Sub
    Private Sub GetParamcaption()
        Dim fileName As String = "GetParams" & iname
        Dim errorLog As String = logobj.CreateLog("Getting params.", sid, fileName, "new")
        Dim iXml As String
        iXml = "<root name =""" & iname & """ axpapp =""" & Session("project") & """ sessionid = """ & sid & """  trace =" & Chr(34) & errorLog & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & ">"
        iXml &= Session("axApps").ToString() + Application("axProps").ToString() + Session("axGlobalVars").ToString() + Session("axUserVars").ToString() & "</root>"
        Dim ires As String = ""
        Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
        objIview.requestJSON = False
        ires = objWebServiceExt.CallGetParamsWS(iname, iXml, objIview)
        ires = ires.Split("♠")(1)
        Dim resparam As String = resparamVal
        Dim xmlDoc As New XmlDocument()
        Dim productNodes As XmlNodeList = Nothing
        Dim baseDataNodes As XmlNodeList = Nothing
        Dim iCnt As Integer = 0
        Dim fldNo As Integer = 0
        Dim dpCnt As Integer = 0
        Try
            xmlDoc.LoadXml(ires)
        Catch ex As XmlException
            Response.Redirect(util.ERRPATH + ex.Message)
        End Try

        productNodes = xmlDoc.SelectNodes("//root")

        For Each productNode As XmlNode In productNodes
            baseDataNodes = productNode.ChildNodes
            For Each baseDataNode As XmlNode In baseDataNodes

                For Each tstNode As XmlNode In baseDataNode
                    If tstNode.Name = "a0" Then
                        paName.Add(tstNode.InnerText)
                    ElseIf tstNode.Name = "a2" Then
                        paCaption.Add(tstNode.InnerText)
                    ElseIf tstNode.Name = "a21" Then
                        paHidden.Add(tstNode.InnerText.ToLower())
                    End If
                Next
            Next
        Next

        Dim resnewParam = resparam.Split("~")
        Dim resparamExhidden As String = ""
        Dim j As Integer
        'For j = 0 To resnewParam.Length - 1
        '    Dim x As Integer
        '    For x = 0 To paName.Count - 1
        '        If resnewParam(j).ToString().Equals(paName(x).ToString()) Then
        '            If paHidden(x).ToString() = "false" Then
        '                Dim resCapchange = resnewParam(j).ToString().Replace(paName(x).ToString(), paCaption(x).ToString()).Replace("♠", ":")
        '                If resparamExhidden = "" Then
        '                    resparamExhidden = resCapchange
        '                Else
        '                    resparamExhidden = resparamExhidden & "~" & resCapchange
        '                End If
        '                Exit For
        '            End If
        '        End If
        '    Next
        'Next

        For j = 0 To resnewParam.Length - 1
            Dim x As Integer
            For x = 0 To paName.Count - 1
                If resnewParam(j).ToString().IndexOf(paName(x).ToString()) > -1 Then
                    Dim resnewParams = resnewParam(j).Split("♠")
                    If resnewParams(0) = paName(x).ToString() Then
                        If paHidden(x).ToString() = "false" Then
                            Dim resCapchange = resnewParam(j).ToString().Replace(paName(x).ToString(), paCaption(x).ToString()).Replace("♠", ":")
                            If resparamExhidden = "" Then
                                resparamExhidden = resCapchange
                            Else
                                resparamExhidden = resparamExhidden & "~" & resCapchange
                            End If
                            Exit For
                        End If
                    End If
                End If
            Next
        Next

        resparamVal = resparamExhidden
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
        Dim time As DateTime = DateTime.Now
        Dim dgrExport As New GridView
        dgrExport.DataSource = dataTable
        dgrExport.DataBind()
        Dim str As String = String.Empty
        Dim icolcount As Integer = 0
        For Each row As GridViewRow In dgrExport.Rows
            For i As Integer = 0 To row.Cells.Count - 1
                If objIview.colHide(i) = "false" And objIview.ColNoPrint(i) = "false" Then
                    str += "<td class='gridHeader'>" + dgrExport.HeaderRow.Cells(i).Text + "</td>"
                    icolcount = icolcount + 1
                End If
            Next
            Exit For
        Next

        sb.Append("<HTML>")
        sb.Append("<head>")
        sb.Append("<STYLE>")
        sb.Append(".stdPageMainHdr { color: #808080; text-align: center; padding-left: 4px; padding-top: 4px; padding-bottom: 4px; width: 100%; font-size: 22pt; }")
        sb.Append(".stdPVTblLCell { background-color: #00a7e7; color: #808080; font-weight: bold; text-align: center; padding-left: 4px; padding-top: 4px; padding-bottom: 4px; width: 100%; font-size: 13pt; }")
        sb.Append(".stdPageHdr { color: #808080; font-style:normal;text-align: center; padding-left: 4px; padding-top: 4px; padding-bottom: 4px; width: 100%; font-size: 12pt; }")
        sb.Append(".stdAddlHdr { color: black; font-style:normal;font-weight: bold;text-align: center; padding-left: 4px; padding-top: 4px; width: 100%; font-size: 13.5pt; }")
        sb.Append(".gridHeader { color: #000000;   font-weight: bold;font-size: 12pt;vertical-align:middle; text-align:center; border: solid thin Black; }")
        sb.Append(".subhead { color: #8B0000; font-size: 12pt;border: solid thin Black;text-decoration : underline;}")
        sb.Append(".stot { color: #8B0000; font-size: 12pt; font-weight: normal;border: solid thin Black;}")
        sb.Append(".gtot { color: #2B8B00; font-size: 12pt; font-weight: normal;border: solid thin Black;text-decoration : underline;}")
        sb.Append(".searchresultitem, .searchresultitem- {  color: Black; font-size: 8pt;border: solid thin Black; }")
        sb.Append(".searchresultaltitem {  color: Black; font-size: 8pt;border: solid thin Black; }")
        sb.Append(".stdPageftr { color: #808080; font-style:normal;text-align: center; padding-left: 4px; padding-top: 4px; padding-bottom: 4px; width: 100%; font-size: 12pt; }")
        sb.Append(".right { text-align:right; }")
        sb.Append(".left { text-align:left; }")
        sb.Append(".middle { text-align:midddle; }")
        sb.Append("</STYLE>")
        sb.Append("</head>")
        sb.Append("<body>")
        sb.Append("<table>")
        If Session("AxShowAppTitle") <> Nothing AndAlso Session("AxShowAppTitle").ToString().ToLower() = "true" Then
            If Session("AxAppTitle") <> Nothing AndAlso Session("AxAppTitle") <> String.Empty Then
                sb.Append("<tr><td class='stdPageMainHdr' colspan='" + icolcount.ToString() + "'>" + Session("AxAppTitle").ToString() + "</td></tr>")
            ElseIf Session("projTitle") <> Nothing Then
                sb.Append("<tr><td class='stdPageMainHdr' colspan='" + icolcount.ToString() + "'>" + Session("projTitle").ToString() + "</td></tr>")
            End If
        End If

        Dim format As String = "dd/MM/yyyy"

        If Session("ClientLocale") = "en-us" Then
            format = "MM/dd/yyyy"
        End If



        sb.Append("<tr><td class='stdPageHdr' colspan='" + icolcount.ToString() + "'>" + objIview.IviewCaption + "</td></tr>")
        ' If ivtype <> "listview" And ivtype <> "lview" Then

        If objIview IsNot Nothing And objIview.ReportHdrs.count > 0 Then
            Dim indx As Integer = 0
            For indx = 0 To objIview.ReportHdrs.Count - 1 Step 1
                sb.Append("<tr><td class='tdarrow' colspan='" + icolcount.ToString() + "'><center><font style= " & objIview.ReportHdrStyles(indx).ToString() & ">" & objIview.ReportHdrs(indx).ToString() & "</font></center></td></tr>")
            Next
        End If
        'End If
        sb.Append("<tr><td colspan='" + icolcount.ToString() + "' align='right'> Date :" + time.ToString(format, CultureInfo.InvariantCulture) + "</td></tr>")
        sb.Append("<tr><td colspan='" + icolcount.ToString() + "' align='left'><br style='mso-data-placement:same-cell;'/></td></tr>")
        Dim resnewParam = resparamVal.Split("~")
        If resnewParam.Count > 0 Then
            For Each ppaaram In resnewParam
                sb.Append("<tr><td class='stdAddlHdr' colspan='" + icolcount.ToString() + "'>" + ppaaram + "</td></tr>")
            Next
        End If
        sb.Append("<tr>")
        sb.AppendFormat("{0}", str)
        sb.Append("</tr>")
        sb.Append("<tr>")

        For i As Integer = 1 To icolcount
            sb.Append("<td class='searchresultitem' style='" + exportVerticalAlignStyle + "'></td>")
        Next
        sb.Append("</tr>")
        For Each row As GridViewRow In dgrExport.Rows
            sb.Append("<tr>")
            For i As Integer = 0 To row.Cells.Count - 1
                If objIview.colHide(i) = "false" And objIview.ColNoPrint(i) = "false" Then
                    Dim regex As Regex = New Regex("[0-9.-]*(e|E)[0-9.-]*")
                    Dim match As Match = regex.Match(row.Cells(i).Text)
                    If match.Success And match.Index = 0 Or row.Cells(i).Text.StartsWith("0") Then
                        sb.Append("<td class='searchresultitem' style='" + exportVerticalAlignStyle + ">" + "&nbsp;" & row.Cells(i).Text & "</td>")
                    Else
                        sb.Append("<td class='searchresultitem' style='" + exportVerticalAlignStyle + ">" + row.Cells(i).Text + "</td>")
                    End If
                End If
            Next
            sb.Append("</tr>")
        Next

        If objIview.ReportFtrs.Count > 0 Then
            Dim iv As Integer
            sb.Append("<tr>")
            For iv = 0 To objIview.ReportFtrs.Count - 1 Step 1
                sb.Append("<td class='stdPageftr' colspan='" + icolcount.ToString() + "'><br/><br style='mso-data-placement:same-cell;'/>" + objIview.ReportFtrs(iv).ToString() + "</td>")
            Next
            sb.Append("</tr>")
        End If
        sb.Append("</table>")
        sb.Append("</body>")
        sb.Append("</HTML>")
        Return sb.ToString()
    End Function
End Class
