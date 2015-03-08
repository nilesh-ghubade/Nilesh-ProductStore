using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MyProductStore.Models
{
    public class ProductRepositoryXML : IProductRepository
    {
        private List<Product> allProducts = new List<Product>();
        private int _nextSku = 1;
        private XDocument productData;

        // For unit testing purpose.
        public ProductRepositoryXML(string str)
        {
            allProducts.Add(new Product { Sku = "Sku1", Name = "Test1", Description = "Test Description1", Price = 1.1M });
            allProducts.Add(new Product { Sku = "Sku2", Name = "Test2", Description = "Test Description2", Price = 2.2M });
        }
        public ProductRepositoryXML()
        {
            if (HttpContext.Current != null)
            {
                productData = XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/Products.xml"));
                var products = from product in productData.Descendants("product")
                                select new Product
                                {
                                    Sku = product.Element("sku").Value,
                                    Name = product.Element("name").Value,
                                    Description = product.Element("description").Value,
                                    Price = (Decimal)product.Element("price")
                                };

                allProducts.AddRange(products.ToList<Product>());
            }

            _nextSku = allProducts.Count - 1;
            
        }

        public IEnumerable<Product> GetAll()
        {
            return allProducts;
        }

        public Product Get(string sku)
        {
            return allProducts.Find(p => p.Sku.ToLower() == sku.ToLower());
        }

        public Product Add(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Null item!");
            }

            var query = (from p in allProducts
                         orderby Convert.ToInt32(p.Sku.Substring(3)) descending
                        select p.Sku).FirstOrDefault();
            
            if ((query != null) && (!query.ToString().Equals("")))
                _nextSku = Convert.ToInt32(query.ToString().Substring(3));

            _nextSku++;
            item.Sku = "Sku" + _nextSku;

            productData.Root.Add(new XElement("product", new XElement("sku", item.Sku), 
                                                         new XElement("name", item.Name),
                                                         new XElement("description", item.Description),
                                                         new XElement("price", item.Price)
                                              )
                                 );

            productData.Save(HttpContext.Current.Server.MapPath("~/App_Data/Products.xml"));
            allProducts.Add(item);
            return item;
        }

        public void Remove(string sku)
        {
            productData.Root.Elements("product").Where(i => ((string)i.Element("sku")).ToLower() == sku.ToLower()).Remove();
            productData.Save(HttpContext.Current.Server.MapPath("~/App_Data/Products.xml"));
            allProducts.Remove(Get(sku));
        }

        public bool Update(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Null item!");
            }
            
            XElement node = productData.Root.Elements("product").Where(i => ((string)i.Element("sku")).ToLower() == item.Sku.ToLower()).FirstOrDefault();
            if (node == null)
            {
                return false;
            }
            node.SetElementValue("name", item.Name);
            node.SetElementValue("description", item.Description);
            node.SetElementValue("price", item.Price);

            productData.Save(HttpContext.Current.Server.MapPath("~/App_Data/Products.xml"));
            allProducts.Remove(Get(item.Sku));
            allProducts.Add(item);
            return true;
        }
    }
}