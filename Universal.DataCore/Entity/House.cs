using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Universal.Tools;

namespace Universal.DataCore.Entity
{

    /// <summary>
    /// 资源类别
    /// </summary>
    public enum HouseType
    {
        [EnumShowName("来客")]
        visitor = 1,
        [EnumShowName("来电")]
        message = 2,
        [EnumShowName("外拓")]
        outside = 3
    }

    public enum HouseGender
    {
        [EnumShowName("先生")]
        male = 1,
        [EnumShowName("女士")]
        female = 2
    }

    /// <summary>
    /// 来访成员构成
    /// </summary>
    public enum HouseVisitors
    {
        [EnumShowName("单独来")]
        a = 1,
        [EnumShowName("夫妻来")]
        b,
        [EnumShowName("全家来")]
        c,
        [EnumShowName("与朋友来")]
        d,
        [EnumShowName("与老客户来")]
        e
    }
    
    public enum HouseJiaoTong
    {
        [EnumShowName("公共交通")]
        a = 1,
        [EnumShowName("私家车(10万以下)")]
        b,
        [EnumShowName("私家车(10万-20万)")]
        c,
        [EnumShowName("私家车(20万-30万)")]
        d,
        [EnumShowName("私家车(30万以上)")]
        e
    }

    /// <summary>
    /// 媒体渠道
    /// </summary>
    public enum HouseMeiTi
    {
        [EnumShowName("报纸")]
        a = 1,
        [EnumShowName("网络")]
        b,
        [EnumShowName("短信")]
        c,
        [EnumShowName("社交工具")]
        d,
        [EnumShowName("户外")]
        e,
        [EnumShowName("亲友介绍")]
        f,
        [EnumShowName("电台")]
        g,
        [EnumShowName("路过")]
        h,
        [EnumShowName("派单")]
        i,
        [EnumShowName("CALL客")]
        j,
        [EnumShowName("老客户介绍")]
        k
    }

    /// <summary>
    /// 置业因素
    /// </summary>
    public enum HouseYinSu
    {
        [EnumShowName("自住")]
        a = 1,
        [EnumShowName("改善居住环境")]
        b,
        [EnumShowName("为父母购房")]
        c,
        [EnumShowName("为子女购房")]
        d,
        [EnumShowName("投资")]
        e,
        [EnumShowName("投资兼自住")]
        f
    }

    /// <summary>
    /// 预算
    /// </summary>
    public enum HouseYuSuan
    {
        [EnumShowName("29万以下")]
        a = 1,
        [EnumShowName("30~49万")]
        b,
        [EnumShowName("49~99万")]
        c,
        [EnumShowName("100万以上")]
        d
    }

    /// <summary>
    /// 考虑因素
    /// </summary>
    public enum HouseKaoLv
    {
        [EnumShowName("品牌")]
        a = 1,
        [EnumShowName("景观资源")]
        b,
        [EnumShowName("地段")]
        c,
        [EnumShowName("价格")]
        d,
        [EnumShowName("物业")]
        e,
        [EnumShowName("生活配套")]
        f,
        [EnumShowName("产品规划")]
        g,
        [EnumShowName("园林设计")]
        h,
        [EnumShowName("发展前景")]
        i,
        [EnumShowName("投资回报率")]
        j,
        [EnumShowName("其他")]
        k
    }

    /// <summary>
    /// 置业需求
    /// </summary>
    public enum HouseXuQiu
    {
        [EnumShowName("商业(一层)")]
        a = 1,
        [EnumShowName("商业(二层)")]
        b,
        [EnumShowName("商业(三层)")]
        c,
        [EnumShowName("两房两厅")]
        d,
        [EnumShowName("三房两厅")]
        e,
        [EnumShowName("酒店式公寓")]
        f
    }

