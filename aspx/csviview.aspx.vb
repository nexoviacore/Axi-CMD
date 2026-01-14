Imports System.Xml
Imports System.Text
Imports System.IO
Imports Util
Imports System.Data

Partial Class csviview
    Inherits System.Web.UI.Page
    Public sb As StringBuilder = New StringBuilder()
    Dim _xmlString As String =
                   "<?xml version=""1.0"" encoding=""utf-8"" ?>"
    Dim util As Util.Util = New Util.Util()
    Dim logobj As LogFile.Log = New LogFile.Log()
    Dim colHide As New ArrayList()
    Dim colFld As New ArrayList()
    Dim numCol As New ArrayList()
    Dim colHead As New ArrayList()
    Dim iname As String
    Dim user As String
    Dim sid As String
    Dim ivtype As String
    Dim proj As String
    Dim paramVal As String
    Dim paramXml As String = ""
    Dim pivotGroupHeaderNames As New ArrayList()
    Dim pivotStartCol As New ArrayList()
    Dim pivotEndCol As New ArrayList()
    Dim htmlColumns As New ArrayList()
    Dim objIview
    Dim Ikey As String = String.Empty
    Dim listres As String = String.Empty



    Private Function GetCSV() As String
        If Not IsPostBack Then
            iname = Request.QueryString("ivname")
            user = Session("user")
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

            If iname = Nothing And Session("ivname") <> Nothing Then
                iname = Session("ivname").ToString()
            End If
            If paramVal = Nothing And Session("params") <> Nothing Then
                paramVal = Session("params").ToString()
            End If
            If ivtype = Nothing And Session("ivtype") <> Nothing Then
                ivtype = Session("ivtype").ToString()
            End If
            If paramVal <> Nothing Then
                paramVal = paramVal.Replace("^^sq", "'")
            End If

            ' If ivtype = "listview" Or ivtype = "lview" Then
            '     objIview = New ListviewData()
            '     objIview = CType(Session(Ikey), ListviewData)
            ' Else
                objIview = New IviewData()
                objIview = CType(Session(Ikey), IviewData)
            ' End If

            ViewState("ivtype") = ivtype
            ViewState("params") = paramVal
            proj = Session("project")
            ViewState("iname") = iname
            ViewState("ivtype") = ivtype
            ViewState("proj") = proj
            ViewState("user") = user
            ViewState("sid") = sid
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


        GetParam(paramVal)

        Dim sortCol As String = String.Empty
        Dim sortOrd As String = String.Empty
        Dim filterCol As String = String.Empty
        Dim filterColVal As String = String.Empty
        Dim filterValue1 As String = String.Empty
        Dim filterOpr As String = String.Empty
        Dim header1 As String = String.Empty
        Dim header2 As String = String.Empty
        Dim header3 As String = String.Empty
        Dim footer1 As String = String.Empty
        Dim footer2 As String = String.Empty



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

        Dim fileName As String = "GetLView" & iname
        Dim errorLog As String = logobj.CreateLog("Getting CsvIview.", sid, fileName, "new")
        Dim iXml As String
        iXml = "<root name =""" & iname & """ axpapp =""" & Session("project") & """ appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " sessionid = """ & sid & """ trace =" & Chr(34) & errorLog & Chr(34) & " pageno=""0"" firsttime=""yes"" sorder=""" & sortOrd & """ scol=""" & sortCol & """ fcolopr=""" + filterOpr + """ fcolnm=""" & filterCol & """ fcolval1=""" + filterColVal + """ fcolval2=""" + filterValue1 + """><params> "
        iXml = iXml & paramXml
        iXml = iXml & "</params>"
        iXml &= Session("axApps").ToString() + Application("axProps").ToString() + Session("axGlobalVars").ToString() + Session("axUserVars").ToString() & "</root>"
        Dim ires As String = ""
        Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()

        'Call service
        If ivtype = "listview" Or ivtype = "lview" Then

            If objIview.FilterXml Is String.Empty Then                 'Checking for filtering in listview
                ires = objWebServiceExt.CallGetLViewWS(iname, iXml, objIview.WebServiceTimeout)
            Else
                ires = objIview.FilterXml.ToString()
            End If
            If objIview.Listres IsNot String.Empty Then
                listres = objIview.Listres
            End If

            Dim index As Integer = -1
            index = ires.IndexOf(">")
            ires = ires.Insert(index + 1, listres)
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
                ires = ires.Insert(7, res)

            End If
        End If

        If ires <> Constants.ERROR Or Mid(ires, 1, 7) <> Constants.ERROR Then

            'Fix for BAF-000244 -Replace the '<B>,</B>,<BR>, </BR>' tags before creating the csv file.
            ires = util.ReplaceTextAreaChars(ires, "csv")
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
                For Each baseDataNode As XmlNode In baseDataNodes
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
                    ElseIf baseDataNode.Name = "rowno" Then
                        colFld.Add(baseDataNode.Name)
                        colHead.Add(baseDataNode.InnerText)
                        colHide.Add("false")
                    Else
                        If baseDataNode.Name.StartsWith("html_") Then
                            htmlColumns.Add(baseDataNode.Name)
                        End If
                        colFld.Add(baseDataNode.Name)
                        colHead.Add(baseDataNode.InnerText)
                        If Not baseDataNode.Attributes("hide") Is Nothing Then
                            If baseDataNode.Name.StartsWith("hide_") Then
                                colHide.Add("true")
                            Else
                                colHide.Add(baseDataNode.Attributes("hide").Value)
                            End If

                            '' to check numeric columns
                            If baseDataNode.Attributes("hide").Value = "false" Then
                                If baseDataNode.Attributes("type").Value = "n" Then
                                    numCol.Add(baseDataNode.Name.ToString())
                                End If
                            End If
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
                        'Remove pivot header from headrow node.
                        If baseDataNode3.Name = "pivotghead" Then
                            baseDataNode3.ParentNode.RemoveChild(baseDataNode3)
                        End If
                        For hidx = 0 To colFld.Count - 1
                            If colFld(hidx) = baseDataNode3.Name Then
                                If colHide(hidx) = "true" Then
                                    If baseDataNode3.Name = "axrowtype" Then
                                        baseDataNode3.ParentNode.RemoveChild(baseDataNode3)
                                    ElseIf baseDataNode3.Name = "axp__font" Then
                                        baseDataNode3.ParentNode.RemoveChild(baseDataNode3)
                                    End If
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
                            If colFld(idx) = baseDataNode1.Name Then
                                If colHide(idx) = "true" Then
                                    If colFld(idx) = "axp__font" Then
                                        baseDataNode1.ParentNode.RemoveChild(baseDataNode1)
                                    ElseIf colFld(idx) = "axrowtype" Then
                                        baseDataNode1.ParentNode.RemoveChild(baseDataNode1)
                                    End If
                                End If
                            End If
                        Next
                    Next
                Next
            Next

            'create pivot headers
            If Session("AxShowAppTitle") <> Nothing And Session("AxShowAppTitle").ToString().ToLower() = "true" Then
                Dim headerText As String = ""
                If Session("AxAppTitle") <> Nothing AndAlso Session("AxAppTitle") <> String.Empty Then
                    headerText = Session("AxAppTitle").ToString()
                ElseIf Session("projTitle") <> Nothing Then
                    headerText = Session("projTitle").ToString()
                End If
                If headerText <> "" Then
                    sb.Append(headerText)
                    sb.AppendLine("")
                    sb.AppendLine("")
                End If
            End If

            If objIview.ReportHdrs.Count > 0 Then
                Dim indx As Integer
                For indx = 0 To objIview.ReportHdrs.Count - 1 Step 1
                    sb.Append(objIview.ReportHdrs(indx).ToString())
                    sb.AppendLine("")
                Next
            End If


            Dim pivotRow As String = String.Empty
            If pivotGroupHeaderNames.Count > 1 Then
                Dim inc As Integer
                Dim val As String = String.Empty
                Dim colSpan As Integer
                For inc = 0 To pivotGroupHeaderNames.Count - 1
                    colSpan = CInt(pivotEndCol(inc).ToString) - CInt(pivotStartCol(inc).ToString)
                    Dim x As Integer
                    If colSpan > 1 Then
                        For x = 1 To colSpan
                            val = val & ","
                            x += 1
                        Next
                    End If
                    val = pivotGroupHeaderNames(inc).ToString() & "," & val
                    sb.Append(val)
                    val = ""
                Next
                sb.Append(vbCrLf)
            End If



            'Add Header 
            Dim dproductNodes1 As XmlNodeList
            Dim dproductNode1 As XmlNode
            Dim dbaseDataNodes1 As XmlNodeList

            dproductNodes1 = xmlDoc.SelectNodes("/data/headrow")
            For Each dproductNode1 In dproductNodes1
                dbaseDataNodes1 = dproductNode1.ChildNodes
                Dim colno As Integer
                colno = 1
                For Each dbaseDataNode As XmlNode In dbaseDataNodes1
                    If colno > 2 Then  ' to add "," from second column
                        sb.Append(",")
                    End If
                    If colno = 1 Then  ' to omit slno in csv
                        If ivtype = "listview" Or ivtype = "lview" Then

                            If dbaseDataNode.Name = "rowno" Then
                                sb.Append("""")
                                sb.Append("Sr.no")
                                sb.Append("""")
                                sb.Append(",")
                            End If
                        End If

                    Else

                        sb.Append("""")
                        If dbaseDataNode.InnerText = "axp_slno" Then
                            sb.Append("Sr.no")
                        Else
                            sb.Append(dbaseDataNode.InnerText)
                        End If
                        sb.Append("""")


                    End If
                    colno = colno + 1
                Next
                sb.Append(vbCrLf)
            Next
            ' End Header

            Dim dproductNodes As XmlNodeList
            Dim dproductNode As XmlNode
            Dim dbaseDataNodes As XmlNodeList

            dproductNodes = xmlDoc.SelectNodes("/data/row")
            For Each dproductNode In dproductNodes
                dbaseDataNodes = dproductNode.ChildNodes
                Dim colno As Integer
                colno = 1
                For Each dbaseDataNode As XmlNode In dbaseDataNodes
                    If colno > 2 Then  ' to add "," from second column
                        sb.Append(",")
                    End If
                    If colno = 1 Then  ' to omit slno in csv
                        If ivtype = "listview" Or ivtype = "lview" Then
                            If dbaseDataNode.Name = "rowno" Then
                                sb.Append("""")
                                Dim appStr = dbaseDataNode.InnerText
                                sb.Append(appStr.PadLeft(5))
                                sb.Append("""")
                                sb.Append(",")
                            End If
                        End If
                    Else
                        sb.Append("""")
                        Dim appStr = dbaseDataNode.InnerText

                        If ((numCol.IndexOf(dbaseDataNode.Name.ToString())) > -1 And appStr.IndexOf(",") > -1) Then
                            appStr = appStr.Replace(",", "")
                        End If

                        appStr = appStr.Replace(Chr(10), " ")
                        sb.Append(appStr)
                        sb.Append("""")

                    End If
                    colno = colno + 1
                Next
                sb.Append(vbCrLf)
                'rowno = rowno + 1
            Next

            If objIview.ReportFtrs.Count > 0 Then
                Dim indx As Integer
                For indx = 0 To objIview.ReportFtrs.Count - 1 Step 1
                    sb.Append(objIview.ReportFtrs(indx).ToString())
                    sb.AppendLine("")
                Next
            End If

            Return sb.ToString()
        Else
            Return "IView Result Error"
        End If
    End Function

    Private Sub GetParam(ByVal param As String)
        Dim k As Integer

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
                    arrParams = arrNoOfParams(i).ToString().Split("♠")
                    arrLstParams.Insert(i, arrParams(0))
                    If arrParams(1).Contains("`") = True Then
                        arrParams(1) = arrParams(1).Replace("`", ",")
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
                Else
                    paramVal = ""
                End If
            End If

            If param <> "" Then
                paramXml = paramXml & "<" & tem1 & ">"
                paramXml = paramXml & util.CheckSpecialCharsSaveAs(paramVal)
                paramXml = paramXml & "</" & tem1 & ">"

                params = params & "&" & tem1 & "="
                params = params & paramVal
            Else
                paramXml = "<params>"
            End If

        Next
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Session("project") = "" Then
            sess_expires()
        Else

            If Session("ivKey") <> Nothing Then
                Ikey = Session("ivKey").ToString()

            Else
                If Not Request.QueryString("ivKey") Is Nothing Then
                    Ikey = Request.QueryString("ivKey")

                End If
            End If

        End If
        Response.Clear()
        Response.ContentType = "application/octet-stream"
        Response.Charset = ""
        Dim result As String = GetCSV()
        Dim strFileName As String = iname & ".csv"
        Response.AppendHeader("Content-Disposition", "inline;filename=" & strFileName)
        Response.ContentEncoding = System.Text.Encoding.Unicode
        Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        Response.Write(result)
        Response.End()
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

    Public Sub sess_expires()
        Dim url As String = util.SESSEXPIRYPATH
        Response.Write("<script language=""javascript"">" & vbCrLf)
        Response.Write("parent.parent.location.href='" & url & "';")
        Response.Write(vbCrLf & "</script>")
    End Sub
End Class
