﻿using System;
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
        /// 接口请求日志
        /// </summary>
        public DbSet<Entity.SysLogApiAction> SysLogApiActions { get; set; }

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
        /// 用户
        /// </summary>
        public DbSet<Entity.CusUser> CusUsers { get; set; }
        
        /// <summary>
        /// 验证码
        /// </summary>
        public DbSet<Entity.CusVerification> CusVerifications { get; set; }

        /// <summary>
        /// 用户职位
        /// </summary>
        public DbSet<Entity.CusUserJob> CusUserJobs { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public DbSet<Entity.CusDepartment> CusDepartments { get; set; }

        /// <summary>
        /// 部门管理员
        /// </summary>
        public DbSet<Entity.CusDepartmentAdmin> CusDepartmentAdmins { get; set; }

        /// <summary>
        /// 权限列表
        /// </summary>
        public DbSet<Entity.CusRoute> CusRoutes { get; set; }

        /// <summary>
        /// 用户所拥有的权限列表
        /// </summary>
        public DbSet<Entity.CusUserRoute> CusUserRoutes { get; set; }

        /// <summary>
        /// 秘籍分类
        /// </summary>
        public DbSet<Entity.DocCategory> DocCategorys { get; set; }

        /// <summary>
        /// 秘籍文章
        /// </summary>
        public DbSet<Entity.DocPost> DocPosts { get; set; }

        /// <summary>
        /// 用户秘籍收藏
        /// </summary>
        public DbSet<Entity.CusUserDocFavorites> CusUserDocFavorites { get; set; }
        
        /// <summary>
        /// 通知
        /// </summary>
        public DbSet<Entity.CusNotice> CusNotices { get; set; }
        
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
