namespace KnightFrank.Antares.Domain.UnitTests.Common.BusinessValidators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("ActivityTypeDefinitionValidator")]
    [Trait("FeatureTitle", "ActivityType")]
    public class ActivityTypeDefinitionValidatorTest : IClassFixture<BaseTestClassFixture>
    {
        private readonly List<ActivityTypeDefinition> activityTypeDefinitionsMock;
        private readonly Mock<IGenericRepository<ActivityTypeDefinition>> activityTypeDefinitionRepository;
        public ActivityTypeDefinitionValidatorTest()
        {
            Fixture fixture = new Fixture().Customize();
            this.activityTypeDefinitionsMock = fixture.CreateMany<ActivityTypeDefinition>().ToList();
            this.activityTypeDefinitionRepository = fixture.Freeze<Mock<IGenericRepository<ActivityTypeDefinition>>>();
        }
        [Theory]
        [InlineAutoMoqData]
        public void Given_ValidActivityTypeDefinition_Then_ShouldBeNoExceptionThrown(
            [Frozen]Mock<IGenericRepository<ActivityTypeDefinition>> repository,
            ActivityTypeDefinitionValidator activityTypeDefinitionValidator,
            ActivityTypeDefinition expectedActivityTypeDefinition)
        {
            //Arrange
            this.activityTypeDefinitionsMock.Add(expectedActivityTypeDefinition);
            repository.Setup(x => x.Any(It.IsAny<Expression<Func<ActivityTypeDefinition, bool>>>()))
                .Returns((Expression<Func<ActivityTypeDefinition, bool>> expr) => this.activityTypeDefinitionsMock.Any(expr.Compile()));

            //Act
            activityTypeDefinitionValidator.Validate(expectedActivityTypeDefinition.ActivityTypeId, expectedActivityTypeDefinition.CountryId, expectedActivityTypeDefinition.PropertyTypeId);

            //Assert
            repository.Verify(x => x.Any(It.IsAny<Expression<Func<ActivityTypeDefinition, bool>>>()), Times.Once);
        }

        [Theory]
        [InlineAutoMoqData]
        public void Given_InvalidActivityTypeIdActivityTypeDefinition_Then_ShouldBeNoExceptionThrown(
            ActivityTypeDefinitionValidator activityTypeDefinitionValidator,
            ActivityTypeDefinition expectedActivityTypeDefinition)
        {
            //Arrange
            this.activityTypeDefinitionsMock.Add(expectedActivityTypeDefinition);
            this.activityTypeDefinitionRepository
                .Setup(x => x.Any(It.IsAny<Expression<Func<ActivityTypeDefinition, bool>>>()))
                .Returns((Expression<Func<ActivityTypeDefinition, bool>> expr) => this.activityTypeDefinitionsMock.Any(expr.Compile()));


            //Act & Assert
            Assert.Throws<BusinessValidationException>(() => activityTypeDefinitionValidator.Validate(Guid.Empty, expectedActivityTypeDefinition.CountryId, expectedActivityTypeDefinition.PropertyTypeId));
        }


        [Theory]
        [InlineAutoMoqData]
        public void Given_InvalidCountryIdActivityTypeDefinition_Then_ShouldBeNoExceptionThrown(
            ActivityTypeDefinitionValidator activityTypeDefinitionValidator,
            ActivityTypeDefinition expectedActivityTypeDefinition)
        {
            //Arrange
            this.activityTypeDefinitionsMock.Add(expectedActivityTypeDefinition);
            this.activityTypeDefinitionRepository
                .Setup(x => x.Any(It.IsAny<Expression<Func<ActivityTypeDefinition, bool>>>()))
                .Returns((Expression<Func<ActivityTypeDefinition, bool>> expr) => this.activityTypeDefinitionsMock.Any(expr.Compile()));


            //Act & Assert
            Assert.Throws<BusinessValidationException>(() => activityTypeDefinitionValidator.Validate(expectedActivityTypeDefinition.ActivityTypeId, Guid.Empty, expectedActivityTypeDefinition.PropertyTypeId));
        }

        [Theory]
        [InlineAutoMoqData]
        public void Given_InvalidPropertyTypeIdActivityTypeDefinition_Then_ShouldBeNoExceptionThrown(
            ActivityTypeDefinitionValidator activityTypeDefinitionValidator,
            ActivityTypeDefinition expectedActivityTypeDefinition)
        {
            //Arrange
            this.activityTypeDefinitionsMock.Add(expectedActivityTypeDefinition);
            this.activityTypeDefinitionRepository
                .Setup(x => x.Any(It.IsAny<Expression<Func<ActivityTypeDefinition, bool>>>()))
                .Returns((Expression<Func<ActivityTypeDefinition, bool>> expr) => this.activityTypeDefinitionsMock.Any(expr.Compile()));


            //Act & Assert
            Assert.Throws<BusinessValidationException>(() => activityTypeDefinitionValidator.Validate(expectedActivityTypeDefinition.ActivityTypeId, expectedActivityTypeDefinition.CountryId, Guid.Empty));
        }
    }
}
