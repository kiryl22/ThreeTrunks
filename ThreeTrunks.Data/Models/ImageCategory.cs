using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThreeTrunks.Data.Models
{
    public class ImageCategory : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsGallery { get; set; }

        public bool IsBasket { get; set; }

        public bool IsDeleted { get; set; }

        public string CategoryUrl { get; set; }

        public virtual List<Image> Images { get; set; }

    }
}
