namespace KnightFrank.Antares.Domain.UnitTests.Company.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Company.CommandHandlers;
    using KnightFrank.Antares.Domain.Company.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CreateCompanyComandHandler")]
    [Trait("FeatureTitle", "Company")]
    public class CreateCompanyCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectCommand_When_Handle_Then_ShouldReturnValidId(
            [Frozen] Mock<IGenericRepository<Company>> companyRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            CreateCompanyCommand command,
            CreateCompanyCommandHandler handler)
        {
            // Arrange
            Company addedCompany = null;

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(command.ContactIds.Select(x => new Contact { Id = x }));

            companyRepository.Setup(x => x.Add(It.IsAny<Company>())).Callback<Company>(x => addedCompany = x);

            // Act
            handler.Handle(command);

            // Assert
            addedCompany.Contacts.Select(x => x.Id).ShouldAllBeEquivalentTo(command.ContactIds);
            companyRepository.Verify(x => x.Add(addedCompany), Times.Once);
            companyRepository.Verify(x => x.Save(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateCompanyCommandWithInvalidContacts_When_Handle_Then_ShouldThrowBusinessException(
            [Frozen] Mock<IGenericRepository<Company>> companyRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            CreateCompanyCommand command,
            CreateCompanyCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
            command.ContactIds = fixture.CreateMany<Guid>(2).ToList();

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(
                                 new List<Contact>()
                                 {
                                     fixture.Build<Contact>().With(x => x.Id, command.ContactIds.First()).Create()
                                 }
                );

            // Act + Assert
            var businessValidationException = Assert.Throws<BusinessValidationException>(() => { handler.Handle(command); });
            Assert.Equal(ErrorMessage.Missing_Company_Contacts_Id, businessValidationException.ErrorCode);
        }
    }
}