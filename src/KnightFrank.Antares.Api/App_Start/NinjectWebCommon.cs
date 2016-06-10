namespace KnightFrank.Antares.Api
{
    using System;
    using System.Web;

    using KnightFrank.Antares.Api.Core;
    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Antares.Api.Services.AzureStorage.Factories;
    using KnightFrank.Antares.Dal;
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

                ConfigureDependenciesInHttpRequestScope(kernel);
                ConfigureMediator(kernel);

                RebindAction?.Invoke(kernel);

                kernel.Load<DomainModule>();

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void ConfigureDependenciesInHttpRequestScope(StandardKernel kernel)
        {
            kernel.Bind<KnightFrankContext>().ToSelf().InCustomHttpContextRequestScope();
        }

        private static void ConfigureMediator(KernelBase kernel)
        {
            kernel.Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.Get(t));
            kernel.Bind<MultiInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.GetAll(t));

            kernel.Bind<IMediator>().To<Mediator>();

            kernel.Bind<IStorageClientWrapper>().To<StorageClientWrapper>();
            kernel.Bind<IBlobResourceFactory>().To<BlobResourceFactory>();
            kernel.Bind<ISharedAccessBlobPolicyFactory>().To<SharedAccessBlobPolicyFactory>();
            kernel.Bind<IEntityDocumentStorageProvider>().To<EntityDocumentStorageProvider>();
            kernel.Bind<IDocumentStorageProvider>().To<DocumentStorageProvider>();

        }
    }
}