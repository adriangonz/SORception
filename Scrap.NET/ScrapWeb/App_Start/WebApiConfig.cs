using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using ScrapWeb.Filters;
using Newtonsoft.Json.Converters;

namespace ScrapWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            // Configure bad request filter
            config.Filters.Add(new ValidateModelFilter());
            // Configure exception handling filter
            config.Filters.Add(new ExceptionHandlingFilter());

            var jsonConfig = config.Formatters.JsonFormatter.SerializerSettings;

            // Add for enum to string when serializing
            jsonConfig.Converters.Add(new StringEnumConverter());
            jsonConfig.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
