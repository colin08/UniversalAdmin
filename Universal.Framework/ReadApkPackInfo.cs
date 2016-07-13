using Universal.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Web.Framework
{
    public class ReadApkPackInfo
    {
        private delegate void Action<in T1, in T2>(T1 a, T2 b);

        private static readonly Dictionary<String, Action<ApkPackInfo, String>> ActionMap;

        static ReadApkPackInfo()
        {
            ActionMap = new Dictionary<String, Action<ApkPackInfo, string>>
            {
                    {"package", ReadPackage},
                    {"sdkVersion", (i, s) => i.SdkVersion = GetPropertyInQuote(s)},
                    {"targetSdkVersion", (i, s) => i.TargetSdkVersion = GetPropertyInQuote(s)},
                    {"application-label", (i, s) => i.ApplicationLabel = GetPropertyInQuote(s)},
                    {"launchable-activity", (i, s) => i.LaunchableActivity = GetPropertyInQuote(s)},
                    {"uses-permission", (i, s) => i.UsesPermissions.Add(GetPropertyInQuote(s))},
                    {"application:", ReadApplicationIcon},
                    {"application-icon-", AddIcons},
                    {"uses-feature", (i, s) => i.Features.Add(GetPropertyInQuote(s))},
                    {"uses-implied-feature", AddFeature}
                };
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="path">AAPT.EXE文件的绝对路径</param>
        public ReadApkPackInfo(string path)
        {
            if (File.Exists(path)) return;
            lock (typeof(ReadApkPackInfo))
            {
                if (File.Exists(path)) return;
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    using (Stream ms = IOHelper.GetFileStream(typeof(ReadApkPackInfo).Assembly, "aapt.exe"))
                    {
                        const int bufferSize = 1024;
                        var buffer = new byte[bufferSize];
                        int readLength;
                        while ((readLength = ms.Read(buffer, 0, bufferSize)) > 0)
                        {
                            fs.Write(buffer, 0, readLength);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取APK信息
        /// </summary>
        /// <param name="apkPath">APK的绝对路径</param>
        /// <returns></returns>
        public ApkPackInfo GetApkInfo(string apkPath)
        {
            ApkPackInfo info;
            using (var p = new Process())
            {
                p.StartInfo = new ProcessStartInfo(System.AppDomain.CurrentDomain.BaseDirectory + "aapt.exe", String.Format(" d badging \"{0}\"", apkPath))
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };
                p.Start();
                StreamReader output = p.StandardOutput;
                string line = output.ReadLine();
                if (String.IsNullOrEmpty(line) || !line.StartsWith("package"))
                {
                    throw new ArgumentException("参数不正确，无法正常解析APK包。输出结果为：" + line + Environment.NewLine +
                                                output.ReadToEnd());
                }
                info = new ApkPackInfo();
                do
                {
                    if (String.IsNullOrEmpty(line)) continue;
                    foreach (var action in ActionMap)
                    {
                        if (line.StartsWith(action.Key, StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (line.Contains("application-label-") && !line.Contains("application-label-zh_CN")) //不获取除中文名字之外的名字
                                continue;
                            else
                            {
                                action.Value(info, line);
                            }
                        }
                    }
                } while (!String.IsNullOrEmpty(line = output.ReadLine()));
            }
            return info;
        }

        private static String GetPropertyInQuote(String source)
        {
            return source.Split(new[] { '\'' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }

        private static void ReadPackage(ApkPackInfo info, String line)
        {
            if (String.IsNullOrEmpty(line)) return;
            string[] arr = line.Split(new[] { ' ', '=', ':', '\'' }, StringSplitOptions.RemoveEmptyEntries);
            info.PackageName = arr[2];
            info.VersionCode = arr[4];
            info.VersionName = arr[6];
        }

        private static void ReadApplicationIcon(ApkPackInfo info, String line)
        {
            if (String.IsNullOrEmpty(line)) return;
            string[] arr = line.Split(new[] { '\'' }, StringSplitOptions.RemoveEmptyEntries);
            info.ApplicationIcon = arr[arr.Length - 1];
        }

        private static void AddIcons(ApkPackInfo info, String line)
        {
            //application-icon-160:'res/drawable/icon.png'
            if (String.IsNullOrEmpty(line)) return;
            string[] arr = line.Split(new[] { ':', '\'' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length != 2) return;
            info.ApplicationIcons.Add(arr[0], arr[1]);
        }

        private static void AddFeature(ApkPackInfo info, String line)
        {
            //uses-implied-feature:'android.hardware.screen.portrait','one or more activities have specified a portrait orientation'
            if (String.IsNullOrEmpty(line)) return;
            string[] arr = line.Split(new[] { ':', '\'', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length != 3) return;
            var f = new ApkImpliedFeature(arr[1], arr[2]);
            info.ImpliedFeatures.Add(f);
        }
    }

    /// <summary>
    /// APK包的实体信息
    /// </summary>
    public class ApkPackInfo
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public ApkPackInfo()
        {
            UsesPermissions = new List<string>();
            ApplicationIcons = new Dictionary<string, string>();
            ImpliedFeatures = new List<ApkImpliedFeature>();
            Features = new List<string>();
        }

        /// <summary>
        /// 文件的大小，后期添加 byte
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件的MD5值，后期添加
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// 获取或设置 内部版本号
        /// </summary>
        public String VersionCode { get; set; }

        /// <summary>
        /// 获取或设置 外部版本号
        /// </summary>
        public String VersionName { get; set; }

        /// <summary>
        /// 获取或设置 包名
        /// </summary>
        public String PackageName { get; set; }

        /// <summary>
        /// 获取或设置 所需要的权限
        /// </summary>
        public List<String> UsesPermissions { get; set; }

        /// <summary>
        /// 获取或设置 支持的SDK版本
        /// </summary>
        public String SdkVersion { get; set; }

        /// <summary>
        /// 获取或设置 建议的SDK版本
        /// </summary>
        public String TargetSdkVersion { get; set; }

        /// <summary>
        /// 获取或设置 应用程序名
        /// </summary>
        public String ApplicationLabel { get; set; }

        /// <summary>
        /// 获取或设置 各个分辨率下的图标路径
        /// </summary>
        public Dictionary<String, String> ApplicationIcons { get; set; }

        /// <summary>
        /// 获取或设置 程序的图标
        /// </summary>
        public String ApplicationIcon { get; set; }

        /// <summary>
        /// 获取或设置 暗指的特性
        /// </summary>
        public List<ApkImpliedFeature> ImpliedFeatures { get; set; }

        /// <summary>
        /// 获取或设置 所需设备特性
        /// </summary>
        public List<String> Features { get; set; }

        /// <summary>
        /// 获取或设置 启动界面
        /// </summary>
        public String LaunchableActivity { get; set; }

        public override string ToString()
        {
            return "ApkInfo [VersionCode=" + VersionCode + ",\n VersionName="
                   + VersionName + ",\n PackageName=" + PackageName
                   + ",\n UsesPermissions="
                   + UsesPermissions.Count + ",\n SdkVersion=" + SdkVersion
                   + ",\n TargetSdkVersion=" + TargetSdkVersion
                   + ",\n ApplicationLabel=" + ApplicationLabel
                   + ",\n ApplicationIcons=" + ApplicationIcons.Count
                   + ",\n ApplicationIcon=" + ApplicationIcon
                   + ",\n ImpliedFeatures=" + ImpliedFeatures.Count + ",\n Features="
                   + Features.Count + ",\n LaunchableActivity=" + LaunchableActivity + "\n]";
        }
    }

    /// <summary>
    /// Apk文件特性
    /// </summary>
    public class ApkImpliedFeature
    {
        public ApkImpliedFeature(String feature, String implied)
        {
            Feature = feature;
            Implied = implied;
        }

        /// <summary>
        /// 获取或设置 设备特性名称
        /// </summary>
        public String Feature { get; set; }

        /// <summary>
        /// 获取或设置 所需特性的内容
        /// </summary>
        public String Implied { get; set; }

        public override string ToString()
        {
            return String.Format("Feature [feature={0}, implied={1}]", Feature, Implied);
        }
    }
}
