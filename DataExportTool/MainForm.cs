using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DataExportTool
{
    public partial class MainForm : Form
    {
        private bool m_noticeItemSelected = true;
        private bool m_noticeSelectedAllItem = true;

        private string m_exportPlugin = string.Empty;
        private List<string> m_excludeExt = new List<string>();
        private List<string> m_excelFiles = new List<string>();

        private void LoadExcels(string path, List<string> excludeExt)
        {
            try
            {
                ConfigUtil.SetWorkPath(path);
                cblistTable.Items.Clear();
                m_excelFiles.Clear();
                cbSelectAll.Checked = false;

                string[] excels = Directory.GetFiles(path);
                int selectCount = 0;

                foreach (string excelPath in excels)
                {
                    string ext = excelPath.Substring(excelPath.LastIndexOf('.') + 1);

                    if (excludeExt != null && excludeExt.Contains(ext))
                    {
                        continue;
                    }

                    m_excelFiles.Add(excelPath);
                    string fileName = excelPath.Substring(excelPath.LastIndexOf('\\') + 1);

                    string newMD5 = MD5.Get(excelPath);
                    string oldMD5 = ConfigUtil.GetExcelMD5(excelPath);

                    if (newMD5 == oldMD5)
                    {
                        cblistTable.Items.Add(fileName, CheckState.Unchecked);
                    }
                    else
                    {
                        ConfigUtil.SetExcelMD5(excelPath, newMD5);
                        cblistTable.Items.Add(fileName, CheckState.Checked);
                        selectCount++;
                    }
                }

                lExcelCount.Text = "数量:" + excels.Length.ToString();
                lExcelSelect.Text = "选中:" + selectCount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateExcels(int index, bool isChecked)
        {
            m_noticeSelectedAllItem = false;
            int count = 0;
            for (int i = 0; i < cblistTable.Items.Count; i++)
            {
                if (index == i) // ItemCheck事件在响应的时候，Item的值还未改变，所以用传入的参数判断
                {
                    if (isChecked)
                    {
                        count++;
                    }
                }
                else if (cblistTable.GetItemChecked(i))
                {
                    count++;
                }
            }

            if (count == 0)
            {
                cbSelectAll.CheckState = CheckState.Unchecked;
            }
            else if (count == cblistTable.Items.Count)
            {
                cbSelectAll.CheckState = CheckState.Checked;
            }
            else
            {
                cbSelectAll.CheckState = CheckState.Indeterminate;
            }

            lExcelSelect.Text = "选中:" + count.ToString();
            m_noticeSelectedAllItem = true;
        }

        private void SelectExcels(bool selected)
        {
            m_noticeItemSelected = false;
            for (int i = 0; i < cblistTable.Items.Count; i++)
            {
                cblistTable.SetItemChecked(i, selected);
            }

            if (selected)
            {
                lExcelSelect.Text = "选中:" + cblistTable.Items.Count.ToString();
            }
            else
            {
                lExcelSelect.Text = "选中:0";
            }

            m_noticeItemSelected = true;
        }

        private void CreatePluginMenu()
        {
            foreach (var plugin in PluginManager.Instance().ExportPlgLists)
            {
                ToolStripMenuItem plguinItem = new ToolStripMenuItem(plugin.GetDisplayName());
                plguinItem.CheckOnClick = true;
                plguinItem.Name = plugin.GetDisplayName();

                if (plugin.GetDisplayName() == m_exportPlugin)
                {
                    plguinItem.Checked = true;
                }

                plguinItem.Click += PlguinItem_Click;

                格式GToolStripMenuItem.DropDownItems.Add(plguinItem);
            }

            SelectPluginItem(m_exportPlugin);
        }

        private void PlguinItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            SelectPluginItem(item.Name);
        }

        private void SelectPluginItem(string name)
        {
            if (name.Length == 0)
            {
                return;
            }

            foreach (var v in 格式GToolStripMenuItem.DropDownItems)
            {
                ToolStripMenuItem item = v as ToolStripMenuItem;

                if (item.Name == name)
                {
                    item.Checked = true;
                }
                else
                {
                    item.Checked = false;
                }
            }

            m_exportPlugin = name;
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                cblistTable.CheckOnClick = true;

                ConfigUtil.LoadConfig();

                cbAutoOpenExport.Checked = ConfigUtil.GetAutoOpenExport();
                m_excludeExt = ConfigUtil.GetExcludeExt();
                m_exportPlugin = ConfigUtil.GetExportPlugin();

                //LoadExcels(Application.StartupPath + "\\excel");
                string workPath = ConfigUtil.GetWorkPath();

                if (workPath != string.Empty)
                {
                    LoadExcels(workPath, null);
                }

                PluginManager.Instance().LoadExportPlugins(Application.StartupPath + "\\ExportPlugin");
                CreatePluginMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadExcels(dialog.SelectedPath, null);
            }
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string workPath = ConfigUtil.GetWorkPath();

            if (workPath == string.Empty)
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadExcels(dialog.SelectedPath, null);
                }
            }
            else
            {
                LoadExcels(workPath, null);
            }
        }

        private void 导出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int exportCount = 0;
                string exportPath = Application.StartupPath + "\\Export\\";

                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }

                if (m_exportPlugin.Length == 0)
                {
                    MessageBox.Show("请选择导出格式");
                    return;
                }

                foreach (string excelPath in m_excelFiles)
                {
                    string fileName = excelPath.Substring(excelPath.LastIndexOf('\\') + 1);

                    for (int i = 0; i < cblistTable.Items.Count; ++i)
                    {
                        if (cblistTable.GetItemText(cblistTable.Items[i]) == fileName)
                        {
                            if (cblistTable.GetItemChecked(i))
                            {
                                string name = fileName.Substring(0, fileName.LastIndexOf('.'));
                                if (ExcelExport.Export(m_exportPlugin, excelPath, name, exportPath + name))
                                {
                                    exportCount++;
                                }
                            }
                        }
                    }
                }

                if (cbAutoOpenExport.Checked && exportCount != 0)
                {
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\Export");
                }

                MessageBox.Show("共导出:" + exportCount.ToString() + "个文件");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void 插件PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PluginForm form = new PluginForm();
            form.ShowDialog();
        }

        private void 帮助AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Excel Export Tool\r\nBy UnSkyToo -_-!");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfigUtil.SetAutoOpenExport(cbAutoOpenExport.Checked);
            ConfigUtil.SetExcludeExt(m_excludeExt);
            ConfigUtil.SetExportPlugin(m_exportPlugin);

            ConfigUtil.SaveConfig();
        }

        private void cblistTable_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (m_noticeItemSelected)
            {
                UpdateExcels(e.Index, e.NewValue == CheckState.Checked);
            }
        }

        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (m_noticeSelectedAllItem)
            {
                SelectExcels(cbSelectAll.Checked);
            }
        }
    }
}
