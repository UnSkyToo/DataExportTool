using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DataExportTool.Helper;

namespace DataExportTool
{
    public partial class MainForm
    {
        public string Version = "1.0.0";

        private string ExportPlugin_ = string.Empty;
        private readonly List<string> ExcelFiles_ = new List<string>();
        private List<string> WorkPathList_ = new List<string>();
        private bool NotifySelectItemEvent_ = true;
        private bool NotifySelectAllItemEvent_ = true;
        private bool NotifyWorkPathItemEvent_ = true;
        private bool NotifyExtSupportChangeEvent_ = true;

        private FileSystemWatcher Watcher_ = null;

        private void Startup()
        {
            try
            {
                Logger.Rich = RichTextBoxLogger;
                CacheHelper.LoadCache();

                this.Text = $"DataExportTool v{Version}";

                CheckedListBoxExcels.CheckOnClick = true;
                CheckBoxAutoOpen.Checked = CacheHelper.GetAutoOpenExport();
                ExportPlugin_ = CacheHelper.GetExportPlugin();

                var ExtSupport = CacheHelper.GetExtSupport();
                NotifyExtSupportChangeEvent_ = false;
                CheckBoxExtXlsx.Checked = ExtSupport[0];
                CheckBoxExtXls.Checked = ExtSupport[1];
                CheckBoxExtXlsm.Checked = ExtSupport[2];
                NotifyExtSupportChangeEvent_ = true;

                var WorkPath = CacheHelper.GetWorkPath();
                if (WorkPath != string.Empty)
                {
                    LoadExcels(WorkPath);
                }

                WorkPathList_ = CacheHelper.GetWorkPathList();
                UpdateWorkPathList();

                CheckBoxExportAllSheet.Checked = CacheHelper.GetExportAllSheet();

                PluginManager.LoadExportPlugins($"{AppDomain.CurrentDomain.BaseDirectory}ExportPlugin");
                CreatePluginMenu();

                ComboBoxSortType.Items.Clear();
                ComboBoxSortType.Items.Add("文件名");
                ComboBoxSortType.Items.Add("修改时间");
                ComboBoxSortType.SelectedIndex = CacheHelper.GetExcelSortType();

                //StartMonitorExcelPath();

                CheckUpdate();
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex.Message);
            }
        }

        private void Shutdown()
        {
            //StopMonitorExcelPath();

            CacheHelper.SetAutoOpenExport(CheckBoxAutoOpen.Checked);
            CacheHelper.SetExportPlugin(ExportPlugin_);
            CacheHelper.SetWorkPathList(WorkPathList_);
            CacheHelper.SetExportAllSheet(CheckBoxExportAllSheet.Checked);
            CacheHelper.SetExcelSortType(ComboBoxSortType.SelectedIndex);

            CacheHelper.SaveCache();
        }

