using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    public class BLLProjectStage
    {

        /// <summary>
        /// 获取所有分期
        /// </summary>
        /// <returns></returns>
        public static List<Model.ProjectStage> GetAllStage(int project_id)
        {
            List<Model.ProjectStage> result = new List<Model.ProjectStage>();
            var db_list = new BLL.BaseBLL<Entity.ProjectStage>().GetListBy(0, p => p.ProjectID == project_id, "ID ASC", p => p.FileList);
            foreach (var item in db_list)
            {
                result.Add(BuildModel(item));
            }
            return result;
        }

        /// <summary>
        /// 获取单个分期
        /// </summary>
        /// <param name="stage_id"></param>
        /// <returns></returns>
        public static Model.ProjectStage GetStage(int stage_id)
        {
            return BuildModel(new BLL.BaseBLL<Entity.ProjectStage>().GetModel(p => p.ID == stage_id, p => p.FileList));
        }

        /// <summary>
        /// 添加/修改某期信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Modfify(Model.ProjectStage model, out string msg)
        {
            msg = "";
            if (model == null)
                return false;
            var db = new DataCore.EFDBContext();
            Entity.ProjectStage entity = db.ProjectStages.Find(model.stage_id);
            if (entity == null)
            {
                msg = "期不存在";
                return false;
            }
            db.ProjectStageFiles.Where(p => p.ProjectStageID == entity.ID).ToList().ForEach(p => db.ProjectStageFiles.Remove(p));

            entity.Title = model.title;
            entity.BeginTime = Tools.TypeHelper.ObjectToDateTime(model.begin_time);
            entity.ChaiBuChangjinE = model.ChaiBuChangjinE;
            entity.ChaiBuChangMianJi = model.ChaiBuChangMianJi;
            entity.ChaiJianZhuMianJi = model.ChaiJianZhuMianJi;
            entity.ChaiZhanDiMianJi = model.ChaiZhanDiMianJi;
            entity.JiDiMianJi = model.JiDiMianJi;
            entity.KongDiMianJi = model.KongDiMianJi;
            entity.WeiQYHuShu = model.WeiQYHuShu;
            entity.WeiQYMianJi = model.WeiQYMianJi;
            entity.YiQYHuShu = model.YiQYHuShu;
            entity.YiQYMianJi = model.YiQYMianJi;
            entity.ZhanDiMianJi = model.ZhanDiMianJi;
            entity.ZongHuShu = model.ZongHuShu;

            if(model.file_list != null)
            {
                foreach (var item in model.file_list)
                {
                    Entity.ProjectStageFile entity_file = new Entity.ProjectStageFile();
                    entity_file.FileName = item.file_name;
                    entity_file.FilePath = item.file_path;
                    entity_file.FileSize = item.file_size;
                    entity_file.ProjectStageID = entity.ID;
                    db.ProjectStageFiles.Add(entity_file);
                }
            }
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// 添加某期信息
        /// </summary>
        /// <param name="project_id"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int Add(int project_id,Model.ProjectStage model,out string msg)
        {
            msg = "";
            if(model == null)
            {
                msg = "非法参数";
                return 0;
            }
            BLL.BaseBLL<Entity.Project> bll_project = new BaseBLL<Entity.Project>();
            if(!bll_project.Exists(p=>p.ID == project_id))
            {
                msg = "项目不存在";
                return 0;
            }
            

            BLL.BaseBLL<Entity.ProjectStage> bll = new BaseBLL<Entity.ProjectStage>();
            Entity.ProjectStage entity = UnBuildModel(model, project_id);
            entity.ID = 0;
            bll.Add(entity);
            return entity.ID;
        }


        /// <summary>
        /// 构造实体
        /// </summary>
        /// <returns></returns>
        private static Model.ProjectStage BuildModel(Entity.ProjectStage entity)
        {
            if (entity == null)
                return null;
            Model.ProjectStage model = new Model.ProjectStage();
            model.stage_id = entity.ID;
            model.title = entity.Title;
            model.begin_time = entity.BeginTime.ToString("yyyy-MM-dd");
            model.ChaiBuChangjinE = entity.ChaiBuChangjinE;
            model.ChaiBuChangMianJi = entity.ChaiBuChangMianJi;
            model.ChaiJianZhuMianJi = entity.ChaiJianZhuMianJi;
            model.ChaiZhanDiMianJi = entity.ChaiZhanDiMianJi;
            model.JiDiMianJi = entity.JiDiMianJi;
            model.KongDiMianJi = entity.KongDiMianJi;
            model.WeiQYHuShu = entity.WeiQYHuShu;
            model.WeiQYMianJi = entity.WeiQYMianJi;
            model.YiQYHuShu = entity.YiQYHuShu;
            model.YiQYMianJi = entity.YiQYMianJi;
            model.ZhanDiMianJi = entity.ZhanDiMianJi;
            model.ZongHuShu = entity.ZongHuShu;
            if (entity.FileList != null)
            {
                foreach (var item in entity.FileList)
                {
                    model.file_list.Add(BuildFileModel(item));
                }
            }
            return model;
        }

        /// <summary>
        /// 构造附件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static Model.ProjectStageFile BuildFileModel(Entity.ProjectStageFile entity)
        {
            if (entity == null)
                return null;
            Model.ProjectStageFile model = new Model.ProjectStageFile();
            model.file_name = entity.FileName;
            model.file_path = entity.FilePath;
            model.file_size = entity.FileSize;
            return model;
        }

        /// <summary>
        /// 构造实体
        /// </summary>
        /// <returns></returns>
        private static Entity.ProjectStage UnBuildModel(Model.ProjectStage model, int project_id)
        {
            if (model == null)
                return null;
            Entity.ProjectStage entity = new Entity.ProjectStage();
            entity.ProjectID = project_id;
            entity.ID = model.stage_id;
            entity.Title = model.title;
            entity.BeginTime = Tools.TypeHelper.ObjectToDateTime(model.begin_time);
            entity.ChaiBuChangjinE = model.ChaiBuChangjinE;
            entity.ChaiBuChangMianJi = model.ChaiBuChangMianJi;
            entity.ChaiJianZhuMianJi = model.ChaiJianZhuMianJi;
            entity.ChaiZhanDiMianJi = model.ChaiZhanDiMianJi;
            entity.JiDiMianJi = model.JiDiMianJi;
            entity.KongDiMianJi = model.KongDiMianJi;
            entity.WeiQYHuShu = model.WeiQYHuShu;
            entity.WeiQYMianJi = model.WeiQYMianJi;
            entity.YiQYHuShu = model.YiQYHuShu;
            entity.YiQYMianJi = model.YiQYMianJi;
            entity.ZhanDiMianJi = model.ZhanDiMianJi;
            entity.ZongHuShu = model.ZongHuShu;
            if (model.file_list != null)
            {
                foreach (var item in model.file_list)
                {
                    entity.FileList.Add(UnBuildFileModel(item));
                }
            }
            return entity;
        }

        /// <summary>
        /// 构造附件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static Entity.ProjectStageFile UnBuildFileModel(Model.ProjectStageFile model)
        {
            if (model == null)
                return null;
            Entity.ProjectStageFile entity = new Entity.ProjectStageFile();
            entity.FileName = model.file_name;
            entity.FilePath = model.file_path;
            entity.FileSize = model.file_size;
            return entity;
        }

    }
}
