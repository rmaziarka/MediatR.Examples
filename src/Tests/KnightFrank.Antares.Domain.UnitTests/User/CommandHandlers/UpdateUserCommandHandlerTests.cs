namespace KnightFrank.Antares.Domain.UnitTests.User.CommandHandlers
{
    using System;

    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.User.CommandHandlers;
    using KnightFrank.Antares.Domain.User.Commands;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("UpdateUserCommandHandler")]
    [Trait("FeatureTitle", "User")]
    public class UpdateUserCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_UpdateUserCommand_When_Handle_Then_ShouldUpdateUser(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeValidator,
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            UpdateUserCommand command,
            UpdateUserCommandHandler handler,
            User user)
        {
            userRepository.Setup(r => r.GetById(command.Id)).Returns(user);

            // Act
            Guid userId = handler.Handle(command);

            // Assert
            Assert.Equal(command.Id, userId);
            entityValidator.Verify(x => x.EntityExists(user, command.Id), Times.Once);
            enumTypeValidator.Verify(x => x.ItemExists(EnumType.SalutationFormat, (Guid)command.SalutationFormatId), Times.Once);
            userRepository.Verify(r => r.Save(), Times.Once());
        }
    }
}
