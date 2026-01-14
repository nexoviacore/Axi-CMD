
Partial Class ivtoivload
    Inherits System.Web.UI.Page
    Public resXhtm As String = String.Empty
    Public iname As String = String.Empty
    Dim webService As ASB.WebService = New ASB.WebService()
    Dim util As New Util.Util()
    Dim requestProcess_logtime As String = String.Empty
    Dim ObjExecTr As ExecTrace = ExecTrace.Instance
    Public direction As String = "ltr"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not (Request.QueryString("hdnbElapsTime") = "") Then
                Dim browserElapsTime = Request.QueryString("hdnbElapsTime")
                requestProcess_logtime += ObjExecTr.WireElapsTime(browserElapsTime)
            End If

            If Session("project") = "" Then
                Dim url As String
                url = util.SESSEXPIRYPATH
                Response.Write("<script>" & vbCrLf)
                Response.Write("parent.parent.location.href='" & url & "';")
                Response.Write(vbCrLf & "</script>")
            Else

                'If Request.UrlReferrer IsNot Nothing Then
                '    If Not (Request.UrlReferrer.AbsolutePath.ToLower().Contains("iview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("mainnew.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("listiview.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("ivtoivload.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("tstruct.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("main.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("portal.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("err.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("dashboard.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("cpwd.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("ParamsTstruct.aspx")) Then
                '        Response.Redirect("../CusError/AxCustomError.aspx")
                '    End If
                'End If
                If util.IsValidQueryString(Request.RawUrl, True) = False Then
                    Response.Redirect(util.ERRPATH + Constants.INVALIDURL)
                End If

                If (Request.QueryString("ivname") <> Nothing) Then
                    iname = Request.QueryString("ivname")
                    If Not util.IsValidIvName(iname) Then
                        Response.Redirect(Constants.PARAMERR)
                    End If
                End If
                Dim _isPopiv As String = ""
                If (Request.QueryString("AxIsPop") <> Nothing) Then
                    Session("AxIsPop") = "IviewPop"
                    _isPopiv = Request.QueryString("AxIsPop").ToString()
                    If Not util.IsChar(Request.QueryString("AxIsPop").ToString()) Then
                        Response.Redirect(Constants.PARAMERR)
                    End If
                End If

                If (Request.QueryString("AxSplit") <> Nothing) Then
                    Session("AxSplit") = "true"
                    If Not util.IsChar(Request.QueryString("AxSplit").ToString()) Then
                        Response.Redirect(Constants.PARAMERR)
                    End If
                End If

                Dim pop As String = ""
                pop = Request.QueryString("pop")
                If Not pop Is Nothing Then
                    If Not util.IsChar(pop) Then
                        Response.Redirect(Constants.PARAMERR)
                    End If
                End If
                Session("pop") = pop
                If Session("language").ToString() = "ARABIC" Then
                    direction = "rtl"
                End If

                Dim IsIvOpenAction As Boolean = False
                Dim IsIvNavData As Boolean = False


                If Not Request.QueryString("AxOpenIvAction") Is Nothing Then
                    IsIvOpenAction = Convert.ToBoolean(Request.QueryString("AxOpenIvAction"))
                ElseIf Not Request.QueryString("AxIvNav") Is Nothing Then
                    IsIvNavData = Convert.ToBoolean(Request.QueryString("AxIvNav"))
                End If

                Dim pNames As New ArrayList()
                Dim pValues As New ArrayList()

                'Dim hg As String

                Dim qn As Integer
                If IsIvOpenAction Or IsIvNavData Then
                    Dim strParams As String = String.Empty
                    If IsIvOpenAction Then
                        strParams = Session("AxActionParams" + iname)
                    ElseIf IsIvNavData Then
                        ' strParams = Session("IviewNavigationData-" + iname)
                        strParams = webService.GetIviewNavigationData(iname)
                        'If Not Request.QueryString("AxOpenAct") Is Nothing And Request.QueryString("AxOpenAct") = "true" And _isPopiv <> "true" Then
                        '    strParams = ""
                        'End If
                    End If

                    If strParams <> Nothing Then
                        'strParams = strParams.Replace("&grave;", "~")
                        Dim arrParams = strParams.Split("&")
                        Dim pVal As String = String.Empty
                        For qn = 0 To arrParams.Length - 1
                            If arrParams(qn) <> String.Empty And arrParams(qn).Contains("=") Then
                                Dim arrParamVal = arrParams(qn).Split("=")
                                pNames.Add(arrParamVal(0))
                                pVal = arrParamVal(1).ToString()
                                pVal = pVal.Replace("--.--", "&")
                                pVal = pVal.Replace("@eq@", "=")
                                pValues.Add(pVal)
                            End If
                        Next
                    End If
                Else
                    Dim newval As String
                    For qn = 1 To Request.QueryString.Count - 1 ' eliminate Name fro querystring
                        If Request.QueryString.Keys(qn) <> Nothing Then
                            If Request.QueryString.Keys(qn).ToLower <> "axsplit" And Request.QueryString.Keys(qn).ToLower <> "hdnbelapstime" Then
                                pNames.Add(Request.QueryString.Keys(qn))
                                newval = Request.QueryString.Item(qn)
                                newval = newval.Replace("--.--", "&")
                                newval = newval.Replace("&lt;", "<")
                                newval = util.CheckUrlSpecialChars(newval)
                                pValues.Add(newval)
                            End If
                        End If
                        'hg = hg & Request.QueryString.Keys(qn) & "--" & Request.QueryString.Item(qn) & "<br>"
                    Next
                End If




                Dim sXml As String
                Dim user As String
                Dim sid As String


                sXml = ""
                user = Session("user")


                sid = Session("nsessionid")

                Dim g As Integer
                For g = 0 To pNames.Count - 1
                    resXhtm = resXhtm & "<input type=hidden name=""" & pNames(g) & """ value=""" & pValues(g) & """>"
                Next
                resXhtm = resXhtm & "<input type =""hidden"" value=""Ok"">"
                If requestProcess_logtime IsNot Nothing Then
                    resXhtm = resXhtm & "<input type=hidden name=""reqProc_logtime"" value=""" & requestProcess_logtime & """>"
                End If
            End If
            'Adding the page url to the back and forward navigation if the page is not loaded through back or forward button click.
            If Session("backForwBtnPressed") IsNot Nothing AndAlso Not Convert.ToBoolean(Session("backForwBtnPressed")) Then
                Session("AxFromHypLink") = "true"
                util.UpdateNavigateUrl(HttpContext.Current.Request.Url.AbsoluteUri)
                Session("AxFromHypLink") = True
                Session("backForwBtnPressed") = True
            End If
        Catch ex As Exception
            Response.Redirect(util.ERRPATH + ex.Message + ex.StackTrace)
        End Try
    End Sub

End Class
