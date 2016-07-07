namespace KnightFrank.Antares.Domain.UnitTests.User.QueryHandlers
{
    using System;
    using System.Linq;

    using Xunit;
    using FluentAssertions;
    using Moq;

    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.User.Queries;
    using KnightFrank.Antares.Domain.User.QueryHandlers;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    [Collection("UserQueryHandler")]
    [Trait("FeatureTitle", "Users")]
    public class UserQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ExistingUser_When_Handling_Then_CorrectUserReturned(
           Guid searchByUserId,
           [Frozen] Mock<IReadGenericRepository<User>> userRepository,
           UserQueryHandler handler,
           IFixture fixture)
        {
            // Arrange
            User expectedUser =
                fixture.Build<User>().With(u => u.Id, searchByUserId).Create();
            userRepository.Setup(r => r.GetWithInclude(x => x.Department)).Returns(new[] { expectedUser }.AsQueryable());

            var query = new UserQuery() { Id = searchByUserId };

            // Act
            User user = handler.Handle(query);

            // Assert
            user.Should().NotBeNull();
            user.ShouldBeEquivalentTo(expectedUser);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistingUser_When_Handling_Then_NullReturned(
          Guid userId,
          UserQuery query,
          UserQueryHandler handler,
          [Frozen] Mock<IReadGenericRepository<User>> userRepository)
        {
            // Arrange
            userRepository.Setup(r => r.GetWithInclude(x => x.Department)).Returns(new User[] { }.AsQueryable());

            query.Id = userId;

            //Act
            User user = handler.Handle(query);
          
            //Assert
            user.Should().BeNull();
        }

    }
}
