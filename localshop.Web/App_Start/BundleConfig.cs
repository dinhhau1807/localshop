using System.Web;
using System.Web.Optimization;

namespace localshop
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            RegisterAdminStyles(bundles);
            RegisterAdminScripts(bundles);

            bundles.Add(new ScriptBundle("~/content/modernizr").Include(
              "~/Assets/plugins/modernizr-2.8.3.js"));
        }

        private static void RegisterAdminStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/content/admin/css/vendor").Include(
                "~/Assets/admin/css/bootstrap.css",
                "~/Assets/admin/css/app.css"));

            bundles.Add(new StyleBundle("~/content/admin/css/toastr").Include("~/Assets/admin/libs/toastr/toastr.min.css"));

            bundles.Add(new StyleBundle("~/content/admin/css/datatables_sweetalert").Include(
                "~/Assets/admin/libs/datatables/dataTables.bootstrap4.css",
                "~/Assets/admin/libs/datatables/responsive.bootstrap4.css",
                "~/Assets/admin/libs/sweetalert2/sweetalert2.min.css"));

            bundles.Add(new StyleBundle("~/content/admin/css/dropify").Include("~/Assets/admin/libs/dropify/dropify.min.css"));
        }

        private static void RegisterAdminScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/content/admin/js/vendor").Include("~/Assets/admin/js/vendor.js"));

            bundles.Add(new ScriptBundle("~/content/admin/js/app").Include("~/Assets/admin/js/app.js"));

            bundles.Add(new ScriptBundle("~/content/admin/js/toastr").Include(
                "~/Assets/admin/libs/toastr/toastr.min.js",
                "~/Assets/admin/js/pages/toastr.init.js"));

            bundles.Add(new ScriptBundle("~/content/admin/js/datatables_sweetalert").Include(
                "~/Assets/admin/libs/datatables/jquery.dataTables.min.js",
                "~/Assets/admin/libs/datatables/dataTables.bootstrap4.js",
                "~/Assets/admin/libs/datatables/dataTables.responsive.min.js",
                "~/Assets/admin/libs/datatables/responsive.bootstrap4.min.js",
                "~/Assets/admin/libs/sweetalert2/sweetalert2.min.js",
                "~/Assets/admin/js/pages/sweet-alerts.init.js"));

            bundles.Add(new ScriptBundle("~/content/admin/js/validation").Include(
                "~/Assets/admin/libs/parsleyjs/parsley.min.js",
                "~/Assets/admin/js/pages/form-validation.init.js"));

            bundles.Add(new ScriptBundle("~/content/admin/js/dropify").Include(
                "~/Assets/admin/libs/dropify/dropify.min.js",
                "~/Assets/admin/js/pages/form-fileupload.init.js"));

            // Controller
            bundles.Add(new ScriptBundle("~/content/admin/js/category/index").Include("~/Assets/admin/js/controllers/category/index.js"));
            bundles.Add(new ScriptBundle("~/content/admin/js/configuration/index").Include("~/Assets/admin/js/controllers/configuration/index.js"));
            bundles.Add(new ScriptBundle("~/content/admin/js/contact/index").Include("~/Assets/admin/js/controllers/contact/index.js"));
            bundles.Add(new ScriptBundle("~/content/admin/js/homePage/addBanner").Include("~/Assets/admin/js/controllers/homePage/addBanner.js"));
            bundles.Add(new ScriptBundle("~/content/admin/js/homePage/banners").Include("~/Assets/admin/js/controllers/homePage/banners.js"));
            bundles.Add(new ScriptBundle("~/content/admin/js/homePage/editBanner").Include("~/Assets/admin/js/controllers/homePage/editBanner.js"));
            bundles.Add(new ScriptBundle("~/content/admin/js/homePage/sepcialFeatured").Include("~/Assets/admin/js/controllers/homePage/sepcialFeatured.js"));
            bundles.Add(new ScriptBundle("~/content/admin/js/order/index").Include("~/Assets/admin/js/controllers/order/index.js"));
            bundles.Add(new ScriptBundle("~/content/admin/js/product/add").Include("~/Assets/admin/js/controllers/product/add.js"));
            bundles.Add(new ScriptBundle("~/content/admin/js/product/edit").Include("~/Assets/admin/js/controllers/product/edit.js"));
            bundles.Add(new ScriptBundle("~/content/admin/js/product/index").Include("~/Assets/admin/js/controllers/product/index.js"));
            bundles.Add(new ScriptBundle("~/content/admin/js/review/index").Include("~/Assets/admin/js/controllers/review/index.js"));
            bundles.Add(new ScriptBundle("~/content/admin/js/user/index").Include("~/Assets/admin/js/controllers/user/index.js"));
        }
    }
}
