using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Order.Event;
using MediatR;

namespace Domain.Statistics.EventHandler
{
    public class OrderPlacedHandler : INotificationHandler<OrderPlaced>
    {
        public void Handle(OrderPlaced notification)
        {
            // get additional data from other domains
            // put data into denormalized table
            // run signalR notification about changing data
        }
    }
}
