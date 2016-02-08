using Common.Handler;
using Domain.Account.Command;
using Domain.Account.CommandResponse;

namespace Domain.Account.CommandHandlers
{
    public class MakeOrderComplaintHandler: ValidatorHandler<ChangeUserAddressData, UserAddressData>
    {
        public override UserAddressData HandleRequest(ChangeUserAddressData request)
        {
            // find user
            // change address data

            return new UserAddressData();
        }
    }
}
