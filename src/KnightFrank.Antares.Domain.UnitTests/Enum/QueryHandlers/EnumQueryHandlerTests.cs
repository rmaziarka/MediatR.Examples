namespace KnightFrank.Antares.Domain.UnitTests.Enum.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Enum.Queries;
    using KnightFrank.Antares.Domain.Enum.QueryHandlers;
    using KnightFrank.Antares.Domain.Enum.QueryResults;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("Enums")]
    [Trait("FeatureTitle", "Enums")]
    public class EnumQueryHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [InlineAutoMoqData("en")]
        public void Given_EnumQueryHandler_When_Handling_Then_CorrectResultShouldBeReturned(
            string isoCode,     
            [Frozen] Mock<IReadGenericRepository<EnumLocalised>> enumLocalisedRepository,
            EnumQuery query,
            EnumQueryHandler handler,
            List<EnumLocalised> mockedData,
            IFixture fixture)
        {
            // Arrange
            Locale locale = fixture.Build<Locale>().With(x => x.IsoCode, isoCode).Create();

            EnumType enumType = fixture.Build<EnumType>().With(x => x.Code, query.Code).Create();

            EnumTypeItem enumTypeItem = fixture.Build<EnumTypeItem>().With(x => x.EnumType, enumType).Create();

            EnumLocalised enumLocalisedFirst =
                fixture.Build<EnumLocalised>().With(x => x.Locale, locale).With(x => x.EnumTypeItem, enumTypeItem).Create();

            EnumLocalised enumLocalisedSecond =
                fixture.Build<EnumLocalised>().With(x => x.Locale, locale).With(x => x.EnumTypeItem, enumTypeItem).Create();

            mockedData.AddRange(new[] { enumLocalisedFirst, enumLocalisedSecond });

            var expectedResult = new List<EnumQueryItemResult>
                                     {
                                         new EnumQueryItemResult { Value = enumLocalisedFirst.Value },
                                         new EnumQueryItemResult { Value = enumLocalisedSecond.Value }
                                     };

            enumLocalisedRepository.Setup(x => x.Get()).Returns(mockedData.AsQueryable());

            // Act
            EnumQueryResult queryResult = handler.Handle(query);

            // Asserts
            queryResult.Items.Should().HaveCount(2);
            queryResult.Items.Should().Equal(expectedResult, (x1, x2) => x1.Value == x2.Value);
        }
    }
}
