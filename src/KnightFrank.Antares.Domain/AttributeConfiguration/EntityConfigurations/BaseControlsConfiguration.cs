namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public abstract class BaseControlsConfiguration<TKey> : IControlsConfiguration<TKey> where TKey : IStructuralEquatable, IStructuralComparable, IComparable
    {
        public Dictionary<Tuple<PageType, ControlCode>, Tuple<Control, IList<IField>>> AvailableControls { get; }
        public Dictionary<Tuple<TKey, PageType>, IList<Tuple<Control, IList<IField>>>> ControlPageConfiguration { get; }

        [SuppressMessage("ReSharper", "VirtualMemberCallInContructor")]
        protected BaseControlsConfiguration()
        {
            this.AvailableControls = new Dictionary<Tuple<PageType, ControlCode>, Tuple<Control, IList<IField>>>();
            this.ControlPageConfiguration = new Dictionary<Tuple<TKey, PageType>, IList<Tuple<Control, IList<IField>>>>();
        }

        protected void Init()
        {
            this.DefineControls();
            this.DefineMappings();
            this.BuildConfiguration();
        }

        private void BuildConfiguration()
        {
            foreach (KeyValuePair<Tuple<TKey, PageType>, IList<Tuple<Control, IList<IField>>>> controlPageConfiguration in this.ControlPageConfiguration)
            {
                /*
                 * For each defined set of page type and a tuple key (e.g. property type and activity type)
                 * we need to find controls that have not been explicitly defined. These controls need now to be
                 * added to configuration with hidden expressions so that data shaping process could easily find
                 * fields that need to be got ridden of from entity.
                 */
                IEnumerable<KeyValuePair<Tuple<PageType, ControlCode>, Tuple<Control, IList<IField>>>> notConfiguredControls =
                    this.AvailableControls.Where(
                        x =>
                            /*
                             * In the list of available controls for current page type
                             * look for those controls that have not been configured i.e. are not present in controlPageConfiguration
                             */ 
                            x.Key.Item1 == controlPageConfiguration.Key.Item2 &&
                            controlPageConfiguration.Value.All(cpc => cpc.Item1.ControlCode != x.Key.Item2));

                foreach (KeyValuePair<Tuple<PageType, ControlCode>, Tuple<Control, IList<IField>>> control in notConfiguredControls)
                {
                    this.Use(control.Value.Item1.ControlCode, new List<Tuple<TKey, PageType>> { controlPageConfiguration.Key }).Hidden().Readonly();
                }

                foreach (Tuple<Control, IList<IField>> controlConfiguraiton in controlPageConfiguration.Value)
                {
                    foreach (IField field in controlConfiguraiton.Item2)
                    {
                        controlConfiguraiton.Item1.AddField(field.InnerField);
                    }
                }
            }
        }

        public IList<InnerFieldState> GetInnerFieldsState(PageType pageType, TKey key, object entity)
        {
            var configurationKey = new Tuple<TKey, PageType>(key, pageType);
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

        protected IList<Tuple<Control, IList<IField>>> Use(IEnumerable<ControlCode> controlCodes, IList<Tuple<TKey, PageType>> list)
        {
            var result = new List<Tuple<Control, IList<IField>>>();
            foreach (ControlCode controlCode in controlCodes)
            {
                result.AddRange(this.Use(controlCode, list));
            }

            return result;
        }

        protected IList<Tuple<Control, IList<IField>>> Use(ControlCode controlCode, IList<Tuple<TKey, PageType>> list)
        {
            var result = new List<Tuple<Control, IList<IField>>>();
            foreach (Tuple<TKey, PageType> configuration in list)
            {
                PageType pageType = configuration.Item2;
                var avialableControlsKey = new Tuple<PageType, ControlCode>(pageType, controlCode);

                if (!this.AvailableControls.ContainsKey(avialableControlsKey))
                {
                    throw new Exception($"{controlCode} control does not exist in {pageType} page configuration.");
                }

                Control control = this.AvailableControls[avialableControlsKey].Item1;
                IList<IField> fields = this.AvailableControls[avialableControlsKey].Item2;
                Control controlCopy = control.Copy();
                IList<IField> fieldsCopy = fields.Select(x => x.Copy()).ToList();

                var controlDefinition = new Tuple<Control, IList<IField>>(controlCopy, fieldsCopy);

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
    }
}