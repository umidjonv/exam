using System;
using System.IO;
using System.Windows.Forms;

namespace EX.Desktop
{
    public static class AppSettings
    {

        public const string AppName = "ExamDesktop";

        public const string IdUrl = "http://sp-id.uzex.uz";

        public static readonly string ApiUrl = "http://sp-exam-api.uzex.uz";

        public static readonly string AppUrl = "http://sp-exam.uzex.uz";

        public const int SpeedTime = 5;

        public const int LanLimit = 0;

        public const int DelayTime = 1000;

        public static readonly TimeSpan SyncTime = TimeSpan.FromMinutes(1);

        public static string AppRoot => Path.GetDirectoryName(Application.ExecutablePath);

    }
}