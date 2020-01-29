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

        public ProductController(IMapper mapper, IProductRepository productRepo)
        {
            _mapper = mapper;
            _productRepo = productRepo;
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
                Categories = null
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(AddProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_productRepo.FindBySku(productDTO.Sku) != null)
            {
                ModelState.AddModelError("", "SKU is used by another product.");
                return View();
            }

            var product = _mapper.Map<Product>(productDTO);
            _productRepo.Save(product);

            return RedirectToAction("index");
        }
    }
}