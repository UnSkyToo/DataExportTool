using System;
using System.Collections.Generic;
using DataExportTool.Helper;
using Excel = Microsoft.Office.Interop.Excel;

namespace DataExportTool
{
    public static class ExcelOfficeExport
    {
        public static bool Export(string PluginName, string ExcelPath, string SheetName, string OutputPath)
        {
            Excel.Worksheet Table = null;

            try
            {
                var Plugin = PluginManager.GetExportPluginWithName(PluginName);
                if (Plugin == null)
                {
                    throw new Exception($"{PluginName}格式导出插件创建失败");
                }

                Table = ExcelOfficeHelper.OpenExcel(ExcelPath, ref SheetName, true);
                if (Table == null)
                {
                    throw new Exception($"{ExcelPath}表{SheetName}页读取错误");
                }

                var RowCount = ExcelOfficeHelper.GetExcelRowCount(Table); // 表的行数

                var Data = new IExportPlugin.ExportData
                {
                    Name = SheetName,
                    InputPath = ExcelPath,
                    OutputPath = OutputPath,
                    Data = new List<List<string>>()
                };

                for (var Index = 0; Index < RowCount; ++Index)
                {
                    var Line = ExcelOfficeHelper.GetExcelDataLine(Table, Index);
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
            }
        }

        public static int ExportDirectory(string PluginName, string[] ExcelPaths, bool ExportAllSheet, string OutputPath, Action<int, int> OnProgress)
        {
            if (string.IsNullOrWhiteSpace(PluginName))
            {
                Logger.Warning($"错误的导出插件格式 : {PluginName}");
                return 0;
            }

            var ExportCount = 0;
            try
            {
                PathHelper.CreateDirectory(OutputPath);

                ExcelOfficeHelper.OpenExcelApp();
                foreach (var ExcelPath in ExcelPaths)
                {
                    var Name = PathHelper.GetFileNameWithoutExt(ExcelPath);
                    if (Export(PluginName, ExcelPath, null, OutputPath))
                    {
                        CacheHelper.SetExcelMD5(ExcelPath, PathHelper.FileMD5(ExcelPath));
                        ExportCount++;
                        OnProgress?.Invoke(ExportCount, ExcelPaths.Length);
                    }
                }

                return ExportCount;
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex.Message);
                OnProgress?.Invoke(ExcelPaths.Length, ExcelPaths.Length);
                return ExportCount;
            }
            finally
            {
                ExcelOfficeHelper.CloseExcelApp();
            }
        }
    }
}
