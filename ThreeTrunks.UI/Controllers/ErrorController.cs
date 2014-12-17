using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThreeTrunks.Data.Repositories;
using ThreeTrunks.Logic.Managers;

namespace ThreeTrunks.UI.Controllers
{
    public class ErrorController : Controller
    {
        
        private UnitOfWork _unitOfWork;
        private SettingsManager _settingsManager;

        public ErrorController()
        {
            _unitOfWork = new UnitOfWork();
            _settingsManager = new SettingsManager(_unitOfWork);
        }

        public ActionResult Index()
        {
            return View("GenericError");
        }

        public ActionResult Unavailable()
        {
            var message = _settingsManager.GetValue("MessageUnavailable");
            ViewBag.Message = message;
            return View();
        }
	}
}