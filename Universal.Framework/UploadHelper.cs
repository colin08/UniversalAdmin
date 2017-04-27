using Universal.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;

namespace Universal.Web.Framework
{
    /// <summary>
    /// 文件上传帮助类
    /// </summary>
    public class UploadHelper
    {
        /// <summary>
        /// 上传文件-处理base64位图片数据的方法
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="save_path"></param>
        /// <returns></returns>
        public WebAjaxEntity<string> Upload_For_Base64Data(string base64, string ServerPath)
        {
            WebAjaxEntity<string> response_entity = new WebAjaxEntity<string>();
            response_entity.msg = 0;

            if (string.IsNullOrWhiteSpace(base64) || string.IsNullOrWhiteSpace(ServerPath))
            {
                response_entity.msgbox = "缺少参数";
                return response_entity;
            }

            //保存的绝对路径，不包含文件名
            string filepath_no_name = IOHelper.GetMapPath(ServerPath);

            try
            {
                string guid = Guid.NewGuid().ToString();
                string saveName = guid;// +".JPG"; // 保存文件名称
                string file_ext = "";
                string file_path = IOHelper.Base64ToImgSave(base64, ServerPath, saveName, out file_ext);

                if (string.IsNullOrWhiteSpace(file_path) || string.IsNullOrWhiteSpace(file_ext))
                {
                    response_entity.msgbox = "文件保存失败";
                    return response_entity;
                }

                saveName = saveName + "." + file_ext;

                //保存后文件的绝对路径，包括文件名
                string filePath = IOHelper.GetMapPath(file_path);

                //获取MD5值
                string md5 = IOHelper.GetMD5HashFromFile(filePath);
                if (System.IO.File.Exists(filepath_no_name + md5 + "." + file_ext))
                {
                    System.IO.File.Delete(filePath); //把刚刚上传的给删掉，只用原有的文件
                }
                else //不存在，改名为md5值保存
                {
                    System.IO.File.Move(filePath, filepath_no_name + md5 + "." + file_ext); //给文件改名
                }
                response_entity.msg = 1;
                response_entity.msgbox = "上传成功";
                response_entity.data = ServerPath + md5 + "." + file_ext;
                return response_entity;
            }
            catch (Exception ex)
            {
                response_entity.msgbox = ex.Message;
                return response_entity;
            }

        }

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="fileData">/uploads/avatar/  结尾必须有“/”</param>
        /// <param name="ServerPath">保存的文件地址相对路径</param>
        /// <param name="IsThumb">是否生成缩略图</param>
        /// <returns></returns>
        public WebAjaxEntity<String> Upload(HttpPostedFileBase fileData, string ServerPath, bool IsThumb = true)
        {

            WebAjaxEntity<string> response_entity = new WebAjaxEntity<string>();
            response_entity.msg = 0;

            if (fileData == null)
            {
                response_entity.msgbox = "请选择要上传的文件";
                return response_entity;
            }

            //保存的目录
            string filePath = IOHelper.GetMapPath(ServerPath);
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            try
            {
                //int file_size = fileData.ContentLength; //获取文件大小 KB
                string fileExt = IOHelper.GetFileExt(fileData.FileName).ToLower(); //文件扩展名，不含“.”
                //if (!CheckFileExt("*." + fileExt, file_type))
                //{
                //    response_entity.msgbox = "非法的文件格式";
                //    return response_entity;
                //}

                //if (!CheckFileSize(fileExt, file_size,file_type))
                //{
                //    response_entity.msgbox = "文件大小超出限制";
                //    return response_entity;
                //}
                string guid = Guid.NewGuid().ToString();
                string saveName = guid + "." + fileExt; // 保存文件名称
                fileData.SaveAs(filePath + saveName);

                //if (file_type == SiteEnums.UploadType.image)
                //{
                //如果是上传图片
                //if (file_size > SiteConfig.ImgSize * 1024)
                //{ 
                //    //如果超出了限制，则进行压缩
                //    ImgThumbnail ya = new ImgThumbnail();
                //    //获取图片的宽高
                //    Hashtable hash =IOHelper.GetImageWidthHeight(filePath + saveName);
                //    //临时文件名
                //    string saveTempName = guid+"_temp."+fileExt;
                //    if (ya.Thumbnail(filePath + saveName, filePath + saveTempName, TypeHelper.ObjectToInt(hash["w"]), TypeHelper.ObjectToInt(hash["h"]), 20, ImgThumbnail.ImgThumbnailType.H))
                //    {
                //        //压缩成功,删掉大文件
                //        IOHelper.DeleteIOFile(filePath + saveName);
                //        saveName = saveTempName;//把文件指向新压缩的文件
                //    }
                //    else
                //    {
                //        response_entity.msgbox = "图片太大，压缩失败";
                //        return response_entity;
                //    }
                //}
                //}
                //获取MD5值
                string md5 = IOHelper.GetMD5HashFromFile(filePath + saveName);
                if (System.IO.File.Exists(filePath + md5 + "." + fileExt))
                {
                    System.IO.File.Delete(filePath + saveName); //把刚刚上传的给删掉，只用原有的文件
                }
                else //不存在，改名为md5值保存
                {
                    System.IO.File.Move(filePath + saveName, filePath + md5 + "." + fileExt); //给文件改名
                }

                //if (IsThumb)
                //{
                //    string[] img_arr = { "jpg", "jpeg", "bmp", "png" };
                //    if (img_arr.Contains(fileExt))
                //    {
                //        //如果是图片，就压缩
                //        IOHelper.GenerateThumb_ME(IOHelper.GetMapPath(ServerPath + md5 + "." + fileExt));
                //    }
                //}

                response_entity.msg = 1;
                response_entity.msgbox = "上传成功";
                response_entity.data = ServerPath + md5 + "." + fileExt;
                return response_entity;
            }
            catch (Exception ex)
            {
                response_entity.msgbox = ex.Message;
                return response_entity;
            }

        }

