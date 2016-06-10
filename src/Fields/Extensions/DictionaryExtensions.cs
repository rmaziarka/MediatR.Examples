
namespace Fields.Extensions
{
    using System.Collections.Generic;

    using Fields.Enums;

    public static class DictionaryExtensions
    {
        public static void AddControl(this IList<Control> list, PageType pageType, ControlCode controlCode,InnerField field)
        {
            list.Add(new Control(pageType, controlCode, field));
        }

        public static void AddControl(this IList<Control> list, PageType pageType, ControlCode controlCode, IList<InnerField> fields)
        {
            list.Add(new Control(pageType, controlCode, fields));
        }
    }
}
