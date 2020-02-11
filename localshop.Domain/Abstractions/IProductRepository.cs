using localshop.Core.DTO;
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
        IEnumerable<ProductDTO> Products { get; }

        IEnumerable<string> GetImages(string id);

        ProductDTO FindById(string id);

        ProductDTO FindBySku(string sku);

        ProductDTO FindByMetaTitle(string metaTitle);

        void Save(ProductDTO product);

        ProductDTO Delete(string id);
    }
}
