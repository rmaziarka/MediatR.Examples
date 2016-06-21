namespace KnightFrank.Antares.Domain.UnitTests.Enum.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Enum.Queries;
    using KnightFrank.Antares.Domain.Enum.QueryHandlers;
    using KnightFrank.Antares.Domain.Enum.QueryResults;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("EnumItemQueryHandler")]
    [Trait("FeatureTitle", "Enums")]
    public class EnumItemQueryHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ExistEnumType_When_Handling_Then_ShouldReturnDictionaryOfEnumtItems(
            [Frozen] Mock<IReadGenericRepository<EnumType>> enumTypeRepository,
            EnumItemQuery query,
            EnumItemQueryHandler handler,
            EnumType enumType,
            EnumTypeItem enumTypeItem)
        {
            // Arrange
            enumType.EnumTypeItems = new List<EnumTypeItem>();
            enumType.EnumTypeItems.Add(enumTypeItem);

            List<EnumItemResult> enumItemResults = enumType.EnumTypeItems.Select(eti => Mapper.Map<EnumItemResult>(eti)).ToList();
            enumTypeRepository.Setup(r => r.GetWithInclude(et => et.EnumTypeItems)).Returns(new[] { enumType }.AsQueryable());

            // Act
            Dictionary<string, ICollection<EnumItemResult>> dictionary = handler.Handle(query);

            // Assert
            dictionary.Should().NotBeNull();
            dictionary.Should().HaveCount(1);
            dictionary.Should().ContainKey(enumType.Code);
            dictionary[enumType.Code].Should().NotBeNull();
            dictionary[enumType.Code].Should().HaveCount(enumType.EnumTypeItems.Count);
            dictionary[enumType.Code].ShouldAllBeEquivalentTo(enumItemResults);
        }
    }
}
