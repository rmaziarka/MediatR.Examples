namespace KnightFrank.Antares.Domain.Common
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Offer;

    public class ChainTransactionProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<ChainTransaction, ChainTransaction>()
                .ForMember(d => d.Activity, m => m.Ignore())
                .ForMember(d => d.Requirement, m => m.Ignore())
                .ForMember(d => d.Parent, m => m.Ignore())
                .ForMember(d => d.Property, m => m.Ignore())
                .ForMember(d => d.AgentUser, m => m.Ignore())
                .ForMember(d => d.AgentContact, m => m.Ignore())
                .ForMember(d => d.AgentCompany, m => m.Ignore())
                .ForMember(d => d.SolicitorContact, m => m.Ignore())
                .ForMember(d => d.SolicitorCompany, m => m.Ignore())
                .ForMember(d => d.Mortgage, m => m.Ignore())
                .ForMember(d => d.Survey, m => m.Ignore())
                .ForMember(d => d.Searches, m => m.Ignore())
                .ForMember(d => d.Enquiries, m => m.Ignore())
                .ForMember(d => d.ContractAgreed, m => m.Ignore());
        }
    }
}
