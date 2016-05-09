namespace KnightFrank.Antares.Api.Controllers
{
    using System.Collections.Specialized;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Configuration;
    using System.Web.Http;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.User.QueryResults;

    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private static readonly NameValueCollection config = WebConfigurationManager.AppSettings;

        // TODO: here should inject repository (we should use handler)
        private readonly IReadGenericRepository<EnumTypeItem> enumTypeItemRepository;

        public UsersController(IReadGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.enumTypeItemRepository = enumTypeItemRepository;
        }

        [Route("data")]
        public UserDataResult GetUserData()
        {
            var identity = (ClaimsIdentity)this.User.Identity;

            EnumTypeItem division = this.GetEnumTypeItemByCode(config["CurrentUser.Division"]);
            var user = new UserDataResult
                           {
                               Name = identity.Name,
                               Email = identity.Claims.First(c => c.Type == ClaimTypes.Email).Value,
                               Country = identity.Claims.First(c => c.Type == ClaimTypes.Country).Value,
                               Roles = identity.FindAll(ClaimTypes.Role).Select(claim => claim.Value),
                               Division = division
                           };

            return user;
        }

        private EnumTypeItem GetEnumTypeItemByCode(string enumTypeItemCode)
        {
            return this.enumTypeItemRepository.Get().Single(eti => eti.Code == enumTypeItemCode);
        }
    }
}
