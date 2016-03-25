using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Dal.Model.Field
{
    using System.ComponentModel.DataAnnotations;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class TextAttribute: Attribute
    {
        public Field Text { get; set; }
    }
}
