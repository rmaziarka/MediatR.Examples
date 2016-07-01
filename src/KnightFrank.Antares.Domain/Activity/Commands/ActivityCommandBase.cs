namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    public abstract class ActivityCommandBase : IRequest<Guid>
    {
        public Guid ActivityStatusId { get; set; }

        public decimal? MarketAppraisalPrice { get; set; }

        public decimal? RecommendedPrice { get; set; }

        public decimal? VendorEstimatedPrice { get; set; }

        public decimal? ShortLetPricePerWeek { get; set; }

        public decimal? AskingPrice { get; set; }

        public Guid ActivityTypeId { get; set; }

        public UpdateActivityUser LeadNegotiator { get; set; }

        public IList<UpdateActivityUser> SecondaryNegotiators { get; set; } = new List<UpdateActivityUser>();

        public IList<UpdateActivityDepartment> Departments { get; set; } = new List<UpdateActivityDepartment>();

        public IList<Guid> ContactIds { get; set; } = new List<Guid>();

        public Guid SourceId { get; set; }

        public string SourceDescription { get; set; }

        public Guid SellingReasonId { get; set; }

        public string PitchingThreats { get; set; }

        public string KeyNumber { get; set; }

        public string AccessArrangements { get; set; }

        public UpdateActivityAppraisalMeeting AppraisalMeeting { get; set; } = new UpdateActivityAppraisalMeeting();
    }
}