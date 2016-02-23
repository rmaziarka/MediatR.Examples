namespace KnightFrank.Antares.API.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    using KnightFrank.Antares.API.Models;

    /// <summary>
    ///     Controller class for contacts
    /// </summary>
    [RoutePrefix("api/contacts")]
    public class ContactController : ApiController
    {
        /// <summary>
        ///     Get contact list
        /// </summary>
        /// <returns>Contact entity collection</returns>
        [HttpGet]
        public IEnumerable<ContactDto> GetContacts()
        {
            return new[] { new ContactDto { FirstName = "value", Surname = "value" } };
        }

        /// <summary>
        ///     Get contact
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <returns>Contact entity</returns>
        [HttpGet]
        [Route("{id}")]
        public ContactDto GetContact(int id)
        {
            return new ContactDto { FirstName = "value", Surname = "value" };
        }

        /// <summary>
        ///     Create contact
        /// </summary>
        /// <param name="contact">Contact entity</param>
        [HttpPost]
        public void CreateContact([FromBody] ContactDto contact)
        {
        }

        /// <summary>
        ///     Update contact
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <param name="contact">Contact entity</param>
        [HttpPut]
        [Route("{id}")]
        public void UpdateContact(int id, [FromBody] ContactDto contact)
        {
        }

        /// <summary>
        ///     Delete contact
        /// </summary>
        /// <param name="id">Contact id</param>
        [HttpDelete]
        [Route("{id}")]
        public void DeleteContact(int id)
        {
        }
    }
}
