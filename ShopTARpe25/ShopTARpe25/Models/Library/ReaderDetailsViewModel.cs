namespace ShopTARpe25.Models.Library
{
    public class ReaderDetailsViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Sub-collection holding all active checked-out books for this person
        public List<BorrowedBookViewModel> BorrowedBooks { get; set; } = new List<BorrowedBookViewModel>();
    }

    public class BorrowedBookViewModel
    {
        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateOnly LoanDate { get; set; }
        public DateOnly? ReturnDate { get; set; } // Matches our new nullable setup
    }
}
