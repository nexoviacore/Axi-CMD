Imports System.Xml
Imports System.IO
Imports System.Data
Imports Newtonsoft.Json

Partial Class ivpicklist
    Inherits System.Web.UI.Page
    Dim name As String = ""
    Dim val As String = String.Empty
    Dim iXml As String = ""
    Dim ires As String = ""
    Dim _xmlString As String =
      "<?xml version=""1.0"" encoding=""utf-8"" ?>"
    Dim headName As New ArrayList()
    Dim fldlist As String = ""
    Public resrows As Integer
    Dim calledFrom As String = ""
    Dim unitName As String = ""
    Dim totalRows As Integer
    Dim util As Util.Util = New Util.Util()
    Dim logobj As LogFile.Log = New LogFile.Log()
    Dim sysErrorlog As Boolean = False
    Public sqlsearch As String = ""
    Dim pXml As String = String.Empty
    Public errlog As String
    Public EnableOldTheme As String
    Public direction As String = "ltr"

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

        If Session("AxEnableOldTheme") IsNot Nothing Then
            EnableOldTheme = Session("AxEnableOldTheme").ToString()
        End If

        If Session("project") = "" Then
            sysErrorlog = Convert.ToBoolean(Session("trace"))
            Dim url As String
            url = "sess.aspx"
            Response.Write("<script>" & vbCrLf)
            Response.Write("parent.parent.location.href='" & url & "';")
            Response.Write(vbCrLf & "</script>")
        Else
            If Request.UrlReferrer IsNot Nothing Then
                If Not (Request.UrlReferrer.AbsolutePath.ToLower().Contains("iview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("ivpicklist.aspx")) Then
                    Response.Redirect("../CusError/AxCustomError.aspx")
                End If
            End If
            If (Not IsPostBack) Then
                If Session("language") <> Nothing Then
                    If Session("language").ToString() = "ARABIC" Then
                        direction = "rtl"
                    End If
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "var gl_language='" & Session("language") & "';", True)
                End If
                sqlsearch = Request.QueryString("sqlsearch")
                If Not util.IsUserNameValid(sqlsearch) Then
                    Response.Redirect(Constants.PARAMERR)
                End If

                srchFld.Value = sqlsearch
                Dim isPlDepParBound As String = Request.QueryString("isPlDepParBound")
                If isPlDepParBound <> "true" Then
                    callWebservice("1")
                End If
            End If
        End If
    End Sub
    Public Sub callWebservice(ByVal pgno As String)

        iXml = ""
        Dim tempSearch As String = ""
        tempSearch = Request.QueryString("searchval")


        Dim ivName As String
        ivName = Request.QueryString("ivname")
        If Not util.IsValidIvName(ivName) Then
            Response.Redirect(Constants.PARAMERR)
        End If

        Dim filename As String
        filename = "iviewpicklist-" & ivName
        If ((tempSearch = "")) Then
            Messagebox(4000)
        Else
            If (tempSearch <> "") Then
                tempSearch = tempSearch
            End If

            tempSearch = CheckSpecialChars(tempSearch)

            logobj.CreateLog("Call to GetIView Web Service for page no " & pgno, Session("nsessionid"), filename, "")
            logobj.CreateLog("Start Time " & Now.ToString(), Session("nsessionid"), filename, "")
            errlog = logobj.CreateLog("", Session("nsessionid"), filename, "")

            iXml = iXml & "<sqlresultset axpapp=" & Chr(34) & Session("project") & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " value=" & Chr(34) & tempSearch & Chr(34) & " sessionid= " & Chr(34) & Session("nsessionid") & Chr(34) & " pname=" & Chr(34) & srchFld.Value & Chr(34) & " trace=""" & errlog & """ user=" & Chr(34) & Session("user") & Chr(34) & " ivname=" & Chr(34) & ivName & Chr(34) & "  pageno=""" & pgno & """ pagesize=""" & GridView1.PageSize & """>"

            If (ViewState("inputParam") <> Nothing) Then
                pXml = ViewState("inputParam").ToString()
                iXml = iXml & "<varlist>" & pXml & "</varlist>"
            End If
            iXml = iXml & Session("axApps").ToString() & Application("axProps").ToString() & Session("axGlobalVars").ToString() & Session("axUserVars").ToString()
            iXml = iXml & "</sqlresultset>"

            Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
            ires = objWebServiceExt.CallGetParamChoicesWS(ivName, iXml)

            If ires = "" Then
                dvPickList.Visible = False
            Else
                ires = _xmlString & ires
                Dim xmlDoc1 As New XmlDocument
                xmlDoc1.LoadXml(ires)

                Dim compNodes As XmlNodeList
                Dim cNode As XmlNode

                compNodes = xmlDoc1.SelectNodes("//sqlresultset/response")
                Dim totalRows As Integer
                For Each cNode In compNodes
                    If pgno = "1" Then
                        Dim tnode As XmlNode = cNode.Attributes("totalrows")
                        If tnode Is Nothing Then
                            totalRows = 0
                        Else
                            totalRows = CInt(tnode.Value)
                            'If (Session("ivname") = "cpf_bdst") Or (Session("ivname") = "nps_bdst") Or (Session("ivname") = "gpf_brsh") Or (Session("ivname") = "GisBrdSH") Then
                            '    totalRows = totalRows + 1
                            'End If
                            cNode.Attributes.RemoveNamedItem("totalrows")
                        End If
                        Session("pl_noofpages") = totalRows
                    Else
                        totalRows = Session("pl_noofpages")
                    End If
                Next

                Dim sw As New StringWriter()
                Dim xw As New XmlTextWriter(sw)
                xmlDoc1.WriteTo(xw)

                Dim ires2 As String = ""
                ires2 = sw.ToString()


                searchlist.Items.Clear()
                searchlistval.Items.Clear()
                BindDataGrid(ires2, totalRows, pgno)
            End If
        End If
    End Sub
    Protected Sub Messagebox(ByVal Msg As String)
        Page.ClientScript.RegisterStartupScript([GetType](), "myrest", "<script>showAlertDialog('info'," + Msg + ",'client');</script>")
    End Sub
    Private Function CheckSpecialChars(ByVal str As String) As String
        If str <> Nothing Then
            str = Regex.Replace(str, "<", "&lt;")
            str = Regex.Replace(str, "&", "&amp;")
            str = Regex.Replace(str, ">", "&gt;")
            str = Regex.Replace(str, "'", "&apos;")
        End If
        Return str
    End Function
    Private Sub BindDataGrid(ByVal a As String, ByVal totRows As Integer, ByVal pageno As String)
        Dim ds As New DataSet()
        Dim sr As New System.IO.StringReader(a)
        ds.ReadXml(sr)

        If (ds.Tables().Count <= 1) Then
            Message()
        Else
            Panel1.Visible = True
            Dim value As String = ""
            Dim loadxmlDoc As New XmlDocument
            loadxmlDoc.LoadXml(a)
            Dim WrproductNodes As XmlNodeList
            WrproductNodes = loadxmlDoc.GetElementsByTagName("response")
            If (WrproductNodes.Count = 0) Then

            Else
                For Each WrproductNode As XmlNode In WrproductNodes
                    If (Not WrproductNode.Attributes("value") Is Nothing) Then
                        value = WrproductNode.Attributes("value").Value
                    End If
                Next
            End If

            Dim fields As String = ""
            Dim getnodes As XmlNodeList
            getnodes = loadxmlDoc.GetElementsByTagName("sqlresultset")

            hdnIViewData2.Value = JsonConvert.SerializeXmlNode(loadxmlDoc)


            Dim idcol As String = ""
            Dim map As String = ""
            For Each getnode As XmlNode In getnodes
                'Added condition for checking Null in case of workflow delegation, it gives error.
                If Not getnode.Attributes("idcol") Is Nothing Then
                    idcol = getnode.Attributes("idcol").Value

                End If
                If Not getnode.Attributes("map") Is Nothing Then
                    map = getnode.Attributes("map").Value
                End If
            Next


            Dim mapstr As String = ""
            Dim j As Integer = 0
            If (map <> "") Then
                Dim mapfield = map.Split(",")

                Dim mapnodes As XmlNodeList
                Dim childmapnodes As XmlNodeList
                mapnodes = loadxmlDoc.GetElementsByTagName("row")
                For Each mapnode As XmlNode In mapnodes
                    childmapnodes = mapnode.ChildNodes
                    For Each childmapnode As XmlNode In childmapnodes
                        For j = 0 To mapfield.Length - 1
                            Dim mpfld = mapfield(j).ToString()
                            Dim mpfldArr = mpfld.Split("=")
                            If childmapnode.Name.ToString().ToLower() = mpfldArr(0).ToString().ToLower() Then

                                mapstr &= "~" & mpfldArr(1).ToString() & "***" & childmapnode.InnerText.ToString()

                            End If
                        Next

                    Next
                    fldlist &= "#" & mapstr
                    mapstr = ""
                Next
            End If

            Dim i As Integer = 0
            If (value <> "--") Then
                Dim rownodes As XmlNodeList
                Dim childnodes As XmlNodeList
                rownodes = loadxmlDoc.GetElementsByTagName("row")
                For Each rownode As XmlNode In rownodes
                    childnodes = rownode.ChildNodes
                    For Each chilnode As XmlNode In childnodes
                        i = i + 1
                        If ((idcol = "yes")) Then
                            If (i = 2) Then
                                name = name & "~" & chilnode.InnerText.ToString()
                            End If
                        Else
                            If (i = 1) Then
                                name = name & "~" & chilnode.InnerText.ToString()
                            End If
                        End If

                        If calledFrom = "workflow" And (i = 2) Then
                            val = val & "~" & chilnode.InnerText.ToString()
                        End If
                    Next
                    i = 0
                Next

                If calledFrom = "workflow" Then
                    Dim empValues = val.Split("~")
                    For k As Integer = 1 To empValues.Length - 1
                        lstValues.Items.Add(empValues(k).ToString())
                    Next
                End If
                Dim wfname = name.Split("~")
                For m As Integer = 1 To wfname.Length - 1
                    searchlist.Visible = True
                    searchlist.Items.Add(wfname(m).ToString())
                Next

                Dim wflist = fldlist.Split("#")
                For mn As Integer = 1 To wflist.Length - 1
                    searchlistval.Items.Add(wflist(mn).ToString())
                Next

            End If
            setGv(a, totRows, pageno)
        End If
    End Sub
    'For grid view
    Private Sub setGv(ByVal a As String, ByVal totRows As Integer, ByVal pageno As String)
        Dim xmlDoc1 As New XmlDocument
        xmlDoc1.LoadXml(a)
        Dim cnodes As XmlNodeList

        Dim cNode As XmlNode
        cNode = xmlDoc1.SelectSingleNode("//response")

        cnodes = cNode.ChildNodes
        Dim sw As New StringWriter()
        Dim xw As New XmlTextWriter(sw)
        cNode.WriteTo(xw)
        Dim nXml As String = sw.ToString()
        'If (Session("ivname") = "cpf_bdst") Or (Session("ivname") = "nps_bdst") Or (Session("ivname") = "gpf_brsh") Or (Session("ivname") = "GisBrdSH") Then
        '    nXml = nXml.Replace("<response>", "<response><row><DDO_NAME>ALL</DDO_NAME></row>")
        'End If
        nXml = nXml.Replace("<row>", "<row><SNO></SNO><SELECT></SELECT>")
        If nXml = "<response/>" Or cnodes.Count = 0 Then
            Message()
        Else
            Panel1.Visible = True
            BindDataGrid1(nXml, totRows, pageno)
        End If

    End Sub

    Private Sub BindDataGrid1(ByVal a As String, ByVal totRows As Integer, ByVal pageno As String)

        GridView1.Columns.Clear()
        Dim ds As New DataSet()
        Dim sr As New StringReader(a)

        ds.ReadXml(sr)

        ' Important : the datasource store in session as datatable. for paging and sorting
        ' IMP : Create a new dataset - use clone - which create new structure then change
        '       Column datatype to int, double,string and date - which is needed for Sorting
        Dim n As Integer
        n = 0
        Dim ds1 As New DataSet()
        ds1 = ds.Clone()
        For Each dc1 As DataColumn In ds1.Tables(0).Columns

            Try
                dc1.DataType = GetType(String)
            Catch ex As Exception
                dc1.DataType = GetType(Int32)
            End Try

            n = n + 1
        Next

        'Dim culture As System.Globalization.CultureInfo
        'culture = New System.Globalization.CultureInfo("en-GB")
        Dim rno As Integer
        rno = 0
        For Each dr1 As DataRow In ds.Tables(0).Rows

            ' Before import ds to ds1 change the row value from str to date while datacol type is date
            ' rno for find row no and id for col ... make new date then attach to dr1 -datarow then import 
            ds1.Tables(0).ImportRow(dr1)
            rno = rno + 1
        Next


        Session("order") = ds1.Tables(0)
        resrows = ds1.Tables(0).Rows.Count

        Dim i As Integer = 0
        Dim dc As DataColumn
        If ds1.Tables.Count > 0 Then
            For Each dc In ds1.Tables(0).Columns

                Dim bfield As New BoundField
                ''initialiae the data field value
                bfield.DataField = dc.ColumnName
                ''initialise the header text value
                ''To remove the "_" from the sql columnname replace is used.
                bfield.HeaderText = Regex.Replace(dc.ColumnName, "_", " ")
                '' add newly created columns to gridview
                If dc.ColumnName = "SELECT" Then
                    bfield.HeaderText = String.Empty
                End If

                GridView1.Columns.Add(bfield)

            Next
        End If

        GridView1.DataSource = ds1
        ' to change the header Name and set the column width
        Dim idx As Integer
        For idx = 0 To headName.Count - 1

            ' For change the Column Heading from fld name to Caption
            GridView1.Columns(idx).HeaderText = headName(idx)
        Next



        GridView1.DataBind()
        Dim pg As Double = CInt(totRows) / CInt(GridView1.PageSize)
        Dim pg1 As Integer = Math.Floor(pg)
        If (totRows Mod GridView1.PageSize) > 0 Then
            pg1 += 1
        End If

        If totRows > 0 Then
            Panel2.Visible = True
            records.Text = "No. of records : " & totRows & " - " & "Pages : " & pg1
            pgCap.Visible = True
            lvPage.Visible = True
            lblErrMsg.Text = ""
        Else
            lblErrMsg.Text = lblNodata.Text
            records.Visible = False
            pgCap.Visible = False
            lvPage.Visible = False
        End If


        Dim Newpgno As Integer
        lvPage.Items.Clear()
        Newpgno = Convert.ToInt32(pageno)
        For Newpgno = 1 To pg1
            lvPage.Items.Add(Newpgno.ToString())
        Next
        lvPage.SelectedValue = pageno
    End Sub
    Private Sub Message()
        searchlist.Visible = False
        Panel1.Visible = False
        Panel2.Visible = False
        lblErrMsg.Visible = True
        lblErrMsg.Text = lblNodata.Text
        records.Text = ""
        pgCap.Visible = False
        lvPage.Visible = False
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

        Dim p As String = GridView1.PageIndex

        Dim drv As System.Data.DataRowView
        drv = CType(e.Row.DataItem, System.Data.DataRowView)
        If e.Row.RowType = DataControlRowType.DataRow Then
            If drv IsNot Nothing Then
                ' first change  col 1 to check box
                Dim catName As String = e.Row.RowIndex.ToString()
                ' for change the content to component like check box or input box
                'to Remove checkbox from first column
                Dim fld As String = Request.QueryString("sqlsearch")

                e.Row.Cells(1).Text = "<input class=""form-check-input"" type=radio value=" & catName & " onclick=loadParent(this.value,'" & fld & "');>"
                Dim n As Integer
                For n = 0 To e.Row.Cells.Count - 1
                    If e.Row.Cells(n).Text = "*" Then
                        e.Row.Cells(n).Text = ""
                    End If
                Next
            End If
        End If
        '' page is loading first time
        If ViewState("PageNo") Is Nothing Then
            If e.Row.RowIndex = -1 Then
            Else
                e.Row.Cells(0).Text = Convert.ToString(((e.Row.RowIndex) + 1))
            End If
        End If
        ''page index changed
        If ViewState("PageNo") IsNot Nothing Then
            If e.Row.RowIndex = -1 Then
            Else
                If ViewState("PageNo") = 1 Then
                    e.Row.Cells(0).Text = Convert.ToString(((e.Row.RowIndex) + 1))
                Else
                    e.Row.Cells(0).Text = Convert.ToString((Convert.ToInt32(ViewState("PageNo")) - 1)) + Convert.ToString(((e.Row.RowIndex) + 1))

                    If e.Row.RowIndex = 9 Then
                        e.Row.Cells(0).Text = Convert.ToString(ViewState("PageNo")) + "0"
                    End If
                End If
            End If
        End If
        ''End
        'e.Row.Cells(0).Width = "20"
        'e.Row.Cells(1).Width = "30"
        'for NOWRAP in IE
        Dim m As Integer
        For m = 0 To e.Row.Cells.Count - 1
            If e.Row.Cells(m).Text.Length > 0 Then
                e.Row.Cells(m).Text = "<nobr>" + e.Row.Cells(m).Text + "</nobr>"
            End If
        Next
    End Sub
    Protected Sub lvPage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvPage.SelectedIndexChanged
        Dim pgNo As String = lvPage.SelectedValue
        ViewState("PageNo") = pgNo
        callWebservice(pgNo)
    End Sub

    Protected Sub btnTemp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTemp.Click

        Dim strP As String = paramXml.Value
        strP = strP.Replace("###", "^")
        Dim strPXml As String() = strP.Split("^")

        For i As Integer = 0 To strPXml.Length - 1
            If strPXml(i) <> "" Then
                Dim pDetails As String() = strPXml(i).Split("¿")
                If pDetails(0) <> "" Then
                    pXml += "<" & pDetails(0) & ">" & pDetails(1) & "</" & pDetails(0) & ">"
                End If
            End If
        Next
        pXml = InPutXmlSpChar(pXml)
        ViewState("inputParam") = pXml
        callWebservice("1")


    End Sub
    Private Function InPutXmlSpChar(ByVal str As String) As String
        If str <> Nothing Then
            str = Regex.Replace(str, "&", "&amp;")
            str = Regex.Replace(str, "'", "&apos;")
            str = Regex.Replace(str, "\n", "<br>")
            str = Regex.Replace(str, "™", "&#8482;")
            str = Regex.Replace(str, "®", "&#174;")
        End If
        Return str
    End Function

End Class
