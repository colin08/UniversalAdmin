using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 体检报告
    /// </summary>
    public class BLLMpUserMedicalReport
    {
        /// <summary>
        /// 添加体检报告
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        /// <param name="id_number"></param>
        /// <param name="file_path"></param>
        /// <returns></returns>
        public static bool Add(string title,string id_number,string file_path,out string msg)
        {
            msg = "";
            if(string.IsNullOrWhiteSpace(title))
            {
                msg = "报告名称为空";
                return false;
            }
            if(string.IsNullOrWhiteSpace(id_number))
            {
                msg = "身份证号为空";
                return false;
            }
            if(string.IsNullOrWhiteSpace(file_path))
            {
                msg = "附件为空";
                return false;
            }
            using (var db=new DataCore.EFDBContext())
            {
                Entity.MpUserMedicalReport entity_report = new Entity.MpUserMedicalReport();
                entity_report.FilePath = file_path;
                entity_report.IDCardNumber = id_number;
                entity_report.Title = title;
                db.MpUserMedicalReports.Add(entity_report);
                db.SaveChanges();
                return true;
            }
        }
    }
}
