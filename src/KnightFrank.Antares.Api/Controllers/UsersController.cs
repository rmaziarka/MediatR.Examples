using System.Linq;

namespace KnightFrank.Antares.Api.Controllers
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Security.Claims;
    using System.Web.Configuration;
    using System.Web.Http;
    
    using Domain.Enum.Queries;
    using Domain.Enum.QueryResults;
    using Domain.User.Queries;
    using Domain.User.QueryResults;

    using MediatR;

    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IMediator mediator;
        private static readonly NameValueCollection config = WebConfigurationManager.AppSettings;

        /// <summary>
        ///  Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
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

            var user = new UserDataResult
            {
                Name = identity.Name,
                Email = identity.Claims.First(c => c.Type == ClaimTypes.Email).Value,
                Country = identity.Claims.First(c => c.Type == ClaimTypes.Country).Value,
                Roles = identity.FindAll(ClaimTypes.Role).Select(claim => claim.Value),
                Division = divisions.Items.Single(i => i.Code == config["CurrentUser.Division"])
            };

            return user;
        }

        /// <summary>
        /// Get all users matching criteria within query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<UsersQueryResult> GetUsers([FromUri(Name = "")] UsersQuery query)
        {
            if (query == null)
            {
                query = new UsersQuery();
            }

            return this.mediator.Send(query);
        }
    }
}