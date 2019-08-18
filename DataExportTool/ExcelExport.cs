using System;

namespace DataExportTool
{
    public static class ExcelExport
    {
        public static bool UseOfficeApi { get; set; } = false;

        public static bool Export(string PluginName, string ExcelPath, string SheetName, string OutputPath)
        {
            if (UseOfficeApi)
            {
                return ExcelOfficeExport.Export(PluginName, ExcelPath, SheetName, OutputPath);
            }
            else
            {
                return ExcelAceExport.Export(PluginName, ExcelPath, SheetName, OutputPath);
            }
        }

        public static int ExportDirectory(string PluginName, string[] ExcelPaths, bool ExportAllSheet, string OutputPath, Action<int, int> OnProgress)
        {
            if (UseOfficeApi)
            {
                return ExcelOfficeExport.ExportDirectory(PluginName, ExcelPaths, ExportAllSheet, OutputPath, OnProgress);
            }
            else
            {
                return ExcelAceExport.ExportDirectory(PluginName, ExcelPaths, ExportAllSheet, OutputPath, OnProgress);
            }
        }
    }
}