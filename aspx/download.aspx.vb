Imports System.IO.Packaging
Imports System.IO

Partial Class aspx_download
    Inherits System.Web.UI.Page
    Public langType As String = "en"
    Dim util As Util.Util = New Util.Util()
    Public direction As String = "ltr"

    Protected Overrides Sub InitializeCulture()
        If Session("language") IsNot Nothing Then
            Util = New Util.Util()
            Dim dirLang As String = String.Empty
            dirLang = Util.SetCulture(Session("language").ToString().ToUpper())
            If Not String.IsNullOrEmpty(dirLang) Then
                direction = dirLang.Split("-")(0)
                langType = dirLang.Split("-")(1)
            End If
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim act As String = Request.QueryString("act")
        If act = "" Then
            ZipFiles()
        End If
        Dim sid As String = Session("nsessionid").ToString()
        Dim ScriptsPath As String = ConfigurationManager.AppSettings("ScriptsPath")
        Dim filePath = ScriptsPath & "axpert/" & sid & "/downloads/myzip.zip"
        Dim file As New System.IO.FileInfo(filePath)
        If (Request.Browser.Type <> "IE7" And Request.Browser.Type <> "IE8") Or act <> "" Then
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
        End If
    End Sub
    Private Sub ZipFiles()
        Try

            Dim sid As String = Session("nsessionid").ToString()
            Dim ScriptsPath As String = ConfigurationManager.AppSettings("ScriptsPath")
            Dim zipPath As String = ScriptsPath & "axpert\" & sid & "\downloads\myzip.zip"

            'create a new one 
            Dim zip As Package = ZipPackage.Open(zipPath, IO.FileMode.Create, IO.FileAccess.ReadWrite)

            'Add all files in the folder:
            Dim folderPath As String = ScriptsPath & "axpert\" & sid & "\downloads\"
            Dim di As New IO.DirectoryInfo(folderPath)
            Dim diFileinfo As IO.FileInfo() = di.GetFiles()
            Dim drfile As IO.FileInfo

            For Each drfile In diFileinfo
                If drfile.ToString <> "myzip.zip" Then
                    AddToArchive(zip, ScriptsPath & "axpert\" & sid & "\downloads\" & drfile.ToString)
                End If
            Next
            zip.Close() 'Close the zip file
            'delete files after Zip
            For Each drfile In diFileinfo
                If drfile.ToString <> "myzip.zip" Then
                    File.Delete(ScriptsPath & "axpert\" & sid & "\downloads\" & drfile.ToString)
                End If
            Next
        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        End Try

    End Sub

    Private Sub AddToArchive(ByVal zip As Package, ByVal fileToAdd As String)
        Try
            'Replae spaces with an underscore (_) 
            Dim uriFileName As String = fileToAdd.Replace(" ", "_")

            'A Uri always starts with a forward slash "/" 
            Dim zipUri As String = String.Concat("/", IO.Path.GetFileName(uriFileName))

            Dim partUri As New Uri(zipUri, UriKind.Relative)
            Dim contentType As String = Net.Mime.MediaTypeNames.Application.Zip

            'The PackagePart contains the information: 
            ' Where to extract the file when it's extracted (partUri) 
            ' The type of content stream (MIME type):  (contentType) 
            ' The type of compression:  (CompressionOption.Normal)   
            Dim pkgPart As PackagePart = zip.CreatePart(partUri, contentType, CompressionOption.Normal)

            'Read all of the bytes from the file to add to the zip file 
            Dim bites As Byte() = File.ReadAllBytes(fileToAdd)

            'Compress and write the bytes to the zip file 
            pkgPart.GetStream().Write(bites, 0, bites.Length)
        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        End Try

    End Sub
End Class
