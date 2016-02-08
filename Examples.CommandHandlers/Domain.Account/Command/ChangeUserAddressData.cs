using Common.Command;
using Domain.Account.CommandResponse;
using FluentValidation;

namespace Domain.Account.Command
{
    public class ChangeUserAddressData: ICommand<UserAddressData>
    {
        public string City { get; set; }

        public string Street { get; set; }
    }

    public class MakeOrderComplaintValidator : AbstractValidator<ChangeUserAddressData>
    {
        public MakeOrderComplaintValidator()
        {
            RuleFor(el => el.City).NotEmpty();
            RuleFor(el => el.Street).NotEmpty();
        }
    }
}
