using AutoMapper;
using localshop.Core.Common;
using localshop.Core.DTO;
using localshop.Domain.Abstractions;
using localshop.Domain.Entities;
using localshop.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace localshop.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IMapper _mapper;
        private IOrderRepository _orderRepo;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IMapper mapper, IOrderRepository orderRepo)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _mapper = mapper;
            _orderRepo = orderRepo;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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
        public ViewResult Index()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return View(model: user.FullName);
        }

        [HttpGet]
        public ViewResult Orders()
        {
            var model = new List<OrderViewModel>();

            var orders = _orderRepo.GetOrders(User.Identity.GetUserId()).OrderByDescending(o => o.OrderDate).ToList();
            foreach (var o in orders)
            {
                var order = new OrderViewModel
                {
                    Order = o,
                    OrderStatus = _orderRepo.GetOrderStatus(o.OrderStatusId)
                };
                model.Add(order);
            }

            return View(model);
        }

        [HttpGet]
        public ViewResult Info()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return View(user);
        }

        [HttpGet]
        public ViewResult UpdateInfo()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var model = _mapper.Map<ApplicationUser, UpdateProfileDTO>(user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInfo(UpdateProfileDTO updateProfileDTO)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            if (!ModelState.IsValid)
            {
                return View(updateProfileDTO);
            }

            var image = user.Image;
            user = _mapper.Map(updateProfileDTO, user);
            user.Image = image;

            UserManager.Update(user);

            TempData["SaveSuccess"] = "true";
            return RedirectToAction("info");
        }

        [HttpGet]
        public ViewResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["SaveSuccess"] = "false";
                TempData["ErrorMessage"] = "Something went wrong!";
                return View(changePasswordViewModel);
            }

            var result = UserManager.ChangePassword(User.Identity.GetUserId(),
                                                    changePasswordViewModel.CurrentPassword,
                                                    changePasswordViewModel.NewPassword);

            if (!result.Succeeded)
            {
                TempData["SaveSuccess"] = "false";
                TempData["ErrorMessage"] = result.Errors.First().ToString();
                return View(changePasswordViewModel);
            }

            TempData["SaveSuccess"] = "true";
            return RedirectToAction("changepassword");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }

            var model = new LoginRegisterViewModel
            {
                Login = new LoginViewModel(),
                Register = new RegisterViewModel()
            };

            ViewBag.ReturnUrl = returnUrl;
            return View("LoginRegister", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            var model = new LoginRegisterViewModel
            {
                Login = loginViewModel,
                Register = new RegisterViewModel()
            };
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View("LoginRegister", model);
            }

            var result = await SignInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                // return View("LockedOut");
                // Not implemented
                case SignInStatus.RequiresVerification:
                // return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                // Not implemented
                case SignInStatus.Failure:
                // Not implemented
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    TempData["ErrorMessage"] = "Invalid login attempt.";
                    return View("LoginRegister", model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            TempData["ActiveRegister"] = "active";
            return RedirectToAction("login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel registerViewModel, string returnUrl)
        {
            var model = new LoginRegisterViewModel
            {
                Login = new LoginViewModel(),
                Register = registerViewModel
            };
            ViewBag.ReturnUrl = returnUrl;
            TempData["ActiveRegister"] = "active";

            if (!ModelState.IsValid)
            {
                return View("LoginRegister", model);
            }

            var user = new ApplicationUser { UserName = registerViewModel.RegisterEmail, Email = registerViewModel.RegisterEmail };
            var result = await UserManager.CreateAsync(user, registerViewModel.RegisterPassword);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                await UserManager.AddToRoleAsync(user.Id, RoleNames.Customer);

                return RedirectToLocal(returnUrl);
            }

            AddErrors(result);
            return View("LoginRegister", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("index", "home");
        }



        #region Helpers
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("index", "home");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        #endregion
    }
}