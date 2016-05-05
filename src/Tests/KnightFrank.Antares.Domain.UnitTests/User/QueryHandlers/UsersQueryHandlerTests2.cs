using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace KnightFrank.Antares.Domain.UnitTests.User.QueryHandlers
{
    using FluentAssertions;
    using Moq;

    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.User.Queries;
    using KnightFrank.Antares.Domain.User.QueryHandlers;
    using KnightFrank.Antares.Domain.User.QueryResults;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    [Collection("UserQueryHandler2")]
    [Trait("FeatureTitle", "Users")]
    public class UsersQueryHandlerTests2
    {
        private readonly Mock<IReadGenericRepository<User>> userRepository;
        private readonly UsersQueryHandler handler;
      private readonly ICollection<Department> mockedDepartmentData;
        private readonly UsersQuery query;

        public UsersQueryHandlerTests2()
        {

            //???:Isn't it better to have a shared fixture that is rune once per class rather than one per test?
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.Clear();
            fixture.RepeatCount = 1;
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.userRepository = fixture.Freeze<Mock<IReadGenericRepository<User>>>();
            this.mockedDepartmentData = fixture.CreateMany<Department>().ToList();
            this.query = fixture.Create<UsersQuery>();
            this.handler = fixture.Create<UsersQueryHandler>();
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExistingUsersInQuery_When_Handling_Then_CorrectResultsReturned()
        {
            // Arrange
            IList<User> userList = this.CreateUserList(this.mockedDepartmentData.FirstOrDefault());
            this.userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());
            this.query.PartialName = "jon";

            // Act
            IEnumerable<UsersQueryResult> resultUserList = this.handler.Handle(this.query).AsQueryable();

            //Assert
            resultUserList.Should().HaveCount(2);
            resultUserList.Should().BeInAscendingOrder(x => x.FirstName);

            //first name OR last name matches query.
            Assert.All(resultUserList,
                user => Assert.True(user.FirstName.StartsWith(this.query.PartialName, StringComparison.CurrentCultureIgnoreCase)
                                    || user.LastName.StartsWith(this.query.PartialName, StringComparison.CurrentCultureIgnoreCase))
                );
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistsingUserInQuery_When_Handling_Then_ShouldReturnEmptyList()
        {
            //Arrange
            IList<User> userList = this.CreateUserList(this.mockedDepartmentData.FirstOrDefault());
            this.userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());

            this.query.PartialName = "abc";

            //Act
            IEnumerable<UsersQueryResult> resultUserList = this.handler.Handle(this.query).AsQueryable();

            //Assert
            resultUserList.Should().BeEmpty();
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyStringInQuery_When_Handling_Then_ShouldReturnEmptyList()
        {
            //Arrange
            IList<User> userList = this.CreateUserList(this.mockedDepartmentData.FirstOrDefault());
            this.userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());

            this.query.PartialName = string.Empty;

            //Act
            IEnumerable<UsersQueryResult> resultUserList = this.handler.Handle(this.query).AsQueryable();

            //Assert
            resultUserList.Should().BeEmpty();
        }

        [Theory]
        [AutoMoqData]
        public void Given_NullStringInQuery_When_Handling_Then_ShouldReturnEmptyList()
        {
            //Arrange
            IList<User> userList = this.CreateUserList(this.mockedDepartmentData.FirstOrDefault());
            this.userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());

            this.query.PartialName = null;

            //Act
            IEnumerable<UsersQueryResult> resultUserList = this.handler.Handle(this.query).AsQueryable();

            //Assert
            resultUserList.Should().BeEmpty();
        }

        private IList<User> CreateUserList(Department userDepartment)
        {
            var userList = new List<User>
            {
                new User() { FirstName = "Jon", LastName = "smoth", Department = userDepartment },
                new User() { FirstName = "Andy", LastName = "jon", Department = userDepartment },
                new User() { FirstName = "Andy", LastName = "San", Department = userDepartment }
            };
            return userList;
        }
    }
}