        /// <summary>
        /// 压缩包上传
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="ServerPath"></param>
        /// <returns></returns>
        public WebAjaxEntity<string> Upload_Zip(HttpPostedFileBase fileData, string ServerPath)
        {
            WebAjaxEntity<string> response_entity = new WebAjaxEntity<string>();
            response_entity.msg = 0;

            if (fileData == null)
            {
                response_entity.msgbox = "请选择要上传的文件";
                return response_entity;
            }

            //保存的目录
            string filePath = IOHelper.GetMapPath(ServerPath);
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            try
            {
                int file_size = fileData.ContentLength; //获取文件大小 KB
                string fileExt = IOHelper.GetFileExt(fileData.FileName); //文件扩展名，不含“.”
                //if (!CheckFileExt("*." + fileExt, SiteEnums.UploadType.zip))
                //{
                //    response_entity.msgbox = "只允上传*.zip格式的文件";
                //    return response_entity;
                //}

                //if (!CheckFileSize(fileExt, file_size, SiteEnums.UploadType.zip))
                //{
                //    response_entity.msgbox = "文件大小超出限制";
                //    return response_entity;
                //}

                string saveName = Guid.NewGuid().ToString() + "." + fileExt; // 保存文件名称
                fileData.SaveAs(filePath + saveName);


                string md5 = IOHelper.GetMD5HashFromFile(filePath + saveName);
                if (Directory.Exists(filePath + md5 + "/"))
                {
                    System.IO.File.Delete(filePath + saveName); //把刚刚上传的给删掉，使用原来解压的文件

                }
                else //不存在，改名为md5值保存
                {
                    Directory.CreateDirectory(filePath + md5 + "/"); //创建解压后保存的目录
                    System.IO.File.Move(filePath + saveName, filePath + md5 + "." + fileExt); //给文件改名

                    try
                    {
                        //解压操作
                        string glassDir = filePath;
                        FastZip fastZip = new FastZip();
                        fastZip.ExtractZip(filePath + md5 + "." + fileExt, filePath + md5 + "/", string.Empty);
                    }
                    catch
                    {
                        //删除刚刚创建的目录
                        Directory.Delete(filePath + md5 + "/");
                        response_entity.msgbox = "解压失败";
                        return response_entity;
                    }

                }

                System.IO.File.Delete(filePath + md5 + "." + fileExt); //把刚刚上传的ZIP文件给删掉


                response_entity.msg = 1;
                response_entity.msgbox = "上传成功";
                response_entity.data = ServerPath + md5 + "/";
                return response_entity;
            }
            catch (Exception ex)
            {
                response_entity.msgbox = ex.Message;
                return response_entity;
            }
        }

