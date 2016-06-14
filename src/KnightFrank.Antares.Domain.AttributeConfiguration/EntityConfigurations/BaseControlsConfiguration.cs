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
        public IDictionary<PageType, IList<Control>> ControlsDictionary = new Dictionary<PageType, IList<Control>>();
        public IDictionary<Tuple<TKey1, TKey2, PageType>, Control> ControlsConfig = new Dictionary<Tuple<TKey1, TKey2, PageType>, Control>();

        protected BaseControlsConfiguration()
        {
            this.DefineControls();
            this.DefineMappings();
        }

        public IList<InnerFieldState> GetInnerFieldsState(PageType pageType, TKey1 key1, TKey2 key2, object entity)
        {
            Console.WriteLine("*** GetState {0} {1} {2}", pageType, key1, key2);
            var innerFieldStates = new List<InnerFieldState>();
            IList<Control> controls;
            if (this.ControlsDictionary.TryGetValue(pageType, out controls))
            {
                foreach (Control control in controls)
                {
                    innerFieldStates.AddRange(control.GetFieldStates(entity));
                }

                return innerFieldStates;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public abstract void DefineControls();

        public abstract void DefineMappings();


        protected void AddControl(PageType pageType, ControlCode controlCode, InnerField field)
        {
            this.ControlsDictionary[pageType].AddControl(pageType, controlCode, field);
        }

        protected void AddControl(PageType pageType, ControlCode controlCode, IList<InnerField> field)
        {
            this.ControlsDictionary[pageType].AddControl(pageType, controlCode, field);
        }

        protected IList<Control> Use(ControlCode controlCode, IList<Tuple<TKey1, TKey2, PageType>> list)
        {
            var result = new List<Control>();
            foreach (Tuple<TKey1, TKey2, PageType> item in list)
            {
                PageType pageType = item.Item3;
                if (!this.ControlsDictionary.ContainsKey(pageType))
                {
                    throw new Exception();
                }

                var control = this.ControlsDictionary[pageType].FirstOrDefault(x => x.ControlCode == controlCode);
                if (control == null)
                {
                    throw new Exception();
                }

                // TODO: deep clone - don't use serialization, it does not work here
                //Control controlClone = control.Clone();
                //Control controlCopy = AutoMapper.Mapper.Map<Control>(control);

                this.ControlsConfig.Add(item, control);
                result.Add(control);
            }

            return result;
        }
        protected IList<Tuple<PropertyType, ActivityType, PageType>> When(PropertyType propertyType, ActivityType activityType, params PageType[] pages)
        {
            return pages.Select(
                page => new Tuple<PropertyType, ActivityType, PageType>(propertyType, activityType, page)).ToList();
        }
    }
}