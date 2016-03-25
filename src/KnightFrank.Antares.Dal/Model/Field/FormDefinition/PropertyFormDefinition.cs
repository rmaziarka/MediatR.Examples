using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Dal.Model.Field.FormDefinition
{
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Resource;

    public class PropertyFormDefinition: BaseEntity
    {
        public Guid PropertyTypeDefinitionId { get; set; }

        public PropertyTypeDefinition PropertyTypeDefinition { get; set; }
    }
}
