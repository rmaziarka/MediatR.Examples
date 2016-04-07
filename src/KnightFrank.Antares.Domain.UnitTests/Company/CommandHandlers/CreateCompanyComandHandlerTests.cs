namespace KnightFrank.Antares.Domain.UnitTests.Company.CommandHandlers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Company.Command;
    using KnightFrank.Antares.Domain.Company.CommandHandlers;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CreateCompanyComandHandler")]
    [Trait("FeatureTitle", "Company")]
    public class CreateCompanyComandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectCommand_When_Handle_Then_ShouldReturnValidId(
            [Frozen] Mock<IGenericRepository<Company>> companyRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IDomainValidator<CreateCompanyCommand>> createCompanyCommandDomainValidator,
            CreateCompanyCommand command,
            CreateCompanyCommandHandler comandHandler)
        {
            Company addedCompany = null;

            createCompanyCommandDomainValidator.Setup(x => x.Validate(It.IsAny<CreateCompanyCommand>()))
                                               .Returns(new ValidationResult());

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(command.ContactIds.Select(x => new Contact { Id = x }));

            companyRepository.Setup(x => x.Add(It.IsAny<Company>())).Callback<Company>(x => addedCompany = x);

            comandHandler.Handle(command);

            addedCompany.Contacts.Select(x => x.Id).ShouldAllBeEquivalentTo(command.ContactIds);
            companyRepository.Verify(x => x.Add(addedCompany), Times.Once);
            companyRepository.Verify(x => x.Save(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectCommand_When_Handle_Then_ShouldThrowDomainException(
            [Frozen] Mock<IGenericRepository<Company>> companyRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IDomainValidator<CreateCompanyCommand>> createCompanyCommandDomainValidator,
            CreateCompanyCommand command,
            CreateCompanyCommandHandler commandHandler)
        {
            createCompanyCommandDomainValidator.Setup(x => x.Validate(It.IsAny<CreateCompanyCommand>()))
                                               .Returns(ValidationResultBuilder.BuildValidationResult());

            Action act = () => commandHandler.Handle(command);

            act.ShouldThrow<DomainValidationException>();
            companyRepository.Verify(x => x.Add(It.IsAny<Company>()), Times.Never);
            companyRepository.Verify(x => x.Save(), Times.Never);
        }
    }
}