namespace KnightFrank.Antares.Domain
{
    using System;
    using System.Linq;
    using System.Reflection;

    using FluentValidation;

    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    using Ninject.Modules;

    public class DomainModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(typeof(IResourceLocalisedRepositoryProvider)).To(typeof(ResourceLocalisedRepositoryProvider));

            this.Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));
            this.Bind<INinjectInstanceResolver>().To<NinjectInstanceResolver>();

            this.Bind(typeof(IReadGenericRepository<>)).To(typeof(ReadGenericRepository<>));
            this.Bind<IEntityValidator>().To(typeof(EntityValidator));
            this.Bind<IEnumParser>().To<EnumParser>();
            this.Bind<IControlsConfiguration<Tuple<PageType, ActivityType>>>().To(typeof(ActivityControlsConfiguration));
            this.Bind<IAttributeValidator<Tuple<PropertyType, ActivityType>>>().To(typeof(AttributeValidator<>));
            
            this.Bind<ICollectionValidator>().To(typeof(CollectionValidator));
            this.Bind<IEnumTypeItemValidator>().To(typeof(EnumTypeItemValidator));
            this.Bind<IAddressValidator>().To(typeof(AddressValidator));
            this.Bind<IActivityTypeDefinitionValidator>().To(typeof(ActivityTypeDefinitionValidator));

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
