Imports System.Xml
Imports System.Data
Imports System.IO
Imports CacheMgr


Partial Class srchComponent
    Inherits System.Web.UI.Page
    Public EnableOldTheme As String
    Dim name As String = ""
    Dim iXml As String = ""
    Dim search As String = ""
    Dim fldname As String = ""
    Dim ires As String = ""
    Dim field As String = ""
    Dim tid As String = ""
    Dim sid As String = ""
    Dim proj As String = ""
    Dim language As String = ""
    Dim AxRole As String = ""
    Dim user As String = ""
    Dim fieldxml As String = ""
    Dim fldlist As String = ""
    Dim idcol As String = ""
    Dim _xmlString As String =
      "<?xml version=""1.0"" encoding=""utf-8"" ?>"
    Dim headName As New ArrayList()
    Dim logobj As LogFile.Log = New LogFile.Log()
    Dim util As Util.Util = New Util.Util()
    Dim cm As CacheManager
    Public resrows As Integer
    Public tstStructXml As String = String.Empty
    Dim calledFrom As String = ""
    Dim unitName As String = ""
    Dim val As String = String.Empty
    Public direction As String = "ltr"
    Public clssdir As String = "left"
    Dim activeRow As String = ""
    Dim frameno As String = ""
    Dim objKey As String = ""
    Dim prow As String = ""
    Dim pdc As String = ""
    Dim parInfo As String = ""
    Dim subInfo As String = ""
    Dim arrHiddenCols As New ArrayList

    ''' <summary>
    ''' onload fill the search columns as items into a dropdown list.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
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
            Dim url As String
            url = util.SESSEXPIRYPATH
            Response.Write("<script>" & vbCrLf)
            Response.Write("if(window.opener && !window.opener.closed){window.opener.parent.location.href='" + url + "';window.close();}else {parent.parent.location.href='" + url + "';}")
            Response.Write(vbCrLf & "</script>")
        Else
            If Request.UrlReferrer IsNot Nothing Then
                If Not (Request.UrlReferrer.AbsolutePath.ToLower().Contains("tstruct.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("srchcomponent.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("users.aspx")) Then
                    Response.Redirect("../CusError/AxCustomError.aspx")
                End If
            End If
            If (Not IsPostBack) Then

                ViewState("pageno") = ""
                search = Request.QueryString("search")
                search = CheckSpecialChars(search)
                fldname = Request.QueryString("fldname")

                If Not util.IsSearchFieldNameValid(fldname) Then
                    Response.Redirect(Constants.PARAMERR)
                End If

                activeRow = Convert.ToString(Request.QueryString("activeRow"))
                If Not activeRow = Nothing Then
                    If Not util.IsNumber(activeRow) Then
                        Response.Redirect(Constants.PARAMERR)
                    End If
                End If


                frameno = Convert.ToString(Request.QueryString("frameno"))
                If Not frameno = Nothing Then
                    If Not util.IsNumber(frameno) Then
                        Response.Redirect(Constants.PARAMERR)
                    End If
                End If

                objKey = Convert.ToString(Request.QueryString("key"))
                If Not objKey = Nothing Then
                    If Not util.IsAlphaNumUnd(objKey) Then
                        Response.Redirect(Constants.PARAMERR)
                    End If
                End If

                If Not Request.QueryString("parStr") Is Nothing Then

                    Dim parStr As String = ""
                    parStr = Request.QueryString("parStr")

                    If Not util.IsAlphaNum(parStr) Then
                        Response.Redirect(Constants.PARAMERR)
                    End If

                    parInfo = parStr
                    Dim idx As Integer = -1
                    idx = parStr.IndexOf("~")
                    If (parStr <> "" And idx <> -1) Then
                        pdc = parStr.Substring(0, idx)
                        prow = parStr.Substring(idx + 1)
                    End If
                End If

                If Not Request.QueryString("subStr") Is Nothing Then
                    subInfo = Request.QueryString("subStr")
                    If Not util.IsAlphaNum(subInfo) Then
                        Response.Redirect(Constants.PARAMERR)
                    End If
                End If

                If fldname.LastIndexOf("F") = -1 And fldname <> "" And fldname.Length > 5 Then
                    field = fldname.Substring(0, fldname.Length - 5)
                Else
                    field = fldname.Substring(0, fldname.LastIndexOf("F") - 3)
                End If
                ViewState("key") = objKey
                ViewState("activeRow") = activeRow
                ViewState("frameno") = frameno
                ViewState("fldname") = fldname
                ViewState("field") = field
                ViewState("pdc") = pdc
                ViewState("prow") = prow
                ViewState("parInfo") = parInfo
                ViewState("subInfo") = subInfo
                fname.Value = ViewState("fldname").ToString()
                tid = Request.QueryString("transid")
                ViewState("tid") = tid
                fieldxml = Request.QueryString("fldxml")
                ViewState("fldxml") = fieldxml
                sid = Session("nsessionid")
                proj = Session("project")
                AxRole = Session("AxRole")
                user = Session("user")

                If Not Request.QueryString("pagename") Is Nothing Then
                    calledFrom = Regex.Replace(Request.QueryString("pagename"), "[^0-9a-zA-Z]+", "") 'Blind SQL Injection
                End If
                If Not Request.QueryString("unit") Is Nothing Then
                    unitName = Request.QueryString("unit")
                End If
                ViewState("pageName") = calledFrom
                ViewState("unit") = unitName
                'Normally s1 is filled by webservice, but here for workflow delegation
                'It is fixed.Adding items to the Search Cols dropdown.
                If calledFrom = "workflow" Then
                    s1.Items.Add("User Name")
                    s1.Items.Add("Employee Code")
                Else
                    cm = GetCacheObject()
                    Dim strObj As TStructDef = GetStrObject(cm)
                    tstStructXml = strObj.structRes
                    Dim srchCols As New ArrayList()
                    srchCols = GetSearchCols()
                    BindCombo(srchCols)
                    ViewState("structXml") = tstStructXml
                    fname.Value = fldname
                End If
            Else
                calledFrom = ViewState("pageName")
                unitName = ViewState("unit")
                pdc = ViewState("pdc")
                prow = ViewState("prow")
                parInfo = ViewState("parInfo")
                subInfo = ViewState("subInfo")
                activeRow = ViewState("activeRow")
                frameno = ViewState("frameno")
                fldname = ViewState("fldname")
                field = ViewState("field")
                'fname.Value = ViewState("fldname")
            End If
            SetLangStyles()
            searchlist.Attributes.Add("onchange", "SetSelectedValue(this,'" & searchlistval.ClientID & "');")
        End If
    End Sub
    ''' <summary>
    ''' function to replace the special characters in a given string.
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Private Function CheckSpecialChars(ByVal str As String) As String
        If str <> Nothing Then
            str = Regex.Replace(str, "amp;", "&")
            str = Regex.Replace(str, "hash;", "#")
            str = Regex.Replace(str, "&", "&amp;")
            str = Regex.Replace(str, "<", "&lt;")
            str = Regex.Replace(str, ">", "&gt;")
            str = Regex.Replace(str, Chr(34), "&quot;")
            str = Regex.Replace(str, "'", "&apos;")
        End If
        Return str
    End Function
    Private Function GetStrObject(ByVal cacheMgr As CacheManager) As TStructDef
        Dim strObj As TStructDef = Nothing
        ' cachemanager and TStructDef objects throw exceptions
        Try
            strObj = cacheMgr.GetStructDef(proj, sid, user, tid, AxRole)
        Catch ex As Exception
            Response.Redirect(util.ERRPATH + ex.Message)
        End Try

        If strObj Is Nothing Then
            Response.Redirect(util.ERRPATH & "Server error. Please try again later")
        End If


        Return strObj
    End Function

    Private Function GetCacheObject() As CacheManager
        Dim cacheMgr As CacheManager = Nothing
        Dim errorLog As String = ""
        Try
            cacheMgr = New CacheManager(errorLog)
        Catch ex As Exception
            Response.Redirect(util.ERRPATH + ex.Message)
        End Try

        If cacheMgr Is Nothing Then
            Response.Redirect(util.ERRPATH & "Server error. Please try again later")
        End If

        Return cacheMgr
    End Function


    'Function to get the filter columns from the structure xml, for picklist
    Private Function GetSearchCols() As ArrayList
        Dim xmlDoc As New XmlDocument
        Dim a26Array As New ArrayList()
        Dim a63Array As New ArrayList()
        Dim fldCaption As String = String.Empty
        Dim fldName As String = String.Empty
        xmlDoc.LoadXml(tstStructXml)
        Dim tstBaseDataNodes As XmlNodeList
        tstBaseDataNodes = xmlDoc.SelectNodes("//root")
        For Each tstBaseDataNode As XmlNode In tstBaseDataNodes

            Dim tstChildDataNodes As XmlNodeList = Nothing
            tstChildDataNodes = tstBaseDataNode.ChildNodes

            For Each tstChildDataNode As XmlNode In tstChildDataNodes
                If Not tstChildDataNode.Attributes("cat") Is Nothing Then
                    If tstChildDataNode.Attributes("cat").Value.ToString() = "field" Then
                        For Each fldNode As XmlNode In tstChildDataNode
                            If fldNode.Name = "a2" Then
                                fldCaption = fldNode.InnerText
                            ElseIf fldNode.Name = "a1" Then
                                fldName = fldNode.InnerText
                            ElseIf (fldNode.Name = "a7") Then
                                For Each fldDetail As XmlNode In fldNode
                                    If fldDetail.Name = "a26" Then
                                        If Not fldDetail.Attributes("stype") Is Nothing Then
                                            If fldDetail.Attributes("stype").Value = "tstruct" Or fldDetail.Attributes("stype").Value = "sql" Then
                                                If fldName = field Then
                                                    a26Array.Add(fldCaption)
                                                End If
                                            End If
                                        End If

                                    ElseIf fldDetail.Name = "a63" Then
                                        For Each cnNode As XmlNode In fldDetail
                                            If Not cnNode.Attributes("cn") Is Nothing Then
                                                If fldName = field Then
                                                    a63Array.Add(cnNode.Attributes("cn").Value)
                                                End If
                                            End If
                                        Next
                                    End If
                                Next
                            End If
                        Next
                    End If
                End If
            Next
        Next

        If (a63Array.Count = 0) Then
            Return a26Array
        Else
            Return a63Array
        End If

    End Function


    ''' <summary>
    ''' function to handle if there are no records to display.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Message()
        searchlist.Visible = False
        GridView1.Columns.Clear()
        'dvGrid.Visible = False
        dvMsg.Visible = True
        records.Text = lblNodata.Text
        pgCap.Visible = False
        pages.Text = ""
        lvPage.Visible = False
    End Sub

    ''' <summary>
    ''' function to fill the combo from the data got from the service.
    ''' </summary>
    ''' <param name="a"></param>
    ''' <remarks></remarks>
    Private Sub BindCombo(ByVal cols As ArrayList)
        s1.Items.Add(New ListItem(Constants.EMPTYOPTION, String.Empty))
        Dim k As Integer = 0

        For k = 0 To cols.Count - 1
            If (cols(k).ToString() <> "") Then
                s1.Items.Add(cols(k).ToString())
            End If
        Next
        s1.SelectedIndex = 1

    End Sub

    ''' <summary>
    ''' function which calls the function to bind the data to the gridview control.
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="totRows"></param>
    ''' <param name="pageno"></param>
    ''' <remarks></remarks>
    Private Sub BindDataGrid(ByVal a As String, ByVal totRows As Integer, ByVal pageno As String)
        Dim ds As New DataSet()
        Dim sr As New System.IO.StringReader(a)
        ds.ReadXml(sr)

        If (ds.Tables().Count <= 1) Then
            Message()
        Else
            'dvGrid.Visible = True
            Panel1.Visible = True
            Dim value As String = ""
            Dim loadxmlDoc As New XmlDocument
            Dim WrproductNodes As XmlNodeList

            loadxmlDoc.LoadXml(a)

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

            Dim map As String = ""

            For Each getnode As XmlNode In getnodes
                If (Not getnode.Attributes("idcol") Is Nothing) Then
                    idcol = getnode.Attributes("idcol").Value
                End If
                If (Not getnode.Attributes("map") Is Nothing) Then
                    map = getnode.Attributes("map").Value
                End If
                If (Not getnode.Attributes("colmap") Is Nothing) Then
                    ViewState("colmap") = getnode.Attributes("colmap").Value
                Else
                    ViewState("colmap") = ""
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
                    searchlistval.Items.Add(mapstr.ToString())
                    mapstr = ""
                Next
            End If

            Dim wcount As Integer
            Dim i As Integer = 0
            If (value <> "--") Then
                Dim rownodes As XmlNodeList
                Dim childnodes As XmlNodeList
                rownodes = loadxmlDoc.GetElementsByTagName("row")
                wcount = rownodes.Count
                For Each rownode As XmlNode In rownodes
                    childnodes = rownode.ChildNodes
                    For Each chilnode As XmlNode In childnodes
                        i = i + 1
                        If ((idcol = "yes")) Then
                            If (i = 2) Then
                                name = name & "¿" & chilnode.InnerText.ToString()
                            ElseIf i = 1 Then
                                If name = String.Empty Then
                                    name = chilnode.InnerText.ToString()
                                Else
                                    name &= "~" & chilnode.InnerText.ToString()
                                End If
                            End If
                        Else
                            If (i = 1) Then
                                If name = String.Empty Then
                                    name = chilnode.InnerText.ToString()
                                Else
                                    name &= "~" & chilnode.InnerText.ToString()
                                End If
                            End If
                        End If

                        If calledFrom = "workflow" And (i = 2) Then
                            val = val & "~" & chilnode.InnerText.ToString()
                        End If
                    Next
                    i = 0
                Next

                If calledFrom = "workflow" Then
                    GridView1.AllowPaging = True
                    GridView1.PageSize = 10
                    totRows = wcount
                    If pageno = "1" Then
                        Session("pl_noofpages") = totRows
                    Else
                        totRows = Session("pl_noofpages")
                    End If


                    Dim empValues = name.Split("~")
                    For k As Integer = 0 To empValues.Length - 1
                        lstValues.Items.Add(empValues(k).ToString())
                    Next
                End If

                Dim wfname = name.Split("~")
                For m As Integer = 0 To wfname.Length - 1
                    searchlist.Visible = True
                    searchlist.Items.Add(wfname(m).ToString())
                Next

            End If

            SetGv(a, totRows, pageno)
        End If

    End Sub

    ''' <summary>
    ''' function to call the webservice to fill search data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        If calledFrom = "workflow" Then
            callDelegation("1")
        Else
            CallWebservice("1")
        End If

    End Sub

    ''' <summary>
    '''  function to call the function whether to bind the data to gridview or show message.
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="totRows"></param>
    ''' <param name="pageno"></param>
    ''' <remarks></remarks>
    Private Sub SetGv(ByVal a As String, ByVal totRows As Integer, ByVal pageno As String)
        Dim xmlDoc1 As New XmlDocument
        xmlDoc1.LoadXml(a)

        Dim cNode As XmlNode
        cNode = xmlDoc1.SelectSingleNode("//response")

        Dim rowNodes As XmlNodeList
        rowNodes = cNode.ChildNodes
        Dim chNode As XmlNode




        Dim i As Integer = 0
        For Each chNode In rowNodes
            For Each chhNode In chNode
                If chhNode.Attributes.Count > 0 Then
                    If (i = 0 And (chhNode.Attributes("w").Value = "0")) Then
                        arrHiddenCols.Add(chhNode.Name)
                    End If
                    chhNode.Attributes.RemoveAll()
                End If
            Next
            i = i + 1
        Next

        Dim sw As New StringWriter()
        Dim xw As New XmlTextWriter(sw)
        cNode.WriteTo(xw)
        Dim nXml As String = sw.ToString()
        nXml = nXml.Replace("<row>", "<row><SNO></SNO>") '.Replace(" w=""0""", "").Replace(" w=""""", "")
        If nXml = "<response />" Then
            Message()
        Else
            Panel1.Visible = True
            BindDataGrid1(nXml, totRows, pageno)
        End If

    End Sub

    ''' <summary>
    ''' Actuall function which binds the data to the gridview control.
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="totRows"></param>
    ''' <param name="pageno"></param>
    ''' <remarks></remarks>
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

            dc1.DataType = GetType(String)
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
                bfield.HeaderText = dc.ColumnName
                '' add newly created columns to gridview
                GridView1.Columns.Add(bfield)


                If calledFrom <> "workflow" Then
                    ' code for hiding the columns which are not in the dropdown list
                    Dim items As String
                    Dim present As Boolean = False
                    For Each item As ListItem In s1.Items
                        items = item.Value.ToString().ToLower()

                        If (i > 0) Then

                            If ((idcol = "yes") And i = 1) Then
                                Continue For
                            End If

                            If ViewState("colmap") = "" Then
                                'If items.ToLower() = dc.ColumnName.ToLower() Then
                                present = True
                                Exit For
                                'End If
                            Else
                                'If ViewState("colmap").ToString().ToLower() = dc.ColumnName.ToLower() Then
                                present = True
                                Exit For
                                'End If
                            End If

                        Else
                            present = True
                            Exit For
                        End If
                    Next


                    If (arrHiddenCols.IndexOf(GridView1.Columns(i).HeaderText) <> -1) Then
                        present = False
                    End If




                    If (Not present) Then

                        GridView1.Columns(i).Visible = False
                    End If
                    i = i + 1
                End If
            Next
        End If

        GridView1.DataSource = ds1
        ' to change the header Name and set the column width
        Dim idx As Integer
        For idx = 0 To headName.Count - 1
            ' For change the Column Heading from fld name to Caption
            GridView1.Columns(idx).HeaderText = headName(idx)
        Next

        If GridView1.Columns(0).HeaderText = "SNO" Then
            GridView1.Columns(0).HeaderText = ""
            GridView1.Columns(0).ItemStyle.Width = 20
        End If


        GridView1.DataBind()
        Dim pg As Double = CInt(totRows) / CInt(GridView1.PageSize)
        Dim pg1 As Integer = Math.Floor(pg)
        If (totRows Mod GridView1.PageSize) > 0 Then
            pg1 += 1
        End If


        If totRows > 0 Then
            records.Text = "Total no. of records: " & totRows
            pages.Text = " of " & pg1
            If calledFrom = "workflow" Then
                lvPage.Visible = False
                pages.Visible = False
                pgCap.Visible = False
            Else
                pgCap.Visible = True
                lvPage.Visible = True
                pages.Visible = True
            End If

        Else
            records.Text = lblNodata.Text
            pages.Text = ""
            pgCap.Visible = False
            lvPage.Visible = False
        End If

        Dim pgno As Integer
        If pageno = "1" Then
            lvPage.Items.Clear()
            For pgno = 1 To pg1
                lvPage.Items.Add(pgno.ToString)
            Next
        End If

        If (lvPage.Items.Count > 0) Then
            If calledFrom = "workflow" Then
                lvPage.Visible = False
            Else
                lvPage.Visible = True
            End If

        End If
        ScriptManager.RegisterStartupScript(Me, Page.GetType(), "searchInTstruct", "if(jQuery('#dvSrchGrid .customSetupTableMN').length)bindUpdownEvents('dvSrchGrid','single');", True)
    End Sub

    ''' <summary>
    ''' rowdatabound function of the gridview where column values can be customized.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
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
                If calledFrom = "workflow" Then
                    e.Row.Cells(0).Text = "<input style=""width:10px;"" type=radio value=" & catName & " onclick=LoadWorkflow(this.value);>"
                Else
                    e.Row.Cells(0).Text = "<input style=""width:10px;"" type=radio value=" & catName & " onclick=loadParent(this.value);>"
                End If

                Dim n As Integer
                For n = 0 To e.Row.Cells.Count - 1
                    If e.Row.Cells(n).Text = "*" Then
                        e.Row.Cells(n).Text = ""
                    End If
                Next
            End If
        End If
        'for NOWRAP in IE
        Dim m As Integer
        For m = 0 To e.Row.Cells.Count - 1
            If e.Row.Cells(m).Text.Length > 0 Then
                e.Row.Cells(m).Text = "<nobr>" + e.Row.Cells(m).Text + "</nobr>"
            End If
        Next

    End Sub

    ''' <summary>
    ''' Gridview controls pagination function.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging

        Dim dtOrders As New System.Data.DataTable()
        dtOrders = Session("order")
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataSource = dtOrders.DefaultView
        pgno.Text = GridView1.PageIndex
        GridView1.DataBind()

    End Sub

    ''' <summary>
    ''' calls the webservice on change of the dropdown control.
    ''' i.e. handling pagination of the gridview onchange of the dropdown control.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lvPage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvPage.SelectedIndexChanged
        Dim pgNo As String = lvPage.SelectedValue
        If (calledFrom = "workflow") Then
            callDelegation(pgNo)
        Else
            CallWebservice(pgNo)
        End If

    End Sub
    Public Sub callDelegation(ByVal pgno As String)

        Dim totalRows As Integer
        Dim tempSearch As String = ""
        tempSearch = txtSearch.Text.Trim()
        If (tempSearch <> "") Then
            tempSearch = "%" & tempSearch
        Else
            ' Messagebox("Please enter the search criteria.")
        End If
        Dim strQry As String = String.Empty

        If s1.SelectedItem.Text = "User Name" Then
            strQry = "lower(username) like lower('" + tempSearch + "%')"
        Else
            strQry = "lower(employee_id) like lower('" + tempSearch + "%')"
        End If
        Dim selectedUser As String = ""

        'passing Session("selectedUser") which user should not come in the sql
        If Session("selectedUser") IsNot Nothing Then
            selectedUser = "and Username &lt;&gt; '" & Session("selectedUser") & "'"
        End If

        iXml = iXml & "<sqlresultset axpapp=" & Chr(34) & Session("project") & Chr(34) & " sessionid=" & Chr(34) & Session("nsessionid") & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " user=" & Chr(34) & Session("user") & Chr(34) & " trace=" & Chr(34) & Session("trace") & Chr(34) & ">"
        iXml = iXml & "<sql>Select Username from axusers where " & strQry & " and (actflag='Y' or actflag='T')" & selectedUser & "</sql>"
        iXml = iXml & Session("axApps").ToString() & Application("axProps").ToString() & Session("axGlobalVars").ToString() & Session("axUserVars").ToString() & "</sqlresultset>"

        Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
        ires = objWebServiceExt.CallGetChoiceWS("workflow", iXml)

        If ires = "" Then
            Response.Redirect("./err.aspx")
        ElseIf ires.Contains(Constants.SESSIONEXPMSG) Then
            Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG)
            Return
        Else
            ires = _xmlString & ires

            Dim xmlDoc1 As New XmlDocument
            xmlDoc1.LoadXml(ires)

            Dim sw As New StringWriter()
            Dim xw As New XmlTextWriter(sw)
            xmlDoc1.WriteTo(xw)

            Dim ires2 As String = ""
            ires2 = sw.ToString()


            searchlist.Items.Clear()
            searchlistval.Items.Clear()
            BindDataGrid(ires2, totalRows, pgno)
        End If


    End Sub

    ''' <summary>
    ''' function to call the webservice to get the search data.
    ''' </summary>
    ''' <param name="pgno"></param>
    ''' <remarks></remarks>
    Public Sub CallWebservice(ByVal pgno As String)

        ' fake three second delay so that we can see the Wait cursor
        'System.Threading.Thread.Sleep(3000)
        If (tstStructXml = "") Then
            tstStructXml = ViewState("structXml")
        End If

        Dim tempSearch As String = ""
        tempSearch = txtSearch.Text.Trim()
        If (tempSearch <> "") Then
            tempSearch = "%" & tempSearch
        Else
            MessageBox(4000)
        End If

        Dim sqlsearch As String = ""
        If (s1.SelectedItem.Text = "") Then
            sqlsearch = ""
        Else
            sqlsearch = s1.SelectedItem.Text
        End If
        Dim fileName As String = "GetFieldChoices" + tid
        Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
        Dim tstData As TStructData
        tstData = DirectCast(Session(ViewState("key").ToString()), TStructData)

        Dim fldArray As New ArrayList()
        Dim fldDbRowNo As New ArrayList()
        Dim fldValueArray As New ArrayList()
        Dim fldDeletedArray As New ArrayList()
        Dim strFieldValueXml As String = ""
        tstData.GetPicklistInputXml(fldArray, fldDbRowNo, fldValueArray, fldDeletedArray, field, frameno, parInfo, subInfo, activeRow)
        Dim errorLog As String = logobj.CreateLog("GetFieldChoices.", Session("nsessionid"), fileName, "new")

        tempSearch = CheckSpecialChars(tempSearch)
        iXml = iXml & "<sqlresultset axpapp=" & Chr(34) & Session("project") & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " value=" & Chr(34) & tempSearch & Chr(34) & " sessionid= " & Chr(34) & Session("nsessionid") & Chr(34) & " field=" & Chr(34) & ViewState("field") & Chr(34) & " sqlfield=" & Chr(34) & sqlsearch & Chr(34) & " activerow=" & Chr(34) & ViewState("activeRow") & Chr(34) & " frameno=" & Chr(34) & ViewState("frameno") & Chr(34) & " pdc=" & Chr(34) & pdc & Chr(34) & " prow=" & Chr(34) & prow & Chr(34) & " trace=" & Chr(34) & errorLog & Chr(34) & " transid=" & Chr(34) & ViewState("tid") & Chr(34) & "  pageno=""" & pgno & """ pagesize=""" & GridView1.PageSize & """>"

        strFieldValueXml = tstData.fieldValueXml
        strFieldValueXml = strFieldValueXml.Replace("'", Chr(34))


        iXml &= strFieldValueXml & tstData.memVarsData
        iXml &= Session("axApps").ToString() & Application("axProps").ToString() & Session("axGlobalVars").ToString() & Session("axUserVars").ToString()
        iXml = iXml & "</sqlresultset>"

        'If tstData.tstStrObj.IsFldInFDDynamic(ViewState("field").ToString()) Then
        '    Dim fObj As FDR = DirectCast(Session("FDR"), FDR)
        '    ires = fObj.GetFastAdvPicklist(ViewState("tid"), iXml, ViewState("field").ToString(), sqlsearch, tempSearch, pgno, GridView1.PageSize, tstData.tstStrObj.IsFldInFDActive(ViewState("field").ToString()))
        'Else
        'Call service
        ires = objWebServiceExt.CallGetFieldChoicesWS(tid, iXml, tstStructXml)
        'End If

        Dim errMsg As String = String.Empty
        errMsg = util.ParseXmlErrorNode(ires)

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
                Response.Redirect(util.ERRPATH + errMsg)
            End If
        Else
            ires = _xmlString & ires

            Dim xmlDoc1 As New XmlDocument
            xmlDoc1.LoadXml(ires)

            Dim compNodes As XmlNodeList
            Dim cNode As XmlNode
            compNodes = xmlDoc1.SelectNodes("//sqlresultset/response")
            Dim totalRows As Integer
            For Each cNode In compNodes
                'Getting total if pageno=1 else pick it from session
                'if dynamically records are added, chance of mismatch, rare chance
                If pgno = "1" Then
                    Dim tnode As XmlNode = cNode.Attributes("totalrows")
                    If tnode Is Nothing Then
                        totalRows = 0
                    Else
                        totalRows = CInt(tnode.Value)
                        cNode.Attributes.RemoveNamedItem("totalrows")
                    End If
                    Session("pl_noofpages") = totalRows
                    Exit For
                Else
                    Dim tnode As XmlNode = cNode.Attributes("totalrows")
                    If tnode Is Nothing Then
                    Else
                        cNode.Attributes.RemoveNamedItem("totalrows")
                    End If
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

    End Sub
    Protected Sub MessageBox(ByVal Msg As String)
        Page.ClientScript.RegisterStartupScript([GetType](), "myrest", "<script>showAlertDialog('info'," + Msg + ",'client');</script>")
    End Sub
    Private Sub SetLangStyles()
        language = Session("language").ToString()
        If Session("language").ToString() = "ARABIC" Then
            direction = "rtl"
            searchoverlay.Attributes("class") = "arabicoverlay"
            clssdir = "right"
            'dvsearchdir.Attributes("style") = "height: 42px;margin-right:25px;float:right;"
            'dvSearchFor.Attributes("style") = "float:right;"
        Else
            searchoverlay.Attributes("class") = "overlay"
            'dvsearchdir.Attributes("style") = "height: 42px;margin-left:25px;float:left;"
            'dvSearchFor.Attributes("style") = "float:left;"
        End If
    End Sub
End Class
