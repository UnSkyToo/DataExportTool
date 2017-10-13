using System;
using System.IO;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

namespace DataExportTool
{
    public class ExcelExport
    {
        public static bool Export(string pluginName, string excelPath, string sheetName, string outputPath)
        {
            DataTable table = null;

            try
            {
                var plugin = PluginManager.Instance().GetExportPluginWithName(pluginName);

                if (plugin == null)
                {
                    throw new Exception("格式导出插件创建失败");
                }

                table = ExcelUtil.ExcelToDataTable(excelPath, sheetName);

                if (table == null)
                {
                    throw new Exception(excelPath + "表" + sheetName + "页读取错误");
                }
                
                //int column = ExcelUtil.GetExcelColumn(table); // 表的列数
                int row = ExcelUtil.GetExcelRow(table); // 表的行数
                
                IExportPlugin.ExportData data = new IExportPlugin.ExportData();
                data.Name = sheetName;
                data.InputPath = excelPath;
                data.OutputPath = outputPath;
                data.Data = new List<List<string>>();

                for (int i = 0; i < row; i++)
                {
                    List<string> line = ExcelUtil.GetExcelDataLine(table, i);
                    data.Data.Add(line);
                }

                string checkCode = plugin.Check(data);

                if (checkCode.Length != 0)
                {
                    throw new Exception(checkCode);
                }
                
                return plugin.Export(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (table != null)
                {
                    table.Dispose();
                }
            }
        }
    }
}
