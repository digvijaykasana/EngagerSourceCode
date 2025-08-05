using System.Web;
using System.Web.Optimization;

namespace EngagerMark4
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/moment.min.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-submenu.min.js",
                      "~/Scripts/bootstrap-datetimepicker.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/Custom/Common.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/AngularJS/angular.min.js",
                        "~/Scripts/AngularJS/Chart.js",
                        "~/Scripts/AngularJS/angular-chart.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-submenu.min.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/site.css",
                      "~/Content/custombootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/dropzone").Include(
                    "~/Content/dropzone/dropzone.css"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                        "~/Scripts/DropZone/dropzone.js"
                    ));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                  "~/Content/bootstrap.min.css",
                  "~/fonts/font-awesome-4.4.0/css/font-awesome.min.css",
                  "~/Content/bootstrap-submenu.min.css",
                  "~/Content/bootstrap-datetimepicker.css",
                  "~/Content/custombootstrap.css"
              ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/core.css",
                        "~/Content/themes/base/resizable.css",
                        "~/Content/themes/base/selectable.css",
                        "~/Content/themes/base/accordion.css",
                        "~/Content/themes/base/autocomplete.css",
                        "~/Content/themes/base/button.css",
                        "~/Content/themes/base/dialog.css",
                        "~/Content/themes/base/slider.css",
                        "~/Content/themes/base/tabs.css",
                        "~/Content/themes/base/datepicker.css",
                        "~/Content/themes/base/progressbar.css",
                        "~/Content/themes/base/theme.css",
                        "~/Content/uploadfile.css",
                        "~/Content/PagedList.css"
                        //,"~/Content/Treeview.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/angular").Include(
                    "~/Content/angularjs/angular-chart.css"));
        }
    }
}
