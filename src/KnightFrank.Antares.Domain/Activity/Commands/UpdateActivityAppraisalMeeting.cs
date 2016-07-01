namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;
    using System.Collections.Generic;

    public class UpdateActivityAppraisalMeeting
    {
        public DateTimeOffset Start { get; set; }

        public DateTimeOffset End { get; set; }

        public string InvitationText { get; set; }

        public IList<UpdateActivityAttendee> Attendees { get; set; } = new List<UpdateActivityAttendee>();
    }
}
