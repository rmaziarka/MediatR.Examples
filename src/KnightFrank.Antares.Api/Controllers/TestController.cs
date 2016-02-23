namespace KnightFrank.Antares.Api.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    using KnightFrank.Antares.Domain;

    public class TestController : ApiController
    {
        public IHttpActionResult GetContacts()
        {
            var svc = new TestService();
            IEnumerable<Contact> contacts = svc.GetContacts();
            return this.Ok(contacts);
        }
    }
}
