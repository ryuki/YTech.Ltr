﻿using System.Web.Mvc;

namespace YTech.Ltr.Web.Controllers.Master
{
    [HandleError]
    public class AgenController : Controller
    {
        public ActionResult Index()
        {
            ViewData["CurrentItem"] = "Form Penjualan";
            return View();
        }
    }
}