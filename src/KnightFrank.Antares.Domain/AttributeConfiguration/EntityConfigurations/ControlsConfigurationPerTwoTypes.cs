namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    public abstract class ControlsConfigurationPerTwoTypes<TEnum1, TEnum2> : BaseControlsConfiguration<Tuple<TEnum1, TEnum2>> where TEnum1 : struct where TEnum2 : struct
    {
        protected IList<Tuple<Tuple<TEnum1, TEnum2>, PageType>> When(TEnum1 enum1, TEnum2 enum2, params PageType[] pages)
        {
            return pages.Select(
                page => Tuple.Create(Tuple.Create(enum1, enum2), page)).ToList();
        }

        protected IList<Tuple<Tuple<TEnum1, TEnum2>, PageType>> When(IEnumerable<TEnum1> enums1, TEnum2 enum2, params PageType[] pages)
        {
            return
                pages.SelectMany(
                    page =>
                        enums1.Select(e => Tuple.Create(Tuple.Create(e, enum2), page)))
                     .ToList();
        }

        protected IList<Tuple<Tuple<TEnum1, TEnum2>, PageType>> When(TEnum1 enum1, IEnumerable<TEnum2> enums2, params PageType[] pages)
        {
            return
                pages.SelectMany(
                    page =>
                        enums2.Select(e => Tuple.Create(Tuple.Create(enum1, e), page)))
                     .ToList();
        }

        protected IList<Tuple<Tuple<TEnum1, TEnum2>, PageType>> When(IEnumerable<Tuple<TEnum1, TEnum2>> enumsPairs, params PageType[] pages)
        {
            return
                pages.SelectMany(
                    page =>
                        enumsPairs.Select(pair => Tuple.Create(pair, page)))
                     .ToList();
        }

        /// <summary>
        /// Produces a set of all possible combinations of property types and activity types.
        /// </summary>
        /// <param name="pages">The pages.</param>
        /// <returns></returns>
        protected IList<Tuple<Tuple<TEnum1, TEnum2>, PageType>> ForAll(params PageType[] pages)
        {
            List<TEnum1> enums1 = EnumExtensions.GetValues<TEnum1>().ToList();
            List<TEnum2> enums2 = EnumExtensions.GetValues<TEnum2>().ToList();
            return
                pages.SelectMany(
                    page =>
                        enums1.SelectMany(
                            enum1 => enums2.Select(enum2 => Tuple.Create(Tuple.Create(enum1, enum2), page))))
                     .ToList();
        }
    }
}