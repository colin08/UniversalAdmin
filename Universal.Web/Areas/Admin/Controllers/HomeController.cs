using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            var viewModelLogin = new Models.ViewModelLogin();
            if (WorkContext.UserInfo != null)
                return RedirectToAction("Index");
            //如果保存了cookie，则为用户做自动登录
            if (!string.IsNullOrWhiteSpace(WebHelper.GetCookie(CookieKey.Is_Remeber)))
            {
                if (WebHelper.GetCookie(CookieKey.Is_Remeber) == "1")
                {
                    int uid = TypeHelper.ObjectToInt(WebHelper.GetCookie(CookieKey.Login_UserID));
                    string upwd = WebHelper.GetCookie(CookieKey.Login_UserPassword);
                    using (DataCore.EFDBContext db = new DataCore.EFDBContext())
                    {
                        Entity.SysUser model = db.SysUsers.Where(s => s.ID == uid & s.Password == upwd).Include(s => s.SysRole.SysRoleRoutes.Select(y => y.SysRoute)).FirstOrDefault();
                        if (model != null)
                        {
                            if (model.Status)
                            {
                                AddAdminLogs(db,Entity.SysLogMethodType.Login, "已记住密码，做自动登录", model.ID);
                                Session[SessionKey.Admin_User_Info] = model;
                                Session.Timeout = 60; //一小时不操作，session就过期
                                model.LastLoginTime = DateTime.Now;
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }
                            else
                                return View(viewModelLogin);
                        }
                        else
                            return View(viewModelLogin);
                    }
                }
                else
                    return View(viewModelLogin);
            }

            return View(viewModelLogin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.ViewModelLogin viewModelLogin)
        {
            //登陆错误次数限制
            if (Session[SessionKey.Login_Fail_Total] != null)
            {
                if (TypeHelper.ObjectToInt(Session[SessionKey.Login_Fail_Total]) > 3)
                {
                    ModelState.AddModelError("user_name", "失败次数过多，重启浏览器后再试");
                    return View(viewModelLogin);
                }
            }

            if (ModelState.IsValid)
            {
                using (DataCore.EFDBContext db = new DataCore.EFDBContext())
                {
                    string passworld = SecureHelper.MD5(viewModelLogin.password);
                    Entity.SysUser model = db.SysUsers.Where(s => s.UserName == viewModelLogin.user_name & s.Password == passworld).Include(s => s.SysRole.SysRoleRoutes.Select(y => y.SysRoute)).FirstOrDefault();
                    if (model == null)
                    {
                        ModelState.AddModelError("user_name", "用户名或密码错误");
                        return View(viewModelLogin);
                    }

                    if (!model.Status)
                    {
                        ModelState.AddModelError("user_name", "用户已被禁用");
                        return View(viewModelLogin);
                    }

                    Session[SessionKey.Admin_User_Info] = model;
                    Session.Timeout = 60;
                    if (viewModelLogin.is_rember)
                    {
                        WebHelper.SetCookie(CookieKey.Is_Remeber, "1", 14400);
                        WebHelper.SetCookie(CookieKey.Login_UserID, model.ID.ToString(), 14400);
                        WebHelper.SetCookie(CookieKey.Login_UserPassword, model.Password, 14400);
                    }
                    else
                    {
                        WebHelper.SetCookie(CookieKey.Login_UserID, model.ID.ToString());
                        WebHelper.SetCookie(CookieKey.Login_UserPassword, model.Password);
                    }
                    model.LastLoginTime = DateTime.Now;
                    AddAdminLogs(db,Entity.SysLogMethodType.Login, "通过后台网页登陆", model.ID);
                    db.SaveChanges();
                    return RedirectToAction("Index","Home");
                }
            }


            return View(viewModelLogin);
        }


        // GET: Admin/Home
        [AdminPermissionAttribute("其他", "后台管理首页")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Center()
        {
            Models.ViewModelSysInfo entity = new Models.ViewModelSysInfo();
            Tools.SystemInfo sys = new Tools.SystemInfo();
            //CPU占用比例
            entity.CpuLoad = (int)sys.CpuLoad;
            //CPU个数
            entity.ProcessorTotal = sys.ProcessorCount;
            //系统内存
            long PhysicalMemory = sys.PhysicalMemory;
            entity.PhysicalMemory = WebHelper.ByteToGB(PhysicalMemory);
            //可用内存
            long MemoryAvailable = sys.MemoryAvailable;
            entity.MemoryAvailable = WebHelper.ByteToGB(MemoryAvailable);
            //可用内存比例
            entity.MemoryScale = (int)(((float)MemoryAvailable / (float)PhysicalMemory) * 100);

            //当前程序所使用内存
            System.Diagnostics.Process ps = System.Diagnostics.Process.GetCurrentProcess();
            long SiteMemory = ps.WorkingSet64;
            entity.SiteScale = (int)(((float)SiteMemory / (float)PhysicalMemory) * 100);
            entity.SiteMemory = WebHelper.ByteToGB(SiteMemory);

            //当前站点所在盘符
            char[] DiskNameArr = HttpRuntime.AppDomainAppPath.Substring(0, 1).ToCharArray();
            List<SystemInfo_DiskInfo> disk_list = sys.GetLogicalDrives(DiskNameArr[0]);
            if (disk_list.Count == 1)
            {
                //可用空间
                long FreeSpace = disk_list[0].FreeSpace;
                //总大小
                long TotalSize = disk_list[0].Size;
                entity.DiskName = DiskNameArr[0].ToString();
                entity.DiskAvailable = WebHelper.ByteToGB(FreeSpace);
                entity.PhysicalDisk = WebHelper.ByteToGB(TotalSize);
                entity.DiskScale = (int)(((float)FreeSpace / (float)TotalSize) * 100);
            }
            List<SystemInfo_ProcessInfo> process_list = sys.GetProcessInfo().OrderByDescending(p=>p.WorkingSet64).Take(10).ToList();
            List<Models.ViewModelSysTopList> top_list = new List<Models.ViewModelSysTopList>();
            foreach (var item in process_list)
            {
                Models.ViewModelSysTopList model = new Models.ViewModelSysTopList();
                model.Id = item.Id;
                model.FileName = item.FileName;
                model.ProcessName = item.ProcessName;
                //大于300MB就显示警告颜色
                model.MemeoryColor = item.WorkingSet64 >= 314572800 ? "label-warning" : "label-primary";
                model.Time = WebHelper.ConvertMilliseconds(item.TotalMilliseconds);
                model.WorkingSet64 = WebHelper.ByteToGB(item.WorkingSet64);
                model.StartTime = item.StartTime;
                top_list.Add(model);
            }
            entity.MemooryTopList = top_list;
            return View(entity);
        }
        
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            WebHelper.SetCookie(CookieKey.Is_Remeber, "", -1);
            WebHelper.SetCookie(CookieKey.Login_UserID, "", -1);
            WebHelper.SetCookie(CookieKey.Login_UserPassword, "", -1);
            Session[SessionKey.Admin_User_Info] = null;
            return RedirectToAction("Login", "Home");
        }


        /// <summary>
        /// 登陆错误次数+1
        /// </summary>
        private void LoginFileTimesAdd()
        {
            //登陆次数+1
            if (Session[SessionKey.Login_Fail_Total] == null)
            {
                Session[SessionKey.Login_Fail_Total] = 1;
            }
            else
            {
                Session[SessionKey.Login_Fail_Total] = TypeHelper.ObjectToInt(Session[SessionKey.Login_Fail_Total]) + 1;
            }
        }

    }
}