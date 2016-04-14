namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;

    public class RequirementNote : BaseAuditableEntity
    {
        public Guid RequirementId { get; set; }

        public virtual Requirement Requirement { get; set; }

        public string Description { get; set; }
    }
}
