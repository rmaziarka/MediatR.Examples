using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Dal.Model.Field
{
    using KnightFrank.Antares.Dal.Model.Enum;

    public class Field:BaseEntity
    {
        public string FieldName { get; set; }

        public IEnumerable<FieldLocalized> PlaceholderLocalizeds { get; set; }

        public int EntityTypeId { get; set; }
        public EnumTypeItem EntityType { get; set; }

        public int StorageTypeId { get; set; }
        public EnumTypeItem StorageType { get; set; }

        public int UiFieldTypeId { get; set; }
        public EnumTypeItem UiFieldType { get; set; }

        public int DataTypeId { get; set; }
        public EnumTypeItem DataType { get; set; }
    }
}
