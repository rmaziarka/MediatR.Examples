[assembly: Microsoft.Owin.OwinStartup(typeof(KnightFrank.Antares.Api.Startup))]
namespace KnightFrank.Antares.Api
{
    using System.Web.Http;

    using KnightFrank.Antares.API;

    using Ninject.Web.Common.OwinHost;
    using Ninject.Web.WebApi.OwinHost;

    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var server = new HttpServer(config);

            WebApiConfig.Register(config);

            var kernel = NinjectWebCommon.CreateKernel();

            SwaggerConfig.Register(config);

            app
                .UseNinjectMiddleware(() => kernel)
                .UseNinjectWebApi(server)
                .UseWebApi(server);
        }
    }
}
