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

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Assets/js/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/Assets/js/jquery-ui-1.10.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Assets/js/plugins/validate/jquery.validate.min.js",
                        "~/Assets/js/plugins/validate/jquery.validate.unobtrusive.min.js",
                        "~/Assets/js/plugins/validate/messages_zh.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/kindeditor").Include(
                        "~/Assets/js/plugins/kindeditor-4.1.10/kindeditor-min.js",
                        "~/Assets/js/plugins/kindeditor-4.1.10/lang/zh_CN.js"));

            
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Assets/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/layer").Include(
                      "~/Assets/js/plugins/layer/layer.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/laypage").Include(
                      "~/Assets/js/plugins/layer/laypage/laypage.js"));

            bundles.Add(new ScriptBundle("~/bundles/my97datepicker").Include(
                        "~/Assets/js/plugins/My97DatePicker/WdatePicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/tools").Include(
                      "~/Assets/js/tools.js"));

            bundles.Add(new ScriptBundle("~/bundles/uploadifive").Include(
                      "~/Assets/js/plugins/uploadifive/jquery.uploadifive.min.js"));

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

            bundles.Add(new ScriptBundle("~/bundles/hplus").Include(
                      "~/Assets/js/hplus.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/contabs").Include(
                      "~/Assets/js/contabs.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                      "~/Assets/js/plugins/toastr/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/ztree_excheck").Include(
                      "~/Assets/js/plugins/ztree/js/jquery.ztree.core.min.js",
                      "~/Assets/js/plugins/ztree/js/jquery.ztree.excheck.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/textareacounter").Include(
                      "~/Assets/js/plugins/textareacounter.js"));

            bundles.Add(new StyleBundle("~/Content/admin/css").Include(
                      "~/Assets/css/bootstrap.min.css",
                      "~/Assets/css/font-awesome.min.css",
                      "~/Assets/css/animate.min.css",
                      "~/Assets/css/style.min.css",
                      "~/Assets/css/admin.css"));

            bundles.Add(new StyleBundle("~/Content/uploadifive").Include(
                      "~/Assets/js/plugins/uploadifive/uploadifive.css"));

            bundles.Add(new StyleBundle("~/Content/toastr").Include(
                      "~/Assets/css/plugins/toastr/toastr.min.css"));

            bundles.Add(new StyleBundle("~/Content/simditor").Include(
                       "~/Assets/css/plugins/simditor/simditor.css",
                       "~/Assets/css/plugins/simditor/simditor-markdown.css"));

            bundles.Add(new StyleBundle("~/Content/ztree_metroStyle").Include(
                      "~/Assets/js/plugins/ztree/css/metroStyle/metroStyle.css"));

        }
    }
}