Imports System.Xml
Imports System.Data
Imports System.IO
Imports System.String
Imports System.Web
Imports Newtonsoft.Json
Imports System.Xml.Linq
Partial Public Class pvtpages_useraccess
    Inherits System.Web.UI.Page

    Dim _xmlString As String = "<?xml version=""1.0"" encoding=""utf-8"" ?>"
    Dim logobj As LogFile.Log = New LogFile.Log()
    Public cac_tcexpr As String
    Dim sid As String
    Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
    Dim util As Util.Util = New Util.Util()
    Dim isCloudApp As Boolean = False
    Public direction As String = "ltr"
    Public langType As String = "en"
    Dim recordUpdated As Boolean = False

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
                Dim filcustom As FileInfo
                filcustom = New FileInfo(HttpContext.Current.Server.MapPath("~/Js/lang/content-" + langType + ".js"))
                If Not (filcustom.Exists) Then
                    direction = "ltr"
                    langType = "en"
                End If
                recordUpdated = False
                If Session("language").ToString() = "ARABIC" Then
                    direction = "rtl"
                End If
                If util.IsValidQueryString(Request.RawUrl) = False Then
                    Response.Redirect(util.ERRPATH + Constants.INVALIDURL)
                End If
                sid = Session("nsessionid")

                Dim transid As String
                transid = Request.QueryString("transid")
                cac_tcexpr = transid & "tcexpr"

                Dim selrole As String
                selrole = Request.QueryString("role")

                Dim errlog As String = logobj.CreateLog("Calling GetUserAccessTstDetails web service", sid, "GetUserAccessDetails", "new")
                Dim detiXml As String
                detiXml = "<root axpapp=""" & Session("project") & """ sessionid= " & Chr(34) & Session("nsessionid") & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " trace=""" & errlog & """ transid=""" & transid & """ user=""" & Session("user") & """ role=""" & selrole & """>"
                detiXml &= Session("axApps").ToString() + Application("axProps").ToString() & Session("axGlobalVars").ToString() & Session("axUserVars").ToString() & "</root>"
                Dim res As String = String.Empty
                'Call service
                res = objWebServiceExt.CallGetUserAccessTstDetailsWS(transid, detiXml)

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

                    Dim xDoc As XDocument = XDocument.Parse(res)
                    Dim jsonString As String = JsonConvert.SerializeXNode(xDoc, Newtonsoft.Json.Formatting.None, True)
                    hdnUsrAccessRes.Value = jsonString

                    Dim xmldoc As New XmlDocument
                    xmldoc.LoadXml(res)

                    Dim roots As XmlNodeList
                    Dim prnode As XmlNode
                    Dim baseDataNodes As XmlNodeList
                    roots = xmldoc.SelectNodes("//root/dcs")

                    Dim dcRes As String = ""
                    Dim rs As Integer = 1
                    Dim nrs As String = "001"
                    For Each prnode In roots
                        baseDataNodes = prnode.ChildNodes
                        For Each baseDataNode As XmlNode In baseDataNodes
                            Dim dcname As String = baseDataNode.Name
                            Dim dcview As String = ""
                            Dim dcenable As String = ""
                            Dim dccap As String = ""

                            Dim nodedccap As XmlNode = baseDataNode.Attributes("cap")
                            If (nodedccap Is Nothing) Then
                                dccap = baseDataNode.Name
                            Else
                                dccap = nodedccap.Value
                            End If


                            Dim nodedcview As XmlNode = baseDataNode.Attributes("view")
                            If (nodedcview Is Nothing) Then
                                dcview = ""
                            Else
                                dcview = nodedcview.Value
                            End If

                            Dim nodedcenable As XmlNode = baseDataNode.Attributes("enable")
                            If (nodedcenable Is Nothing) Then
                                dcenable = ""
                            Else
                                dcenable = nodedcenable.Value
                            End If

                            Dim dcview_cb As String
                            dcview_cb = "<input class=""w-15px h-15px"" id=""dcview" & nrs & """ data-dc-view=" & nrs & """ type=""checkbox"" value=""" & dcname & """"
                            If dcview = "true" Then
                                dcview_cb = dcview_cb & "  checked=""true""  />"
                            Else
                                dcview_cb = dcview_cb & "  />"
                            End If

                            ' if view is selected only then enable is meaningful. 
                            Dim dcenable_cb As String
                            dcenable_cb = "<input class=""w-15px h-15px"" id=""dcenable" & nrs & """ data-dc-enable=" & nrs & """ type=""checkbox"" value=""" & dcname & """"
                            If dcenable = "true" And dcview = "true" Then
                                dcenable_cb = dcenable_cb & "  checked=""true""  />"
                            Else
                                dcenable_cb = dcenable_cb & "  />"
                            End If


                            rs = rs + 1
                            If rs.ToString.Length = 1 Then
                                nrs = "00" & rs
                            ElseIf rs.ToString.Length = 2 Then
                                nrs = "0" & rs
                            ElseIf rs.ToString.Length = 3 Then
                                nrs = rs
                            End If
                            dcRes = dcRes & "<tr><td>" & dccap & "</td><td class=""text-center"">" & dcview_cb & "</td><td class=""text-center"">" & dcenable_cb & "<input type=hidden id=""dccap_" & dcname & """ value=""" & dccap & """></td></tr>"
                        Next
                    Next

                    dcRes = "<table class='table table-sm table-row-bordered w-100 gridData'><thead><tr class=""text-center""><th id='thDCName'>DC Name</th><th class='view'>View</th><th class='enable'>Enable</th></tr></thead>" & dcRes & "</table>"
                    dc_div.InnerHtml = dcRes

                    ' For Fields
                    Dim froots As XmlNodeList
                    Dim prnodeflds As XmlNode
                    Dim baseDataNodesFlds As XmlNodeList
                    froots = xmldoc.SelectNodes("//root/fields")

                    Dim fldsRes As String = ""
                    Dim fldrs As Integer = 1
                    Dim nfldrs As String = "001"
                    For Each prnodeflds In froots
                        baseDataNodesFlds = prnodeflds.ChildNodes
                        For Each baseDataNodefld As XmlNode In baseDataNodesFlds

                            Dim fldname As String = baseDataNodefld.Name
                            Dim fldview As String = ""
                            Dim fldenable As String = ""
                            Dim fldcap As String = ""

                            Dim nodefldcap As XmlNode = baseDataNodefld.Attributes("cap")
                            If (nodefldcap Is Nothing) Then
                                fldcap = baseDataNodefld.Name
                            Else
                                fldcap = If(nodefldcap.Value IsNot String.Empty, nodefldcap.Value, DirectCast(nodefldcap, System.[Xml].XmlAttribute).OwnerElement.Name)
                            End If


                            Dim nodefldview As XmlNode = baseDataNodefld.Attributes("view")
                            If (nodefldview Is Nothing) Then
                                fldview = ""
                            Else
                                fldview = nodefldview.Value
                            End If

                            Dim nodefldenable As XmlNode = baseDataNodefld.Attributes("enable")
                            If (nodefldenable Is Nothing) Then
                                fldenable = ""
                            Else
                                fldenable = nodefldenable.Value
                            End If

                            Dim fldview_cb As String

                            fldview_cb = "<input class=""w-15px h-15px"" id=""fldview" & nfldrs & """ data-fld-view=" & nfldrs & """  type=""checkbox"" value=""" & fldname & """"
                            If fldview = "true" Then
                                fldview_cb = fldview_cb & "  checked=""true""  />"
                            Else
                                fldview_cb = fldview_cb & "  />"
                            End If


                            Dim fldenable_cb As String
                            fldenable_cb = "<input class=""w-15px h-15px"" id=""fldenable" & nfldrs & """ data-fld-enable=" & nfldrs & """  type=""checkbox"" value=""" & fldname & """"
                            If fldenable = "true" Then
                                fldenable_cb = fldenable_cb & "  checked=""true""  />"
                            Else
                                fldenable_cb = fldenable_cb & "  />"
                            End If

                            fldrs = fldrs + 1
                            If fldrs.ToString.Length = 1 Then
                                nfldrs = "00" & fldrs
                            ElseIf fldrs.ToString.Length = 2 Then
                                nfldrs = "0" & fldrs
                            ElseIf fldrs.ToString.Length = 3 Then
                                nfldrs = fldrs
                            End If
                            fldsRes = fldsRes & "<tr><td>" & fldcap & "</td><td class=""text-center"">" & fldview_cb & "</td><td class=""text-center"">" & fldenable_cb & "<input type=hidden id=""fdcap_" & fldname & """ value=""" & fldcap & """></td></tr>"
                        Next
                    Next

                    fldsRes = "<table class='table table-sm table-row-bordered w-100 gridData'><thead><tr class=""text-center""><th id='thFieldName'>Field Name</th><th class='view'>View</th><th class='enable'>Enable</th></tr></thead>" & fldsRes & "</table>"
                    fields_div.InnerHtml = fldsRes

                    'Buttons
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
                            butRes = butRes & "<tr><td>" & butname & "</td><td class=""text-center"">" & butview_cb & "</td><td class=""text-center"">" & butenable_cb & "</td></tr>"
                        Next
                    Next

                    butRes = "<table class='table table-sm table-row-bordered w-100 gridData'><thead><tr class=""text-center""><th id='thButtonName'>Button Name</th><th class='view'>View</th><th class='enable'>Enable</th></tr></thead>" & butRes & "</table>"
                    but_div.InnerHtml = butRes

                    'List View Buttons
                    Dim lvbutroots As XmlNodeList
                    Dim prnodelvbuts As XmlNode
                    Dim baseDataNodeslvButs As XmlNodeList
                    lvbutroots = xmldoc.SelectNodes("//root/listviewbuttons")

                    Dim lvbutRes As String = ""
                    Dim lvbutrs As Integer = 1
                    Dim nlvbutrs As String = "001"
                    For Each prnodelvbuts In lvbutroots
                        baseDataNodeslvButs = prnodelvbuts.ChildNodes
                        For Each baseDataNodelvbut As XmlNode In baseDataNodeslvButs

                            Dim lvbutname As String = ""
                            Dim lvbutview As String = ""
                            Dim lvbutenable As String = ""
                            Dim nodelvbutname As XmlNode = baseDataNodelvbut.Attributes("cap")
                            If (nodelvbutname Is Nothing) Then
                                lvbutname = baseDataNodelvbut.Name
                            Else
                                lvbutname = nodelvbutname.Value
                            End If

                            Dim nodelvbutview As XmlNode = baseDataNodelvbut.Attributes("view")
                            If (nodelvbutview Is Nothing) Then
                                lvbutview = ""
                            Else
                                lvbutview = nodelvbutview.Value
                            End If

                            Dim nodelvbutenable As XmlNode = baseDataNodelvbut.Attributes("enable")
                            If (nodelvbutenable Is Nothing) Then
                                lvbutenable = ""
                            Else
                                lvbutenable = nodelvbutenable.Value
                            End If

                            Dim lvbutview_cb As String

                            lvbutview_cb = "<input class=""w-15px h-15px"" id=""lvbview" & nlvbutrs & """ data-lbtn-view=" & nlvbutrs & """  type=""checkbox"" value=""" & baseDataNodelvbut.Name & """"
                            If lvbutview = "true" Then
                                lvbutview_cb = lvbutview_cb & "  checked=""true""  />"
                            Else
                                lvbutview_cb = lvbutview_cb & "   />"
                            End If


                            Dim lvbutenable_cb As String
                            lvbutenable_cb = "<input class=""w-15px h-15px"" id=""lvbutenable" & nlvbutrs & """ data-lbtn-enable=" & nlvbutrs & """  type=""checkbox"" value=""" & lvbutname & """"
                            If lvbutenable = "true" Then
                                lvbutenable_cb = lvbutenable_cb & "  checked=""true""  />"
                            Else
                                lvbutenable_cb = lvbutenable_cb & "   />"
                            End If

                            lvbutrs = lvbutrs + 1
                            If lvbutrs.ToString.Length = 1 Then
                                nlvbutrs = "00" & lvbutrs
                            ElseIf lvbutrs.ToString.Length = 2 Then
                                nlvbutrs = "0" & lvbutrs
                            ElseIf lvbutrs.ToString.Length = 3 Then
                                nlvbutrs = lvbutrs
                            End If
                            lvbutRes = lvbutRes & "<tr><td>" & lvbutname & "</td><td class=""text-center"">" & lvbutview_cb & "</td><td class=""text-center"">" & lvbutenable_cb & "</td></tr>"
                        Next
                    Next

                    lvbutRes = "<table class='table table-sm table-row-bordered w-100 gridData'><thead><tr class=""text-center""><th id='thLVButtonName'>Button Name</th><th class='view'>View</th><th class='enable'>Enable</th></tr></thead>" & lvbutRes & "</table>"
                    lv_div.InnerHtml = lvbutRes


                    ''For tc Flds
                    'Dim tcfldroots As XmlNodeList
                    'Dim prnodetcflds As XmlNode
                    'Dim baseDataNodestcflds As XmlNodeList
                    'tcfldroots = xmldoc.SelectNodes("//root/tcfld")
                    'tc_fld_cb.Items.Add("")
                    'tc_values_cb.Items.Add("")
                    'tc_betvalues_cb.Items.Add("")
                    'For Each prnodetcflds In tcfldroots
                    '    baseDataNodestcflds = prnodetcflds.ChildNodes
                    '    For Each baseDataNodetcfld As XmlNode In baseDataNodestcflds
                    '        tc_fld_cb.Items.Add(baseDataNodetcfld.Name)
                    '        tc_values_cb.Items.Add(":" & baseDataNodetcfld.Name)
                    '        tc_betvalues_cb.Items.Add(":" & baseDataNodetcfld.Name)
                    '    Next
                    'Next

                    'For Tc Expression
                    'For tc Flds
                    Dim tcexprroots As XmlNodeList
                    Dim prnodetcexpr As XmlNode
                    Dim baseDataNodestcexpr As XmlNodeList
                    tcexprroots = xmldoc.SelectNodes("//root")
                    Dim ntcexp As String = ""
                    Dim ndcs As String = ""
                    Dim nflds As String = ""
                    Dim nbuts As String = ""
                    Dim nlvbuts As String = ""
                    For Each prnodetcexpr In tcexprroots
                        baseDataNodestcexpr = prnodetcexpr.ChildNodes
                        For Each baseDataNodetcexpr As XmlNode In baseDataNodestcexpr
                            If baseDataNodetcexpr.Name = "tcexpr" Then
                                ntcexp = baseDataNodetcexpr.InnerXml
                            ElseIf baseDataNodetcexpr.Name = "dcs" Then
                                ndcs = baseDataNodetcexpr.InnerXml
                            ElseIf baseDataNodetcexpr.Name = "fields" Then
                                nflds = baseDataNodetcexpr.InnerXml
                            ElseIf baseDataNodetcexpr.Name = "buttons" Then
                                nbuts = baseDataNodetcexpr.InnerXml
                            ElseIf baseDataNodetcexpr.Name = "listviewbuttons" Then
                                nlvbuts = baseDataNodetcexpr.InnerXml
                            End If
                        Next
                    Next

                    ' Save as xml node in to _xml hiddenfld so that status store properly
                    dc_xml.Value = ndcs
                    fld_xml.Value = nflds
                    but_xml.Value = nbuts
                    lv_xml.Value = nlvbuts

                    ntcexp = "<tcexpr>" & ntcexp & "</tcexpr>"

                    Cache.Insert(cac_tcexpr, ntcexp, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                End If ' end of res error
            End If
        End If

        If (ConfigurationManager.AppSettings("isCloudApp") <> Nothing) Then
            isCloudApp = Convert.ToBoolean(ConfigurationManager.AppSettings("isCloudApp").ToString())
        End If
        ClientScript.RegisterStartupScript([GetType](), "name", "<script language=""javascript"">var isCloudApp = '" + isCloudApp.ToString() + "'</script>")
    End Sub

    Private Sub BindDataGrid()
        tc_expr_gv.Columns.Clear()
        Dim ds As New DataSet()
        Dim cac_str As String = Request.QueryString("transid") & "tcexpr"
        Dim a As String = Cache.Get(cac_str)
        Dim sr As New StringReader(a)
        ds.ReadXml(sr)

        Dim dc As DataColumn
        If ds.Tables.Count > 0 Then
            For Each dc In ds.Tables(0).Columns


                Dim bfield As New BoundField
                ''initialiae the data field value
                bfield.DataField = dc.ColumnName
                ''initialise the header text value
                bfield.HeaderText = dc.ColumnName
                '' add newly created columns to gridview
                tc_expr_gv.Columns.Add(bfield)
            Next
        End If
        tc_expr_gv.DataSource = ds
        tc_expr_gv.Columns(0).HeaderText = "Expression"
        tc_expr_gv.Columns(1).HeaderText = "View"
        tc_expr_gv.Columns(2).HeaderText = "Edit"
        tc_expr_gv.Columns(3).HeaderText = "Delete"
        tc_expr_gv.Columns(4).Visible = False
        tc_expr_gv.Columns(5).Visible = False
        tc_expr_gv.Columns(6).Visible = False
        tc_expr_gv.Columns(7).Visible = False
        tc_expr_gv.DataBind()
        Dim str As String
        If (recordUpdated) Then
            str = "showTransactionExp();showAlertDialog('success','Transaction saved successfully')"
        Else
            str = ""
        End If

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", str, True) 'to format Grid action buttons(Edit, Delete) text to icons
    End Sub

    Protected Sub tc_expr_gv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles tc_expr_gv.RowDataBound
        Dim drv As System.Data.DataRowView
        drv = CType(e.Row.DataItem, System.Data.DataRowView)
        If e.Row.RowType = DataControlRowType.DataRow Then
            If drv IsNot Nothing Then

                Dim expr As String = drv(0).ToString()
                If expr = "" Then
                    e.Row.Visible = False
                End If

                Dim chStatusview As String = drv(1).ToString()
                Dim checkedv As String = ""
                If chStatusview.ToLower() = "true" Then
                    checkedv = "checked "
                Else
                    checkedv = " "
                End If
                e.Row.Cells(2).Text = "<input class=""w-15px h-15px"" type=checkbox " & checkedv & " disabled>"
                e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Center

                Dim chStatusedit As String = drv(2).ToString()
                Dim checkede As String = ""
                If chStatusedit.ToLower() = "true" Then
                    checkede = "checked "
                Else
                    checkede = " "
                End If
                e.Row.Cells(3).Text = "<input class=""w-15px h-15px"" type=checkbox " & checkede & " disabled>"
                e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Center

                Dim chStatusdelete As String = drv(3).ToString()
                Dim checkedd As String = ""
                If chStatusdelete.ToLower() = "true" Then
                    checkedd = "checked "
                Else
                    checkedd = " "
                End If
                e.Row.Cells(4).Text = "<input class=""w-15px h-15px"" type=checkbox " & checkedd & " disabled>"
                e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Center

            End If
        End If

    End Sub
    Protected Sub tc_ok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tc_ok.Click
        recordUpdated = True
        Dim exp As String
        If tc_betvalues_cb.SelectedValue = "" Then
            exp = "if " & tc_fld_cb.SelectedValue & " " & tc_opr_cb.SelectedValue & " " & tc_values_cb.SelectedValue
        Else
            exp = "if " & tc_fld_cb.SelectedValue & " " & tc_opr_cb.SelectedValue & " " & tc_values_cb.SelectedValue & "," & tc_betvalues_cb.SelectedValue
        End If


        Dim exprstr As String
        exprstr = "<row>"
        exprstr = exprstr & "<expr>" & exp & "</expr>"
        exprstr = exprstr & "<view>" & tc_view_cb.Checked & "</view><edit>" & tc_edit_cb.Checked & "</edit><delete>" & tc_delete_cb.Checked & "</delete>"
        exprstr = exprstr & "<selfld>" & tc_fld_cb.SelectedValue & "</selfld><opr>" & tc_opr_cb.SelectedValue & "</opr><val>" & tc_values_cb.SelectedValue & "</val><bval>" & tc_betvalues_cb.SelectedValue & "</bval>"
        exprstr = exprstr & "</row>"


        Dim cac_str As String = Request.QueryString("transid") & "tcexpr"
        Dim tcexpr_xml As String = Cache.Get(cac_str)
        Dim tcexplen As Integer = tcexpr_xml.Length

        Dim newtcexpr As String
        If editmode.Value = "" Then
            newtcexpr = Mid(tcexpr_xml, 1, tcexplen - 9) & exprstr & "</tcexpr>"
            Cache.Insert(cac_str, newtcexpr, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            BindDataGrid()
        Else
            editgv_row()
        End If
        tc_expr_gv.EditIndex = -1
        tc_expr_gv.DataBind()
        recordUpdated = True

    End Sub



    Private Sub editgv_row()

        Dim exp As String
        If tc_betvalues_cb.SelectedValue = "" Then
            exp = "if " & tc_fld_cb.SelectedValue & " " & tc_opr_cb.SelectedValue & " " & tc_values_cb.SelectedValue
        Else
            exp = "if " & tc_fld_cb.SelectedValue & " " & tc_opr_cb.SelectedValue & " " & tc_values_cb.SelectedValue & "," & tc_betvalues_cb.SelectedValue
        End If


        Dim exprstr As String

        exprstr = "<expr>" & exp & "</expr>"
        exprstr = exprstr & "<view>" & tc_view_cb.Checked & "</view><edit>" & tc_edit_cb.Checked & "</edit><delete>" & tc_delete_cb.Checked & "</delete>"
        exprstr = exprstr & "<selfld>" & tc_fld_cb.SelectedValue & "</selfld><opr>" & tc_opr_cb.SelectedValue & "</opr><val>" & tc_values_cb.SelectedValue & "</val><bval>" & tc_betvalues_cb.SelectedValue & "</bval>"



        Dim cac_str As String = Request.QueryString("transid") & "tcexpr"
        Dim tcexpr_xml As String = Cache.Get(cac_str)

        Dim editidx As Integer
        editidx = CInt(editmode.Value)

        Dim xmlDoc As New XmlDocument
        xmlDoc.LoadXml(tcexpr_xml)

        Dim rownodes As XmlNodeList
        Dim rxmlNode As XmlNode
        rownodes = xmlDoc.SelectNodes("//row")


        Dim nxmlele As XmlElement
        nxmlele = xmlDoc.CreateElement("row")
        nxmlele.InnerXml = exprstr

        Dim st As String = ""
        Dim rowno As Integer = 0
        For Each rxmlNode In rownodes
            If editidx = rowno Then
                rxmlNode.InnerXml = exprstr
            End If
            rowno = rowno + 1
        Next

        Dim sw As New StringWriter()
        Dim xw As New XmlTextWriter(sw)
        xmlDoc.WriteTo(xw)


        Cache.Insert(cac_str, sw.ToString(), Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        BindDataGrid()

    End Sub

    Protected Sub tc_expr_gv_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles tc_expr_gv.RowDeleting

        Dim cac_str As String = Request.QueryString("transid") & "tcexpr"
        Dim tcexpr_xml As String = Cache.Get(cac_str)

        Dim xmlDoc As New XmlDocument
        xmlDoc.LoadXml(tcexpr_xml)

        Dim rownodes As XmlNodeList
        Dim rxmlNode As XmlNode
        rownodes = xmlDoc.SelectNodes("//row")

        Dim st As String = ""
        Dim rowno As Integer = 0
        For Each rxmlNode In rownodes
            If e.RowIndex = rowno Then
                rxmlNode.ParentNode.RemoveChild(rxmlNode)
            End If
            rowno = rowno + 1
        Next

        Dim sw As New StringWriter()
        Dim xw As New XmlTextWriter(sw)
        xmlDoc.WriteTo(xw)


        Cache.Insert(cac_str, sw.ToString(), Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        recordUpdated = True
        BindDataGrid()

    End Sub

    Protected Sub tc_expr_gv_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles tc_expr_gv.RowEditing

        Dim cac_str As String = Request.QueryString("transid") & "tcexpr"
        Dim tcexpr_xml As String = Cache.Get(cac_str)

        Dim xmlDoc As New XmlDocument
        xmlDoc.LoadXml(tcexpr_xml)

        Dim rownodes As XmlNodeList
        Dim rxmlNode As XmlNode
        Dim rnodechild As XmlNodeList
        rownodes = xmlDoc.SelectNodes("//row")

        Dim st As String = ""
        Dim rowno As Integer = 0

        Dim viewsts As String = ""
        Dim editsts As String = ""
        Dim deletests As String = ""

        For Each rxmlNode In rownodes

            If e.NewEditIndex = rowno Then
                rnodechild = rxmlNode.ChildNodes
                For Each rNodetcfld As XmlNode In rnodechild
                    If rNodetcfld.Name = "selfld" Then
                        tc_fld_cb.SelectedValue = rNodetcfld.InnerText
                    ElseIf rNodetcfld.Name = "opr" Then
                        tc_opr_cb.SelectedValue = rNodetcfld.InnerText
                    ElseIf rNodetcfld.Name = "val" Then
                        tc_values_cb.SelectedValue = rNodetcfld.InnerText
                    ElseIf rNodetcfld.Name = "bval" Then
                        tc_betvalues_cb.SelectedValue = rNodetcfld.InnerText
                    ElseIf rNodetcfld.Name = "view" Then
                        viewsts = rNodetcfld.InnerText
                    ElseIf rNodetcfld.Name = "edit" Then
                        editsts = rNodetcfld.InnerText
                    ElseIf rNodetcfld.Name = "delete" Then
                        deletests = rNodetcfld.InnerText
                    End If
                Next
            End If
            rowno = rowno + 1
        Next

        editmode.Value = e.NewEditIndex

        If viewsts.ToLower = "true" Then
            tc_view_cb.Checked = True
        Else
            tc_view_cb.Checked = False
        End If

        If editsts.ToLower = "true" Then
            tc_edit_cb.Checked = True
        Else
            tc_edit_cb.Checked = False
        End If

        If deletests.ToLower = "true" Then
            tc_delete_cb.Checked = True
        Else
            tc_delete_cb.Checked = False
        End If

        If tc_opr_cb.SelectedValue = "Between" Then
            tc_betvalues_cb.Enabled = True
        Else
            tc_betvalues_cb.Enabled = False
            tc_betvalues_cb.SelectedValue = ""
        End If
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "editTransactionControl()", True) 'to format Grid action buttons(Edit, Delete) text to icons

    End Sub

    Private Sub setdcval()
        If dc_xml.Value <> "" Then
            Dim xmldoc As New XmlDocument
            Dim res As String
            res = "<root><dcs>" & dc_xml.Value & "</dcs></root>"
            res = res.Replace("&", "&amp;")
            xmldoc.LoadXml(res)

            Dim roots As XmlNodeList
            Dim prnode As XmlNode
            Dim baseDataNodes As XmlNodeList
            roots = xmldoc.SelectNodes("//root/dcs")

            Dim dcRes As String = ""
            Dim rs As Integer = 1
            Dim nrs As String = "001"
            For Each prnode In roots
                baseDataNodes = prnode.ChildNodes
                For Each baseDataNode As XmlNode In baseDataNodes

                    Dim dcname As String = baseDataNode.Name
                    Dim dcview As String = ""
                    Dim dcenable As String = ""
                    Dim dccap As String = ""

                    Dim nodedccap As XmlNode = baseDataNode.Attributes("cap")
                    If (nodedccap Is Nothing) Then
                        dccap = baseDataNode.Name
                    Else
                        dccap = nodedccap.Value
                    End If

                    Dim nodedcview As XmlNode = baseDataNode.Attributes("view")
                    If (nodedcview Is Nothing) Then
                        dcview = ""
                    Else
                        dcview = nodedcview.Value
                    End If

                    Dim nodedcenable As XmlNode = baseDataNode.Attributes("enable")
                    If (nodedcenable Is Nothing) Then
                        dcenable = ""
                    Else
                        dcenable = nodedcenable.Value
                    End If

                    Dim dcview_cb As String
                    dcview_cb = "<input id=""dcview" & nrs & """  data-dc-view=" & nrs & """ type=""checkbox"" value=""" & dcname & """"
                    If dcview = "true" Then
                        dcview_cb = dcview_cb & "  checked=""true""  />"
                    Else
                        dcview_cb = dcview_cb & "   />"
                    End If


                    Dim dcenable_cb As String
                    dcenable_cb = "<input id=""dcenable" & nrs & """ data-dc-enable=" & nrs & """ type=""checkbox"" value=""" & dcname & """"
                    If dcenable = "true" Then
                        dcenable_cb = dcenable_cb & "  checked=""true""  />"
                    Else
                        dcenable_cb = dcenable_cb & "   />"
                    End If

                    rs = rs + 1
                    If rs.ToString.Length = 1 Then
                        nrs = "00" & rs
                    ElseIf rs.ToString.Length = 2 Then
                        nrs = "0" & rs
                    ElseIf rs.ToString.Length = 3 Then
                        nrs = rs
                    End If
                    dcRes = dcRes & "<tr><td>" & dccap & "</td><td class='text-center'>" & dcview_cb & "</td><td  class='text-center'>" & dcenable_cb & "<input type=hidden id=""dccap_" & dcname & """ value=""" & dccap & """></td></tr>"
                Next
            Next

            dcRes = "<table class='table table-sm table-row-bordered w-100 gridData'><thead><tr class=""text-center""><th id='thDCName'>DC Name</th><th class='view'>View</th><th class='enable'>Enable</th></tr></thead>" & dcRes & "</table>"

            dc_div.InnerHtml = ""
            dc_div.InnerHtml = dcRes
        End If
    End Sub

    Private Sub setfldval()
        If fld_xml.Value <> "" Then
            Dim xmldoc As New XmlDocument
            Dim res As String
            res = "<root><fields>" & fld_xml.Value & "</fields></root>"
            res = res.Replace("&", "&amp;")
            xmldoc.LoadXml(res)

            Dim froots As XmlNodeList
            Dim prnodeflds As XmlNode
            Dim baseDataNodesFlds As XmlNodeList
            froots = xmldoc.SelectNodes("//root/fields")

            Dim fldsRes As String = ""
            Dim fldrs As Integer = 1
            Dim nfldrs As String = "001"
            For Each prnodeflds In froots
                baseDataNodesFlds = prnodeflds.ChildNodes
                For Each baseDataNodefld As XmlNode In baseDataNodesFlds

                    Dim fldname As String = baseDataNodefld.Name
                    Dim fldview As String = ""
                    Dim fldenable As String = ""
                    Dim fldcap As String = ""

                    Dim nodefldcap As XmlNode = baseDataNodefld.Attributes("cap")
                    If (nodefldcap Is Nothing) Then
                        fldcap = baseDataNodefld.Name
                    Else
                        fldcap = If(nodefldcap.Value IsNot String.Empty, nodefldcap.Value, DirectCast(nodefldcap, System.[Xml].XmlAttribute).OwnerElement.Name)
                    End If

                    Dim nodefldview As XmlNode = baseDataNodefld.Attributes("view")
                    If (nodefldview Is Nothing) Then
                        fldview = ""
                    Else
                        fldview = nodefldview.Value
                    End If

                    Dim nodefldenable As XmlNode = baseDataNodefld.Attributes("enable")
                    If (nodefldenable Is Nothing) Then
                        fldenable = ""
                    Else
                        fldenable = nodefldenable.Value
                    End If

                    Dim fldview_cb As String

                    fldview_cb = "<input id=""fldview" & nfldrs & """ data-fld-view=" & nfldrs & """  type=""checkbox"" value=""" & fldname & """"
                    If fldview = "true" Then
                        fldview_cb = fldview_cb & "  checked=""true""  />"
                    Else
                        fldview_cb = fldview_cb & "  />"
                    End If


                    Dim fldenable_cb As String
                    fldenable_cb = "<input id=""fldenable" & nfldrs & """ data-fld-enable=" & nfldrs & """  type=""checkbox"" value=""" & fldname & """"
                    If fldenable = "true" Then
                        fldenable_cb = fldenable_cb & "  checked=""true""  />"
                    Else
                        fldenable_cb = fldenable_cb & "  />"
                    End If

                    fldrs = fldrs + 1
                    If fldrs.ToString.Length = 1 Then
                        nfldrs = "00" & fldrs
                    ElseIf fldrs.ToString.Length = 2 Then
                        nfldrs = "0" & fldrs
                    ElseIf fldrs.ToString.Length = 3 Then
                        nfldrs = fldrs
                    End If
                    fldsRes = fldsRes & "<tr><td>" & fldcap & "</td><td class=""text-center"">" & fldview_cb & "</td><td class=""text-center"">" & fldenable_cb & "<input type=hidden id=""fdcap_" & fldname & """ value=""" & fldcap & """></td></tr>"
                Next
            Next

            fldsRes = "<table class='table table-sm table-row-bordered w-100 gridData'><thead><tr class=""text-center""><th id='thFieldName'>Field Name</th><th class='view'>View</th><th class='enable'>Enable</th></tr></thead>" & fldsRes & "</table>"
            fields_div.InnerHtml = fldsRes
        End If
    End Sub

    Private Sub setbutval()
        If but_xml.Value <> "" Then
            Dim xmldoc As New XmlDocument
            Dim res As String
            res = "<root><buttons>" & but_xml.Value & "</buttons></root>"
            res = res.Replace("&", "&amp;")
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

                    butview_cb = "<input class=""w-15px h-15px"" id=""butview" & nbutrs & """ data-fld-view=" & nbutrs & """  type=""checkbox"" value=""" & baseDataNodebut.Name & """"
                    If butview = "true" Then
                        butview_cb = butview_cb & "  checked=""true""  />"
                    Else
                        butview_cb = butview_cb & "   />"
                    End If


                    Dim butenable_cb As String
                    butenable_cb = "<input class=""w-15px h-15px"" id=""butenable" & nbutrs & """ data-fld-enable=" & nbutrs & """  type=""checkbox"" value=""" & butname & """"
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
                    butRes = butRes & "<tr><td>" & butname & "</td><td class=""text-center"">" & butview_cb & "</td><td class=""text-center"">" & butenable_cb & "</td></tr>"
                Next
            Next

            butRes = "<table class='table table-sm table-row-bordered w-100 gridData'><thead><tr class=""text-center""><th id='thButtonName'>Button Name</th><th class='view'>View</th><th class='enable'>Enable</th></tr></thead>" & butRes & "</table>"
            but_div.InnerHtml = butRes
        End If

    End Sub

    Private Sub setlvval()
        If lv_xml.Value <> "" Then

            Dim xmldoc As New XmlDocument
            Dim res As String
            res = "<root><listviewbuttons>" & lv_xml.Value & "</listviewbuttons></root>"
            res = res.Replace("&", "&amp;")
            xmldoc.LoadXml(res)

            'List View Buttons
            Dim lvbutroots As XmlNodeList
            Dim prnodelvbuts As XmlNode
            Dim baseDataNodeslvButs As XmlNodeList
            lvbutroots = xmldoc.SelectNodes("//root/listviewbuttons")

            Dim lvbutRes As String = ""
            Dim lvbutrs As Integer = 1
            Dim nlvbutrs As String = "001"
            For Each prnodelvbuts In lvbutroots
                baseDataNodeslvButs = prnodelvbuts.ChildNodes
                For Each baseDataNodelvbut As XmlNode In baseDataNodeslvButs

                    Dim lvbutname As String = ""
                    Dim lvbutview As String = ""
                    Dim lvbutenable As String = ""
                    Dim nodelvbutname As XmlNode = baseDataNodelvbut.Attributes("cap")
                    If (nodelvbutname Is Nothing) Then
                        lvbutname = baseDataNodelvbut.Name
                    Else
                        lvbutname = nodelvbutname.Value
                    End If

                    Dim nodelvbutview As XmlNode = baseDataNodelvbut.Attributes("view")
                    If (nodelvbutview Is Nothing) Then
                        lvbutview = ""
                    Else
                        lvbutview = nodelvbutview.Value
                    End If

                    Dim nodelvbutenable As XmlNode = baseDataNodelvbut.Attributes("enable")
                    If (nodelvbutenable Is Nothing) Then
                        lvbutenable = ""
                    Else
                        lvbutenable = nodelvbutenable.Value
                    End If

                    Dim lvbutview_cb As String

                    lvbutview_cb = "<input class=""w-15px h-15px"" id=""lvbview" & nlvbutrs & """ data-fld-view=" & nlvbutrs & """  type=""checkbox"" value=""" & baseDataNodelvbut.Name & """"
                    If lvbutview = "true" Then
                        lvbutview_cb = lvbutview_cb & "  checked=""true""  />"
                    Else
                        lvbutview_cb = lvbutview_cb & "   />"
                    End If


                    Dim lvbutenable_cb As String
                    lvbutenable_cb = "<input class=""w-15px h-15px"" id=""lvbutenable" & nlvbutrs & """ data-fld-enable=" & nlvbutrs & """  type=""checkbox"" value=""" & lvbutname & """"
                    If lvbutenable = "true" Then
                        lvbutenable_cb = lvbutenable_cb & "  checked=""true""  />"
                    Else
                        lvbutenable_cb = lvbutenable_cb & "   />"
                    End If

                    lvbutrs = lvbutrs + 1
                    If lvbutrs.ToString.Length = 1 Then
                        nlvbutrs = "00" & lvbutrs
                    ElseIf lvbutrs.ToString.Length = 2 Then
                        nlvbutrs = "0" & lvbutrs
                    ElseIf lvbutrs.ToString.Length = 3 Then
                        nlvbutrs = lvbutrs
                    End If
                    lvbutRes = lvbutRes & "<tr><td>" & lvbutname & "</td><td class='text-center'>" & lvbutview_cb & "</td><td class=""text-center"">" & lvbutenable_cb & "</td></tr>"
                Next
            Next

            lvbutRes = "<table class='table table-sm table-row-bordered w-100 gridData'><thead><tr class=""text-center""><th id='thLVButtonName'>Button Name</th><th class='view'>View</th><th class='enable'>Enable</th></tr></thead>" & lvbutRes & "</table>"
            lv_div.InnerHtml = lvbutRes
        End If
    End Sub

    Protected Sub save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save.Click
        setValues()
        Dim iXml As String
        iXml = "<dcs>" & dc_xml.Value & "</dcs>"
        iXml = iXml & "<fields>" & fld_xml.Value & "</fields>"
        iXml = iXml & "<buttons>" & but_xml.Value & "</buttons>"
        iXml = iXml & "<listviewbuttons>" & lv_xml.Value & "</listviewbuttons>"


        Dim transid As String
        transid = Request.QueryString("transid")
        Dim cac_str As String = Request.QueryString("transid") & "tcexpr"
        Dim cacstr As String = Cache.Get(cac_str)

        iXml = iXml & cacstr

        Dim selrole As String
        selrole = Request.QueryString("role")

        Dim errlog As String = logobj.CreateLog("Calling SaveAccessTstDetails web service", Session("nsessionid"), "SaveUserAccessDetails", "")
        iXml = iXml.Replace("&", "&amp;")
        iXml = "<root axpapp=""" & Session("project") & """ user=""" & Session("user") & """ appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " sessionid= " & Chr(34) & Session("nsessionid") & Chr(34) & " trace=""" & errlog & """ transid=""" & transid & """  role=""" & selrole & """> " & iXml
        iXml &= Session("axApps").ToString() & Application("axProps").ToString() & Session("axGlobalVars").ToString() & Session("axUserVars").ToString() & "</root>"
        Dim res As String = String.Empty
        'Call service
        res = objWebServiceExt.CallSaveAccessTstDetailsWS(transid, iXml)

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
            hdnFormChanges.Value = "false"
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "showAlertDialog(""success"", eval(callParent('lcm[340]')))", True)
        End If
    End Sub

    Private Sub setValues()
        setdcval()
        setfldval()
        setbutval()
        setlvval()
    End Sub

    Private Sub ResetSessionTime()
        If Session("AxSessionExtend") IsNot Nothing AndAlso Session("AxSessionExtend").ToString() = "true" Then
            ClientScript.RegisterStartupScript(Me.[GetType](), "SessionAlert", "eval(callParent('ResetSession()', 'function'));", True)
        End If
    End Sub

    Protected Sub tc_cancel_Click(sender As Object, e As EventArgs)
        tc_expr_gv.EditIndex = -1
        tc_expr_gv.DataBind()
        recordUpdated = True
        BindDataGrid()
    End Sub

    Class TransactionControl
        Public Property Index As Int32
        Public Property Expression As String
        Public Property View As Boolean
        Public Property Edit As Boolean
        Public Property Delete As Boolean
    End Class

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function BindGridData(transid As String) As Object
        Dim SelResPageNo As String = String.Empty
        Dim ixml As String = String.Empty
        Dim response As String = String.Empty, result As String = String.Empty
        Dim ds As New DataSet()
        Dim cac_str As String = transid & "tcexpr"
        Dim a As String = System.Web.HttpContext.Current.Cache.Get(cac_str)
        If a IsNot Nothing Then
            Dim sr As New StringReader(a)
            ds.ReadXml(sr)
            Dim dc As DataColumn
            If ds.Tables.Count > 0 Then
                If ds.Tables("row").Rows.Count > 0 Then
                    Dim Tabindx As Integer = 0
                    'If ds.Tables(0).TableName = "row" Then Tabindx = 0 Else Tabindx = 1
                    Dim tcList = ds.Tables(0).AsEnumerable().[Select](Function(dataRow) New TransactionControl With
                            {
                            .Expression = dataRow.Field(Of String)("expr"),
                            .View = If(dataRow.Table.Columns("view") Is Nothing, False, (dataRow.Field(Of String)("view"))),
                            .Edit = If(dataRow.Table.Columns("edit") Is Nothing, False, (dataRow.Field(Of String)("edit"))),
                            .Delete = If(dataRow.Table.Columns("delete") Is Nothing, False, (dataRow.Field(Of String)("delete")))
                            }).ToList()
                    Return tcList
                End If
            End If
        End If
        Return New List(Of TransactionControl)
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function DeleteTC(index As Int32, transid As String) As Object
        Dim cac_str As String = transid & "tcexpr"
        Dim tcexpr_xml As String = System.Web.HttpContext.Current.Cache.Get(cac_str)

        Dim xmlDoc As New XmlDocument
        xmlDoc.LoadXml(tcexpr_xml)

        Dim rownodes As XmlNodeList
        Dim rxmlNode As XmlNode
        rownodes = xmlDoc.SelectNodes("//row")

        Dim st As String = ""
        Dim rowno As Integer = 0
        For Each rxmlNode In rownodes
            If index = rowno Then
                rxmlNode.ParentNode.RemoveChild(rxmlNode)
            End If
            rowno = rowno + 1
        Next

        Dim sw As New StringWriter()
        Dim xw As New XmlTextWriter(sw)
        xmlDoc.WriteTo(xw)


        System.Web.HttpContext.Current.Cache.Insert(cac_str, sw.ToString(), Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Dim result = BindGridData(transid)
        Return result
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GetTCDetails(index As Int32, transid As String) As Object

        Dim tc_betvalues_cb As String, tc_fld_cb As String, tc_opr_cb As String, tc_values_cb As String, tc_view_cb As Boolean, tc_edit_cb As Boolean, tc_delete_cb As Boolean, actionType As String
        Dim cac_str As String = transid & "tcexpr"
        Dim tcexpr_xml As String = System.Web.HttpContext.Current.Cache.Get(cac_str)

        Dim xmlDoc As New XmlDocument
        xmlDoc.LoadXml(tcexpr_xml)

        Dim rownodes As XmlNodeList
        Dim rxmlNode As XmlNode
        Dim rnodechild As XmlNodeList
        rownodes = xmlDoc.SelectNodes("//row")

        Dim st As String = ""
        Dim rowno As Integer = 0

        Dim viewsts As String = ""
        Dim editsts As String = ""
        Dim deletests As String = ""

        For Each rxmlNode In rownodes

            If index = rowno Then
                rnodechild = rxmlNode.ChildNodes
                For Each rNodetcfld As XmlNode In rnodechild
                    If rNodetcfld.Name = "selfld" Then
                        tc_fld_cb = rNodetcfld.InnerText
                    ElseIf rNodetcfld.Name = "opr" Then
                        tc_opr_cb = rNodetcfld.InnerText
                    ElseIf rNodetcfld.Name = "val" Then
                        tc_values_cb = rNodetcfld.InnerText
                    ElseIf rNodetcfld.Name = "bval" Then
                        tc_betvalues_cb = rNodetcfld.InnerText
                    ElseIf rNodetcfld.Name = "view" Then
                        viewsts = rNodetcfld.InnerText
                    ElseIf rNodetcfld.Name = "edit" Then
                        editsts = rNodetcfld.InnerText
                    ElseIf rNodetcfld.Name = "delete" Then
                        deletests = rNodetcfld.InnerText
                    End If
                Next
            End If
            rowno = rowno + 1
        Next

        If viewsts.ToLower = "true" Then
            tc_view_cb = True
        Else
            tc_view_cb = False
        End If

        If editsts.ToLower = "true" Then
            tc_edit_cb = True
        Else
            tc_edit_cb = False
        End If

        If deletests.ToLower = "true" Then
            tc_delete_cb = True
        Else
            tc_delete_cb = False
        End If

        If tc_opr_cb = "Between" Then
            'tc_betvalues_cb = True
        Else
            tc_betvalues_cb = ""
        End If
        Dim result = New With {
            Key .tc_fld_cb = tc_fld_cb,
            Key .tc_opr_cb = tc_opr_cb,
            Key .tc_values_cb = tc_values_cb,
            Key .tc_betvalues_cb = tc_betvalues_cb,
            Key .tc_view_cb = tc_view_cb,
            Key .tc_edit_cb = tc_edit_cb,
            Key .tc_delete_cb = tc_delete_cb
        }
        Return result
    End Function
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function tcUpdate(tc_betvalues_cb As String, tc_fld_cb As String, tc_opr_cb As String, tc_values_cb As String, tc_view_cb As Boolean, tc_edit_cb As Boolean, tc_delete_cb As Boolean, transid As String, actionType As String, index As Int32) As Object
        Dim exp As String
        If tc_betvalues_cb = "" Then
            exp = "if " & tc_fld_cb & " " & tc_opr_cb & " " & tc_values_cb
        Else
            exp = "if " & tc_fld_cb & " " & tc_opr_cb & " " & tc_values_cb & "," & tc_betvalues_cb
        End If

        Dim exprstr As String
        exprstr = "<row>"
        exprstr = exprstr & "<expr>" & exp & "</expr>"
        exprstr = exprstr & "<view>" & tc_view_cb & "</view><edit>" & tc_edit_cb & "</edit><delete>" & tc_delete_cb & "</delete>"
        exprstr = exprstr & "<selfld>" & tc_fld_cb & "</selfld><opr>" & tc_opr_cb & "</opr><val>" & tc_values_cb & "</val><bval>" & tc_betvalues_cb & "</bval>"
        exprstr = exprstr & "</row>"

        Dim cac_str As String = transid & "tcexpr"
        Dim tcexpr_xml As String = System.Web.HttpContext.Current.Cache.Get(cac_str)
        Dim tcexplen As Integer = tcexpr_xml.Length
        Dim newtcexpr As String
        If actionType = "add" Then
            newtcexpr = Mid(tcexpr_xml, 1, tcexplen - 9) & exprstr & "</tcexpr>"
            System.Web.HttpContext.Current.Cache.Insert(cac_str, newtcexpr, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Else
            If tc_betvalues_cb = "" Then
                exp = "if " & tc_fld_cb & " " & tc_opr_cb & " " & tc_values_cb
            Else
                exp = "if " & tc_fld_cb & " " & tc_opr_cb & " " & tc_values_cb & "," & tc_betvalues_cb
            End If

            exprstr = "<expr>" & exp & "</expr>"
            exprstr = exprstr & "<view>" & tc_view_cb & "</view><edit>" & tc_edit_cb & "</edit><delete>" & tc_delete_cb & "</delete>"
            exprstr = exprstr & "<selfld>" & tc_fld_cb & "</selfld><opr>" & tc_opr_cb & "</opr><val>" & tc_values_cb & "</val><bval>" & tc_betvalues_cb & "</bval>"

            tcexpr_xml = System.Web.HttpContext.Current.Cache.Get(cac_str)

            Dim editidx As Integer
            editidx = CInt(index)

            Dim xmlDoc As New XmlDocument
            xmlDoc.LoadXml(tcexpr_xml)

            Dim rownodes As XmlNodeList
            Dim rxmlNode As XmlNode
            rownodes = xmlDoc.SelectNodes("//row")


            Dim nxmlele As XmlElement
            nxmlele = xmlDoc.CreateElement("row")
            nxmlele.InnerXml = exprstr

            Dim st As String = ""
            Dim rowno As Integer = 0
            For Each rxmlNode In rownodes
                If editidx = rowno Then
                    rxmlNode.InnerXml = exprstr
                End If
                rowno = rowno + 1
            Next

            Dim sw As New StringWriter()
            Dim xw As New XmlTextWriter(sw)
            xmlDoc.WriteTo(xw)
            System.Web.HttpContext.Current.Cache.Insert(cac_str, sw.ToString(), Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        End If
        Dim result = BindGridData(transid)
        Return result
    End Function
End Class
