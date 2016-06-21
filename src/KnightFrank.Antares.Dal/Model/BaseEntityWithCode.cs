namespace KnightFrank.Antares.Dal.Model
{
    public abstract class BaseEntityWithCode : BaseEntity
    {
        public string Code { get; set; }

        public string EnumCode { get; set; }
    }
}