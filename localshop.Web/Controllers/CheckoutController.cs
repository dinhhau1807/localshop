using AutoMapper;
using localshop.Core.DTO;
using localshop.Domain.Abstractions;
using localshop.Domain.Entities;
using localshop.Models;
using localshop.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace localshop.Controllers
{
    public class CheckoutController : Controller
    {
        private IMapper _mapper;
        private ApplicationUserManager _userManager;
        private IProductRepository _productRepo;
        private IOrderRepository _orderRepo;

        public CheckoutController(IMapper mapper, ApplicationUserManager userManager, IProductRepository productRepo, IOrderRepository orderRepo)
        {
            _mapper = mapper;
            _userManager = userManager;
            _productRepo = productRepo;
            _orderRepo = orderRepo;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public ActionResult Index(Cart cart)
        {
            if (cart.LineCollection.Count == 0)
            {
                TempData["EmptyMessage"] = "true";
                return RedirectToAction("index", "cart");
            }

            var model = new CheckoutViewModel();

            // If user logged in => Get address infomation
            if (Request.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                model.Order = _mapper.Map<ApplicationUser, OrderDTO>(user);
            }
            else
            {
                model.Order = new OrderDTO();
            }

            // Get order details from cart
            model.OrderDetails = GetOrderDetails(cart);

            model.Order.SubTotal = cart.Summary;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(Cart cart, OrderDTO order)
        {
            if (!ModelState.IsValid)
            {
                var model = new CheckoutViewModel()
                {
                    Order = order,
                    OrderDetails = GetOrderDetails(cart)
                };
                return View("index", model);
            }

            if (User.Identity.IsAuthenticated)
            {
                order.UserId = User.Identity.GetUserId();
            }

            var orderDetails = GetOrderDetails(cart, false);
            order.SubTotal = cart.Summary;

            var result = _orderRepo.Save(order, orderDetails);

            if (result)
            {
                TempData["OrderSuccess"] = "true";
                cart.LineCollection.Clear();
            }
            else
            {
                TempData["OrderSuccess"] = "false";
            }

            return RedirectToAction("index", "cart");
        }

        public List<OrderDetailDTO> GetOrderDetails(Cart cart, bool turnOnNotification = true)
        {
            var orderDetails = new List<OrderDetailDTO>();

            foreach (var line in cart.LineCollection)
            {
                var orderDetail = _mapper.Map<ProductDTO, OrderDetailDTO>(line.Product);
                var product = _productRepo.FindById(line.Product.Id);
                orderDetail.Price = product.DiscountPrice ?? product.Price;

                if (line.Quantity > product.Quantity)
                {
                    if (turnOnNotification)
                    {
                        TempData["OutOfStock"] = "true";
                    }
                    line.Quantity = product.Quantity;
                }

                orderDetail.Quantity = line.Quantity;
                orderDetail.SubTotal = orderDetail.Price * orderDetail.Quantity;

                orderDetails.Add(orderDetail);
            }

            return orderDetails;
        }

    }
}