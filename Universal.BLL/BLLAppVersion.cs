using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    /// <summary>
    /// app版本
    /// </summary>
    public class BLLAppVersion
    {
        /// <summary>
        /// 获取最新版本
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static Entity.AppVersion GetLatest(Entity.APPVersionPlatforms platform)
        {
            BLL.BaseBLL<Entity.AppVersion> bll = new BaseBLL<Entity.AppVersion>();
            return bll.GetModel(p => p.Platforms == platform, "VersionCode DESC");
        }
    }
}
