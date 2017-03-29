using System;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;

namespace Universal.Tools
{
    /// <summary>
    /// IO帮助类
    /// </summary>
    public class IOHelper
    {
        //是否已经加载了JPEG编码解码器
        private static bool _isloadjpegcodec = false;
        //当前系统安装的JPEG编码解码器
        private static ImageCodecInfo _jpegcodec = null;


        /// <summary>
        /// 获取图片的宽高,返回HashTable，宽的键:w,高的键:h
        /// </summary>
        /// <param name="file_path">绝对路径</param>
        /// <returns></returns>
        public static Hashtable GetImageWidthHeight(string file_path)
        {
            Hashtable hash = new Hashtable();
            System.Drawing.Image temp_image = System.Drawing.Image.FromFile(file_path);
            hash["w"] = temp_image.Width;
            hash["h"] = temp_image.Height;
            temp_image.Dispose();
            return hash;
        }

        /// <summary>
        /// 自定义生成缩略图
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="thumbPath">缩略图路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>   
        public static void GenerateThumb_ME(string imagePath)
        {
            Image image = Image.FromFile(imagePath);
            
            string extension = imagePath.Substring(imagePath.LastIndexOf(".")).ToLower();
            ImageFormat imageFormat = null;
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".bmp":
                    imageFormat = ImageFormat.Bmp;
                    break;
                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;
                case ".gif":
                    imageFormat = ImageFormat.Gif;
                    break;
                default:
                    imageFormat = ImageFormat.Jpeg;
                    break;
            }

            string save_folder = imagePath.Substring(0, imagePath.LastIndexOf('\\')) + "\\"; //保存的路径
            string save_name = imagePath.Substring(imagePath.LastIndexOf('\\') + 1, imagePath.LastIndexOf('.') - imagePath.LastIndexOf('\\') - 1) + "_thumb"; //保存的文件名，不包含拓展名
            string save_io_path = save_folder + save_name + extension;//保存的最终路径

            int toWidth = image.Width / 3;
            int toHeight = image.Height;

            int x = 0;
            int y = 0;
            int ow = image.Width;
            int oh = image.Height;

            //款为原来的 1/3
            toHeight = image.Height * toWidth / image.Width;
            
            //新建一个bmp
            Image bitmap = new Bitmap(toWidth, toHeight);

            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(image,
                        new Rectangle(0, 0, toWidth, toHeight),
                        new Rectangle(x, y, ow, oh),
                        GraphicsUnit.Pixel);
            g.Dispose();

            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = 60;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int i = 0; i < arrayICI.Length; i++)
                {
                    if (arrayICI[i].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[i];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    bitmap.Save(save_io_path, jpegICIinfo, ep);
                }
                else
                    bitmap.Save(save_io_path, imageFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (g != null)
                    g.Dispose();
                if (bitmap != null)
                    bitmap.Dispose();
                if (image != null)
                    image.Dispose();
            }
        }


