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

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("UpdateCompanyComandHandler")]
    [Trait("FeatureTitle", "Company")]
    public class UpdateCompanyCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectCommand_When_Handle_Then_ShouldReturnValidId(
            [Frozen] Mock<IGenericRepository<Company>> companyRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            UpdateCompanyCommand command,
            UpdateCompanyCommandHandler handler)
        {
			// Arrange
			Company updatedCompany = this.CreateCompany(command.Id);

			companyRepository.Setup(r => r.GetWithInclude(It.IsAny<Expression<Func<Company, bool>>>(), 
														  It.IsAny<Expression<Func<Company, object>>[]>()))
							 .Returns(new List<Company> { updatedCompany });

			contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(command.Contacts.Select(x => new Contact { Id = x.Id }));

            // Act
            handler.Handle(command);

			// Assert
            companyRepository.Verify(x => x.GetWithInclude(It.IsAny<Expression<Func<Company, bool>>>(),
														  It.IsAny<Expression<Func<Company, object>>[]>()), Times.Once);
            companyRepository.Verify(x => x.Save(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_UpdateCompanyCommandWithInvalidContacts_When_Handle_Then_ShouldThrowBusinessException(
            [Frozen] Mock<IGenericRepository<Company>> companyRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            UpdateCompanyCommand command,
            UpdateCompanyCommandHandler handler,
            IFixture fixture)
        {
			// Arrange
			Company updatedCompany = this.CreateCompany(command.Id);

			companyRepository.Setup(r => r.GetWithInclude(It.IsAny<Expression<Func<Company, bool>>>(),
														  It.IsAny<Expression<Func<Company, object>>[]>()))
							 .Returns(new List<Company> { updatedCompany });

			command.Contacts = fixture.CreateMany<Contact>(2).ToList();

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(
                                 new List<Contact>()
                                 {
                                     fixture.Build<Contact>().With(x => x.Id, command.Contacts.Select(x=>x.Id).First()).Create()
                                 }
                );

            // Act + Assert
            var businessValidationException = Assert.Throws<BusinessValidationException>(() => { handler.Handle(command); });
            Assert.Equal(ErrorMessage.Missing_Company_Contacts_Id, businessValidationException.ErrorCode);
        }

	    private Company CreateCompany(Guid id)
	    {
		    return new Company
		    {
			    Id = id,
			    WebsiteUrl = "www.x.com",
			    ClientCarePageUrl = "www.y.com",
				Contacts = new List<Contact>()
		    };
	    }
    }
}