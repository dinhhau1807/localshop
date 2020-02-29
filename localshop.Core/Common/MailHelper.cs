﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace localshop.Core.Common
{
    public static class MailHelper
    {
        public static string CreateConfirmEmailBody(ControllerContext controllerContext, string callbackUrl)
        {
            string body;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(controllerContext.HttpContext.Server.MapPath("~/Content/ConfirmEmailTemplate.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{logo-link}", $"{controllerContext.HttpContext.Request.Url.Scheme}://{controllerContext.HttpContext.Request.Url.Authority}");
            body = body.Replace("{confirm-link}", callbackUrl);

            return body;
        }
    }
}