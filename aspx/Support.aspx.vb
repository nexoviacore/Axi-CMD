Imports System.Data
Imports System.IO


Partial Class Support
    Inherits System.Web.UI.Page
    Dim objWebServiceExt As ASBExt.WebServiceExt = New ASBExt.WebServiceExt()
    Dim colHide As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then
            'If Not (Session("validated") = "True" And Session("user") = "admin") Then
            'Session.Abandon()
            'Response.Redirect("default.aspx?msg=Invalid user")
            'End If
        End If
        If Session("project") = "" Then
            Response.Redirect("../default.aspx?msg=Session Expired")
        Else

        End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        outxml.InnerHtml = ""
        Dim iXml As String
        Dim sql As String = insql.Value.ToString().ToLower()

        Dim sqlArrayList(4) As String
        sqlArrayList(0) = "DELETE"
        sqlArrayList(1) = "UPDATE"
        sqlArrayList(2) = "ALTER"
        sqlArrayList(3) = "TRUNCATE"
        sqlArrayList(4) = "DROP"

        Dim sqlflag As Boolean = True

        For Each element As String In sqlArrayList
            If sql.Contains(element.ToLower()) Then
                sqlflag = False
                Exit For
            End If
        Next

        If sqlflag Then
            lblErr.Text = ""
            iXml = ""
            iXml = iXml & "<sqlresultset axpapp=" & Chr(34) & Session("project") & Chr(34) & " sessionid= " & Chr(34) & Session("nsessionid") & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " Trace = " & Chr(34) & "False" & Chr(34) & " user=" & Chr(34) & Session("user") & Chr(34) & " transid="""">"
            iXml = iXml & "<sql>" & sql & "</sql></sqlresultset>"
            Dim result As String = ""
            Dim qexception As Boolean = False
            'get sql result from getchoices webservice.
            Try
                result = objWebServiceExt.CallGetChoiceWebService("", iXml)
            Catch ex As Exception
                qexception = True
                outxml.InnerHtml = ex.ToString()
            End Try

            If Not qexception Then
                LogQuery(sql)
            End If

            GridView1.Columns.Clear()
            GridView1.DataBind()
            outxml.InnerHtml = ""

            If RadioButtonList1.SelectedValue = "XML" Then
                outxml.Visible = True
                GridView1.Visible = False
                Panel2.Visible = False
                errmsg.InnerHtml = ""
                outxml.InnerHtml = "<textarea id=""outxmlres"" cols=""100"" rows=""10"" width=""95%"">" & result & "</textarea>"
            Else
                GridView1.Visible = True
                outxml.Visible = False
                Panel2.Visible = True
                Try
                    Dim sr As New StringReader(result)
                    Dim ds As New DataSet
                    ds.ReadXml(sr)
                    GridView1.DataSource = ds.Tables(2)
                    Session("sqlres") = ds.Tables(2)
                    colHide = 0
                    GridView1.DataBind()
                Catch ex As Exception

                End Try
            End If
        Else
            LogQuery(sql)
            lblErr.Text = "The query should Not contain DELETE/UPDATE/ALTER/TRUNCATE/DROP."
            GridView1.Visible = False
        End If
    End Sub



    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        outxml.InnerHtml = ""
        GridView1.Columns.Clear()
        GridView1.DataBind()
        GridView1.Visible = False
        Panel2.Visible = False
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Dim m As Integer
        For m = 0 To e.Row.Cells.Count - 1
            If e.Row.Cells(m).Text.Contains("response_Id") Then
                e.Row.Cells(m).Visible = False
                colHide = m
            End If
        Next
        If colHide <> 0 Then
            Try
                e.Row.Cells(colHide).Visible = False
            Catch ex As Exception
            End Try
        End If
    End Sub

    Protected Sub LogQuery(ByVal sql As String)
        Dim cdt As Date = DateTime.Now()
        sql = sql.Replace("'", "''")
        Dim ipadd As String = ""
        ipadd = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If (ipadd = String.Empty) Then
            ipadd = Request.ServerVariables("REMOTE_ADDR")
        End If

        Dim lsql As String = "insert into webquerylog values('" & Session("user") & "',to_date('" & cdt & "','MM/dd/yyyy HH:MI:SS PM'),'" & sql & "','" & ipadd & "')"
        Dim logXML As String = ""
        logXML = logXML & "<sqlresultset axpapp=" & Chr(34) & Session("project") & Chr(34) & " sessionid= " & Chr(34) & Session("nsessionid") & Chr(34) & " appsessionkey=" & Chr(34) & Session("AppSessionKey").ToString() & Chr(34) & " username=" & Chr(34) & Session("username").ToString() & Chr(34) & " trace=" & Chr(34) & "false" & Chr(34) & " user=" & Chr(34) & Session("user") & Chr(34) & " transid="""">"
        logXML = logXML & "<sql>" & lsql & "</sql></sqlresultset>"
        Dim logresult As String = ""
        Try
            logresult = objWebServiceExt.CallGetChoiceWebService("", logXML)
        Catch ex As Exception

        End Try
    End Sub


End Class
