using localshop.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace localshop.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        private IContactRepository _contactRepo;

        public ContactController(IContactRepository contactRepo)
        {
            _contactRepo = contactRepo;
        }

        public ActionResult Index()
        {
            var contacts = _contactRepo.Contacts.OrderBy(c => c.IsRead).ThenByDescending(c => c.Id).ToList();
            return View(contacts);
        }

        [HttpGet]
        public JsonResult GetContact(int contactId)
        {
            var contact = _contactRepo.Contacts.FirstOrDefault(c => c.Id == contactId);
            if (contact==null)
            {
                return Json(new
                {
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }

            _contactRepo.SetRead(contactId);

            return Json(new
            {
                success = true,
                contact = contact
            }, JsonRequestBehavior.AllowGet);
        }
    }
}