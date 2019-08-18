namespace DataExportTool
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.CheckedListBoxExcels = new System.Windows.Forms.CheckedListBox();
            this.MenuStripMain = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemExport = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPlugin = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.LabelExcelTotalCount = new System.Windows.Forms.Label();
            this.LabelExcelSelectCount = new System.Windows.Forms.Label();
            this.CheckBoxAutoOpen = new System.Windows.Forms.CheckBox();
            this.RichTextBoxLogger = new System.Windows.Forms.RichTextBox();
            this.ProgressBarMain = new System.Windows.Forms.ProgressBar();
            this.ComboBoxWorkPath = new System.Windows.Forms.ComboBox();
            this.LabelWorkPath = new System.Windows.Forms.Label();
            this.CheckBoxExtXlsx = new System.Windows.Forms.CheckBox();
            this.CheckBoxExtXls = new System.Windows.Forms.CheckBox();
            this.CheckBoxExtXlsm = new System.Windows.Forms.CheckBox();
            this.TextBoxFilter = new System.Windows.Forms.TextBox();
            this.CheckBoxExportAllSheet = new System.Windows.Forms.CheckBox();
            this.LabelSortType = new System.Windows.Forms.Label();
            this.ComboBoxSortType = new System.Windows.Forms.ComboBox();
            this.ToolStripMenuItemUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // CheckedListBoxExcels
            // 
            this.CheckedListBoxExcels.FormattingEnabled = true;
            this.CheckedListBoxExcels.Location = new System.Drawing.Point(0, 55);
            this.CheckedListBoxExcels.Name = "CheckedListBoxExcels";
            this.CheckedListBoxExcels.Size = new System.Drawing.Size(204, 564);
            this.CheckedListBoxExcels.TabIndex = 0;
            this.CheckedListBoxExcels.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListBoxExcels_ItemCheck);
            // 
            // MenuStripMain
            // 
            this.MenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemOpen,
            this.ToolStripMenuItemRefresh,
            this.ToolStripMenuItemFormat,
            this.ToolStripMenuItemExport,
            this.ToolStripMenuItemPlugin,
            this.ToolStripMenuItemUpdate,
            this.ToolStripMenuItemHelp});
            this.MenuStripMain.Location = new System.Drawing.Point(0, 0);
            this.MenuStripMain.Name = "MenuStripMain";
            this.MenuStripMain.Size = new System.Drawing.Size(909, 25);
            this.MenuStripMain.TabIndex = 1;
            this.MenuStripMain.Text = "menuStrip1";
            // 
            // ToolStripMenuItemOpen
            // 
            this.ToolStripMenuItemOpen.Name = "ToolStripMenuItemOpen";
            this.ToolStripMenuItemOpen.Size = new System.Drawing.Size(62, 21);
            this.ToolStripMenuItemOpen.Text = "打开(&O)";
            this.ToolStripMenuItemOpen.Click += new System.EventHandler(this.ToolStripMenuItemOpen_Click);
            // 
            // ToolStripMenuItemRefresh
            // 
            this.ToolStripMenuItemRefresh.Name = "ToolStripMenuItemRefresh";
            this.ToolStripMenuItemRefresh.Size = new System.Drawing.Size(58, 21);
            this.ToolStripMenuItemRefresh.Text = "刷新(&F)";
            this.ToolStripMenuItemRefresh.Click += new System.EventHandler(this.ToolStripMenuItemRefresh_Click);
            // 
            // ToolStripMenuItemFormat
            // 
            this.ToolStripMenuItemFormat.Name = "ToolStripMenuItemFormat";
            this.ToolStripMenuItemFormat.Size = new System.Drawing.Size(61, 21);
            this.ToolStripMenuItemFormat.Text = "格式(&G)";
            // 
            // ToolStripMenuItemExport
            // 
            this.ToolStripMenuItemExport.Name = "ToolStripMenuItemExport";
            this.ToolStripMenuItemExport.Size = new System.Drawing.Size(59, 21);
            this.ToolStripMenuItemExport.Text = "导出(&E)";
            this.ToolStripMenuItemExport.Click += new System.EventHandler(this.ToolStripMenuItemExport_Click);
            // 
            // ToolStripMenuItemPlugin
            // 
            this.ToolStripMenuItemPlugin.Name = "ToolStripMenuItemPlugin";
            this.ToolStripMenuItemPlugin.Size = new System.Drawing.Size(59, 21);
            this.ToolStripMenuItemPlugin.Text = "插件(&P)";
            this.ToolStripMenuItemPlugin.Click += new System.EventHandler(this.ToolStripMenuItemPlugin_Click);
            // 
            // ToolStripMenuItemHelp
            // 
            this.ToolStripMenuItemHelp.Name = "ToolStripMenuItemHelp";
            this.ToolStripMenuItemHelp.Size = new System.Drawing.Size(60, 21);
            this.ToolStripMenuItemHelp.Text = "帮助(&A)";
            this.ToolStripMenuItemHelp.Click += new System.EventHandler(this.ToolStripMenuItemHelp_Click);
            // 
            // CheckBoxSelectAll
            // 
            this.CheckBoxSelectAll.AutoSize = true;
            this.CheckBoxSelectAll.Location = new System.Drawing.Point(154, 650);
            this.CheckBoxSelectAll.Name = "CheckBoxSelectAll";
            this.CheckBoxSelectAll.Size = new System.Drawing.Size(48, 16);
            this.CheckBoxSelectAll.TabIndex = 2;
            this.CheckBoxSelectAll.Text = "全选";
            this.CheckBoxSelectAll.UseVisualStyleBackColor = true;
            this.CheckBoxSelectAll.CheckedChanged += new System.EventHandler(this.CheckBoxSelectAll_CheckedChanged);
            // 
            // LabelExcelTotalCount
            // 
            this.LabelExcelTotalCount.AutoSize = true;
            this.LabelExcelTotalCount.Location = new System.Drawing.Point(-1, 650);
            this.LabelExcelTotalCount.Name = "LabelExcelTotalCount";
            this.LabelExcelTotalCount.Size = new System.Drawing.Size(41, 12);
            this.LabelExcelTotalCount.TabIndex = 3;
            this.LabelExcelTotalCount.Text = "数量:0";
            // 
            // LabelExcelSelectCount
            // 
            this.LabelExcelSelectCount.AutoSize = true;
            this.LabelExcelSelectCount.Location = new System.Drawing.Point(73, 650);
            this.LabelExcelSelectCount.Name = "LabelExcelSelectCount";
            this.LabelExcelSelectCount.Size = new System.Drawing.Size(41, 12);
            this.LabelExcelSelectCount.TabIndex = 4;
            this.LabelExcelSelectCount.Text = "选中:0";
            // 
            // CheckBoxAutoOpen
            // 
            this.CheckBoxAutoOpen.AutoSize = true;
            this.CheckBoxAutoOpen.Location = new System.Drawing.Point(762, 625);
            this.CheckBoxAutoOpen.Name = "CheckBoxAutoOpen";
            this.CheckBoxAutoOpen.Size = new System.Drawing.Size(144, 16);
            this.CheckBoxAutoOpen.TabIndex = 5;
            this.CheckBoxAutoOpen.Text = "导出完毕自动打开目录";
            this.CheckBoxAutoOpen.UseVisualStyleBackColor = true;
            // 
            // RichTextBoxLogger
            // 
            this.RichTextBoxLogger.BackColor = System.Drawing.Color.Black;
            this.RichTextBoxLogger.Location = new System.Drawing.Point(212, 54);
            this.RichTextBoxLogger.Name = "RichTextBoxLogger";
            this.RichTextBoxLogger.ReadOnly = true;
            this.RichTextBoxLogger.Size = new System.Drawing.Size(696, 565);
            this.RichTextBoxLogger.TabIndex = 6;
            this.RichTextBoxLogger.Text = "";
            // 
            // ProgressBarMain
            // 
            this.ProgressBarMain.Location = new System.Drawing.Point(210, 648);
            this.ProgressBarMain.Name = "ProgressBarMain";
            this.ProgressBarMain.Size = new System.Drawing.Size(696, 15);
            this.ProgressBarMain.TabIndex = 7;
            // 
            // ComboBoxWorkPath
            // 
            this.ComboBoxWorkPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxWorkPath.FormattingEnabled = true;
            this.ComboBoxWorkPath.Location = new System.Drawing.Point(257, 28);
            this.ComboBoxWorkPath.Name = "ComboBoxWorkPath";
            this.ComboBoxWorkPath.Size = new System.Drawing.Size(452, 20);
            this.ComboBoxWorkPath.TabIndex = 8;
            this.ComboBoxWorkPath.SelectedIndexChanged += new System.EventHandler(this.ComboBoxWorkPath_SelectedIndexChanged);
            // 
            // LabelWorkPath
            // 
            this.LabelWorkPath.AutoSize = true;
            this.LabelWorkPath.Location = new System.Drawing.Point(210, 32);
            this.LabelWorkPath.Name = "LabelWorkPath";
            this.LabelWorkPath.Size = new System.Drawing.Size(41, 12);
            this.LabelWorkPath.TabIndex = 9;
            this.LabelWorkPath.Text = "工作区";
            // 
            // CheckBoxExtXlsx
            // 
            this.CheckBoxExtXlsx.AutoSize = true;
            this.CheckBoxExtXlsx.Location = new System.Drawing.Point(1, 625);
            this.CheckBoxExtXlsx.Name = "CheckBoxExtXlsx";
            this.CheckBoxExtXlsx.Size = new System.Drawing.Size(48, 16);
            this.CheckBoxExtXlsx.TabIndex = 10;
            this.CheckBoxExtXlsx.Text = "xlsx";
            this.CheckBoxExtXlsx.UseVisualStyleBackColor = true;
            this.CheckBoxExtXlsx.CheckedChanged += new System.EventHandler(this.CheckBoxExtXlsx_CheckedChanged);
            // 
            // CheckBoxExtXls
            // 
            this.CheckBoxExtXls.AutoSize = true;
            this.CheckBoxExtXls.Location = new System.Drawing.Point(55, 625);
            this.CheckBoxExtXls.Name = "CheckBoxExtXls";
            this.CheckBoxExtXls.Size = new System.Drawing.Size(42, 16);
            this.CheckBoxExtXls.TabIndex = 10;
            this.CheckBoxExtXls.Text = "xls";
            this.CheckBoxExtXls.UseVisualStyleBackColor = true;
            this.CheckBoxExtXls.CheckedChanged += new System.EventHandler(this.CheckBoxExtXls_CheckedChanged);
            // 
            // CheckBoxExtXlsm
            // 
            this.CheckBoxExtXlsm.AutoSize = true;
            this.CheckBoxExtXlsm.Location = new System.Drawing.Point(103, 625);
            this.CheckBoxExtXlsm.Name = "CheckBoxExtXlsm";
            this.CheckBoxExtXlsm.Size = new System.Drawing.Size(48, 16);
            this.CheckBoxExtXlsm.TabIndex = 11;
            this.CheckBoxExtXlsm.Text = "xlsm";
            this.CheckBoxExtXlsm.UseVisualStyleBackColor = true;
            this.CheckBoxExtXlsm.CheckedChanged += new System.EventHandler(this.CheckBoxExtXlsm_CheckedChanged);
            // 
            // TextBoxFilter
            // 
            this.TextBoxFilter.Location = new System.Drawing.Point(3, 27);
            this.TextBoxFilter.Name = "TextBoxFilter";
            this.TextBoxFilter.Size = new System.Drawing.Size(201, 21);
            this.TextBoxFilter.TabIndex = 12;
            this.TextBoxFilter.TextChanged += new System.EventHandler(this.TextBoxFilter_TextChanged);
            // 
            // CheckBoxExportAllSheet
            // 
            this.CheckBoxExportAllSheet.AutoSize = true;
            this.CheckBoxExportAllSheet.Location = new System.Drawing.Point(212, 625);
            this.CheckBoxExportAllSheet.Name = "CheckBoxExportAllSheet";
            this.CheckBoxExportAllSheet.Size = new System.Drawing.Size(102, 16);
            this.CheckBoxExportAllSheet.TabIndex = 13;
            this.CheckBoxExportAllSheet.Text = "导出全部Sheet";
            this.CheckBoxExportAllSheet.UseVisualStyleBackColor = true;
            // 
            // LabelSortType
            // 
            this.LabelSortType.AutoSize = true;
            this.LabelSortType.Location = new System.Drawing.Point(715, 32);
            this.LabelSortType.Name = "LabelSortType";
            this.LabelSortType.Size = new System.Drawing.Size(53, 12);
            this.LabelSortType.TabIndex = 14;
            this.LabelSortType.Text = "排序方式";
            // 
            // ComboBoxSortType
            // 
            this.ComboBoxSortType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxSortType.FormattingEnabled = true;
            this.ComboBoxSortType.Location = new System.Drawing.Point(774, 28);
            this.ComboBoxSortType.Name = "ComboBoxSortType";
            this.ComboBoxSortType.Size = new System.Drawing.Size(132, 20);
            this.ComboBoxSortType.TabIndex = 15;
            this.ComboBoxSortType.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSortType_SelectedIndexChanged);
            // 
            // ToolStripMenuItemUpdate
            // 
            this.ToolStripMenuItemUpdate.Name = "ToolStripMenuItemUpdate";
            this.ToolStripMenuItemUpdate.Size = new System.Drawing.Size(61, 21);
            this.ToolStripMenuItemUpdate.Text = "更新(&U)";
            this.ToolStripMenuItemUpdate.Click += new System.EventHandler(this.ToolStripMenuItemUpdate_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 666);
            this.Controls.Add(this.ComboBoxSortType);
            this.Controls.Add(this.LabelSortType);
            this.Controls.Add(this.CheckBoxExportAllSheet);
            this.Controls.Add(this.TextBoxFilter);
            this.Controls.Add(this.CheckBoxExtXlsm);
            this.Controls.Add(this.CheckBoxExtXls);
            this.Controls.Add(this.CheckBoxExtXlsx);
            this.Controls.Add(this.LabelWorkPath);
            this.Controls.Add(this.ComboBoxWorkPath);
            this.Controls.Add(this.ProgressBarMain);
            this.Controls.Add(this.RichTextBoxLogger);
            this.Controls.Add(this.CheckBoxAutoOpen);
            this.Controls.Add(this.LabelExcelSelectCount);
            this.Controls.Add(this.LabelExcelTotalCount);
            this.Controls.Add(this.CheckBoxSelectAll);
            this.Controls.Add(this.CheckedListBoxExcels);
            this.Controls.Add(this.MenuStripMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.MenuStripMain;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ExcelExportTool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MenuStripMain.ResumeLayout(false);
            this.MenuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStripMain;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemOpen;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRefresh;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemFormat;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExport;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPlugin;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemHelp;
        private System.Windows.Forms.CheckedListBox CheckedListBoxExcels;
        private System.Windows.Forms.CheckBox CheckBoxSelectAll;
        private System.Windows.Forms.CheckBox CheckBoxAutoOpen;
        private System.Windows.Forms.Label LabelExcelTotalCount;
        private System.Windows.Forms.Label LabelExcelSelectCount;
        private System.Windows.Forms.RichTextBox RichTextBoxLogger;
        private System.Windows.Forms.ProgressBar ProgressBarMain;
        private System.Windows.Forms.ComboBox ComboBoxWorkPath;
        private System.Windows.Forms.Label LabelWorkPath;
        private System.Windows.Forms.CheckBox CheckBoxExtXlsx;
        private System.Windows.Forms.CheckBox CheckBoxExtXls;
        private System.Windows.Forms.CheckBox CheckBoxExtXlsm;
        private System.Windows.Forms.TextBox TextBoxFilter;
        private System.Windows.Forms.CheckBox CheckBoxExportAllSheet;
        private System.Windows.Forms.Label LabelSortType;
        private System.Windows.Forms.ComboBox ComboBoxSortType;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemUpdate;
    }
}