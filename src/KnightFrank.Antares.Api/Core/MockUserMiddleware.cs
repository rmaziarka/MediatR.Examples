namespace KnightFrank.Antares.Api.Core
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;

    using Microsoft.Owin;

    using Ninject;

    public class MockUserMiddleware : OwinMiddleware
    {
        private readonly IKernel kernel;

        public MockUserMiddleware(OwinMiddleware next, IKernel kernel) : base(next)
        {
            this.kernel = kernel;
        }

        public override async Task Invoke(IOwinContext context)
        {
            // Temporary solution to get user
            User user = this.kernel.Get<IGenericRepository<User>>().FindBy(x => true).First();

            var userIdentity = new ClaimsIdentity();
            userIdentity.AddClaim(new Claim("firstName", user.FirstName));
            userIdentity.AddClaim(new Claim("lastName", user.LastName));
            userIdentity.AddClaim(new Claim(ClaimTypes.Email, "user@gmail.com"));
            userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            userIdentity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            userIdentity.AddClaim(new Claim(ClaimTypes.Role, "superuser"));
            userIdentity.AddClaim(new Claim(ClaimTypes.Country, "GB"));
            userIdentity.AddClaim(new Claim("departmentName", user.Department.Name));
            userIdentity.AddClaim(new Claim("departmentId", user.DepartmentId.ToString()));

            var userPrincipal = new ClaimsPrincipal(userIdentity);

            context.Request.User = userPrincipal;

            await this.Next.Invoke(context);
        }
    }
}