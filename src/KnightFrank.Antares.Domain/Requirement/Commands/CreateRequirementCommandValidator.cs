namespace KnightFrank.Antares.Domain.Requirement.Commands
{
    using FluentValidation;

    public class CreateRequirementCommandValidator : AbstractValidator<CreateRequirementCommand>
    {
        public CreateRequirementCommandValidator()
        {
            this.RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(x => x.MinPrice).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MaxBedrooms).GreaterThanOrEqualTo(x => x.MinBedrooms).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MaxReceptionRooms).GreaterThanOrEqualTo(x => x.MinReceptionRooms).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MaxBathrooms).GreaterThanOrEqualTo(x => x.MinBathrooms).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MaxParkingSpaces).GreaterThanOrEqualTo(x => x.MinParkingSpaces).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MaxArea).GreaterThanOrEqualTo(x => x.MinArea).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MaxLandArea).GreaterThanOrEqualTo(x => x.MinLandArea).GreaterThanOrEqualTo(0);

            this.RuleFor(x => x.MinPrice).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MinBedrooms).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MinReceptionRooms).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MinBathrooms).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MinParkingSpaces).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MinArea).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MinLandArea).GreaterThanOrEqualTo(0);

            this.RuleFor(x => x.Description).Length(0, 4000);
        }
    }
}