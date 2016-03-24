namespace KnightFrank.Antares.Domain.Property.Commands
{
    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Property;

    using MediatR;

    public class CreatePropertyCommand : IRequest<Property>
    {
        public CreateOrUpdatePropertyAddress Address { get; set; }
    }
}
