using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Catalog.Dto;
using Domain.Catalog.Event;
using MediatR;

namespace Domain.Catalog.Application
{
    public class ProductsService
    {
        private IMediator mediator;

        public ProductsService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public void PurchaseProducts(NewPurchaseDto dto)
        {
            // check if user can purchase these products
            // save products with decreased quantity
            // handle concurent purchasing 
            // create event based on database data

            var @event = new ProductsPurchased();
            mediator.Publish(@event);
        }
    }
}
