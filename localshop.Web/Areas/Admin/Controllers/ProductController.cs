using AutoMapper;
using localshop.Areas.Admin.ViewModels;
using localshop.Core.DTO.Admin;
using localshop.Domain.Abstractions;
using localshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace localshop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private IMapper _mapper;
        private IProductRepository _productRepo;
        private ICategoryRepository _categoryRepo;
        private IStatusRepository _statusRepo;

        public ProductController(IMapper mapper, IProductRepository productRepo, ICategoryRepository categoryRepo, IStatusRepository statusRepo)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _statusRepo = statusRepo;
        }

        public ViewResult Index()
        {
            var model = _productRepo.Products
                .Include(p => p.Status)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .ToList();
            return View(model);
        }

        [HttpGet]
        public ViewResult Add()
        {
            var model = new AddProductViewModel
            {
                Product = new Product(),
                Categories = _categoryRepo.Categories.AsEnumerable(),
                Statuses = _statusRepo.Statuses.AsEnumerable()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                var model = new AddProductViewModel
                {
                    Product = _mapper.Map<Product>(productDTO),
                    Categories = _categoryRepo.Categories.AsEnumerable(),
                    Statuses = _statusRepo.Statuses.AsEnumerable()
                };
                return View(model);
            }

            if (_productRepo.FindBySku(productDTO.Sku) != null)
            {
                var model = new AddProductViewModel
                {
                    Product = _mapper.Map<Product>(productDTO),
                    Categories = _categoryRepo.Categories.AsEnumerable(),
                    Statuses = _statusRepo.Statuses.AsEnumerable()
                };
                ModelState.AddModelError("", "SKU is used by another product.");
                return View(model);
            }

            var product = _mapper.Map<Product>(productDTO);
            _productRepo.Save(product);

            TempData["Success"] = "Success";
            return RedirectToAction("add");
        }

        [HttpPost]
        public JsonResult Delete(string productId)
        {
            var product = _productRepo.FindById(productId);
            if (product != null)
            {
                _productRepo.Delete(productId);

                return Json(new
                {
                    success = true
                });
            }

            return Json(new
            {
                success = false
            });
        }
    }
}