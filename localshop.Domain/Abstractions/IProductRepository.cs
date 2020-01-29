using localshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localshop.Domain.Abstractions
{
    public interface IProductRepository : IDisposable
    {
        IQueryable<Product> Products { get; }

        IEnumerable<string> GetImages(string id);

        Product FindById(string id);

        Product FindBySku(string sku);

        void Save(Product product);

        Product Delete(string id);
    }
}
