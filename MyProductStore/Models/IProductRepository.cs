using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProductStore.Models
{
    interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product Get(string sku);
        Product Add(Product item);
        void Remove(string sku);
        bool Update(Product item);
    }
}
