namespace KnightFrank.Antares.Domain
{
    using System;
    using System.Linq;
    using System.Reflection;

    using FluentValidation;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Offer.OfferHelpers;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Tenancy.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Company.CustomValidators;
    using KnightFrank.Antares.Domain.Tenancy.Commands;

    using Ninject.Modules;

    using ActivityType = KnightFrank.Antares.Domain.Common.Enums.ActivityType;
    using TenancyType = KnightFrank.Antares.Domain.Common.Enums.TenancyType;

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
            this.Bind<ICollectionValidator>().To(typeof(CollectionValidator));
            this.Bind<IEnumTypeItemValidator>().To(typeof(EnumTypeItemValidator));
            this.Bind<IAddressValidator>().To(typeof(AddressValidator));
            this.Bind<IActivityTypeDefinitionValidator>().To(typeof(ActivityTypeDefinitionValidator));
            this.Bind<IOfferProgressStatusHelper>().To<OfferProgressStatusHelper>();
            this.Bind<ICompanyCommandCustomValidator>().To<CompanyCommandCustomValidator>();

            this.Bind<IActivityReferenceMapper<ActivityAttendee>>().To<ActivityAppraisalMeetingAttendeesMapper>();
            this.Bind<IActivityReferenceMapper<Dal.Model.Contacts.Contact>>().To<ActivityContactsMapper>();
            this.Bind<IActivityReferenceMapper<ActivityDepartment>>().To<ActivityDepartmentsMapper>();
            this.Bind<IActivityReferenceMapper<ActivityUser>>().To<ActivityUsersMapper>();

            this.Bind<ITenancyReferenceMapper<TenancyTerm>>().To<TenancyTermsMapper>();
            this.Bind<IReferenceMapper<CreateTenancyCommand, Dal.Model.Tenancy.Tenancy, Dal.Model.Contacts.Contact>>().To<TenancyContactMapper>();

            this.ConfigureAttributeConfigurations();

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

        private void ConfigureAttributeConfigurations()
        {
            this.Bind<IControlsConfiguration<Tuple<PropertyType, ActivityType>>>().To<ActivityControlsConfiguration>().InSingletonScope();
            this.Bind<IControlsConfiguration<Tuple<OfferType, RequirementType>>>().To<OfferControlsConfiguration>().InSingletonScope();
            this.Bind<IControlsConfiguration<Tuple<RequirementType>>>().To<RequirementControlsConfiguration>().InSingletonScope();
            this.Bind<IControlsConfiguration<Tuple<TenancyType, RequirementType>>>().To<TenancyControlsConfiguration>();
           
            this.Bind<IAttributeValidator<Tuple<PropertyType, ActivityType>>>().To(typeof(AttributeValidator<>));
            this.Bind<IAttributeValidator<Tuple<RequirementType>>>().To(typeof(AttributeValidator<>));
            this.Bind<IAttributeValidator<Tuple<OfferType, RequirementType>>>().To(typeof(AttributeValidator<>));
            this.Bind<IAttributeValidator<Tuple<TenancyType, RequirementType>>>().To(typeof(AttributeValidator<>));

            this.Bind<IEntityMapper<Dal.Model.Property.Activities.Activity>>().To<ActivityEntityMapper>();
            this.Bind<IEntityMapper<Dal.Model.Offer.Offer>>().To<OfferEntityMapper>();
            this.Bind<IEntityMapper<Dal.Model.Property.Requirement>>().To<RequirementEntityMapper>();
            this.Bind<IEntityMapper<Dal.Model.Tenancy.Tenancy>>().To<TenancyEntityMapper>();
        }
    }
}
