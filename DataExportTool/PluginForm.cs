using System;
using System.Windows.Forms;

namespace DataExportTool
{
    public partial class PluginForm : Form
    {
        public PluginForm()
        {
            InitializeComponent();
        }

        private void PluginForm_Load(object Sender, EventArgs Args)
        {
            RefreshWithPlugin();
        }

        private void RefreshWithPlugin()
        {
            listViewPlugin.View = View.Details;
            listViewPlugin.FullRowSelect = true;

            listViewPlugin.Clear();

            listViewPlugin.Columns.Add("名称", 100, HorizontalAlignment.Left);
            listViewPlugin.Columns.Add("版本", 60, HorizontalAlignment.Left);
            listViewPlugin.Columns.Add("后缀", 60, HorizontalAlignment.Left);
            listViewPlugin.Columns.Add("路径", 300, HorizontalAlignment.Left);

            foreach (var Plugin in PluginManager.ExportPlugins)
            {
                var Item = new ListViewItem();
                Item.Text = Plugin.GetDisplayName();
                Item.SubItems.Add(Plugin.GetVersion());
                Item.SubItems.Add(Plugin.GetExportExt());
                Item.SubItems.Add(Plugin.GetPath());
                listViewPlugin.Items.Add(Item);
            }
        }
    }
}