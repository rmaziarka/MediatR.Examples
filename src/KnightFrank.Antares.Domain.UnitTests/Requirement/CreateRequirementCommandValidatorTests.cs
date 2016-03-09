namespace KnightFrank.Antares.Domain.UnitTests
{

    using FluentValidation.TestHelper;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Xunit;

    public class CreateRequirementCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private CreateRequirementCommandValidator validator;

        [Fact]
        public void Should_have_validation_error_when_min_price_is_less_than_zero()
        {
            this.validator = new CreateRequirementCommandValidator();
            this.validator.ShouldHaveValidationErrorFor(x => x.MinPrice, -1);
        }

        [Fact]
        public void Should_have_validation_error_when_max_price_is_less_than_zero()
        {
            this.validator = new CreateRequirementCommandValidator();
            this.validator.ShouldHaveValidationErrorFor(x => x.MaxPrice, -1);
        }

        [Fact]
        public void Should_have_validation_error_when_max_price_is_less_than_min_price()
        {
            this.validator = new CreateRequirementCommandValidator();

            var command = new CreateRequirementCommand
            {
                MinPrice = 10000,
                MaxPrice = 9999
            };

            this.validator.ShouldHaveValidationErrorFor(x => x.MaxPrice, command);
        }
    }
}
