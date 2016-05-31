namespace KnightFrank.Antares.Dal.Model.LatestView
{
    using System;

    using KnightFrank.Antares.Dal.Model.Common;

    public class LatestView : BaseEntity
    {
        public DateTime CreatedDate { get; set; }

        public Guid UserId { get; set; }

        public virtual User.User User { get; set; }

        public Guid EntityId { get; set; }

        public EntityTypeEnum EntityType { get; set; }

        public string EntityTypeString
        {
            get
            {
                return this.EntityType.ToString();
            }

            private set
            {
                this.EntityType = (EntityTypeEnum)Enum.Parse(typeof(EntityTypeEnum), value, true);
            }
        }
    }
}
