using System;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;

/// <summary>
/// Summary description for AppConfiguration
/// </summary>
namespace AppConfiguration
{
    public class AppConfiguration
    {
        public void SaveConfigFile(string proj, string configStr)
        {
            try
            {
                if (proj == "") return;
                string path = string.Empty;
                path = HttpContext.Current.Server.MapPath("~/Config");

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                path = path + "\\" + proj + ".cfg";
                if (!File.Exists(path))
                    System.IO.File.Create(path).Close();
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(path, false))
                {
                    writer.WriteLine(configStr);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string SaveDefaultConfigFile(string proj)
        {
            SaveConfigFile(proj, Constants.DEFAULT_CONFIGSTR);
            return Constants.DEFAULT_CONFIGSTR;
        }

        public string ValidateAppConfiguration(string configStr, ref bool resetConfig)
        {

            JArray configNode = null;
            JObject objConfig = JObject.Parse(configStr);
            configNode = (JArray)objConfig["configStr"];

            JArray defaultConfigNode = null;
            JObject objDefaultConfig = JObject.Parse(Constants.DEFAULT_CONFIGSTR);
            defaultConfigNode = (JArray)objDefaultConfig["configStr"];

            StringBuilder str = new StringBuilder();
            str.Append("{\"configStr\": ");

            if (configNode != null && defaultConfigNode != null)
            {
                str.Append(AppendNewConfigKeys(configNode.ToString(), defaultConfigNode, ref resetConfig));
            }

            configNode = (JArray)objConfig["configLangKeys"];
            defaultConfigNode = (JArray)objDefaultConfig["configLangKeys"];
            str.Append(",\"configLangKeys\": ");

            if (configNode != null && defaultConfigNode != null)
            {
                str.Append(AppendNewConfigKeys(configNode.ToString(), defaultConfigNode, ref resetConfig));
            }

            str.Append("}");
            JObject.Parse(str.ToString());
            configStr = str.ToString();
            str.Clear();

            objConfig = JObject.Parse(configStr);
            configNode = (JArray)objConfig["configStr"];
            defaultConfigNode = (JArray)objDefaultConfig["configStr"];
            str.Append("{\"configStr\": ");

            if (configNode != null && defaultConfigNode != null)
            {
                str.Append(ConstructAppConfigString(defaultConfigNode.ToString(), configNode, ref resetConfig, true));
            }

            configNode = (JArray)objConfig["configLangKeys"];
            defaultConfigNode = (JArray)objDefaultConfig["configLangKeys"];
            str.Append(",\"configLangKeys\": ");

            if (configNode != null && defaultConfigNode != null)
            {
                str.Append(ConstructAppConfigString(defaultConfigNode.ToString(), configNode, ref resetConfig, false));
            }

            str.Append("}");
            JObject.Parse(str.ToString());

            return str.ToString();
        }

        public string AppendNewConfigKeys(string configStr, JArray defaultConfigNode, ref bool resetConfig)
        {
            StringBuilder configNewStr = new StringBuilder();
            configNewStr.Append(configStr.Substring(0, configStr.LastIndexOf("]")));
            foreach (JObject content in defaultConfigNode.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    if (!configStr.ToUpper().Contains(prop.Name.ToUpper()))
                    {
                        configNewStr.Append(",{\"" + prop.Name + "\": \"" + prop.Value.ToString() + "\"}");
                        resetConfig = true;
                    }
                }
            }
            configNewStr.Append("]");            
            return configNewStr.ToString();
        }

        public string ConstructAppConfigString(string defaultConfigStr, JArray configNode, ref bool resetConfig, bool containsCheck)
        {
            StringBuilder configNewStr = new StringBuilder();
            configNewStr.Append("[");            
            foreach (JObject content in configNode.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    //'containsCheck' is only needed for 'configStr' Keys and not for 'configLangKeys' keys.
                    //We have to add the 'configStr' keys only if it is available in DEFAULT_CONFIGSTR
                    //We have to add all 'configLangKeys', since keys for different languages will be available.
                    if (containsCheck) {
                        if (defaultConfigStr.ToUpper().Contains("\"" + prop.Name.ToUpper() + "\""))
                        {
                            configNewStr.Append("{\"" + prop.Name + "\": \"" + prop.Value.ToString() + "\"},");
                        }
                        else
                        {
                            resetConfig = true;
                        }
                    }
                    else
                        configNewStr.Append("{\"" + prop.Name + "\": \"" + prop.Value.ToString() + "\"},");
                }
            }
            return configNewStr.ToString().TrimEnd(',') + "]";
        }
    }
}