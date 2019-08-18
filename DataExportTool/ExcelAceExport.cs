using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using DataExportTool.Helper;

namespace DataExportTool
{
    public class ExcelAceExport
    {
        public static bool Export(string PluginName, string ExcelPath, string SheetName, string OutputPath)
        {
            DataTable Table = null;
            //Microsoft.Office.Interop.Excel.Worksheet Table = null;

            try
            {
                var Plugin = PluginManager.GetExportPluginWithName(PluginName);
                if (Plugin == null)
                {
                    throw new Exception($"{PluginName}格式导出插件创建失败");
                }

                Table = ExcelAceHelper.OpenExcel(ExcelPath, ref SheetName, true);
                if (Table == null)
                {
                    throw new Exception($"{ExcelPath}表{SheetName}页读取错误");
                }

                var RowCount = ExcelAceHelper.GetExcelRowCount(Table); // 表的行数

                var Data = new IExportPlugin.ExportData
                {
                    Name = SheetName,
                    InputPath = ExcelPath,
                    OutputPath = OutputPath,
                    Data = new List<List<string>>()
                };

                for (var Index = 0; Index < RowCount; ++Index)
                {
                    var Line = ExcelAceHelper.GetExcelDataLine(Table, Index);
                    Data.Data.Add(Line);
                }

                var CheckCode = Plugin.Check(Data);
                if (!string.IsNullOrWhiteSpace(CheckCode))
                {
                    throw new Exception(CheckCode);
                }
                
                return Plugin.Export(Data);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex.Message);
                return false;
            }
            finally
            {
                Table?.Dispose();
            }
        }

        public static bool ExportAllSheets(string PluginName, string ExcelPath, string OutputPath)
        {
            var Sheets = ExcelAceHelper.GetExcelSheets(ExcelPath, true);
            if (Sheets == null)
            {
                Logger.Warning($"不能读取Excel的Sheets : {ExcelPath}");
                return false;
            }

            Logger.Info($"Export File : {ExcelPath}");
            foreach (var Sheet in Sheets)
            {
                if (Sheet.StartsWith("#"))
                {
                    continue;
                }
                Logger.Info($"  Sheet : {Sheet}");
                Export(PluginName, ExcelPath, Sheet, OutputPath);
            }

            return true;
        }

        public static int ExportDirectory(string PluginName, string[] ExcelPaths, bool ExportAllSheet, string OutputPath, Action<int, int> OnProgress)
        {
            if (string.IsNullOrWhiteSpace(PluginName))
            {
                Logger.Warning($"错误的导出插件格式 : {PluginName}");
                return 0;
            }

            var ExportCount = 0;
            var UnExportCount = 0;
            try
            {
                PathHelper.CreateDirectory(OutputPath);

                foreach (var ExcelPath in ExcelPaths)
                {
                    if (ExportAllSheet && ExportAllSheets(PluginName, ExcelPath, OutputPath))
                    {
                        CacheHelper.SetExcelMD5(ExcelPath, PathHelper.FileMD5(ExcelPath));
                        ExportCount++;
                    }
                    else if (Export(PluginName, ExcelPath, null, OutputPath))
                    {
                        CacheHelper.SetExcelMD5(ExcelPath, PathHelper.FileMD5(ExcelPath));
                        ExportCount++;
                    }
                    else
                    {
                        UnExportCount++;
                    }

                    OnProgress?.Invoke(ExportCount + UnExportCount, ExcelPaths.Length);
                }

                return ExportCount;
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex.Message);
                OnProgress?.Invoke(ExcelPaths.Length, ExcelPaths.Length);
                return ExportCount;
            }
        }
    }
}