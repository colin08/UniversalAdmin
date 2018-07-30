using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Universal.DataCore
{
    /// <summary>
    /// EF数据上下文
    /// </summary>
    public class EFDBContext : DbContext
    {
        public EFDBContext()
            : base("DBConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        #region 系统核心
        /// <summary>
        /// 用户信息
        /// </summary>
        public DbSet<Entity.SysUser> SysUsers { get; set; }

        /// <summary>
        /// 用户组信息
        /// </summary>
        public DbSet<Entity.SysRole> SysRoles { get; set; }

        /// <summary>
        /// 系统操作日志
        /// </summary>
        public DbSet<Entity.SysLogMethod> SysLogMethods { get; set; }

        /// <summary>
        /// 系统异常日志
        /// </summary>
        public DbSet<Entity.SysLogException> SysLogExceptions { get; set; }

        /// <summary>
        /// 系统需要权限控制的路由表
        /// </summary>
        public DbSet<Entity.SysRoute> SysRoutes { get; set; }

        /// <summary>
        /// 用户组权限信息
        /// </summary>
        public DbSet<Entity.SysRoleRoute> SysRoleRoutes { get; set; }

        /// <summary>
        /// 无限级分类
        /// </summary>
        public DbSet<Entity.CusCategory> CusCategorys { get; set; }


        #endregion


        #region 朗形

        /// <summary>
        /// 案例展示-创意视觉
        /// </summary>
        public DbSet<Entity.CaseShow> CaseShows { get; set; }

        /// <summary>
        /// 系统栏目
        /// </summary>
        public DbSet<Entity.Category> Categorys { get; set; }

        /// <summary>
        /// 未来愿景
        /// </summary>
        public DbSet<Entity.FutureVision> FutureVisions { get; set; }

        /// <summary>
        /// 首页Banner
        /// </summary>
        public DbSet<Entity.Banner> Banners { get; set; }

        /// <summary>
        /// 加入我们
        /// </summary>
        public DbSet<Entity.JoinUS> JoinUSs { get; set; }

        /// <summary>
        /// 加入我们-职位分类
        /// </summary>
        public DbSet<Entity.JoinUSCategory> JoinUSCategorys { get; set; }

        /// <summary>
        /// 新闻咨询
        /// </summary>
        public DbSet<Entity.News> News { get; set; }
        
        /// <summary>
        /// 合作企业
        /// </summary>
        public DbSet<Entity.TeamWork> TeamWorks { get; set; }

        /// <summary>
        /// 大事件
        /// </summary>
        public DbSet<Entity.TimeLine> TimeLines { get; set; }

        /// <summary>
        /// 荣誉证书
        /// </summary>
        public DbSet<Entity.Honour> Honours { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
