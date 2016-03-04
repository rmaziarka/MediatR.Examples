using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnightFrank.Antares.Api
{
    using KnightFrank.Antares.Domain;

    using MediatR;

    using Ninject;
    using Ninject.Planning.Bindings.Resolvers;
    using Ninject.Web.Common;

    public class NinjectWebCommon
    {
        /* should be used only in integration testing scenarios */
        public static Action<IKernel> RebindAction { private get; set; }

        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Components.Add<IBindingResolver, ContravariantBindingResolver>();

                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RebindAction?.Invoke(kernel);

                ConfigureMediator(kernel);

                kernel.Load<DomainModule>();
                
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void ConfigureMediator(KernelBase kernel)
        {
            kernel.Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.Get(t));
            kernel.Bind<MultiInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.GetAll(t));

            kernel.Bind<IMediator>().To<Mediator>();
        }
    }
}