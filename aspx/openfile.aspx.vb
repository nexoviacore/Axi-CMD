Imports System.IO
Imports System.IO.Packaging
Imports System.Net

Partial Class openfile
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' onload get the path of the file and open the file.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim fpath As String = ""
        Dim zipPath As String = ""
        Dim savePath As String = ""
        Dim docID As String = ""
        Dim dirPath As String = ""
        Dim strOpenfile As String = ""
        Dim validFilename As Boolean = False
        Dim strRequestfromImp As String = ""
        Dim strRequestToTemp As String = ""
        Dim utilObj As Util.Util = New Util.Util
        Dim sid As String = ""

        If Session("nsessionid") <> Nothing Then
            sid = Session("nsessionid").ToString()
        End If
        If utilObj.IsValidQueryString(Request.RawUrl) = False Then
            Response.Redirect(utilObj.ERRPATH + Constants.INVALIDURL)
        End If
        Dim strRequest As String = Request.QueryString("fpath") '-- if something was passed to the file querystring
        strRequest = strRequest.Replace(";bkslh", "\")
        strRequest = strRequest.Replace("\\", "\")

        Try
            strRequest = utilObj.ReplaceUrlSpecialChars(strRequest)
            If (strRequest.IndexOf("♠") > -1) Then

                strRequest = strRequest.Replace("♠", "&")
            End If
            Dim iname As String = Request.QueryString("iname")
            If iname <> Nothing And iname = "ad_implg" Then
                Dim ScriptsPath As String = ""
                Dim filePath As String = ""
                ScriptsPath = HttpContext.Current.Application("ScriptsPath")
                filePath = ScriptsPath & "importedfiles\" & strRequest
                strOpenfile = filePath
                Dim sfile As New System.IO.FileInfo(strOpenfile)

                If (sfile.Exists) Then
                    Dim dirname As String = Request.QueryString("dirname") '-- if something was passed to the file querystring
                    dirname = dirname.Replace(";bkslh", "\")
                    dirname = dirname.Replace("\\", "\")
                    Response.AddHeader("Content-Disposition", "attachment; filename=" & dirname)
                    Response.AddHeader("Content-Length", sfile.Length.ToString())
                    Response.ContentType = "application/octet-stream"
                    Response.WriteFile(sfile.FullName)
                    Response.Flush()
                End If
                Return

            End If
            If Request.QueryString("ImpIview") <> Nothing Then
                strRequestToTemp = Request.QueryString("ImpIview").ToString()
            End If

            If strRequest <> "" Then 'get absolute path of the file
                Dim ScriptsPath As String = ""
                Dim filePath = ""
                If sid <> "" Then
                    ScriptsPath = HttpContext.Current.Application("ScriptsPath")

                    filePath = ScriptsPath & "axpert\" & sid & "\" & strRequest

                    'If Request.QueryString("fpath").Contains(".docx") Or Request.QueryString("fpath").Contains(".doc") Then
                    '    filePath = ScriptsPath & "axpert\" & sid & "\" & "Docs\" & strRequest
                    'Else
                    '    filePath = ScriptsPath & "axpert\" & sid & "\" & strRequest
                    'End If
                End If
                If strRequestToTemp <> "" Then
                    If strRequest = "ImpTemplate" Then
                        If Session("AxImpExpTemptPath") <> Nothing Then
                            Dim imppath As String = Session("AxImpExpTemptPath")
                            filePath = imppath & "\" & strRequestToTemp & ".xlt"
                        End If
                    End If
                End If

                strOpenfile = filePath


                If Request.QueryString("docid") <> Nothing Then
                    If (utilObj.IsAlphaNum(Request.QueryString("docid").ToString())) Then
                        docID = Request.QueryString("docid").ToString()
                    End If
                End If
                If Request.QueryString("path") <> Nothing Then
                    savePath = Request.QueryString("path").ToString()
                    savePath = utilObj.ReplaceUrlSpecialChars(savePath)
                End If

                If Request.QueryString("dirName") <> Nothing Then
                    dirPath = Request.QueryString("dirName").ToString()
                    dirPath = dirPath.Replace(";bkslh", "\")
                    dirPath = utilObj.ReplaceUrlSpecialChars(dirPath)
                End If

                If Request.QueryString("Imp") <> Nothing Then 'Imp is set t to download the file
                    strRequestfromImp = Request.QueryString("Imp").ToString()
                End If


                Dim file As New System.IO.FileInfo(filePath)

                Dim folderPath = ScriptsPath & "axpert\" & sid & "\" & dirPath & "\"
                If file.Exists Then 'set appropriate headers
                    If savePath <> "" Then           'If Path is given for saving file
                        If docID <> "" Then
                            fpath = savePath & docID
                        Else
                            fpath = savePath
                        End If

                        Dim di As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(folderPath)
                        Dim diFileinfo() As System.IO.FileInfo = di.GetFiles()
                        Dim drfile As System.IO.FileInfo
                        Dim strFilename As String = String.Empty

                        If Not Directory.Exists(fpath) Then
                            Directory.CreateDirectory(fpath)
                        End If

                        For Each drfile In diFileinfo
                            If (drfile.ToString() <> docID & ".zip") Then
                                System.IO.File.Copy(folderPath & "\" & drfile.ToString(), fpath & "\" & drfile.ToString(), True)
                                strFilename = fpath & "\" & drfile.ToString()
                            End If
                        Next

                        strOpenfile = strFilename

                    End If
                    If Request.QueryString("compress") <> Nothing Then
                        If Request.QueryString("compress").ToString().ToLower() = "True" Then           'If file should be compressed
                            If savePath <> "" Then
                                zipPath = savePath
                            Else
                                zipPath = folderPath
                            End If

                            If docID <> "" Then
                                zipPath = zipPath & docID
                            Else
                                zipPath = zipPath & file.Name
                            End If
                            Dim zip As Package = ZipPackage.Open(zipPath & ".zip", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite)
                            Dim dir As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(folderPath)
                            Dim dirFileinfo As System.IO.FileInfo() = dir.GetFiles()
                            AddToArchive(zip, folderPath & file.Name)
                            zip.Close()

                            strOpenfile = zipPath & ".zip"
                        End If
                    End If
                    Dim sfile As New System.IO.FileInfo(strOpenfile)

                    If (sfile.Exists) Then               'If path is not given and compression = false
                        If strRequestfromImp = "t" Then
                            Response.AddHeader("Content-Disposition", "attachment; filename=" & sfile.Name)
                            Response.AddHeader("Content-Length", sfile.Length.ToString())
                            Response.ContentType = "application/octet-stream"
                            Response.WriteFile(sfile.FullName)
                            Response.Flush()
                        Else
                            Response.Clear()
                            Response.AddHeader("Content-Disposition", "inline; filename=" & sfile.Name)
                            Response.AddHeader("Content-Length", sfile.Length.ToString())
                            'Response.ContentType = "application/x-download"
                            Response.ContentType = "application/pdf"
                            Response.WriteFile(sfile.FullName)
                            Response.Flush()

                        End If
                    End If
                End If
            End If
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


    <System.Web.Services.WebMethod()>
    Public Shared Function GetPdfFile(ByVal urfpath As String, ByVal urdocid As String, ByVal urpath As String, ByVal dirName As String) As String
        Dim fpath As String = ""
        Dim zipPath As String = ""
        Dim savePath As String = ""
        Dim docID As String = ""
        Dim dirPath As String = ""
        Dim strOpenfile As String = ""
        Dim validFilename As Boolean = False
        Dim strRequestToTemp As String = ""
        Dim utilObj As Util.Util = New Util.Util
        Dim logObj As LogFile.Log = New LogFile.Log
        Dim sid As String = ""

        If HttpContext.Current.Session("nsessionid") <> Nothing Then
            sid = HttpContext.Current.Session("nsessionid").ToString()
        End If

        Dim strRequest As String = urfpath '-- if something was passed to the file querystring
        strRequest = strRequest.Replace(";bkslh", "\")
        strRequest = strRequest.Replace("\\", "\")

        Try
            strRequest = utilObj.ReplaceUrlSpecialChars(strRequest)

            If strRequest <> "" Then 'get absolute path of the file
                Dim ScriptsPath As String = ""
                Dim filePath = ""
                If sid <> "" Then
                    ScriptsPath = HttpContext.Current.Application("ScriptsPath")
                    filePath = ScriptsPath & "axpert\" & sid & "\" & strRequest
                    strRequestToTemp = ScriptsPath & "axpert\" & sid
                    If strRequest.Contains("PDF\") Then
                        strRequestToTemp = strRequestToTemp & "\PDF"
                    End If
                End If

                strOpenfile = filePath

                If urdocid <> Nothing Then
                    If (utilObj.IsAlphaNum(urdocid.ToString())) Then
                        docID = urdocid.ToString()
                    End If
                End If
                If urpath <> Nothing Then
                    savePath = urpath.ToString()
                    savePath = utilObj.ReplaceUrlSpecialChars(savePath)
                End If

                If dirName <> Nothing Then
                    dirPath = dirName.ToString()
                    dirPath = dirPath.Replace(";bkslh", "\")
                    dirPath = utilObj.ReplaceUrlSpecialChars(dirPath)
                End If

                filePath = Uri.UnescapeDataString(filePath)
                Dim file As New System.IO.FileInfo(filePath)

                Dim folderPath = ScriptsPath & "axpert\" & sid & "\" & dirPath & "\"
                If file.Exists Then 'set appropriate headers
                    If savePath <> "" Then           'If Path is given for saving file
                        If docID <> "" Then
                            fpath = savePath & docID
                        Else
                            fpath = savePath
                        End If

                        Dim di As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(folderPath)
                        Dim diFileinfo() As System.IO.FileInfo = di.GetFiles()
                        Dim drfile As System.IO.FileInfo
                        Dim strFilename As String = String.Empty

                        If Not Directory.Exists(fpath) Then
                            Directory.CreateDirectory(fpath)
                        End If

                        For Each drfile In diFileinfo
                            If (drfile.ToString() <> docID & ".zip") Then
                                System.IO.File.Copy(folderPath & "\" & drfile.ToString(), fpath & "\" & drfile.ToString(), True)
                                strFilename = fpath & "\" & drfile.ToString()
                            End If
                        Next

                        strOpenfile = strFilename

                    End If

                    strOpenfile = Uri.UnescapeDataString(strOpenfile)
                    Dim sfile As New System.IO.FileInfo(strOpenfile)

                    If (sfile.Exists) Then
                        Dim strPath As String = HttpContext.Current.Server.MapPath("../downloads/")
                        Dim strFilename As String
                        strFilename = utilObj.CopyFilesPDF(strRequestToTemp, strPath, sfile.Name)
                        Return "../downloads/" + strFilename
                    End If
                End If
            End If
        Catch ex As Exception
            logObj.CreateLog("Open PDF:   " + ex.Message, HttpContext.Current.Session("nsessionid").ToString(), "Openfile", "new")
            Return "error"
        End Try
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function DeletePrintPDF(pdfPath As String) As String
        If File.Exists(HttpContext.Current.Server.MapPath(pdfPath)) Then
            File.Delete(HttpContext.Current.Server.MapPath(pdfPath))
        End If
        Return "true"
    End Function

End Class
