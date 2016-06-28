using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Activities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    using KnightFrank.Antares.Dal.Model.Property.Activities;

    internal sealed class ActivityAttendeeConfiguration : BaseEntityConfiguration<ActivityAttendee>
    {
        public ActivityAttendeeConfiguration()
        {
            this.HasRequired(an => an.Activity).WithMany(a => a.ActivityAttendees).HasForeignKey(an => an.ActivityId).WillCascadeOnDelete(false);
            this.HasOptional(an => an.Contact).WithMany().HasForeignKey(an => an.ContactId).WillCascadeOnDelete(false);
            this.HasOptional(an => an.User).WithMany().HasForeignKey(an => an.UserId).WillCascadeOnDelete(false);


            var uniqueIndexName = "IX_ActivityId_UserId_ContactId";
            this.Property(an => an.ActivityId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 1)
                    {
                        IsUnique = true
                    }));

            this.Property(an => an.ContactId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 2)
                    {
                        IsUnique = true
                    }));

            this.Property(an => an.UserId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 3)
                    {
                        IsUnique = true
                    }));
        }
    }
}
