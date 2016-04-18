namespace KnightFrank.Antares.Domain.UnitTests.RequirementNote.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.RequirementNote.Commands;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class CreateRequirementNoteCommandDomainValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidCreateRequirementNoteCommand_When_Validating_Then_IsValid(
            [Frozen] Mock<IGenericRepository<Requirement>> requirementRepository,
            Requirement requirement,
            CreateRequirementNoteCommandDomainValidator validator,
            CreateRequirementNoteCommand cmd)
        {
            // Arrange
            requirementRepository.Setup(p => p.GetById(It.IsAny<Guid>())).Returns(requirement);

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeTrue();
            requirementRepository.Verify(p => p.GetById(cmd.RequirementId), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_RequirementDoesNotExist_When_Validating_Then_IsInvalid(
            [Frozen] Mock<IGenericRepository<Requirement>> requirementRepository,
            CreateRequirementNoteCommandDomainValidator validator,
            CreateRequirementNoteCommand cmd)
        {
            // Arrange
            requirementRepository.Setup(p => p.GetById(It.IsAny<Guid>())).Returns(default(Requirement));

            // Act
            ValidationResult validationResult = validator.Validate(cmd);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.RequirementId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorMessage == "Invalid requirement has been provided.");
            requirementRepository.Verify(p => p.GetById(cmd.RequirementId), Times.Once);
        }
    }
}
