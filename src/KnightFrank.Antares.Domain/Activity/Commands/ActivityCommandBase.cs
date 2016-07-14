namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    public abstract class ActivityCommandBase : IRequest<Guid>
    {
        public Guid ActivityStatusId { get; set; }

        public decimal? ShortLetPricePerWeek { get; set; }

        public decimal? AskingPrice { get; set; }

        public Guid ActivityTypeId { get; set; }

        public UpdateActivityUser LeadNegotiator { get; set; }

        public IList<UpdateActivityUser> SecondaryNegotiators { get; set; } = new List<UpdateActivityUser>();

        public IList<UpdateActivityDepartment> Departments { get; set; } = new List<UpdateActivityDepartment>();
        
        public IList<Guid> ContactIds { get; set; } = new List<Guid>();

        public Guid? SourceId { get; set; }

        public string SourceDescription { get; set; }

        public Guid? SellingReasonId { get; set; }

        public string PitchingThreats { get; set; }

        public string KeyNumber { get; set; }

        public string AccessArrangements { get; set; }

        public DateTime? AppraisalMeetingStart { get; set; }

        public DateTime? AppraisalMeetingEnd { get; set; }

        public string AppraisalMeetingInvitationText { get; set; }

        public IList<UpdateActivityAttendee> AppraisalMeetingAttendeesList { get; set; } = new List<UpdateActivityAttendee>();

        public IList<UpdateActivityChainTransaction> ChainTransactions { get; set; } = new List<UpdateActivityChainTransaction>();

        public decimal? ServiceChargeAmount { get; set; }
        public string ServiceChargeNote { get; set; }
        public decimal? GroundRentAmount { get; set; }
        public string GroundRentNote { get; set; }

        public string OtherCondition { get; set; }

        public Guid? DisposalTypeId { get; set; }

        public Guid? DecorationId { get; set; }

        public decimal? KfValuationPrice { get; set; }

        public decimal? VendorValuationPrice { get; set; }

        public decimal? AgreedInitialMarketingPrice { get; set; }

        public decimal? ShortKfValuationPrice { get; set; }

        public decimal? ShortVendorValuationPrice { get; set; }

        public decimal? ShortAgreedInitialMarketingPrice { get; set; }

        public decimal? LongKfValuationPrice { get; set; }

        public decimal? LongVendorValuationPrice { get; set; }

        public decimal? LongAgreedInitialMarketingPrice { get; set; }
    }
}