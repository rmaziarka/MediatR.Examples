using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Dal.Model.Field
{
    using KnightFrank.Antares.Dal.Model.Resource;

    public class AttributeLocalized: BaseEntity
    {
        public Guid AttributeId { get; set; }

        public Attribute Attribute { get; set; }

        public Guid LocaleId { get; set; }

        public Locale Locale { get; set; }
        
        public string Value { get; set; }
    }
}
