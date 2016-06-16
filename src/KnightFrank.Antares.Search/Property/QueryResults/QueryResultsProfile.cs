namespace KnightFrank.Antares.Search.Property.QueryResults
{
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Search.Models;

    public class QueryResultsProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<Property, PropertyResult>()
                .BeforeMap((src, dest) => { src.Ownerships = src.Ownerships.Where(o => o.Id != null).ToList(); });

            this.CreateMap<Address, AddressResult>();

            this.CreateMap<EnumTypeItem, EnumTypeItemResult>();

            this.CreateMap<Ownership, OwnershipResult>();

            this.CreateMap<Contact, ContactResult>();
        }
    }
}
