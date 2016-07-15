namespace KnightFrank.Antares.Domain.Common
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.Commands;

    public class ChainTransactionProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<UpdateChainTransaction, ChainTransaction>();
        }
    }
}
