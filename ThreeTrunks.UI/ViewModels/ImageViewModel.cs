using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ThreeTrunks.Data.Models;

namespace ThreeTrunks.UI.ViewModels
{
    public class ImageViewModel
    {
        private static readonly string ImagesFolder = ConfigurationManager.AppSettings["ImagesFolder"];
        private static readonly string ThumbnailImagesFolder = ConfigurationManager.AppSettings["ThumbnailImagesFolder"];

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("thumbnail")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty("isCarousel")]
        public bool IsCarousel { get; set; }

        [JsonProperty("followUrl")]
        public string FollowUrl { get; set; }

        public static List<ImageViewModel> MapList(IEnumerable<Image> images)
        {
            if (images == null)
                return new List<ImageViewModel>();

            return new List<ImageViewModel>(images.Select(Map));
        }

        public static ImageViewModel Map(Image image)
        {
            if (image == null)
                return new ImageViewModel();

            return new ImageViewModel()
                {
                    Id = image.Id,
                    CategoryId = image.CategoryId,
                    ThumbnailUrl = string.Format("{0}/{1}", ThumbnailImagesFolder, image.FileName),
                    Url = string.Format("{0}/{1}", ImagesFolder, image.FileName),
                    Title = image.Title,
                    Description = image.Description,
                    IsCarousel = image.IsCarouselImage,
                    FollowUrl = image.FolowUrl
                    
                };
        }

        public static Image Map(ImageViewModel image)
        {
            if (image == null)
                return new Image();

            return new Image()
            {
                Id = image.Id,
                CategoryId = image.CategoryId,
                FolowUrl = image.FollowUrl,
                Description = image.Description,
                IsCarouselImage = image.IsCarousel,
                Title = image.Title
            };
        }
    }
}