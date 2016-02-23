using System.Web.Http;
using Owin;

namespace KnightFrank.Antares.Api
{
    /// <summary>
    /// Owin startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Owin configuration.
        /// </summary>
        /// <param name="app">Application builder.</param>
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var server = new HttpServer(config);

            WebApiConfig.Register(config);

            app.UseWebApi(server);
        }
    }
}