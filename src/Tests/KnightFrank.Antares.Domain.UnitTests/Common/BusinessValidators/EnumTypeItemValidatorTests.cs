namespace KnightFrank.Antares.Domain.UnitTests.Common.BusinessValidators
{
    using System;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("EnumTypeItemValidator")]
    [Trait("FeatureTitle", "Common validators")]
    public class EnumTypeItemValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_NonExistingEnumTypeItemForEnumType_When_Validating_Then_ShouldThrowException(
           Guid enumTypeItemId,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           EnumTypeItemValidator validator,
           Domain.Common.Enums.EnumType enumType)
        {
            // Arrange
            enumTypeItemRepository.Setup(x => x.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(false);

            // Act Assert
            Assert.ThrowsAny<BusinessValidationException>(() => validator.ItemExists(enumType, enumTypeItemId));
        }
    }
}