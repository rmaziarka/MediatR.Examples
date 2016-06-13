namespace Fields
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

            var createCommand = new CreateCommand();
            createCommand.BuyPrice = 10m;
            createCommand.StatusId = 2;


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

            Console.ReadLine();
        }
    }

    public class ActivityControlsConfiguration
    {
        public IDictionary<PageType, IList<Control>> ControlsDictionary = new Dictionary<PageType, IList<Control>>();
        public IDictionary<Tuple<PropertyType, ActivityType, PageType>, Control> ControlsConfig = new Dictionary<Tuple<PropertyType, ActivityType, PageType>, Control>();

        public ActivityControlsConfiguration()
        {
            this.DefineControls();
            this.DefineMapping();
        }

        private void AddControl(PageType pageType, ControlCode controlCode, InnerField field)
        {
            this.ControlsDictionary[pageType].AddControl(pageType, controlCode, field);
        }

        private void AddControl(PageType pageType, ControlCode controlCode, IList<InnerField> field)
        {
            this.ControlsDictionary[pageType].AddControl(pageType, controlCode, field);
        }

        public void DefineControls()
        {
            this.ControlsDictionary.Add(PageType.Create, new List<Control>());
            this.ControlsDictionary.Add(PageType.Update, new List<Control>());
            this.ControlsDictionary.Add(PageType.Details, new List<Control>());

            //AddControl(PageType.Create, ControlCode.Status, Field<CreateCommand>.CreateDictionary(x => x.StatusId, "StatusTypes").InnerField);

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

        public void DefineMapping()
        {
            Use(ControlCode.BuyPrice, When(PropertyType.Flat, ActivityType.Lettings, PageType.Create))
                .IsReadonlyWhen<CreateCommand>(x => x.StatusId == 1)
                .FieldIsReadonlyWhen<CreateCommand>(x => x.BuyPrice, x => x.BuyPrice > 10);

            //Use(ControlCode.BuyPrice, When(PropertyType.Flat, ActivityType.Lettings, PageType.Create)).IsReadonlyWhen<CreateCommand>(x => x.StatusId == 2);
        }

        private IList<Control> Use(ControlCode controlCode, IList<Tuple<PropertyType, ActivityType, PageType>> list)
        {
            var result = new List<Control>();
            foreach (var item in list)
            {
                PropertyType propertyType = item.Item1;
                ActivityType activityType = item.Item2;
                PageType pagetType = item.Item3;

                if (!this.ControlsDictionary.ContainsKey(pagetType))
                {
                    throw new Exception();
                }

                var control = this.ControlsDictionary[pagetType].FirstOrDefault(x => x.ControlCode == controlCode);
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

        private IList<Tuple<PropertyType, ActivityType, PageType>> When(PropertyType propertyType, ActivityType activityType, params PageType[] pages)
        {
            return pages.Select(
                page => new Tuple<PropertyType, ActivityType, PageType>(propertyType, activityType, page)).ToList();
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