        /// <summary>
        /// 上传APK
        /// </summary>
        /// <returns></returns>
        public Hashtable Upload_APK(HttpPostedFileBase fileData)
        {
            Hashtable hash = new Hashtable();
            if (fileData == null)
            {
                hash["msg"] = 0;
                hash["msgbox"] = "请上传文件";
                return hash;
            }
            string ServerPath = "/uploads/apk/";
            try
            {
                string OldfileName = fileData.FileName;
                int file_size = fileData.ContentLength; //获取文件大小 KB
                string fileExt = IOHelper.GetFileExt(fileData.FileName); //文件扩展名，不含“.”
                if (fileExt.ToLower() != "apk")
                {
                    hash["msg"] = 0;
                    hash["msgbox"] = "只允许上传APK格式的文件";
                    return hash;
                }
                string filePath = IOHelper.GetMapPath(ServerPath);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string saveName = Guid.NewGuid().ToString() + "." + fileExt; // 保存文件名称
                fileData.SaveAs(filePath + saveName);
                //获取APK的MD5值
                string md5 = IOHelper.GetMD5HashFromFile(filePath + saveName);

                //校验文件是否存在
                if (System.IO.File.Exists(filePath + md5 + "." + fileExt))
                {
                    System.IO.File.Delete(filePath + saveName); //把刚刚上传的给删掉，只用原有的文件

                }
                else //不存在，改名为md5值保存
                {
                    System.IO.File.Move(filePath + saveName, filePath + md5 + "." + fileExt); //给文件改名
                }

                //解析APK包
                ApkPackInfo ApkInfoModel = GetAPKInfo(filePath + md5 + ".apk", md5);
                if (ApkInfoModel == null)
                {
                    hash["msg"] = 0;
                    hash["msgbox"] = "解析APK文件出错";
                    return hash;
                }
                else
                {
                    hash["msg"] = 1;
                    hash["msgbox"] = "APK处理成功";
                    hash["size"] = file_size;
                    hash["md5"] = ApkInfoModel.MD5;
                    hash["version"] = ApkInfoModel.VersionName;
                    hash["version_code"] = ApkInfoModel.VersionCode;
                    hash["down_url"] = ServerPath + md5 + ".apk";
                    hash["logo_img"] = ServerPath + md5 + ".image";
                    return hash;
                }
            }
            catch (Exception ex)
            {
                IOHelper.WriteLogs(ex.StackTrace);
                hash["msg"] = 0;
                hash["msgbox"] = ex.Message;
                return hash;
            }

        }

        #region 私有方法


        /// <summary>
        /// 解析APK包
        /// </summary>
        /// <param name="apk_path">APK的绝对路径</param>
        /// <param name="md5">APK的MD5值，用作icon的名字</param>
        /// <returns></returns>
        private ApkPackInfo GetAPKInfo(string apk_path, string md5)
        {
            ApkPackInfo model = new ApkPackInfo();
            try
            {
                string aaptFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "aapt.exe");
                string icon_path = IOHelper.GetMapPath("/uploads/apk/");
                if (!Directory.Exists(icon_path))
                {
                    Directory.CreateDirectory(icon_path);
                }
                string default_icon_name = md5 + ".image";
                model = new ReadApkPackInfo(aaptFilePath).GetApkInfo(apk_path);
                model.MD5 = md5;
                //获取ICON
                IOHelper.UnZipPath(apk_path, icon_path, model.ApplicationIcon, default_icon_name);
            }
            catch (Exception ex)
            {
                IOHelper.WriteLogs("解析apk异常："+ex.Message);
                IOHelper.DeleteFile(apk_path); //删除APK
            }
            return model;
        }

        ///// <summary>
        ///// 检查文件格式是否合法
        ///// </summary>
        ///// <param name="_fileExt">*.xxx</param>
        ///// <returns></returns>
        //private bool CheckFileExt(string _fileExt, SiteEnums.UploadType file_type)
        //{
        //    bool isOK = false;
        //    string[] file_ext = new string[] { };
        //    switch (file_type)
        //    {

        //        case SiteEnums.UploadType.image://只允许上传图片
        //            file_ext = SiteConfig.Picfileextension.Split(';');
        //            break;
        //        case SiteEnums.UploadType.zip: //只允许上传zip格式的压缩包
        //            file_ext = new string[] { "*.zip" };
        //            break;
        //        case SiteEnums.UploadType.apk: //只允许上传APK
        //            file_ext = new string[] { "*.apk" };
        //            break;
        //        default:
        //            break;
        //    }
        //    foreach (string item in file_ext)
        //    {
        //        if (item.ToLower().Replace(" ", "") == _fileExt.ToLower().Replace(" ", ""))
        //        {
        //            isOK = true;
        //            break;
        //        }
        //    }
        //    return isOK;
        //}

        ///// <summary>
        ///// 检查文件大小是否合法
        ///// </summary>
        ///// <param name="_fileExt">文件扩展名，不含“.”</param>
        ///// <param name="_fileSize">文件大小(B)</param>
        //private bool CheckFileSize(string _fileExt, int _fileSize, SiteEnums.UploadType file_type)
        //{
        //    int maxSize = 0;

        //    switch (file_type)
        //    {
        //        case SiteEnums.UploadType.image:
        //            maxSize = SiteConfig.ImgSize;
        //            break;
        //        case SiteEnums.UploadType.zip:
        //            maxSize = SiteConfig.ZipSize;
        //            break;
        //        case SiteEnums.UploadType.apk:
        //            maxSize = SiteConfig.APKSize;
        //            break;
        //        default:
        //            break;
        //    }

        //    if (maxSize < 0)
        //        maxSize = 0;

        //    if (maxSize == 0)
        //        return true;

        //    if (_fileSize > maxSize * 1024)
        //        return false;
        //    else
        //        return true;
        //}
        #endregion
    }
}
