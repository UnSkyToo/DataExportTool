using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace DataExportTool.Helper
{
    public static class CacheHelper
    {
        private static readonly string ConfigPath_ = $"{AppDomain.CurrentDomain.BaseDirectory}cache.dat";
        private static readonly Dictionary<string, string> Caches_ = new Dictionary<string, string>();

        private static string GetValue(string Key)
        {
            if (Caches_.ContainsKey(Key))
            {
                return Caches_[Key];
            }
            return string.Empty;
        }

        private static void SetValue(string Key, string Value)
        {
            if (Caches_.ContainsKey(Key))
            {
                Caches_[Key] = Value;
            }
            else
            {
                Caches_.Add(Key, Value);
            }
        }

        public static void SaveCache()
        {
            StreamWriter OutStream = null;

            try
            {
                OutStream = new StreamWriter(ConfigPath_, false, Encoding.UTF8);

                foreach (var Ce in Caches_)
                {
                    OutStream.WriteLine("{0},{1}", Ce.Key, Ce.Value);
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex.Message);
            }
            finally
            {
                FileHelper.SafeRelease(OutStream);
            }
        }

        public static void LoadCache()
        {
            StreamReader InStream = null;

            try
            {
                if (!File.Exists(ConfigPath_))
                {
                    return;
                }

                Caches_.Clear();
                InStream = new StreamReader(ConfigPath_, Encoding.UTF8);

                while (!InStream.EndOfStream)
                {
                    var Line = InStream.ReadLine();
                    var Index = Line.IndexOf(',');

                    if (Index == -1)
                    {
                        throw new Exception("cache.dat file format error!");
                    }

                    var Key = Line.Substring(0, Index);
                    var Value = Line.Substring(Index + 1);
                    Caches_.Add(Key, Value);
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex.Message);
            }
            finally
            {
                FileHelper.SafeRelease(InStream);
            }
        }

        public static string GetWorkPath()
        {
            return GetValue("WorkPath");
        }

        public static void SetWorkPath(string Path)
        {
            SetValue("WorkPath", Path);
        }

        public static List<string> GetWorkPathList()
        {
            var PathList = new List<string>();
            var Line = GetValue("WorkPathList");
            if (string.IsNullOrWhiteSpace(Line))
            {
                return PathList;
            }

            var Path = Line.Split('|');
            for (var Index = 0; Index < Path.Length; ++Index)
            {
                if (!string.IsNullOrWhiteSpace(Path[Index]))
                {
                    PathList.Add(Path[Index]);
                }
            }
            return PathList;
        }

        public static void SetWorkPathList(List<string> Path)
        {
            if (Path == null || Path.Count == 0)
            {
                return;
            }

            var Line = new StringBuilder();
            for (var Index = 0; Index < Path.Count; ++Index)
            {
                Line.Append(Path[Index]);

                if (Index < Path.Count - 1)
                {
                    Line.Append('|');
                }
            }

            SetValue("WorkPathList", Line.ToString());
        }

        public static string GetExcelMD5(string ExcelPath)
        {
            return GetValue(ExcelPath);
        }

        public static void SetExcelMD5(string ExcelPath, string Md5)
        {
            SetValue(ExcelPath, Md5);
        }

        public static bool GetAutoOpenExport()
        {
            return GetValue("AutoOpen") == "true";
        }

        public static void SetAutoOpenExport(bool Enabled)
        {
            SetValue("AutoOpen", Enabled ? "true" : "false");
        }

        public static string GetExportPlugin()
        {
            return GetValue("ExportPlugin");
        }

        public static void SetExportPlugin(string ExportPlugin)
        {
            if (string.IsNullOrWhiteSpace(ExportPlugin))
            {
                return;
            }

            SetValue("ExportPlugin", ExportPlugin);
        }

        public static List<bool> GetExtSupport()
        {
            var Result = new List<bool>();

            try
            {
                var Line = GetValue("ExtSupport");
                if (string.IsNullOrWhiteSpace(Line))
                {
                    Result.Add(true);
                    Result.Add(true);
                    Result.Add(true);
                }
                else
                {
                    var Val = Line.Split('|');
                    Result.Add(bool.Parse(Val[0]));
                    Result.Add(bool.Parse(Val[1]));
                    Result.Add(bool.Parse(Val[2]));
                }
            }
            catch
            {
                Result.Clear();
                Result.Add(true);
                Result.Add(true);
                Result.Add(true);
            }

            return Result;
        }

        public static void SetExtSupport(bool Xlsx, bool Xls, bool Xlsb)
        {
            var Line = $"{Xlsx}|{Xls}|{Xlsb}";
            SetValue("ExtSupport", Line);
        }

        public static bool GetExportAllSheet()
        {
            var Line = GetValue("ExportAllSheet");
            if (string.IsNullOrWhiteSpace(Line))
            {
                return false;
            }

            return bool.Parse(Line);
        }

        public static void SetExportAllSheet(bool Value)
        {
            SetValue("ExportAllSheet", Value.ToString());
        }

        public static int GetExcelSortType()
        {
            var Line = GetValue("ExcelSortType");
            if (string.IsNullOrWhiteSpace(Line))
            {
                return 0;
            }

            if (int.TryParse(Line, out var Value))
            {
                return Value;
            }

            return 0;
        }

        public static void SetExcelSortType(int Value)
        {
            SetValue("ExcelSortType", Value.ToString());
        }
    }
}