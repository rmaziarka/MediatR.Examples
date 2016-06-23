namespace KnightFrank.Antares.Domain.UnitTests.Company.QueryHandlers
{
    using System;
    using System.Linq;

    using Xunit;
    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Company.Queries;
    using KnightFrank.Antares.Domain.Company.QueryHandlers;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;


    [Collection("CompanyQueryHandler")]
    [Trait("FeatureTitle", "Company")]
    public class CompanyQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ExistingCompanyWithId_When_Handling_Then_ShouldReturnNotNullValue(
            Guid companyId,
            IFixture fixture,
            [Frozen] Mock<IReadGenericRepository<Company>> companyRepository,
            CompanyQuery query,
            CompanyQueryHandler handler
           )
        {
            // Arrange
            Company expectedCompany = fixture.Build<Company>()
                                             .With(c => c.Id, companyId)
                                             .Create();

            companyRepository.Setup(r => r.Get()).Returns(new[] { expectedCompany }.AsQueryable());
            query.Id = companyId;

            //Act
            Company company = handler.Handle(query);

            //Assert
            company.Should().NotBeNull();
            company.ShouldBeEquivalentTo(expectedCompany);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistingActivityWithId_When_Handling_Then_ShouldReturnNull(
            Guid companyId,
            CompanyQuery query,
            CompanyQueryHandler handler,
            [Frozen] Mock<IReadGenericRepository<Company>> companyRepository)
        {
            // Arrange
            companyRepository.Setup(r => r.Get()).Returns(new Company[] { }.AsQueryable());
            query.Id = companyId;

            //Act
            Company company = handler.Handle(query);

            //Assert
            company.Should().BeNull();
        }
    }
}
