namespace KnightFrank.Antares.Domain.UnitTests.Tenancy.CommandHandlers.Relations
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Domain.Tenancy.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Tenancy.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Xunit;

    public class TenancyTermsMapperTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CommandWithNoTerm_When_Assigning_Then_ShouldUpdateTerm(
            Tenancy tenancy,
            CreateTenancyCommand command,
            TenancyTermsMapper mapper)
        {
            // Arrange
            tenancy.Terms = new List<TenancyTerm>();

            // Act
            mapper.ValidateAndAssign(command, tenancy);

            // Assert
            TenancyTerm tenancyTerm = tenancy.Terms.FirstOrDefault();
            tenancyTerm.Should().NotBe(null);

            tenancyTerm.ShouldBeEquivalentTo(command.Term, opt => opt.ExcludingMissingMembers());
        }

        [Theory]
        [AutoMoqData]
        public void Given_CommandWithTerm_When_Assigning_Then_ShouldUpdateTerm(
            Tenancy tenancy,
            TenancyTerm tenancyTerm,
            CreateTenancyCommand command,
            TenancyTermsMapper mapper)
        {
            // Arrange
            tenancy.Terms = new List<TenancyTerm> { tenancyTerm };

            // Act
            mapper.ValidateAndAssign(command, tenancy);

            // Assert
            tenancy.Terms.FirstOrDefault().Should().Be(tenancyTerm);
            tenancyTerm.ShouldBeEquivalentTo(command.Term, opt => opt.ExcludingMissingMembers());
        }
    }
}