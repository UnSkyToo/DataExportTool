using System;
using System.Diagnostics;
using System.Windows.Forms;
using DataExportTool.Helper;

namespace DataExportTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object Sender, EventArgs Args)
        {
            Startup();
        }

        private void ToolStripMenuItemOpen_Click(object Sender, EventArgs Args)
        {
            var Dialog = new FolderBrowserDialog();
            Dialog.SelectedPath = CacheHelper.GetWorkPath().Replace('/', '\\');
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                LoadExcels(Dialog.SelectedPath);
            }
        }

        private void ToolStripMenuItemRefresh_Click(object Sender, EventArgs Args)
        {
            RefreshExcels();
        }

        private void ToolStripMenuItemExport_Click(object Sender, EventArgs Args)
        {
            ExportExcels();
        }
        
        private void ToolStripMenuItemPlugin_Click(object Sender, EventArgs Args)
        {
            new PluginForm().ShowDialog();
        }

        private void ToolStripMenuItemUpdate_Click(object Sender, EventArgs Args)
        {
            StartUpdate();
        }

        private void ToolStripMenuItemHelp_Click(object Sender, EventArgs Args)
        {
            MessageBox.Show("Excel Export Tool\r\nBy UnSkyToo -_-!");
        }

        private void MainForm_FormClosing(object Sender, FormClosingEventArgs Args)
        {
            Shutdown();
        }

        private void CheckedListBoxExcels_ItemCheck(object Sender, ItemCheckEventArgs Args)
        {
            UpdateCheckedListBoxExcels(Args.Index, Args.NewValue == CheckState.Checked);
        }

        private void CheckBoxSelectAll_CheckedChanged(object Sender, EventArgs Args)
        {
            SelectExcels(CheckBoxSelectAll.CheckState == CheckState.Checked);
        }

        private void ComboBoxWorkPath_SelectedIndexChanged(object Sender, EventArgs Args)
        {
            WorkPathListChanged();
        }

        private void CheckBoxExtXlsx_CheckedChanged(object Sender, EventArgs Args)
        {
            UpdateExtSupportChanged();
        }

        private void CheckBoxExtXls_CheckedChanged(object Sender, EventArgs Args)
        {
            UpdateExtSupportChanged();
        }

        private void CheckBoxExtXlsm_CheckedChanged(object Sender, EventArgs Args)
        {
            UpdateExtSupportChanged();
        }

        private void TextBoxFilter_TextChanged(object Sender, EventArgs Args)
        {
            LoadExcels(CacheHelper.GetWorkPath());
        }

        private void ComboBoxSortType_SelectedIndexChanged(object Sender, EventArgs Args)
        {
            LoadExcels(CacheHelper.GetWorkPath());
        }
    }
}