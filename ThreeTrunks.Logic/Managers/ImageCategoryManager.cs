using System.Collections.Generic;
using ThreeTrunks.Data.Models;
using ThreeTrunks.Data.Repositories;

namespace ThreeTrunks.Logic.Managers
{
    public class ImageCategoryManager
    {
        private readonly UnitOfWork _unitOfWork;

        public ImageCategoryManager(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ImageCategory> GetCategories()
        {
            return _unitOfWork.ImageCategoryRepository.GetCategories();
        }

        public IEnumerable<ImageCategory> GetGalleryCategories()
        {
            return _unitOfWork.ImageCategoryRepository.GetCategories(e => e.IsGallery);
        }

        public ImageCategory GetBasketCategory()
        {
            return _unitOfWork.ImageCategoryRepository.GetBasket();
        }

        public bool IsCategoryExist(int id)
        {
            return _unitOfWork.ImageCategoryRepository.GetByID(id) != null;
        }

        public ImageCategory SaveCategory(ImageCategory category)
        {
            if (category.Id > 0)
            {

                var dbcategory = _unitOfWork.ImageCategoryRepository.GetByID(category.Id);
                if (dbcategory != null)
                {
                    dbcategory.Description = category.Description;
                    dbcategory.IsDeleted = category.IsDeleted;
                    dbcategory.Name = category.Name;
                    dbcategory.IsGallery = category.IsGallery;
                    dbcategory.CategoryUrl = category.CategoryUrl;

                    _unitOfWork.ImageCategoryRepository.Update(dbcategory);
                }
            }
            else
            {
                _unitOfWork.ImageCategoryRepository.Insert(category);
            }

            _unitOfWork.Save();

            return _unitOfWork.ImageCategoryRepository.GetByID(category.Id);
        }

        public void DeleteCategory(int id)
        {
            var category = _unitOfWork.ImageCategoryRepository.GetByID(id);
            var basket = _unitOfWork.ImageCategoryRepository.GetBasket();

            if (category != null)
            {
                category.IsDeleted = true;

                var images = category.Images;

                var imageManager = new ImageManager(_unitOfWork);

                images.ForEach(i =>
                {
                    i.CategoryId = basket.Id;
                    imageManager.SaveImage(i);
                });

                _unitOfWork.ImageCategoryRepository.Update(category);

            }
        }

    }
}
