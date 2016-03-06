namespace KnightFrank.Antares.Dal
{
    using System.Data.Entity;

    using KnightFrank.Antares.Dal.Model;

    public class KnightFrankContext : DbContext
    {
        public KnightFrankContext() : base("KnightFrankConnection")
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
