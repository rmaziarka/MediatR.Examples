using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Catalog.Application;
using Domain.Catalog.Dto;
using Domain.Catalog.Event;

namespace WebApi.Controllers
{
    [RoutePrefix("api/catalog")]
    public class CatalogController : ApiController
    {
        private readonly ProductsService _productsService;

        public CatalogController(ProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpPost]
        [Route("purchase")]
        public HttpStatusCode PurchasedProduct(NewPurchaseDto dto)
        {
            // if app service handled purchasing return OK
            // otherwise return bad request
            try
            {
                _productsService.PurchaseProducts(dto);
                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }
        }
    }
}
