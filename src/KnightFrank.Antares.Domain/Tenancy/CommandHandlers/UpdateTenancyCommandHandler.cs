namespace KnightFrank.Antares.Domain.Tenancy.CommandHandlers
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Tenancy.Commands;

    using MediatR;
    
    public class UpdateTenancyCommandHandler : IRequestHandler<UpdateTenancyCommand, Guid>
    {
        private readonly IGenericRepository<Tenancy> tenancyRepository; 

        public UpdateTenancyCommandHandler(IGenericRepository<Tenancy> tenancyRepository)
        {
            this.tenancyRepository = tenancyRepository;
        }

        public Guid Handle(UpdateTenancyCommand message)
        {
            Tenancy tenancy = this.tenancyRepository.GetWithInclude(x => x.Id == message.TenancyId, 
                x=> x.Requirement,
                x=> x.Terms).First();

            return tenancy.Id;
        }

    }
}