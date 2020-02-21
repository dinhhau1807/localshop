using AutoMapper;
using localshop.Core.DTO;
using localshop.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace localshop.Areas.Admin.Controllers
{
    public class HomePageController : BaseController
    {
        private IMapper _mapper;
        private IHomePageRepository _homePageRepo;

        public HomePageController(IMapper mapper, IHomePageRepository homePageRepo)
        {
            _mapper = mapper;
            _homePageRepo = homePageRepo;
        }

        [HttpGet]
        public ViewResult SpecialFeatured()
        {
            var model = _homePageRepo.SpecialFeatureds;
            if (model == null)
            {
                model = new SpecialFeaturedDTO();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SpecialFeatured(SpecialFeaturedDTO specialFeaturedDTO)
        {
            if (!ModelState.IsValid ||
                string.IsNullOrWhiteSpace(specialFeaturedDTO.BackgroundImage) ||
                string.IsNullOrWhiteSpace(specialFeaturedDTO.ProductImage))
            {
                TempData["SaveSpecialFeaturedSuccess"] = "false";
                TempData["SaveSpecialFeaturedSuccessMessage"] = "Something went wrong, you have to add background image and product image!";
                return View(specialFeaturedDTO);
            }

            var result = _homePageRepo.SaveSpecialFeatureds(specialFeaturedDTO);

            if (!result)
            {
                TempData["SaveSpecialFeaturedSuccess"] = "false";
                return View(specialFeaturedDTO);
            }

            TempData["SaveSpecialFeaturedSuccess"] = "true";
            return RedirectToAction("SpecialFeatured");
        }
    }
}