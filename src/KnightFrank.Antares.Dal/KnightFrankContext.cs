using System.Data.Entity;

namespace KnightFrank.Antares.Dal
{
    public class KnightFrankContext : DbContext
    {
        public KnightFrankContext()
            : base("KnightFrankConnection")
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
