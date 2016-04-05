namespace KnightFrank.Antares.Dal.Model.Attributes.Attribute
{
    using KnightFrank.Antares.Dal.Model.Attributes.Field;

    public class TextWithComboboxAttribute : FormAttribute
    {
        public virtual Field Text { get; set; }

        public virtual ComboboxField Combobox { get; set; }
    }
}
