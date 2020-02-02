using AutoMapper;
using localshop.Core.Common;
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

        public IQueryable<Product> Products
        {
            get
            {
                return _context.Products.AsQueryable();
            }
        }

        public IEnumerable<string> GetImages(string id)
        {
            return _context.Images.Where(i => i.ProductId == id).Select(i => i.Path).ToList();
        }

        public Product FindById(string id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public Product FindBySku(string sku)
        {
            return _context.Products.FirstOrDefault(p => p.Sku == sku);
        }

        public bool SetStatus(string productId, string statusName)
        {
            var product = FindById(productId);
            var statusIds = _context.Statuses.Where(s => s.Name == statusName).Select(s => s.Id);

            if (product != null && statusIds.Count() > 0)
            {
                product.StatusId = statusIds.First();
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public Product Delete(string id)
        {
            var product = _context.Products.First(p => p.Id == id);
            _context.Images.RemoveRange(_context.Images.Where(i => i.ProductId == id));
            _context.Products.Remove(product);
            _context.SaveChanges();
            return product;
        }

        public void Save(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Id))
            {
                product.Id = NewId.Next().ToString();
                if (product.Images != null && product.Images.Count > 0)
                {
                    foreach (var image in product.Images)
                    {
                        image.ProductId = product.Id;
                    }
                }

                var metaTitle = product.Name.GenerateSlug();
                if (_context.Products.Any(p => p.MetaTitle == metaTitle))
                {
                    metaTitle += "-" + NewId.Next().ToString().Split('-').Last();
                }

                product.MetaTitle = metaTitle;
                product.DateAdded = DateTime.Now;

                _context.Products.Add(product);
                _context.Images.AddRange(product.Images);
            }
            else
            {
                var dbEntry = _context.Products.First(p => p.Id == product.Id);

                var dbEntryImages = GetImages(product.Id);
                var newImages = product.Images.ToList();

                dbEntry = _mapper.Map<Product>(product);
                dbEntry.Images = null;

                // Remove image
                foreach (var path in dbEntryImages)
                {
                    if (!newImages.Any(i => i.Path == path))
                    {
                        var image = _context.Images.First(i => i.ProductId == product.Id && i.Path == path);
                        _context.Images.Remove(image);
                        continue;
                    }
                }

                // Add image
                foreach (var newImg in newImages)
                {
                    if (!dbEntryImages.Any(path => path == newImg.Path))
                    {
                        var image = new Image { ProductId = dbEntry.Id, Path = newImg.Path };
                        _context.Images.Add(image);
                    }
                }

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