    /// <summary>
    /// 来访时间
    /// </summary>
    public enum HouseVisiterTime
    {
        [EnumShowName("AM9~11")]
        a = 1,
        [EnumShowName("AM11~1")]
        b,
        [EnumShowName("PM1~3")]
        c,
        [EnumShowName("PM3~5")]
        d,
        [EnumShowName("PM5~7")]
        e,
        [EnumShowName("PM7时以后")]
        f,
    }
    /// <summary>
    /// 区域
    /// </summary>
    public enum HouseArea
    {
        [EnumShowName("七星区")]
        a = 1,
        [EnumShowName("象山区")]
        b,
        [EnumShowName("雁山区")]
        c,
        [EnumShowName("秀峰区")]
        d,
        [EnumShowName("叠彩区")]
        e,
        [EnumShowName("灵川县")]
        f,
        [EnumShowName("全州县")]
        g,
        [EnumShowName("兴安县")]
        h,
        [EnumShowName("其他县市")]
        i,
        [EnumShowName("省外")]
        j
    }

    /// <summary>
    /// 职业
    /// </summary>
    public enum HouseJob
    {
        [EnumShowName("外资企业")]
        a = 1,
        [EnumShowName("国有企业")]
        b,
        [EnumShowName("民营/私营企业")]
        c,
        [EnumShowName("中外合资企业")]
        d,
        [EnumShowName("事业单位")]
        e,
        [EnumShowName("个体经营者")]
        f,
        [EnumShowName("其它")]
        g
    }

    /// <summary>
    /// 客户级别
    /// </summary>
    public enum HouseKeHuJiBie
    {
        [EnumShowName("A级：接近成交")]
        a = 1,
        [EnumShowName("B级：可能回头")]
        b,
        [EnumShowName("C级：意愿平平")]
        c,
        [EnumShowName("D级：观光客")]
        d,
        [EnumShowName("E级：替别人拿资料")]
        e,
        [EnumShowName("F级：有能力但并非我客源")]
        f
    }


    /// <summary>
    /// 投资次数
    /// </summary>
    public enum HouseTouZiNum
    {
        [EnumShowName("首次")]
        a = 1,
        [EnumShowName("二次")]
        b,
        [EnumShowName("三次")]
        c,
        [EnumShowName("三次以上")]
        d
    }

    /// <summary>
    /// 关注问题 商铺
    /// </summary>
    public enum HouseGuanZhuWenTi_ShangPu
    {
        [EnumShowName("品牌")]
        a = 1,
        [EnumShowName("地段")]
        b,
        [EnumShowName("价格")]
        c,
        [EnumShowName("配套")]
        d,
        [EnumShowName("规划")]
        e,
        [EnumShowName("投资回报")]
        f,
        [EnumShowName("商业业态")]
        g,
        [EnumShowName("运营能力")]
        h,
        [EnumShowName("后期管理")]
        i,
        [EnumShowName("发展前景")]
        j
    }

    /// <summary>
    /// 关注问题 住宅
    /// </summary>
    public enum HouseGuanZhuWenTi_ZhuZhai
    {
        [EnumShowName("价格")]
        a = 1,
        [EnumShowName("面积")]
        b,
        [EnumShowName("楼栋位置")]
        c,
        [EnumShowName("户型设计")]
        d,
        [EnumShowName("园林")]
        e,
        [EnumShowName("朝向")]
        f,
        [EnumShowName("小区配套")]
        g,
        [EnumShowName("物业管理")]
        h,
        [EnumShowName("开发商品牌")]
        i,
        [EnumShowName("物业费")]
        j,
        [EnumShowName("车位配比")]
        k,
        [EnumShowName("学区")]
        l,
        [EnumShowName("交房时间")]
        m,
        [EnumShowName("其他")]
        n
    }

    /// <summary>
    /// 关注问题 公寓
    /// </summary>
    public enum HouseGuanZhuWenTi_GongYu
    {
        [EnumShowName("价格")]
        a = 1,
        [EnumShowName("面积")]
        b,
        [EnumShowName("层高")]
        c,
        [EnumShowName("投资回报")]
        d,
        [EnumShowName("升值空间")]
        e,
        [EnumShowName("公寓用途")]
        f,
        [EnumShowName("开发商品牌")]
        g,
        [EnumShowName("后期管理")]
        h,
        [EnumShowName("其他")]
        i
    }

