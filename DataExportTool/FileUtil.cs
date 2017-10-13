using System;
using System.Text;
using System.IO;

namespace DataExportTool
{
    public static class FileUtil
    {
        public static void SafeRelease(FileStream fp)
        {
            if (fp != null)
            {
                fp.Close();
                fp.Dispose();
            }
        }

        public static void SafeRelease(StreamReader fp)
        {
            if (fp != null)
            {
                fp.Close();
                fp.Dispose();
            }
        }

        public static void SafeRelease(StreamWriter fp)
        {
            if (fp != null)
            {
                fp.Close();
                fp.Dispose();
            }
        }

        public static void WriteBool(FileStream fp, bool value)
        {
            if (value)
            {
                WriteInt8(fp, 1);
            }
            else
            {
                WriteInt8(fp, 0);
            }
        }

        public static void WriteInt8(FileStream fp, byte value)
        {
            fp.WriteByte(value);
        }

        public static void WriteInt16(FileStream fp, short value)
        {
            fp.WriteByte((byte)((value & 0x00ff)));
            fp.WriteByte((byte)((value & 0xff00) >> 8));
        }

        public static void WriteInt32(FileStream fp, int value)
        {
            fp.WriteByte((byte)((value & 0x000000ff)));
            fp.WriteByte((byte)((value & 0x0000ff00) >> 8));
            fp.WriteByte((byte)((value & 0x00ff0000) >> 16));
            fp.WriteByte((byte)((value & 0xff000000) >> 24));
        }

        public static void WriteString(FileStream fp, string value)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(value);
            fp.WriteByte((byte)buffer.Length);

            foreach (byte c in buffer)
            {
                fp.WriteByte(c);
            }
        }

        public static void WriteString2(FileStream fp, string value)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(value);
            WriteInt16(fp, (short)buffer.Length);

            foreach (byte c in buffer)
            {
                fp.WriteByte(c);
            }
        }

        public static void WriteString4(FileStream fp, string value)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(value);
            WriteInt32(fp, buffer.Length);

            foreach (byte c in buffer)
            {
                fp.WriteByte(c);
            }
        }

        public static bool ReadBool(FileStream fp)
        {
            int value = fp.ReadByte();

            if (value == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static byte ReadInt8(FileStream fp)
        {
            return (byte)fp.ReadByte();
        }

        public static short ReadInt16(FileStream fp)
        {
            byte[] data = new byte[2];
            fp.Read(data, 0, 2);

            short value = (short)((int)data[0] | ((int)data[1]) << 8);

            return value;
        }

        public static int ReadInt32(FileStream fp)
        {
            byte[] data = new byte[4];
            fp.Read(data, 0, 4);

            int value = (int)((int)data[0] | ((int)data[1]) << 8 | ((int)data[2]) << 16 | ((int)data[3]) << 24);

            return value;
        }

        public static string ReadString(FileStream fp)
        {
            int len = fp.ReadByte();
            byte[] buffer = new byte[len];

            fp.Read(buffer, 0, len);

            string value = Encoding.UTF8.GetString(buffer);

            return value;
        }
        public static string ReadString2(FileStream fp)
        {
            int len = (int)ReadInt16(fp);
            byte[] buffer = new byte[len];

            fp.Read(buffer, 0, len);

            string value = Encoding.UTF8.GetString(buffer);

            return value;
        }

        public static string ReadString4(FileStream fp)
        {
            int len = ReadInt32(fp);
            byte[] buffer = new byte[len];

            fp.Read(buffer, 0, len);

            string value = Encoding.UTF8.GetString(buffer);

            return value;
        }
    }
}
