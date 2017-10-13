using System;
using System.Collections.Generic;

namespace IExportPlugin
{
    public struct ExportData
    {
        public string Name;
        public string InputPath;
        public string OutputPath;
        public List<List<string>> Data;
    }

    public interface IExportPlugin
    {
        string GetDisplayName();

        string GetExportExt();

        string GetVersion();

        void SetPath(string path);

        string GetPath();

        bool Export(ExportData data);

        string Check(ExportData data);
    }
}
