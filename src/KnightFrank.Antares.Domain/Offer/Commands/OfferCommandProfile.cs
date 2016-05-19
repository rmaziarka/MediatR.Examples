namespace KnightFrank.Antares.Domain.Offer.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Offer;

    public class OfferCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateOfferCommand, Offer>();
            this.CreateMap<UpdateOfferCommand, Offer>();
        }
    }
}
