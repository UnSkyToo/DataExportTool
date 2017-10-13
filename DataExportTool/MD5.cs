using System;
using System.IO;
using System.Text;

namespace DataExportTool
{
    public static class MD5
    {
        public static string Get(string filePath)
        {
            try
            {
                FileStream fp = new FileStream(filePath, FileMode.Open);

                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] data = md5.ComputeHash(fp);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }

                fp.Close();
                fp.Dispose();

                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
