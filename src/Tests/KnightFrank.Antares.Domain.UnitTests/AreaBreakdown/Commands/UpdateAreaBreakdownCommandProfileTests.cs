namespace KnightFrank.Antares.Domain.UnitTests.AreaBreakdown.Commands
{
    using AutoMapper;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Ploeh.AutoFixture;

    using Xunit;

    [Trait("FeatureTitle", "AreaBreakdown")]
    [Collection("UpdateAreaBreakdownCommandProfile")]
    public class UpdateAreaBreakdownCommandProfileTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_UpdateAreaBreakdownCommand_When_Mapping_Then_ShouldUpdateOnlyNameAndSize(
            UpdateAreaBreakdownCommand command,
            string name,
            int size,
            IFixture fixture)
        {
            // Arrange
            PropertyAreaBreakdown propertyAreaBreakdown =
                fixture.Build<PropertyAreaBreakdown>().With(a => a.Name, name).With(a => a.Size, size).Create();

            // Act
            PropertyAreaBreakdown result = Mapper.Map(command, propertyAreaBreakdown);

            // Asserts
            result.Should().NotBeNull();
            result.Id.Should().Be(propertyAreaBreakdown.Id);
            result.Id.Should().NotBe(command.Id);
            result.Name.Should().Be(command.Name);
            result.Name.Should().NotBe(name);
            result.Order.Should().Be(propertyAreaBreakdown.Order);
            result.PropertyId.Should().Be(propertyAreaBreakdown.PropertyId);
            result.PropertyId.Should().NotBe(command.PropertyId);
            result.Size.Should().Be(command.Size);
            result.Size.Should().NotBe(size);
        }
    }
}
