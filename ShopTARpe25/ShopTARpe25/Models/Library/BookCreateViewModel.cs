namespace ShopTARpe25.Models.Library
{
    public class BookCreateViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateOnly PublishedYear { get; set; } = new DateOnly(2000, 1, 1);
        public bool IsAvailable { get; set; } = true;
    }
}
