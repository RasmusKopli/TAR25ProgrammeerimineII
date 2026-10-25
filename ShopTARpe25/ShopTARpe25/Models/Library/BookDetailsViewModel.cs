namespace ShopTARpe25.Models.Library
{
    public class BookDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateOnly PublishedYear { get; set; }
        public bool IsAvailable { get; set; }

        // Loan / Reader Properties (Only populated if Checked Out)
        public string? ReaderFullName { get; set; }
        public DateOnly? LoanDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
    }
}
