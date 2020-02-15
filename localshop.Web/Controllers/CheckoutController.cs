using localshop.Domain.Abstractions;
using localshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace localshop.Controllers
{
    public class CheckoutController : Controller
    {
        private IProductRepository _productRepo;

        public CheckoutController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public ActionResult Index(Cart cart)
        {
            if (cart.LineCollection.Count == 0)
            {
                TempData["EmptyMessage"] = "true";
                return RedirectToAction("index", "cart");
            }

            if (Request.IsAuthenticated)
            {

            }

            return View();
        }
    }
}