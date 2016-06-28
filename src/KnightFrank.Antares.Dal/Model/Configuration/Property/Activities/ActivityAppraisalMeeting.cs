using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Activities
{
    using System.ComponentModel.DataAnnotations.Schema;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    
    public class ActivityAppraisalMeeting
    {
        public DateTimeOffset? AppraisalMeetingStart { get; set; }

        public DateTimeOffset? AppraisalMeetingEnd { get; set; }
        
        public string InvitationText { get; set; }
    }
}
