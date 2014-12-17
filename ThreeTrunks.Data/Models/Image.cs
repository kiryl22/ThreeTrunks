namespace ThreeTrunks.Data.Models
{
    public class Image:BaseModel
    {
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string FolowUrl { get; set; }

        public bool IsCarouselImage { get; set; }

        public int CategoryId { get; set; }

        public ImageCategory Category { get; set; }

        public Image()
        {

        }

        public Image(string fileName, int categoryId, string title = "Новое изображние" )
        {
            FileName = fileName;
            CategoryId = categoryId;
            Title = title;
        }

    }
}
