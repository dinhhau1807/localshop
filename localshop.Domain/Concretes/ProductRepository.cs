using AutoMapper;
using localshop.Core.Common;
using localshop.Core.DTO;
using localshop.Domain.Abstractions;
using localshop.Domain.Entities;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localshop.Domain.Concretes
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<ProductDTO> Products
        {
            get
            {
                return _context.Products.AsEnumerable().Select(p => _mapper.Map<Product, ProductDTO>(p));
            }
        }

        public IEnumerable<string> GetImages(string id)
        {
            return _context.Images.Where(i => i.ProductId == id).Select(i => i.Path).ToList();
        }

        public ProductDTO FindById(string id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            return _mapper.Map<Product, ProductDTO>(product);
        }

        public ProductDTO FindBySku(string sku)
        {
            var product = _context.Products.FirstOrDefault(p => p.Sku == sku);
            return _mapper.Map<Product, ProductDTO>(product);
        }

        public bool SetStatus(string productId, string statusName)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            var statusIds = _context.Statuses.Where(s => s.Name == statusName).Select(s => s.Id);

            if (product != null && statusIds.Count() > 0)
            {
                product.StatusId = statusIds.First();
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public ProductDTO Delete(string id)
        {
            var product = _context.Products.First(p => p.Id == id);

            if (product == null)
            {
                return null;
            }

            _context.Images.RemoveRange(_context.Images.Where(i => i.ProductId == id));
            _context.Products.Remove(product);
            _context.SaveChanges();

            return _mapper.Map<Product, ProductDTO>(product);
        }

        public void Save(ProductDTO productDTO)
        {
            if (string.IsNullOrWhiteSpace(productDTO.Id))
            {
                productDTO.Id = NewId.Next().ToString();
                var productImages = new List<Image>();

                if (productDTO.Images != null && productDTO.Images.Count > 0)
                {
                    foreach (var image in productDTO.Images)
                    {
                        productImages.Add(new Image
                        {
                            ProductId = productDTO.Id,
                            Path = image
                        });
                    }
                }

                var metaTitle = productDTO.Name.GenerateSlug();
                if (_context.Products.Any(p => p.MetaTitle == metaTitle))
                {
                    metaTitle += "-" + NewId.Next().ToString().Split('-').Last();
                }

                productDTO.MetaTitle = metaTitle;
                productDTO.DateAdded = DateTime.Now;

                var product = _mapper.Map<ProductDTO, Product>(productDTO);

                _context.Products.Add(product);
                _context.Images.AddRange(productImages);
            }
            else
            {
                var dbEntry = _context.Products.First(p => p.Id == productDTO.Id);

                var dbEntryImages = GetImages(productDTO.Id);
                var newImages = productDTO.Images.ToList();

                // Update metatitle
                if (dbEntry.Name != productDTO.Name)
                {
                    var metaTitle = productDTO.Name.GenerateSlug();
                    if (_context.Products.Any(p => (p.MetaTitle != dbEntry.MetaTitle) && (p.MetaTitle == metaTitle)))
                    {
                        metaTitle += "-" + NewId.Next().ToString().Split('-').Last();
                    }

                    productDTO.MetaTitle = metaTitle;
                }

                // Remove image
                foreach (var path in dbEntryImages)
                {
                    if (!newImages.Any(i => i == path))
                    {
                        var image = _context.Images.First(i => i.ProductId == productDTO.Id && i.Path == path);
                        _context.Images.Remove(image);
                        continue;
                    }
                }

                // Add image
                foreach (var newImg in newImages)
                {
                    if (!dbEntryImages.Any(path => path == newImg))
                    {
                        var image = new Image { ProductId = dbEntry.Id, Path = newImg };
                        _context.Images.Add(image);
                    }
                }

                dbEntry = _mapper.Map(productDTO, dbEntry);
                dbEntry.DateModified = DateTime.Now;
            }
            _context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                        _context = null;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
