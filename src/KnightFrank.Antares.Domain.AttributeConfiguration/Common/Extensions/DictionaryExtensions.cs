
namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public static class DictionaryExtensions
    {
        public static void AddControl(this IList<Control> list, ControlType controlType, PageType pageType, ControlCode controlCode,InnerField field)
        {
            list.Add(new Control(controlType, pageType, controlCode, field));
        }

        public static void AddControl(this IList<Control> list, ControlType controlType, PageType pageType, ControlCode controlCode, IList<InnerField> fields)
        {
            list.Add(new Control(controlType, pageType, controlCode, fields));
        }
    }
}
