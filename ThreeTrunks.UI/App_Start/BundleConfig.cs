using System.Web.Optimization;

namespace ThreeTrunks.UI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/angular")
                .Include("~/Scripts/angular.min.js",
                         "~/Scripts/angular-route.min.js",
                         "~/Scripts/angular-ui/ui-bootstrap.min.js",
                         "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-1.9.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-input")
                .Include("~/Scripts/bootstrap.file-input.js"));

            bundles.Add(new ScriptBundle("~/bundles/file-upload")
                .Include("~/Scripts/angular-file-upload.js"));


            bundles.Add(new ScriptBundle("~/bundles/admin-app")
                .Include("~/Scripts/application/admin-app.js",
                "~/Scripts/application/admin-controllers/contentCtrl.js",
                "~/Scripts/application/admin-controllers/imgCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular")
                .Include("~/Scripts/angular.min.js",
                         "~/Scripts/angular-route.min.js",
                         "~/Scripts/angular-ui/ui-bootstrap.min.js",
                         "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/threeTrunkApp").Include(
       "~/Scripts/application/threeTrunkApp.js",
                "~/Scripts/application/controllers/homeController.js",
                "~/Scripts/application/controllers/aboutController.js",
                "~/Scripts/application/controllers/contactController.js",
                "~/Scripts/application/controllers/galleryController.js"));

            bundles.Add(new ScriptBundle("~/bundles/alertify")
                .Include("~/Scripts/application/admin-app.js",
                "~/Scripts/alertify.js",
                "~/Scripts/alertify.min.js"));


            bundles.Add(new StyleBundle("~/bundles/alertify-css").Include(
                "~/Content/alertifyjs/alertify.css",
                "~/Content/alertifyjs/alertify.min.css",
                "~/Content/alertifyjs/themes/default.min.css"));
        }
    }
}
