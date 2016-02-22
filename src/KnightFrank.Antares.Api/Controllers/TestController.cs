using System.Web.Http;
using KnightFrank.Antares.Domain;

namespace KnightFrank.Antares.Api.Controllers
{
    public class TestController : ApiController
    {
        public IHttpActionResult GetContacts()
        {
            TestService svc = new TestService();
            var contacts = svc.GetContacts();
            return Ok(contacts);
        }
    }
}
