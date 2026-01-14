using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Configuration;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

public partial class aspx_AxManager : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
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
        if (!IsPostBack)
        {
            if (Session.Contents.Count > 0)
                ClearApplicationCache();

            string projs = util.CheckForAvailableProjects();
            projs = Regex.Replace(projs, @"[^0-9a-zA-Z,]+", "");
            axAvailableProj.Value = projs.Replace("Please Select,", "");
        }
    }
    public void ClearApplicationCache()
    {
        Session.Contents.Clear();
        Session.Abandon();
        List<string> keys = new List<string>();
        IDictionaryEnumerator enumerator = Cache.GetEnumerator();
        while (enumerator.MoveNext())
        {
            keys.Add(enumerator.Key.ToString());
        }
        for (int i = 0; i < keys.Count; i++)
        {
            Cache.Remove(keys[i]);
        }
        sessionstatus.Visible = true;
        sessionstatus.Text = "Your session has been cleared";
    }

    protected void Messagebox(int MsgCode, string Message, string Param)
    {
        if (Message != "")
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "OraclInfo", "<script type='text/javascript'> showAlertDialog('info','" + Message + "'); </script>", false);
        else
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "OraclInfo", "<script type='text/javascript'> showAlertDialog('info'," + MsgCode + ",'client','" + Param + "'); </script>", false);
    }

    public void CheckOracleClient()
    {
        try
        {
            string[] Keynames;
            string oracle_home;// oracle_home_name, oracle_version;

            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey Orcl = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\ORACLE", false);
            RegistryKey SubKeyBufor;

            if (Orcl == null)
            {
                Messagebox(4002, "", "http://www.oracle.com/downloads");
            }
            else
            {
                Keynames = Orcl.GetSubKeyNames();
                for (int i = 0; i < Keynames.Length; i++)
                {
                    if (Keynames[i].Contains("KEY") || Keynames[i].StartsWith("HOME"))
                    {
                        SubKeyBufor = HKLM.OpenSubKey(@"SOFTWARE\\ORACLE\\" + Keynames[i], false);
                        string[] vnames;

                        vnames = SubKeyBufor.GetValueNames();

                        for (int j = 0; j < vnames.Length; j++)
                        {
                            if (vnames[j].Equals("ORACLE_HOME"))
                            {
                                oracle_home = SubKeyBufor.GetValue(vnames[j]).ToString();
                                if (oracle_home == null)
                                {
                                    Messagebox(4003, "", oracle_home);
                                }
                                else
                                {
                                    string fileName = "oci.dll";
                                    oracle_home += "\\bin";
                                    string targetPath = scriptsPath;
                                    string sourceFile = System.IO.Path.Combine(oracle_home, fileName);
                                    string destFile = System.IO.Path.Combine(targetPath, fileName);
                                    System.IO.File.Copy(sourceFile, destFile, true);
                                    break;
                                }

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Messagebox(0, ex.Message.Replace("'", ""), "");
        }
    }

    protected void btnConnect_Click(object sender, EventArgs e)
    {
        string loginPath = util.LOGINPATH;
        if (CreateAxApps(false))
        {
            if (util.CheckForAvailableProjects().Replace("Please Select,", "").IndexOf(',') == -1)
                CheckOracleClient();

            Session["project"] = projName.Text;
            Response.Redirect(loginPath);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string loginPath = util.LOGINPATH;
        if (CreateAxApps(true))
        {
            if (util.CheckForAvailableProjects().Replace("Please Select,", "").IndexOf(',') == -1)
                CheckOracleClient();
            Session["project"] = projName.Text;
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "savereload", "<script type='text/javascript'>showAlertDialog('success',4011,'client');window.location.href=window.location.href;</script>", false);
        }
    }

    protected void autofillProjDetails_Click(object sender, EventArgs e)
    {
        string proj = projName.Text;
        util.GetAxApps(proj);
        string axApps = Session["axApps"].ToString();
        if (!string.IsNullOrEmpty(axApps))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(axApps);
            XmlNode rootNode = default(XmlNode);
            rootNode = xmlDoc.SelectSingleNode(proj);
            projName.Text = proj;
            foreach (XmlElement elem in xmlDoc.SelectSingleNode(proj).ChildNodes)
            {
                if (elem.Name == "db")
                {
                    ListItem ls = new ListItem(elem.InnerText);
                    int notSelected = dBase.Items.IndexOf(ls) == 1 ? 0 : 1;
                    dBase.Items[notSelected].Selected = false;
                    dBase.Items.FindByText(elem.InnerText).Selected = true;
                }
                else if (elem.Name == "dbcon")
                    clientConName.Text = Regex.Replace(elem.InnerText, @"[^0-9a-zA-Z]+", ""); // Blind SQL Injection
                else if (elem.Name == "dbuser")
                    usrName.Text = elem.InnerText;
            }
        }
    }

    protected void CnfButton_Click(object sender, EventArgs e)
    {
        XDocument doc;
        string loginPath = util.LOGINPATH;
        FileStream file = new FileStream(scriptsPath + "\\axapps.xml", FileMode.Open, FileAccess.ReadWrite);
        using (StreamReader reader = new StreamReader(file))
        {
            doc = XDocument.Load(reader);
            doc.Element("connections").Element(projName.Text).Element("dbcon").Value = clientConName.Text;
            doc.Element("connections").Element(projName.Text).Element("dbuser").Value = usrName.Text;
            doc.Element("connections").Element(projName.Text).Element("db").Value = dBase.SelectedItem.Text;
            file.Close();
            doc.Save(scriptsPath + "\\axapps.xml");
            if (ViewState["SaveClicked"] != null && ViewState["SaveClicked"].ToString() == "True")
            {
                Messagebox(4004, "", projName.Text);
                ViewState["SaveClicked"] = null;
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "savereload", "<script type='text/javascript'>window.location.href=window.location.href; </script>", false);
                return;
            }
            Session["project"] = projName.Text;
            Response.Redirect(loginPath);
        }
    }

    public bool CreateAxApps(bool isSave)
    {
        string projectName = string.Empty;
        string db = string.Empty;
        string clientConNam = string.Empty;
        string uName = string.Empty;
        projectName = projName.Text;
        if (string.IsNullOrEmpty(projectName))
        {
            projName.Focus();
            Messagebox(4005, "", "");
            return false;
        }
        db = dBase.SelectedItem.Text;

        clientConNam = Regex.Replace(clientConName.Text, @"[^0-9a-zA-Z]+", "");
        if (string.IsNullOrEmpty(clientConNam))
        {
            clientConName.Focus();
            Messagebox(4006, "", "");
            return false;
        }
        uName = usrName.Text;
        if (string.IsNullOrEmpty(clientConNam))
        {
            usrName.Focus();
            Messagebox(4007, "", "");
            return false;
        }
        try
        {
            FileInfo fi = new FileInfo(scriptsPath + "\\axapps.xml");
            if (!fi.Exists)
            {
                using (XmlWriter writer = XmlWriter.Create(scriptsPath + "\\axapps.xml"))
                {
                    writer.WriteStartElement("connections");
                    writer.WriteStartElement(projectName);
                    writer.WriteElementString("type", "db");
                    writer.WriteElementString("structurl", "");
                    writer.WriteElementString("db", db);
                    if (db.ToLower() != "ms sql")
                        writer.WriteElementString("driver", "dbx");
                    else
                        writer.WriteElementString("driver", "ado");
                    writer.WriteElementString("dbcon", clientConNam);
                    writer.WriteElementString("dbuser", uName);
                    writer.WriteElementString("pwd", "");
                    writer.WriteEndElement();
                    writer.Close();
                }
            }
            else
            {
                XDocument document;
                FileStream file = new FileStream(scriptsPath + "\\axapps.xml", FileMode.Open, FileAccess.ReadWrite);
                using (StreamReader reader = new StreamReader(file))
                {
                    document = XDocument.Load(reader);
                    reader.Close();
                    if (document.Element("connections").Element(projectName) != null && document.Element("connections").Element(projectName).Value.Contains("db" + db + "dbx" + clientConNam + uName))
                    {
                        if (isSave)
                        {
                            Messagebox(4008, "", projectName);
                            return false;
                        }
                        return true;
                    }
                    else if (document.Element("connections").Element(projectName) != null && (document.Element("connections").Element(projectName).Element("dbcon").Value != clientConNam || document.Element("connections").Element(projectName).Element("dbuser").Value != uName || document.Element("connections").Element(projectName).Element("db").Value != db))
                    {
                        ViewState["SaveClicked"] = isSave;
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "OraclInfoModify", "<script type='text/javascript'> ConfirmSave(); </script>", false);
                        return false;
                    }
                    else if (document.Element("connections").Element(projectName) != null)
                    {
                        if (isSave)
                        {
                            Messagebox(4008, "", projectName);
                            return false;
                        }
                    }
                    XElement root = new XElement(projectName);
                    root.Add(new XElement("type", "db"));
                    root.Add(new XElement("structurl", ""));
                    root.Add(new XElement("db", db));
                    if (db.ToLower() != "ms sql")
                        root.Add(new XElement("driver", "dbx"));
                    else
                        root.Add(new XElement("driver", "ado"));
                    root.Add(new XElement("dbcon", clientConNam));
                    root.Add(new XElement("dbuser", uName));
                    root.Add(new XElement("pwd", ""));
                    document.Element("connections").Add(root);
                    file.Close();
                    document.Save(scriptsPath + "\\axapps.xml");
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Messagebox(0, ex.Message.Replace("'", ""), "");
            return false;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        XDocument doc;
        FileStream file = new FileStream(scriptsPath + "\\axapps.xml", FileMode.Open, FileAccess.ReadWrite);
        using (StreamReader reader = new StreamReader(file))
        {
            doc = XDocument.Load(reader);
            reader.Close();
            if (doc != null)
            {
                if (doc.Element("connections").Element(projName.Text) != null)
                {
                    doc.Element("connections").Element(projName.Text).Remove();
                    file.Close();
                    doc.Save(scriptsPath + "\\axapps.xml");
                    Messagebox(4009, "", "");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "hideload", "<script type='text/javascript'>window.location.href=window.location.href; </script>", false);
                }
                else
                {
                    Messagebox(4010, "", projName.Text);
                }
            }
            else
            {
                Messagebox(4010, "", projName.Text);
            }
        }
    }
}
