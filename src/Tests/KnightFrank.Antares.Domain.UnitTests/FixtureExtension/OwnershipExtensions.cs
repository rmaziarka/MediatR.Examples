namespace KnightFrank.Antares.Domain.UnitTests.FixtureExtension
{
    using System;

    using KnightFrank.Antares.Dal.Model.Property;

    using Ploeh.AutoFixture;

    public static class OwnershipExtensions
    {
        public static Ownership BuildOwnership(this IFixture fixture, DateTime? purchaseDate = null, DateTime? sellDate = null)
        {
            return fixture.Build<Ownership>()
                   .With(x => x.PurchaseDate, purchaseDate)
                   .With(x => x.SellDate, sellDate)
                   .With(x => x.BuyPrice, 1000000)
                   .With(x => x.SellPrice, 2000000)
                   .Create();
        }
    }
}