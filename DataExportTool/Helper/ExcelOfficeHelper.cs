using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace DataExportTool.Helper
{
    public static class ExcelOfficeHelper
    {
        public static Excel.Application ExcelApp { get; private set; } = null;
        public static Excel.Workbook ExcelWorkbook { get; private set; } = null;

        private static string CurrentExcelFilePath_ = string.Empty;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public static bool OpenExcelApp()
        {
            if (ExcelApp != null)
            {
                return true;
            }

            try
            {
                ExcelApp = new Excel.Application();
                ExcelApp.Visible = true;
                ExcelApp.DisplayAlerts = false;
                return true;
            }
            catch(Exception Ex)
            {
                Logger.Error(Ex.Message);
                return false;
            }
        }

        public static void CloseExcelApp()
        {
            CloseExcelWorkbook();

            if (ExcelApp == null)
            {
                return;
            }

            /*var LocalByNameApp = Process.GetProcessesByName("EXCEL");
            if (LocalByNameApp.Length > 0)
            {
                foreach (var App in LocalByNameApp)
                {
                    if (!App.HasExited)
                    {
                        App.Kill();
                    }
                }
            }*/
            GetWindowThreadProcessId(new IntPtr(ExcelApp.Hwnd), out int ProcessId);
            Process.GetProcessById(ProcessId).Kill();

            ExcelApp.Workbooks.Close();
            Marshal.ReleaseComObject(ExcelApp);
            ExcelApp.Quit();
            ExcelApp = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static bool OpenExcelWorkbook(string ExcelFilePath)
        {
            if (ExcelApp == null || ExcelWorkbook != null)
            {
                return true;
            }

            try
            {
                object Miss = Missing.Value;
                CurrentExcelFilePath_ = ExcelFilePath;
                ExcelWorkbook = ExcelApp.Workbooks.Open(ExcelFilePath, Miss, true, Miss, Miss, Miss, Miss, Miss, Miss,
                    true, Miss, Miss, Miss, Miss, Miss);
                return true;
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex.Message);
                return false;
            }
        }

        public static void CloseExcelWorkbook()
        {
            if (ExcelApp == null || ExcelWorkbook == null)
            {
                return;
            }

            ExcelWorkbook.Close(Missing.Value, Missing.Value, Missing.Value);
            Marshal.ReleaseComObject(ExcelWorkbook);
            ExcelWorkbook = null;
        }

        public static List<string> GetCurrentExcelSheets()
        {
            if (ExcelWorkbook == null)
            {
                return null;
            }

            var Sheets = new List<string>();
            foreach (Excel.Worksheet Sheet in ExcelWorkbook.Sheets)
            {
                Sheets.Add(Sheet.Name);
            }
            return Sheets;
        }

        /// <summary>
        /// 打开Excel
        /// </summary>
        /// <param name="ExcelFilePath">Excel文件路径</param>
        /// <param name="SheetName">Sheet表格名字</param>
        /// <returns>返回Excel类型</returns>
        public static Excel.Worksheet OpenExcel(string ExcelFilePath, ref string SheetName, bool IsXlsx)
        {
            try
            {
                if (ExcelApp == null)
                {
                    if (!OpenExcelApp())
                    {
                        return null;
                    }
                }

                if (CurrentExcelFilePath_ != ExcelFilePath)
                {
                    CloseExcelWorkbook();

                    if (!OpenExcelWorkbook(ExcelFilePath))
                    {
                        return null;
                    }
                }

                Excel.Worksheet Sheet = null;

                if (SheetName == null)
                {
                    Sheet = (Excel.Worksheet)ExcelWorkbook.Sheets[1];
                }
                else
                {
                    Sheet = (Excel.Worksheet)ExcelWorkbook.Sheets[SheetName];
                }

                SheetName = Sheet.Name;
                return Sheet;
            }
            catch (Exception Ex)
            {
                Logger.Warning(Ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 返回Excel的列数
        /// </summary>
        /// <param name="Table">数据表</param>
        /// <returns>列数</returns>
        public static int GetExcelColumnCount(Excel.Worksheet Table)
        {
            try
            {
                if (Table != null)
                {
                    return Table.UsedRange.Columns.Count;
                }
                return 0;
            }
            catch (Exception Ex)
            {
                Logger.Warning(Ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 返回Excel的行数
        /// </summary>
        /// <param name="Table">数据表</param>
        /// <returns>行数</returns>
        public static int GetExcelRowCount(Excel.Worksheet Table)
        {
            try
            {
                if (Table != null)
                {
                    return Table.UsedRange.Rows.Count;
                }
                return 0;
            }
            catch (Exception Ex)
            {
                Logger.Warning(Ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 返回Excel中指定的行和列的数据
        /// </summary>
        /// <param name="Table">数据表</param>
        /// <param name="Line">行</param>
        /// <param name="Index">列</param>
        /// <returns>单元格内容</returns>
        public static string GetExcelData(Excel.Worksheet Table, int Line, int Index)
        {
            try
            {
                if (Table != null)
                {
                    return Table.UsedRange.Rows[Line][Index].ToString();
                }
                return string.Empty;
            }
            catch (Exception Ex)
            {
                Logger.Warning(Ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// 返回Excel中指定行的指定项数据
        /// </summary>
        /// <param name="Table">数据表</param>
        /// <param name="Line">行</param>
        /// <param name="Key">键</param>
        /// <returns>单元格内容</returns>
        public static string GetExcelData(Excel.Worksheet Table, int Line, string Key)
        {
            try
            {
                if (Table != null)
                {
                    for (var Index = 0; Index < Table.UsedRange.Columns.Count; ++Index)
                    {
                        if (Table.UsedRange.Rows[0][Index].ToString() == Key)
                        {
                            return Table.UsedRange.Rows[Line][Index].ToString();
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception Ex)
            {
                Logger.Warning(Ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// 返回Excel中指定行的所有数据项
        /// </summary>
        /// <param name="Table">数据表</param>
        /// <param name="Line">行</param>
        /// <returns>一行单元格的内容</returns>
        public static List<string> GetExcelDataLine(Excel.Worksheet Table, int Line)
        {
            try
            {
                if (Table != null)
                {
                    var Result = new List<string>();

                    for (var Index = 0; Index < Table.UsedRange.Columns.Count; ++Index)
                    {
                        Result.Add(Table.UsedRange.Rows[Line][Index].ToString());
                    }

                    return Result;
                }
                return null;
            }
            catch (Exception Ex)
            {
                Logger.Warning(Ex.Message);
                return null;

            }
        }

        /// <summary>
        /// 返回Excel中指定列的所有数据项
        /// </summary>
        /// <param name="Table">数据表</param>
        /// <param name="Key">键值</param>
        /// <returns>一列单元格的内容</returns>
        public static List<string> GetExcelDataLine(Excel.Worksheet Table, string Key)
        {
            try
            {
                if (Table != null)
                {
                    var Result = new List<string>();

                    for (var CIndex = 0; CIndex < Table.UsedRange.Columns.Count; ++CIndex)
                    {
                        if (Table.UsedRange.Rows[0][CIndex].ToString() == Key)
                        {
                            for (var RIndex = 0; RIndex < Table.UsedRange.Rows.Count; ++RIndex)
                            {
                                Result.Add(Table.UsedRange.Rows[RIndex][CIndex].ToString());
                            }

                            return Result;
                        }
                    }
                }
                return null;
            }
            catch (Exception Ex)
            {
                Logger.Warning(Ex.Message);
                return null;
            }
        }
    }
}