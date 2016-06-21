namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    using PropertyType = KnightFrank.Antares.Domain.Common.Enums.PropertyType;
    using ActivityType = KnightFrank.Antares.Domain.Common.Enums.ActivityType;

    public class ActivityEntityMapper : BaseEntityMapper<Activity, Tuple<PropertyType, ActivityType>>
    {
        private readonly IControlsConfiguration<Tuple<PropertyType, ActivityType>> activityControlsConfiguration;
        private readonly IReadGenericRepository<Dal.Model.Property.Activities.ActivityType> activityTypeRepository;
        private readonly IReadGenericRepository<Property> propertyRepository;
        private readonly IReadGenericRepository<Dal.Model.Property.PropertyType> propertyTypeRepository;

        public ActivityEntityMapper(
            IControlsConfiguration<Tuple<PropertyType, ActivityType>> activityControlsConfiguration,
            IReadGenericRepository<Dal.Model.Property.Activities.ActivityType> activityTypeRepository,
            IReadGenericRepository<Property> propertyRepository,
            IReadGenericRepository<Dal.Model.Property.PropertyType> propertyTypeRepository)
        {
            this.activityControlsConfiguration = activityControlsConfiguration;
            this.activityTypeRepository = activityTypeRepository;
            this.propertyRepository = propertyRepository;
            this.propertyTypeRepository = propertyTypeRepository;
        }

        public override Activity MapAllowedValues<TSource>(TSource source, Activity activity, PageType pageType)
        {
            Tuple<PropertyType, ActivityType> configKey = this.GetConfigurationKey(activity);
            activity = base.MapAllowedValues(source, activity, this.activityControlsConfiguration, pageType, configKey);
            return activity;
        }

        public override Activity NullifyDisallowedValues(Activity activity, PageType pageType)
        {
            Tuple<PropertyType, ActivityType> configKey = this.GetConfigurationKey(activity);
            activity = base.NullifyDisallowedValues(activity, this.activityControlsConfiguration, pageType, configKey);
            return activity;
        }

        private Tuple<PropertyType, ActivityType> GetConfigurationKey(Activity activity)
        {
            Guid propertyId = activity.PropertyId;
            Guid activityTypeId = activity.ActivityTypeId;
            Property property = activity.Property ??
                                        this.propertyRepository.GetWithInclude(x => x.PropertyType).Single(x => x.Id == propertyId);

            Dal.Model.Property.PropertyType propertyType = property.PropertyType ?? this.propertyTypeRepository.Get().Single(x => x.Id == property.PropertyTypeId);
            Dal.Model.Property.Activities.ActivityType activityType = activity.ActivityType ?? this.activityTypeRepository.Get().Single(x => x.Id == activityTypeId);

            var propertyTypeEnum = EnumExtensions.ParseEnum<PropertyType>(propertyType.EnumCode);
            var activityTypeEnum = EnumExtensions.ParseEnum<ActivityType>(activityType.EnumCode);

            return new Tuple<PropertyType, ActivityType>(propertyTypeEnum, activityTypeEnum);
        }
    }
}
