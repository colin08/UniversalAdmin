using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    /// <summary>
    /// 数据库备份
    /// </summary>
    public class BLLSysDbBack
    {
        /// <summary>
        /// 获取系统已存在的数据库
        /// </summary>
        /// <returns></returns>
        public List<Entity.SysDbInfo> GetSysDbList()
        {
            using (var db = new DataCore.EFDBContext())
            {
                string strSql = "select name,filename,crdate from sysdatabases WHERE name not in('master','tempdb','model','msdb')";
                return db.Database.SqlQuery<Entity.SysDbInfo>(strSql).ToList();
            }
        }

        /// <summary>
        /// 删除备份的数据库
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool Del(string ids, out string msg, out string del_file)
        {
            msg = "ok";
            del_file = "";
            using (var db = new DataCore.EFDBContext())
            {
                string path_sql = string.Format("select FilePath from SysDbBack where id in({0})", ids);
                var db_paths = db.Database.SqlQuery<string>(path_sql).ToList();
                if (db_paths.Count == 0)
                {
                    msg = "没有找到相关数据";
                    return false;
                }
                del_file = string.Join(",", db_paths.ToArray());

                string del_path = string.Format("delete SysDbBack where ID in({0})", ids);
                var total = db.Database.ExecuteSqlCommand(del_path);
                if (total <= 0)
                {
                    msg = "未能删除成功";
                    return false;
                }

                foreach (var path in db_paths)
                {
                    Tools.IOHelper.DeleteIOFile(path);
                }
                return true;
            }
        }

        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool BackDb(Entity.SysDbBack entity, out string msg)
        {
            msg = "ok";
            if (entity == null)
            {
                msg = "实体为空";
                return false;
            }
            var site_config = Tools.ConfigHelper.GetSiteModel();
            if (!System.IO.Directory.Exists(site_config.DbBackPath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(site_config.DbBackPath);
                }
                catch (Exception ex)
                {
                    msg = "创建数据库备份目录失败：" + ex.Message;
                }
            }
            using (var db =new DataCore.EFDBContext())
            {
                string is_diff = entity.BackType == Entity.SysDbBackType.diff ? " WITH DIFFERENTIAL " : "";
                string back_path = site_config.DbBackPath + entity.DbName + "_" + entity.BackName + ".bak";
                string backSql = string.Format("BACKUP DATABASE {0} to DISK = '{1}' {2};", entity.DbName, back_path, is_diff);
                db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, backSql);
                entity.AddTime = DateTime.Now;
                entity.FilePath = back_path;
                db.SysDbBacks.Add(entity);
                db.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 还原数据库
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool RestoreDb(Entity.SysDbBack entity, out string msg)
        {
            msg = "ok";
            if (entity == null)
            {
                msg = "实体为空";
                return false;
            }
            try
            {
                using (var db = new DataCore.EFDBContext())
                {
                    string kill_conn_sql = String.Format("EXEC sp_KillThread @dbname='{0}'", entity.DbName);
                    db.Database.ExecuteSqlCommand(kill_conn_sql);

                    string restore_sql = string.Format("RESTORE DATABASE {0} FROM DISK='{1}'WITH replace", entity.DbName, entity.FilePath);
                    db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, restore_sql);
                }
            }
            catch (Exception ex)
            {
                msg = "还原数据库出错：" + ex.Message;
                return false;
            }
            return true;
        }

    }
}
