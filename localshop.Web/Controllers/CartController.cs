using localshop.Core.Common;
using localshop.Domain.Abstractions;
using localshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace localshop.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository _productRepo;

        public CartController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public JsonResult GetCart(Cart cart)
        {
            return Json(new
            {
                success = true,
                cart = cart,
                summary = cart.Summary,
                summaryQuantity = cart.SummaryQuantity
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddToCart(Cart cart, string productId, int quantity = 1)
        {
            bool addNew = false;

            var product = _productRepo.FindById(productId);
            if (product != null)
            {
                CartLine line = cart.LineCollection.Where(p => p.Product.Id == product.Id).FirstOrDefault();

                if (line == null)
                {
                    line = new CartLine
                    {
                        Product = product,
                        Quantity = quantity
                    };
                    product.Images = _productRepo.GetImages(product.Id).ToList();
                    if (product.Images.Count == 0)
                    {
                        product.Images.Add(ImageLinks.BrokenProductImage);
                    }

                    cart.LineCollection.Add(line);
                    addNew = true;
                }
                else
                {
                    line.Quantity += quantity;
                }
            }

            return Json(new
            {
                success = true,
                addNew = addNew,
                cart = cart,
                summary = cart.Summary,
                summaryQuantity = cart.SummaryQuantity
            });
        }

        [HttpPost]
        public JsonResult RemoveFromCart(Cart cart, string productId)
        {
            var product = _productRepo.FindById(productId);
            if (product != null)
            {
                cart.LineCollection.RemoveAll(l => l.Product.Id == product.Id);
            }

            return Json(new
            {
                success = true,
                cart = cart,
                summary = cart.Summary,
                summaryQuantity = cart.SummaryQuantity
            });
        }

        [HttpPost]
        public JsonResult ClearAll(Cart cart)
        {
            cart.LineCollection.Clear();

            return Json(new
            {
                success = true,
                cart = cart,
                summary = cart.Summary,
                summaryQuantity = cart.SummaryQuantity
            });
        }
    }
}