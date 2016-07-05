namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    public class UpdateActivityAttendee
    {
        public Guid? UserId { get; set; }

        public Guid? ContactId { get; set; }
    }
}
