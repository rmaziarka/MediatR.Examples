namespace KnightFrank.Antares.Dal.Model.Property
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Property.Activities;

    public class RequirementType : BaseEntityWithCode
    {
        public virtual ICollection<ActivityTypeDefinition> ActivityTypeDefinitions { get; set; }
    }
}
