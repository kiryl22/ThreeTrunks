using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThreeTrunks.Data;
using ThreeTrunks.UI.ViewModels;

namespace ThreeTrunks.UI.Controllers
{
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            if (SecurityProvider.IsAuthenticated)
                return View();

            return RedirectToAction("LogOn");
        }

        public ActionResult Images()
        {
            return PartialView();
        }

        public ActionResult Content()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult LogOn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOn(UserViewModel model, string returnUrl)
        {
            //ToDo: Add remember me field to model (persistCookie)
            if (ModelState.IsValid && SecurityProvider.Login(model.Username, model.Password, persistCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "Имя пользователя или пароль неверны");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            SecurityProvider.Logout();
            return RedirectToAction("Index");
        }

        public ActionResult EditCategory()
        {
            return PartialView();
        }

        public ActionResult EditImage()
        {
            return PartialView();
        }

        public ActionResult TextEditor()
        {
            return PartialView("../Shared/Admin/_TextEditor");
        }

        public ActionResult BoolEditor()
        {
            return PartialView("../Shared/Admin/_BoolEditor");
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        #endregion
    }
}