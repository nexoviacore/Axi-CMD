using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Caching;
using System.Configuration;


namespace LogFile
{
    /// <summary>
    /// Summary description for LogFile
    /// </summary>
    [Serializable]
    public class Log
    {
        string ScriptsPath = string.Empty;
        public string errorlog = "false";
        public string strLog = "Log\\";
        string filename = "";

        public Log()
        {
            //
            // TODO: Add constructor logic here
            //
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["AxTrace"] != null)
                errorlog = HttpContext.Current.Session["AxTrace"].ToString();
            if (HttpContext.Current.Application["ScriptsPath"] != null)
                ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        }

        public string CreateActionLog(string text, string sid, string fname, string newfile)
        {
            string filepath = "";
            //if (HttpContext.Current.Session != null && HttpContext.Current.Session["trace"] != null)
            //    errorlog = HttpContext.Current.Session["trace"].ToString();


            ////Create a Folder
            //string path = ScriptsPath + strLog + sid + "\\";
            //DirectoryInfo di = new DirectoryInfo(path);
            ////' Determine whether the directory exists.
            //if (di.Exists)
            //{

            //}
            //else
            //{
            //    // create the directory.
            //    di.Create();
            //}

            //filepath = path + fname + ".txt";

            //StreamWriter sw = default(StreamWriter);
            //System.IO.StreamReader sr = default(System.IO.StreamReader);
            //if (File.Exists(filepath))
            //{
            //    sr = System.IO.File.OpenText(filepath);
            //    string MyContents = sr.ReadToEnd();
            //    sr.Close();
            //    //If InStr(MyContents, text) = 0 Then
            //    if (newfile != "new")
            //    {
            //        //True for appending
            //        sw = new StreamWriter(filepath, true);
            //    }
            //    else
            //    {
            //        sw = new StreamWriter(filepath, false);
            //        sw.WriteLine("Start Time : " + DateTime.Now.ToString());
            //        sw.Flush();
            //        sw.Close();
            //        sw = new StreamWriter(filepath, true);
            //    }
            //    sw.WriteLine(text);
            //    sw.Flush();
            //    sw.Close();
            //}
            //else
            //{
            //    //End If
            //    sw = new StreamWriter(filepath, false);
            //    sw.WriteLine("Start Time : " + DateTime.Now.ToString());
            //    sw.Flush();
            //    sw.Close();
            //    //Pass the file path and the file name to the StreamWriter constructor.
            //    sw = new StreamWriter(filepath, true);
            //    //True for appending
            //    sw.WriteLine(text);
            //    //Close the file.
            //    sw.Flush();
            //    sw.Close();
            //}
            return filepath;
        }


        public string CreateLog(string text, string sid, string fname, string newfile)
        {
            string filepath = "";
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["AxTrace"] != null)
                errorlog = HttpContext.Current.Session["AxTrace"].ToString();

            if (errorlog.ToLower() != "true")
            {
                return filepath;
            }

            try
            {
                //Create a Folder
                string path = ScriptsPath + strLog + sid + "\\";
                DirectoryInfo di = new DirectoryInfo(path);
                //' Determine whether the directory exists.
                if (!di.Exists)
                {
                    // create the directory.
                    di.Create();
                }

                filepath = path + fname + ".txt";

                StreamWriter sw = default(StreamWriter);
                if (File.Exists(filepath))
                {
                    if (newfile != "new")
                    {
                        //True for appending
                        sw = new StreamWriter(filepath, true);
                    }
                    else
                    {
                        sw = new StreamWriter(filepath, false);
                        sw.WriteLine("Start Time : " + DateTime.Now.ToString());
                        sw.Flush();
                        sw.Close();
                        sw = new StreamWriter(filepath, true);
                    }
                    sw.WriteLine(text);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    //End If
                    sw = new StreamWriter(filepath, false);
                    sw.WriteLine("Start Time : " + DateTime.Now.ToString());
                    sw.Flush();
                    sw.Close();
                    //Pass the file path and the file name to the StreamWriter constructor.
                    sw = new StreamWriter(filepath, true);
                    //True for appending
                    sw.WriteLine(text);
                    //Close the file.
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                filepath = "";
            }
            return filepath;
        }

