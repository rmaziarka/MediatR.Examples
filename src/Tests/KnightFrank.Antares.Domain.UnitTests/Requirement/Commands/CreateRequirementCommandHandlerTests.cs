namespace KnightFrank.Antares.Domain.UnitTests.Requirement.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Commands;
    using KnightFrank.Antares.Domain.Requirement.CommandHandlers;
    using KnightFrank.Antares.Domain.Requirement.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

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
            [Frozen] Mock<IGenericRepository<RequirementType>> requirementTypeRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IAddressValidator> addressValidator,
            [Frozen] Mock<IAttributeValidator<Tuple<Domain.Common.Enums.RequirementType>>> attributeValidator,
            CreateRequirementCommand command,
            CreateRequirementCommandHandler commandHandler,
            IFixture fixture)
        {
            // Arrange
            command.ContactIds = fixture.CreateMany<Guid>(2).ToList();
            var requirementType = new RequirementType { Id = command.RequirementTypeId, EnumCode = "ResidentialLetting"};

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(
                command.ContactIds.Select(x => new Contact { Id = x }));

            requirementTypeRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(requirementType);

            requirementRepository.Setup(x => x.Add(It.IsAny<Requirement>()));
            requirementRepository.Setup(x => x.Save());

            // Act
            commandHandler.Handle(command);

            // Assert
            requirementRepository.VerifyAll();
            requirementTypeRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once());
            contactRepository.VerifyAll();
            attributeValidator.Verify(x => x.Validate(It.IsAny<PageType>(), It.IsAny<Tuple<Domain.Common.Enums.RequirementType>>(), It.IsAny<CreateRequirementCommand>()), Times.Once);
            addressValidator.Verify(x => x.Validate(It.IsAny<CreateOrUpdateAddress>()), Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateRequirementCommandWithInvalidApplicants_When_Handle_Then_ShouldThrowBusinessException(
        [Frozen] Mock<IGenericRepository<Requirement>> requirementRepository,
        [Frozen] Mock<IGenericRepository<RequirementType>> requirementTypeRepository,
        [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
        [Frozen] Mock<IAddressValidator> addressValidator,
        CreateRequirementCommand command,
        CreateRequirementCommandHandler handler,
        IFixture fixture)
        {
            // Arrange
            command.ContactIds = fixture.CreateMany<Guid>(2).ToList();
            var requirementType = new RequirementType { Id = command.RequirementTypeId, EnumCode = "ResidentialLetting" };

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(
                new List<Contact>()
                {
                    fixture.Build<Contact>().With(x => x.Id, command.ContactIds.First()).Create()
                });

            requirementTypeRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(requirementType);

            // Act
            Action act = () => handler.Handle(command);

            // Assert
            act.ShouldThrow<BusinessValidationException>().And.ErrorCode.ShouldBeEquivalentTo(ErrorMessage.Missing_Requirement_Applicants_Id);
        }
    }
}
