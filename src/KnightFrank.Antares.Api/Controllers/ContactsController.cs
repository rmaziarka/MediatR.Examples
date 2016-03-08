namespace KnightFrank.Antares.API.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Http;

    using Dal.Model;

    using Domain.Contact.Commands;

    using Dal.Repository;

    using Domain.Contact;

    using MediatR;

    /// <summary>
    ///     Controller class for contacts
    /// </summary>
    public class ContactsController : ApiController
    {
        private readonly IMediator mediator;
        private readonly IReadGenericRepository<Contact> readRepository;

        /// <summary>
        ///     Contacts controller constructor
        /// </summary>
        /// <param name="mediator">Mediator instance.</param>
        /// <param name="readRepository">Generic read repository.</param>
        public ContactsController(IMediator mediator, IReadGenericRepository<Contact> readRepository)
        {
            this.mediator = mediator;
            this.readRepository = readRepository;
        }

        /// <summary>
        ///     Get contact list
        /// </summary>
        /// <returns>Contact entity collection</returns>
        [HttpGet]
        public IEnumerable<ContactDto> GetContacts()
        {
            IEnumerable<Contact> contacts = this.readRepository.GetAll();
            var contactsDto = AutoMapper.Mapper.Map<IEnumerable<ContactDto>>(contacts);
            return contactsDto;
        }

        /// <summary>
        ///     Get contact
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <returns>Contact entity</returns>
        [HttpGet]
        public Contact GetContact(int id)
        {
            Contact contact = this.readRepository.FindBy(c => c.Id == id).FirstOrDefault();

            if (contact == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Contact not found.");
            }

            return contact;
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
