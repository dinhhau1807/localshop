using localshop.Domain.Abstractions;
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
        private IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var model = _repository.Products
                .Include(p => p.Status)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .ToList();
            return View(model);
        }
    }
}