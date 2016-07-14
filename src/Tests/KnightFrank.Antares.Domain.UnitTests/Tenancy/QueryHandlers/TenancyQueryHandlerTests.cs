namespace KnightFrank.Antares.Domain.UnitTests.Tenancy.QueryHandlers
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Tenancy.Queries;
    using KnightFrank.Antares.Domain.Tenancy.QueryHandlers;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("TenancyQueryHandler")]
    [Trait("FeatureTitle", "Tenancy")]
    public class TenancyQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ExistingTenancyWithId_When_Handling_Then_ShouldReturnNotNullValue(
            Guid tenancyId,
            [Frozen] Mock<IReadGenericRepository<Tenancy>> tenancyRepository,
            TenancyQueryHandler handler,
            IFixture fixture)
        {
            // Arrange
            Tenancy expectedTenancy = fixture.Build<Tenancy>().With(a => a.Id, tenancyId).Create();
            var query = new TenancyQuery { Id = tenancyId };
            tenancyRepository.Setup(r => r.Get()).Returns(new[] { expectedTenancy }.AsQueryable());

            // Act
            Tenancy tenancy = handler.Handle(query);

            // Assert
            tenancy.Should().NotBeNull();
            tenancy.ShouldBeEquivalentTo(expectedTenancy);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistingTenancyWithId_When_Handling_Then_ShouldReturnNull(
            TenancyQuery query,
            [Frozen] Mock<IReadGenericRepository<Tenancy>> tenancyRepository,
            TenancyQueryHandler handler)
        {
            // Arrange
            tenancyRepository.Setup(r => r.Get()).Returns(new Tenancy[] { }.AsQueryable());

            // Act
            Tenancy tenancy = handler.Handle(query);

            // Assert
            tenancy.Should().BeNull();
        }
    }
}
