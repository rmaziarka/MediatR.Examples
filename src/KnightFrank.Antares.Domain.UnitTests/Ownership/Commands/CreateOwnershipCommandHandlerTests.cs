namespace KnightFrank.Antares.Domain.UnitTests.Ownership.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Ownership.CommandHandlers;
    using KnightFrank.Antares.Domain.Ownership.Commands;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class CreateOwnershipCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectCommand_When_Handle_Then_ShouldReturnValidId(
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IDomainValidator<CreateOwnershipCommand>> ownershipDomainValidator,
            CreateOwnershipCommand command,
            CreateOwnershipCommandHandler commandHandler)
        {
            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(new List<Contact>().AsQueryable());
            ownershipDomainValidator.Setup(x=>x.Validate(It.IsAny<CreateOwnershipCommand>())).Returns(new ValidationResult());

            // Act
            commandHandler.Handle(command);

            // Assert
            ownershipRepository.VerifyAll();
            contactRepository.VerifyAll();
        }

        [Theory]
        [AutoMoqData]
        public void Given_OverlappingDates_When_Handle_Then_ShouldReturnDomainException(
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IDomainValidator<CreateOwnershipCommand>> ownershipDomainValidator,
            CreateOwnershipCommand command,
            CreateOwnershipCommandHandler commandHandler)
        {
            var result = new ValidationResult();
            result.Errors.Add(new ValidationFailure(It.IsAny<string>(), It.IsAny<string>()));
            ownershipDomainValidator.Setup(x => x.Validate(It.IsAny<CreateOwnershipCommand>())).Returns(result);
            
            // Act + Assert
            Assert.Throws<DomainValidationException>(() => commandHandler.Handle(command)).Message.Should().Be("Ownership dates overlap.");
        }
    }
}
