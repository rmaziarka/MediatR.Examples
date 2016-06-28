namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    public class ActivityCommandBase : IRequest<Guid>
    {
        public ActivityCommandBase()
        {
            this.SecondaryNegotiators = new List<UpdateActivityUser>();
            this.ContactIds = new List<Guid>();
        }

        public Guid ActivityStatusId { get; set; }

        public decimal? MarketAppraisalPrice { get; set; }

        public decimal? RecommendedPrice { get; set; }

        public decimal? VendorEstimatedPrice { get; set; }

        public decimal? ShortLetPricePerWeek { get; set; }

        public decimal? AskingPrice { get; set; }

        public Guid ActivityTypeId { get; set; }

        public UpdateActivityUser LeadNegotiator { get; set; }

        public IList<UpdateActivityUser> SecondaryNegotiators { get; set; }

        public IList<UpdateActivityDepartment> Departments { get; set; }

        public IList<Guid> ContactIds { get; set; }

        public Guid SourceId { get; set; }

        public string SourceDescription { get; set; }

        public Guid SellingReasonId { get; set; }

        public string PitchingThreats { get; set; }

        public string KeyNumber { get; set; }

        public string AccessArrangements { get; set; }
    }
}