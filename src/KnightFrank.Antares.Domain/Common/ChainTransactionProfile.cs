namespace KnightFrank.Antares.Domain.Common
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Domain.Activity.Commands;

    public class ChainTransactionProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<UpdateActivityChainTransaction, ChainTransaction>();
        }
    }
}
