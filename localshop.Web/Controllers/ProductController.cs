using localshop.Domain.Abstractions;
using localshop.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace localshop.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _productRepo;
        private ICategoryRepository _categoryRepo;
        private IStatusRepository _statusRepo;

        public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, IStatusRepository statusRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _statusRepo = statusRepo;
        }

        public ActionResult Detail(string metaTitle)
        {
            var product = _productRepo.FindByMetaTitle(metaTitle);
            if (product == null)
            {
                return RedirectToAction("NotFound");
            }

            var model = new ProductDetailViewModel
            {
                Product = product,
                Status = _statusRepo.GetStatus(product.StatusId),
                Category = _categoryRepo.GetCategory(product.CategoryId),
                Related = new List<ProductRelatedViewModel>()
            };
            product.Images = _productRepo.GetImages(product.Id).ToList();

            var relatedProduct = _productRepo.Products.Where(p => p.CategoryId == product.CategoryId && p.Id!= product.Id).ToList();
            foreach (var p in relatedProduct)
            {
                var related = new ProductRelatedViewModel
                {
                    Product = p,
                    Status = _statusRepo.GetStatus(product.StatusId),
                    Category = model.Category,
                };
                p.Images = _productRepo.GetImages(p.Id).ToList();
                model.Related.Add(related);
            }

            return View(model);
        }
    }
}