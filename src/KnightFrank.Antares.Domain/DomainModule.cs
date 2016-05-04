namespace KnightFrank.Antares.Domain
{
    using System;
    using System.Linq;
    using System.Reflection;

    using FluentValidation;

    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;

    using Ninject.Extensions.Conventions;
    using Ninject.Modules;

    public class DomainModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(
                x =>
                x.FromThisAssembly()
                 .SelectAllClasses()
                 .InheritedFrom(typeof(IRequestHandler<,>))
                 .BindAllInterfaces()
                 .Configure(y => y.WhenInjectedInto(typeof(ValidatorCommandHandler<,>))));

            this.Bind(typeof(IResourceLocalisedRepositoryProvider)).To(typeof(ResourceLocalisedRepositoryProvider));

            this.Bind(typeof(IRequestHandler<,>)).To(typeof(ValidatorCommandHandler<,>));

            this.Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));

            this.Bind(typeof(IReadGenericRepository<>)).To(typeof(ReadGenericRepository<>));
            this.Bind<IEntityValidator>().To(typeof(EntityValidator));
            this.Bind<IAddressValidator>().To(typeof(AddressValidator));
            AssemblyScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly()).ForEach(
                assemblyScanResult =>
                    {
                        Type domainValidatorType = GetDomainValidatorType(assemblyScanResult);

                        this.Kernel.Bind(domainValidatorType ?? assemblyScanResult.InterfaceType)
                            .To(assemblyScanResult.ValidatorType);
                    });
        }

        private static Type GetDomainValidatorType(AssemblyScanner.AssemblyScanResult assemblyScanResult)
        {
            return
                assemblyScanResult.ValidatorType.GetInterfaces()
                                  .Where(y => y.IsGenericType)
                                  .FirstOrDefault(
                                      y =>
                                      y.GetGenericTypeDefinition() == typeof(IDomainValidator<>)
                                      && ((TypeInfo)assemblyScanResult.ValidatorType).ImplementedInterfaces.Contains(
                                          assemblyScanResult.InterfaceType));
        }
    }
}
