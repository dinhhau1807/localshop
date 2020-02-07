using AutoMapper;
using localshop.Core.Common;
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

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IMapper mapper)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _mapper = mapper;
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
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated && !(
                User.IsInRole(RoleNames.Root) ||
                User.IsInRole(RoleNames.Administrator) ||
                User.IsInRole(RoleNames.Modifier)
                ))
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