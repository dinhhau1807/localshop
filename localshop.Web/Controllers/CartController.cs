﻿using localshop.Core.Common;
using localshop.Domain.Abstractions;
using localshop.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
        public ViewResult Index(Cart cart)
        {
            return View(cart);
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
            var warningMessage = "";

            if (quantity < 0)
            {
                quantity = 1;
            }

            var product = _productRepo.FindById(productId);
            if (product != null)
            {
                CartLine line = cart.LineCollection.Where(p => p.Product.Id == product.Id).FirstOrDefault();

                if (product.Quantity != 0)
                {
                    if (quantity != 0)
                    {
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

                    if (line.Quantity > product.Quantity)
                    {
                        line.Quantity = product.Quantity;
                        warningMessage = "Some product is out of stock, so you can only set max quantity we have in stock!";
                    }
                }
                else
                {
                    warningMessage = "Some product is out of stock, so you can only set max quantity we have in stock!";
                }
            }

            return Json(new
            {
                success = true,
                addNew = addNew,
                warningMessage = warningMessage,
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
        public ActionResult RemoveProduct(Cart cart, string productId)
        {
            var product = _productRepo.FindById(productId);
            if (product != null)
            {
                cart.LineCollection.RemoveAll(l => l.Product.Id == product.Id);
            }

            return RedirectToAction("index");
        }

        [HttpPost]
        public JsonResult UpdateCart(Cart cart, string model)
        {
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            Line[] lines = json_serializer.Deserialize<Line[]>(model);

            foreach (var productLine in lines)
            {
                var line = cart.LineCollection.FirstOrDefault(l => l.Product.Id == productLine.Id);
                if (line != null)
                {
                    if (productLine.Quantity <= 0)
                    {
                        productLine.Quantity = 1;
                    }

                    var currentQuantity = _productRepo.FindById(line.Product.Id).Quantity;
                    if (productLine.Quantity > currentQuantity)
                    {
                        TempData["OutOfStock"] = "true";
                        if (currentQuantity == 0)
                        {
                            cart.LineCollection.Remove(line);
                        }
                        else
                        {
                            line.Quantity = currentQuantity;
                        }
                    }
                    else
                    {
                        line.Quantity = productLine.Quantity;
                    }
                }
            }

            return Json(new
            {
                success = true,
            });
        }

        [HttpPost]
        public ActionResult ClearAll(Cart cart)
        {
            cart.LineCollection.Clear();
            return RedirectToAction("index");
        }
    }
}