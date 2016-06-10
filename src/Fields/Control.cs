namespace Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Fields.Enums;

    public class Control
    {
        public readonly ControlCode ControlCode;
        public readonly IList<InnerField> Fields;
        public readonly PageType PageType;

        public bool IsReadonly(object entity) => entity != null && ((bool?)this.isReadonlyExpression?.DynamicInvoke(entity) ?? false);
        public bool IsHidden(object entity) => entity != null && ((bool?)this.isHiddenExpression?.DynamicInvoke(entity) ?? false);

        private Delegate isHiddenExpression;
        private Delegate isReadonlyExpression;

        public Control(PageType pageType, ControlCode controlCode, IList<InnerField> fields)
        {
            this.ControlCode = controlCode;
            this.Fields = fields;
            this.PageType = pageType;
        }

        public Control(PageType pageType, ControlCode controlCode, InnerField create)
        {
            this.Fields = new List<InnerField> { create };
            this.ControlCode = controlCode;
            this.PageType = pageType;
        }

        public void SetReadonlyRule(LambdaExpression expression)
        {
            this.isReadonlyExpression = expression.Compile();
        }

        public void SetHiddenRule(LambdaExpression expression)
        {
            this.isHiddenExpression = expression.Compile();
        }
    }
}