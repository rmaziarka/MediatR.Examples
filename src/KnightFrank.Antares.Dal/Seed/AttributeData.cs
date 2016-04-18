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
                new Attribute { LabelKey = "Property.Bedrooms", NameKey = "Bedrooms" },
                new Attribute { LabelKey = "Property.Receptions", NameKey = "Receptions" },
                new Attribute { LabelKey = "Property.Bathrooms", NameKey = "Bathrooms" },
                new Attribute { LabelKey = "Property.Area", NameKey = "Area" },
                new Attribute { LabelKey = "Property.LandArea", NameKey = "LandArea" },
                new Attribute { LabelKey = "Property.GuestRooms", NameKey = "GuestRooms" },
                new Attribute { LabelKey = "Property.FunctionRooms", NameKey = "FunctionRooms" },
                new Attribute { LabelKey = "Property.CarParkingSpaces", NameKey = "CarParkingSpaces" }
            };

            context.Attributes.AddOrUpdate(a => a.NameKey, attributes);
            context.SaveChanges();
        }
    }
}
