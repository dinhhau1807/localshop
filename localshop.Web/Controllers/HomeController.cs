using localshop.Domain.Abstractions;
using localshop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace localshop.Controllers
{
    public class HomeController : Controller
    {
        private IHomePageRepository _homePageRepo;
        private IProductRepository _productRepo;
        private IStatusRepository _statusRepo;
        private ICategoryRepository _categoryRepo;

        public HomeController(IHomePageRepository homePageRepo, IProductRepository productRepo, IStatusRepository statusRepo, ICategoryRepository categoryRepo)
        {
            _homePageRepo = homePageRepo;
            _productRepo = productRepo;
            _statusRepo = statusRepo;
            _categoryRepo = categoryRepo;
        }

        public ActionResult Index()
        {
            // Prepare model
            var model = new HomePageViewModel
            {
                SpecialFeatured = _homePageRepo.SpecialFeatureds,
                Banners = _homePageRepo.Banners,
                Featureds = new List<ProductViewModel>(),
                OnSales = new List<ProductViewModel>()
            };

            // Get featureds
            var featureds = _productRepo.Products.Where(p => p.IsFeatured == true).Take(8).ToList();
            foreach (var p in featureds)
            {
                p.Images = _productRepo.GetImages(p.Id).ToList();

                var product = new ProductViewModel
                {
                    Product = p,
                    Status = _statusRepo.GetStatus(p.StatusId),
                    Category = _categoryRepo.GetCategory(p.CategoryId)
                };

                model.Featureds.Add(product);
            }

            // Get onSales
            var onSales = _productRepo.Products.Where(p =>
            {
                if (p.DiscountPrice != null)
                {
                    return _productRepo.GetRealPrice(p) == p.DiscountPrice.Value;
                }
                return false;
            }).Take(8).ToList();
            foreach (var p in onSales)
            {
                p.Images = _productRepo.GetImages(p.Id).ToList();

                var product = new ProductViewModel
                {
                    Product = p,
                    Status = _statusRepo.GetStatus(p.StatusId),
                    Category = _categoryRepo.GetCategory(p.CategoryId)
                };

                model.OnSales.Add(product);
            }

            return View(model);
        }
    }
}