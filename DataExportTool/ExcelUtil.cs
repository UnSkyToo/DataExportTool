using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace DataExportTool
{
    public static class ExcelUtil
    {
        // 读取Excel数据表中的指定Sheet到DataTable
        public static DataTable ExcelToDataTable(string strExcelFileName, string strSheetName)
        {
            try
            {
                //源的定义，第一行在导入长数字时，会被转为科学计数法
                //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
                string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";

                //Sql语句
                string strExcel = $"select * from [{strSheetName}$]"; //这是一种方法
                //string strExcel = "select * from   [sheet1$]";

                //定义存放的数据表
                DataSet ds = new DataSet();

                //连接数据源
                OleDbConnection conn = new OleDbConnection(strConn);

                conn.Open();

                //适配到数据源
                OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, strConn);
                adapter.Fill(ds, strSheetName);

                conn.Close();

                return ds.Tables[strSheetName];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // 返回Excel的列数
        public static int GetExcelColumn(DataTable table)
        {
            try
            {
                if (table != null)
                {
                    return table.Columns.Count;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // 返回Excel的行数
        public static int GetExcelRow(DataTable table)
        {
            try
            {
                if (table != null)
                {
                    return table.Rows.Count;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // 返回Excel中指定的行和列的数据
        public static string GetExcelData(DataTable table, int line, int index)
        {
            try
            {
                if (table != null)
                {
                    return table.Rows[line][index].ToString();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        // 返回Excel中指定行的指定项数据
        public static string GetExcelData(DataTable table, int line, string key)
        {
            try
            {
                if (table != null)
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        if (table.Rows[0][i].ToString() == key)
                        {
                            return table.Rows[line][i].ToString();
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        // 返回Excel中指定行的所有数据项
        public static List<string> GetExcelDataLine(DataTable table, int line)
        {
            try
            {
                if (table != null)
                {
                    List<string> result = new List<string>();

                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        result.Add(table.Rows[line][i].ToString());
                    }

                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        // 返回Excel中指定列的所有数据项
        public static List<string> GetExcelDataLine(DataTable table, string key)
        {
            try
            {
                if (table != null)
                {
                    List<string> result = new List<string>();

                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        if (table.Rows[0][i].ToString() == key)
                        {
                            for (int n = 0; n < table.Rows.Count; n++)
                            {
                                result.Add(table.Rows[n][i].ToString());
                            }

                            return result;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
    }
}
