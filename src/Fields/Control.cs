namespace Fields
{
    using System.Collections.Generic;

    using Fields.Enums;

    public class Control
    {
        public readonly ControlCode ControlCode;
        public readonly IList<InnerField> Fields;
        
        public Control(ControlCode controlCode, IList<InnerField> fields)
        {
            this.ControlCode = controlCode;
            this.Fields = fields;
        }

        public Control(ControlCode controlCode, InnerField create)
        {
            this.Fields = new List<InnerField> { create };
            this.ControlCode = controlCode;
        }
    }
}