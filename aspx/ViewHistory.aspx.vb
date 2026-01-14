Imports System.Xml
Imports System.IO
Imports System.Data

Partial Class ViewHistory
    Inherits System.Web.UI.Page
    Dim _xmlString As String =
               "<?xml version=""1.0"" encoding=""utf-8"" ?>"
    Dim logobj As LogFile.Log = New LogFile.Log()
    Dim util As Util.Util = New Util.Util()
    Dim tid As String
    Dim rid As String
    Dim axpuser As String
    Dim sid As String
    Dim proj As String
    Dim traceVal As String
    Dim userNm As String
    Public noRec As String
    Public htmlText As String = ""
    Public direction As String = "ltr"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            tid = Request.QueryString("tid")
            If Not util.IsTransIdValid(tid) Then
                Response.Redirect(Constants.PARAMERR)
            End If

            axpuser = Session("user")
            sid = Session("nsessionid")
            ViewState("sid") = sid
            ViewState("tid") = tid
            rid = Request.QueryString("rid")

            If Not util.IsNumber(rid) Then
                Response.Redirect(Constants.PARAMERR)
            End If

            ViewState("rid") = rid
            proj = Session("project")
            traceVal = Session("trace")
            ViewState("trace") = traceVal
            ViewState("proj") = proj
            userNm = Session("user")
            ViewState("user") = userNm
        Else
            tid = ViewState("tid")
            rid = ViewState("rid")
            sid = ViewState("sid")
            proj = ViewState("proj")
            traceVal = ViewState("trace")
            userNm = ViewState("user")

        End If
        If Session("language").ToString() = "ARABIC" Then
            direction = "rtl"
        End If
        Dim ires As String = String.Empty
        Dim iXml As String = String.Empty
        Dim fileName As String = "GetHistoryData" + tid
        Dim errorLog As String = logobj.CreateLog("GetHistoryData.", sid, fileName, "new")

        'Call service
        ires = util.GetTstructDefObj(errorLog, tid).structRes

        Dim dbTimesql As String
        dbTimesql = "<root axpapp=" & Chr(34) & proj & Chr(34) & " sessionid= " & Chr(34) & sid & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " trace=" & Chr(34) & errorLog & Chr(34) & "  transid=" & Chr(34) & tid & Chr(34) & " recordid=" & Chr(34) & rid & Chr(34) & ">"
        dbTimesql += Session("axApps").ToString() + Application("axProps").ToString() + Session("axGlobalVars").ToString() + Session("axUserVars").ToString() & "</root>"
        Dim dbTimeres As String = String.Empty
        'Call service
        Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
        dbTimeres = objWebServiceExt.CallGetHistoryDataWS(tid, dbTimesql, ires)

        Dim strErrMsg As String = String.Empty
        strErrMsg = util.ParseXmlErrorNode(dbTimeres)


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

            Dim xmlDoc As New XmlDocument
            Dim xmlList As XmlNodeList
            Dim changeNode As XmlNode
            Dim fieldNodes As XmlNodeList
            Dim fieldNode As XmlNode
            Dim caption As String
            Dim rowNo As String
            Dim oldNodes As XmlNodeList
            Dim newNodes As XmlNodeList
            Dim oldValue As String = ""
            Dim newValue As String = ""
            Dim delFlag As String = ""
            Dim tempChange As String = ""
            Dim tempFields As String = ""
            Dim isDel As Boolean
            Dim isChanged As Boolean
            isDel = False
            isChanged = False
            Dim userName As String
            Dim modDate As String
            Dim oldRowNo As String
            Dim newTrans As String
            Dim attStatus As String
            Dim i As Integer
            xmlDoc.LoadXml(dbTimeres)
            htmlText = "<table>"
            xmlList = xmlDoc.GetElementsByTagName("root")
            If xmlList(0).ChildNodes.Count = 0 Then
                lblErrMsg.Text = "No History Available"
            Else
                For Each changeNode In xmlList(0).ChildNodes
                    userName = changeNode.Attributes("username").Value
                    modDate = changeNode.Attributes("modifieddate").Value
                    If Not changeNode.Attributes("newtrans") Is Nothing Then
                        newTrans = changeNode.Attributes("newtrans").Value
                        Dim canceltrans As String = ""
                        If Not changeNode.Attributes("canceltrans") Is Nothing Then
                            canceltrans = changeNode.Attributes("canceltrans").Value
                        End If
                        If canceltrans <> String.Empty And canceltrans = "yes" Then
                            tempChange = "<tr><td class=HistText>Cancelled by " + userName + " on " + modDate + "</td></tr>"
                            tempChange += "<tr><td class=HistText><font style='color:DarkRed'>Remarks:</font> " + changeNode.InnerText + "</td></tr>"
                        ElseIf newTrans = "yes" Then
                            tempChange = "<tr><td class=HistText>Created by " + userName + " on " + modDate + "</td></tr>"
                        Else
                            tempChange = "<tr><td class=HistText>Changed by " + userName + " on " + modDate + "</td></tr>"
                        End If
                    Else
                        tempChange = "<tr><td class=HistText>Changed by " + userName + " on " + modDate + "</td></tr>"
                    End If


                    fieldNodes = changeNode.ChildNodes
                    oldRowNo = "0"
                    For Each fieldNode In fieldNodes
                        If fieldNode.Name = "attach" Then
                            attStatus = fieldNode.Attributes("status").Value
                            fileName = fieldNode.InnerText
                            tempFields &= "<tr><td class=HistTextNrm>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Attachment " + "<font style='color:DarkRed'>" + fileName + "</font> was "
                            If attStatus = "a" Then
                                tempFields &= "added</td></tr>"
                            ElseIf attStatus = "d" Then
                                tempFields &= "deleted</td></tr>"
                            End If
                        ElseIf fieldNode.Name = "field" Then
                            caption = fieldNode.Attributes("caption").Value
                            rowNo = fieldNode.Attributes("rowno").Value
                            delFlag = fieldNode.Attributes("delflag").Value
                            oldNodes = fieldNode.SelectNodes("oldvalue")
                            If oldNodes.Count > 0 Then

                                For i = 0 To oldNodes(0).ChildNodes.Count - 1
                                    If oldValue = "" Then
                                        oldValue = oldNodes(0).ChildNodes(i).InnerText
                                    Else
                                        oldValue = oldValue + "" + oldNodes(0).ChildNodes(i).InnerText
                                    End If
                                Next
                            End If

                            newNodes = fieldNode.SelectNodes("newvalue")
                            If newNodes.Count > 0 Then
                                For i = 0 To newNodes(0).ChildNodes.Count - 1
                                    If newValue = "" Then
                                        newValue = newNodes(0).ChildNodes(i).InnerText
                                    Else
                                        newValue = newValue + "" + newNodes(0).ChildNodes(i).InnerText
                                    End If
                                Next
                            End If
                            If delFlag = "t" Then
                                ' TODO: CheckBox whether we can get rid of this if condition. rowno can never be zero.
                                If rowNo <> "0" Then
                                    If rowNo <> oldRowNo Then
                                        tempFields &= "<tr><td class=HistTextBld>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Row no. " + "<font style='color:DarkRed'>" + rowNo + "</font>" + " was Deleted</td></tr>"
                                    End If
                                    tempFields &= "<tr><td class=HistTextNrm>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font style='color:DarkRed'>" + caption + ":</font>" + " <font class=HistblTxt>" + oldValue + "</font></td></tr>"
                                Else

                                End If
                            Else
                                If rowNo <> "0" Then
                                    tempFields &= "<tr><td class=HistTextNrm>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font style='color:DarkRed'>" + caption + "</font> at row no. " + rowNo + " from " + "<font class=HistblTxt>" + oldValue + "</font>" + " to " + "<font class=HistblTxt>" + newValue + "</font></td></tr>"
                                Else
                                    tempFields &= "<tr><td class=HistTextNrm>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font style='color:DarkRed'>" + caption + "</font> from " + "<font class=HistblTxt>" + oldValue + "</font>" + " to " + "<font class=HistblTxt>" + newValue + "</font></td></tr>"
                                End If
                            End If

                            oldValue = String.Empty
                            newValue = String.Empty
                            oldRowNo = rowNo
                        End If



                    Next

                    htmlText &= tempChange + tempFields + "<tr><td style='height:10px;'></td><tr>"
                    tempFields = ""
                    tempChange = ""
                Next
                htmlText &= "</table>"
            End If
        End If
    End Sub

End Class
