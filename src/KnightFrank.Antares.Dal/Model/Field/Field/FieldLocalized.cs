using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Dal.Model.Field
{
    using KnightFrank.Antares.Dal.Model.Resource;

    public class FieldLocalized: BaseEntity
    {
        public Guid FieldId { get; set; }

        public Field Field { get; set; }

        public Guid LocaleId { get; set; }

        public Locale Locale { get; set; }
        
        public string Value { get; set; }
    }
}
