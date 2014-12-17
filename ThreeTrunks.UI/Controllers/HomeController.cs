using System.Web.Mvc;
using Newtonsoft.Json;
using ThreeTrunks.Data.Repositories;
using ThreeTrunks.Logic.Managers;
using ThreeTrunks.UI.Attributes;
using ThreeTrunks.UI.ViewModels;

namespace ThreeTrunks.UI.Controllers
{
    [IsPublic(KeyToCheck = "IsPublic", NotifyUrl = "/Error/Unavailable")]
    public class HomeController : BaseController
    {
        private UnitOfWork _unitOfWork;
        private ImageCategoryManager _imageCategoryManager;

        public HomeController()
        {
            _unitOfWork = new UnitOfWork();
            _imageCategoryManager = new ImageCategoryManager(_unitOfWork);
        }

        public ActionResult Index()
        {
            var categories = _imageCategoryManager.GetGalleryCategories();
            ViewBag.galleryCategories = JsonConvert.SerializeObject(ImageCategoryViewModel.MapList(categories));
            return View();
        }

        public ActionResult Home()
        {
            return PartialView();
        }

        public ActionResult Gallery()
        {
            return PartialView();
        }

        public ActionResult AboutUs()
        {
            return PartialView();
        }

        public ActionResult Contacts()
        {
            return PartialView();
        }

    }
}