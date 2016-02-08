using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Account.Command;
using Domain.Account.CommandResponse;
using Domain.Order.Command;
using Domain.Order.CommandResponse;
using MediatR;

namespace WebApi.Controllers
{
    // mediator will validate command if complaint message is not empty
    // if is, attribute will handle validation error and return error messages
    [RoutePrefix("api/useraccount")]
    public class UserAccountController : ApiController
    {
        private IMediator _mediator;

        public UserAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("ordercomplaint")]
        [HandleValidationException]
        public OrderComplaintInfo MakeOrderComplaint(MakeOrderComplaint command)
        {
            return _mediator.Send(command);
        }

        [HttpPost]
        [Route("changeuseraddressdata")]
        [HandleValidationException]
        public UserAddressData ChangeUserAddressData(ChangeUserAddressData command)
        {
            return _mediator.Send(command);
        }
    }
}
