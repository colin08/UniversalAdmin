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
        /// 意见反馈
        /// </summary>
        public DbSet<Entity.Feedback> Feedbacks { get; set; }

        /// <summary>
        /// APP版本升级管理
        /// </summary>
        public DbSet<Entity.AppVersion> AppVersions { get; set; }

        /// <summary>
        /// 系统需要权限控制的路由表
        /// </summary>
        public DbSet<Entity.SysRoute> SysRoutes { get; set; }

        /// <summary>
        /// 用户组权限信息
        /// </summary>
        public DbSet<Entity.SysRoleRoute> SysRoleRoutes { get; set; }

        /// <summary>
        /// 测试
        /// </summary>
        public DbSet<Entity.Demo> Demo { get; set; }

        /// <summary>
        /// 测试相册
        /// </summary>
        public DbSet<Entity.DemoAlbum> DemoAlbums { get; set; }

        /// <summary>
        /// 测试成员
        /// </summary>
        public DbSet<Entity.DemoDept> DemoDepts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Entity.House> Houses { get; set; }

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
