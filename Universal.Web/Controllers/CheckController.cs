using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.DataCore;
using System.Data.Entity;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    public class CheckController : Controller
    {
        
        [HttpPost]
        public JsonResult AllUser(int type,string name)
        {
            WebAjaxEntity<List<Models.ModelAllHouse>> response_entity = new WebAjaxEntity<List<Models.ModelAllHouse>>();
            response_entity.msg = 1;
            var db = new EFDBContext();
            string strSql = "";
            if (string.IsNullOrWhiteSpace(name))
                strSql = "select ID,Name from House where Type = " + type.ToString() + " order by AddTime Desc";
            else
                strSql = "select ID,Name from House where Type = 2 and Name like N'%" + name + "%' order by AddTime Desc";
            List<Models.ModelAllHouse> response_list = db.Database.SqlQuery<Models.ModelAllHouse>(strSql).ToList();
            db.Dispose();
            response_entity.data = response_list;
            return Json(response_entity);
        }

        /// <summary>
        /// 来访
        /// </summary>
        /// <returns></returns>
        public ActionResult Visitor(int? id)
        {
            return Publi(id);
        }
        
        /// <summary>
        /// 来电
        /// </summary>
        /// <returns></returns>
        public ActionResult Message(int? id)
        {
            return Publi(id);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Abc(DataCore.Entity.House entity)
        {
            LoadEnum();
            string view_name = "";
            switch (entity.Type)
            {
                case DataCore.Entity.HouseType.visitor:
                    view_name = "Visitor";
                    break;
                case DataCore.Entity.HouseType.message:
                    view_name = "Message";
                    break;
                case DataCore.Entity.HouseType.outside:
                    view_name = "Outside";
                    break;
                default:
                    break;
            }
            var isAdd = entity.ID == 0 ? true : false;
            var db = new EFDBContext();
            if (!isAdd)
            {
                if (db.Houses.Count(p => p.ID == entity.ID) == 0)
                {
                    ModelState.AddModelError("Name","数据不存在");
                    return View(view_name, entity);
                }
            }
            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {

                    entity.AddTime = DateTime.Now;

                    db.Houses.Add(entity);

                }
                else //修改
                {
                    
                    var old_entity = db.Houses.Find(entity.ID);
                    db.Entry(old_entity).CurrentValues.SetValues(entity);
                }

                db.SaveChanges();
                entity.msg = 1;
                entity.msg_id = entity.ID;
                return View(view_name, entity);
            }
            else
            {
                return View(view_name, entity);
            }
        }

        /// <summary>
        /// 外拓
        /// </summary>
        /// <returns></returns>
        public ActionResult Outside(int? id)
        {
            return Publi(id);
        }

        private ViewResult Publi(int? id)
        {
            LoadEnum();

            if (id != null)
            {
                using (var db = new EFDBContext())
                {
                    var entity = db.Houses.Find(id);
                    if (entity != null)
                        return View(entity);
                    else
                        return View(new DataCore.Entity.House());
                }
            }
            return View(new DataCore.Entity.House());
        }

        private void LoadEnum()
        {
            List<SelectListItem> gender = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseGender)))
            {
                gender.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["Genders"] = gender;

            List<SelectListItem> visitors = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseVisitors)))
            {
                visitors.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["Visitors"] = visitors;

            List<SelectListItem> visitTime = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseVisiterTime)))
            {
                visitTime.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["visitTimes"] = visitTime;

            List<SelectListItem> HouseJiaoTong = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseJiaoTong)))
            {
                HouseJiaoTong.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["HouseJiaoTongs"] = HouseJiaoTong;
            
            List<SelectListItem> meiti = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseMeiTi)))
            {
                meiti.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["MeiTis"] = meiti;

            List<SelectListItem> yinsu = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseYinSu)))
            {
                yinsu.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["YinSus"] = yinsu;

            List<SelectListItem> yusuan = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseYuSuan)))
            {
                yusuan.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["YuSuans"] = yusuan;

            List<SelectListItem> laolv = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseKaoLv)))
            {
                laolv.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["KaoLvs"] = laolv;

            List<SelectListItem> xuqiu = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseXuQiu)))
            {
                xuqiu.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["XuQius"] = xuqiu;
                        
            List<SelectListItem> guanzhuwenti_gongyu = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseGuanZhuWenTi_GongYu)))
            {
                guanzhuwenti_gongyu.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["guanzhuwenti_gongyus"] = guanzhuwenti_gongyu;

            List<SelectListItem> guanzhuwenti_shangpu = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseGuanZhuWenTi_ShangPu)))
            {
                guanzhuwenti_shangpu.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["guanzhuwenti_shangpus"] = guanzhuwenti_shangpu;

            List<SelectListItem> guanzhuwenti_zhuzhai = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseGuanZhuWenTi_ZhuZhai)))
            {
                guanzhuwenti_zhuzhai.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["guanzhuwenti_zhuzhais"] = guanzhuwenti_zhuzhai;

            List<SelectListItem> yongtu = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseYongTu)))
            {
                yongtu.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["yongtus"] = yongtu;

            List<SelectListItem> touzie = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseTouZiE)))
            {
                touzie.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["touzies"] = touzie;

            List<SelectListItem> kehujibie = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseKeHuJiBie)))
            {
                kehujibie.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["kehujibies"] = kehujibie;

            List<SelectListItem> yixiangpuxing = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseYiXiangPuXing)))
            {
                yixiangpuxing.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["yixiangpuxings"] = yixiangpuxing;

            List<SelectListItem> touzinum = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseTouZiNum)))
            {
                touzinum.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["touzinums"] = touzinum;

            List<SelectListItem> mianji_shangpu = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseMianJi_ShangPu)))
            {
                mianji_shangpu.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["mianji_shangpus"] = mianji_shangpu;

            List<SelectListItem> mianji_zhuzhai = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseMianJi_ZhuZhai)))
            {
                mianji_zhuzhai.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["mianji_zhuzhais"] = mianji_zhuzhai;

            List<SelectListItem> huxing = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseHuXing)))
            {
                huxing.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["huxings"] = huxing;
            
            List<SelectListItem> xinlizongjia = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseXinLiZongJia)))
            {
                xinlizongjia.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["xinlizongjias"] = xinlizongjia;

            List<SelectListItem> jiatingjiegou = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseJiaTingJieGou)))
            {
                jiatingjiegou.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["jiatingjiegous"] = jiatingjiegou;
            
            List<SelectListItem> area = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseArea)))
            {
                area.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["areas"] = area;

            List<SelectListItem> job = new List<SelectListItem>();
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(DataCore.Entity.HouseJob)))
            {
                job.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["jobs"] = job;

            //
        }
    }
}