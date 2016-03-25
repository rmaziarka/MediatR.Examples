using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Dal.Model.Field
{
    using System.Data.Entity.Core.Metadata.Edm;

    public class ComboboxField: Field
    {
        public Guid EnumTypeId { get; set; }
        public EnumType EnumType { get; set; }
    }
}
