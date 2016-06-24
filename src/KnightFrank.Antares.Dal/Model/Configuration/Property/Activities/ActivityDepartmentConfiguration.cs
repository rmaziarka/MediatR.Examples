namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Activities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    using KnightFrank.Antares.Dal.Model.Property.Activities;

    internal sealed class ActivityDepartmentConfiguration : BaseEntityConfiguration<ActivityDepartment>
    {
        public ActivityDepartmentConfiguration()
        {
            this.HasRequired(an => an.Activity)
                .WithMany(a => a.ActivityDepartments)
                .HasForeignKey(an => an.ActivityId)
                .WillCascadeOnDelete(false);    

            this.HasRequired(an => an.Department)
                .WithMany()
                .HasForeignKey(an => an.DepartmentId)
                .WillCascadeOnDelete(false);

            var uniqueIndexName = "IX_ActivityId_DepartmentId";
            this.Property(an => an.ActivityId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute(uniqueIndexName, 1) { IsUnique = true }));

            this.Property(an => an.DepartmentId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute(uniqueIndexName, 2) { IsUnique = true }));

            this.HasRequired(a => a.DepartmentType).WithMany().WillCascadeOnDelete(false);
        }
    }
}