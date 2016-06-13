namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using KnightFrank.Antares.Domain.User.QueryResults;

    using Domain.User.Queries;

    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Domain.User.Commands;

    using MediatR;

    /// <summary>
    /// UsersController
    /// </summary>
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {

        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the UsersController class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public User GetUserById(Guid id)
        {
            User user = this.mediator.Send(new UserQuery { Id = id });

            if (user == null)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found."));
            }
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

        /// <summary>
        /// Updates the User.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public User UpdateOffer(UpdateUserCommand command)
        {
            Guid userId = this.mediator.Send(command);
            return this.GetUserById(userId);
        }
    }
}
