using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using ThreeTrunks.Data.Models;
using ThreeTrunks.Data.Repositories;

namespace ThreeTrunks.Logic.Managers
{
    public class ImageManager
    {
        private readonly UnitOfWork _unitOfWork;

        public ImageManager(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Image> GetCategoryImages(string followLink)
        {
            return _unitOfWork.ImageRepository.Get(e => e.Category.CategoryUrl == followLink);
        }

        public IEnumerable<Image> GetCategoryImages(int categoryId)
        {
            return _unitOfWork.ImageRepository.Get(e => e.Category.Id == categoryId);
        }

        public IEnumerable<Image> GetCarouselImages()
        {
            return _unitOfWork.ImageRepository.Get(e => e.IsCarouselImage);
        }

        public Image SaveImage(Image image)
        {
            if (image.Id > 0)
            {
                var dbimage = _unitOfWork.ImageRepository.GetByID(image.Id);
                if (dbimage != null)
                {
                    dbimage.CategoryId = image.CategoryId;
                    dbimage.Description = image.Description;
                    dbimage.FolowUrl = image.FolowUrl;
                    dbimage.IsCarouselImage = image.IsCarouselImage;
                    dbimage.Title = image.Title;

                    _unitOfWork.ImageRepository.Update(dbimage);
                }
            }
            else
            {
                _unitOfWork.ImageRepository.Insert(image);
            }

            _unitOfWork.Save();

            return image;
        }

        public void Delete(int imageId)
        {
            var image = _unitOfWork.ImageRepository.GetByID(imageId);
            if (image != null)
            {
                _unitOfWork.ImageRepository.Delete(image);
                _unitOfWork.Save();
            }

        }

        private string VerifyFormat(string sourcePath)
        {
            var imageFormat = string.Empty;
            if (File.Exists(sourcePath))
            {

                System.Drawing.Image imgInput = System.Drawing.Image.FromFile(sourcePath);
                var thisFormat = imgInput.RawFormat;

                //check only for jpeg, png and gif

                if (ImageFormat.Jpeg.Equals(thisFormat))
                {
                    imageFormat = ImageFormat.Jpeg.ToString();
                }
                else if (ImageFormat.Png.Equals(thisFormat))
                {
                    imageFormat = ImageFormat.Png.ToString();
                }
                else if (ImageFormat.Gif.Equals(thisFormat))
                {
                    imageFormat = ImageFormat.Gif.ToString();
                }
            }

            return imageFormat;
        }
    }
}
