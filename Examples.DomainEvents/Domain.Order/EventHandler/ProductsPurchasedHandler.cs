using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Catalog.Event;
using Domain.Order.Event;
using MediatR;

namespace Domain.Order.EventHandler
{
    public class ProductsPurchasedHandler: INotificationHandler<ProductsPurchased>
    {
        private IMediator _mediator;

        public ProductsPurchasedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Handle(ProductsPurchased productsPurchasedEvent)
        {
            // create order based on event data
            // calculate order total price based on user discount
            // save order to database
            // create event
            var orderPlacedEvent = new OrderPlaced();
            _mediator.Publish(orderPlacedEvent);
        }
    }
}
