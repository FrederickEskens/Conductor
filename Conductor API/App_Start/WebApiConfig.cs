using System;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Conductor_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
                        .Add(new RequestHeaderMapping("Accept",
                                                      "text/html",
                                                      StringComparison.InvariantCultureIgnoreCase,
                                                      true,
                                                      "application/json"));

            // Web API routes
            config.Routes.MapHttpRoute(
                "ConductorApi",
                "api/{controller}/{action}/{id}",
                new { controller = "ConductorApi", id = RouteParameter.Optional }
            );
        }
    }
}
