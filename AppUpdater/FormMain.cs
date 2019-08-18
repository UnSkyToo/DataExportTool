using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AppUpdater
{
    public partial class FormMain : Form
    {
        public static string ClientAppName { get; set; } = "DataExportTool";
        public static string UpdaterHostName { get; set; } = "DESKTOP-9J05AKF";//"DESKTOP-LITE";
        public static string UpdaterUrl { get; set; } = "http://{0}:8080/lite/dataexporttool/";
        public static string ClientVersion { get; set; } = "1.0.0";
        public static bool NeedNotifyMsg { get; set; } = true;

        private readonly Dictionary<string, string> VersionInfo_ = new Dictionary<string, string>();
        private readonly Timer DownloadTimer_ = new Timer();

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object Sender, EventArgs Args)
        {
            DownloadTimer_.Interval = 50;
            DownloadTimer_.Tick += DownloadTimerTicker;
            DownloadTimer_.Start();

            GetUpdateServerIP();
        }

        private void KillApp(string AppName)
        {
            if (string.IsNullOrWhiteSpace(AppName))
            {
                return;
            }

            var Processes = Process.GetProcessesByName(AppName);
            foreach (var App in Processes)
            {
                if (!App.HasExited)
                {
                    App.Kill();
                }
            }
        }

        private void SetInfo(string Text)
        {
            if (LabelInfo.InvokeRequired)
            {
                LabelInfo.Invoke((Action<string>)((Args) => { LabelInfo.Text = Args; }), Text);
            }
            else
            {
                LabelInfo.Text = Text;
            }
        }

        private void SetProgress(long Cur, long Max)
        {
            if (ProgressBarPercent.InvokeRequired)
            {
                ProgressBarPercent.Invoke((Action<long, long>)((ACur, AMax) =>
                {
                    ProgressBarPercent.Maximum = (int)AMax;
                    ProgressBarPercent.Value = (int)ACur;
                }));
            }
            else
            {
                ProgressBarPercent.Maximum = (int)Max;
                ProgressBarPercent.Value = (int)Cur;
            }
        }

        private int CompareVersion(string Left, string Right)
        {
            var LeftCode = Left.Split('.');
            var RightCode = Right.Split('.');
            var LeftValue = int.Parse(LeftCode[0]) * 1000000 + int.Parse(LeftCode[1]) * 1000 + int.Parse(LeftCode[2]);
            var RightValue = int.Parse(RightCode[0]) * 1000000 + int.Parse(RightCode[1]) * 1000 + int.Parse(RightCode[2]);

            if (LeftValue < RightValue)
            {
                return -1;
            }

            if (LeftValue > RightValue)
            {
                return 1;
            }

            return 0;
        }

        private void GetUpdateServerIP()
        {
            SetInfo("获取更新服务器地址...");
            Dns.BeginGetHostAddresses(UpdaterHostName, (AR) =>
            {
                var IPList = Dns.EndGetHostAddresses(AR);

                foreach (var IP in IPList)
                {
                    if (IP.AddressFamily == AddressFamily.InterNetwork)
                    {
                        UpdaterUrl = string.Format(UpdaterUrl, IP.ToString());
                        RequestVersionInfo();
                        return;
                    }
                }

                UpdateDone();
            }, null);
        }

        private void RequestVersionInfo()
        {
            SetInfo("获取新版本信息中...");

            var Req = new RequestItem($"{UpdaterUrl}version.txt", 10);
            Req.Get(null);

            if (Req.Data == null && Req.Data.Count == 0)
            {
                UpdateDone();
                return;
            }

            try
            {
                var VersionInfoText = Encoding.Default.GetString(Req.Data.ToArray()).Replace("\r\n", "\n");
                VersionInfo_.Clear();
                foreach (var Line in VersionInfoText.Split('\n'))
                {
                    VersionInfo_.Add(Line.Split(':')[0], Line.Split(':')[1]);
                }
            }
            catch
            {
                UpdateDone();
            }

            if (CompareVersion(ClientVersion, VersionInfo_["version"]) == -1)
            {
                BeginUpdate();
            }
            else
            {
                UpdateDone();
            }
        }

        private void BeginUpdate()
        {
            var NoticeInfo = $"版本:{VersionInfo_["version"]}\n大小:{int.Parse(VersionInfo_["size"]) / 1024}KB\n说明:\n{VersionInfo_["msg"]}";
            NoticeInfo = NoticeInfo.Replace('`', '\n');

            if (MessageBox.Show(NoticeInfo, "发现新版本，是否更新？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DownloadPatch();
            }
            else
            {
                KillApp("AppUpdater");
            }
        }

        private void DownloadPatch()
        {
            SetInfo("下载热更包...");

            var Task = Downloader.Start($"{UpdaterUrl}{VersionInfo_["patch"]}", $"{AppDomain.CurrentDomain.BaseDirectory}{VersionInfo_["patch"]}", VersionInfo_["md5"], true);
            Task.Progress += OnDownloadProgress;
            Task.Completed += OnDownloadCompleted;
        }

        private void DownloadTimerTicker(object Sender, EventArgs Args)
        {
            Downloader.Update(0.05f);
        }

        private void OnDownloadProgress(string FileName, string StageName, long CurSize, long MaxSie, long Speed)
        {
            SetInfo($"下载 {FileName} {StageName} {CurSize}/{MaxSie} {Speed/1024}KB/s");
            SetProgress(CurSize, MaxSie);
        }

        private void OnDownloadCompleted(string FileName, string Info, bool Succeeded)
        {
            if (Succeeded)
            {
                UnZip();
            }
            else
            {
                MessageBox.Show("更新失败");
                KillApp("AppUpdater");
            }
        }

        private void UnZip()
        {
            SetInfo("解压中...");
            try
            {
                KillApp(ClientAppName);
                System.Threading.Thread.Sleep(500);
                ZipUtil.UnZipFile($"{AppDomain.CurrentDomain.BaseDirectory}{VersionInfo_["patch"]}", AppDomain.CurrentDomain.BaseDirectory, string.Empty, true);
                File.Delete($"{AppDomain.CurrentDomain.BaseDirectory}{VersionInfo_["patch"]}");
                Process.Start($"{ClientAppName}.exe");
                UpdateDone();
            }
            catch (Exception Ex)
            {
                SetInfo(Ex.Message);
            }
        }

        private void UpdateDone()
        {
            if (NeedNotifyMsg)
            {
                MessageBox.Show("当前版本已更新到最新", ClientAppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            KillApp("AppUpdater");
        }
    }
}