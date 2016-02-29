namespace KnightFrank.Antares.API.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    using KnightFrank.Antares.API.Models;
    using KnightFrank.Antares.Domain.Contact.Commands;

    using MediatR;

    /// <summary>
    ///     Controller class for contacts
    /// </summary>
    [RoutePrefix("api/contacts")]
    public class ContactController : ApiController
    {
        private IMediator mediator;

        public ContactController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        /// <summary>
        ///     Get contact list
        /// </summary>
        /// <returns>Contact entity collection</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<ContactDto> GetContacts()
        {
            return new[]
            { new ContactDto { FirstName = "John", Surname = "Doe" }, new ContactDto { FirstName = "David", Surname = "Dummy" } };
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
            return new ContactDto { FirstName = "John", Surname = "Doe" };
        }

        /// <summary>
        ///     Create contact
        /// </summary>
        /// <param name="contact">Contact entity</param>
        [HttpPost]
        [Route("")]
        public int CreateContact([FromBody] CreateContactCommand command)
        {
            return this.mediator.Send(command);
        }

        /// <summary>
        ///     Update contact
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <param name="contact">Contact entity</param>
        [HttpPut]
        [Route("{id}")]
        public void UpdateContact(int id, [FromBody] UpdateContactCommand command)
        {
            this.mediator.Send(command);
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
