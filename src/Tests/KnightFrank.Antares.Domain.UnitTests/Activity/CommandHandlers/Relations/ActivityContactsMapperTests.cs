namespace KnightFrank.Antares.Domain.UnitTests.Activity.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Activity")]
    [Collection("ActivityContactsMapper")]
    public class ActivityContactsMapperTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [InlineAutoMoqData(0, 0)]
        [InlineAutoMoqData(0, 2)]
        [InlineAutoMoqData(1, 0)]
        [InlineAutoMoqData(2, 2)]
        [InlineAutoMoqData(3, 5)]
        public void Given_ValidCommand_When_Handling_Then_ShouldAssignContacts(
            int existingContacts,
            int contactsInCommand,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            ActivityContactsMapper mapper,
            IFixture fixture)
        {
            // Arrange
            int expectedDeletesCount = contactsInCommand != 0 ? existingContacts : 0;

            List<Guid> contactsIds = Enumerable.Range(0, contactsInCommand).Select(i => Guid.NewGuid()).ToList();
            var command = new UpdateActivityCommand { ContactIds = contactsIds };
            var activity = fixture.Create<Activity>();
            activity.Contacts = Enumerable.Range(0, existingContacts).Select(i => new Contact { Id = Guid.NewGuid() }).ToList();

            List<Contact> newContacts = contactsIds.Select(id => new Contact { Id = id }).ToList();
            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(newContacts);

            contactRepository.Setup(x => x.Delete(It.IsAny<Contact>())).Callback<Contact>(x => activity.Contacts.Remove(x));

            // Act
            mapper.ValidateAndAssign(command, activity);

            // Assert
            activity.Contacts.Should().BeEquivalentTo(newContacts);

            contactRepository.Verify(x => x.Delete(It.IsAny<Contact>()), Times.Exactly(expectedDeletesCount));
        }
    }
}
