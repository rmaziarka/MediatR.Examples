namespace KnightFrank.Antares.UITests.Extensions
{
    using System.Data.Entity;

    using KnightFrank.Antares.Dal;

    public static class DatabaseExtensions
    {
        private static KnightFrankContext dataContext;
        private static DbContextTransaction transaction;

        public static KnightFrankContext GetDataContext()
        {
            dataContext = new KnightFrankContext("UI.Settings.SqlConnectionString");
            transaction = dataContext.Database.BeginTransaction();

            return dataContext;
        }

        public static void CommitAndClose(this KnightFrankContext knightFrankContext)
        {
            dataContext.SaveChanges();
            transaction.Commit();
            transaction.Dispose();
            dataContext.Dispose();
        }
    }
}
