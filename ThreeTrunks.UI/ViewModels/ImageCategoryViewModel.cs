using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ThreeTrunks.Data.Models;
using ThreeTrunks.Logic.Managers;

namespace ThreeTrunks.UI.ViewModels
{
    public class ImageCategoryViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("galleryLink")]
        public string Link { get; set; }

        [JsonProperty("isBasket")]
        public bool IsBasket { get; set; }

        [JsonProperty("isGallery")]
        public bool IsGallery { get; set; }

        [JsonProperty("images")]
        public List<ImageViewModel> Images { get; set; }

        public static List<ImageCategoryViewModel> MapList(IEnumerable<ImageCategory> categories)
        {
            var result = new List<ImageCategoryViewModel>();

            if (categories != null)
            {
                result.AddRange(categories.Select(Map));
            }
            return result;
        }

        public static ImageCategoryViewModel Map(ImageCategory category)
        {
            if (category == null)
                return new ImageCategoryViewModel();

            return new ImageCategoryViewModel()
                {
                    Id = category.Id,
                    Description = category.Description,
                    Name = category.Name,
                    IsGallery = category.IsGallery,
                    Link = category.CategoryUrl,
                    IsBasket = category.IsBasket,
                    Images = category.Images != null ? category.Images.Select(ImageViewModel.Map).ToList() : null
                };

        }

        public static ImageCategory Map(ImageCategoryViewModel category)
        {
            if (category == null)
                return new ImageCategory();

            return new ImageCategory()
            {
                Id = category.Id,
                Description = category.Description,
                Name = category.Name,
                IsGallery = category.IsGallery,
                CategoryUrl = category.Link,
            };

        }
    }
}