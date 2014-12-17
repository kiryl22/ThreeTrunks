using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using ThreeTrunks.Data.Models;
using ThreeTrunks.Data.Repositories;
using ThreeTrunks.Logic.Managers;
using ThreeTrunks.UI.Helpers;
using ThreeTrunks.UI.ViewModels;

namespace ThreeTrunks.UI.ApiControllers
{
    public class ImageController : ApiController
    {
        readonly UnitOfWork _unitOfWork = new UnitOfWork();

        private readonly ImageCategoryManager _imageCategoryManager;
        private readonly ImageManager _imageManager;

        private readonly string _imagesFolder = ConfigurationManager.AppSettings["ImagesFolder"];
        private readonly string _thumbnailImagesFolder = ConfigurationManager.AppSettings["ThumbnailImagesFolder"];

        public ImageController()
        {
            _imageCategoryManager = new ImageCategoryManager(_unitOfWork);
            _imageManager = new ImageManager(_unitOfWork);
        }

        [HttpGet]
        public HttpResponseMessage GetGalleryCategories()
        {
            var categories = _imageCategoryManager.GetGalleryCategories();
            var galleryCategories = ImageCategoryViewModel.MapList(categories);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(galleryCategories), Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetCategories()
        {
            var categories = ImageCategoryViewModel.MapList(_imageCategoryManager.GetCategories());

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(categories), Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetCarouselImages()
        {
            var images = _imageManager.GetCarouselImages();
            var carouselImages = ImageViewModel.MapList(images);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(carouselImages), Encoding.UTF8, "application/json");
            return response;
        }

        [HttpPost]
        [ActionName("GetCategoryImages")]
        public HttpResponseMessage GetCategoryImages([FromBody]string type)
        {
            var images = _imageManager.GetCategoryImages(type);
            var categoryImages = ImageViewModel.MapList(images);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(categoryImages), Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        [ActionName("GetCategoryImages")]
        public HttpResponseMessage GetCategoryImages(int id)
        {
            var images = _imageManager.GetCategoryImages(id);
            var categoryImages = ImageViewModel.MapList(images);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(categoryImages), Encoding.UTF8, "application/json");
            return response;
        }

        [HttpPost]
        [ActionName("SaveCategory")]
        public HttpResponseMessage SaveCategory([FromBody]ImageCategoryViewModel category)
        {
            var result = _imageCategoryManager.SaveCategory(ImageCategoryViewModel.Map(category));

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(ImageCategoryViewModel.Map(result)), Encoding.UTF8, "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("DeleteCategory")]
        public HttpResponseMessage DeleteCategory([FromBody]int id)
        {
            _imageCategoryManager.DeleteCategory(id);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            return response;
        }

        [HttpPost]
        [ActionName("SaveImage")]
        public HttpResponseMessage SaveImage([FromBody]ImageViewModel image)
        {
            var result = _imageManager.SaveImage(ImageViewModel.Map(image));

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(ImageViewModel.Map(result)), Encoding.UTF8, "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("DeleteImage")]
        public HttpResponseMessage DeleteImage([FromBody]ImageViewModel image)
        {
            _imageManager.Delete(image.Id);

            DeleteImageFile(image);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(image), Encoding.UTF8, "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("UploadImage")]
        public async Task<HttpResponseMessage> UploadImage(int id)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string fileSaveLocation = HttpContext.Current.Server.MapPath(_imagesFolder);
            string thumbnailLocation = HttpContext.Current.Server.MapPath(_thumbnailImagesFolder);
            var provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
            var files = new List<ImageViewModel>();

            try
            { // Read all contents of multipart message into CustomMultipartFormDataStreamProvider. 
                await Request.Content.ReadAsMultipartAsync(provider);
                int categoryId;
                if (!Int32.TryParse(provider.FormData["categoryId"], out categoryId) ||
                    !_imageCategoryManager.IsCategoryExist(categoryId))
                {
                    categoryId = _imageCategoryManager.GetBasketCategory().Id;
                }

                foreach (MultipartFileData file in provider.FileData)
                {
                    var imageFileName = Path.GetFileName(file.LocalFileName);
                    ImageProccesor.CreateThumbnail(file.LocalFileName, Path.Combine(thumbnailLocation, imageFileName), 290, 290);

                    var image = new Image(imageFileName, categoryId)
                        {
                            FilePath = _imagesFolder
                        };

                    _imageManager.SaveImage(image);

                    files.Add(ImageViewModel.Map(image));
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { CategoryId = categoryId, UploadedImages = files });
            }

            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        private void DeleteImageFile(ImageViewModel image)
        {
            var imagePath = HttpContext.Current.Server.MapPath(image.Url);

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

            var thumbnailPath = HttpContext.Current.Server.MapPath(image.ThumbnailUrl);

            if (File.Exists(thumbnailPath))
            {
                File.Delete(thumbnailPath);
            }
        }

        private void SaveThumbnail(string localFileName)
        {

        }

        public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            public CustomMultipartFormDataStreamProvider(string path)
                : base(path)
            {

            }

            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                var filename = headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                return string.Format("{0}{1}", Guid.NewGuid(), Path.GetExtension(filename));
            }
        }

    }
}