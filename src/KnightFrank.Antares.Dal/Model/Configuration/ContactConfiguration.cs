namespace KnightFrank.Antares.Dal.Migrations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using KnightFrank.Antares.Dal.Model;


    internal class ContactConfiguration : BaseEntityConfiguration<Contact>
    {
        public ContactConfiguration()
        {
        }
    }
}