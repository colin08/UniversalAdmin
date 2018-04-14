//Uncomment this class to provide custom runtime policy for Glimpse
using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;
using System.Collections.Generic;

namespace Universal.Web
{
    public class GlimpseSecurityPolicy : IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            //You can perform a check like the one below to control Glimpse's permissions within your application.
            // More information about RuntimePolicies can be found at http://getglimpse.com/Help/Custom-Runtime-Policy
            var httpContext = policyContext.GetHttpContext();
            //if (!httpContext.User.IsInRole("Administrator"))
            //{
            //    return RuntimePolicy.Off;
            //}
            string now_path = httpContext.Request.Path.ToLower();

            //过滤的地址列表
            List<string> off_path = new List<string>();
            off_path.Add("/admin");
            off_path.Add("/admin/home/index");
            off_path.Add("/admin/tools/uploadfile");
            off_path.Add("/admin/home/center");
            //if (off_path.Contains(now_path))
            //{
            //    return RuntimePolicy.Off;
            //}

            //return RuntimePolicy.On;

            return RuntimePolicy.Off;
        }

        public RuntimeEvent ExecuteOn
        {
             //The RuntimeEvent.ExecuteResource is only needed in case you create a security policy
             //Have a look at http://blog.getglimpse.com/2013/12/09/protect-glimpse-axd-with-your-custom-runtime-policy/ for more details
            get { return RuntimeEvent.EndRequest | RuntimeEvent.ExecuteResource; }
        }
    }
}
