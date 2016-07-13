using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    public class APIController : BaseAPIController
    {
        [HttpGet]
        [Route("api/test/a")]
        public WebAjaxEntity<string> Test()
        {
            WorkContext.AjaxStringEntity.msgbox = "消息";
            return WorkContext.AjaxStringEntity;
        }

        
    }
}
