namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;
    using System.Collections.Generic;

    public class UpdateActivityAppraisalMeeting
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string InvitationText { get; set; }

        public IList<UpdateActivityAttendee> Attendees { get; set; } = new List<UpdateActivityAttendee>();
    }
}
