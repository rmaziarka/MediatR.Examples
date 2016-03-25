namespace KnightFrank.Antares.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Dal.Model;

    using Domain.Contact.Commands;

    using Dal.Repository;

    using KnightFrank.Antares.Domain.Contact.Queries;

    using MediatR;

    /// <summary>
    ///     Controller class for contacts
    /// </summary>
    [RoutePrefix("api/contacts")]
    public class ContactsController : ApiController
    {
        private readonly IMediator mediator;
        private readonly IReadGenericRepository<Contact> contactsRepository;

        /// <summary>
        ///     Contacts controller constructor
        /// </summary>
        /// <param name="mediator">Mediator instance.</param>
        /// <param name="contactsRepository">Generic read repository.</param>
        public ContactsController(IMediator mediator, IReadGenericRepository<Contact> contactsRepository)
        {
            this.mediator = mediator;
            this.contactsRepository = contactsRepository;
        }

        /// <summary>
        ///     Get contact list
        /// </summary>
        /// <returns>Contact entity collection</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<Contact> GetContacts()
        {
            return this.contactsRepository.Get().ToList();
        }

        /// <summary>
        ///     Get contact
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <returns>Contact entity</returns>
        [HttpGet]
        [Route("{id}")]
        public Contact GetContact(Guid id)
        {
            var query =  new ContactQuery() { Id = id};

            Contact contact = this.mediator.Send(query);

            if (contact == null)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Contact not found."));
            }

            return contact;
        }

        /// <summary>
        /// Create contact
        /// </summary>
        /// <param name="command">Contact entity</param>
        [HttpPost]
        [Route("")]
        public Guid CreateContact([FromBody] CreateContactCommand command)
        {
            return this.mediator.Send(command);
        }
    }
}
