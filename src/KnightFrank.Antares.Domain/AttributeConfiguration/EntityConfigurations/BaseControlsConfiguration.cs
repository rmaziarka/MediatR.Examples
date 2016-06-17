namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;

    public abstract class BaseControlsConfiguration<TKey1, TKey2> : IControlsConfiguration<TKey1, TKey2>
    {
        public Dictionary<Tuple<PageType, ControlCode>, Tuple<Control, IList<IField>>> AvailableControls { get; }
        public Dictionary<Tuple<TKey1, TKey2, PageType>, IList<Tuple<Control, IList<IField>>>> ControlPageConfiguration { get; }

        protected BaseControlsConfiguration()
        {
            this.AvailableControls = new Dictionary<Tuple<PageType, ControlCode>, Tuple<Control, IList<IField>>>();
            this.ControlPageConfiguration = new Dictionary<Tuple<TKey1, TKey2, PageType>, IList<Tuple<Control, IList<IField>>>>();

            this.DefineControls();
            this.DefineMappings();
            this.BuildConfiguration();
        }

        private void BuildConfiguration()
        {
            foreach (KeyValuePair<Tuple<TKey1, TKey2, PageType>, IList<Tuple<Control, IList<IField>>>> controlPageConfiguration in this.ControlPageConfiguration)
            {
                foreach (Tuple<Control, IList<IField>> controlConfiguraiton in controlPageConfiguration.Value)
                {
                    foreach (IField field in controlConfiguraiton.Item2)
                    {
                        controlConfiguraiton.Item1.AddField(field.InnerField);
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

            if (!this.ControlPageConfiguration.ContainsKey(configurationKey))
            {
                return innerFieldStates;
            }

            foreach (Tuple<Control, IList<IField>> controlConfiguraiton in this.ControlPageConfiguration[configurationKey])
            {
                innerFieldStates.AddRange(controlConfiguraiton.Item1.GetFieldStates(entity));
            }

            return innerFieldStates;
        }

        public abstract void DefineControls();

        public abstract void DefineMappings();

        public void AddControl(PageType pageType, ControlCode controlCode, IField field)
        {
            this.AddControl(pageType, controlCode, new List<IField> { field });
        }

        public void AddControl(PageType pageType, ControlCode controlCode, IList<IField> field)
        {
            var control = new Control(pageType, controlCode);
            var key = new Tuple<PageType, ControlCode>(pageType, controlCode);

            if (this.AvailableControls.ContainsKey(key))
            {
                throw new NotSupportedException($"{controlCode} control already exists in {pageType} page configuration.");
            }

            var value = new Tuple<Control, IList<IField>>(control, field);
            this.AvailableControls.Add(key, value);
        }

        protected IList<Tuple<Control, IList<IField>>> Use(IEnumerable<ControlCode> controlCodes, IList<Tuple<TKey1, TKey2, PageType>> list)
        {
            var result = new List<Tuple<Control, IList<IField>>>();
            foreach (ControlCode controlCode in controlCodes)
            {
                result.AddRange(this.Use(controlCode, list));
            }

            return result;
        }

        protected IList<Tuple<Control, IList<IField>>> Use(ControlCode controlCode, IList<Tuple<TKey1, TKey2, PageType>> list)
        {
            var result = new List<Tuple<Control, IList<IField>>>();
            foreach (Tuple<TKey1, TKey2, PageType> configuration in list)
            {
                PageType pageType = configuration.Item3;
                var avialableControlsKey = new Tuple<PageType, ControlCode>(pageType, controlCode);

                if (!this.AvailableControls.ContainsKey(avialableControlsKey))
                {
                    throw new Exception($"{controlCode} control does not exist in {pageType} page configuration.");
                }

                Control control = this.AvailableControls[avialableControlsKey].Item1;
                IList<IField> fields = this.AvailableControls[avialableControlsKey].Item2;
                Control controlCopy = control.Copy();
                // TODO: deep copy fields
                var controlDefinition = new Tuple<Control, IList<IField>>(controlCopy, fields);

                if (this.ControlPageConfiguration.ContainsKey(configuration))
                {
                    this.ControlPageConfiguration[configuration].Add(controlDefinition);
                }
                else
                {
                    this.ControlPageConfiguration.Add(configuration, new List<Tuple<Control, IList<IField>>> { controlDefinition });
                }
                result.Add(controlDefinition);
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