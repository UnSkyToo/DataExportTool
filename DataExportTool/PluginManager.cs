using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace DataExportTool
{
    public class PluginManager
    {
        public static readonly string ExportPluginBaseName = "IExportPlugin";

        private static PluginManager instance = null;

        private List<IExportPlugin.IExportPlugin> m_ExportPlugins = new List<IExportPlugin.IExportPlugin>();

        public List<IExportPlugin.IExportPlugin> ExportPlgLists
        {
            get { return m_ExportPlugins; }
        }

        public PluginManager()
        {
            m_ExportPlugins.Clear();
        }

        public static PluginManager Instance()
        {
            if (instance == null)
            {
                instance = new PluginManager();
            }
            return instance;
        }

        public void LoadExportPlugins(string pluginRootPath)
        {
            string[] pluginList = Directory.GetFiles(pluginRootPath);

            foreach (string pluginPath in pluginList)
            {
                if (Path.GetExtension(pluginPath) != ".dll")
                {
                    continue;
                }

                string pluginName = Path.GetFileNameWithoutExtension(pluginPath);

                if (pluginName == ExportPluginBaseName)
                {
                    continue;
                }

                string fullPath = $"{System.Windows.Forms.Application.StartupPath}\\ExportPlugin\\{pluginName}.dll";
                if (!File.Exists(fullPath))
                {
                    continue;
                }

                var ass = Assembly.LoadFile(fullPath);
                foreach (var t in ass.GetTypes())
                {
                    if (t.GetInterface(ExportPluginBaseName) != null)
                    {
                        var plugin = Activator.CreateInstance(t) as IExportPlugin.IExportPlugin;
                        plugin.SetPath(fullPath);
                        m_ExportPlugins.Add(plugin);
                    }
                }
            }
        }

        public IExportPlugin.IExportPlugin GetExportPluginWithName(string name)
        {
            foreach (var plugin in m_ExportPlugins)
            {
                if (plugin.GetDisplayName() == name)
                {
                    return plugin;
                }
            }

            return null;
        }
    }
}
