
namespace Fields.Extensions
{
    using System.Collections.Generic;

    using Fields.Enums;

    public static class DictionaryExtensions
    {
        public static void AddControl(this IList<Control> list, ControlCode controlCode,InnerField field)
        {
            list.Add(new Control(controlCode, field));
        }

        public static void AddControl(this IList<Control> list, ControlCode controlCode, IList<InnerField> fields)
        {
            list.Add(new Control(controlCode, fields));
        }

    }
}