        private void RefreshExcels()
        {
            var WorkPath = CacheHelper.GetWorkPath();

            if (string.IsNullOrWhiteSpace(WorkPath) || !PathHelper.DirectoryExists(WorkPath))
            {
                var Dialog = new FolderBrowserDialog();

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadExcels(Dialog.SelectedPath);
                }
            }
            else
            {
                LoadExcels(WorkPath);
            }
        }

        private bool ExcelFilter(string FilePath)
        {
            var FilterFlag = false;
            if (!string.IsNullOrWhiteSpace(TextBoxFilter.Text))
            {
                var FileName = PathHelper.GetFileNameWithoutExt(FilePath);
                FilterFlag = FileName.Contains(TextBoxFilter.Text);
                var PyCode = SpellHelper.GetSpellCode(FileName);
                FilterFlag |= PyCode.Contains(TextBoxFilter.Text.ToUpper());
            }
            else
            {
                FilterFlag = true;
            }

            return IsSupportExt(PathHelper.GetFileExt(FilePath)) && FilterFlag;
        }

        private List<string> SortExcelFileList(List<string> FileList)
        {
            if (ComboBoxSortType.SelectedIndex == 1)
            {
                FileList.Sort((A, B) =>
                {
                    var ModifyTimeA = PathHelper.GetFileModifyTimeTicks(A);
                    var ModifyTimeB = PathHelper.GetFileModifyTimeTicks(B);
                    if (ModifyTimeA == ModifyTimeB)
                    {
                        return 0;
                    }

                    if (ModifyTimeA > ModifyTimeB)
                    {
                        return -1;
                    }

                    return 1;
                });
            }

            return FileList;
        }

        private void LoadExcels(string WorkPath, Func<string, bool> Filter = null)
        {
            WorkPath = PathHelper.UnifyPath(WorkPath);
            if (Filter == null)
            {
                Filter = ExcelFilter;
            }

            try
            {
                CheckedListBoxExcels.Items.Clear();
                CheckBoxSelectAll.Checked = false;

                CacheHelper.SetWorkPath(WorkPath);
                UpdateWorkPathList();
                ExcelFiles_.Clear();

                var Excels = PathHelper.GetFileList(WorkPath, Filter);
                Excels = SortExcelFileList(Excels);

                var SelectCount = 0;

                foreach (var ExcelPath in Excels)
                {
                    ExcelFiles_.Add(ExcelPath);
                    var FileName = PathHelper.GetFileNameWithoutExt(ExcelPath);

                    var NewMd5 = PathHelper.FileMD5(ExcelPath);
                    var OldMd5 = CacheHelper.GetExcelMD5(ExcelPath);

                    if (NewMd5 == OldMd5)
                    {
                        CheckedListBoxExcels.Items.Add(FileName, CheckState.Unchecked);
                    }
                    else
                    {
                        CheckedListBoxExcels.Items.Add(FileName, CheckState.Checked);
                        SelectCount++;
                    }
                }

                LabelExcelTotalCount.Text = $"数量:{ExcelFiles_.Count}";
                LabelExcelSelectCount.Text = $"选中:{SelectCount}";
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex.Message);
            }
        }

        private void UpdateCheckedListBoxExcels(int ExcelIndex, bool IsChecked)
        {
            if (!NotifySelectAllItemEvent_)
            {
                return;
            }

            try
            {
                NotifySelectAllItemEvent_ = false;
                var Count = 0;
                for (var Index = 0; Index < CheckedListBoxExcels.Items.Count; ++Index)
                {
                    if (ExcelIndex == Index) // ItemCheck事件在响应的时候，Item的值还未改变，所以用传入的参数判断
                    {
                        if (IsChecked)
                        {
                            Count++;
                        }
                    }
                    else if (CheckedListBoxExcels.GetItemChecked(Index))
                    {
                        Count++;
                    }
                }

                if (Count == 0)
                {
                    CheckBoxSelectAll.CheckState = CheckState.Unchecked;
                }
                else if (Count == CheckedListBoxExcels.Items.Count)
                {
                    CheckBoxSelectAll.CheckState = CheckState.Checked;
                }
                else
                {
                    CheckBoxSelectAll.CheckState = CheckState.Indeterminate;
                }

                LabelExcelSelectCount.Text = $"选中:{Count}";
            }
            catch (Exception Ex)
            {
                Logger.Warning(Ex.Message);
            }
            finally
            {
                NotifySelectAllItemEvent_ = true;
            }
        }

        private void UpdateWorkPathList()
        {
            var WorkPath = CacheHelper.GetWorkPath();
            if (!WorkPathList_.Contains(WorkPath))
            {
                WorkPathList_.Add(WorkPath);
            }

            NotifyWorkPathItemEvent_ = false;
            ComboBoxWorkPath.Items.Clear();
            foreach (var Path in WorkPathList_)
            {
                ComboBoxWorkPath.Items.Add(Path);
                if (WorkPath == Path)
                {
                    ComboBoxWorkPath.SelectedIndex = ComboBoxWorkPath.Items.Count - 1;
                }
            }
            NotifyWorkPathItemEvent_ = true;
        }

        private void SelectExcels(bool Selected)
        {
            if (!NotifySelectItemEvent_)
            {
                return;
            }

            try
            {
                NotifySelectItemEvent_ = false;
                for (var Index = 0; Index < CheckedListBoxExcels.Items.Count; ++Index)
                {
                    CheckedListBoxExcels.SetItemChecked(Index, Selected);
                }

                if (Selected)
                {
                    LabelExcelSelectCount.Text = $"选中:{CheckedListBoxExcels.Items.Count}";
                }
                else
                {
                    LabelExcelSelectCount.Text = "选中:0";
                }
            }
            catch (Exception Ex)
            {
                Logger.Warning(Ex.Message);
            }
            finally
            {
                NotifySelectItemEvent_ = true;
            }
        }

        private void WorkPathListChanged()
        {
            if (!NotifyWorkPathItemEvent_)
            {
                return;
            }

            try
            {
                NotifyWorkPathItemEvent_ = false;

                var CurrentPath = (string)(ComboBoxWorkPath.SelectedItem);
                if (string.IsNullOrWhiteSpace(CurrentPath) || !PathHelper.DirectoryExists(CurrentPath) ||
                    CurrentPath == CacheHelper.GetWorkPath())
                {
                    return;
                }

                LoadExcels(CurrentPath);
            }
            catch (Exception Ex)
            {
                Logger.Warning(Ex.Message);
            }
            finally
            {
                NotifyWorkPathItemEvent_ = true;
            }
        }

        private void CreatePluginMenu()
        {
            foreach (var Plugin in PluginManager.ExportPlugins)
            {
                var PluginItem = new ToolStripMenuItem(Plugin.GetDisplayName());
                PluginItem.CheckOnClick = true;
                PluginItem.Name = Plugin.GetDisplayName();

                if (string.IsNullOrWhiteSpace(ExportPlugin_))
                {
                    ExportPlugin_ = Plugin.GetDisplayName();
                    CacheHelper.SetExportPlugin(ExportPlugin_);
                }

                if (Plugin.GetDisplayName() == ExportPlugin_)
                {
                    PluginItem.Checked = true;
                }

                PluginItem.Click += (Sender, Args) =>
                {
                    var Item = Sender as ToolStripMenuItem;
                    SelectPluginItem(Item.Name);
                };

                ToolStripMenuItemFormat.DropDownItems.Add(PluginItem);
            }

            SelectPluginItem(ExportPlugin_);
        }

        private void SelectPluginItem(string PluginName)
        {
            if (string.IsNullOrWhiteSpace(PluginName))
            {
                return;
            }

            foreach (var Item in ToolStripMenuItemFormat.DropDownItems)
            {
                var PluginItem = Item as ToolStripMenuItem;

                if (PluginItem.Name == PluginName)
                {
                    PluginItem.Checked = true;
                }
                else
                {
                    PluginItem.Checked = false;
                }
            }

            ExportPlugin_ = PluginName;
        }

        private void ExportExcels()
        {
            try
            {
                var ExportCount = 0;
                var ExportPath = PathHelper.UnifyPath($"{CacheHelper.GetWorkPath()}Export/");

                PathHelper.CreateDirectory(ExportPath);

                if (string.IsNullOrWhiteSpace(ExportPlugin_))
                {
                    MessageBox.Show("请选择导出格式");
                    return;
                }

                var FileList = new List<string>();

                foreach (var ExcelPath in ExcelFiles_)
                {
                    var FileName = PathHelper.GetFileNameWithoutExt(ExcelPath);

                    for (var Index = 0; Index < CheckedListBoxExcels.Items.Count; ++Index)
                    {
                        if (CheckedListBoxExcels.GetItemText(CheckedListBoxExcels.Items[Index]) == FileName &&
                            CheckedListBoxExcels.GetItemChecked(Index))
                        {
                            FileList.Add(ExcelPath);
                        }
                    }
                }

                ProgressBarMain.Value = 0;
                ProgressBarMain.Maximum = FileList.Count;
                ExportCount = ExcelExport.ExportDirectory(ExportPlugin_, FileList.ToArray(), CheckBoxExportAllSheet.Checked, ExportPath, (Cur, Max) =>
                    {
                        ProgressBarMain.Value = Cur;
                        Application.DoEvents();
                    });

                if (CheckBoxAutoOpen.Checked && ExportCount != 0)
                {
                    Process.Start(ExportPath);
                }

                RefreshExcels();

                Logger.Warning($"总共:{FileList.Count}个文件，导出:{ExportCount}个文件");

                if (ExportCount > 0)
                {
                    MessageBox.Show($"总共:{FileList.Count}个文件，导出:{ExportCount}个文件", "DataExportTool",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex.Message);
            }
        }

        private void StartMonitorExcelPath()
        {
            var WorkPath = CacheHelper.GetWorkPath();
            StopMonitorExcelPath();

            Watcher_ = new FileSystemWatcher(WorkPath, "*.xlsx");
            Watcher_.Changed += (Sender, Args) =>
            {
                /*if (!PathHelper.IsNormalFile(Args.FullPath))
                {
                    return;
                }
                Logger.Info($"监测到文件改变 : {Args.FullPath}");*/
                RefreshExcels();
            };
            Watcher_.Created += (Sender, Args) =>
            {
                /*if (!PathHelper.IsNormalFile(Args.FullPath))
                {
                    return;
                }
                Logger.Info($"监测到文件创建 : {Args.FullPath}");*/
                RefreshExcels();
            };
            Watcher_.EnableRaisingEvents = true;
        }

        private void StopMonitorExcelPath()
        {
            if (Watcher_ == null)
            {
                return;
            }

            Watcher_.EnableRaisingEvents = false;
        }

        private void UpdateExtSupportChanged()
        {
            if (!NotifyExtSupportChangeEvent_)
            {
                return;
            }

            try
            {
                NotifyExtSupportChangeEvent_ = false;

                CacheHelper.SetExtSupport(CheckBoxExtXlsx.Checked, CheckBoxExtXls.Checked, CheckBoxExtXlsm.Checked);
                LoadExcels(CacheHelper.GetWorkPath());
            }
            catch (Exception Ex)
            {
                Logger.Warning(Ex.Message);
            }
            finally
            {
                NotifyExtSupportChangeEvent_ = true;
            }
        }

        private bool IsSupportExt(string Ext)
        {
            return (Ext == ".xlsx" && CheckBoxExtXlsx.Checked) || (Ext == ".xls" && CheckBoxExtXls.Checked) || (Ext == ".xlsm" && CheckBoxExtXlsm.Checked);
        }

        private void StartUpdate()
        {
            Process.Start("AppUpdater.exe", "-c DataExportTool -n DESKTOP-9J05AKF -r http://{0}:8080/lite/dataexporttool/ -v " + Version + " true");
        }

        private void CheckUpdate()
        {
            Process.Start("AppUpdater.exe", "-c DataExportTool -n DESKTOP-9J05AKF -r http://{0}:8080/lite/dataexporttool/ -v " + Version + " false");
        }
    }
}