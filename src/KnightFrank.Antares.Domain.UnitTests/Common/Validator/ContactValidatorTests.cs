namespace KnightFrank.Antares.Domain.UnitTests.Common.Validator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Validator;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Company")]
    [Collection("ContactValidator")]
    public class ContactValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_AtLeastOneContactIdDoesNotExist_When_Validating_Then_IsInvalidAndHasAppropriateErrorCode(
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            ContactValidator validator, 
            ICollection<Contact> fakeContacts)
        {
            // Arrange
            List<Guid> input = fakeContacts.Select(c => c.Id).ToList();
            input.Add(new Guid());
            contactRepository.Setup(r => r.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(fakeContacts);

            // Act
            ValidationResult validationResult = validator.Validate(input);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(p => p.ErrorCode == "contactsinvalid_error");
        }

        [Theory]
        [AutoMoqData]
        public void Given_ContactIdsIsEmpty_When_Validating_Then_IsValid(
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            ContactValidator validator,
            ICollection<Contact> fakeContacts)
        {
            // Arrange
            var input = new List<Guid>();
            contactRepository.Setup(r => r.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns((Expression<Func<Contact, bool>> expr) => fakeContacts.Where(expr.Compile()));

            // Act
            ValidationResult validationResult = validator.Validate(input);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidContactIds_When_Validating_Then_IsValid(
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            ContactValidator validator,
            ICollection<Contact> fakeContacts)
        {
            // Arrange
            List<Guid> input = fakeContacts.Select(c => c.Id).ToList();
            contactRepository.Setup(r => r.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(fakeContacts);

            // Act
            ValidationResult validationResult = validator.Validate(input);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }
    }
}