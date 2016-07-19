namespace KnightFrank.Antares.Domain.Common
{
    public interface IReferenceMapper<TMessage, TEntity, TRelation>
    {
        void ValidateAndAssign(TMessage message, TEntity entity);
    }
}