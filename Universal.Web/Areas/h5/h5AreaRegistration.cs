using System.Web.Mvc;

namespace Universal.Web.Areas.h5
{
    public class h5AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "h5";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "h5_default",
                "h5/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}