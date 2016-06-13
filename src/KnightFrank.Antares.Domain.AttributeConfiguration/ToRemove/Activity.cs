namespace KnightFrank.Antares.Domain.AttributeConfiguration.ToRemove
{
    using System;

    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;

    public class Activity : IActivity
    {
        public int StatusId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SalesPrice { get; set; }
    }
}