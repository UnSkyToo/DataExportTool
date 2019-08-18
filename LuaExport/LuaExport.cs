using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LuaExport
{
    public class LuaExport : IExportPlugin.IExportPlugin
    {
        private string Path_ = string.Empty;
        private int KeyRow = 3;
        private int TypeRow = 4;
        private int DataStartRow = 10;

        private List<List<string>> Data_ = null;
        private StreamWriter OutStream_ = null;
        private int MainKeyColumn_ = 0;

        public LuaExport()
        {
        }

        public string GetDisplayName()
        {
            return "LuaExport";
        }

        public string GetExportExt()
        {
            return "lua";
        }

        public string GetVersion()
        {
            return "1.0.0.0";
        }

        public void SetPath(string Path)
        {
            Path_ = Path;
        }

        public string GetPath()
        {
            return Path_;
        }

        private void WriteSpace(int Level)
        {
            if (Level == 0)
            {
                return;
            }

            OutStream_.Write(string.Empty.PadLeft(Level * 4, ' '));
        }

        private void WriteValueStringTable(string Value, string Type, int SpaceLevel)
        {
            OutStream_.Write("{\n");
            WriteSpace(SpaceLevel);

            var Values = Value.Split(',');
            foreach (var Val in Values)
            {
                WriteValueString(Type, Val, SpaceLevel + 1);
                OutStream_.Write(", ");
            }

            OutStream_.Write('\n');
            WriteSpace(SpaceLevel - 1);
            OutStream_.Write("}");
        }

        private void WriteValueStringTable2(string Value, string Type, int SpaceLevel)
        {
            OutStream_.Write("{\n");
            WriteSpace(SpaceLevel);

            Value = Value.Replace(';', '|');
            var Values = Value.Split('|');
            foreach (var Val in Values)
            {
                WriteValueString(Type, Val, SpaceLevel + 1);
                OutStream_.Write(", ");
            }

            OutStream_.Write('\n');
            WriteSpace(SpaceLevel - 1);
            OutStream_.Write("}");
        }

        // 0:number
        // 1:string
        // 3:bool
        // 2:array
        // 4:doublearray
        private void WriteValueString(string Type, string Value, int SpaceLevel)
        {
            if (Value == "null")
            {
                OutStream_.Write("nil");
                return;
            }

            switch (Type)
            {
                case "0":
                    OutStream_.Write(Value);
                    break;
                case "1":
                    OutStream_.Write($"\"{Value}\"");
                    break;
                case "3":
                    OutStream_.Write(Value);
                    break;
                case "20":
                    WriteValueStringTable(Value, "0", SpaceLevel);
                    break;
                case "21":
                    WriteValueStringTable(Value, "1", SpaceLevel);
                    break;
                case "23":
                    WriteValueStringTable(Value, "3", SpaceLevel);
                    break;
                case "40":
                    WriteValueStringTable2(Value, "20", SpaceLevel);
                    break;
                case "41":
                    WriteValueStringTable2(Value, "21", SpaceLevel);
                    break;
                case "43":
                    WriteValueStringTable2(Value, "23", SpaceLevel);
                    break;
                default:
                    break;
            }
        }

        private void WriteValueString(int Row, int Column, int SpaceLevel)
        {
            var Type = Data_[TypeRow][Column];
            var Value = Data_[Row][Column];
            WriteValueString(Type, Value, SpaceLevel);
        }

        private void WriteTableValue(int Row, int Column, int SpaceLevel, bool HasKey)
        {
            WriteSpace(SpaceLevel);
            // Key
            OutStream_.Write($"[\"{Data_[KeyRow][Column]}\"] = ");
            WriteValueString(Row, Column, SpaceLevel + 1);
            OutStream_.Write(",\n");
        }

        private void WriteTableLine(int Row, int SpaceLevel)
        {
            WriteSpace(SpaceLevel);

            OutStream_.Write("[");
            WriteValueString(Row, MainKeyColumn_, SpaceLevel);
            OutStream_.Write("] = {\n");

            for (var Index = 0; Index < Data_[Row].Count; ++Index)
            {
                if (string.IsNullOrWhiteSpace(Data_[TypeRow][Index]))
                {
                    continue;
                }

                WriteTableValue(Row, Index, SpaceLevel + 1, true);
            }

            WriteSpace(SpaceLevel);
            OutStream_.Write("},\n");
        }

        public bool Export(IExportPlugin.ExportData Data)
        {
            try
            {
                var FullPath = $"{Data.OutputPath}{Data.Name}.{GetExportExt()}";
                OutStream_ = new StreamWriter(FullPath, false, new UTF8Encoding(false));
                Data_ = Data.Data;

                // 0 : descript
                // 3 : key
                // 4 : type
                // 10 - n : data

                var Find = false;
                for (var RowIndex = 0; RowIndex < 4 && !Find; ++RowIndex)
                {
                    for (var ColumnIndex = 0; ColumnIndex < Data_[RowIndex].Count && !Find; ++ColumnIndex)
                    {
                        if (Data_[RowIndex][ColumnIndex].StartsWith("!"))
                        {
                            KeyRow = RowIndex;
                            TypeRow = RowIndex + 1;
                            DataStartRow = RowIndex + 7;
                            MainKeyColumn_ = ColumnIndex;
                            Find = true;
                            break;
                        }
                    }
                }

                for (var Index = 0; Index < Data_[KeyRow].Count; ++Index)
                {
                    if (Data_[KeyRow][Index].StartsWith("!"))
                    {
                        MainKeyColumn_ = Index;
                        Data_[KeyRow][Index] = Data_[KeyRow][Index].Substring(1);
                        break;
                    }
                }

                OutStream_.Write($"local {Data.Name} = " + "{\n");

                for (var RowIndex = DataStartRow; RowIndex < Data.Data.Count; ++RowIndex)
                {
                    WriteTableLine(RowIndex, 1);
                }

                OutStream_.Write("}\n\n");
                OutStream_.Write($"return {Data.Name}");

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                OutStream_?.Close();
                OutStream_?.Dispose();
                OutStream_ = null;
            }
        }

        public string Check(IExportPlugin.ExportData Data)
        {
            try
            {
                // 0 : descript
                // 3 : key
                // 4 : type
                // 10 - n : data

                var Find = false;
                for (var RowIndex = 0; RowIndex < 4 && !Find; ++RowIndex)
                {
                    for (var ColumnIndex = 0; ColumnIndex < Data.Data[RowIndex].Count && !Find; ++ColumnIndex)
                    {
                        if (Data.Data[RowIndex][ColumnIndex].StartsWith("!"))
                        {
                            KeyRow = RowIndex;
                            TypeRow = RowIndex + 1;
                            DataStartRow = RowIndex + 7;
                            Find = true;
                            break;
                        }
                    }
                }

                // 类型检测
                for (var Index = 0; Index < Data.Data[KeyRow].Count; ++Index)
                {
                    var Type = Data.Data[TypeRow][Index];

                    if (string.IsNullOrWhiteSpace(Type))
                    {
                        continue;
                    }

                    // 0:number
                    // 1:string
                    // 3:bool
                    // 2:array
                    // 4:doublearray
                    if (Type != "0" && Type != "1" && Type != "3" && Type != "20" && Type != "21" && Type != "23" && Type != "40" && Type != "41" && Type != "43")
                    {
                        return $"{Data.Name}表 第{Index + 1}列不支持的类型{Type}";
                    }
                }

                // 空单元格检测
                for (var RowIndex = DataStartRow; RowIndex < Data.Data.Count; ++RowIndex)
                {
                    for (var ColumnIndex = 0; ColumnIndex < Data.Data[RowIndex].Count; ++ColumnIndex)
                    {
                        if (string.IsNullOrWhiteSpace(Data.Data[TypeRow][ColumnIndex]))
                        {
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(Data.Data[RowIndex][ColumnIndex]))
                        {
                            return $"{Data.Name}表 第{RowIndex + 1}行第{ColumnIndex + 1}列为空";
                        }

                        if (Data.Data[TypeRow][ColumnIndex] != "1" && Data.Data[RowIndex][ColumnIndex].Contains(" "))
                        {
                            return $"{Data.Name}表 第{RowIndex + 1}行第{ColumnIndex + 1}列包含空";
                        }

                        if (Data.Data[TypeRow][ColumnIndex].Contains("1"))
                        {
                            if (string.IsNullOrWhiteSpace(Data.Data[RowIndex][ColumnIndex]))
                            {
                                return $"{Data.Name}表 第{RowIndex + 1}行第{ColumnIndex + 1}列为空";
                            }
                        }
                        else if (Data.Data[TypeRow][ColumnIndex].Contains("0"))
                        {
                            if (Data.Data[RowIndex][ColumnIndex] == "null")
                            {
                                continue;
                            }

                            if (!double.TryParse(Data.Data[RowIndex][ColumnIndex], out double v))
                            {
                                return $"{Data.Name}表 第{RowIndex + 1}行第{ColumnIndex + 1}列不是数字";
                            }
                        }
                        else if (Data.Data[TypeRow][ColumnIndex].Contains("3"))
                        {
                            if (Data.Data[RowIndex][ColumnIndex] == "null")
                            {
                                continue;
                            }

                            if (!bool.TryParse(Data.Data[RowIndex][ColumnIndex], out bool v))
                            {
                                return $"{Data.Name}表 第{RowIndex + 1}行第{ColumnIndex + 1}列不是boolean型";
                            }
                        }
                    }
                }

                // key重复检测
                var Keys = new List<string>();
                for (var Index = DataStartRow; Index < Data.Data.Count; ++Index)
                {
                    var Key = Data.Data[Index][0];
                    var OldIndex = Keys.IndexOf(Key);

                    if (OldIndex != -1)
                    {
                        return $"{Data.Name}表 第{Index + 1}行与第{OldIndex + 1}行 Id重复 {Key}";
                    }

                    Keys.Add(Key);
                }

                return string.Empty;
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }
        }
    }
}
