﻿using localshop.Core.Common;
using localshop.Domain.Abstractions;
using localshop.Models;
using localshop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Index(ProductFilter filter)
        {
            // Set null with defautl filter
            filter.View = filter.View.GetValueOrDefault(20) == 20 ? null : filter.View;
            filter.SortBy = filter.SortBy.GetValueOrDefault(SortByEnums.Default) == SortByEnums.Default ? null : filter.SortBy;

            // Get all active product
            var products = _productRepo.Products.ToList();

            // Filter
            // -- Search
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                products = products.Where(p => p.Name.ToLower().ToUnsigned().Contains(filter.Search.ToLower().ToUnsigned())).ToList();
            }

            // -- Category
            if (!string.IsNullOrWhiteSpace(filter.Category))
            {
                var categoryId = _categoryRepo.GetId(filter.Category);
                products = products.Where(p => p.CategoryId == categoryId).ToList();
            }

            // -- Price
            if (products.Count > 0)
            {
                filter.PriceFilter.MinPrice = Math.Floor(products.Min(p => p.DiscountPrice ?? p.Price));
                filter.PriceFilter.MaxPrice = Math.Ceiling(products.Max(p => p.DiscountPrice ?? p.Price));
            }
            else
            {
                filter.PriceFilter.MinPrice = 0;
                filter.PriceFilter.MaxPrice = 1;
            }
            if (filter.MinPrice != null && filter.MaxPrice != null)
            {
                products = products.Where(p => ((p.DiscountPrice ?? p.Price) >= filter.MinPrice.Value) && ((p.DiscountPrice ?? p.Price) <= filter.MaxPrice.Value)).ToList();
            }

            // -- Sort by
            switch (filter.SortBy)
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

            // Get result
            filter.FilteredResult = products.Count;

            var model = new ShopViewModel
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = filter.Page.GetValueOrDefault(1),
                    ItemsPerPage = filter.View.GetValueOrDefault(20),
                    TotalItems = products.Count()
                },
                Products = new List<ProductViewModel>(),
                Categories = _categoryRepo.Categories.AsEnumerable(),
                Statuses = _statusRepo.Statuses.AsEnumerable(),
                Filter = filter
            };

            products = products.Skip((model.PagingInfo.CurrentPage - 1) * model.PagingInfo.ItemsPerPage).Take(model.PagingInfo.ItemsPerPage).ToList();

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