    /// <summary>
    /// 用途
    /// </summary>
    public enum HouseYongTu
    {
        [EnumShowName("投资")]
        a = 1,
        [EnumShowName("自用")]
        b
    }

    /// <summary>
    /// 意向投资额
    /// </summary>
    public enum HouseTouZiE
    {
        [EnumShowName("29万以下")]
        a,
        [EnumShowName("30-49万")]
        b,
        [EnumShowName("50-99万")]
        c,
        [EnumShowName("100万以上")]
        d
    }


    /// <summary>
    /// 意向铺型
    /// </summary>
    public enum HouseYiXiangPuXing
    {
        [EnumShowName("单铺")]
        a = 1,
        [EnumShowName("连铺")]
        b
    }

    /// <summary>
    /// 需求面积 商铺
    /// </summary>
    public enum HouseMianJi_ShangPu
    {


        [EnumShowName("30-50㎡")]
        a = 1,
        [EnumShowName("50-100㎡")]
        b,
        [EnumShowName("100-200㎡")]
        c,
        [EnumShowName("200㎡以上")]
        d

    }

    /// <summary>
    /// 需求面积 住宅
    /// </summary>
    public enum HouseMianJi_ZhuZhai
    {
        [EnumShowName("80-90㎡")]
        a = 1,
        [EnumShowName("90-100㎡")]
        b,
        [EnumShowName("100-110㎡")]
        c,
        [EnumShowName("110-120㎡")]
        d
    }

    /// <summary>
    /// 户型
    /// </summary>
    public enum HouseHuXing
    {
        [EnumShowName("二房")]
        a = 1,
        [EnumShowName("三房")]
        b
    }

    /// <summary>
    /// 心里总价
    /// </summary>
    public enum HouseXinLiZongJia
    {
        [EnumShowName("29万以下")]
        a = 1,
        [EnumShowName("30-39万")]
        b,
        [EnumShowName("40-49万")]
        c,
        [EnumShowName("50万以上")]
        d
    }

    /// <summary>
    /// 家庭结构
    /// </summary>
    public enum HouseJiaTingJieGou
    {
        [EnumShowName("单身")]
        a = 1,
        [EnumShowName("两人世界")]
        b,
        [EnumShowName("三口之家")]
        c,
        [EnumShowName("四口之家")]
        d,
        [EnumShowName("四口以上")]
        e
    }

    /// <summary>
    /// 
    /// </summary>
    public class House
    {
        [NotMapped]
        public int msg { get; set; }

        [NotMapped]
        public int msg_id { get; set; }

        public int ID { get; set; }

        public HouseType Type { get; set; }

        [Display(Name = "姓名"), Required(ErrorMessage = "姓名不能为空"), MaxLength(10, ErrorMessage = "不能超过10个字符")]
        public string Name { get; set; }

        [Display(Name = "性别")]
        public HouseGender Gender { get; set; }

        [Display(Name = "年龄"), Required(ErrorMessage = "年龄不能为空"), Range(1, 100, ErrorMessage = "年龄在1-100之间")]
        public int Age { get; set; }

        [Display(Name = "电话"), Required(ErrorMessage = "电话不能为空"), MaxLength(20, ErrorMessage = "最大长度20")]
        public string Telphone { get; set; }

        [Display(Name = "地址"), Required(ErrorMessage = "地址不能为空"), MaxLength(500, ErrorMessage = "最大长度500")]
        public string Address { get; set; }

        [Display(Name = "来访次数")]
        public HouseTouZiNum VisitCount { get; set; }

        [Display(Name = "成员构成")]
        public HouseVisitors ChengYuanGouCheng { get; set; }

        [Display(Name = "来访时间")]
        public HouseVisiterTime VisitTime { get; set; }

        [Display(Name = "区域")]
        public HouseArea Area { get; set; }

        [Display(Name = "职业")]
        public HouseJob Job { get; set; }

        [Display(Name = "交通方式")]
        public HouseJiaoTong JiaoTong { get; set; }

