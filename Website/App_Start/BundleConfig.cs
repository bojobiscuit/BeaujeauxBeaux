using System.Web;
using System.Web.Optimization;

namespace Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include(
                "~/Scripts/jquery.js",
                "~/Scripts/popper.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/custom.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include(
                "~/Content/Superhero.css",
                "~/Content/site.css"
                ));
        }
    }
}
