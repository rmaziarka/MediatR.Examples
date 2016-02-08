using Common.Event;

namespace Domain.Order.Event
{
    public class OrderPlaced: DomainEvent
    {
        public int OrderId { get; set; }
    }
}
