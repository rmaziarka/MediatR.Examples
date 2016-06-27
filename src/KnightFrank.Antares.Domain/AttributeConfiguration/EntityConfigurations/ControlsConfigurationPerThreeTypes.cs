namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    public abstract class ControlsConfigurationPerThreeTypes<TEnum1, TEnum2, TEnum3> : BaseControlsConfiguration<Tuple<TEnum1, TEnum2, TEnum3>> where TEnum1 : struct where TEnum2 : struct where TEnum3 : struct
    {
        protected IList<Tuple<Tuple<TEnum1, TEnum2, TEnum3>, PageType>> When(TEnum1 enum1, TEnum2 enum2, TEnum3 enum3, params PageType[] pages)
        {
            return pages.Select(
                page =>
                    new Tuple<Tuple<TEnum1, TEnum2, TEnum3>, PageType>(new Tuple<TEnum1, TEnum2, TEnum3>(enum1, enum2, enum3), page))
                        .ToList();
        }

        protected IList<Tuple<Tuple<TEnum1, TEnum2, TEnum3>, PageType>> When(IList<TEnum1> enums1, TEnum2 enum2, TEnum3 enum3, params PageType[] pages)
        {
            return
                pages.SelectMany(
                    page =>
                        enums1.Select(
                            e =>
                                new Tuple<Tuple<TEnum1, TEnum2, TEnum3>, PageType>(
                                    new Tuple<TEnum1, TEnum2, TEnum3>(e, enum2, enum3), page))).ToList();
        }

        protected IList<Tuple<Tuple<TEnum1, TEnum2, TEnum3>, PageType>> When(TEnum1 enum1, IList<TEnum2> enums2, TEnum3 enum3, params PageType[] pages)
        {
            return
                pages.SelectMany(
                    page =>
                        enums2.Select(e => new Tuple<Tuple<TEnum1, TEnum2, TEnum3>, PageType>(new Tuple<TEnum1, TEnum2, TEnum3>(enum1, e, enum3), page)))
                     .ToList();
        }

        /// <summary>
        /// Produces a set of all possible combinations of property types and activity types.
        /// </summary>
        /// <param name="pages">The pages.</param>
        /// <returns></returns>
        protected IList<Tuple<Tuple<TEnum1, TEnum2, TEnum3>, PageType>> ForAll(params PageType[] pages)
        {
            List<TEnum1> enums1 = EnumExtensions.GetValues<TEnum1>().ToList();
            List<TEnum2> enums2 = EnumExtensions.GetValues<TEnum2>().ToList();
            List<TEnum3> enums3 = EnumExtensions.GetValues<TEnum3>().ToList();

            return pages.SelectMany(
                page =>
                    enums1.SelectMany(
                        enum1 => enums2.SelectMany(enum2 =>
                            enums3.Select(
                                enum3 =>
                                    new Tuple<Tuple<TEnum1, TEnum2, TEnum3>, PageType>(
                                        new Tuple<TEnum1, TEnum2, TEnum3>(enum1, enum2, enum3), page)))))
                        .ToList();
        }
    }
}