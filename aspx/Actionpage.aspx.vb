Imports System.Xml
Imports System.IO

Partial Class Actionpage
    Inherits System.Web.UI.Page
    Public bodyonload As String = ""
    Public direction As String = "ltr"
    Dim util As Util.Util = New Util.Util()
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

        Dim _xmlString As String =
           "<?xml version=""1.0"" encoding=""utf-8"" ?>"
        If util.IsValidQueryString(Request.RawUrl) = False Then
            Response.Redirect(util.ERRPATH + Constants.INVALIDURL)
        End If
        Dim test2 As String
        test2 = ""
        Dim pgname As String
        pgname = Request.QueryString("name")
        Dim hltype As String
        hltype = Request.QueryString("hltype")

        ' Read Other Params in Querystring
        Dim pNames As New ArrayList()
        Dim pValues As New ArrayList()
        Dim qn As Integer
        For qn = 1 To Request.QueryString.Count - 1 ' eliminate Name fro querystring
            If Request.QueryString.Keys(qn) = "hltype" Then   ' remove hltype from params
            Else
                pNames.Add(Request.QueryString.Keys(qn))
                Dim strTemp As String = Request.QueryString.Item(qn)
                strTemp = strTemp.Replace("quot;", "'")
                pValues.Add(strTemp)
            End If
        Next

        If Session("project") = "" Then
            Dim url As String
            url = util.SESSEXPIRYPATH
            Response.Write("<script>" & vbCrLf)
            Response.Write("parent.parent.location.href='" & url & "';")
            Response.Write(vbCrLf & "</script>")
        Else
            If Session("language").ToString() = "ARABIC" Then
                direction = "rtl"
            End If
            Dim htmlframe As String = String.Empty
            Dim sXml As String = String.Empty
            Dim user As String = String.Empty
            Dim sid As String = String.Empty
            Dim result1 As String = String.Empty

            Dim tstWidth As String = String.Empty
            Dim topWidth As Integer
            topWidth = 100

            Dim logobj As LogFile.Log = New LogFile.Log()
            Dim fileName As String = "ActionPage" & pgname
            Dim errorLog As String = logobj.CreateLog("Get Structure object from cache for ActionPage.", pgname, "ActionPage-" & pgname, "new")

            pgname = pgname.Substring(6)
            'Call service
            result1 = util.GetTstructDefObj(errorLog, pgname).structRes

            Dim strErrMsg As String = String.Empty
            strErrMsg = util.ParseXmlErrorNode(result1)

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
                result1 = _xmlString & result1

                Dim xmlDoc As New XmlDocument
                xmlDoc.LoadXml(result1)

                Dim xHtm As String
                xHtm = ""

                'c container
                Dim cname As String
                cname = ""
                Dim cht As String
                cht = ""


                Dim topincpx As Double    ' % of increment is set here for top 
                topincpx = 0.0

                Dim productNodes As XmlNodeList
                Dim productNode As XmlNode
                Dim baseDataNodes As XmlNodeList

                productNodes = xmlDoc.SelectNodes("/root")
                For Each productNode In productNodes
                    baseDataNodes = productNode.ChildNodes
                    For Each baseDataNode As XmlNode In baseDataNodes

                        Dim cat As String
                        cat = baseDataNode.Attributes("cat").Value.ToUpper

                        Dim nodename As String
                        nodename = ""

                        If cat = "CNTR" Then
                            nodename = baseDataNode.Name
                            cname = nodename

                            'this var is used to find any iframe are in div 
                            Dim noIframe As String
                            noIframe = "None"

                            Dim color As String = ""
                            If Not baseDataNode.Attributes("color") Is Nothing Then
                                color = baseDataNode.Attributes("color").Value
                            End If


                            Dim tlhw = Split(baseDataNode.Attributes("tlhw").Value, ",")
                            Dim wid As Integer
                            wid = topWidth

                            If tlhw(3).ToString() = "0" Then
                                tstWidth = "100%"
                            Else
                                tstWidth = tlhw(3).ToString & "px"
                            End If


                            ' style = "top:" & tlhw(0).ToString & "px; left:" & tlhw(1).ToString & "px; height:" & tlhw(2).ToString & "px; width:" & wid & "px; position: absolute;border-right: lightgrey 1px solid; border-top: lightgrey 1px solid; border-left: lightgrey 1px solid; border-bottom: lightgrey 1px solid;"
                            Dim hgt As Integer
                            hgt = CInt(tlhw(2).ToString)

                            Dim top As Integer
                            'top = CInt(tlhw(0).ToString) + (CInt(tlhw(0).ToString) * topincpx / 100)
                            top = CInt(tlhw(0).ToString) + topincpx

                            cht = hgt

                            Dim clientpnl As String
                            clientpnl = ""
                            If baseDataNode.Attributes("parent").Value = "ClientPanel" Then
                                'xHtm = xHtm & "<div id=" & Chr(34) & "a2" & Chr(34) & " style=" & Chr(34) & style & Chr(34) & ">"
                                clientpnl = clientpnl & "<div id=" & Chr(34) & "a2" & Chr(34)
                                'Response.Write(xHtm & nodename & "</div>")
                            End If

                            ' Second XML read start to find the Child Nodes of Container

                            Dim productNodes2 As XmlNodeList
                            Dim productNode2 As XmlNode
                            Dim baseDataNodes2 As XmlNodeList

                            productNodes2 = xmlDoc.SelectNodes("/root")

                            Dim ivIfr As String
                            Dim butIfr As String
                            Dim noIfr As String
                            Dim imgIfr As String
                            htmlframe = ""
                            ivIfr = ""
                            butIfr = ""
                            noIfr = ""
                            imgIfr = ""

                            For Each productNode2 In productNodes2
                                baseDataNodes2 = productNode2.ChildNodes
                                For Each baseDataNode2 As XmlNode In baseDataNodes2
                                    ' New codes       
                                    Dim cat2 As String
                                    cat2 = baseDataNode2.Attributes("cat").Value

                                    Dim par As String

                                    ' this rel and actions are eliminated b'se of they don't have parennt as attribute
                                    If cat2 = "rel" Or cat2 = "actions" Then
                                        par = ""
                                    Else
                                        par = baseDataNode2.Attributes("parent").Value
                                    End If

                                    If par = cname Then
                                        noIframe = "YES"
                                        Dim nname As String
                                        nname = baseDataNode2.Name
                                        'iview code - find and call web service get iview structure    
                                        If cat2 = "iview" Then
                                            Dim iname As String
                                            iname = baseDataNode2.Attributes("name").Value

                                            'here create params list
                                            Dim paramStr As String
                                            paramStr = ""
                                            Dim ps As Integer
                                            For ps = 0 To pNames.Count - 1
                                                ' to remove . in the parameter name
                                                Dim ivdotPos As Integer
                                                ivdotPos = InStr(pNames(ps), ".")
                                                Dim iPname As String
                                                iPname = Mid(pNames(ps), ivdotPos + 1)
                                                Dim strValue = pValues(ps)
                                                strValue = strValue.ToString().Replace("'", "quot;")
                                                If (iPname <> "") Then
                                                    paramStr = paramStr & "&" & iPname & "=" & strValue
                                                End If
                                            Next

                                            cht = "300"  ' ht fixed for 10 rows
                                            htmlframe = htmlframe & "./nextquery.aspx?ivname=" & iname & paramStr

                                        ElseIf cat2 = "tstruct" Then

                                            Dim tid As String
                                            tid = baseDataNode2.Attributes("transid").Value

                                            ' if hyper link mode is not open then load the tstruct
                                            If (hltype <> "open") Then

                                                fileName = "GetRecordId" & tid
                                                errorLog = logobj.CreateLog("Getting Record Id.", sid, fileName, "new")
                                                Dim getRecXml As String
                                                getRecXml = "<root axpapp=" & Chr(34) & Session("project") & Chr(34) & " sessionid= " & Chr(34) & sid & Chr(34) & " trace=" & Chr(34) & errorLog & Chr(34) & " transid=" & Chr(34) & tid & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & "><values>"
                                                Dim g As Integer
                                                For g = 0 To pNames.Count - 1

                                                    ' to remove . in the parameter name
                                                    Dim dotPos As Integer = InStr(pNames(g), ".")
                                                    Dim tPname As String = Mid(pNames(g), dotPos + 1)
                                                    If (tPname <> "name") Then
                                                        getRecXml = getRecXml & "<" & tPname & ">" & pValues(g) & "</" & tPname & ">"
                                                    End If
                                                Next
                                                getRecXml = getRecXml & "</values>"
                                                getRecXml = getRecXml & Session("axApps").ToString() & Application("axProps").ToString() & Session("axGlobalVars").ToString() & Session("axUserVars").ToString() & "</root>"

                                                Dim gRecIdres As String = String.Empty

                                                Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()

                                                'Call service
                                                gRecIdres = objWebServiceExt.CallGetRecordIdWS(tid, getRecXml)
                                                gRecIdres = gRecIdres.Split("♠")(1)

                                                Dim errMsg As String = String.Empty
                                                errMsg = util.ParseXmlErrorNode(result1)


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

                                                    Dim xmlDoc2 As New XmlDocument
                                                    xmlDoc2.LoadXml(gRecIdres)

                                                    Dim pRecidNodes As XmlNodeList
                                                    Dim precIdNode As XmlNode
                                                    Dim recid As String = String.Empty

                                                    pRecidNodes = xmlDoc2.SelectNodes("/root/recordid")
                                                    For Each precIdNode In pRecidNodes
                                                        recid = precIdNode.InnerText
                                                    Next
                                                    tstWidth = "100%"
                                                    cht = "auto"
                                                    If recid = "0" Then
                                                        htmlframe = htmlframe & "tstruct.aspx?transid=" & tid
                                                    Else
                                                        htmlframe = htmlframe & "tstruct.aspx?act=load&transid=" & tid & "&recordid=" & recid
                                                    End If
                                                End If
                                            Else
                                                'here create params list
                                                Dim paramStr As String = String.Empty
                                                Dim ps As Integer
                                                For ps = 0 To pNames.Count - 1
                                                    ' to remove . in the parameter name
                                                    Dim ivdotPos As Integer
                                                    ivdotPos = InStr(pNames(ps), ".")
                                                    Dim iPname As String
                                                    iPname = Mid(pNames(ps), ivdotPos + 1)
                                                    If (iPname <> "name") Then
                                                        paramStr = paramStr & "&" & iPname & "=" & pValues(ps)
                                                    End If

                                                Next
                                                htmlframe = htmlframe & "tstruct.aspx?act=open&transid=" & tid & paramStr
                                            End If


                                        ElseIf cat2 = "btn" Then

                                            Dim btncap As String
                                            Dim btnimg As String
                                            btncap = baseDataNode2.Attributes("caption").Value
                                            btnimg = baseDataNode2.Attributes("img").Value
                                            Dim affcont As String
                                            affcont = baseDataNode2.Attributes("container").Value

                                            Dim sname As String
                                            sname = baseDataNode2.Attributes("sname").Value

                                            Dim action As String
                                            action = baseDataNode2.Attributes("action").Value

                                            Dim type As String
                                            type = ""
                                            Dim showin As String
                                            showin = ""
                                            Dim itname As String
                                            itname = ""

                                            If action <> "" Then
                                                ' third XML read start to find the Child Nodes of Container


                                                Dim productNodes3 As XmlNodeList
                                                Dim productNode3 As XmlNode
                                                Dim baseDataNodes3 As XmlNodeList

                                                productNodes3 = xmlDoc.SelectNodes("/root/actions")
                                                For Each productNode3 In productNodes3
                                                    baseDataNodes3 = productNode3.ChildNodes
                                                    For Each baseDataNode3 As XmlNode In baseDataNodes3
                                                        'xHtm = xHtm & baseDataNode3.Name & "....."   'Action names
                                                        If action = baseDataNode3.Name Then
                                                            Dim baseDataNodes4 As XmlNodeList
                                                            'baseDataNodes4 = baseDataNode3("r1").ChildNodes
                                                            baseDataNodes4 = baseDataNode3.ChildNodes
                                                            For Each baseDataNode5 As XmlNode In baseDataNodes4
                                                                'xHtm = xHtm & baseDataNode31.Name & "....."   'r1
                                                                If type = "" Then
                                                                    type = baseDataNode5.Attributes("task").Value
                                                                Else
                                                                    type = type & "|" & baseDataNode5.Attributes("task").Value
                                                                End If

                                                                Dim baseDataNodes6 As XmlNodeList
                                                                'baseDataNodes4 = baseDataNode3("r1").ChildNodes
                                                                baseDataNodes6 = baseDataNode5.ChildNodes
                                                                For Each baseDataNode7 As XmlNode In baseDataNodes6
                                                                    'xHtm = xHtm & baseDataNode5.Name & "....."  ' Param1
                                                                    Dim baseDataNodes8 As XmlNodeList
                                                                    baseDataNodes8 = baseDataNode7.ChildNodes
                                                                    For Each baseDataNode9 As XmlNode In baseDataNodes8
                                                                        'xHtm = xHtm & baseDataNode9.Name & "....."  'visible ,caption,showin

                                                                        If baseDataNode9.Name = "showin" Then
                                                                            If showin = "" Then
                                                                                showin = baseDataNode9.InnerText
                                                                            Else
                                                                                showin = showin & "|" & baseDataNode9.InnerText
                                                                            End If
                                                                        ElseIf baseDataNode9.Name = "name" Then
                                                                            If itname = "" Then
                                                                                itname = baseDataNode9.InnerText
                                                                            Else
                                                                                itname = itname & "|" & baseDataNode9.InnerText
                                                                            End If

                                                                        End If

                                                                    Next
                                                                Next
                                                            Next
                                                        End If
                                                    Next
                                                Next

                                            End If


                                            If btnimg <> "" Then
                                                htmlframe = htmlframe & "&nbsp;&nbsp;<span style=""left: 5px; position: relative; top: 7px;""><a href=""javascript:ref('" & affcont & "','" & sname & "');"" style='border:0px;' class=btncap><img src=..\AxpImages\" & btnimg & "  border=0  > <font class=btncap> " & btncap & " </font></a></span>"
                                            Else
                                                htmlframe = htmlframe & "<span class=handCur><a class=""squarebutton""  style=""margin-left: 6px"" href=""javascript:ref('" & showin & "','" & itname & "','" & type & "');"" ><span>" & btncap & "</span></a></span>"
                                            End If


                                            'xHtm = xHtm & "&nbsp;&nbsp;<span style=""left: 5px; position: relative; top: 7px;""><a href=""javascript:ref('" & affcont & "');"" border=0 class=btncap><img src=.\AxpImages\" & btnimg & "  border=0  > <font class=btncap> " & btncap & " </font></a></span>"

                                        ElseIf cat2 = "img" Then
                                            Dim imgname As String
                                            imgname = baseDataNode2.Attributes("file").Value
                                            htmlframe = htmlframe & "<img src=""pageimg/" & imgname & """ style='border:0px;'>"
                                        End If

                                    End If   'if par = cname



                                Next
                            Next
                            If noIframe = "None" Then
                                htmlframe = htmlframe & "./mid1.aspx"
                            End If

                            xHtm = xHtm & "</div>"
                        End If


                    Next
                Next
            End If
            Dim docHt As Integer = 100
            Dim tst_Scripts As String = ""
            tst_Scripts = tst_Scripts & "<script language=""javascript"" type=""text/javascript"" >function setDocht(){ adjustwin(""" & docHt & """,window);}</script>"
            Response.Write(tst_Scripts)


            bodyonload = "javascript:setDocht();ProcessFillGrid('" & htmlframe & "');"
        End If
    End Sub

End Class
