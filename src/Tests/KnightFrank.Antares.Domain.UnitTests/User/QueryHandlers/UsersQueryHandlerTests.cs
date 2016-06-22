namespace KnightFrank.Antares.Domain.UnitTests.User.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;
    using FluentAssertions;
    using Moq;

    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.User.Queries;
    using KnightFrank.Antares.Domain.User.QueryHandlers;
    using KnightFrank.Antares.Domain.User.QueryResults;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    [Collection("UserQueryHandler")]
    [Trait("FeatureTitle", "Users")]
    public class UsersQueryHandlerTests
    {
        private readonly Mock<IReadGenericRepository<User>> userRepository;
        private readonly UsersQueryHandler handler;
        private readonly Department mockedDepartmentData;
        private readonly UsersQuery query;

        public UsersQueryHandlerTests()
        {
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.Clear();
            fixture.RepeatCount = 1;
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.userRepository = fixture.Freeze<Mock<IReadGenericRepository<User>>>();
            this.mockedDepartmentData = fixture.Create<Department>();
            this.query = fixture.Build<UsersQuery>().With(x => x.Take, 0).Create();
            this.handler = fixture.Create<UsersQueryHandler>();
        }

        [Theory]
        [InlineAutoData("ev", new[] { "50000000-0000-0000-0000-000000000000" })]
        [InlineAutoData("eve", new[] { "50000000-0000-0000-0000-000000000000" })]
        [InlineAutoData("jon", new []{ "10000000-0000-0000-0000-000000000000", "30000000-0000-0000-0000-000000000000" })]
        [InlineAutoData("jo", new[] { "10000000-0000-0000-0000-000000000000", "30000000-0000-0000-0000-000000000000", "40000000-0000-0000-0000-000000000000" })]
        [InlineAutoData("jon smath", new[] { "10000000-0000-0000-0000-000000000000" })]
        [InlineAutoData("jon sma", new[] { "10000000-0000-0000-0000-000000000000" })]
        [InlineAutoData("sma j", new[] { "10000000-0000-0000-0000-000000000000" })]
        [InlineAutoData("jo sm", new[] { "10000000-0000-0000-0000-000000000000", "40000000-0000-0000-0000-000000000000" })]
        [InlineAutoData("sm jo", new[] { "10000000-0000-0000-0000-000000000000", "40000000-0000-0000-0000-000000000000" })]
        public void Given_ExistingUsersInQuery_When_Handling_Then_CorrectAllResultsReturned(string partialName, string[] expectedIds)
        {
            // Arrange
            IList<User> userList = this.CreateUserList(this.mockedDepartmentData);
            this.userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());
            this.query.PartialName = partialName.ToLower();

            // Act
            IEnumerable<UsersQueryResult> resultUserList = this.handler.Handle(this.query).AsQueryable();

            //Assert
            resultUserList.Should().HaveCount(expectedIds.Length);
            resultUserList.Should().BeInAscendingOrder(x => x.FirstName);

            resultUserList.Select(u => u.Id).ShouldAllBeEquivalentTo(expectedIds.Select(i => new Guid(i)));
        }

        [Theory]
        [InlineAutoData("abc")]
        [InlineAutoData("jon abc")]
        [InlineAutoData("abc jon")]
        public void Given_NotExistsingUserInQuery_When_Handling_Then_ShouldReturnEmptyList(string partialName)
        {
            //Arrange
            IList<User> userList = this.CreateUserList(this.mockedDepartmentData);
            this.userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());

            this.query.PartialName = partialName;

            //Act
            IEnumerable<UsersQueryResult> resultUserList = this.handler.Handle(this.query).AsQueryable();

            //Assert
            resultUserList.Should().BeEmpty();
        }

        [Fact]
        public void Given_EmptyStringInQuery_When_Handling_Then_ShouldReturnEmptyList()
        {
            //Arrange
            IList<User> userList = this.CreateUserList(this.mockedDepartmentData);
            this.userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());

            this.query.PartialName = string.Empty;

            //Act
            IEnumerable<UsersQueryResult> resultUserList = this.handler.Handle(this.query).AsQueryable();

            //Assert
            resultUserList.Should().BeEmpty();
        }

        [Fact]
        public void Given_NullStringInQuery_When_Handling_Then_ShouldReturnEmptyList()
        {
            //Arrange
            IList<User> userList = this.CreateUserList(this.mockedDepartmentData);
            this.userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());

            this.query.PartialName = null;

            //Act
            IEnumerable<UsersQueryResult> resultUserList = this.handler.Handle(this.query).AsQueryable();

            //Assert
            resultUserList.Should().BeEmpty();
        }

        [Fact]
        public void Given_TakenParameterIs0_When_Handling_Then_ResultShouldHave3Items()
        {
            //Arrange
            IList<User> userList = this.CreateUserList(this.mockedDepartmentData);
            this.userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());

            this.query.PartialName = "j";
            this.query.Take = 0;


            //Act
            IEnumerable<UsersQueryResult> resultUserList = this.handler.Handle(this.query).AsQueryable();

            //Assert
            resultUserList.Should().HaveCount(3);
        }

        [Theory]
        [InlineAutoData(2)]
        [InlineAutoData(3)]
        public void Given_TakenParameter_When_Handling_Then_CorrectNumberOfResultsReturned(int takeValue)
        {
            //Arrange
            IList<User> userList = this.CreateUserList(this.mockedDepartmentData);
            this.userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());

            this.query.PartialName = "j";
            this.query.Take = takeValue;


            //Act
            IEnumerable<UsersQueryResult> resultUserList = this.handler.Handle(this.query).AsQueryable();

            //Assert
            resultUserList.Should().HaveCount(takeValue);
        }


        [Fact]
        public void Given_ExcludedParameter_When_Handling_Then_ExcludedIdIsNotReturned()
        {
            //Arrange
            IList<User> userList = this.CreateUserList(this.mockedDepartmentData);
            this.userRepository.Setup(x => x.Get()).Returns(userList.AsQueryable());
            this.query.PartialName = "j";

            IList<Guid> enumerable = userList.Select(x => x.Id).Take(2).ToList();

            this.query.ExcludedIds = enumerable.ToList();

            //Act
            IEnumerable<UsersQueryResult> resultUserList = this.handler.Handle(this.query).AsQueryable();

            //Assert
            resultUserList.Should().NotContain(enumerable.Any());
            Assert.True(resultUserList.Any());
        }


        private IList<User> CreateUserList(Department userDepartment)
        {
            var userList = new List<User>
            {
                new User()
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000000"),
                    FirstName = "jon",
                    LastName = "smath",
                    Department = userDepartment
                },
                 new User()
                {
                    Id = new Guid("20000000-0000-0000-0000-000000000000"),
                    FirstName = "andy",
                    LastName = "san",
                    Department = userDepartment
                },
                new User()
                {
                    Id = new Guid("30000000-0000-0000-0000-000000000000"),
                    FirstName = "andy",
                    LastName = "jon",
                    Department = userDepartment
                },
                new User()
                {
                    Id = new Guid("40000000-0000-0000-0000-000000000000"),
                    FirstName = "joe",
                    LastName = "smith",
                    Department = userDepartment
                },
                new User()
                {
                    Id = new Guid("50000000-0000-0000-0000-000000000000"),
                    FirstName = "eve",
                    LastName = "doe",
                    Department = userDepartment
                }

            };
            return userList;
        }
    }
}
