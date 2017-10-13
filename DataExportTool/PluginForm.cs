using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataExportTool
{
    public partial class PluginForm : Form
    {
        public PluginForm()
        {
            InitializeComponent();
        }

        private void PluginForm_Load(object sender, EventArgs e)
        {
            refreshWithPlugin();
        }

        private void refreshWithPlugin()
        {
            listViewPlugin.View = View.Details;
            listViewPlugin.FullRowSelect = true;

            listViewPlugin.Clear();

            listViewPlugin.Columns.Add("名称", 100, HorizontalAlignment.Left);
            listViewPlugin.Columns.Add("版本", 60, HorizontalAlignment.Left);
            listViewPlugin.Columns.Add("后缀", 60, HorizontalAlignment.Left);
            listViewPlugin.Columns.Add("路径", 300, HorizontalAlignment.Left);

            foreach (var plugin in PluginManager.Instance().ExportPlgLists)
            {
                ListViewItem item = new ListViewItem();
                item.Text = plugin.GetDisplayName();
                item.SubItems.Add(plugin.GetVersion());
                item.SubItems.Add(plugin.GetExportExt());
                item.SubItems.Add(plugin.GetPath());
                listViewPlugin.Items.Add(item);
            }
        }
    }
}
