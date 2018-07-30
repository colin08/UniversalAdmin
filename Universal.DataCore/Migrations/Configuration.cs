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
            #region ϵͳ����
            var role_list = new List<Entity.SysRole>() {
                new Entity.SysRole() {
                    AddTime = DateTime.Now,
                    RoleName = "����Ա",
                    RoleDesc = "����Ա��",
                    IsAdmin = true
                },
                new Entity.SysRole() {
                    AddTime = DateTime.Now,
                    RoleName = "�༭�û�",
                    RoleDesc = "�༭�û���",
                    IsAdmin = false
                }
            };

            role_list.ForEach(p => context.SysRoles.AddOrUpdate(x => x.RoleName, p));
            context.SaveChanges();

            var role_root = context.SysRoles.Where(p => p.RoleName == "����Ա").FirstOrDefault();
            string pwd = SecureHelper.MD5("admin");
            var user_root = new Entity.SysUser()
            {
                LastLoginTime = DateTime.Now,
                RegTime = DateTime.Now,
                NickName = "��������Ա",
                Password = pwd,
                Status = true,
                SysRole = role_root,
                UserName = "admin",
                Gender = Entity.UserGender.��,
                Avatar = ""
            };
            context.SysUsers.AddOrUpdate(p => p.UserName, user_root);

            var category_a = new Entity.CusCategory();
            category_a.PID = null;
            category_a.Title = "����";
            context.CusCategorys.Add(category_a);

            var category_b = new Entity.CusCategory();
            category_b.PID = null;
            category_b.Title = "����";
            context.CusCategorys.Add(category_b);

            var category_1 = new Entity.CusCategory();
            category_1.PCategory = category_a;
            category_1.Title = "���";
            category_1.Depth = 2;
            context.CusCategorys.Add(category_1);

            var category_2 = new Entity.CusCategory();
            category_2.PCategory = category_a;
            category_2.Title = "����";
            category_2.Depth = 2;
            context.CusCategorys.Add(category_2);

            var category_3 = new Entity.CusCategory();
            category_3.PCategory = category_a;
            category_3.Title = "�Ļ�";
            category_3.Depth = 2;
            context.CusCategorys.Add(category_3);

            var category_4 = new Entity.CusCategory();
            category_4.PCategory = category_b;
            category_4.Title = "���";
            category_4.Depth = 2;
            context.CusCategorys.Add(category_4);

            var category_5 = new Entity.CusCategory();
            category_5.PCategory = category_b;
            category_5.Title = "Ҫ��";
            category_5.Depth = 2;
            context.CusCategorys.Add(category_5);

            var category_6 = new Entity.CusCategory();
            category_6.PCategory = category_b;
            category_6.Title = "����";
            category_6.Depth = 2;
            context.CusCategorys.Add(category_6);

            #endregion

            #region ��Ŀ����
            
            //����չʾ
            var entity_shuzi = new Entity.Category();
            entity_shuzi.Title = "����չʾ";
            entity_shuzi.TitleEr = "�й������Ӿ�������չʾ�캽��";
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
            entity_shuzipinpaiguan.Title = "����Ʒ�ƹ�";
            entity_shuzipinpaiguan.Summary = "ȫ��λƷ�������裬ͨ�����ֿƼ��Ӿ�չ�֣��ı���ҵ��ͳƷ������ģʽ�������ֿƼ�չ������Ʒ�Ƴɳ�";
            entity_shuzipinpaiguan.CallName = "Digital-Brand-Pavilion";
            entity_shuzipinpaiguan.Remark = "";
            entity_shuzipinpaiguan.Weight = 98;
            entity_shuzipinpaiguan.Depth = 2;
            entity_shuzipinpaiguan.ImgUrl = "";
            entity_shuzipinpaiguan.PCategory = entity_shuzi;
            context.Categorys.Add(entity_shuzipinpaiguan);

            var entity_shuzitiyanguan = new Entity.Category();
            entity_shuzitiyanguan.Title = "���������";
            entity_shuzitiyanguan.Summary = "���ݷḻ��ǿ�����������ֵ������ֶ�ý��չ�ݣ����д��������ʵ����δ��ʵ���뼼����չʾ����";
            entity_shuzitiyanguan.CallName = "Digital-Experience-Hall";
            entity_shuzitiyanguan.Remark = "";
            entity_shuzitiyanguan.Weight = 97;
            entity_shuzitiyanguan.Depth = 2;
            entity_shuzitiyanguan.ImgUrl = "";
            entity_shuzitiyanguan.PCategory = entity_shuzi;
            context.Categorys.Add(entity_shuzitiyanguan);

            var entity_chengshiguihuaguan = new Entity.Category();
            entity_chengshiguihuaguan.Title = "���й滮��";
            entity_chengshiguihuaguan.Summary = "���滮չʾ���Ļ������������еȶ๦����һ����ۺ���չ��������ʵ���˹滮չʾ�����Ե�ȫ��λ����";
            entity_chengshiguihuaguan.CallName = "City-Planning-Hall";
            entity_chengshiguihuaguan.Remark = "";
            entity_chengshiguihuaguan.Weight = 96;
            entity_chengshiguihuaguan.Depth = 2;
            entity_chengshiguihuaguan.ImgUrl = "";
            entity_chengshiguihuaguan.PCategory = entity_shuzi;
            context.Categorys.Add(entity_chengshiguihuaguan);

            var entity_shuzigongchengfangzhen = new Entity.Category();
            entity_shuzigongchengfangzhen.Title = "���ֹ��̷���";
            entity_shuzigongchengfangzhen.Summary = "�������ַ�����ص㣬���д������Ĳ��ú�ר��Ӳ������ƣ���֤��RTDS���е�ʵʱ�Ժ;��бջ����Ե�����";
            entity_shuzigongchengfangzhen.CallName = "Digital-Engineering-Simulation";
            entity_shuzigongchengfangzhen.Remark = "";
            entity_shuzigongchengfangzhen.Weight = 95;
            entity_shuzigongchengfangzhen.Depth = 2;
            entity_shuzigongchengfangzhen.ImgUrl = "";
            entity_shuzigongchengfangzhen.PCategory = entity_shuzi;
            context.Categorys.Add(entity_shuzigongchengfangzhen);

            var entity_vrjiaohu= new Entity.Category();
            entity_vrjiaohu.Title = "VR��������";
            entity_vrjiaohu.Summary = "����ʽ�������������������ݣ����û������侳�ĸо� ��Ӱ���������羰�����š����λ����е�����̬ʽ���� ";
            entity_vrjiaohu.CallName = "VR-Interactive-Experience";
            entity_vrjiaohu.Remark = "";
            entity_vrjiaohu.Weight = 94;
            entity_vrjiaohu.Depth = 2;
            entity_vrjiaohu.ImgUrl = "";
            entity_vrjiaohu.PCategory = entity_shuzi;
            context.Categorys.Add(entity_vrjiaohu);

            var entity_arhudong = new Entity.Category();
            entity_arhudong.Title = "AR��������";
            entity_arhudong.Summary = "����ʽ�������������������ݣ����û������侳�ĸо�����Ӱ���������羰�����š����λ����е�����̬ʽ����";
            entity_arhudong.CallName = "AR-Interactive-Entertainment";
            entity_arhudong.Remark = "";
            entity_arhudong.Weight = 93;
            entity_arhudong.Depth = 2;
            entity_arhudong.ImgUrl = "";
            entity_arhudong.PCategory = entity_shuzi;
            context.Categorys.Add(entity_arhudong);

            //�����Ӿ�
            var entity_chuangyishujue = new Entity.Category();
            entity_chuangyishujue.Title = "�����Ӿ�";
            entity_chuangyishujue.TitleEr = "�й������Ӿ�������չʾ�캽��";
            entity_chuangyishujue.Summary = "";
            entity_chuangyishujue.CallName = "Creative-Vision";
            entity_chuangyishujue.Remark = "";
            entity_chuangyishujue.Weight = 89;
            entity_chuangyishujue.Depth = 1;
            entity_chuangyishujue.ImgUrl = "";
            entity_chuangyishujue.PID = null;
            context.Categorys.Add(entity_chuangyishujue);

            var entity_chuangyiguanggao = new Entity.Category();
            entity_chuangyiguanggao.Title = "������";
            entity_chuangyiguanggao.Summary = "�����鶯�ı���봫�������ں������ݵ�������Ŀ�꣬����ʱ������";
            entity_chuangyiguanggao.CallName = "Creative-Advertising";
            entity_chuangyiguanggao.Remark = "";
            entity_chuangyiguanggao.Weight = 88;
            entity_chuangyiguanggao.Depth = 2;
            entity_chuangyiguanggao.ImgUrl = "";
            entity_chuangyiguanggao.PCategory = entity_chuangyishujue;
            context.Categorys.Add(entity_chuangyiguanggao);

            var entity_shijuedonghua = new Entity.Category();
            entity_shijuedonghua.Title = "�Ӿ�����";
            entity_shijuedonghua.Summary = "�㼯�ڶ�������һ��ͨ�����ֿƼ�չ�ַ�ʽ������Ӿ���ϵͳ";
            entity_shijuedonghua.CallName = "Visual-Animation";
            entity_shijuedonghua.Remark = "";
            entity_shijuedonghua.Weight = 87;
            entity_shijuedonghua.Depth = 2;
            entity_shijuedonghua.ImgUrl = "";
            entity_shijuedonghua.PCategory = entity_chuangyishujue;
            context.Categorys.Add(entity_shijuedonghua);

            var entity_xinmeitihudong = new Entity.Category();
            entity_xinmeitihudong.Title = "��ý�廥��";
            entity_xinmeitihudong.Summary = "�����ý�������Գɣ�ͨ�����ֻ���ý�廥�����棬���û��ṩ��Ϣ�봫ý���ַ���";
            entity_xinmeitihudong.CallName = "New-Media-Interaction";
            entity_xinmeitihudong.Remark = "";
            entity_xinmeitihudong.Weight = 86;
            entity_xinmeitihudong.Depth = 2;
            entity_xinmeitihudong.ImgUrl = "";
            entity_xinmeitihudong.PCategory = entity_chuangyishujue;
            context.Categorys.Add(entity_xinmeitihudong);

            var entity_jianzhukeshihua = new Entity.Category();
            entity_jianzhukeshihua.Title = "�������ӻ�";
            entity_jianzhukeshihua.Summary = "������������ͼ��������δ���������и��ģ��չʾ";
            entity_jianzhukeshihua.CallName = "Architectural-Visualization";
            entity_jianzhukeshihua.Remark = "";
            entity_jianzhukeshihua.Weight = 85;
            entity_jianzhukeshihua.Depth = 2;
            entity_jianzhukeshihua.ImgUrl = "";
            entity_jianzhukeshihua.PCategory = entity_chuangyishujue;
            context.Categorys.Add(entity_jianzhukeshihua);

            //��������
            var entity_guanyulangxing = new Entity.Category();
            entity_guanyulangxing.Title = "��������";
            entity_guanyulangxing.Summary = "";
            entity_guanyulangxing.CallName = "About";
            entity_guanyulangxing.Remark = "";
            entity_guanyulangxing.Weight = 79;
            entity_guanyulangxing.Depth = 1;
            entity_guanyulangxing.ImgUrl = "";
            entity_guanyulangxing.PID = null;
            context.Categorys.Add(entity_guanyulangxing);

            var entity_gongsijianjie = new Entity.Category();
            entity_gongsijianjie.Title = "��˾���";
            entity_gongsijianjie.Summary = "Ϊ��ҵ�ṩ��ߴ���ĸ��������ֿƼ�չʾ����";
            entity_gongsijianjie.CallName = "Company-Profile";
            entity_gongsijianjie.Remark = "";
            entity_gongsijianjie.Weight = 78;
            entity_gongsijianjie.Depth = 2;
            entity_gongsijianjie.ImgUrl = "";
            entity_gongsijianjie.PCategory = entity_guanyulangxing;
            context.Categorys.Add(entity_gongsijianjie);

            var entity_qiyewenhua = new Entity.Category();
            entity_qiyewenhua.Title = "��ҵ�Ļ�";
            entity_qiyewenhua.Summary = "Ϊ��ҵ�ṩ��ߴ���ĸ��������ֿƼ�չʾ����";
            entity_qiyewenhua.CallName = "Company-Culture";
            entity_qiyewenhua.Remark = "";
            entity_qiyewenhua.Weight = 77;
            entity_qiyewenhua.Depth = 2;
            entity_qiyewenhua.ImgUrl = "";
            entity_qiyewenhua.PCategory = entity_guanyulangxing;
            context.Categorys.Add(entity_qiyewenhua);

            var entity_tuanduijieshao = new Entity.Category();
            entity_tuanduijieshao.Title = "�Ŷӽ���";
            entity_tuanduijieshao.Summary = "Ϊ��ҵ�ṩ��ߴ���ĸ��������ֿƼ�չʾ����";
            entity_tuanduijieshao.CallName = "Team-Introduction";
            entity_tuanduijieshao.Remark = "";
            entity_tuanduijieshao.Weight = 76;
            entity_tuanduijieshao.Depth = 2;
            entity_tuanduijieshao.ImgUrl = "";
            entity_tuanduijieshao.PCategory = entity_guanyulangxing;
            context.Categorys.Add(entity_tuanduijieshao);

            var entity_rongyu = new Entity.Category();
            entity_rongyu.Title = "��˾����";
            entity_rongyu.Summary = "Ϊ��ҵ�ṩ��ߴ���ĸ��������ֿƼ�չʾ����";
            entity_rongyu.CallName = "Company-Honor";
            entity_rongyu.Remark = "";
            entity_rongyu.Weight = 75;
            entity_rongyu.Depth = 2;
            entity_rongyu.ImgUrl = "";
            entity_rongyu.PCategory = entity_guanyulangxing;
            context.Categorys.Add(entity_rongyu);

            var entity_dashijian = new Entity.Category();
            entity_dashijian.Title = "���¼�";
            entity_dashijian.Summary = "Ϊ��ҵ�ṩ��ߴ���ĸ��������ֿƼ�չʾ����";
            entity_dashijian.CallName = "Memorabilia";
            entity_dashijian.Remark = "";
            entity_dashijian.Weight = 74;
            entity_dashijian.Depth = 2;
            entity_dashijian.ImgUrl = "";
            entity_dashijian.PCategory = entity_guanyulangxing;
            context.Categorys.Add(entity_dashijian);

            var entity_weilaiyuanj = new Entity.Category();
            entity_weilaiyuanj.Title = "δ��&Ը��";
            entity_weilaiyuanj.Summary = "Ϊ��ҵ�ṩ��ߴ���ĸ��������ֿƼ�չʾ����";
            entity_weilaiyuanj.CallName = "Future-Vision";
            entity_weilaiyuanj.Remark = "";
            entity_weilaiyuanj.Weight = 74;
            entity_weilaiyuanj.Depth = 2;
            entity_weilaiyuanj.ImgUrl = "";
            entity_weilaiyuanj.PCategory = entity_guanyulangxing;
            context.Categorys.Add(entity_weilaiyuanj);


            //��ϵ����
            var entity_lianxiwm = new Entity.Category();
            entity_lianxiwm.Title = "��ϵ����";
            entity_lianxiwm.Summary = "";
            entity_lianxiwm.CallName = "Contact-US";
            entity_lianxiwm.Remark = "";
            entity_lianxiwm.Weight = 69;
            entity_lianxiwm.Depth = 1;
            entity_lianxiwm.ImgUrl = "";
            entity_lianxiwm.PID = null;
            context.Categorys.Add(entity_lianxiwm);

            var entity_jiaruwm = new Entity.Category();
            entity_jiaruwm.Title = "��������";
            entity_jiaruwm.Summary = "";
            entity_jiaruwm.CallName = "Join-US";
            entity_jiaruwm.Remark = "";
            entity_jiaruwm.Weight = 68;
            entity_jiaruwm.Depth = 2;
            entity_jiaruwm.ImgUrl = "";
            entity_jiaruwm.PCategory = entity_lianxiwm;
            context.Categorys.Add(entity_jiaruwm);
            
            var entity_news = new Entity.Category();
            entity_news.Title = "������Ѷ";
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
