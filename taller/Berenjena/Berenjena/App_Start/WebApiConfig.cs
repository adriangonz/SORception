using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Berenjena
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Activamos los macros de Route
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Ponemos por defecto JSON para serializar
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            
        }
    }
}
