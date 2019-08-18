using System;
using System.Drawing;
using System.Windows.Forms;

namespace DataExportTool
{
    public static class Logger
    {
        public static RichTextBox Rich { get; set; }

        private static void AppendToRichText(string Msg, Color ForeColor)
        {
            if (Rich.InvokeRequired)
            {
                Action<string, Color> ActDelegate = (AMsg, AColor) =>
                {
                    var Index = Rich.TextLength;
                    Rich.AppendText($"{AMsg}\n");
                    Rich.SelectionStart = Index;
                    Rich.SelectionLength = Rich.TextLength - Index;
                    Rich.SelectionColor = AColor;
                    Rich.ScrollToCaret();
                };

                Rich.Invoke(ActDelegate, Msg, ForeColor);
            }
            else
            {
                var Index = Rich.TextLength;
                Rich.AppendText($"{Msg}\n");
                Rich.SelectionStart = Index;
                Rich.SelectionLength = Rich.TextLength - Index;
                Rich.SelectionColor = ForeColor;
                Rich.ScrollToCaret();
            }
        }

        public static void Info(string Msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Msg);

            AppendToRichText(Msg, Color.White);
        }

        public static void Info(string Format, params object[] Args)
        {
            Info(string.Format(Format, Args));
        }

        public static void Warning(string Msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Msg);
            Console.ForegroundColor = ConsoleColor.White;

            AppendToRichText(Msg, Color.Yellow);
        }

        public static void Warning(string Format, params object[] Args)
        {
            Warning(string.Format(Format, Args));
        }

        public static void Error(string Msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Msg);
            Console.ForegroundColor = ConsoleColor.White;

            AppendToRichText(Msg, Color.Red);
        }

        public static void Error(string Format, params object[] Args)
        {
            Error(string.Format(Format, Args));
        }
    }
}