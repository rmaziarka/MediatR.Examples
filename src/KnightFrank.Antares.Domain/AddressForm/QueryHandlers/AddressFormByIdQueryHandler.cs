namespace KnightFrank.Antares.Domain.AddressForm.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AddressForm.Queries;
    using MediatR;
    using QueryResults;
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Address;

    public class AddressFormByIdQueryHandler : IRequestHandler<AddressFormByIdQuery, AddressFormQueryResult>
    {
        private readonly IReadGenericRepository<AddressForm> addressFormRepository;
        private readonly IReadGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository;

        public AddressFormByIdQueryHandler(IReadGenericRepository<AddressForm> addressFormRepository,IReadGenericRepository<AddressFieldDefinition> addressFieldDefinitionRepository )
        {
            this.addressFormRepository = addressFormRepository;
            this.addressFieldDefinitionRepository = addressFieldDefinitionRepository;
        }

        public AddressFormQueryResult Handle(AddressFormByIdQuery message)
        {

            var addressForm = this.addressFormRepository.Get()
                .Include(addr => addr.AddressFieldDefinitions)
                .Include(addr => addr.AddressFieldDefinitions.Select(d=>d.AddressField))
                .Include(addr => addr.AddressFieldDefinitions.Select(d=>d.AddressFieldLabel))
                .SingleOrDefault(addr => addr.Id == message.Id);

            return Mapper.Map<AddressFormQueryResult>(addressForm);
        }
    }
}
