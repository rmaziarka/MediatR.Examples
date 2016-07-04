namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using KnightFrank.Antares.Domain.User.QueryResults;

    using Domain.User.Queries;

    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.User.Commands;

    using MediatR;

    /// <summary>
    /// UsersController
    /// </summary>
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {

        private readonly IMediator mediator;
        private readonly IReadGenericRepository<User> userRepository;

        /// <summary>
        /// Initializes a new instance of the UsersController class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="userRepository">User repository</param>
        public UsersController(IMediator mediator, IReadGenericRepository<User> userRepository)
        {
            this.mediator = mediator;
            this.userRepository = userRepository;
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
        public User UpdateUser(UpdateUserCommand command)
        {
            Guid userId = this.mediator.Send(command);
            return this.GetUserById(userId);
        }

        /// <summary>
        /// Get First user in database
        /// </summary>
        /// <returns></returns>
        [Route("current")]
        public User GetUserData()
        {
            //ToDo: waiting for authentication to be completed
            //hardcoded for now as first user in database 
            User user = this.userRepository.Get()
                 .Include(u => u.Locale)
                 .Include(u => u.Department)
                 .Include(u => u.Country)
                 .Include(u => u.Roles)
                 .Include(u => u.Division)
                 .First();
            return user;
        }
  }
}
