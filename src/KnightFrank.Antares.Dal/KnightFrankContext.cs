namespace KnightFrank.Antares.Dal
{
    using System.Data.Entity;

    public class KnightFrankContext : DbContext
    {
        public KnightFrankContext()
            : base("KnightFrankConnection")
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
