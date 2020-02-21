using localshop.Domain.Abstractions;
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

        public HomeController(IHomePageRepository homePageRepo)
        {
            _homePageRepo = homePageRepo;
        }

        public ActionResult Index()
        {
            var specialFeatured = _homePageRepo.SpecialFeatureds;

            return View(specialFeatured);
        }
    }
}