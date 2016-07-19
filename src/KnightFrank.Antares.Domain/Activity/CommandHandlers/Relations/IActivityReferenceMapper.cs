namespace KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations
{
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common;

    public interface IActivityReferenceMapper<TRelation> : IReferenceMapper<ActivityCommandBase, Activity, TRelation>
    {
    }
}