using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace KnightFrank.Antares.Domain.UnitTests.User.QueryHandlers
{
    using System.Collections;

    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Domain.UnitTests.User.Queries;
    using KnightFrank.Antares.Domain.User.Queries;
    using KnightFrank.Antares.Domain.User.QueryHandlers;
    using KnightFrank.Antares.Domain.User.QueryResults;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    public class UsersQueryHandlerTests
    {
        private IList<User> userList; 
        //public UsersQueryHandlerTests()
        //{
        //    IFixture fixture= new Fixture();
        //    User userFirst = 
        //}

        //[Theory]
        //[AutoData]
        //public void Given_UsersQueryHandler_When_Handling_Then_CorrectResultsReturned(
        //    UsersQueryHandler handler,
        //    UsersQuery query,
        //    )
        //{
        //    // Arrange
        //    User user = 
        //    query.PartialName = "j";

        //    // Act
        //    IEnumerable<UsersQueryResult> users = handler.Handle(query);


        //}
    }
}
