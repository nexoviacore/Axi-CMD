Imports System.Data
Imports System.Xml
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions
Imports LogFile
Partial Class OpenLogFile
    Inherits System.Web.UI.Page
    Dim logobj As LogFile.Log = New LogFile.Log()
    Public Log As String = "Log\"
    Public cac_order As String
    Public EnableOldTheme As String

    Dim util As New Util.Util()
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
    ''' <summary>
    ''' onload call the function to bind the gridview related to logfiles.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'util.IsValidAxpertSession()


        If Session("AxEnableOldTheme") IsNot Nothing Then
            EnableOldTheme = Session("AxEnableOldTheme").ToString()
        End If

        Session("Logfile") = Nothing
        If Session("project") = String.Empty Or Session("nsessionid") = String.Empty Then
            Dim url As String
            url = util.SESSEXPIRYPATH
            Response.Write("<script>" & vbCrLf)
            Response.Write("parent.parent.location.href='" & url & "';")
            Response.Write(vbCrLf & "</script>")
            'ElseIf Not util.licencedValidSessionCheck() Then
            '    HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG, False)
            '    Return
        End If
        If Request.UrlReferrer IsNot Nothing Then
            If Not (Request.UrlReferrer.AbsolutePath.ToLower().Contains("main.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("openlogfile.aspx") Or Request.UrlReferrer.AbsolutePath.ToLower().Contains("mainnew.aspx")) Then
                Response.Redirect("../CusError/AxCustomError.aspx")
            End If
        End If
        If (Not IsPostBack) Then

            If Session("language") <> Nothing Then
                If Session("language").ToString() = "ARABIC" Then
                    direction = "rtl"
                End If
            End If
            Dim strFilenme As String = ""
            Dim sid As String
            If (Session("nsessionid") <> Nothing) Then
                sid = Session("nsessionid").ToString()
                cac_order = sid & "log"

                strFilenme = logobj.GetFileNames(sid)

                If (strFilenme <> "") Then
                    BindDataGrid(strFilenme)
                End If
                If (strFilenme = "") Then
                    BindDataGrid(strFilenme)
                End If
            Else
                Dim url As String
                url = util.SESSEXPIRYPATH
                Response.Write("<script>" & vbCrLf)
                Response.Write("parent.parent.location.href='" & url & "';")
                Response.Write(vbCrLf & "</script>")
            End If

        End If


    End Sub

    ''' <summary>
    ''' function which binds the data to the gridview control.
    ''' </summary>
    ''' <param name="fname"></param>
    ''' <remarks></remarks>
    Private Sub BindDataGrid(ByVal fname As String)

        Dim dt As New System.Data.DataTable()
        If Session("Logfile") IsNot Nothing Then
            dt = DirectCast(Session("Logfile"), System.Data.DataTable)
        Else
            dt.Columns.Add("FileName")
            dt.Columns.Add("link")
        End If

        If (fname = "") Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "CallShowAlertDialog", "ifTraceNdf();", True)
        End If

        Dim filearr = fname.Split(",")
        Dim i As Integer = 0
        For i = 0 To filearr.Length - 1
            Dim drow As DataRow = dt.NewRow()
            drow("FileName") = filearr(i).ToString()
            drow("link") = filearr(i).ToString()

            dt.Rows.Add(drow)
            Session("Logfile") = dt
        Next

        If Session("Logfile") IsNot Nothing Then
            dt = DirectCast(Session("Logfile"), System.Data.DataTable)
            If (dt IsNot Nothing) AndAlso (dt.Rows.Count > 0) Then
                gvLogfiles.Visible = True
                gvLogfiles.DataSource = dt
                gvLogfiles.DataBind()
            Else
                gvLogfiles.Visible = False
            End If
        End If

    End Sub

    ''' <summary>
    ''' rowcommand event of the gridview, onclick of the link in the gridview.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvLogfiles_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLogfiles.RowCommand
        If (e.CommandName = "Select") Then
            Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).Parent.Parent, GridViewRow)
            Dim link As String = e.CommandArgument.ToString()
            OpenFiles(link)
        End If
    End Sub

    ''' <summary>
    ''' function to open the log file.
    ''' </summary>
    ''' <param name="link"></param>
    ''' <remarks></remarks>
    Private Sub OpenFiles(ByVal link As String)

        Dim ScriptsPath As String = HttpContext.Current.Application("ScriptsPath")

        Dim outputPath As String
        outputPath = ScriptsPath & Log & Session("nsessionid").ToString() & "\" & link
        'Dim file As System.IO.FileInfo = New System.IO.FileInfo(path) '-- if the file exists on the server
        Dim file As New System.IO.FileInfo(outputPath)
        If file.Exists Then 'set appropriate headers

            Response.Clear()
            Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
            Response.AddHeader("Content-Length", file.Length.ToString())
            Response.ContentType = "application/x-download"
            Response.WriteFile(file.FullName)
            Response.End() 'if file does not exist
        Else
            Response.Write("This file does not exist.")
        End If 'nothing in the URL as HTTP GET
    End Sub

End Class
