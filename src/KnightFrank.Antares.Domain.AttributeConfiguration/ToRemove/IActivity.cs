namespace KnightFrank.Antares.Domain.AttributeConfiguration.ToRemove
{
    using System;

    public interface IActivity
    {
        int StatusId { get; set; }
        DateTime From { get; set; }
        DateTime To { get; set; }
        decimal BuyPrice { get; set; }
        decimal SalesPrice { get; set; }
    }
}