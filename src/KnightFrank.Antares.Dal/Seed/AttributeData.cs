namespace KnightFrank.Antares.Dal.Seed
{
    using System.Data.Entity.Migrations;

    using KnightFrank.Antares.Dal.Model.Attribute;

    internal static class AttributeData
    {
        public static void Seed(KnightFrankContext context)
        {
            var attributes = new[]
            {
                new Attribute { LabelKey = "PROPERTY.BEDROOMS", NameKey = "Bedrooms" },
                new Attribute { LabelKey = "PROPERTY.RECEPTIONS", NameKey = "Receptions" },
                new Attribute { LabelKey = "PROPERTY.BATHROOMS", NameKey = "Bathrooms" },
                new Attribute { LabelKey = "PROPERTY.AREA", NameKey = "Area" },
                new Attribute { LabelKey = "PROPERTY.LANDAREA", NameKey = "LandArea" },
                new Attribute { LabelKey = "PROPERTY.GUESTROOMS", NameKey = "GuestRooms" },
                new Attribute { LabelKey = "PROPERTY.FUNCTIONROOMS", NameKey = "FunctionRooms" },
                new Attribute { LabelKey = "PROPERTY.CARPARKINGSPACES", NameKey = "CarParkingSpaces" }
            };

            context.Attributes.AddOrUpdate(a => a.NameKey, attributes);
            context.SaveChanges();
        }
    }
}
