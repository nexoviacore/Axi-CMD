
Imports System.Xml
Imports System.Text
Imports System.IO
Imports System.Data

Partial Class xmliview
    Inherits System.Web.UI.Page

    Dim _xmlString As String =
                   "<?xml version=""1.0"" encoding=""utf-8"" ?>"
    'Dim a As Ams.IAMSservice = New Ams.IAMSservice()
    Dim logobj As LogFile.Log = New LogFile.Log()
    Dim colHide As New ArrayList()
    Dim colFld As New ArrayList()
    Dim colHead As New ArrayList()
    Dim iname As String
    Dim user As String
    Dim sid As String
    Dim ivtype As String
    Dim proj As String
    Dim paramVal As String
    Dim paramXml As String = ""
    Dim util As New Util.Util
    Dim objIview
    Dim filename As String = String.Empty
    Dim listres As String = String.Empty




    Private Function getXML() As String

        Dim Ikey As String = String.Empty

        If Request.UrlReferrer IsNot Nothing Then
            If Not (Request.UrlReferrer.AbsolutePath.ToLower().Contains("iview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("listiview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("saveas.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("xmliview.aspx")) Then
                Response.Redirect("../CusError/AxCustomError.aspx")
            End If
        End If

        If Not IsPostBack Then
            iname = Request.QueryString("ivname")

            If Not iname Is Nothing Then
                If Not util.IsValidIvName(iname) Then
                    Response.Redirect(Constants.PARAMERR)
                End If
            End If

            user = Session("user")
            sid = Session("nsessionid")
            ivtype = Request.QueryString("ivtype")
            If Not ivtype Is Nothing Then
                If Not ivtype Is Nothing And Not util.IsChar(ivtype) Then
                    Response.Redirect(Constants.PARAMERR)
                End If
            End If
            If Session("project") = "" Then
                Response.Redirect("sess.aspx")
            Else
                proj = Session("project")

                If Session("ivKey") <> Nothing Then
                    Ikey = Session("ivKey").ToString()
                Else
                    If Not Request.QueryString("ivKey") Is Nothing Then
                        Ikey = Request.QueryString("ivKey")
                    End If
                End If

                If Not Request.QueryString("axpCache") Is Nothing Then
                    paramVal = Session("AxIvExportParams-" + iname).ToString()
                    Session("AxIvExportParams-" + iname) = ""
                Else
                    paramVal = Request.QueryString("params")
                End If

                If Session("ivname") <> Nothing Then
                    iname = Session("ivname").ToString()
                End If
                If Session("params") <> Nothing Then
                    paramVal = Session("params").ToString()
                End If
                If Session("ivtype") <> Nothing Then
                    ivtype = Session("ivtype").ToString()
                End If
                ViewState("ivtype") = ivtype
                ViewState("params") = paramVal
                ViewState("iname") = iname
                ViewState("ivtype") = ivtype
                ViewState("proj") = proj
                ViewState("user") = user
                ViewState("sid") = sid
            End If
        Else
            user = ViewState("user")
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

        ' If ivtype = "listview" Or ivtype = "lview" Then
        '     filename = objIview.LviewCaption.ToString()
        ' Else
        filename = objIview.IviewCaption.ToString()
        ' End If
        filename = filename.Replace(",", "")
        filename = filename.Replace(" ", "_")

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
        Dim header1 As String = String.Empty
        Dim header2 As String = String.Empty
        Dim header3 As String = String.Empty
        Dim footer1 As String = String.Empty
        Dim footer2 As String = String.Empty


        If Session("sOrder") <> Nothing And Session("sOrder").ToString() <> "" Then
            sortOrd = Session("sOrder").ToString()
        End If

        If Session("sColumn") <> Nothing And Session("sColumn").ToString() <> "" Then
            sortCol = Session("sColumn").ToString()
        End If

        If Session("fCol") <> Nothing And Session("fCol").ToString() <> "" Then
            filterCol = Session("fCol").ToString()
        End If

        If Session("fColVal") <> Nothing And Session("fColVal").ToString() <> "" Then
            filterColVal = Session("fColVal").ToString()
        End If


        Dim iXml As String
        iXml = "<root name =""" & iname & """ axpapp =""" & Session("project") & """ sessionid = """ & sid & """ appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " trace =" & Chr(34) & Session("trace") & Chr(34) & " pageno=""0"" firsttime=""yes"" sorder=""" & sortOrd & """ scol=""" & sortCol & """ fcolnm=""" & filterCol & """ fcolval=""" & filterColVal & """><params> "
        iXml = iXml & paramXml
        iXml = iXml & "</params>"
        iXml &= Session("axApps").ToString() + Application("axProps").ToString() + Session("axGlobalVars").ToString() + Session("axUserVars").ToString() & "</root>"
        Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
        Dim ires As String = String.Empty

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
            Else
                Dim dt As System.Data.DataTable = New System.Data.DataTable()
                dt = CType(Session("FilteredData"), System.Data.DataTable)
                Dim res As String = Session("res")
                ires = ConvertDatatableToXML(dt)
                ires = ires.Insert(7, res)

            End If
        End If

        If Mid(ires, 1, 7) <> Constants.ERROR Then
            ires = _xmlString & ires

            Dim xmlDoc As New XmlDocument
            xmlDoc.LoadXml(ires)





            Dim headXml As String = ""


            If objIview.ReportHdrs.Count > 0 Then
                Dim indx As Integer
                For indx = 0 To objIview.ReportHdrs.Count - 1 Step 1
                    headXml = headXml & "<header" & indx + 1 & ">" & objIview.ReportHdrs(indx).ToString() & "</header" & indx + 1 & ">"

                Next
            End If


            Dim elementNode As XmlElement = xmlDoc.CreateElement("header")
            elementNode.InnerXml = headXml
            xmlDoc.DocumentElement.PrependChild(elementNode)

            If iname <> String.Empty Then
                Dim elementNode4 As XmlElement = xmlDoc.CreateElement("ivname")
                elementNode4.InnerXml = filename
                xmlDoc.DocumentElement.PrependChild(elementNode4)

            End If
            If proj <> String.Empty Then
                Dim elementNode3 As XmlElement = xmlDoc.CreateElement("appTitle")
                elementNode3.InnerXml = proj
                xmlDoc.DocumentElement.PrependChild(elementNode3)

            End If


            Dim productNodes As XmlNodeList
            Dim productNode As XmlNode
            Dim baseDataNodes As XmlNodeList
            productNodes = xmlDoc.SelectNodes("//headrow")
            Dim hcolNos As Integer
            hcolNos = 0


            For Each productNode In productNodes
                baseDataNodes = productNode.ChildNodes
                For Each baseDataNode As XmlNode In baseDataNodes

                    colFld.Add(baseDataNode.Name)
                    colHead.Add(baseDataNode.InnerText)
                    'teat = teat & baseDataNode.Name & "......" & baseDataNode.InnerText & "..."
                    If baseDataNode.Name = "rowno" Then
                        colHide.Add("false")
                    Else
                        If Not baseDataNode.Attributes("hide") Is Nothing Then
                            colHide.Add(baseDataNode.Attributes("hide").Value)
                        End If
                        If baseDataNode.Name = "pivotghead" Then
                            colHide.Add("true")
                        End If
                        hcolNos = hcolNos + 1
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
                            If colFld(hidx) = baseDataNode3.Name Then
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
                            If colFld(idx) = baseDataNode1.Name Then
                                If colHide(idx) = "true" Then
                                    baseDataNode1.ParentNode.RemoveChild(baseDataNode1)
                                End If
                            End If
                        Next

                    Next
                Next

            Next
            Dim footerXml As String = ""

            If objIview.ReportFtrs.Count > 0 Then
                Dim indx As Integer
                For indx = 0 To objIview.ReportFtrs.Count - 1 Step 1
                    footerXml = footerXml & "<footer" & indx + 1 & ">" & objIview.ReportFtrs(indx).ToString() & "</footer" & indx + 1 & ">"

                Next
            End If

            Dim elementNode1 As XmlElement = xmlDoc.CreateElement("footer")
            elementNode1.InnerXml = footerXml
            xmlDoc.DocumentElement.AppendChild(elementNode1)


            Dim sw As New StringWriter()
            Dim xw As New XmlTextWriter(sw)
            xmlDoc.WriteTo(xw)

            Dim nXml As String
            nXml = sw.ToString()

            Return nXml
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
                paramXml = paramXml & paramVal
                paramXml = paramXml & "</" & tem1 & ">"

                params = params & "&" & tem1 & "="
                params = params & paramVal
            Else
                paramXml = "<params>"
            End If

        Next
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim encoding As New System.Text.ASCIIEncoding()
        Dim buffer() As Byte = encoding.GetBytes(getXML())

        If filename = String.Empty Then
            filename = "filename"
        End If

        Response.Clear()
        Response.ContentType = "file"
        Response.AppendHeader("Content-Disposition", "attachment;filename=" & filename & ".xml")
        Response.Charset = ""

        Response.BinaryWrite(buffer)

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
End Class
