namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Dynamic;
    using System.Linq;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public static class ListExtensions
    {
        [SuppressMessage("ReSharper", "RedundantCast")]
        public static IDictionary<ControlCode, object> MapToResponse(this IList<InnerFieldState> fieldStates)
        {
            Dictionary<ControlCode, dynamic> configResult =
                fieldStates
                    .Where(control => !control.Hidden)
                    .GroupBy(control => control.ControlCode)
                    .ToDictionary(control => control.Key, control =>
                    {
                        dynamic controlConfigResult = control.ToDictionary(
                            fieldState => fieldState.Name,
                            fieldState =>
                            {
                                IDictionary<string, dynamic> fieldConfigResult = new ExpandoObject();
                                fieldConfigResult["Active"] = (object)!fieldState.Readonly;
                                fieldConfigResult["Required"] = (object)fieldState.Required;
                                if (fieldState.AllowedCodes != null)
                                {
                                    fieldConfigResult["AllowedCodes"] = fieldState.AllowedCodes;
                                }
                                return fieldConfigResult;
                            });

                        return controlConfigResult;
                    });

            return configResult;
        }
    }
}
