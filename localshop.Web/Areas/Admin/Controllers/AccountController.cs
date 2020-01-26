using localshop.Areas.Admin.ViewModels;
using localshop.Core.Common;
using localshop.Core.DTO.Admin;
using localshop.Infrastructures.Attributes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace localshop.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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
        public new ViewResult Profile()
        {
            var model = UserManager.FindById(User.Identity.GetUserId());
            return View(model);
        }

        [HttpGet]
        public ViewResult UpdateProfile()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var userDto = new UpdateProfileDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Country = user.Country,
                City = user.City,
                State = user.State,
                Zip = user.Zip,
                Address1 = user.Address1,
                Address2 = user.Address2,
                Image = user.Image
            };

            return View(userDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(UpdateProfileDTO userDto, HttpPostedFileBase image)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            if (!ModelState.IsValid)
            {
                userDto.Image = user.Image;
                return View(userDto);
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Country = userDto.Country;
            user.City = userDto.City;
            user.State = userDto.State;
            user.Zip = userDto.Zip;
            user.Address1 = userDto.Address1;
            user.Address2 = userDto.Address2;

            if (image != null)
            {
                userDto.Image = Path.Combine("/Assets/images/useravatars", Path.GetFileName(image.FileName));
                string path = Path.Combine(Server.MapPath("~/Assets/images/useravatars"), Path.GetFileName(image.FileName));
                image.SaveAs(path);
            }
            else
            {
                userDto.Image = user.Image;
            }

            user.Image = userDto.Image;

            UserManager.Update(user);

            return RedirectToAction("profile");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated && (
                User.IsInRole(RoleNames.Root) ||
                User.IsInRole(RoleNames.Administrator) ||
                User.IsInRole(RoleNames.Modifier)
                ))
            {
                return RedirectToLocal(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                //return View("LockedOut");
                // Not implemented
                case SignInStatus.RequiresVerification:
                //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                // Not implemented
                case SignInStatus.Failure:
                // Not implemented
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("index", "dashboard");
        }


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

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("index", "dashboard");
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