using System.Collections.Generic;

namespace IExportPlugin
{
    public class ExportData
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

        void SetPath(string Path);

        string GetPath();

        bool Export(ExportData Data);

        string Check(ExportData Data);
    }
}