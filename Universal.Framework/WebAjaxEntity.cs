namespace Universal.Web.Framework
{
    /// <summary>
    /// 统一返回参数格式
    /// </summary>
    public class UnifiedResultEntity<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public int msg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string msgbox { get; set; }

        public T data { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int total { get; set; }
    }
}
