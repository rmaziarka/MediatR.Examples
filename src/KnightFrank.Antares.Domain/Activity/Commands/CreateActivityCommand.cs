namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    public class CreateActivityCommand : ActivityCommandBase
    {
        public Guid PropertyId { get; set; }
    }
}