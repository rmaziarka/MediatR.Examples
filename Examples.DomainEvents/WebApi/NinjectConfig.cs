using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Catalog;
using Domain.Logging;
using Domain.Order;
using Domain.Statistics;
using MediatR;
using Ninject;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Planning.Bindings;
using Ninject.Planning.Bindings.Resolvers;

namespace WebApi
{
    public static class NinjectConfig
    {
        public static Lazy<IKernel> CreateKernel = new Lazy<IKernel>(() =>
        {
            StandardKernel kernel = new StandardKernel();

            kernel.Load(Assembly.GetExecutingAssembly());
            ConfigureMediator(kernel);
            LoadModules(kernel);

            return kernel;
        });

        private static void ConfigureMediator(KernelBase kernel)
        {
            kernel.Components.Add<IBindingResolver, ContravariantBindingResolver>();
            kernel.Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.Get(t));
            kernel.Bind<MultiInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.GetAll(t));
            kernel.Bind<IMediator>().To<Mediator>();
        }

        private static void LoadModules(KernelBase kernel)
        {
            kernel.Load<CatalogNinjectModule>();
            kernel.Load<OrderNinjectModule>();
            kernel.Load<LoggingNinjectModule>();
            kernel.Load<StatisticsNinjectModule>();
        }

        private class ContravariantBindingResolver : NinjectComponent, IBindingResolver
        {
            /// <summary>
            /// Returns any bindings from the specified collection that match the specified service.
            /// </summary>
            public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, Type service)
            {
                if (service.IsGenericType)
                {
                    var genericType = service.GetGenericTypeDefinition();
                    var genericArguments = genericType.GetGenericArguments();
                    if (genericArguments.Count() == 1
                     && genericArguments.Single().GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant))
                    {
                        var argument = service.GetGenericArguments().Single();
                        var matches = bindings.Where(kvp => kvp.Key.IsGenericType
                                                               && kvp.Key.GetGenericTypeDefinition().Equals(genericType)
                                                               && kvp.Key.GetGenericArguments().Single() != argument
                                                               && kvp.Key.GetGenericArguments().Single().IsAssignableFrom(argument))
                            .SelectMany(kvp => kvp.Value);
                        return matches;
                    }
                }

                return Enumerable.Empty<IBinding>();
            }
        }
    }
}