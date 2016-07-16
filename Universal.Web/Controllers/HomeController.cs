using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace Universal.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            //TODO admin更新需要权限控制的路由表

            //string path = Tools.IOHelper.GetMapPath("~/bin/Universal.Web.dll");
            
            //byte [] buffer = System.IO.File.ReadAllBytes(path);
            //Assembly assembly = Assembly.Load(buffer);

            //foreach (var type in assembly.ExportedTypes)
            //{
            //    System.Reflection.MemberInfo[] properties = type.GetMembers();
            //    foreach (var item in properties)
            //    {
            //        string controllerName = item.ReflectedType.Name.Replace("Controller", "").ToString();
            //        string actionName = item.Name.ToString();
            //        object[] attrs = item.GetCustomAttributes(typeof(Framework.AdminPermissionAttribute), true);
            //        if (attrs.Length == 1)
            //        {
            //            Framework.AdminPermissionAttribute attr = (Framework.AdminPermissionAttribute)attrs[0];

            //        }
            //    }
            //}
            
            
            return View();
        }
        
        public ContentResult test()
        { 
            return Content("哈哈哈");
        }

    }

}