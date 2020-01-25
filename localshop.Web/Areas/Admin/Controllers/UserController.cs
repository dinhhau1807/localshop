using localshop.Areas.Admin.ViewModels;
using localshop.Core.Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace localshop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private ApplicationUserManager _userManager;

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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

        // GET: Admin/User
        public ActionResult Index()
        {
            var model = new List<ListUserViewModel>();

            foreach (var user in UserManager.Users.ToList())
            {
                var roles = UserManager.GetRoles(user.Id);

                if (roles.Any(r => r.Contains(RoleNames.Root)))
                {
                    continue;
                }

                var u = new ListUserViewModel
                {
                    User = user,
                    Roles = roles
                };

                model.Add(u);
            }

            return View(model);
        }
    }
}