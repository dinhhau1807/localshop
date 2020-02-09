using localshop.Core.Common;
using localshop.Domain.Abstractions;
using localshop.Models;
using localshop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace localshop.Controllers
{
    public class ShopController : Controller
    {
        private IProductRepository _productRepo;
        private ICategoryRepository _categoryRepo;
        private IStatusRepository _statusRepo;

        public ShopController(IProductRepository productRepo, ICategoryRepository categoryRepo, IStatusRepository statusRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _statusRepo = statusRepo;
        }

        public ActionResult Index(int? page, int? view, int? minPrie, int? maxPrice, SortByEnums? sortBy)
        {
            var products = _productRepo.Products.Where(p => p.IsActive == true).ToList();

            var model = new ShopViewModel
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page.GetValueOrDefault(1),
                    ItemsPerPage = view.GetValueOrDefault(20),
                    TotalItems = products.Count()
                },
                Products = new List<ProductViewModel>(),
                Categories = _categoryRepo.Categories.AsEnumerable(),
                Statuses = _statusRepo.Statuses.AsEnumerable(),

                FilteredResult = products.Count(),
                SortBy = sortBy.GetValueOrDefault(SortByEnums.Default) == SortByEnums.Default ? null : sortBy,
                View = view.GetValueOrDefault(20) == 20 ? null : view,

                PriceFilter = new PriceFilter()
            };

            switch (sortBy)
            {
                case SortByEnums.NameAZ:
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
                case SortByEnums.NameZA:
                    products = products.OrderByDescending(p => p.Name).ToList();
                    break;
                case SortByEnums.PriceLowToHigh:
                    products = products.OrderBy(p => p.DiscountPrice ?? p.Price).ToList();
                    break;
                case SortByEnums.PriceHightToLow:
                    products = products.OrderByDescending(p => p.DiscountPrice ?? p.Price).ToList();
                    break;
                case SortByEnums.Default:
                default:
                    break;
            }

            products = products.Skip((model.PagingInfo.CurrentPage - 1) * model.PagingInfo.ItemsPerPage).Take(model.PagingInfo.ItemsPerPage).ToList();

            if (products.Count > 0)
            {
                model.PriceFilter.MinPrice = products.Select(p => p.DiscountPrice ?? p.Price).Min();
                model.PriceFilter.MaxPrice = products.Select(p => p.DiscountPrice ?? p.Price).Max();
            }
            else
            {
                model.PriceFilter.MinPrice = 0;
                model.PriceFilter.MaxPrice = 1;
            }

            foreach (var p in products)
            {
                p.Images = _productRepo.GetImages(p.Id).ToList();

                var product = new ProductViewModel
                {
                    Product = p,
                    Status = _statusRepo.GetStatus(p.StatusId),
                    Category = _categoryRepo.GetCategory(p.CategoryId)
                };

                model.Products.Add(product);
            }

            return View(model);
        }
    }
}