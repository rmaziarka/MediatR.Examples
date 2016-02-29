namespace KnightFrank.Antares.Api
{
    using System.Web.Http;

    using Ninject.Web.Common.OwinHost;
    using Ninject.Web.WebApi.OwinHost;

    using Owin;

    /// <summary>
    ///     Owin startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Owin configuration.
        /// </summary>
        /// <param name="app">Application builder.</param>
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var server = new HttpServer(config);

            WebApiConfig.Register(config);

            var kernel = NinjectWebCommon.CreateKernel();

            app
                .UseNinjectMiddleware(() => kernel)
                .UseNinjectWebApi(server)
                .UseWebApi(server);
        }
    }
}
