using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Universal.Web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //关闭压缩功能，否则发布后文件加载不了
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/admin_base").Include(
                        "~/Assets/js/jquery.min.js",
                        "~/Assets/js/bootstrap.min.js",
                        "~/Assets/js/plugins/layer/layer.js",
                        "~/Assets/js/plugins/pace/pace.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Assets/js/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/Assets/js/jquery-ui-1.10.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Assets/js/plugins/validate/jquery.validate.min.js",
                        "~/Assets/js/plugins/validate/jquery.validate.unobtrusive.min.js",
                        "~/Assets/js/plugins/validate/messages_zh.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Assets/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/pace").Include(
                        "~/Assets/js/plugins/pace/pace.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/icheck").Include(
                        "~/Assets/js/plugins/iCheck/icheck.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/chosen").Include(
                        "~/Assets/js/plugins/chosen/chosen.jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/layer").Include(
                      "~/Assets/js/plugins/layer/layer.js"));

            bundles.Add(new ScriptBundle("~/bundles/laypage").Include(
                      "~/Assets/js/plugins/layer/laypage/laypage.js"));

            bundles.Add(new ScriptBundle("~/bundles/tools").Include(
                      "~/Assets/js/tools.js"));

            bundles.Add(new ScriptBundle("~/bundles/uploadifive").Include(
                      "~/Assets/js/plugins/uploadifive/jquery.uploadifive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/webuploader").Include(
                      "~/Assets/js/plugins/webuploader/webuploader.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/simditor").Include(
                      "~/Assets/js/plugins/simditor/module.min.js",
                      "~/Assets/js/plugins/simditor/hotkeys.min.js",
                      "~/Assets/js/plugins/simditor/uploader.min.js",
                      "~/Assets/js/plugins/simditor/simditor.min.js",
                      "~/Assets/js/plugins/simditor/simditor-autosave.js",
                      "~/Assets/js/plugins/simditor/marked.js",
                      "~/Assets/js/plugins/simditor/to-markdown.js",
                      "~/Assets/js/plugins/simditor/simditor-markdown.js"));

            bundles.Add(new ScriptBundle("~/bundles/metisMenu").Include(
                      "~/Assets/js/plugins/metisMenu/jquery.metisMenu.js"));

            bundles.Add(new ScriptBundle("~/bundles/slimscroll").Include(
                      "~/Assets/js/plugins/slimscroll/jquery.slimscroll.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/ztree_excheck").Include(
                      "~/Assets/js/plugins/ztree/js/jquery.ztree.core.min.js",
                      "~/Assets/js/plugins/ztree/js/jquery.ztree.excheck.min.js"));


            bundles.Add(new StyleBundle("~/Content/admin/css").Include(
                      "~/Assets/css/bootstrap.min.css",
                      "~/Assets/css/font-awesome.min.css",
                      "~/Assets/css/animate.css",
                      "~/Assets/css/style.css",
                      "~/Assets/css/admin.css"));

            bundles.Add(new StyleBundle("~/Content/uploadifive").Include(
                      "~/Assets/js/plugins/uploadifive/uploadifive.css"));

            bundles.Add(new StyleBundle("~/Content/webuploader").Include(
                      "~/Assets/js/plugins/webuploader/webuploader.css"));

            bundles.Add(new StyleBundle("~/Content/icheck").Include(
                      "~/Assets/css/plugins/iCheck/custom.css"));

            bundles.Add(new StyleBundle("~/Content/chosen").Include(
                      "~/Assets/css/plugins/chosen/chosen.css"));

            bundles.Add(new StyleBundle("~/Content/simditor").Include(
                       "~/Assets/css/plugins/simditor/simditor.css",
                       "~/Assets/css/plugins/simditor/simditor-markdown.css"));

            bundles.Add(new StyleBundle("~/Content/ztree_metroStyle").Include(
                      "~/Assets/js/plugins/ztree/css/metroStyle/metroStyle.css"));

        }
    }
}