        public string CreateDirectDBLog(string fName, string fnName, string ex, string ip, string op)
        {
            string filePath = string.Empty;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string sid = string.Empty;
            string uName = string.Empty;
            DateTime cTime = new DateTime();

            if (HttpContext.Current.Session != null && HttpContext.Current.Session["AxTrace"] != null)
                errorlog = HttpContext.Current.Session["AxTrace"].ToString();
            if (errorlog.ToLower() != "true")
                return filePath;
            else
            {
                if (HttpContext.Current.Session["nsessionid"] != null)
                    sid = HttpContext.Current.Session["nsessionid"].ToString();
                if (HttpContext.Current.Session["user"] != null)
                    uName = HttpContext.Current.Session["user"].ToString();
                cTime = DateTime.Now;

                string path = ScriptsPath + strLog + sid + "\\";
                DirectoryInfo di = new DirectoryInfo(path);
                //' Determine whether the directory exists.
                if (!di.Exists)
                    di.Create();

                filePath = path + fName + ".txt";

                //Starting File Creation
                sb.Append(fName + Environment.NewLine);
                sb.Append("---------------------------------------------------" + Environment.NewLine);
                sb.Append("Calling Function - " + fnName + Environment.NewLine);
                sb.Append("Session ID - " + sid + Environment.NewLine);
                sb.Append("Current User - " + uName + Environment.NewLine);
                sb.Append("Inputs - " + ip + Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("Output - " + op + Environment.NewLine);
                if (ex != string.Empty)
                {
                    sb.Append("Exception Occurred in -" + Environment.NewLine);
                    sb.Append(ex + Environment.NewLine);
                }
                sb.Append("Created Time - " + cTime + Environment.NewLine);
                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    sw.WriteLine(sb);
                    sw.Flush();
                }

            }
            return filePath;
        }

        public string CreateLog(string text, string sid, string fname, string newfile, string trace)
        {
            string filepath = "";

            if (trace.ToLower() != "true")
            {
                return filepath;
            }

            try
            {
                //Create a Folder
                string path = ScriptsPath + strLog + sid + "\\";
                DirectoryInfo di = new DirectoryInfo(path);
                //' Determine whether the directory exists.
                if (!di.Exists)
                {
                    // create the directory.
                    di.Create();
                }

                filepath = path + fname + ".txt";

                StreamWriter sw = default(StreamWriter);
                if (File.Exists(filepath))
                {
                    if (newfile != "new")
                    {
                        //True for appending
                        sw = new StreamWriter(filepath, true);
                    }
                    else
                    {
                        sw = new StreamWriter(filepath, false);
                        sw.WriteLine("Start Time : " + DateTime.Now.ToString());
                        sw.Flush();
                        sw.Close();
                        sw = new StreamWriter(filepath, true);
                    }
                    sw.WriteLine(text);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    //End If
                    sw = new StreamWriter(filepath, false);
                    sw.WriteLine("Start Time : " + DateTime.Now.ToString());
                    sw.Flush();
                    sw.Close();
                    //Pass the file path and the file name to the StreamWriter constructor.
                    sw = new StreamWriter(filepath, true);
                    //True for appending
                    sw.WriteLine(text);
                    //Close the file.
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                filepath = "";
            }
            return filepath;
        }

        public void DeleteLog(string sid)
        {
            DirectoryInfo di = new DirectoryInfo(ScriptsPath + strLog + sid);
            //' Determine whether the directory exists.
            if (di.Exists)
            {
                di.Delete(true);
            }
        }

        public string GetFileNames(string sid)
        {
            DirectoryInfo di = new DirectoryInfo(ScriptsPath + strLog + sid);

            if (di.Exists)
            {
                FileInfo[] files = di.GetFiles("*.txt");

                DateTime[] creationTimes = new DateTime[files.Length];
                for (int j = 0; j < files.Length; j++)
                {
                    creationTimes[j] = new FileInfo(files[j].FullName).CreationTime;
                }
                Array.Sort(creationTimes, files);

                foreach (System.IO.FileInfo fi in files)
                {
                    if (string.IsNullOrEmpty(filename))
                    {
                        filename += fi.Name;
                    }
                    else
                    {
                        filename += "," + fi.Name;
                    }
                }
            }

            return filename;
        }
    }

    [Serializable]
    public class ErrorLog
    {
        string ScriptsPath = string.Empty;
        public ErrorLog()
        {
            if (HttpContext.Current.Application["ScriptsPath"] != null)
                ScriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
        }

        public void LogException(Exception exc, string source, string appdetails)
        {
            try
            {
                string path = ScriptsPath + "Log\\ApplicationError" + "\\";
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                {
                    di.Create();
                }

                string logFile = path + "ApplicationError.txt";
                StreamWriter sw = default(StreamWriter);
                if (!File.Exists(logFile))
                {
                    sw = File.CreateText(logFile);
                }
                else
                    sw = File.AppendText(logFile);

                sw.WriteLine("********** {0} **********", DateTime.Now);
                if (exc.InnerException != null)
                {
                    sw.Write("Inner Exception Type: ");
                    sw.WriteLine(exc.InnerException.GetType().ToString());
                    sw.Write("Inner Exception: ");
                    sw.WriteLine(exc.InnerException.Message);
                    sw.Write("Inner Source: ");
                    sw.WriteLine(exc.InnerException.Source);
                    if (exc.InnerException.StackTrace != null)
                    {
                        sw.WriteLine("Inner Stack Trace: ");
                        sw.WriteLine(exc.InnerException.StackTrace);
                    }
                }
                sw.Write("Exception Type: ");
                sw.WriteLine(exc.GetType().ToString());
                sw.WriteLine("Exception: " + exc.Message);
                sw.WriteLine("Source: " + source);
                sw.WriteLine("Stack Trace: ");
                if (exc.StackTrace != null)
                {
                    sw.WriteLine(exc.StackTrace);
                    sw.WriteLine();
                }
                sw.WriteLine(appdetails);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
