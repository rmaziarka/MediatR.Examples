using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Dal.Model.Field.FormDefinition
{
    public class FormDefinition : BaseEntity
    {
        public int ColumnOrder { get; set; }

        public int RowOrder { get; set; }

        public bool Required { get; set; }

        public Guid AttributeId { get; set; }

        public Attribute Attribute { get; set; }
    }
}
