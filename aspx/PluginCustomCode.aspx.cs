using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;

public partial class PluginCustomCode : System.Web.UI.Page
{
    public static string[] allowedExtensions = { ".js", ".css", ".html", ".json", ".png", ".svg", ".jpeg", ".jpg" };
    public static long maxFileSize = 10 * 1024 * 1024; // 10MB
    Util.Util util;
    protected void Page_Load(object sender, EventArgs e)
    {
        util = new Util.Util();

        if (HttpContext.Current.Session["project"] == null || HttpContext.Current.Session["project"].ToString() == "")
        {
            SessionExpired();
        }
        else
        {
            if (!util.CheckValidLogin())
            {
                SessionExpired();
                return;
            }
            string strutureJson = GetStructuresDetails();
            if (strutureJson.IndexOf("{\"error\":{\"msg\"") != -1)
            {
                var jsonObj = JObject.Parse(strutureJson);
                string msg = jsonObj["error"]["msg"].ToString();

                Response.Redirect(Constants.ERRPATH + msg);
            }

            hdnStructures.Value = strutureJson;
        }
    }

    private string GetStructuresDetails()
    {
        string dbType = System.Web.HttpContext.Current.Session["axdb"].ToString();

        string _sqlQuery = "select name, caption caption, 'tstruct' as type from tstructs t  union all select name, caption caption, 'report' as type from iviews i union all select name, caption caption, 'html' as type from axpages where pagetype = 'web' order by caption ASC";

        if (dbType.ToLower().Replace(" ", "") == "mssql")
            _sqlQuery = "select name, caption collate Arabic_CI_AS caption, 'tstruct' as type from tstructs t  union all select name, caption collate Arabic_CI_AS caption, 'report' as type from iviews i union all select name, caption collate Arabic_CI_AS caption, 'html' as type from axpages where pagetype = 'web' order by caption ASC";

        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        string result = asbExt.ExecuteSQL("homecards", _sqlQuery, "JSON");
        return result;
    }

    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }

    [WebMethod]
    public static string CreateFolder(string parentFolder, string folderName)
    {
        try
        {
            if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
            {
                return Constants.SESSIONTIMEOUT;
            }

            string applicationPath = HttpRuntime.AppDomainAppPath;
            string folderPath = Path.Combine(applicationPath, parentFolder);

            string validFolder = validateFolder(folderPath);
            if (validFolder == "Valid")
            {
                string newFolderPath = Path.Combine(folderPath, folderName);
                Directory.CreateDirectory(newFolderPath);
                return "Folder created successfully.";
            }
            else
                return validFolder;
        }
        catch (Exception ex)
        {
            return "Error creating folder: " + ex.Message;
        }
    }

    [WebMethod]
    public static string AddorEditHTMLPlugin(string name, string context, string htmlText, string action)
    {
        string result = String.Empty;
        try
        {
            if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
            {
                return Constants.SESSIONTIMEOUT;
            }
            AnalyticsUtils _aUtils = new AnalyticsUtils();
            string apiUrl = _aUtils.ARM_URL + "/api/v1/AddorEditHTMLPlugin";

            var inputJson = new
            {
                ARMSessionId = _aUtils.ARMSessionId,
                AxSessionId = HttpContext.Current.Session.SessionID,
                Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
                AppName = HttpContext.Current.Session["project"].ToString(),
                Roles = HttpContext.Current.Session["AxRole"].ToString(),
                UserName = HttpContext.Current.Session["username"].ToString(),
                Name = name,
                Context = context,
                HTMLText = htmlText,
                Action = action
            };

            try
            {
                Util.Util _util = new Util.Util();
                FDW fdwObj = new FDW();
                FDR fObj = (FDR)HttpContext.Current.Session["FDR"];
                if (fObj == null)
                    fObj = new FDR();
                string fdData = Constants.REDISARMAXCARDSLIST;
                var dbVarKeys = fObj.GetWildCardKeyNames(_util.GetRedisServerkey(fdData, ""));
                fdwObj.DeleteKeys(dbVarKeys);
            }
            catch (Exception ex) { }

            result = _aUtils.CallWebAPI(apiUrl, "POST", "application/json", JsonConvert.SerializeObject(inputJson));
            return result;
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }

    [WebMethod]
    public static string GetHTMLPlugins()
    {
        string result = String.Empty;
        try
        {
            if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
            {
                return Constants.SESSIONTIMEOUT;
            }
            AnalyticsUtils _aUtils = new AnalyticsUtils();
            string apiUrl = _aUtils.ARM_URL + "/api/v1/GetHTMLPlugins";

            var inputJson = new
            {
                ARMSessionId = _aUtils.ARMSessionId,
                AxSessionId = HttpContext.Current.Session.SessionID,
                Trace = Convert.ToBoolean(HttpContext.Current.Session["AxTrace"].ToString() ?? "false"),
                AppName = HttpContext.Current.Session["project"].ToString(),
                Roles = HttpContext.Current.Session["AxRole"].ToString(),
                UserName = HttpContext.Current.Session["username"].ToString()
            };

            var plugins = _aUtils.CallWebAPI(apiUrl, "POST", "application/json", JsonConvert.SerializeObject(inputJson));
            return plugins;
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }


    [WebMethod]
    public static string UploadFile(string fileData, string customFileName, string targetFolder)
    {
        try
        {
            string applicationPath = HttpRuntime.AppDomainAppPath;
            string folderPath = Path.Combine(applicationPath, targetFolder);

            string validFolder = validateFolder(folderPath);

            if (validFolder != "Valid")
            {
                return validFolder;
            }

            if (string.IsNullOrWhiteSpace(fileData))
            {
                return "No file data was provided.";
            }

            // Decode the Base64 string to byte array
            byte[] fileBytes;
            try
            {
                fileBytes = Convert.FromBase64String(fileData);
            }
            catch
            {
                return "Invalid file data.";
            }

            // Determine the filename
            string fileExtension = string.Empty;
            if (!string.IsNullOrWhiteSpace(customFileName))
            {
                fileExtension = Path.GetExtension(customFileName).ToLower();
            }
            else
            {
                return "Invalid custom file name.";
            }

            // Validate file extension
            if (string.IsNullOrWhiteSpace(fileExtension) || !allowedExtensions.Contains(fileExtension))
            {
                return "Invalid or unsupported file extension.";
            }

            // Clean the filename
            string fileName = CleanFileName(customFileName);
            string finalFileFilePath = Path.Combine(folderPath, fileName);

            string validFile = validateFile(finalFileFilePath);
            if (validFile != "Valid")
            {
                return validFile;
            }

            if (!Directory.Exists(folderPath))
            {
                return "Target folder does not exist.";
            }

            try
            {
                // Save the file
                File.WriteAllBytes(finalFileFilePath, fileBytes);

                // Verify file was saved
                if (!File.Exists(finalFileFilePath))
                {
                    throw new Exception("File failed to save properly.");
                }

                // Return success response
                return "File uploaded successfully.";
            }
            catch (Exception ex)
            {
                // Clean up if file save failed
                if (File.Exists(finalFileFilePath))
                {
                    File.Delete(finalFileFilePath);
                }
                return "Error saving file: " + ex.Message;
            }
        }
        catch (Exception ex)
        {
            return "Error uploading file: " + ex.Message;
        }
    }

    private static string CleanFileName(string fileName)
    {
        // Remove invalid characters
        string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        foreach (char c in invalidChars)
        {
            fileName = fileName.Replace(c.ToString(), "");
        }

        // Replace spaces with underscores
        fileName = fileName.Replace(" ", "_");

        // Ensure filename isn't too long
        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
        string extension = Path.GetExtension(fileName);
        if (fileNameWithoutExt.Length > 50)
        {
            fileNameWithoutExt = fileNameWithoutExt.Substring(0, 50);
            fileName = fileNameWithoutExt + extension;
        }

        return fileName;
    }

    [WebMethod]
    public static string GetFilesAndFolders()
    {
        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        string applicationPath = HttpRuntime.AppDomainAppPath;
        string project = Convert.ToString(HttpContext.Current.Session["project"]);
        string[] folderPaths = {
        applicationPath + "\\CustomPages",
        applicationPath + "\\" + project
    };

        List<FileDetails> fileList = new List<FileDetails>();
        List<FolderDetails> folderList = new List<FolderDetails>();
        string[] skipFolders = { ".vs" }; // Mention folder names in lowercase;

        foreach (string folderPath in folderPaths)
        {
            if (Directory.Exists(folderPath))
            {
                DirectoryInfo parentDirInfo = new DirectoryInfo(folderPath);
                folderList.Add(new FolderDetails
                {
                    FolderName = parentDirInfo.FullName.Replace(applicationPath, ""),
                    FullPath = parentDirInfo.FullName.Replace(applicationPath, ""),
                    CreatedOn = parentDirInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    ModifiedOn = parentDirInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
                });

                // Add folders to folderList
                foreach (string subFolder in Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(subFolder);
                    if (Array.Exists(skipFolders, folder => subFolder.ToLower().Contains(folder)))
                    {
                        continue;
                    }

                    folderList.Add(new FolderDetails
                    {
                        FolderName = dirInfo.FullName.Replace(applicationPath, ""),
                        FullPath = dirInfo.FullName.Replace(applicationPath, ""),
                        CreatedOn = dirInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        ModifiedOn = dirInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
                    });
                }

                // Add files to fileList
                foreach (string file in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (Array.Exists(skipFolders, folder => fileInfo.DirectoryName.ToLower().Contains(folder)))
                    {
                        continue;
                    }

                    if (Array.Exists(allowedExtensions, ext => ext.Equals(fileInfo.Extension, StringComparison.OrdinalIgnoreCase)))
                    {
                        fileList.Add(new FileDetails
                        {
                            FolderName = fileInfo.Directory.FullName.Replace(applicationPath, ""),
                            FileName = fileInfo.Name,
                            FullFilePath = fileInfo.FullName.Replace(applicationPath, ""),
                            FileType = fileInfo.Extension,
                            FileSize = Math.Round(fileInfo.Length / 1024.0, 2),
                            CreatedOn = fileInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            ModifiedOn = fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
                        });
                    }
                }
            }
        }

        var result = new
        {
            files = fileList,
            folders = folderList
        };
        return JsonConvert.SerializeObject(result);
    }

    [WebMethod]
    public static string GetFileContent(string filePath)
    {
        string applicationPath = HttpRuntime.AppDomainAppPath;
        string fullFilePath = Path.Combine(applicationPath, filePath);

        string validFile = validateFile(fullFilePath);

        if (validFile == "Valid")
        {

            if (File.Exists(fullFilePath))
            {
                return File.ReadAllText(fullFilePath);
            }
            else
            {
                string project = HttpContext.Current.Session["project"].ToString();
                string tmpPath = filePath.ToLower().Replace(project.ToLower(), "app");
                string tempFilePath = "";
                switch (tmpPath.ToLower())
                {
                    case "custompages\\agilebizmainpage.html":
                    case "custompages\\app-mainpage.html":
                        tempFilePath = Path.Combine(applicationPath, "aspx\\mainhomeconfigtemplate.html");
                        break;
                    default:
                        break;
                }
                if (tempFilePath != "" && File.Exists(tempFilePath))
                {
                    return File.ReadAllText(tempFilePath);
                }
            }
            return "File not found.";
        }
        else
            return validFile;
    }

    [WebMethod]
    public static string DeleteFile(string filePath)
    {
        string applicationPath = HttpRuntime.AppDomainAppPath;
        string fullFilePath = Path.Combine(applicationPath, filePath);

        string validFile = validateFile(fullFilePath);

        if (validFile == "Valid")
        {

            if (File.Exists(fullFilePath))
            {
                string project = HttpContext.Current.Session["project"].ToString();
                string backupFilePath = Path.Combine(applicationPath, "UI-Plugin-Backups", project);
                Directory.CreateDirectory(backupFilePath);

                string fileName = Path.GetFileName(fullFilePath);
                fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + fileName;

                string targetPath = Path.Combine(backupFilePath, fileName);

                File.Move(fullFilePath, targetPath);

                return "File is deleted.";
            }
            else
            {
                return "File not found.";
            }

        }
        else
            return validFile;
    }


    [WebMethod]
    public static string SaveFileContent(string filePath, string content)
    {
        string applicationPath = HttpRuntime.AppDomainAppPath;
        string fullFilePath = Path.Combine(applicationPath, filePath);

        string validFile = validateFile(fullFilePath);

        if (validFile == "Valid")
        {
            try
            {
                string directory = Path.GetDirectoryName(fullFilePath);
                Directory.CreateDirectory(directory);
                File.WriteAllText(fullFilePath, content);
                return "File saved successfully.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        else
            return validFile;
    }

    [WebMethod]
    public static string CreateFile(string pageType, string fileName, string fileType, string content, string filePath)
    {
        string applicationPath = HttpRuntime.AppDomainAppPath;
        string fullFilePath = Path.Combine(applicationPath, filePath, fileName + fileType);

        string validFile = validateFile(fullFilePath);
        if (validFile == "Valid")
        {

            if (File.Exists(fullFilePath))
            {
                return "File already exists.";
            }

            try
            {
                string directory = Path.GetDirectoryName(fullFilePath);
                Directory.CreateDirectory(directory);
                File.WriteAllText(fullFilePath, content);
                return "File created successfully.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        else
            return validFile;
    }

    private static string validateFile(string fullFilePath)
    {

        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        string extension = Path.GetExtension(fullFilePath);
        string[] allowedExtensions = { ".js", ".css", ".html", ".json", ".png", ".svg", ".jpeg", ".jpg" };
        if (string.IsNullOrWhiteSpace(extension) || !extension.StartsWith(".") || !Array.Exists(allowedExtensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase)))
        {
            return "Invalid file type. Allowed types are: .js, .css, .html, .json";
        }

        string fileName = Path.GetFileName(fullFilePath);

        if (string.IsNullOrWhiteSpace(fileName))
        {
            return "File name cannot be empty.";
        }


        string applicationPath = HttpRuntime.AppDomainAppPath;
        string project = HttpContext.Current.Session["project"].ToString();
        string customPagesPath = Path.Combine(applicationPath, "CustomPages");
        string projectPath = Path.Combine(applicationPath, project);
        string backupPath = Path.Combine(applicationPath, "UI-Plugin-Backups" + "\\" + project);

        if (!fullFilePath.StartsWith(customPagesPath, StringComparison.OrdinalIgnoreCase) &&
            !fullFilePath.StartsWith(projectPath, StringComparison.OrdinalIgnoreCase) && !fullFilePath.StartsWith(backupPath, StringComparison.OrdinalIgnoreCase))
        {
            return "Access denied. Folder is not accessible.";
        }

        return "Valid";
    }
    private static string validateImage(string fullFilePath)
    {

        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }

        string extension = Path.GetExtension(fullFilePath);
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg" };
        if (string.IsNullOrWhiteSpace(extension) || !extension.StartsWith(".") || !Array.Exists(allowedExtensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase)))
        {
            return "Invalid file type. Allowed types are: .jpg, .jpeg, .png, .gif, .bmp, .svg";
        }

        string fileName = Path.GetFileName(fullFilePath);

        if (string.IsNullOrWhiteSpace(fileName))
        {
            return "File name cannot be empty.";
        }


        string applicationPath = HttpRuntime.AppDomainAppPath;
        string project = HttpContext.Current.Session["project"].ToString();
        string customPagesPath = Path.Combine(applicationPath, "CustomPages");
        string projectPath = Path.Combine(applicationPath, project);
        string backupPath = Path.Combine(applicationPath, "UI-Plugin-Backups" + "\\" + project);

        if (!fullFilePath.StartsWith(customPagesPath, StringComparison.OrdinalIgnoreCase) &&
            !fullFilePath.StartsWith(projectPath, StringComparison.OrdinalIgnoreCase) && !fullFilePath.StartsWith(backupPath, StringComparison.OrdinalIgnoreCase))
        {
            return "Access denied. Folder is not accessible.";
        }

        return "Valid";
    }


    private static string validateFolder(string fullFilePath)
    {

        if (HttpContext.Current.Session["project"] == null || Convert.ToString(HttpContext.Current.Session["project"]) == string.Empty)
        {
            return Constants.SESSIONTIMEOUT;
        }


        string applicationPath = HttpRuntime.AppDomainAppPath;
        string project = HttpContext.Current.Session["project"].ToString();
        string customPagesPath = Path.Combine(applicationPath, "CustomPages");
        string projectPath = Path.Combine(applicationPath, project);
        string backupPath = Path.Combine(applicationPath, "UI-Plugin-Backups" + "\\" + project);

        if (!fullFilePath.StartsWith(customPagesPath, StringComparison.OrdinalIgnoreCase) &&
            !fullFilePath.StartsWith(projectPath, StringComparison.OrdinalIgnoreCase) && !fullFilePath.StartsWith(backupPath, StringComparison.OrdinalIgnoreCase))
        {
            return "Access denied. Folder is not accessible.";
        }

        return "Valid";
    }


    [WebMethod]
    public static string GetImages(string filePath)
    {
        try
        {
            string applicationPath = HttpRuntime.AppDomainAppPath;
            string imageFolderPath = Path.Combine(applicationPath, filePath);

            string validFolder = validateFolder(imageFolderPath);
            if (validFolder == "Valid")
            {

                if (!Directory.Exists(imageFolderPath))
                {
                    Directory.CreateDirectory(imageFolderPath);
                }

                var imageFiles = Directory.GetFiles(imageFolderPath)
                        .Select(path => new
                        {
                            FileName = Path.GetFileName(path),
                            FullFilePath = path.Replace(applicationPath, ""),
                            FileSize = new FileInfo(path).Length,
                            LastModified = File.GetLastWriteTime(path)
                        })
                        .OrderBy(f => f.FileName)
                        .ToList();

                return JsonConvert.SerializeObject(imageFiles);
            }
            return validFolder;
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }

    [WebMethod]
    public static string UploadImage(string fileName, string filePath, byte[] fileData)
    {
        try
        {
            string applicationPath = HttpRuntime.AppDomainAppPath;
            string imageFilePath = Path.Combine(applicationPath, filePath, fileName);

            string validImage = validateImage(imageFilePath);
            if (validImage == "Valid")
            {

                if (fileData == null || fileData.Length == 0)
                {
                    return "File content is empty";
                }

                string directory = Path.GetDirectoryName(imageFilePath);
                Directory.CreateDirectory(directory);
                File.WriteAllBytes(imageFilePath, fileData);
                return "Image uploaded successfully.";
            }
            return validImage;
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }

    [WebMethod]
    public static string DeleteImage(string filePath)
    {
        try
        {
            string applicationPath = HttpRuntime.AppDomainAppPath;
            string imageFilePath = Path.Combine(applicationPath, filePath);

            string validImage = validateImage(imageFilePath);
            if (validImage == "Valid")
            {

                if (File.Exists(imageFilePath))
                {
                    File.Delete(imageFilePath);
                    return "Image deleted successfully.";
                }
                else
                {
                    return "File not found";
                }
            }
            return validImage;
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }

    public class FileDetails
    {
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string FullFilePath { get; set; }
        public string FileType { get; set; }
        public double FileSize { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
    }
    public class FolderDetails
    {
        public string FolderName { get; set; }
        public string FullPath { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
    }
}
