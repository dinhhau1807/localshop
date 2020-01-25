using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace localshop.Areas.Admin.Controllers
{
    public class NavController : BaseController
    {
        [ChildActionOnly]
        public PartialViewResult Topbar(string header)
        {
            return PartialView("_Topbar", header);
        }

        [ChildActionOnly]
        public PartialViewResult Sidebar()
        {
            return PartialView("_Sidebar");
        }

        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            return PartialView("_Footer");
        }
    }
}