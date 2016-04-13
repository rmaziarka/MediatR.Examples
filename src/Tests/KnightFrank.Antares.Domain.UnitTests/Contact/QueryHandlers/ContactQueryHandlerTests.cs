namespace KnightFrank.Antares.Domain.UnitTests.Contact.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Contact.Queries;
    using KnightFrank.Antares.Domain.Contact.QueryHandlers;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("ContactQueryHandler")]
    [Trait("FeatureTitle", "Contacts")]
    public class ContactQueryHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ExistingContactIdInQuery_When_Handling_Then_ShouldReturnNotNullValue(
            [Frozen] Mock<IReadGenericRepository<Contact>> contactRepository,
            IList<Contact> mockedData,
            Contact expectedContact,
            ContactQuery query,
            ContactQueryHandler handler)
        {
            expectedContact.Id = query.Id;
            mockedData.Add(expectedContact);
            contactRepository.Setup(x => x.Get()).Returns(mockedData.AsQueryable());

            Contact contact = handler.Handle(query);

            contact.Should().NotBeNull();
            contact.ShouldBeEquivalentTo(expectedContact);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistingContactInQuery_When_Handling_Then_ShouldReturnNullValue(
            [Frozen] Mock<IReadGenericRepository<Contact>> contactRepository,
            IList<Contact> mockedData,
            ContactQuery query,
            ContactQueryHandler handler)
        {
            contactRepository.Setup(x => x.Get()).Returns(mockedData.AsQueryable());

            Contact contact = handler.Handle(query);

            contact.Should().BeNull();
        }
    }
}