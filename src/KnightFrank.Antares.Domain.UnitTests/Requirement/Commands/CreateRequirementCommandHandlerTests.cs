namespace KnightFrank.Antares.Domain.UnitTests.Requirement.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contact;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Requirement.CommandHandlers;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class CreateRequirementCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectCommand_When_Handle_Then_ShouldReturnValidId(
            [Frozen] Mock<IGenericRepository<Requirement>> requirementRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IDomainValidator<CreateRequirementCommand>> requirementDomainValidator,
            CreateRequirementCommand command,
            CreateRequirementCommandHandler commandHandler)
        {
            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(new List<Contact>().AsQueryable());
            requirementRepository.Setup(x => x.Add(It.IsAny<Requirement>()));
            requirementRepository.Setup(x => x.Save());

            commandHandler.Handle(command);

            requirementRepository.VerifyAll();
            contactRepository.VerifyAll();
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvalidValidationResult_When_Handle_Then_ShouldReturnDomainException(
            [Frozen] Mock<IGenericRepository<Requirement>> requirementRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IDomainValidator<CreateRequirementCommand>> requirementDomainValidator,
            CreateRequirementCommand command,
            CreateRequirementCommandHandler commandHandler)
        {
            var result = new ValidationResult();
            result.Errors.Add(new ValidationFailure(It.IsAny<string>(), It.IsAny<string>()));
            requirementDomainValidator.Setup(x => x.Validate(It.IsAny<CreateRequirementCommand>())).Returns(result);

            // Act + Assert
            Assert.Throws<DomainValidationException>(() => commandHandler.Handle(command));
        }
    }
}
