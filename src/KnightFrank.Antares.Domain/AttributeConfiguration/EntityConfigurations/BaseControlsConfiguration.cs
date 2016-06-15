namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;

    public abstract class BaseControlsConfiguration<TKey1, TKey2> : IControlsConfiguration<TKey1, TKey2>
    {
        public IDictionary<PageType, IList<Control>> AvailableControls = new Dictionary<PageType, IList<Control>>();
        public IDictionary<Tuple<TKey1, TKey2, PageType>, IList<Control>> ControlsConfig = new Dictionary<Tuple<TKey1, TKey2, PageType>, IList<Control>>();

        protected BaseControlsConfiguration()
        {
            foreach (PageType pageType in EnumExtensions.GetValues<PageType>())
            {
                this.AvailableControls.Add(pageType, new List<Control>());
            }

            this.DefineControls();
            this.DefineMappings();
            this.ProcessBaseControls();
        }

        private void ProcessBaseControls()
        {
            foreach (PageType pageType in EnumExtensions.GetValues<PageType>())
            {
                List<Control> baseControls = this.AvailableControls[pageType].Where(x => x.ControlType == ControlType.Base).ToList();

                if (baseControls.Count > 1)
                {
                    foreach (Tuple<TKey1, TKey2, PageType> configurationKey in this.ControlsConfig.Keys)
                    {
                        if (configurationKey.Item3 == pageType)
                        {
                            baseControls.ForEach(c => this.ControlsConfig[configurationKey].Add(c.Copy()));
                        }
                    }
                }
            }
        }

        public IList<InnerFieldState> GetInnerFieldsState(PageType pageType, TKey1 key1, TKey2 key2, object entity)
        {
            //TODO: Controls config
            Console.WriteLine("*** GetState {0} {1} {2}", pageType, key1, key2);

            var configurationKey = new Tuple<TKey1, TKey2, PageType>(key1, key2, pageType);

            var innerFieldStates = new List<InnerFieldState>();

            if (!this.ControlsConfig.ContainsKey(configurationKey))
            {
                return innerFieldStates;
            }

            foreach (Control control in this.ControlsConfig[configurationKey])
            {
                innerFieldStates.AddRange(control.GetFieldStates(entity));
            }

            return innerFieldStates;
        }

        public abstract void DefineControls();

        public abstract void DefineMappings();
        
        protected void AddControl(PageType pageType, ControlCode controlCode, InnerField field)
        {
            this.AvailableControls[pageType].AddControl(ControlType.Extended, pageType, controlCode, field);
        }

        protected void AddControl(PageType pageType, ControlCode controlCode, IList<InnerField> field)
        {
            this.AvailableControls[pageType].AddControl(ControlType.Extended, pageType, controlCode, field);
        }

        protected void AddBaseControl(PageType pageType, ControlCode controlCode, InnerField field)
        {
            this.AvailableControls[pageType].AddControl(ControlType.Base, pageType, controlCode, field);
        }

        protected void AddBaseControl(PageType pageType, ControlCode controlCode, IList<InnerField> field)
        {
            this.AvailableControls[pageType].AddControl(ControlType.Base, pageType, controlCode, field);
        }

        protected IList<Control> Use(ControlCode controlCode, IList<Tuple<TKey1, TKey2, PageType>> list)
        {
            var result = new List<Control>();
            foreach (Tuple<TKey1, TKey2, PageType> item in list)
            {
                PageType pageType = item.Item3;
                if (!this.AvailableControls.ContainsKey(pageType))
                {
                    throw new Exception();
                }

                Control control = this.AvailableControls[pageType].FirstOrDefault(x => x.ControlCode == controlCode);
                if (control == null)
                {
                    throw new Exception();
                }

                Control controlCopy = control.Copy();

                if (this.ControlsConfig.ContainsKey(item))
                {
                    this.ControlsConfig[item].Add(controlCopy);
                }
                else
                {
                    this.ControlsConfig.Add(item, new List<Control> { controlCopy });
                }

                result.Add(controlCopy);
            }

            return result;
        }

        protected IList<Control> Use(IEnumerable<ControlCode> controlCodes, IList<Tuple<TKey1, TKey2, PageType>> list)
        {
            var result = new List<Control>();
            foreach (ControlCode controlCode in controlCodes)
            {
                result.AddRange(this.Use(controlCode, list));
            }

            return result;
        }

        protected IList<Tuple<PropertyType, ActivityType, PageType>> When(PropertyType propertyType, ActivityType activityType, params PageType[] pages)
        {
            return pages.Select(
                page => new Tuple<PropertyType, ActivityType, PageType>(propertyType, activityType, page)).ToList();
        }

        protected IList<Tuple<PropertyType, ActivityType, PageType>> When(IList<PropertyType> propertyType, ActivityType activityType, params PageType[] pages)
        {
            return pages.SelectMany(page => propertyType.Select(pt => new Tuple<PropertyType, ActivityType, PageType>(pt, activityType, page))).ToList();
        }

        protected IList<Tuple<PropertyType, ActivityType, PageType>> When(PropertyType propertyType, IList<ActivityType> activityType, params PageType[] pages)
        {
            return pages.SelectMany(page => activityType.Select(at => new Tuple<PropertyType, ActivityType, PageType>(propertyType, at, page))).ToList();
        }

        /// <summary>
        /// Produces a set of all possible combinations of property types and activity types.
        /// </summary>
        /// <param name="pages">The pages.</param>
        /// <returns></returns>
        protected IList<Tuple<PropertyType, ActivityType, PageType>> ForAll(params PageType[] pages)
        {
            List<ActivityType> allActivityTypes = EnumExtensions.GetValues<ActivityType>().ToList();
            List<PropertyType> allPropertyTypes = EnumExtensions.GetValues<PropertyType>().ToList();
            return
                pages.SelectMany(
                    page =>
                        allActivityTypes.SelectMany(
                            at => allPropertyTypes.Select(pt => new Tuple<PropertyType, ActivityType, PageType>(pt, at, page))))
                     .ToList();
        }
    }
}