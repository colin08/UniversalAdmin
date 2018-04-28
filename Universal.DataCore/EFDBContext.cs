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

        ///// <summary>
        ///// 接口请求日志
        ///// </summary>
        //public DbSet<Entity.SysLogApiAction> SysLogApiActions { get; set; }
        
        /// <summary>
        /// 系统需要权限控制的路由表
        /// </summary>
        public DbSet<Entity.SysRoute> SysRoutes { get; set; }

        /// <summary>
        /// 用户组权限信息
        /// </summary>
        public DbSet<Entity.SysRoleRoute> SysRoleRoutes { get; set; }

        ///// <summary>
        ///// 测试
        ///// </summary>
        //public DbSet<Entity.Demo> Demo { get; set; }

        ///// <summary>
        ///// 测试相册
        ///// </summary>
        //public DbSet<Entity.DemoAlbum> DemoAlbums { get; set; }

        ///// <summary>
        ///// 测试成员
        ///// </summary>
        //public DbSet<Entity.DemoDept> DemoDepts { get; set; }

        ///// <summary>
        ///// 无限级分类
        ///// </summary>
        //public DbSet<Entity.CusCategory> CusCategorys { get; set; }

        /// <summary>
        /// 微信用户
        /// </summary>
        public DbSet<Entity.MPUser> MPUsers { get; set; }

        /// <summary>
        /// 微信拓展医生资料用户
        /// </summary>
        public DbSet<Entity.MPUserDoctors> MPUserDoctors { get; set; }

        /// <summary>
        /// 体检套餐订单
        /// </summary>
        public DbSet<Entity.OrderMedical> OrderMedicals { get; set; }

        /// <summary>
        /// 体检套餐订单项
        /// </summary>
        public DbSet<Entity.OrderMedicalItem> OrderMedicalItems { get; set; }

        /// <summary>
        /// 诊所地区
        /// </summary>
        public DbSet<Entity.ClinicArea> ClinicAreas { get; set; }

        /// <summary>
        /// 诊所
        /// </summary>
        public DbSet<Entity.Clinic> Clinics { get; set; }

        /// <summary>
        /// 诊所科室
        /// </summary>
        public DbSet<Entity.ClinicDepartment> ClinicDepartments { get; set; }

        /// <summary>
        /// 医生特长标签
        /// </summary>
        public DbSet<Entity.DoctorsSpecialty> DoctorsSpecialtys { get; set; }

        /// <summary>
        /// 用户体检报告
        /// </summary>
        public DbSet<Entity.MpUserMedicalReport> MpUserMedicalReports { get; set; }

        /// <summary>
        /// 体检套餐轮播图
        /// </summary>
        public DbSet<Entity.MedicalBanner> MedicalBanners { get; set; }
        
        /// <summary>
        /// 体检套餐
        /// </summary>
        public DbSet<Entity.Medical> Medicals { get; set; }

        /// <summary>
        /// 体检套餐项
        /// </summary>
        public DbSet<Entity.MedicalItem> MedicalItems { get; set; }

        /// <summary>
        /// 账户支出明细
        /// </summary>
        public DbSet<Entity.MPUserAmountDetails> MPUserAmountDetails { get; set; }

        /// <summary>
        /// 医学通识文章
        /// </summary>
        public DbSet<Entity.News> News { get; set; }

        /// <summary>
        /// 医学通识分类
        /// </summary>
        public DbSet<Entity.NewsCategory> NewsCategorys { get; set; }

        /// <summary>
        /// 医学通识标签
        /// </summary>
        public DbSet<Entity.NewsTag> NewsTags { get; set; }

        /// <summary>
        /// 医学通识轮播图
        /// </summary>
        public DbSet<Entity.NewsBanner> NewsBanners { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new MPUserDoctorsMapping());
        }

    }
}
