namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Configuration;
    using System.Web.Http;
    using System.Web.Security;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.User.QueryResults;

    using Domain.User.Queries;

    using KnightFrank.Antares.Domain.Common.Enums;

    using MediatR;

    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private static readonly NameValueCollection config = WebConfigurationManager.AppSettings;

        // TODO: here should inject repository (we should use handler)
        private readonly IReadGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly IMediator mediator;

        public UsersController(IReadGenericRepository<EnumTypeItem> enumTypeItemRepository, IMediator mediator)
        {
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.mediator = mediator;
        }

        [Route("data")]
        public UserDataResult GetUserData()
        {
            var identity = (ClaimsIdentity)this.User.Identity;

            EnumTypeItem division = this.GetEnumTypeItemByCode(config["CurrentUser.Division"]);
            var user = new UserDataResult
            {
                Id = Guid.Parse(identity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value),
                FirstName = identity.Claims.First(c => c.Type == "firstName").Value,
                LastName = identity.Claims.First(c => c.Type == "lastName").Value,
                Name = $"{identity.Claims.First(c => c.Type == "firstName").Value} {identity.Claims.First(c => c.Type == "lastName").Value}",
                Email = identity.Claims.First(c => c.Type == ClaimTypes.Email).Value,
                Country = identity.Claims.First(c => c.Type == ClaimTypes.Country).Value,
                Roles = identity.FindAll(ClaimTypes.Role).Select(claim => claim.Value),
                Division = division,
                Department = new UserDepartment(identity.Claims.First(c => c.Type == "departmentId").Value, identity.Claims.First(c => c.Type == "departmentName").Value)
            };

            return user;
        }

        private EnumTypeItem GetEnumTypeItemByCode(string enumTypeItemCode)
        {
            return this.enumTypeItemRepository.Get().Single(eti => eti.Code == enumTypeItemCode);
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
