namespace KnightFrank.Antares.Domain.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IActivity
    {
        decimal RecommendedPrice { get; set; }
        decimal MarketAppraisalPrice { get; set; }
    }

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
            var expression = (MemberExpression)fieldSelector.Body;
            string fieldName = expression.Member.Name;
            return new Field<TEntity>
            {
                Name = fieldName,
                IsRequired = isRequired
            };
        }

        public void Validate(TEntity entity)
        {
            foreach (var validator in this.validators)
            {
                validator.Validate(entity);
            }
        }
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
        protected List<Field<TEntity>> InnerFields = new List<Field<TEntity>>();
    }

    public class PriceControl<TEntity> : Control<TEntity>
    {
        protected PriceControl(Expression<Func<TEntity, decimal?>> priceSelector, bool required)
        {
            Field<TEntity> field = Field<TEntity>.Create(priceSelector, required);
            this.InnerFields.Add(field);
        }
    }

    public class AppraisalPriceControl : PriceControl<IActivity>
    {
        public AppraisalPriceControl(bool required)
            : base(x => x.MarketAppraisalPrice, required)
        {
        }
    }

    public class RecommendedPriceControl : PriceControl<IActivity>
    {
        public RecommendedPriceControl(bool required)
            : base(x => x.RecommendedPrice, required)
        {
        }
    }

    public class ControlsConfiguration<TEntity>
    {
        public readonly List<Control<TEntity>> VisibleControls = new List<Control<TEntity>>();
    }

    public class CreateActivityControlsConfiguration : ControlsConfiguration<IActivity>
    {
        public CreateActivityControlsConfiguration()
        {
            this.VisibleControls.AddRange(new Control<IActivity>[]
            {
                new AppraisalPriceControl(required: false),
                new RecommendedPriceControl(required: true)
            });
        }
    }

    public class NewActivity : IActivity
    {
        public decimal RecommendedPrice { get; set; }
        public decimal MarketAppraisalPrice { get; set; }
    }

    public static class DoIt
    {
        public static object GetConfig()
        {
            var config = new CreateActivityControlsConfiguration();
            return config;
        }

        public static void Validate()
        {
            var newActivity = new NewActivity();
            var config = new CreateActivityControlsConfiguration();
        }
    }
}
