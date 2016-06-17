namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    using Ploeh.AutoFixture;

    using Xunit;

    public class UpdateActivityDepartmentValidatorTests
    {
        private readonly UpdateActivityDepartmentValidator validator;

        public UpdateActivityDepartmentValidatorTests()
        {
            IFixture fixture = new Fixture().Customize();
            this.validator = fixture.Create<UpdateActivityDepartmentValidator>();
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityDepartment_When_Validating_Then_IsValid(UpdateActivityDepartment data)
        {
            // Act
            ValidationResult validationResult = this.validator.Validate(data);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvaliUpdateActivityDepartmentWithEmptyDepartmentId_When_Validating_Then_IsInvalid(
            UpdateActivityDepartment data)
        {
            // Arrange
            data.DepartmentId = Guid.Empty;

            // Act
            ValidationResult validationResult = this.validator.Validate(data);

            // Assert
            validationResult.IsInvalid(nameof(data.DepartmentId), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvaliUpdateActivityDepartmentWithEmptyDepartmentTypeId_When_Validating_Then_IsInvalid(
            UpdateActivityDepartment data)
        {
            // Arrange
            data.DepartmentTypeId = Guid.Empty;

            // Act
            ValidationResult validationResult = this.validator.Validate(data);

            // Assert
            validationResult.IsInvalid(nameof(data.DepartmentTypeId), nameof(Messages.notempty_error));
        }
    }
}
