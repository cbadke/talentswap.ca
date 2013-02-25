namespace TalentSwap

open System
open System.Web
open System.Web.Mvc
open System.Web.Http
open System.Web.Routing
open System.Web.Optimization

type LessTransform() = 
    interface IBundleTransform with
        member x.Process (context:BundleContext, response:BundleResponse) =
            response.Content <- dotless.Core.Less.Parse response.Content
            response.ContentType <- "text/css"


type BundleConfig() =
    static member RegisterBundles (bundles:BundleCollection) =

        bundles.UseCdn <- true

        let jqueryCdnPath = "//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"
        let jqueryUiCdnPath = "//ajax.googleapis.com/ajax/libs/jqueryui/1.10.0/jquery-ui.min.js"

        let bootstrapCssCdnPath = "//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.0/css/bootstrap-combined.min.css"
        let bootstrapJsCdnPath = "//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.0/js/bootstrap.min.js"

        bundles.Add(ScriptBundle("~/bundles/jquery", jqueryCdnPath).Include("~/Scripts/lib/jquery-{version}.js"))
        bundles.Add(ScriptBundle("~/bundles/jquery-ui", jqueryUiCdnPath).Include("~/Scripts/lib/jquery-ui-{version}.js"))
        bundles.Add(ScriptBundle("~/bundles/modernizr").Include("~/Scripts/lib/modernizr-{version}.js"))
        bundles.Add(ScriptBundle("~/bundles/bootstrap", bootstrapJsCdnPath).Include("~/Scripts/lib/bootstrap.js"))

        bundles.Add(StyleBundle("~/Content/bootstrap/css", bootstrapCssCdnPath).Include("~/Content/bootstrap/bootstrap.css", "~/Content/bootstrap/bootstrap-responsive.css"))
        bundles.Add(StyleBundle("~/Content/jquery-ui/themes/base/css").IncludeDirectory("~/Content/jquery-ui/themes/base","*.css"))

        let lessBundle = (new Bundle ("~/Content/site-css")).IncludeDirectory("~/Content/less","*.less")
        lessBundle.Transforms.Add(new LessTransform())
        lessBundle.Transforms.Add(new CssMinify())
        bundles.Add(lessBundle)


type Route = { controller : string
               action : string
               id : UrlParameter }

type Global() =
    inherit System.Web.HttpApplication() 

    static member RegisterGlobalFilters (filters:GlobalFilterCollection) =
        filters.Add(new HandleErrorAttribute())

    static member RegisterRoutes(routes:RouteCollection) =
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
        routes.MapRoute("Default", 
                        "{controller}/{action}/{id}", 
                        { controller = "Home"; action = "Index"
                          id = UrlParameter.Optional } )

    member this.Start() =
        AreaRegistration.RegisterAllAreas()
        Global.RegisterRoutes RouteTable.Routes |> ignore
        Global.RegisterGlobalFilters GlobalFilters.Filters
        BundleConfig.RegisterBundles BundleTable.Bundles
