using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;

namespace DataExportTool.Helper
{
    public static class ExcelAceHelper
    {
        /// <summary>
        /// 打开Excel
        /// </summary>
        /// <param name="ExcelFilePath">Excel文件路径</param>
        /// <param name="SheetName">Sheet表格名字</param>
        /// <returns>返回Excel类型</returns>
        public static DataTable OpenExcel(string ExcelFilePath, ref string SheetName, bool IsXlsx)
        {
            try
            {
                //源的定义，第一行在导入长数字时，会被转为科学计数法
                //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
                var StrConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + ExcelFilePath + ";" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
                if (IsXlsx)
                {
                    StrConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + ExcelFilePath + ";" + "Extended Properties='Excel 12.0;HDR=NO;IMEX=1';";
                }

                var DbCon = new OleDbConnection(StrConn);
                DbCon.Open();

                if (SheetName == null)
                {
                    // 获取excel sheet list
                    var Tables = DbCon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (Tables != null)
                    {
                        //var ExcelSheets = new string[Tables.Rows.Count];
                        // 加入工作表名称到字符串数组 
                        foreach (DataRow Row in Tables.Rows)
                        {
                            var StrSheetTableName = Row["TABLE_NAME"].ToString();
                            //过滤无效SheetName
                            if (StrSheetTableName.Contains("$") && StrSheetTableName.Replace("'", "").EndsWith("$"))
                            {
                                SheetName = StrSheetTableName.Substring(0, StrSheetTableName.Length - 1);
                                break;
                            }
                        }
                    }

                    if (SheetName == null)
                    {
                        Logger.Error("确保excel里的sheet名字正确，这将作为文件的导出名字");
                        return null;
                    }
                }

                var StrExcelSql = $"select * from [{SheetName}$]";
                //string strExcel = "select * from   [sheet1$]";

                var Ds = new DataSet();
                var Adapter = new OleDbDataAdapter(StrExcelSql, StrConn);
                Adapter.Fill(Ds, SheetName);

                DbCon.Close();

                return Ds.Tables[SheetName];
            }
            catch (Exception Ex)
            {
                Logger.Warning(Ex.Message);
                return null;
            }
        }

        public static List<string> GetExcelSheets(string ExcelFilePath, bool IsXlsx)
        {
            try
            {
                //源的定义，第一行在导入长数字时，会被转为科学计数法
                //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
                var StrConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + ExcelFilePath + ";" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
                if (IsXlsx)
                {
                    StrConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + ExcelFilePath + ";" + "Extended Properties='Excel 12.0;HDR=NO;IMEX=1';";
                }

                var DbCon = new OleDbConnection(StrConn);
                DbCon.Open();

                var Sheets = new List<string>();

                // 获取excel sheet list
                var Tables = DbCon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (Tables != null)
                {
                    foreach (DataRow Row in Tables.Rows)
                    {
                        var StrSheetTableName = Row["TABLE_NAME"].ToString();
                        //过滤无效SheetName
                        if (StrSheetTableName.Contains("$") && StrSheetTableName.Replace("'", "").EndsWith("$"))
                        {
                            Sheets.Add(StrSheetTableName.Substring(0, StrSheetTableName.Length - 1));
                        }
                    }
                }

                DbCon.Close();
                return Sheets;
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
        public static int GetExcelColumnCount(DataTable Table)
        {
            try
            {
                if (Table != null)
                {
                    return Table.Columns.Count;
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
        public static int GetExcelRowCount(DataTable Table)
        {
            try
            {
                if (Table != null)
                {
                    return Table.Rows.Count;
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
        public static string GetExcelData(DataTable Table, int Line, int Index)
        {
            try
            {
                if (Table != null)
                {
                    return Table.Rows[Line][Index].ToString();
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
        public static string GetExcelData(DataTable Table, int Line, string Key)
        {
            try
            {
                if (Table != null)
                {
                    for (var Index = 0; Index < Table.Columns.Count; ++Index)
                    {
                        if (Table.Rows[0][Index].ToString() == Key)
                        {
                            return Table.Rows[Line][Index].ToString();
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
        public static List<string> GetExcelDataLine(DataTable Table, int Line)
        {
            try
            {
                if (Table != null)
                {
                    var Result = new List<string>();

                    for (var Index = 0; Index < Table.Columns.Count; ++Index)
                    {
                        Result.Add(Table.Rows[Line][Index].ToString());
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
        public static List<string> GetExcelDataLine(DataTable Table, string Key)
        {
            try
            {
                if (Table != null)
                {
                    var Result = new List<string>();

                    for (var CIndex = 0; CIndex < Table.Columns.Count; ++CIndex)
                    {
                        if (Table.Rows[0][CIndex].ToString() == Key)
                        {
                            for (var RIndex = 0; RIndex < Table.Rows.Count; ++RIndex)
                            {
                                Result.Add(Table.Rows[RIndex][CIndex].ToString());
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