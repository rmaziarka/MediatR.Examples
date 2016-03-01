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
    public class ContactsController : ApiController
    {
        private IMediator mediator;
        
        public ContactsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        /// <summary>
        ///     Get contact list
        /// </summary>
        /// <returns>Contact entity collection</returns>
        [HttpGet]
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
        public ContactDto GetContact(int id)
        {
            return new ContactDto { FirstName = "John", Surname = "Doe" };
        }

        /// <summary>
        /// Create contact
        /// </summary>
        /// <param name="command">Contact entity</param>
        [HttpPost]
        public int CreateContact([FromBody] CreateContactCommand command)
        {
            return this.mediator.Send(command);
        }

        /// <summary>
        ///     Update contact
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <param name="command">Contact entity</param>
        [HttpPut]
        public void UpdateContact(int id, [FromBody] UpdateContactCommand command)
        {
            this.mediator.Send(command);
        }

        /// <summary>
        ///     Delete contact
        /// </summary>
        /// <param name="id">Contact id</param>
        [HttpDelete]
        public void DeleteContact(int id)
        {
        }
    }
}
