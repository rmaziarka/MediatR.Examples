namespace KnightFrank.Antares.Domain.Property.Commands
{
    using KnightFrank.Antares.Dal.Model;

    using MediatR;

    public class CreatePropertyCommand : IRequest<Property>
    {
        public CreateOrUpdatePropertyAddress Address { get; set; }
    }
}
