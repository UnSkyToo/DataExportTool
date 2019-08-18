using System;
using System.Windows.Forms;

namespace AppUpdater
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main(string[] Args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Args.Length == 0)
            {
                Usage();
                return;
            }

            ParseArgs(Args);
            Application.Run(new FormMain());
        }

        private static void Usage()
        {
            Console.WriteLine("AppUpdater");
            Console.WriteLine("用法：AppUpdater [-option] NeedNotify");
            Console.WriteLine("");
            Console.WriteLine("其中选项包括：");
            Console.WriteLine("  -c\t指定客户端进程名称");
            Console.WriteLine("  -n\t指定更新服务器HostName");
            Console.WriteLine("  -r\t指定更新服务器Url地址");
            Console.WriteLine("  -v\t指定客户端版本号");
            Console.WriteLine("  -h\t帮助");
        }

        private static void ParseArgs(string[] Args)
        {
            try
            {
                for (var Index = 0; Index < Args.Length;)
                {
                    if (Args[Index].StartsWith("-"))
                    {
                        switch (Args[Index])
                        {
                            case "-c":
                                FormMain.ClientAppName = Args[Index + 1];
                                Index += 2;
                                break;
                            case "-n":
                                FormMain.UpdaterHostName = Args[Index + 1];
                                Index += 2;
                                break;
                            case "-r":
                                FormMain.UpdaterUrl = Args[Index + 1];
                                Index += 2;
                                break;
                            case "-v":
                                FormMain.ClientVersion = Args[Index + 1];
                                Index += 2;
                                break;
                            case "-h":
                                Usage();
                                Index++;
                                break;
                            default:
                                Index++;
                                break;

                        }
                    }
                    else
                    {
                        Index++;
                    }
                }

                if (Args.Length > 0)
                {
                    bool.TryParse(Args[Args.Length - 1], out var Notify);
                    FormMain.NeedNotifyMsg = Notify;
                }
            }
            catch
            {
                throw new Exception("参数错误");
            }
        }
    }
}