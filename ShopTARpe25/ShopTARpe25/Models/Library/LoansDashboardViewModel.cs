namespace ShopTARpe25.Models.Library
{
    public class LoansDashboardViewModel
    {
        public List<LoanDetailsViewModel> ActiveLoans { get; set; } = new List<LoanDetailsViewModel>();
        public List<LoanDetailsViewModel> LoanHistory { get; set; } = new List<LoanDetailsViewModel>();
    }

    public class LoanDetailsViewModel
    {
        public Guid BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookAuthor { get; set; } = string.Empty;
        public string ReaderFullName { get; set; } = string.Empty;
        public DateOnly LoanDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
    }
}
