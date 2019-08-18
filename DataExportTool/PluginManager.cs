using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace DataExportTool
{
    public static class PluginManager
    {
        public static readonly string ExportPluginBaseName = "IExportPlugin";
        public static List<IExportPlugin.IExportPlugin> ExportPlugins { get; } = new List<IExportPlugin.IExportPlugin>();

        static PluginManager()
        {
            ExportPlugins.Clear();
        }

        public static void LoadExportPlugins(string PluginRootPath)
        {
            var PluginList = Directory.GetFiles(PluginRootPath);

            foreach (var PluginPath in PluginList)
            {
                if (Path.GetExtension(PluginPath) != ".dll")
                {
                    continue;
                }

                var PluginName = Path.GetFileNameWithoutExtension(PluginPath);
                if (PluginName == ExportPluginBaseName)
                {
                    continue;
                }

                var FullPath = $"{AppDomain.CurrentDomain.BaseDirectory}ExportPlugin/{PluginName}.dll";
                if (!File.Exists(FullPath))
                {
                    continue;
                }

                var PluginAss = Assembly.LoadFile(FullPath);
                foreach (var T in PluginAss.GetTypes())
                {
                    if (T.GetInterface(ExportPluginBaseName) != null)
                    {
                        if (Activator.CreateInstance(T) is IExportPlugin.IExportPlugin Plugin)
                        {
                            Plugin.SetPath(FullPath);
                            ExportPlugins.Add(Plugin);
                        }
                        else
                        {
                            Logger.Error($"unknown plugin type : {T.FullName}");
                        }
                    }
                }
            }
        }

        public static IExportPlugin.IExportPlugin GetExportPluginWithName(string Name)
        {
            foreach (var Plugin in ExportPlugins)
            {
                if (Plugin.GetDisplayName() == Name)
                {
                    return Plugin;
                }
            }

            return null;
        }
    }
}