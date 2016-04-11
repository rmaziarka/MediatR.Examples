namespace KnightFrank.Antares.Dal.Seed
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;

    internal class PropertyAttributeFromData
    {
        public static void Seed(KnightFrankContext context)
        {
            var forms = CreatePropertyAttributeForms(context);

            context.PropertyAttributeForm.AddOrUpdate(p => new { p.PropertyTypeId, p.CountryId }, forms.ToArray());

            context.SaveChanges();
        }

        private static IEnumerable<PropertyAttributeForm> CreatePropertyAttributeForms(KnightFrankContext context)
        {
            var propertyTypes = context.PropertyType.ToList();

            var attributes = context.Attribute.ToList();

            foreach (var country in propertyAttributeFormData)
            {
                foreach (var data in country.Value)
                {
                    yield return new PropertyAttributeForm
                    {
                        CountryId = context.Country.Single(c => c.IsoCode == country.Key).Id,
                        PropertyTypeId = propertyTypes.Single(p => p.Code == data.Key).Id,
                        PropertyAttributeFormDefinitions = CreatePropertyAttributeFormDefinitions(attributes, data.Value).ToList()
                    };
                }
            };
        }

        private static IEnumerable<PropertyAttributeFormDefinition> CreatePropertyAttributeFormDefinitions(List<Model.Attribute.Attribute> attributes, List<string> value)
        {
            var i = 0;
            foreach (var attribute in attributes)
            {
                if (value.Contains(attribute.NameKey))
                {
                    yield return new PropertyAttributeFormDefinition
                    {
                        AttributeId = attribute.Id,
                        Order = i
                    };

                    i++;
                }
            }
        }

        private static readonly Dictionary<string, Dictionary<string, List<string>>> propertyAttributeFormData = new Dictionary
            <string, Dictionary<string, List<string>>>
        {
            ["GB"] = new Dictionary<string, List<string>>
            {
                ["House"] = new List<string> { "Bedrooms", "Receptions", "Bathrooms", "Area", "LandArea", "CarParkingSpaces" },
                ["Flat"] = new List<string> { "Bedrooms", "Receptions", "Bathrooms", "Area", "LandArea", "CarParkingSpaces" },
                ["Bungalow"] = new List<string> { "Bedrooms", "Receptions", "Bathrooms", "Area", "LandArea", "CarParkingSpaces" },
                ["Houseboat"] = new List<string> { "Bedrooms", "Receptions", "Bathrooms", "Area", "CarParkingSpaces" },
                ["Maisonette"] = new List<string> { "Bedrooms", "Receptions", "Bathrooms", "Area", "CarParkingSpaces" },
                ["Development Plot"] = new List<string> { "LandArea" },
                ["Hotel"] = new List<string> { "Area", "LandArea", "GuestRooms", "FunctionRooms", "CarParkingSpaces" }
            }
        };
    }
}
