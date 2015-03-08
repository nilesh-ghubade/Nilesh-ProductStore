using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProductStore;
using MyProductStore.Controllers;
using MyProductStore.Models;

namespace MyProductStore.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTest
    {
        [TestMethod]
        public void Get()
        {
            try
            {
                // Arrange
                ProductsController controller = new ProductsController("TDD");

                // Act
                IEnumerable<Product> result = controller.GetAllProducts();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count());
                Assert.AreEqual("Sku1", ((Product)result.ElementAt(0)).Sku);
                Assert.AreEqual("Sku2", ((Product)result.ElementAt(1)).Sku);
            }
            catch (Exception e)
            {
                Console.WriteLine("Get exception: " + e);
            }
        }

        [TestMethod]
        public void GetBySku()
        {
            try
            {
                // Arrange
                ProductsController controller = new ProductsController("TDD");

                // Act
                Product result = controller.GetProduct("Sku1");

                // Assert
                Assert.AreEqual("Sku1", result.Sku);
            }
            catch (Exception e)
            {
                Console.WriteLine("GetBySku exception: " + e);
            }
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            try
            {
                ProductsController controller = new ProductsController("TDD");
                Product p = new Product { Sku = "Sku3", Name = "TestNew3", Description = "Test New Description3", Price = 3.3M };
                // Act
                HttpResponseMessage result = controller.PostProduct(p);

                // Assert
                Assert.AreEqual("201", result.StatusCode);
            }
            catch(Exception e)
            {
                Console.WriteLine("Post exception: " + e);
            }
        }

        [TestMethod]
        public void Put()
        {
            try
            {
                // Arrange
                ProductsController controller = new ProductsController("TDD");
                Product p = new Product { Sku = "Sku1", Name = "TestUpdate1", Description = "Test Update Description1", Price = 1.1M };

                // Act
                HttpResponseMessage result = controller.PutProduct("Sku1", p);

                // Assert
                Assert.AreEqual("200", result.StatusCode);
            }
            catch (Exception e)
            {
                Console.WriteLine("Put exception: " + e);
            }
        }

        [TestMethod]
        public void Delete()
        {
            try
            {
                // Arrange
                ProductsController controller = new ProductsController("TDD");

                // Act
                HttpResponseMessage result = controller.DeleteProduct("Sku1");

                // Assert
                Assert.AreEqual("200", result.StatusCode);
            }
            catch (Exception e)
            {
                Console.WriteLine("Delete exception: " + e);
            }
        }
    }
}