        /// <summary>
        /// 获得文件物理路径
        /// </summary>
        /// <returns></returns>
        public static string GetMapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            else
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(path);
            }
        }

        /// <summary>
        /// 保存BAse64位的图片，返回相对路径
        /// <param name="file_path">文件保存的相对路径</param>
        /// <param name="ImageText">BASE64数据</param>
        /// <param name="file_name">文件名，不包括后缀，保存为PNG类型的图片</param>
        /// </summary>
        /// <returns></returns>
        public static string Base64ToImgSave(string ImageText, string file_path, string file_name,out string file_ext)
        {
            file_ext = "";   

            try
            {
                string ext_reg = "data:image/(?<EXT>[^<].*?);base64";
                string[] ext_list = GetRegValue(ImageText, ext_reg, "EXT", true);
                if (ext_list.Length != 1)
                {
                    return null;
                }
                file_ext = ext_list[0].ToString();
                string replace_text = "data:image/" + file_ext + ";base64,";
                ImageText = ImageText.Replace(replace_text,"");

                file_name = file_name+ "." + file_ext;//重新拼接

                Byte[] bitmapData = new Byte[ImageText.Length];
                bitmapData = Convert.FromBase64String(ImageText);
                System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);
                System.Drawing.Image image = Image.FromStream(streamBitmap);
                if (!Directory.Exists(GetMapPath(file_path)))//如果不存在该路径，则创建
                    Directory.CreateDirectory(GetMapPath(file_path));
                image.Save(GetMapPath(file_path) + file_name, GetImageFormat(file_ext));
                return file_path + file_name;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("保存BASE64位数据错误，错误原因：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据图片后缀返回后缀实例类型
        /// </summary>
        /// <param name="file_ext"></param>
        /// <returns></returns>
        public static System.Drawing.Imaging.ImageFormat GetImageFormat(string file_ext)
        {
            switch (file_ext.ToLower())
            {
                case "bmp":
                    return System.Drawing.Imaging.ImageFormat.Bmp;
                case "gif":
                    return System.Drawing.Imaging.ImageFormat.Gif;
                case "jpg":
                case "jpeg":
                    return System.Drawing.Imaging.ImageFormat.Jpeg;
                case "png":
                    return System.Drawing.Imaging.ImageFormat.Png;
                default:
                    return System.Drawing.Imaging.ImageFormat.Jpeg;
            }
        }

        /// <summary>
        /// 正则表达式取值
        /// </summary>
        /// <param name="HtmlCode">源码</param>
        /// <param name="RegexString">正则表达式</param>
        /// <param name="GroupKey">正则表达式分组关键字</param>
        /// <param name="RightToLeft">是否从右到左</param>
        /// <returns></returns>
        public static string[] GetRegValue(string HtmlCode, string RegexString, string GroupKey, bool RightToLeft)
        {
            MatchCollection m;
            Regex r;
            if (RightToLeft == true)
            {
                r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.RightToLeft);
            }
            else
            {
                r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
            m = r.Matches(HtmlCode);
            string[] MatchValue = new string[m.Count];
            for (int i = 0; i < m.Count; i++)
            {
                MatchValue[i] = m[i].Groups[GroupKey].Value;
            }
            return MatchValue;
        }


        
        /// <summary>
        /// 图片镜像翻转方法
        /// </summary>
        /// <param name="source_path">原图片绝对路径</param>
        /// <param name="save_path">保存的文件绝对路径</param>
        public static Hashtable RotateImage(string source_path, string save_path)
        {
            Hashtable hash = new Hashtable();
            Image img = Image.FromFile(source_path);
            hash["w"] = img.Width;
            hash["h"] = img.Height;
            PropertyItem[] exif = img.PropertyItems;
            byte orientation = 0;
            foreach (PropertyItem i in exif)
            {
                if (i.Id == 274)
                {
                    orientation = i.Value[0];
                    i.Value[0] = 1;
                    img.SetPropertyItem(i);
                }
            }

            bool is_hander = true;
            switch (orientation)
            {
                case 2:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case 3:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 4:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
                case 5:
                    img.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;
                case 6:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 7:
                    img.RotateFlip(RotateFlipType.Rotate270FlipX);
                    break;
                case 8:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                default:
                    is_hander = false;
                    break;
            }

            if (is_hander)
                img.Save(save_path);

            img.Dispose();
            return hash;

        }

        #region 图片旋转函数
        /// <summary>
        /// 以逆时针为方向对图像进行旋转，处理成功后会覆盖原有图片
        /// </summary>
        /// <param name="file_path">文件相对路径</param>
        /// <param name="angle">旋转角度[0,360]</param>
        /// <returns></returns>
        public static void ImageRotate(string file_path, int angle)
        {
            //文件拓展名
            string ext = GetFileExt(file_path);

            Image img_temp = Image.FromFile(GetMapPath(file_path));
            Bitmap b = new Bitmap(img_temp);
            angle = angle % 360;
            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //原图的宽和高

            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));
            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);
            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);
            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);
            //重至绘图的所有变换
            g.ResetTransform();
            g.Save();
            g.Dispose();
            b.Dispose();
            img_temp.Dispose();
            dsImage.Save(GetMapPath(file_path),GetImageFormat(ext));
            GC.Collect();//强制回收
            ////旋转后图片提及会变大，这里进行压缩
            //ImgThumbnail ya = new ImgThumbnail();
            ////临时文件名
            //string file_path_io = GetMapPath(file_path);
            //string fileExt = IOHelper.GetFileExt(file_path_io);
            //string guid = Guid.NewGuid().ToString();
            //string file_path_server = file_path.Substring(0, file_path.LastIndexOf("/")) + "/";
            //Hashtable hash = IOHelper.GetImageWidthHeight(file_path_io);
            //string saveTempName = guid + "_temp." + fileExt;
            //if (ya.Thumbnail(file_path_io, IOHelper.GetMapPath(file_path_server + saveTempName), TypeHelper.ObjectToInt(hash["w"]), TypeHelper.ObjectToInt(hash["h"]), 20, ImgThumbnail.ImgThumbnailType.H))
            //{
            //    //压缩成功,删掉大文件
            //    IOHelper.DeleteIOFile(file_path_io);
            //    //把新文件进行改名
            //    System.IO.File.Move(IOHelper.GetMapPath(file_path_server + saveTempName), file_path_io); //给文件改名
            //}
            //else
            //{
            //    IOHelper.WriteLogs("图片压缩失败");
            //}


        }
        #endregion 图片旋转函数


        /// <summary>
        /// 功能：解压zip格式的文件。并保存文件
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>
        /// <param name="err">出错信息</param>
        /// <param name="applicationIoc"></param>
        /// <param name="fileName">文件名字</param>
        /// <returns>解压是否成功</returns>
        public static string UnZipPath(string zipFilePath, string unZipDir, string applicationIoc, string fileName)
        {
            if (zipFilePath == string.Empty)
            {
                throw new Exception("压缩文件不能为空！");
            }
            if (!File.Exists(zipFilePath))
            {
                throw new System.IO.FileNotFoundException("压缩文件不存在！");
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("\\"))
                unZipDir += "\\";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (applicationIoc == theEntry.Name)
                    {
                        //string directoryName = Path.GetDirectoryName(theEntry.Name);
                        //fileName = Path.GetFileName(theEntry.Name);
                        //if (directoryName.Length > 0)
                        //{
                        //    Directory.CreateDirectory(unZipDir + directoryName);
                        //}
                        //if (!directoryName.EndsWith("\\"))
                        //    directoryName += "\\";
                        if (fileName != String.Empty)
                        {
                            if (theEntry.Name.EndsWith(".ico") || theEntry.Name.EndsWith(".png"))
                            {
                                using (FileStream streamWriter = File.Create(unZipDir + fileName))
                                {
                                    int size = 2048;
                                    byte[] data = new byte[2048];
                                    while (true)
                                    {
                                        size = s.Read(data, 0, data.Length);
                                        if (size > 0)
                                        {
                                            streamWriter.Write(data, 0, size);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
                return unZipDir + fileName;
            }
        }
                
        /// <summary>
        ///     获取文件流
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Stream GetFileStream(Assembly assembly, String filename)
        {
            string[] names = assembly.GetManifestResourceNames();
            string name = String.Empty;
            foreach (string s in names)
            {
                if (String.Equals(s, filename, StringComparison.CurrentCultureIgnoreCase) || s.EndsWith(filename))
                    name = s;
            }
            return String.IsNullOrEmpty(name) ? null : assembly.GetManifestResourceStream(name);
        }

        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns></returns>
        public static string ToFileSize(long size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " 字节";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " KB";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " MB";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " GB";
            return m_strSize;
        }

        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns></returns>
        public static string ToFileSize(string _filepath)
        {
            long size = GetFileSizeByte(_filepath);
            string m_strSize = "";
            long FactSize = 0;
            FactSize = size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " 字节";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " KB";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " MB";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " GB";
            return m_strSize;
        }

        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="_filepath">文件绝对路径</param>
        /// <returns></returns>
        public static string ToIOFileSize(string _filepath)
        {
            long size = GetIOFileSizeByte(_filepath);
            string m_strSize = "";
            long FactSize = 0;
            FactSize = size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " 字节";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " KB";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " MB";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " GB";
            return m_strSize;
        }

        /// <summary>
        /// 返回文件大小KB
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>int</returns>
        public static int GetFileSize(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return 0;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                FileInfo fileInfo = new FileInfo(fullpath);
                return ((int)fileInfo.Length) / 1024;
            }
            return 0;
        }

        /// <summary>
        /// 返回文件大小Byte
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>int</returns>
        public static long GetFileSizeByte(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return 0;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                FileInfo fileInfo = new FileInfo(fullpath);
                return fileInfo.Length;
            }
            return 0;
        }

        /// <summary>
        /// 返回文件大小Byte
        /// </summary>
        /// <param name="_filepath">文件绝对路径</param>
        /// <returns>int</returns>
        public static long GetIOFileSizeByte(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return 0;
            }
            if (File.Exists(_filepath))
            {
                FileInfo fileInfo = new FileInfo(_filepath);
                return fileInfo.Length;
            }
            return 0;
        }

        /// <summary>
        /// 返回文件扩展名，不含“.”
        /// </summary>
        /// <param name="_filepath">文件全名称</param>
        /// <returns>string</returns>
        public static string GetFileExt(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return "";
            }
            if (_filepath.LastIndexOf(".") > 0)
            {
                return _filepath.Substring(_filepath.LastIndexOf(".") + 1); //文件扩展名，不含“.”
            }
            return "";
        }

        /// <summary>
        /// 返回文件名，不含路径
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>string</returns>
        public static string GetFileName(string _filepath)
        {
            return _filepath.Substring(_filepath.LastIndexOf(@"/") + 1);
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>bool</returns>
        public static bool FileExists(string _filepath)
        {
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        public static bool DeleteFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return false;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="_filepath">文件绝对路径</param>
        public static bool DeleteIOFile(string _filepath)
        {
            try
            {
                if (string.IsNullOrEmpty(_filepath))
                {
                    return false;
                }
                if (File.Exists(_filepath))
                {
                    File.Delete(_filepath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 计算文件的MD5校验
        /// </summary>
        /// <param name="fileName">要校验的文件的绝对路径</param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }


        #region  序列化

        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="obj">序列对象</param>
        /// <param name="filePath">XML文件路径</param>
        /// <returns>是否成功</returns>
        public static bool SerializeToXml(object obj, string filePath)
        {
            bool result = false;

            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return result;

        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="type">目标类型(Type类型)</param>
        /// <param name="filePath">XML文件路径</param>
        /// <returns>序列对象</returns>
        public static object DeserializeFromXML(Type type, string filePath)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        #endregion

        #region  水印,缩略图

        /// <summary>
        /// 获得当前系统安装的JPEG编码解码器
        /// </summary>
        /// <returns></returns>
        public static ImageCodecInfo GetJPEGCodec()
        {
            if (_isloadjpegcodec == true)
                return _jpegcodec;

            ImageCodecInfo[] codecsList = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecsList)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                {
                    _jpegcodec = codec;
                    break;
                }

            }
            _isloadjpegcodec = true;
            return _jpegcodec;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="thumbPath">缩略图路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>   
        public static void GenerateThumb(string imagePath, string thumbPath, int width, int height, string mode)
        {
            Image image = Image.FromFile(imagePath);

            string extension = imagePath.Substring(imagePath.LastIndexOf(".")).ToLower();
            ImageFormat imageFormat = null;
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".bmp":
                    imageFormat = ImageFormat.Bmp;
                    break;
                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;
                case ".gif":
                    imageFormat = ImageFormat.Gif;
                    break;
                default:
                    imageFormat = ImageFormat.Jpeg;
                    break;
            }

            int toWidth = width > 0 ? width : image.Width;
            int toHeight = height > 0 ? height : image.Height;

            int x = 0;
            int y = 0;
            int ow = image.Width;
            int oh = image.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）           
                    break;
                case "W"://指定宽，高按比例             
                    toHeight = image.Height * width / image.Width;
                    break;
                case "H"://指定高，宽按比例
                    toWidth = image.Width * height / image.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）           
                    if ((double)image.Width / (double)image.Height > (double)toWidth / (double)toHeight)
                    {
                        oh = image.Height;
                        ow = image.Height * toWidth / toHeight;
                        y = 0;
                        x = (image.Width - ow) / 2;
                    }
                    else
                    {
                        ow = image.Width;
                        oh = image.Width * height / toWidth;
                        x = 0;
                        y = (image.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp
            Image bitmap = new Bitmap(toWidth, toHeight);

            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(image,
                        new Rectangle(0, 0, toWidth, toHeight),
                        new Rectangle(x, y, ow, oh),
                        GraphicsUnit.Pixel);

            try
            {
                bitmap.Save(thumbPath, imageFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (g != null)
                    g.Dispose();
                if (bitmap != null)
                    bitmap.Dispose();
                if (image != null)
                    image.Dispose();
            }
        }
        #endregion
    }
}
