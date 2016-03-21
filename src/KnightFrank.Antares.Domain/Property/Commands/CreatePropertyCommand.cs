namespace KnightFrank.Antares.Domain.Property.Commands
{
    using KnightFrank.Antares.Dal.Model;

    using MediatR;

    public class CreatePropertyCommand : IRequest<Property>
    {
        public CreatePropertyAddress Address { get; set; }
    }
}
