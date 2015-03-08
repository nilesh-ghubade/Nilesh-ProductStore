using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MyProductStore.Models;

namespace MyProductStore.Controllers
{
    public class ProductsController : ApiController
    {
        readonly IProductRepository repository = new ProductRepositoryXML();

        //For unit testing purposes.
        public ProductsController(string str)
        {
            repository = new ProductRepositoryXML("TDD");
        }

        public ProductsController()
        {
            repository = new ProductRepositoryXML();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return repository.GetAll();
        }

        public Product GetProduct(string sku)
        {
            Product item = repository.Get(sku);
            if (item != null)
            {
                return item;
                //throw new HttpResponseException(HttpStatusCode.NotFound); 
            }
            return null;
            
        }


        public HttpResponseMessage PostProduct(Product item)
        {
            item = repository.Add(item);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

            string uri = Url.Link("DefaultApi", new { sku = item.Sku });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public HttpResponseMessage PutProduct(string sku, Product product)
        {
            product.Sku = sku;
            if (!repository.Update(product))
            {
                return Request.CreateResponse<Product>(HttpStatusCode.NotFound, product);
            }
            return Request.CreateResponse<Product>(HttpStatusCode.OK, product);
        }

        public HttpResponseMessage DeleteProduct(string sku)
        {
            Product item = repository.Get(sku);
            if (item != null)
            {
                var response = Request.CreateResponse<Product>(HttpStatusCode.OK, item);
                repository.Remove(sku);
                //throw new HttpResponseException(HttpStatusCode.NotFound);
                return response;
            }
            return Request.CreateResponse<Product>(HttpStatusCode.NotFound, item);
            
        }
    }
}
