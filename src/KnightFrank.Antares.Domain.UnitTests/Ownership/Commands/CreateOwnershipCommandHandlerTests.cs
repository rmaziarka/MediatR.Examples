namespace KnightFrank.Antares.Domain.UnitTests.Ownership.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Ownership.CommandHandlers;
    using KnightFrank.Antares.Domain.Ownership.Commands;

    using Moq;

    using Xunit;

    public class CreateOwnershipCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Fact]
        public void HandleWhenCalledShouldReturnValidId()
        {
            // Arrange
            var ownershipRepositoryMock = new Mock<IGenericRepository<Ownership>>();
            var contactRepositoryMock = new Mock<IGenericRepository<Contact>>();
            var commandHandler = new CreateOwnershipCommandHandler(ownershipRepositoryMock.Object, contactRepositoryMock.Object);

            var command = new CreateOwnershipCommand
            {
                ContactIds = new Collection<Guid>
                {
                    Guid.NewGuid(),
                    Guid.NewGuid()
                }
            };

            contactRepositoryMock.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(new List<Contact>().AsQueryable());
            ownershipRepositoryMock.Setup(x => x.Add(It.IsAny<Ownership>()));
            ownershipRepositoryMock.Setup(x => x.Save());

            // Act
            commandHandler.Handle(command);

            // Assert
            ownershipRepositoryMock.VerifyAll();
            contactRepositoryMock.VerifyAll();
        }
    }
}
