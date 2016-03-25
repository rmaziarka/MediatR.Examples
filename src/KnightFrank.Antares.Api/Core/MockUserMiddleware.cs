namespace KnightFrank.Antares.Api.Core
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.Owin;

    public class MockUserMiddleware : OwinMiddleware
    {
        public MockUserMiddleware(OwinMiddleware next) : base(next) { }

        public override async Task Invoke(IOwinContext context)
        {
            var userIdentity = new ClaimsIdentity();
            userIdentity.AddClaim(new Claim(ClaimTypes.Name, "user"));
            userIdentity.AddClaim(new Claim(ClaimTypes.Email, "user@gmail.com"));
            userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "F2CA558D-E5CD-41ED-A093-C334C6395C2A"));
            userIdentity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            userIdentity.AddClaim(new Claim(ClaimTypes.Role, "superuser"));
            userIdentity.AddClaim(new Claim(ClaimTypes.Country, "GB"));

            var userPrincipal = new ClaimsPrincipal(userIdentity);

            context.Request.User = userPrincipal;

            await this.Next.Invoke(context);
        }
    }
}