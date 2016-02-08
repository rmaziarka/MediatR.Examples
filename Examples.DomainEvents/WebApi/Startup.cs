using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Owin;
using Newtonsoft.Json.Serialization;
using NLog;
using Owin;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;

[assembly: OwinStartup(typeof(WebApi.Startup))]
namespace WebApi
{
    public class Startup
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public void Configuration(IAppBuilder app)
        {
            _logger.Info("Startup - Configuration starts");

            try
            {
                app.UseNinjectMiddleware(() => NinjectConfig.CreateKernel.Value);
                
                ConfigureWebApi(app);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            _logger.Info("Startup - Configuration ends");
        }

        public void ConfigureWebApi(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;

            app.UseNinjectWebApi(config);
        }
    }
}
