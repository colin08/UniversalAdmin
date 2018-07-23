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

            bundles.Add(new ScriptBundle("~/bundles/froala_editor").Include(
                       "~/Assets/js/plugins/froala_editor/codemirror.min.js",
                       "~/Assets/js/plugins/froala_editor/xml.min.js",
                       "~/Assets/js/plugins/froala_editor/froala_editor.min.js",
                       "~/Assets/js/plugins/froala_editor/froala_editor.pkgd.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/align.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/char_counter.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/code_beautifier.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/code_view.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/colors.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/draggable.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/emoticons.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/entities.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/file.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/font_size.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/font_family.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/fullscreen.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/image.min.js",
                       //"~/Assets/js/plugins/froala_editor/plugins/image_manager.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/line_breaker.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/inline_style.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/link.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/lists.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/paragraph_format.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/paragraph_style.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/quick_insert.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/quote.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/table.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/save.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/url.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/video.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/help.min.js",
                       "~/Assets/js/plugins/froala_editor/third_party/spell_checker.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/special_characters.min.js",
                       "~/Assets/js/plugins/froala_editor/plugins/word_paste.min.js",
                       "~/Assets/js/plugins/froala_editor/languages/zh_cn.js"
                       ));

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

            bundles.Add(new StyleBundle("~/Content/froala_editor").Include(
                     "~/Assets/css/plugins/froala_editor/froala_editor.css",
                     "~/Assets/css/plugins/froala_editor/froala_style.css",
                     "~/Assets/css/plugins/froala_editor/plugins/code_view.css",
                     "~/Assets/css/plugins/froala_editor/plugins/draggable.css",
                     "~/Assets/css/plugins/froala_editor/plugins/colors.css",
                     "~/Assets/css/plugins/froala_editor/plugins/emoticons.css",
                     //"~/Assets/css/plugins/froala_editor/plugins/image_manager.css",
                     "~/Assets/css/plugins/froala_editor/plugins/image.css",
                     "~/Assets/css/plugins/froala_editor/plugins/line_breaker.css",
                     "~/Assets/css/plugins/froala_editor/plugins/table.css",
                     "~/Assets/css/plugins/froala_editor/plugins/char_counter.css",
                     "~/Assets/css/plugins/froala_editor/plugins/video.css",
                     "~/Assets/css/plugins/froala_editor/plugins/fullscreen.css",
                     "~/Assets/css/plugins/froala_editor/plugins/file.css",
                     "~/Assets/css/plugins/froala_editor/plugins/quick_insert.css",
                     "~/Assets/css/plugins/froala_editor/plugins/help.css",
                     "~/Assets/css/plugins/froala_editor/third_party/spell_checker.css",
                     "~/Assets/css/plugins/froala_editor/plugins/special_characters.css",
                     "~/Assets/css/plugins/froala_editor/codemirror.min.css"
                     ));

            bundles.Add(new StyleBundle("~/Content/ztree_metroStyle").Include(
                      "~/Assets/js/plugins/ztree/css/metroStyle/metroStyle.css"));

        }
    }
}