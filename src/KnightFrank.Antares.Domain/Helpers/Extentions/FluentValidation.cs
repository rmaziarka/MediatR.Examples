namespace KnightFrank.Antares.Domain.Helpers.Extentions
{
    using System.Linq;

    using global::FluentValidation.Results;

    public static class FluentValidation
    {
        public static ValidationResult Merge(this ValidationResult result, ValidationResult otherResult)
        {
            otherResult.Errors.ToList().ForEach(f => result.Errors.Add(f));

            return result;
        }
    }
}