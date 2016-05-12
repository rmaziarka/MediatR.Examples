namespace KnightFrank.Antares.Dal.Model.Offer
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;

    public class Offer : BaseEntity
    {
        public Guid StatusId { get; set; }

        public EnumTypeItem Status { get; set; }

        public Guid RequirementId { get; set; }

        public virtual Requirement Requirement { get; set; }

        public Guid ActivityId { get; set; }

        public virtual Activity Activity { get; set; }

        public Guid NegotiatorId { get; set; }

        public virtual User Negotiator { get; set; }

        public decimal Price { get; set; }

        public DateTime OfferDate { get; set; }

        public DateTime? ExchangeDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public string SpecialConditions { get; set; }

    }
}
