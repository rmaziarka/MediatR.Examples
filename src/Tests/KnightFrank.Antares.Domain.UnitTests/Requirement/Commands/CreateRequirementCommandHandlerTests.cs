namespace KnightFrank.Antares.Domain.UnitTests.Requirement.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Commands;
    using KnightFrank.Antares.Domain.Requirement.CommandHandlers;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class CreateRequirementCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectCommand_When_Handle_Then_ShouldCreateRequirement(
            [Frozen] Mock<IGenericRepository<Requirement>> requirementRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IAddressValidator> addressValidator,
            CreateRequirementCommand command,
            CreateRequirementCommandHandler commandHandler,
            IFixture fixture)
        {
            // Arrange
            command.ContactIds = fixture.CreateMany<Guid>(2).ToList();

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(
                command.ContactIds.Select(x => new Contact { Id = x }));

            requirementRepository.Setup(x => x.Add(It.IsAny<Requirement>()));
            requirementRepository.Setup(x => x.Save());

            // Act
            commandHandler.Handle(command);

            // Assert
            requirementRepository.VerifyAll();
            contactRepository.VerifyAll();
            addressValidator.Verify(x => x.Validate(It.IsAny<CreateOrUpdateAddress>()), Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateRequirementCommandWithInvalidApplicants_When_Handle_Then_ShouldThrowBusinessException(
        [Frozen] Mock<IGenericRepository<Requirement>> requirementRepository,
        [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
        [Frozen] Mock<IAddressValidator> addressValidator,
        CreateRequirementCommand command,
        CreateRequirementCommandHandler handler,
        IFixture fixture)
        {
            // Arrange
            command.ContactIds = fixture.CreateMany<Guid>(2).ToList();

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(
                new List<Contact>()
                {
                    fixture.Build<Contact>().With(x => x.Id, command.ContactIds.First()).Create()
                });

            // Act + Assert
            var businessValidationException = Assert.Throws<BusinessValidationException>(() => { handler.Handle(command); });
            Assert.Equal(ErrorMessage.Missing_Requirement_Applicants_Id, businessValidationException.ErrorCode);
        }
    }
}
