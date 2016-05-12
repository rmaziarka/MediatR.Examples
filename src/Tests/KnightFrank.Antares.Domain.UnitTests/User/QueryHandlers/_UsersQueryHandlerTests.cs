using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace KnightFrank.Antares.Domain.UnitTests.User.QueryHandlers
{
    using FluentAssertions;
    using Moq;
    using Ploeh.AutoFixture.Xunit2;

    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.User.Queries;
    using KnightFrank.Antares.Domain.User.QueryHandlers;
    using KnightFrank.Antares.Domain.User.QueryResults;

    [Collection("UserQueryHandler")]
    [Trait("FeatureTitle", "Users")]
    public class UsersQueryHandlerTests
    {
      
        [Theory]
        [AutoMoqData]
        public void Given_ExistingUsersInQuery_When_Handling_Then_CorrectResultsReturned(
            [Frozen] Mock<IReadGenericRepository<User>> userRepository,
            Department mockDepartment,
            UsersQueryHandler handler,
            UsersQuery query
            )
        {
            // Arrange
            IList<User> userList = this.CreateUserList(mockDepartment);
            userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());
            query.PartialName = "jon";

            // Act
            IEnumerable<UsersQueryResult> resultUserList = handler.Handle(query).AsQueryable();
           
            //Assert
            resultUserList.Should().HaveCount(2);
            resultUserList.Should().BeInAscendingOrder(x => x.FirstName);
           
            Assert.All(resultUserList, 
                user =>Assert.True(user.FirstName.StartsWith(query.PartialName,StringComparison.CurrentCultureIgnoreCase)
                || user.LastName.StartsWith(query.PartialName,StringComparison.CurrentCultureIgnoreCase))
                );
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistsingUserInQuery_When_Handling_Then_ShouldReturnEmptyList(
            [Frozen] Mock<IReadGenericRepository<User>> userRepository,
             Department mockDepartment,
            UsersQueryHandler handler,
            UsersQuery query)
        {
            //Arrange
            IList<User> userList = this.CreateUserList(mockDepartment);
            userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());

            query.PartialName = "abc";

            //Act
            IEnumerable<UsersQueryResult> resultUserList = handler.Handle(query).AsQueryable();

            //Assert
            resultUserList.Should().BeEmpty();
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyStringInQuery_When_Handling_Then_ShouldReturnEmptyList(
            [Frozen] Mock<IReadGenericRepository<User>> userRepository,
            Department mockDepartment,
            UsersQueryHandler handler,
            UsersQuery query)
        {
            //Arrange
            IList<User> userList = this.CreateUserList(mockDepartment);
            userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());

            query.PartialName = string.Empty;

            //Act
            IEnumerable<UsersQueryResult> resultUserList = handler.Handle(query).AsQueryable();

            //Assert
            resultUserList.Should().BeEmpty();
        }

        [Theory]
        [AutoMoqData]
        public void Given_NullStringInQuery_When_Handling_Then_ShouldReturnEmptyList(
            [Frozen] Mock<IReadGenericRepository<User>> userRepository,
             Department mockDepartment,
            UsersQueryHandler handler,
            UsersQuery query)
        {
            //Arrange
            IList<User> userList = this.CreateUserList(mockDepartment);
            userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());

            query.PartialName = null;

            //Act
            IEnumerable<UsersQueryResult> resultUserList = handler.Handle(query).AsQueryable();

            //Assert
            resultUserList.Should().BeEmpty();
        }

        private IList<User> CreateUserList(Department userDepartment)
        {
            var userList = new List<User>
            {
                new User() {Id= new Guid("10000000-0000-0000-0000-000000000000"), FirstName = "Jon", LastName = "smoth", Department = userDepartment },
                new User() {Id= new Guid("20000000-0000-0000-0000-000000000000"), FirstName = "Andy", LastName = "jon", Department = userDepartment },
                new User() {Id= new Guid("30000000-0000-0000-0000-000000000000"), FirstName = "Andy", LastName = "San", Department = userDepartment }
            };
            return userList;
        }
    }
}
