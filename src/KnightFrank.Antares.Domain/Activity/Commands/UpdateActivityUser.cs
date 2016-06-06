namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    public class UpdateActivityUser
    {
        public Guid UserId { get; set; }

        public DateTime? CallDate { get; set; }
    }
}
