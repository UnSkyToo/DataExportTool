using System;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;

namespace AppUpdater
{
    public static class ZipUtil
    {
        public static readonly int BufferSize = 4096;

        public static void ZipFileToStream(Stream outStream, string FilePath, string password, int zipLevel)
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    throw new Exception("文件" + FilePath + "不存在");
                }

                using (FileStream inStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    FileInfo fileInfo = new FileInfo(FilePath);

                    ZipOutputStream zipStream = new ZipOutputStream(outStream);
                    zipLevel = zipLevel > 9 ? 9 : zipLevel < 0 ? 0 : zipLevel;
                    zipStream.SetLevel(zipLevel);
                    zipStream.Password = password;

                    byte[] buffer = new byte[BufferSize];
                    int size = 0;

                    ZipEntry zipEntry = new ZipEntry(Path.GetFileName(FilePath));
                    zipEntry.DateTime = fileInfo.CreationTime > fileInfo.LastWriteTime ? fileInfo.LastWriteTime : fileInfo.CreationTime;
                    zipEntry.Size = fileInfo.Length;
                    zipStream.PutNextEntry(zipEntry);

                    while (true)
                    {
                        size = inStream.Read(buffer, 0, BufferSize);

                        if (size <= 0)
                        {
                            break;
                        }

                        zipStream.Write(buffer, 0, size);
                    }

                    zipStream.Finish();
                    inStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ZipFile(string zipFilePath, string filePath, string password, int zipLevel, bool overwrite)
        {
            try
            {
                if (File.Exists(zipFilePath) && !overwrite)
                {
                    return;
                }

                using (FileStream outStream = new FileStream(zipFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    ZipFileToStream(outStream, filePath, password, zipLevel);
                    outStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ZipDirectoryToZipStream(ZipOutputStream outStream, string directoryPath, string parentPath)
        {
            try
            {
                if (directoryPath == string.Empty)
                {
                    return;
                }

                if (directoryPath[directoryPath.Length - 1] != '/')
                {
                    directoryPath += '/';
                }

                if (parentPath[parentPath.Length - 1] != '/')
                {
                    parentPath += '/';
                }

                ICSharpCode.SharpZipLib.Checksums.Crc32 crc = new ICSharpCode.SharpZipLib.Checksums.Crc32();
                string[] filePaths = Directory.GetFileSystemEntries(directoryPath);

                foreach (string path in filePaths)
                {
                    var filePath = path.Replace('\\', '/');
                    if (Directory.Exists(filePath)) // 是目录
                    {
                        string pPath = parentPath + filePath.Substring(filePath.LastIndexOf('/') + 1);
                        pPath += '/';
                        ZipDirectoryToZipStream(outStream, filePath, pPath);
                    }
                    else
                    {
                        using (FileStream inStream = File.OpenRead(filePath))
                        {
                            byte[] buffer = new byte[inStream.Length];
                            inStream.Read(buffer, 0, buffer.Length);
                            inStream.Close();

                            crc.Reset();
                            crc.Update(buffer);

                            string entryPath = parentPath + filePath.Substring(filePath.LastIndexOf('/') + 1);
                            ZipEntry zipEntry = new ZipEntry(entryPath);
                            FileInfo fileInfo = new FileInfo(filePath);

                            zipEntry.DateTime = fileInfo.CreationTime > fileInfo.LastWriteTime ? fileInfo.LastWriteTime : fileInfo.CreationTime;
                            zipEntry.Size = fileInfo.Length;
                            zipEntry.Crc = crc.Value;

                            outStream.PutNextEntry(zipEntry);
                            outStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ZipDirectoryToStream(Stream outStream, string directoryPath, string password, int zipLevel)
        {
            try
            {
                ZipOutputStream zipStream = new ZipOutputStream(outStream);
                zipLevel = zipLevel > 9 ? 9 : zipLevel < 0 ? 0 : zipLevel;
                zipStream.SetLevel(zipLevel);
                zipStream.Password = password;
                ZipDirectoryToZipStream(zipStream, directoryPath, string.Empty);
                zipStream.Finish();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ZipDirectoriesToStream(Stream outStream, string parentPath, string[] directories, string password, int zipLevel)
        {
            try
            {
                ZipOutputStream zipStream = new ZipOutputStream(outStream);
                zipLevel = zipLevel > 9 ? 9 : zipLevel < 0 ? 0 : zipLevel;
                zipStream.SetLevel(zipLevel);
                zipStream.Password = password;

                foreach (var directoryPath in directories)
                {
                    ZipDirectoryToZipStream(zipStream, directoryPath, directoryPath.Substring(parentPath.Length + 1));
                }

                zipStream.Finish();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ZipDirectory(string zipFilePath, string directoryPath, string password, int zipLevel, bool overwrite)
        {
            try
            {
                if (File.Exists(zipFilePath) && !overwrite)
                {
                    return;
                }

                using (FileStream outStream = new FileStream(zipFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    ZipDirectoryToStream(outStream, directoryPath, password, zipLevel);
                    outStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UnZipFile(string zipFilePath, string unzipDirectoryPath, string password, bool overwrite)
        {
            try
            {
                if (!File.Exists(zipFilePath))
                {
                    throw new Exception("文件" + zipFilePath + "不存在");
                }

                if (unzipDirectoryPath == string.Empty)
                {
                    unzipDirectoryPath = zipFilePath.Substring(0, zipFilePath.LastIndexOf('/') + 1);

                    if (unzipDirectoryPath == string.Empty)
                    {
                        unzipDirectoryPath = Directory.GetCurrentDirectory();
                    }
                }
                if (!unzipDirectoryPath.EndsWith("/"))
                {
                    unzipDirectoryPath += "/";
                }

                using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    zipStream.Password = password;
                    ZipEntry zipEntry = null;

                    while ((zipEntry = zipStream.GetNextEntry()) != null)
                    {
                        if (zipEntry.Name == string.Empty)
                        {
                            continue;
                        }

                        string directoryPath = Path.GetDirectoryName(zipEntry.Name);

                        Directory.CreateDirectory(unzipDirectoryPath + directoryPath);

                        if (!zipEntry.IsDirectory)
                        {
                            if (!File.Exists(unzipDirectoryPath + zipEntry.Name) || overwrite)
                            {
                                using (FileStream outStream = File.Create(unzipDirectoryPath + zipEntry.Name))
                                {
                                    byte[] buffer = new byte[BufferSize];
                                    int size = 0;

                                    while (true)
                                    {
                                        size = zipStream.Read(buffer, 0, BufferSize);

                                        if (size <= 0)
                                        {
                                            break;
                                        }

                                        outStream.Write(buffer, 0, size);
                                    }

                                    outStream.Close();
                                }
                            }
                        }
                    }

                    zipStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UnZipFileEx(string zipFilePath, string unzipDirectoryPath, string password, bool overwrite, Action<string, long, long> progressCallback)
        {
            try
            {
                if (!File.Exists(zipFilePath))
                {
                    throw new Exception("文件" + zipFilePath + "不存在");
                }

                if (unzipDirectoryPath == string.Empty)
                {
                    unzipDirectoryPath = zipFilePath.Substring(0, zipFilePath.LastIndexOf('/') + 1);

                    if (unzipDirectoryPath == string.Empty)
                    {
                        unzipDirectoryPath = Directory.GetCurrentDirectory();
                    }
                }
                if (!unzipDirectoryPath.EndsWith("/"))
                {
                    unzipDirectoryPath += "/";
                }

                using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(zipFilePath)))
                {

                    zipStream.Password = password;
                    ZipEntry zipEntry = null;

                    while ((zipEntry = zipStream.GetNextEntry()) != null)
                    {
                        if (zipEntry.Name == string.Empty)
                        {
                            continue;
                        }

                        string directoryPath = Path.GetDirectoryName(zipEntry.Name);

                        Directory.CreateDirectory(unzipDirectoryPath + directoryPath);

                        if (!zipEntry.IsDirectory)
                        {
                            if (!File.Exists(unzipDirectoryPath + zipEntry.Name) || overwrite)
                            {
                                using (FileStream outStream = File.Create(unzipDirectoryPath + zipEntry.Name))
                                {
                                    byte[] buffer = new byte[BufferSize];
                                    int size = 0;
                                    long maxSize = zipStream.Length;
                                    long curSize = size;

                                    while (true)
                                    {
                                        size = zipStream.Read(buffer, 0, BufferSize);

                                        if (size <= 0)
                                        {
                                            break;
                                        }

                                        outStream.Write(buffer, 0, size);

                                        curSize += size;
                                        if (progressCallback != null)
                                        {
                                            progressCallback(zipEntry.Name, curSize, maxSize);
                                        }
                                    }

                                    outStream.Close();
                                }
                            }
                        }
                    }

                    zipStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static long GetFileSizeUnCompresssed(string ZipFilePath, string Password)
        {
            try
            {
                long UnCompresssedSize = 0;
                using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(ZipFilePath)))
                {
                    ZipEntry ZipEntry = null;
                    while ((ZipEntry = zipStream.GetNextEntry()) != null)
                    {
                        if (ZipEntry.Name == string.Empty)
                        {
                            continue;
                        }

                        if (!ZipEntry.IsDirectory)
                        {
                            UnCompresssedSize += ZipEntry.Size;
                        }
                    }
                }

                return UnCompresssedSize;
            }
            catch
            {
                return long.MaxValue;
            }
        }
    }
}
