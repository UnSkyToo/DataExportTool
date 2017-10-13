using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace DataExportTool
{
    public static class ConfigUtil
    {
        private static string m_configPath = System.Windows.Forms.Application.StartupPath + "\\Config.dat";
        private static Dictionary<string, string> m_config = new Dictionary<string, string>();

        private static string GetValue(string key)
        {
            if (m_config.ContainsKey(key))
            {
                return m_config[key];
            }
            return string.Empty;
        }

        private static void SetValue(string key, string value)
        {
            if (m_config.ContainsKey(key))
            {
                m_config[key] = value;
            }
            else
            {
                m_config.Add(key, value);
            }
        }

        public static void SaveConfig()
        {
            StreamWriter fp = null;

            try
            {
                fp = new StreamWriter(m_configPath, false, Encoding.Default);

                foreach (KeyValuePair<string, string> pair in m_config)
                {
                    fp.WriteLine("{0},{1}", pair.Key, pair.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FileUtil.SafeRelease(fp);
            }
        }

        public static void LoadConfig()
        {
            StreamReader fp = null;

            try
            {
                if (!File.Exists(m_configPath))
                {
                    return;
                }

                m_config.Clear();

                fp = new StreamReader(m_configPath, Encoding.Default);

                while (!fp.EndOfStream)
                {
                    string line = fp.ReadLine();
                    string[] keyValue = line.Split(',');

                    if (keyValue.Length != 2)
                    {
                        throw new Exception("Config data format error!");
                    }

                    m_config.Add(keyValue[0], keyValue[1]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FileUtil.SafeRelease(fp);
            }
        }

        public static string GetWorkPath()
        {
            return GetValue("WorkPath");
        }

        public static void SetWorkPath(string path)
        {
            SetValue("WorkPath", path);
        }

        public static string GetExcelMD5(string excelName)
        {
            return GetValue(excelName);
        }

        public static void SetExcelMD5(string excelName, string md5)
        {
            SetValue(excelName, md5);
        }

        public static bool GetAutoOpenExport()
        {
            return GetValue("AutoOpen") == "true";
        }

        public static void SetAutoOpenExport(bool enabled)
        {
            SetValue("AutoOpen", enabled == true ? "true" : "false");
        }

        public static List<string> GetExcludeExt()
        {
            string excludeExt = GetValue("ExcludeExt");

            if (excludeExt.Length == 0)
            {
                return null;
            }

            string[] result = excludeExt.Split(new char[] { ',' });
            
            return new List<string>(result);
        }

        public static void SetExcludeExt(List<string> excludeExt)
        {
            StringBuilder result = new StringBuilder();

            if (excludeExt == null)
            {
                return;
            }

            foreach (string v in excludeExt)
            {
                result.Append(v);
                result.Append(",");
            }

            SetValue("ExcludeExt", result.ToString());
        }

        public static string GetExportPlugin()
        {
            return GetValue("ExportPlugin");
        }

        public static void SetExportPlugin(string exportPlugin)
        {
            if (exportPlugin.Length == 0)
            {
                return;
            }

            SetValue("ExportPlugin", exportPlugin);
        }
    }
}
