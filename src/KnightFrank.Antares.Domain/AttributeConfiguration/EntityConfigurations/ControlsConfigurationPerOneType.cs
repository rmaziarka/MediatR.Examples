namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    public abstract class ControlsConfigurationPerOneType<TEnum1> : BaseControlsConfiguration<Tuple<TEnum1>> where TEnum1 : struct
    {
        protected IList<Tuple<Tuple<TEnum1>, PageType>> When(TEnum1 enum1, params PageType[] pages)
        {
            return pages.Select(
                page => new Tuple<Tuple<TEnum1>, PageType>(new Tuple<TEnum1>(enum1), page)).ToList();
        }

        protected IList<Tuple<Tuple<TEnum1>, PageType>> When(IList<TEnum1> enums1, params PageType[] pages)
        {
            return
                pages.SelectMany(
                    page =>
                        enums1.Select(e => new Tuple<Tuple<TEnum1>, PageType>(new Tuple<TEnum1>(e), page)))
                     .ToList();
        }

        /// <summary>
        /// Produces a set of all possible combinations of enum.
        /// </summary>
        /// <param name="pages">The pages.</param>
        /// <returns></returns>
        protected IList<Tuple<Tuple<TEnum1>, PageType>> ForAll(params PageType[] pages)
        {
            List<TEnum1> enums1 = EnumExtensions.GetValues<TEnum1>().ToList();
            return
                pages.SelectMany(
                    page =>
                        enums1.Select(
                            enum1 => new Tuple<Tuple<TEnum1>, PageType>(new Tuple<TEnum1>(enum1), page)))
                     .ToList();
        }
    }
}