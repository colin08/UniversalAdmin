namespace Universal.DataCore.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using Tools;

    internal sealed class Configuration : DbMigrationsConfiguration<Universal.DataCore.EFDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Universal.DataCore.EFDBContext context)
        {
            #region 系统核心
            var role_list = new List<Entity.SysRole>() {
                new Entity.SysRole() {
                    AddTime = DateTime.Now,
                    RoleName = "管理员",
                    RoleDesc = "管理员组",
                    IsAdmin = true
                },
                new Entity.SysRole() {
                    AddTime = DateTime.Now,
                    RoleName = "编辑用户",
                    RoleDesc = "编辑用户组",
                    IsAdmin = false
                }
            };

            role_list.ForEach(p => context.SysRoles.AddOrUpdate(x => x.RoleName, p));
            context.SaveChanges();

            var role_root = context.SysRoles.Where(p => p.RoleName == "管理员").FirstOrDefault();
            string pwd = SecureHelper.MD5("admin");
            var user_root = new Entity.SysUser()
            {
                LastLoginTime = DateTime.Now,
                RegTime = DateTime.Now,
                NickName = "超级管理员",
                Password = pwd,
                Status = true,
                SysRole = role_root,
                UserName = "admin",
                Gender = Entity.UserGender.男,
                Avatar = ""
            };
            context.SysUsers.AddOrUpdate(p => p.UserName, user_root);

            var category_a = new Entity.CusCategory();
            category_a.PID = null;
            category_a.Title = "国内";
            context.CusCategorys.Add(category_a);

            var category_b = new Entity.CusCategory();
            category_b.PID = null;
            category_b.Title = "世界";
            context.CusCategorys.Add(category_b);

            var category_1 = new Entity.CusCategory();
            category_1.PCategory = category_a;
            category_1.Title = "社会";
            category_1.Depth = 2;
            context.CusCategorys.Add(category_1);

            var category_2 = new Entity.CusCategory();
            category_2.PCategory = category_a;
            category_2.Title = "经济";
            category_2.Depth = 2;
            context.CusCategorys.Add(category_2);

            var category_3 = new Entity.CusCategory();
            category_3.PCategory = category_a;
            category_3.Title = "文化";
            category_3.Depth = 2;
            context.CusCategorys.Add(category_3);

            var category_4 = new Entity.CusCategory();
            category_4.PCategory = category_b;
            category_4.Title = "格局";
            category_4.Depth = 2;
            context.CusCategorys.Add(category_4);

            var category_5 = new Entity.CusCategory();
            category_5.PCategory = category_b;
            category_5.Title = "要闻";
            category_5.Depth = 2;
            context.CusCategorys.Add(category_5);

            var category_6 = new Entity.CusCategory();
            category_6.PCategory = category_b;
            category_6.Title = "趋势";
            category_6.Depth = 2;
            context.CusCategorys.Add(category_6);

            #endregion

            #region 栏目分类
            
            //数字展示
            var entity_shuzi = new Entity.Category();
            entity_shuzi.Title = "数字展示";
            entity_shuzi.TitleEr = "中国创意视觉与数字展示领航者";
            entity_shuzi.Summary = "";
            entity_shuzi.CallName = "Digital-Display";
            entity_shuzi.Remark = "";
            entity_shuzi.Weight = 99;
            entity_shuzi.Depth = 1;
            entity_shuzi.ImgUrl = "";
            entity_shuzi.PID = null;
            //entity_shuzi.AddUser = user_root;
            //entity_shuzi.LastUpdateUser = user_root;
            context.Categorys.Add(entity_shuzi);

            var entity_shuzipinpaiguan = new Entity.Category();
            entity_shuzipinpaiguan.Title = "数字品牌馆";
            entity_shuzipinpaiguan.Summary = "全方位品牌形象建设，通过数字科技视觉展现，改变企业传统品牌宣传模式，以数字科技展馆主力品牌成长";
            entity_shuzipinpaiguan.CallName = "Digital-Brand-Pavilion";
            entity_shuzipinpaiguan.Remark = "";
            entity_shuzipinpaiguan.Weight = 98;
            entity_shuzipinpaiguan.Depth = 2;
            entity_shuzipinpaiguan.ImgUrl = "";
            entity_shuzipinpaiguan.PCategory = entity_shuzi;
            context.Categorys.Add(entity_shuzipinpaiguan);

            var entity_shuzitiyanguan = new Entity.Category();
            entity_shuzitiyanguan.Title = "数字体验馆";
            entity_shuzitiyanguan.Summary = "内容丰富，强化互动，与乐的人数字多媒体展馆，具有创新意义的实践，未来实景与技术的展示区域";
            entity_shuzitiyanguan.CallName = "Digital-Experience-Hall";
            entity_shuzitiyanguan.Remark = "";
            entity_shuzitiyanguan.Weight = 97;
            entity_shuzitiyanguan.Depth = 2;
            entity_shuzitiyanguan.ImgUrl = "";
            entity_shuzitiyanguan.PCategory = entity_shuzi;
            context.Categorys.Add(entity_shuzitiyanguan);

            var entity_chengshiguihuaguan = new Entity.Category();
            entity_chengshiguihuaguan.Title = "城市规划馆";
            entity_chengshiguihuaguan.Summary = "集规划展示、文化、教育、休闲等多功能于一体的综合性展览建筑，实现了规划展示功能性的全方位复合";
            entity_chengshiguihuaguan.CallName = "City-Planning-Hall";
            entity_chengshiguihuaguan.Remark = "";
            entity_chengshiguihuaguan.Weight = 96;
            entity_chengshiguihuaguan.Depth = 2;
            entity_chengshiguihuaguan.ImgUrl = "";
            entity_chengshiguihuaguan.PCategory = entity_shuzi;
            context.Categorys.Add(entity_chengshiguihuaguan);

            var entity_shuzigongchengfangzhen = new Entity.Category();
            entity_shuzigongchengfangzhen.Title = "数字工程仿真";
            entity_shuzigongchengfangzhen.Summary = "具有数字仿真的特点，并行处理技术的采用和专门硬件的设计，保证了RTDS运行的实时性和具有闭环测试的能力";
            entity_shuzigongchengfangzhen.CallName = "Digital-Engineering-Simulation";
            entity_shuzigongchengfangzhen.Remark = "";
            entity_shuzigongchengfangzhen.Weight = 95;
            entity_shuzigongchengfangzhen.Depth = 2;
            entity_shuzigongchengfangzhen.ImgUrl = "";
            entity_shuzigongchengfangzhen.PCategory = entity_shuzi;
            context.Categorys.Add(entity_shuzigongchengfangzhen);

            var entity_vrjiaohu= new Entity.Category();
            entity_vrjiaohu.Title = "VR交互体验";
            entity_vrjiaohu.Summary = "沉浸式进入虚拟世界消费内容，给用户身临其境的感觉 电影、比赛、风景、新闻、旅游互动中的物理动态式体验 ";
            entity_vrjiaohu.CallName = "VR-Interactive-Experience";
            entity_vrjiaohu.Remark = "";
            entity_vrjiaohu.Weight = 94;
            entity_vrjiaohu.Depth = 2;
            entity_vrjiaohu.ImgUrl = "";
            entity_vrjiaohu.PCategory = entity_shuzi;
            context.Categorys.Add(entity_vrjiaohu);

            var entity_arhudong = new Entity.Category();
            entity_arhudong.Title = "AR互动娱乐";
            entity_arhudong.Summary = "沉浸式进入虚拟世界消费内容，给用户身临其境的感觉，电影、比赛、风景、新闻、旅游互动中的物理动态式体验";
            entity_arhudong.CallName = "AR-Interactive-Entertainment";
            entity_arhudong.Remark = "";
            entity_arhudong.Weight = 93;
            entity_arhudong.Depth = 2;
            entity_arhudong.ImgUrl = "";
            entity_arhudong.PCategory = entity_shuzi;
            context.Categorys.Add(entity_arhudong);

            //创意视觉
            var entity_chuangyishujue = new Entity.Category();
            entity_chuangyishujue.Title = "创意视觉";
            entity_chuangyishujue.TitleEr = "中国创意视觉与数字展示领航者";
            entity_chuangyishujue.Summary = "";
            entity_chuangyishujue.CallName = "Creative-Vision";
            entity_chuangyishujue.Remark = "";
            entity_chuangyishujue.Weight = 89;
            entity_chuangyishujue.Depth = 1;
            entity_chuangyishujue.ImgUrl = "";
            entity_chuangyishujue.PID = null;
            context.Categorys.Add(entity_chuangyishujue);

            var entity_chuangyiguanggao = new Entity.Category();
            entity_chuangyiguanggao.Title = "创意广告";
            entity_chuangyiguanggao.Summary = "配以灵动的表达与传播，深挖核心内容的主题与目标，引领时代潮流";
            entity_chuangyiguanggao.CallName = "Creative-Advertising";
            entity_chuangyiguanggao.Remark = "";
            entity_chuangyiguanggao.Weight = 88;
            entity_chuangyiguanggao.Depth = 2;
            entity_chuangyiguanggao.ImgUrl = "";
            entity_chuangyiguanggao.PCategory = entity_chuangyishujue;
            context.Categorys.Add(entity_chuangyiguanggao);

            var entity_shijuedonghua = new Entity.Category();
            entity_shijuedonghua.Title = "视觉动画";
            entity_shijuedonghua.Summary = "汇集众多艺术与一身，通过数字科技展现方式，冲击视觉神经系统";
            entity_shijuedonghua.CallName = "Visual-Animation";
            entity_shijuedonghua.Remark = "";
            entity_shijuedonghua.Weight = 87;
            entity_shijuedonghua.Depth = 2;
            entity_shijuedonghua.ImgUrl = "";
            entity_shijuedonghua.PCategory = entity_chuangyishujue;
            context.Categorys.Add(entity_shijuedonghua);

            var entity_xinmeitihudong = new Entity.Category();
            entity_xinmeitihudong.Title = "新媒体互动";
            entity_xinmeitihudong.Summary = "万物皆媒、互娱自成，通过数字化新媒体互动界面，向用户提供信息与传媒娱乐服务";
            entity_xinmeitihudong.CallName = "New-Media-Interaction";
            entity_xinmeitihudong.Remark = "";
            entity_xinmeitihudong.Weight = 86;
            entity_xinmeitihudong.Depth = 2;
            entity_xinmeitihudong.ImgUrl = "";
            entity_xinmeitihudong.PCategory = entity_chuangyishujue;
            context.Categorys.Add(entity_xinmeitihudong);

            var entity_jianzhukeshihua = new Entity.Category();
            entity_jianzhukeshihua.Title = "建筑可视化";
            entity_jianzhukeshihua.Summary = "依托数字虚拟图像技术，对未来场景进行概念化模拟展示";
            entity_jianzhukeshihua.CallName = "Architectural-Visualization";
            entity_jianzhukeshihua.Remark = "";
            entity_jianzhukeshihua.Weight = 85;
            entity_jianzhukeshihua.Depth = 2;
            entity_jianzhukeshihua.ImgUrl = "";
            entity_jianzhukeshihua.PCategory = entity_chuangyishujue;
            context.Categorys.Add(entity_jianzhukeshihua);

            //关于朗形
            var entity_guanyulangxing = new Entity.Category();
            entity_guanyulangxing.Title = "关于朗形";
            entity_guanyulangxing.Summary = "";
            entity_guanyulangxing.CallName = "About";
            entity_guanyulangxing.Remark = "";
            entity_guanyulangxing.Weight = 79;
            entity_guanyulangxing.Depth = 1;
            entity_guanyulangxing.ImgUrl = "";
            entity_guanyulangxing.PID = null;
            context.Categorys.Add(entity_guanyulangxing);

            var entity_gongsijianjie = new Entity.Category();
            entity_gongsijianjie.Title = "公司简介";
            entity_gongsijianjie.Summary = "为企业提供最具创意的高质量数字科技展示服务";
            entity_gongsijianjie.CallName = "Company-Profile";
            entity_gongsijianjie.Remark = "";
            entity_gongsijianjie.Weight = 78;
            entity_gongsijianjie.Depth = 2;
            entity_gongsijianjie.ImgUrl = "";
            entity_gongsijianjie.PCategory = entity_guanyulangxing;
            context.Categorys.Add(entity_gongsijianjie);

            var entity_qiyewenhua = new Entity.Category();
            entity_qiyewenhua.Title = "企业文化";
            entity_qiyewenhua.Summary = "为企业提供最具创意的高质量数字科技展示服务";
            entity_qiyewenhua.CallName = "Company-Culture";
            entity_qiyewenhua.Remark = "";
            entity_qiyewenhua.Weight = 77;
            entity_qiyewenhua.Depth = 2;
            entity_qiyewenhua.ImgUrl = "";
            entity_qiyewenhua.PCategory = entity_guanyulangxing;
            context.Categorys.Add(entity_qiyewenhua);

            var entity_tuanduijieshao = new Entity.Category();
            entity_tuanduijieshao.Title = "团队介绍";
            entity_tuanduijieshao.Summary = "为企业提供最具创意的高质量数字科技展示服务";
            entity_tuanduijieshao.CallName = "Team-Introduction";
            entity_tuanduijieshao.Remark = "";
            entity_tuanduijieshao.Weight = 76;
            entity_tuanduijieshao.Depth = 2;
            entity_tuanduijieshao.ImgUrl = "";
            entity_tuanduijieshao.PCategory = entity_guanyulangxing;
            context.Categorys.Add(entity_tuanduijieshao);

            var entity_rongyu = new Entity.Category();
            entity_rongyu.Title = "公司荣誉";
            entity_rongyu.Summary = "为企业提供最具创意的高质量数字科技展示服务";
            entity_rongyu.CallName = "Company-Honor";
            entity_rongyu.Remark = "";
            entity_rongyu.Weight = 75;
            entity_rongyu.Depth = 2;
            entity_rongyu.ImgUrl = "";
            entity_rongyu.PCategory = entity_guanyulangxing;
            context.Categorys.Add(entity_rongyu);

            var entity_dashijian = new Entity.Category();
            entity_dashijian.Title = "大事记";
            entity_dashijian.Summary = "为企业提供最具创意的高质量数字科技展示服务";
            entity_dashijian.CallName = "Memorabilia";
            entity_dashijian.Remark = "";
            entity_dashijian.Weight = 74;
            entity_dashijian.Depth = 2;
            entity_dashijian.ImgUrl = "";
            entity_dashijian.PCategory = entity_guanyulangxing;
            context.Categorys.Add(entity_dashijian);

            var entity_weilaiyuanj = new Entity.Category();
            entity_weilaiyuanj.Title = "未来&愿景";
            entity_weilaiyuanj.Summary = "为企业提供最具创意的高质量数字科技展示服务";
            entity_weilaiyuanj.CallName = "Future-Vision";
            entity_weilaiyuanj.Remark = "";
            entity_weilaiyuanj.Weight = 74;
            entity_weilaiyuanj.Depth = 2;
            entity_weilaiyuanj.ImgUrl = "";
            entity_weilaiyuanj.PCategory = entity_guanyulangxing;
            context.Categorys.Add(entity_weilaiyuanj);


            //联系我们
            var entity_lianxiwm = new Entity.Category();
            entity_lianxiwm.Title = "联系我们";
            entity_lianxiwm.Summary = "";
            entity_lianxiwm.CallName = "Contact-US";
            entity_lianxiwm.Remark = "";
            entity_lianxiwm.Weight = 69;
            entity_lianxiwm.Depth = 1;
            entity_lianxiwm.ImgUrl = "";
            entity_lianxiwm.PID = null;
            context.Categorys.Add(entity_lianxiwm);

            var entity_jiaruwm = new Entity.Category();
            entity_jiaruwm.Title = "加入我们";
            entity_jiaruwm.Summary = "";
            entity_jiaruwm.CallName = "Join-US";
            entity_jiaruwm.Remark = "";
            entity_jiaruwm.Weight = 68;
            entity_jiaruwm.Depth = 2;
            entity_jiaruwm.ImgUrl = "";
            entity_jiaruwm.PCategory = entity_lianxiwm;
            context.Categorys.Add(entity_jiaruwm);
            
            var entity_news = new Entity.Category();
            entity_news.Title = "最新资讯";
            entity_news.Summary = "";
            entity_news.CallName = "News";
            entity_news.Remark = "";
            entity_news.Weight = 67;
            entity_news.Depth = 2;
            entity_news.ImgUrl = "";
            entity_news.PCategory = entity_lianxiwm;
            context.Categorys.Add(entity_news);

            #endregion




            context.SaveChanges();
        }
    }
}
