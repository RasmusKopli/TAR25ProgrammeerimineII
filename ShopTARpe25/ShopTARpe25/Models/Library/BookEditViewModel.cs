using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShopTARpe25.Models.Library
{
    public class BookEditViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; }

        public Guid? SelectedReaderId { get; set; }
        public IEnumerable<SelectListItem> ReadersList { get; set; } = new List<SelectListItem>();
    }
}
