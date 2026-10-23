namespace ShopTARpe25.Models.Library
{
    public class BookIndexViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateOnly PublishedYear { get; set; }
        public bool IsAvailable { get; set; }
    }
}
