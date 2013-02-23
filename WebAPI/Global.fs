namespace TalentSwap

open System
open System.Web
open System.Web.Mvc
open System.Web.Http
open System.Web.Routing
open System.Web.Optimization

type BundleConfig() =
    static member RegisterBundles (bundles:BundleCollection) =

        bundles.UseCdn <= true |> ignore

        let jqueryCdnPath = "http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"
        let jqueryUiCdnPath = "http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.0/jquery-ui.min.js"

        bundles.Add(ScriptBundle("~/bundles/jquery", jqueryCdnPath).Include("~/Scripts/lib/jquery-{version}.js"))
        bundles.Add(ScriptBundle("~/bundles/jquery-ui", jqueryUiCdnPath).Include("~/Scripts/lib/jquery-ui-{version}.js"))
        bundles.Add(ScriptBundle("~/bundles/modernizr").Include("~/Scripts/lib/modernizr-{version}.js"))
        bundles.Add(ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/lib/bootstrap.js"))

        bundles.Add(StyleBundle("~/Content/bootstrap/bundle").Include("~/Content/bootstrap.css", "~/Content/bootstrap-responsive.css"))
        bundles.Add(StyleBundle("~/Content/jquery-ui/themes/base/bundle").IncludeDirectory("~/Content/themes/base","*.css"))


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
