namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    public class UpdateActivityCommand : ActivityCommandBase
    {
        public Guid Id { get; set; }

        public Guid? SalesBoardTypeId { get; set; }

        public Guid? SalesBoardStatusId { get; set; }
    }
}
