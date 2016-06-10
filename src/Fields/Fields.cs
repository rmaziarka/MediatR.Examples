namespace Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IActivity
    {
        decimal RecommendedPrice { get; set; }
        decimal MarketAppraisalPrice { get; set; }
        DateTime? From { get; set; }
        DateTime? To { get; set; }
        Guid StatusId { get; set; }
    }

    public class Activity : IActivity
    {
        public decimal RecommendedPrice { get; set; }
        public decimal MarketAppraisalPrice { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid StatusId { get; set; }
    }

    public class CreateCommand
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid StatusId { get; set; }
    }

    [Serializable]
    public class Field<TEntity>
    {
        private Field()
        {
        }

        public bool IsRequired { get; set; }

        public string Name { get; protected set; }

        private readonly List<FieldValidator<TEntity>> validators = new List<FieldValidator<TEntity>>();

        public List<FieldValidator<TEntity>> GetValidators()
        {
            return this.validators;
        }

        public static Field<TEntity> Create<TProperty>(Expression<Func<TEntity, TProperty>> fieldSelector, bool isRequired = false)
        {
            return new Field<TEntity>
            {
                Name = fieldSelector.GetMemberName(),
                IsRequired = isRequired
            };
        }

        public void Validate(TEntity entity)
        {
            foreach (FieldValidator<TEntity> validator in this.validators)
            {
                validator.Validate(entity);
            }
        }
    }
    public enum ControlCode
    {
        Status,
        DateRange,
        RecommendedPrice,
        MarketAppraisalPrice
    }

    public abstract class FieldValidator<TEntity>
    {
        public abstract void Validate(TEntity entity);
    }

    public class RequiredFieldValidator<TEntity> : FieldValidator<TEntity>
    {
        public override void Validate(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }

    public class Control<TEntity>
    {
        public List<Field<TEntity>> InnerFields = new List<Field<TEntity>>();
        public readonly ControlCode ControlCode;

        public Control(ControlCode controlCode)
        {
            this.ControlCode = controlCode;
        }
    }

    public class PriceControl<TEntity> : Control<TEntity>
    {
        protected PriceControl(ControlCode controlCode, Expression<Func<TEntity, decimal?>> priceSelector, bool required)
            :base(controlCode)
        {
            Field<TEntity> field = Field<TEntity>.Create(priceSelector, required);
            this.InnerFields.Add(field);
        }
    }

    public class AppraisalPriceControl : PriceControl<IActivity>
    {
        public AppraisalPriceControl(bool required = false)
            : base(ControlCode.MarketAppraisalPrice, x => x.MarketAppraisalPrice, required)
        {
        }
    }

    public class RecommendedPriceControl : PriceControl<IActivity>
    {
        public RecommendedPriceControl(bool required = false)
            : base(ControlCode.RecommendedPrice, x => x.RecommendedPrice, required)
        {
        }
    }

    public class ActivityDatesControl : Control<IActivity>
    {
        public ActivityDatesControl()
            : base(ControlCode.DateRange)
        {
            this.InnerFields.Add(Field<IActivity>.Create(x => x.From));
            this.InnerFields.Add(Field<IActivity>.Create(x => x.To));
        }
    }

    public enum PageType
    {
        Create,
        Update,
        Details
    }

    public class ControlsConfiguration<TEntity>
    {
        public readonly IDictionary<PageType, List<Control<TEntity>>> VisibleControls
            = new Dictionary<PageType, List<Control<TEntity>>>
            {
                { PageType.Create, new List<Control<TEntity>>() },
                { PageType.Update, new List<Control<TEntity>>() },
                { PageType.Details, new List<Control<TEntity>>() }
            };
    }

    public enum PropertyType
    {
        Flat,
        House
    }

    public enum ActivityType
    {
        Sales,
        Lettings
    }

    public class ActivityControlsConfiguration : ControlsConfiguration<IActivity>
    {
        public ActivityControlsConfiguration()
        {
            this.VisibleControls[PageType.Create].AddRange(new Control<IActivity>[]
            {
                new AppraisalPriceControl(required: false),
                new RecommendedPriceControl(required: true)
            });

            this.VisibleControls[PageType.Update].AddRange(new Control<IActivity>[]
            {
                new AppraisalPriceControl(required: false),
                new RecommendedPriceControl(required: true),
                new ActivityDatesControl()
            });

            this.VisibleControls[PageType.Details].AddRange(new Control<IActivity>[]
            {
                new AppraisalPriceControl(),
                new RecommendedPriceControl(),
                new ActivityDatesControl()
            });
        }

    }

    
    
}
