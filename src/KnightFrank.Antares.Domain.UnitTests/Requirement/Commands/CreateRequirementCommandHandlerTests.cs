namespace KnightFrank.Antares.Domain.UnitTests.Requirement.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Contact;
    using KnightFrank.Antares.Domain.Requirement.CommandHandlers;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Moq;

    using Xunit;

    public class CreateRequirementCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Fact]
        public void HandleWhenCalledShouldReturnValidId()
        {
            var requirementRepositoryMock = new Mock<IGenericRepository<Dal.Model.Requirement>>();
            var contactRepositoryMock = new Mock<IReadGenericRepository<Contact>>();
            var commandHandler = new CreateRequirementCommandHandler(requirementRepositoryMock.Object, contactRepositoryMock.Object);

            var command = new CreateRequirementCommand
            {
                CreateDate = DateTime.UtcNow,
                Contacts = new Collection<ContactDto>
                {
                    new ContactDto { Id = Guid.NewGuid() },
                    new ContactDto { Id = Guid.NewGuid() }
                }
            };
            
            contactRepositoryMock.Setup(x => x.Get()).Returns(new List<Contact>().AsQueryable());
            requirementRepositoryMock.Setup(x => x.Add(It.IsAny<Dal.Model.Requirement>()));
            requirementRepositoryMock.Setup(x => x.Save());

            commandHandler.Handle(command);

            requirementRepositoryMock.VerifyAll();
            contactRepositoryMock.VerifyAll();
        }
    }
}
