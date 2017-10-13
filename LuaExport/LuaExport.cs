using System;

namespace LuaExport
{
    public class LuaExport : IExportPlugin.IExportPlugin
    {
        private string m_path = string.Empty;

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

        public void SetPath(string path)
        {
            m_path = path;
        }

        public string GetPath()
        {
            return m_path;
        }

        public bool Export(IExportPlugin.ExportData data)
        {
            return false;
        }

        public string Check(IExportPlugin.ExportData data)
        {
            return string.Empty;
        }
    }
}
