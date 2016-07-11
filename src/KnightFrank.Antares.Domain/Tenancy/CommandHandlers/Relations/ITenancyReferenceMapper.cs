namespace KnightFrank.Antares.Domain.Tenancy.CommandHandlers.Relations
{
    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Tenancy.Commands;

    public interface ITenancyReferenceMapper<TRelation> : IReferenceMapper<TenancyCommandBase, Tenancy, TRelation>
    {
    }
}