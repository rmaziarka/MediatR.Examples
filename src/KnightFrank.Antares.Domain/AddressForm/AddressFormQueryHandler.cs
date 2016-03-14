namespace KnightFrank.Antares.Domain.AddressForm
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;

    using MediatR;
    
    using Ninject.Extensions.Conventions.Syntax;

    public class AddressFormQueryHandler : IRequestHandler<AddressFormQuery, AddressFormQueryResult>
    {
        

        private readonly IReadGenericRepository<AddressForm> addressFormRepository;
        private readonly IReadGenericRepository<AddressFormEntityType> addressFormEntityTypeRepository;

        public AddressFormQueryHandler(
            IReadGenericRepository<AddressForm> addressFormRepository,
            IReadGenericRepository<AddressFormEntityType> addressFormEntityTypeRepository)
        {
            this.addressFormRepository = addressFormRepository;
            this.addressFormEntityTypeRepository = addressFormEntityTypeRepository;
        }

        public AddressFormQueryResult Handle(AddressFormQuery message)
        {
            AddressForm addressForm =
                this.addressFormEntityTypeRepository.Get()
                    .Where(
                        addressFormEntityType =>
                        addressFormEntityType.EnumTypeItem.EnumType.Code == message.EntityType
                        && addressFormEntityType.AddressForm.Country.Code == message.CountryCode)
                    .Select(addressFormEntityType =>addressFormEntityType.AddressForm)
                    .FirstOrDefault();

            if (addressForm != null)
            {
                return AutoMapper.Mapper.Map<AddressFormQueryResult>(addressForm);
            }
            
            return new AddressFormQueryResult();
        }

        
    }
}
