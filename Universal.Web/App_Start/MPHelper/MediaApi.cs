using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.Containers;
using Universal.Tools;

namespace Universal.Web.MPHelper
{
    public class MediaApi
    {
        /// <summary>
        /// 站点配置文件
        /// </summary>
        private static WebSiteModel WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);

        /// <summary>
        /// 下载微信图片
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static string DownloadImage(string mediaId)
        {
            var accessToken = AccessTokenContainer.GetAccessToken(WebSite.WeChatAppID);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                Senparc.Weixin.MP.AdvancedAPIs.MediaApi.Get(accessToken, mediaId, ms);
                if (ms.Length == 0) return "";

                //保存到文件
                string file_folder = "/uploads/mpimg/";
                string file_io_folder = IOHelper.GetMapPath(file_folder);
                if (!System.IO.Directory.Exists(file_io_folder)) System.IO.Directory.CreateDirectory(file_io_folder);

                string file_server_path = file_folder + DateTime.Now.ToFileTime() + ".jpg";
                string file_io_path = IOHelper.GetMapPath(file_server_path);

                using (System.IO.FileStream fs = new System.IO.FileStream(file_io_path, System.IO.FileMode.Create))
                {
                    ms.Position = 0;
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        fs.Write(buffer, 0, bytesRead);
                    }
                    fs.Flush();
                }

                return file_server_path;
            }
        }


        /// <summary>
        /// 下载微信语音
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static string DownloadVoice(string mediaId)
        {
            var accessToken = AccessTokenContainer.GetAccessToken(WebSite.WeChatAppID);
            string mp4_path = "";
            string amr_path = "";
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                Senparc.Weixin.MP.AdvancedAPIs.MediaApi.Get(accessToken, mediaId, ms);
                if (ms.Length == 0) return "";

                //保存到文件
                string file_folder = "/uploads/voice/";
                string file_io_folder = IOHelper.GetMapPath(file_folder);
                if (!System.IO.Directory.Exists(file_io_folder)) System.IO.Directory.CreateDirectory(file_io_folder);

                string file_server_path = file_folder + DateTime.Now.ToFileTime() + ".amr";
                string file_io_path = IOHelper.GetMapPath(file_server_path);
                mp4_path = file_folder + DateTime.Now.ToFileTime() + ".mp4";
                using (System.IO.FileStream fs = new System.IO.FileStream(file_io_path, System.IO.FileMode.Create))
                {
                    ms.Position = 0;
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        fs.Write(buffer, 0, bytesRead);
                    }
                    fs.Flush();
                }

                amr_path = file_server_path;
            }
            //转换为MP4格式
            ConverAMRToMP4 converAMRToMP4 = new ConverAMRToMP4();
            converAMRToMP4.Convert(amr_path, mp4_path);
            if (!IOHelper.FileExists(mp4_path))
            {
                System.Diagnostics.Trace.WriteLine("AMR转换MP4失败：" + amr_path);
                return "";
            }
            else
            {
                //转换完成后删除amr文件
                IOHelper.DeleteFile(amr_path);
                return mp4_path;
            }

        }



    }

    public class ConverAMRToMP4
    {
        /// <summary>
        /// amr格式转换为mp4
        /// </summary>
        /// <returns></returns>
        public void Convert(string amr_path, string mp4_path)
        {
            string ffmpeg_path = "/ffmpeg.exe";
            if (!IOHelper.FileExists(ffmpeg_path)) return;
            if (!IOHelper.FileExists(amr_path)) return;
            try
            {
                string cmd_comm = IOHelper.GetMapPath(ffmpeg_path) + " -y -i " + IOHelper.GetMapPath(amr_path) + " -ar 8000 -ab 12.2k -ac 1 " + IOHelper.GetMapPath(mp4_path);
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.Start();
                process.StandardInput.WriteLine(cmd_comm);
                process.StandardInput.AutoFlush = true;
                System.Threading.Thread.Sleep(1000);
                process.StandardInput.WriteLine("exit");
                process.WaitForExit();
            }
            catch
            { }

        }
    }

}