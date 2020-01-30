using AutoMapper;
using localshop.Core.Common;
using localshop.Domain.Abstractions;
using localshop.Domain.Entities;
using MassTransit;
using System;
using System.Collections.Generic;
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
            return new List<string>();
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
            _context.Products.Remove(product);
            _context.SaveChanges();
            return product;
        }

        public void Save(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Id))
            {
                product.Id = NewId.Next().ToString();

                var metaTitle = product.Name.GenerateSlug();
                if (_context.Products.Any(p => p.MetaTitle == metaTitle))
                {
                    metaTitle += "-" + NewId.Next().ToString().Split('-').Last();
                }

                product.MetaTitle = metaTitle;
                product.DateAdded = DateTime.Now;
                _context.Products.Add(product);
            }
            else
            {
                var dbEntry = _context.Products.First(p => p.Id == product.Id);
                dbEntry = _mapper.Map<Product>(product);
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
