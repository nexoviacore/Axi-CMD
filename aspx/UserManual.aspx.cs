using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class aspx_UserManual : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    public string direction = "ltr";
    public string langType = "en";

    protected override void InitializeCulture()
    {
        if (Session["language"] != null)
        {
            util = new Util.Util();
            string dirLang = string.Empty;
            dirLang = util.SetCulture(Session["language"].ToString().ToUpper());
            if (!string.IsNullOrEmpty(dirLang))
            {
                direction = dirLang.Split('-')[0];
                langType = dirLang.Split('-')[1];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["Project"] == null || HttpContext.Current.Session["Project"].ToString() == string.Empty)
        {
            SessionExpired();
            return;
        }
        else
        {
            string folderPath = "";

            if (Session["UserManualPath"] != null)
                folderPath = Session["UserManualPath"].ToString();
            else
            {
                string filePath = "~/CustomPages/userManual";
                folderPath = Server.MapPath(filePath);
            }
            string destPath = HttpContext.Current.Application["scriptspath"].ToString() + "axpert\\" + HttpContext.Current.Session["nsessionid"];
            string scriptURL = HttpContext.Current.Application["scriptsurlpath"].ToString() + "axpert/" + HttpContext.Current.Session["nsessionid"];

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<ul id=\"fileList\">");
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    string fname = file.Replace(folderPath + "\\", "");
                    MoveFiles(folderPath, destPath, fname);
                    sb.Append("<li class=\"fileName\"><a href=\"javascript:void(0)\" id='' class='grdAttach handCur' onclick='ShowFiles(\"" + scriptURL + "/" + fname + "\")'>" + fname + "</a></li>");
                }
                sb.Append("</ul>");
                listOfFiles.Text = sb.ToString();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }


    private void MoveFiles(string sourcePath, string destPath, string fname)
    {
            DirectoryInfo source = new DirectoryInfo(sourcePath);
            DirectoryInfo destination = new DirectoryInfo(destPath);
        
            if (!destination.Exists)
                destination.Create();

            if (source.Exists)
            {
                System.IO.BinaryReader brReader = default(System.IO.BinaryReader);
                System.IO.BinaryWriter brWriter = default(System.IO.BinaryWriter);
                string strFile = sourcePath + "\\" + fname;
                string strDest = destPath + "\\" + fname;
                FileStream input = null;
                try
                {
                    input = new FileStream(strFile, FileMode.Open, FileAccess.Read);
                }
                catch (FileNotFoundException ex)
                {

                }
                if (input != null)
                {
                    FileStream output = new FileStream(strDest, FileMode.Create, FileAccess.Write);
                    brReader = new System.IO.BinaryReader(input);
                    brWriter = new System.IO.BinaryWriter(output);
                    int bufsize = 30000;
                    // this is buffer size
                    int readcount = 0;
                    int bsize = 0;

                    int indexer = 0;
                    FileInfo fileInfo = new FileInfo(strFile);

                    int FileLen = Convert.ToInt32(fileInfo.Length);
                    while ((readcount < FileLen))
                    {
                        if (bufsize < FileLen - readcount)
                        {
                            bsize = bufsize;
                        }
                        else
                        {
                            bsize = FileLen - readcount;
                        }
                        byte[] buffer = new byte[bsize];

                        brReader.Read(buffer, indexer, bsize);
                        brWriter.Write(buffer, indexer, bsize);

                        readcount = readcount + bsize;
                    }
                    brReader.Close();
                    brWriter.Close();
                    brReader.Dispose();
                    brWriter.Dispose();
                    output.Dispose();
                    input.Dispose();
                }
            }
    }

    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }
}
