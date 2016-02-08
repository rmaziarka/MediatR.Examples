using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Command;
using Common.Handler;
using Domain.Order.CommandResponse;
using FluentValidation;
using MediatR;

namespace Domain.Order.Command
{
    public class MakeOrderComplaint: ICommand<OrderComplaintInfo>
    {
        public int OrderId { get; set; }

        public string ComplaintMessage { get; set; }
    }

    public class MakeOrderComplaintValidator : AbstractValidator<MakeOrderComplaint>
    {
        public MakeOrderComplaintValidator()
        {
            RuleFor(el => el.ComplaintMessage).NotEmpty();
        }
    }
}
