namespace KnightFrank.Antares.Domain.Offer.Commands
{
    using System;

    using MediatR;

    public class UpdateOfferCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public Guid StatusId { get; set; }

        public decimal? Price { get; set; }

        public decimal? PricePerWeek { get; set; }

        public DateTime OfferDate { get; set; }

        public DateTime? ExchangeDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public string SpecialConditions { get; set; }

        public Guid? SearchStatusId { get; set; }

        public Guid? MortgageSurveyStatusId { get; set; }

        public Guid? MortgageStatusId { get; set; }

        public Guid? AdditionalSurveyStatusId { get; set; }

        public Guid? BrokerId { get; set; }

        public Guid? BrokerCompanyId { get; set; }

        public Guid? LenderId { get; set; }

        public Guid? LenderCompanyId { get; set; }

        public Guid? SurveyorId { get; set; }

        public Guid? SurveyorCompanyId { get; set; }

        public Guid? AdditionalSurveyorId { get; set; }

        public Guid? AdditionalSurveyorCompanyId { get; set; }

        public Guid? EnquiriesId { get; set; }

        public bool ContractApproved { get; set; }

        public int? MortgageLoanToValue { get; set; }

        public DateTime? MortgageSurveyDate { get; set; }

        public DateTime? AdditionalSurveyDate { get; set; }

        public string ProgressComment { get; set; }
    }
}
