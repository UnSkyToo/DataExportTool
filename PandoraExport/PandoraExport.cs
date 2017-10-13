using System;
using System.IO;
using System.Collections.Generic;

namespace PandoraExport
{
    public class PandoraExport : IExportPlugin.IExportPlugin
    {
        private string m_path = string.Empty;

        public PandoraExport()
        {
        }

        public string GetDisplayName()
        {
            return "PandoraExport";
        }

        public string GetExportExt()
        {
            return "pda";
        }

        public string GetVersion()
        {
            return "1.0.0.5";
        }

        public void SetPath(string path)
        {
            m_path = path;
        }

        public string GetPath()
        {
            return m_path;
        }

        public bool Export(IExportPlugin.ExportData data)
        {
            FileStream file = null;

            try
            {
                string fullPath = $"{data.OutputPath}.{GetExportExt()}";
                file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);

                // 0 : descript
                // 1 : key
                // 2 : type
                // 3 - n : data

                FileUtil.WriteInt8(file, (byte)data.Data[0].Count);

                foreach (string key in data.Data[1])
                {
                    FileUtil.WriteString2(file, key);
                }

                foreach (string type in data.Data[2])
                {
                    if (type == "int")
                    {
                        FileUtil.WriteInt8(file, 1);
                    }
                    else if (type == "short")
                    {
                        FileUtil.WriteInt8(file, 2);
                    }
                    else if (type == "byte")
                    {
                        FileUtil.WriteInt8(file, 3);
                    }
                    else if (type == "bool")
                    {
                        FileUtil.WriteInt8(file, 4);
                    }
                    else
                    {
                        FileUtil.WriteInt8(file, 5);
                    }
                }

                for (int n = 3; n < data.Data.Count; ++n)
                {
                    for (int i = 0; i < data.Data[n].Count; ++i)
                    {
                        if (data.Data[2][i] == "int")
                        {
                            FileUtil.WriteInt32(file, int.Parse(data.Data[n][i]));
                        }
                        else if (data.Data[2][i] == "short")
                        {
                            FileUtil.WriteInt16(file, short.Parse(data.Data[n][i]));
                        }
                        else if (data.Data[2][i] == "byte")
                        {
                            FileUtil.WriteInt8(file, byte.Parse(data.Data[n][i]));
                        }
                        else if (data.Data[2][i] == "bool")
                        {
                            FileUtil.WriteBool(file, bool.Parse(data.Data[n][i]));
                        }
                        else
                        {
                            FileUtil.WriteString2(file, data.Data[n][i]);
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                FileUtil.SafeRelease(file);
            }
        }

        public string Check(IExportPlugin.ExportData data)
        {
            try
            {
                // 类型检测
                for (int i = 0; i < data.Data[2].Count; ++i)
                {
                    string type = data.Data[2][i];

                    if (type != "string" && type != "int" && type != "short" && type != "byte" && type != "bool")
                    {
                         return $"{data.Name}表 第{i + 1}列不支持的类型{type}";
                    }
                }

                // 空单元格检测
                for (int n = 3; n < data.Data.Count; ++n)
                {
                    for (int i = 0; i < data.Data[n].Count; ++i)
                    {
                        if (string.IsNullOrWhiteSpace(data.Data[n][i]))
                        {
                            return $"{data.Name}表 第{n + 1}行第{i + 1}列为空";
                        }

                        if (data.Data[2][i] != "string" && data.Data[n][i].Contains(" "))
                        {
                            return $"{data.Name}表 第{n + 1}行第{i + 1}列包含空";
                        }

                        if (data.Data[2][i] == "string")
                        {
                            if (string.IsNullOrWhiteSpace(data.Data[n][i]))
                            {
                                return $"{data.Name}表 第{n + 1}行第{i + 1}列为空";
                            }
                        }
                        else if (data.Data[2][i] == "int")
                        {
                            int v = 0;
                            if (!int.TryParse(data.Data[n][i], out v))
                            {
                                return $"{data.Name}表 第{n + 1}行第{i + 1}列不是int型";
                            }
                        }
                        else if (data.Data[2][i] == "short")
                        {
                            short v = 0;
                            if (!short.TryParse(data.Data[n][i], out v))
                            {
                                return $"{data.Name}表 第{n + 1}行第{i + 1}列不是short型";
                            }
                        }
                        else if (data.Data[2][i] == "byte")
                        {
                            byte v = 0;
                            if (!byte.TryParse(data.Data[n][i], out v))
                            {
                                return $"{data.Name}表 第{n + 1}行第{i + 1}列不是byte型";
                            }
                        }
                        else if (data.Data[2][i] == "bool")
                        {
                            bool v = true;
                            if (!bool.TryParse(data.Data[n][i], out v))
                            {
                                return $"{data.Name}表 第{n + 1}行第{i + 1}列不是bool型";
                            }
                        }
                    }
                }
                
                // key重复检测
                List<string> keys = new List<string>();
                for (int n = 3; n < data.Data.Count; ++n)
                {
                    string key = data.Data[n][0];
                    int index = keys.IndexOf(key);

                    if (index != -1)
                    {
                        throw new Exception($"{data.Name}表 第{index + 4}行与第{n + 1}行 Id重复 {key}");
                    }

                    keys.Add(key);
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
