using System.Linq;

namespace KnightFrank.Antares.Api.Controllers
{
    using System.Collections.Specialized;
    using System.Security.Claims;
    using System.Web.Configuration;
    using System.Web.Http;

    using KnightFrank.Antares.Domain.Enum.Queries;
    using KnightFrank.Antares.Domain.Enum.QueryResults;
    using KnightFrank.Antares.Domain.User.QueryResults;

    using MediatR;

    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IMediator mediator;
        private static readonly NameValueCollection config = WebConfigurationManager.AppSettings;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Route("data")]
        public UserDataResult GetUserData()
        {
            var identity = (ClaimsIdentity)this.User.Identity;

            var divisionsQuery = new EnumQuery { Code = "Division" };
            EnumQueryResult divisions = this.mediator.Send(divisionsQuery);

            var user = new UserDataResult()
            {
                Name = identity.Name,
                Email = identity.Claims.First(c => c.Type == ClaimTypes.Email).Value,
                Country = identity.Claims.First(c => c.Type == ClaimTypes.Country).Value,
                Roles = identity.FindAll(ClaimTypes.Role).Select(claim => claim.Value),
                Division = divisions.Items.Single(i => i.Code == config["CurrentUser.Division"])
            };

            return user;
        }
    }
}