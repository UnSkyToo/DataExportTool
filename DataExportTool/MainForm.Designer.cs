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
            this.cblistTable = new System.Windows.Forms.CheckedListBox();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.打开OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.格式GToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.插件PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbSelectAll = new System.Windows.Forms.CheckBox();
            this.lExcelCount = new System.Windows.Forms.Label();
            this.lExcelSelect = new System.Windows.Forms.Label();
            this.cbAutoOpenExport = new System.Windows.Forms.CheckBox();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // cblistTable
            // 
            this.cblistTable.FormattingEnabled = true;
            this.cblistTable.Location = new System.Drawing.Point(0, 28);
            this.cblistTable.Name = "cblistTable";
            this.cblistTable.Size = new System.Drawing.Size(165, 644);
            this.cblistTable.TabIndex = 0;
            this.cblistTable.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cblistTable_ItemCheck);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开OToolStripMenuItem,
            this.刷新ToolStripMenuItem,
            this.格式GToolStripMenuItem,
            this.导出EToolStripMenuItem,
            this.插件PToolStripMenuItem,
            this.帮助AToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(943, 25);
            this.menuMain.TabIndex = 1;
            this.menuMain.Text = "menuStrip1";
            // 
            // 打开OToolStripMenuItem
            // 
            this.打开OToolStripMenuItem.Name = "打开OToolStripMenuItem";
            this.打开OToolStripMenuItem.Size = new System.Drawing.Size(62, 21);
            this.打开OToolStripMenuItem.Text = "打开(&O)";
            this.打开OToolStripMenuItem.Click += new System.EventHandler(this.打开OToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.刷新ToolStripMenuItem.Text = "刷新(&F)";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // 格式GToolStripMenuItem
            // 
            this.格式GToolStripMenuItem.Name = "格式GToolStripMenuItem";
            this.格式GToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.格式GToolStripMenuItem.Text = "格式(&G)";
            // 
            // 导出EToolStripMenuItem
            // 
            this.导出EToolStripMenuItem.Name = "导出EToolStripMenuItem";
            this.导出EToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.导出EToolStripMenuItem.Text = "导出(&E)";
            this.导出EToolStripMenuItem.Click += new System.EventHandler(this.导出EToolStripMenuItem_Click);
            // 
            // 插件PToolStripMenuItem
            // 
            this.插件PToolStripMenuItem.Name = "插件PToolStripMenuItem";
            this.插件PToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.插件PToolStripMenuItem.Text = "插件(&P)";
            this.插件PToolStripMenuItem.Click += new System.EventHandler(this.插件PToolStripMenuItem_Click);
            // 
            // 帮助AToolStripMenuItem
            // 
            this.帮助AToolStripMenuItem.Name = "帮助AToolStripMenuItem";
            this.帮助AToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.帮助AToolStripMenuItem.Text = "帮助(&A)";
            this.帮助AToolStripMenuItem.Click += new System.EventHandler(this.帮助AToolStripMenuItem_Click);
            // 
            // cbSelectAll
            // 
            this.cbSelectAll.AutoSize = true;
            this.cbSelectAll.Location = new System.Drawing.Point(117, 678);
            this.cbSelectAll.Name = "cbSelectAll";
            this.cbSelectAll.Size = new System.Drawing.Size(48, 16);
            this.cbSelectAll.TabIndex = 2;
            this.cbSelectAll.Text = "全选";
            this.cbSelectAll.UseVisualStyleBackColor = true;
            this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
            // 
            // lExcelCount
            // 
            this.lExcelCount.AutoSize = true;
            this.lExcelCount.Location = new System.Drawing.Point(5, 682);
            this.lExcelCount.Name = "lExcelCount";
            this.lExcelCount.Size = new System.Drawing.Size(41, 12);
            this.lExcelCount.TabIndex = 3;
            this.lExcelCount.Text = "数量:0";
            // 
            // lExcelSelect
            // 
            this.lExcelSelect.AutoSize = true;
            this.lExcelSelect.Location = new System.Drawing.Point(59, 682);
            this.lExcelSelect.Name = "lExcelSelect";
            this.lExcelSelect.Size = new System.Drawing.Size(41, 12);
            this.lExcelSelect.TabIndex = 4;
            this.lExcelSelect.Text = "选中:0";
            // 
            // cbAutoOpenExport
            // 
            this.cbAutoOpenExport.AutoSize = true;
            this.cbAutoOpenExport.Location = new System.Drawing.Point(171, 41);
            this.cbAutoOpenExport.Name = "cbAutoOpenExport";
            this.cbAutoOpenExport.Size = new System.Drawing.Size(144, 16);
            this.cbAutoOpenExport.TabIndex = 5;
            this.cbAutoOpenExport.Text = "导出完毕自动打开目录";
            this.cbAutoOpenExport.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 698);
            this.Controls.Add(this.cbAutoOpenExport);
            this.Controls.Add(this.lExcelSelect);
            this.Controls.Add(this.lExcelCount);
            this.Controls.Add(this.cbSelectAll);
            this.Controls.Add(this.cblistTable);
            this.Controls.Add(this.menuMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuMain;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ExcelExportTool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox cblistTable;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助AToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbSelectAll;
        private System.Windows.Forms.Label lExcelCount;
        private System.Windows.Forms.Label lExcelSelect;
        private System.Windows.Forms.ToolStripMenuItem 导出EToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbAutoOpenExport;
        private System.Windows.Forms.ToolStripMenuItem 打开OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 格式GToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 插件PToolStripMenuItem;
    }
}

