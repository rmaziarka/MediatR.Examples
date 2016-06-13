﻿namespace Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fields.Enums;
    using Fields.Extensions;

    class Program
    {
        static void Main(string[] args)
        {
            // pobierz czysta konfiguracje
            // pobierz konfiguracje dla obietku
            // Waliduj

            //date range validator

            var createCommand = new CreateCommand
            {
                BuyPrice = 200m,
                StatusId = 2
            };

            var createCommand2 = new CreateCommand
            {
                BuyPrice = 10m,
                StatusId = 1
            };


            var xyz = new ActivityControlsConfiguration();

            foreach (Control item in xyz.ControlsDictionary[PageType.Create])
            {
                Console.WriteLine("IsReadonly: " + item.IsReadonly(createCommand));
                Console.WriteLine("IsHidden: " + item.IsHidden(createCommand));
                foreach (InnerField innerField in item.Fields)
                {
                    innerField.Validate(createCommand);
                }
            }

            var states = xyz.GetInnerFieldState(PageType.Create, PropertyType.Flat, ActivityType.Lettings, createCommand);
            foreach (var state in states)
            {
                Console.WriteLine("[{0}] -- hidden: {1}, readonly: {2}, no. of validators: {3}", state.Name, state.IsHidden, state.IsReadonly, state.Validators.Count);
            }

            states = xyz.GetInnerFieldState(PageType.Create, PropertyType.Flat, ActivityType.Lettings, createCommand2);
            foreach (var state in states)
            {
                Console.WriteLine("[{0}] -- hidden: {1}, readonly: {2}, no. of validators: {3}", state.Name, state.IsHidden, state.IsReadonly, state.Validators.Count);
            }

            Console.ReadLine();
        }
    }

    public interface IControlsConfiguration<in TKey1, in TKey2>
    {
        IList<InnerFieldState> GetInnerFieldState(PageType pageType, TKey1 key1, TKey2 key2, object entity);
    }

    public abstract class BaseControlsConfiguration<TKey1, TKey2> : IControlsConfiguration<TKey1, TKey2>
    {
        public IDictionary<PageType, IList<Control>> ControlsDictionary = new Dictionary<PageType, IList<Control>>();
        public IDictionary<Tuple<TKey1, TKey2, PageType>, Control> ControlsConfig = new Dictionary<Tuple<TKey1, TKey2, PageType>, Control>();

        protected BaseControlsConfiguration()
        {
            this.DefineControls();
            this.DefineMappings();
        }

        public IList<InnerFieldState> GetInnerFieldState(PageType pageType, TKey1 key1, TKey2 key2, object entity)
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

    public class ActivityControlsConfiguration : BaseControlsConfiguration<PropertyType, ActivityType>
    {
        public override void DefineControls()
        {
            this.ControlsDictionary.Add(PageType.Create, new List<Control>());
            this.ControlsDictionary.Add(PageType.Update, new List<Control>());
            this.ControlsDictionary.Add(PageType.Details, new List<Control>());

            //AddControl(PageType.Create, ControlCode.Status, Field<CreateCommand>.CreateDictionary(x => x.StatusId, "StatusTypes").InnerField);

            // TODO: verify if page type has field for one entity type

            AddControl(PageType.Create, ControlCode.BuyPrice, Field<CreateCommand>.Create(x => x.BuyPrice).GreaterThan(100).InnerField);

            AddControl(PageType.Update, ControlCode.Status, Field<CreateCommand>.CreateDictionary(x => x.StatusId, "StatusTypes").InnerField);

            AddControl(PageType.Update, ControlCode.BuyPrice, Field<UpdateCommand>.Create(x => x.BuyPrice).InnerField);

            AddControl(PageType.Update, ControlCode.SalesPrice, Field<UpdateCommand>.Create(x => x.SalesPrice).InnerField);

            AddControl(PageType.Update, ControlCode.DateRange,
                new List<InnerField>
                    {
                        Field<UpdateCommand>.Create(x => x.From).InnerField,
                        Field<UpdateCommand>.Create(x => x.To).InnerField
                    });

            AddControl(PageType.Details, ControlCode.Status, Field<IActivity>.CreateDictionary(x => x.StatusId, "StatusTypes").InnerField);

            AddControl(PageType.Details, ControlCode.BuyPrice, Field<IActivity>.Create(x => x.BuyPrice).InnerField);

            AddControl(PageType.Details, ControlCode.SalesPrice, Field<IActivity>.Create(x => x.SalesPrice).InnerField);

            AddControl(PageType.Details, ControlCode.DateRange,
                    new List<InnerField>
                    {
                        Field<IActivity>.Create(x => x.From).InnerField,
                        Field<IActivity>.Create(x => x.To).InnerField
                    });
        }

        public override void DefineMappings()
        {
            Use(ControlCode.BuyPrice, When(PropertyType.Flat, ActivityType.Lettings, PageType.Create))
                .IsReadonlyWhen<CreateCommand>(x => x.StatusId == 1)
                .FieldIsReadonlyWhen<CreateCommand, decimal>(x => x.BuyPrice, x => x.BuyPrice > 10);

            //Use(ControlCode.BuyPrice, When(PropertyType.Flat, ActivityType.Lettings, PageType.Create)).IsReadonlyWhen<CreateCommand>(x => x.StatusId == 2);
        }
    }

    public class CreateCommand
    {
        public int StatusId { get; set; }
        public decimal BuyPrice { get; set; }
    }

    public class UpdateCommand
    {
        public int StatusId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SalesPrice { get; set; }
    }

    public interface IActivity
    {
        int StatusId { get; set; }
        DateTime From { get; set; }
        DateTime To { get; set; }
        decimal BuyPrice { get; set; }
        decimal SalesPrice { get; set; }
    }
    public class Activity : IActivity
    {
        public int StatusId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SalesPrice { get; set; }
    }
}
