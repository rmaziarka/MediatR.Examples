using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Dal.Model.Field
{
    using System.ComponentModel.DataAnnotations;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class Attribute: BaseEntity
    {
        public Guid AttributeTypId { get; set; }

        public EnumTypeItem AttributeType { get; set; }

        public IEnumerable<AttributeLocalized> LabelLocalizeds { get; set; } 
    }
}
