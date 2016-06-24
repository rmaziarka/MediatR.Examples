namespace KnightFrank.Antares.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using MediatR;
    using Domain.Requirement.Commands;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Attachment.Queries;
    using KnightFrank.Antares.Domain.Requirement.Queries;
    using KnightFrank.Antares.Domain.RequirementNote.Commands;
    using KnightFrank.Antares.Domain.RequirementNote.Queries;

    /// <summary>
    /// Requirement controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/requirements")]
    public class RequirementsController : ApiController
    {
        private readonly IMediator mediator;
        private readonly IGenericRepository<User> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequirementsController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public RequirementsController(IMediator mediator, IGenericRepository<User> userRepository)
        {
            this.mediator = mediator;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Create requirement
        /// </summary>
        /// <returns>Requirement identifier.</returns>
        [HttpPost]
        [Route("")]
        public Requirement CreateRequirement(CreateRequirementCommand command)
        {
            Guid requirementId =  this.mediator.Send(command);

            return this.GetRequirementById(requirementId);
        }

        /// <summary>
        /// Gets the requirement by Id.
        /// </summary>
        /// <param name="id">Requirement Id.</param>
        /// <returns>Requirement.</returns>
        [HttpGet]
        [Route("{id}")]
        public Requirement GetRequirementById(Guid id)
        {
            Requirement requirement = this.mediator.Send(new RequirementQuery { Id = id });

            if (requirement == null)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Requirement not found."));
            }

            return requirement;
        }

        /// <summary>
        ///     Creates the requirement note.
        /// </summary>
        /// <param name="id">Requirement Id</param>
        /// <param name="command">Requirement note data</param>
        /// <returns>Requirement note</returns>
        [HttpPost]
        [Route("{id}/notes")]
        public RequirementNote CreateRequirementNote(Guid id, CreateRequirementNoteCommand command)
        {
            command.RequirementId = id;
            Guid requirementNoteId = this.mediator.Send(command);

            RequirementNote requirementNote =
                this.mediator.Send(new RequirementNoteQuery { Id = requirementNoteId });

            return requirementNote;
        }

        /// <summary>
        ///     Creates the attachment.
        /// </summary>
        /// <param name="id">Activity Id</param>
        /// <param name="command">Attachment data</param>
        /// <returns>Created attachment</returns>
        [HttpPost]
        [Route("{id}/attachments")]
        public Attachment CreateAttachment(Guid id, CreateRequirementAttachmentCommand command)
        {
            // User id is mocked.
            // TODO Set correct user id from header.
            if (command.Attachment != null)
            {
                command.Attachment.UserId = this.userRepository.FindBy(u => true).First().Id;
            }

            command.EntityId = id;
            Guid attachmentId = this.mediator.Send(command);

            var attachmentQuery = new AttachmentQuery { Id = attachmentId };
            return this.mediator.Send(attachmentQuery);
        }
    }
}
