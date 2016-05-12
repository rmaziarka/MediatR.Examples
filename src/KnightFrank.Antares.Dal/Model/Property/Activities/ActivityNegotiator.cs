namespace KnightFrank.Antares.Dal.Model.Property.Activities
{
    using System;

    using KnightFrank.Antares.Dal.Model.User;

    public class ActivityNegotiator : BaseEntity
    {
        public Guid ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        public Guid NegotiatorId { get; set; }
        public virtual User Negotiator { get; set; }
        public bool IsLead { get; set; }
    }
}