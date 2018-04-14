using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.Tools;
using System.Data;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 套餐订单操作
    /// </summary>
    public class BLLOrderMedical
    {
        /// <summary>
        /// 添加一个临时订单
        /// </summary>
        /// <param name="m_id">套餐ID</param>
        /// <param name="u_id">用户ID</param>
        /// <param name="msg">返回状态信息</param>
        /// <returns></returns>
        public static string AddTempOrder(int m_id,int u_id,out string msg)
        {
            msg = "ok";
            string order_num = string.Empty;
            using (var db = new DataCore.EFDBContext())
            {
                var entity_medical = db.Medicals.Where(p => p.ID == m_id).Include(p=>p.MedicalItems).AsNoTracking().FirstOrDefault();
                if(entity_medical == null)
                {
                    msg = "该套餐不存在";
                    return order_num;
                }
                if(entity_medical.Status == Entity.MedicalStatus.Down)
                {
                    msg = "该套餐已下架";
                    return order_num;
                }

                var entity_user = db.MPUsers.Where(p => p.ID == u_id).AsNoTracking().FirstOrDefault();
                if(entity_user == null)
                {
                    msg = "用户不存在";
                    return order_num;
                }
                if(!entity_user.Status)
                {
                    msg = "该用户已被禁用";
                    return order_num;
                }

                order_num = WebHelper.GenerateRandomIntNumber(10);
                int i = 1;
                while (db.OrderMedicals.Any(p=>p.OrderNum == order_num))
                {
                    if(i==10)
                    {
                        msg = "无法生成唯一订单号";
                        return string.Empty ;
                    }
                    order_num = WebHelper.GenerateRandomIntNumber(6);
                    i++;
                }
                var entity_order = new Entity.OrderMedical();
                entity_order.OrderNum = order_num;
                entity_order.MedicalID = entity_medical.ID;
                entity_order.Amount = entity_medical.YPrice;
                entity_order.Title = entity_medical.Title;
                entity_order.ImgUrl = entity_medical.ImgUrl;
                entity_order.MPUserID = entity_user.ID;
                entity_order.RelAmount = entity_medical.Price;
                entity_order.MPrice = entity_medical.Price;
                entity_order.MYPrice = entity_medical.YPrice;
                foreach (var item in entity_medical.MedicalItems)
                {
                    var type = Entity.OrderMedicalItemType.套餐内;
                    var entity_item = new Entity.OrderMedicalItem();
                    entity_item.Desc = item.Desc;
                    entity_item.OnlyID = item.OnlyID;
                    entity_item.MedicalID = item.ID;
                    entity_item.OrderMedical = entity_order;
                    entity_item.Price = item.Price;
                    entity_item.Title = item.Title;
                    entity_item.Weight = item.Weight;
                    entity_item.Type = type;
                    entity_order.OrderMedicalItems.Add(entity_item);
                }
                db.OrderMedicals.Add(entity_order);
                db.SaveChanges();
                return order_num;                
            }
        }

        /// <summary>
        /// 修改订单项
        /// </summary>
        /// <param name="m_id"></param>
        /// <param name="u_id"></param>
        /// <param name="o"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool ModifyOrderItem(string o,string ids,out string msg)
        {
            if(string.IsNullOrWhiteSpace(o))
            {
                msg = "非法参数";
                return false;
            }
            msg = "ok";
            //新的订单项ID
            var new_list = new List<string>(ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)).Select<string, int>(q => Convert.ToInt32(q)).ToList();
            using (var db=new DataCore.EFDBContext())
            {
                var entity_order = db.OrderMedicals.Where(p => p.OrderNum == o).AsNoTracking().FirstOrDefault();
                if(entity_order == null)
                {
                    msg = "订单不存在";
                    return false;
                }

                var entity_medical = db.Medicals.Where(p => p.ID == entity_order.MedicalID).AsNoTracking().FirstOrDefault();
                if(entity_medical == null)
                {
                    msg = "套餐不存在";
                    return false;
                }
                if(entity_medical.Status == Entity.MedicalStatus.Down)
                {
                    msg = "套餐已下架";
                    return false;
                }

                #region 复杂模式
                ////原有的项ID
                //var old_list = db.Database.SqlQuery<int>("select MedicalID from OrderMedicalItem where OrderMedicalID=" + entity_order.ID.ToString() + " and Type=2").ToList();
                ////找出新增的项 new_list存在而old_list不存在
                //var add_list = new_list.Except(old_list).ToList();
                //var entity_add_list = db.MedicalItems.Where(p => add_list.Contains(p.ID)).AsNoTracking().ToList();
                //foreach (var item in entity_add_list)
                //{
                //    //如果存在，就跳过添加
                //    if(db.OrderMedicalItems.Any(p=>p.OrderMedicalID == entity_order.ID && p.Type == Entity.OrderMedicalItemType.套餐内 && p.MedicalID == item.ID))
                //    {
                //        continue;
                //    }
                //    var entity_add =new Entity.OrderMedicalItem();
                //    entity_add.Desc = item.Desc;
                //    entity_add.MedicalID = item.ID;
                //    entity_add.OnlyID = item.OnlyID;
                //    entity_add.OrderMedicalID = entity_order.ID;
                //    entity_add.Price = item.Price;
                //    entity_add.Title = item.Title;
                //    entity_add.Type = Entity.OrderMedicalItemType.额外自选;
                //    db.OrderMedicalItems.Add(entity_add);
                //}

                ////找出删除的项
                //var remove_list = old_list.Except(new_list).ToList();
                //db.OrderMedicalItems.Where(p =>p.Type == Entity.OrderMedicalItemType.额外自选 && remove_list.Contains(p.MedicalID)).ToList().ForEach(p => db.OrderMedicalItems.Remove(p));

                //db.SaveChanges();
                ////重新计算价格
                //decimal total_price = entity_medical.YPrice;
                //decimal total_relprice = entity_medical.Price;
                //var new_item_list = db.OrderMedicalItems.Where(p => p.OrderMedicalID == entity_order.ID).AsNoTracking().ToList();
                //foreach (var item in new_item_list)
                //{
                //    if(item.Price >0)
                //    {
                //        total_relprice += item.Price;
                //        total_price += item.Price;
                //    }
                //}
                //var temp_entity = db.OrderMedicals.Find(entity_order.ID);
                //temp_entity.RelAmount = total_relprice;
                //temp_entity.Amount = total_price;
                //db.SaveChanges(); 
                #endregion

                //先清空自选项目
                db.OrderMedicalItems.Where(p => p.OrderMedicalID == entity_order.ID && p.Type == Entity.OrderMedicalItemType.额外自选).ToList().ForEach(p => db.OrderMedicalItems.Remove(p));
                //再全部添加
                var db_item_list = db.MedicalItems.Where(p => p.Status && new_list.Contains(p.ID)).AsNoTracking().ToList();
                foreach (var item in db_item_list)
                {
                    //如果套内存在，就跳过添加
                    if (db.OrderMedicalItems.Any(p => p.OrderMedicalID == entity_order.ID && p.Type == Entity.OrderMedicalItemType.套餐内 && p.MedicalID == item.ID))
                    {
                        continue;
                    }
                    var entity_add = new Entity.OrderMedicalItem();
                    entity_add.Desc = item.Desc;
                    entity_add.MedicalID = item.ID;
                    entity_add.OnlyID = item.OnlyID;
                    entity_add.OrderMedicalID = entity_order.ID;
                    entity_add.Price = item.Price;
                    entity_add.Title = item.Title;
                    entity_add.Weight = item.Weight;
                    entity_add.Type = Entity.OrderMedicalItemType.额外自选;
                    db.OrderMedicalItems.Add(entity_add);
                }
                db.SaveChanges();
                //重新计算价格
                decimal total_price = entity_medical.YPrice;
                decimal total_relprice = entity_medical.Price;
                var new_item_list = db.OrderMedicalItems.Where(p =>p.Type == Entity.OrderMedicalItemType.额外自选 && p.OrderMedicalID == entity_order.ID).AsNoTracking().ToList();
                foreach (var item in new_item_list)
                {
                    if (item.Price > 0)
                    {
                        total_relprice += item.Price;
                        total_price += item.Price;
                    }
                }
                var temp_entity = db.OrderMedicals.Find(entity_order.ID);
                temp_entity.RelAmount = total_relprice;
                temp_entity.Amount = total_price;
                db.SaveChanges(); 
                return true;
            }
        }

    }
}
