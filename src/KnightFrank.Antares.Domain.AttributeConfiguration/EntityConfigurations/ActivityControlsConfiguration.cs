namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.AttributeConfiguration.ToRemove;

    public class ActivityControlsConfiguration : BaseControlsConfiguration<PropertyType, ActivityType>
    {
        public override void DefineControls()
        {
            this.ControlsDictionary.Add(PageType.Create, new List<Control>());
            this.ControlsDictionary.Add(PageType.Update, new List<Control>());
            this.ControlsDictionary.Add(PageType.Details, new List<Control>());

            //AddControl(PageType.Create, ControlCode.Status, Field<CreateCommand>.CreateDictionary(x => x.StatusId, "StatusTypes").InnerField);

            // TODO: verify if page type has field for one entity type

            this.AddControl(PageType.Create, ControlCode.BuyPrice, Field<CreateCommand>.Create(x => x.BuyPrice).GreaterThan(100).InnerField);

            this.AddControl(PageType.Update, ControlCode.Status, Field<CreateCommand>.CreateDictionary(x => x.StatusId, "StatusTypes").InnerField);

            this.AddControl(PageType.Update, ControlCode.BuyPrice, Field<UpdateCommand>.Create(x => x.BuyPrice).InnerField);

            this.AddControl(PageType.Update, ControlCode.SalesPrice, Field<UpdateCommand>.Create(x => x.SalesPrice).InnerField);

            this.AddControl(PageType.Update, ControlCode.DateRange,
                new List<InnerField>
                {
                    Field<UpdateCommand>.Create(x => x.From).InnerField,
                    Field<UpdateCommand>.Create(x => x.To).InnerField
                });

            this.AddControl(PageType.Details, ControlCode.Status, Field<IActivity>.CreateDictionary(x => x.StatusId, "StatusTypes").InnerField);

            this.AddControl(PageType.Details, ControlCode.BuyPrice, Field<IActivity>.Create(x => x.BuyPrice).InnerField);

            this.AddControl(PageType.Details, ControlCode.SalesPrice, Field<IActivity>.Create(x => x.SalesPrice).InnerField);

            this.AddControl(PageType.Details, ControlCode.DateRange,
                new List<InnerField>
                {
                    Field<IActivity>.Create(x => x.From).InnerField,
                    Field<IActivity>.Create(x => x.To).InnerField
                });
        }

        public override void DefineMappings()
        {
            this.Use(ControlCode.BuyPrice, this.When(PropertyType.Flat, ActivityType.Lettings, PageType.Create))
                .IsReadonlyWhen<CreateCommand>(x => x.StatusId == 1)
                .FieldIsReadonlyWhen<CreateCommand, decimal>(x => x.BuyPrice, x => x.BuyPrice > 10);

            //Use(ControlCode.BuyPrice, When(PropertyType.Flat, ActivityType.Lettings, PageType.Create)).IsReadonlyWhen<CreateCommand>(x => x.StatusId == 2);
        }
    }
}