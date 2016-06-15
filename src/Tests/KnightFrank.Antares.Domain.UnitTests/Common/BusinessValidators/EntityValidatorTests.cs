namespace KnightFrank.Antares.Domain.UnitTests.Common.BusinessValidators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;

    using Xunit;

    [Collection("EntityValidator")]
    [Trait("FeatureTitle", "Common validators")]
    public class EntityValidatorTests
    {
        private readonly Mock<IGenericRepository<Contact>> contactRepository;
        private readonly EntityValidator validator;

        public EntityValidatorTests()
        {
            IFixture fixture = new Fixture().Customize();
            this.contactRepository = fixture.Freeze<Mock<IGenericRepository<Contact>>>();

            var ninjectInstanceResolverMock = fixture.Freeze<Mock<INinjectInstanceResolver>>();
            ninjectInstanceResolverMock.Setup(x => x.GetEntityGenericRepository<Contact>()).Returns(this.contactRepository.Object);

            this.validator = fixture.Create<EntityValidator>();
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExistingEntityId_When_CallingEntityExists_Then_ShouldNotThrowException(
           Contact contactToVerify,
           ICollection<Contact> contacts)
        {
            // Arrange
            contacts.Add(contactToVerify);
            this.contactRepository.Setup(x => x.Any(It.IsAny<Expression<Func<Contact, bool>>>()))
                .Returns((Expression<Func<Contact, bool>> expr) => contacts.Any(expr.Compile()));

            // Act Assert
            this.validator.EntityExists<Contact>(contactToVerify.Id);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistingEntityId_When_CallingEntityExists_Then_ShouldThrowException(
           Contact contactToVerify,
           ICollection<Contact> contacts)
        {
            // Arrange
            this.contactRepository.Setup(x => x.Any(It.IsAny<Expression<Func<Contact, bool>>>()))
                            .Returns((Expression<Func<Contact, bool>> expr) => contacts.Any(expr.Compile()));

            // Act Assert
            Assert.Throws<BusinessValidationException>(() => this.validator.EntityExists<Contact>(contactToVerify.Id));
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyEntities_When_CallingEntityExists_Then_ShouldThrowException(
           Contact contactToVerify)
        {
            // Arrange
            this.contactRepository.Setup(x => x.Any(It.IsAny<Expression<Func<Contact, bool>>>()))
                            .Returns((Expression<Func<Contact, bool>> expr) => new List<Contact>().Any(expr.Compile()));

            // Act Assert
            Assert.Throws<BusinessValidationException>(() => this.validator.EntityExists<Contact>(contactToVerify.Id));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExistingEntity_When_CallingEntityExists_Then_ShouldNotThrowException(
           Contact contactToVerify,
           ICollection<Contact> contacts)
        {
            // Act Assert
            this.validator.EntityExists(contactToVerify, contactToVerify.Id);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistingEntity_When_CallingEntityExists_Then_ShouldThrowException(
           ICollection<Contact> contacts)
        {
            // Act Assert
            Assert.Throws<BusinessValidationException>(() => this.validator.EntityExists<Contact>(null, default(Guid)));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExistingEntityIds_When_CallingEntitiesExist_Then_ShouldNotThrowException(
           ICollection<Contact> contacts)
        {
            // Arrange
            IList<Guid> contactIds = contacts.Select(c => c.Id).ToList();

            this.contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                            .Returns((Expression<Func<Contact, bool>> expr) => contacts.Where(expr.Compile()));

            // Act Assert
            this.validator.EntitiesExist<Contact>(contactIds);
        }

        [Theory]
        [AutoMoqData]
        public void Given_AnyNotExistingEntityIds_When_CallingEntitiesExist_Then_ShouldThrowException(
           Contact notExistingContact,
           ICollection<Contact> contacts)
        {
            // Arrange
            IList<Guid> contactIds = contacts.Select(c => c.Id).ToList();
            contactIds.Add(notExistingContact.Id);

            this.contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                            .Returns((Expression<Func<Contact, bool>> expr) => contacts.Where(expr.Compile()));

            // Act Assert
            Assert.Throws<BusinessValidationException>(() => this.validator.EntitiesExist<Contact>(contactIds));
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyEntities_When_CallingEntitiesExist_Then_ShouldThrowException(
           Contact notExistingContact)
        {
            // Arrange
            IList<Guid> contactIds = new List<Guid> { notExistingContact.Id };
            this.contactRepository.Setup(x => x.Any(It.IsAny<Expression<Func<Contact, bool>>>()))
                            .Returns((Expression<Func<Contact, bool>> expr) => new List<Contact>().Any(expr.Compile()));

            // Act Assert
            Assert.Throws<BusinessValidationException>(() => this.validator.EntitiesExist<Contact>(contactIds));
        }
    }
}