namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public static class ListExtensions
    {
        public static IDictionary<ControlCode, object> MapToResponse(this IList<InnerFieldState> fieldStates)
        {
            Dictionary<ControlCode, dynamic> result =
                fieldStates
                    .Where(x => !x.Hidden)
                    .GroupBy(x => x.ControlCode)
                    .ToDictionary(x => x.Key, x =>
                    {
                        dynamic r = x.ToDictionary(f => f.Name, f => (object)new { active = !f.Readonly });
                        r["Active"] = !x.All(f => f.Readonly);
                        return r;
                    });

            return result;
        }
    }
}
