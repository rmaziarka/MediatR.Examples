namespace Fields
{
    using System.Collections.Generic;

    using Fields.Enums;

    public class Control
    {
        public readonly ControlCode ControlCode;
        public readonly IList<InnerField> Fields;
        public readonly PageType PageType;
        
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
    }
}