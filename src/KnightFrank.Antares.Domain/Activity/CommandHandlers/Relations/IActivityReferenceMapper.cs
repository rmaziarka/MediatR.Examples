namespace KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations
{
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;

    public interface IActivityReferenceMapper<TRelation>
    {
        void ValidateAndAssign(ActivityCommandBase message, Activity activity);
    }
}