        [Display(Name = "媒体渠道")]
        public HouseMeiTi MeiTi { get; set; }

        [Display(Name = "置业因素")]
        public HouseYinSu YinSu { get; set; }

        [Display(Name = "预算")]
        public HouseYuSuan YuSuan { get; set; }

        [Display(Name = "考虑因素")]
        public HouseKaoLv Kaolv { get; set; }

        [Display(Name = "置业需求")]
        public HouseXuQiu XuQiu { get; set; }

        /// <summary>
        /// 酒店式公寓
        /// </summary>
        [Display(Name = "意向面积")]
        public HouseMianJi_ZhuZhai MianJi_GongYu { get; set; }

        /// <summary>
        /// 酒店式公寓
        /// </summary>
        [Display(Name = "投资次数")]
        public HouseTouZiNum TouZiCiShu { get; set; }

        /// <summary>
        /// 酒店式公寓
        /// </summary>
        [Display(Name = "关注问题")]
        public HouseGuanZhuWenTi_GongYu GuanZhuWenTi_GongYu { get; set; }

        /// <summary>
        /// 酒店式公寓
        /// </summary>
        [Display(Name = "用途")]
        public HouseYongTu YongTu { get; set; }

        /// <summary>
        /// 酒店式公寓
        /// </summary>
        [Display(Name = "意向投资额")]
        public HouseTouZiE TouZiE { get; set; }

        /// <summary>
        /// 商业
        /// </summary>
        [Display(Name = "意向铺型")]
        public HouseYiXiangPuXing YiXiangPuXing { get; set; }

        /// <summary>
        /// 商业
        /// </summary>
        [Display(Name = "投资次数")]
        public HouseTouZiNum TouZiNum { get; set; }

        /// <summary>
        /// 商业
        /// </summary>
        [Display(Name = "需求面积")]
        public HouseMianJi_ShangPu MianJi_ShangPu { get; set; }

        /// <summary>
        /// 商业
        /// </summary>
        [Display(Name = "关注问题")]
        public HouseGuanZhuWenTi_ShangPu GuanZhuWenTi_ShangPu { get; set; }

        /// <summary>
        /// 住宅
        /// </summary>
        [Display(Name = "户型")]
        public HouseHuXing HuXing { get; set; }

        /// <summary>
        /// 住宅
        /// </summary>
        [Display(Name = "心里总价")]
        public HouseXinLiZongJia XinLiZongJia { get; set; }

        /// <summary>
        /// 住宅
        /// </summary>
        [Display(Name = "家庭结构")]
        public HouseJiaTingJieGou JiaTingJieGou { get; set; }

        /// <summary>
        /// 住宅
        /// </summary>
        [Display(Name = "置业次数")]
        public HouseTouZiNum ZhiYeCiShu { get; set; }

        /// <summary>
        /// 住址啊
        /// </summary>
        [Display(Name="需求面积")]
        public HouseMianJi_ZhuZhai MianJi_ZhuZhai { get; set; }

        /// <summary>
        /// 住宅
        /// </summary>
        [Display(Name = "关注问题")]
        public HouseGuanZhuWenTi_ZhuZhai GuanZhuWenTi_ZhuZhai { get; set; }

        [Display(Name = "来电日期"),Required()]
        public string LaiDianRiQi { get; set; }

        [Display(Name = "客户级别")]
        public HouseKeHuJiBie KeHuJiBie { get; set; }

        [Display(Name = "置业顾问"),Required()]
        public string ZhiYeGuWen { get; set; }

        [Display(Name = "跟踪情况"), MaxLength(500, ErrorMessage = "跟踪情况最多500个字符")]
        public string GenZongQingKuang { get; set; }

        [Display(Name = "来访描述"), MaxLength(500, ErrorMessage = "来访描述最多500个字符")]
        public string LaiFangMiaoShu { get; set; }

        [Required]
        public DateTime AddTime { get; set; }

        public House()
        {
            this.LaiDianRiQi = DateTime.Now.ToString("yyyy-MM-dd");
        }

    }
}
