using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Library;

namespace ShopTARpe25.Controllers
{
    public class LoansController : Controller
    {
        private readonly ShopTARpe25Context _context;

        public LoansController(ShopTARpe25Context context)
        {
            _context = context;
        }

        public IActionResult Index(string searchName)
        {
            var allLoans = _context.Loans.ToList();
            var books = _context.Books.ToList();
            var readers = _context.Readers.ToList();

            var compiledLoans = allLoans.Select(loan =>
            {
                var book = books.FirstOrDefault(b => b.Id.ToString() == loan.BookId.ToString());
                var reader = readers.FirstOrDefault(r => r.Id.ToString() == loan.ReaderId.ToString());

                return new LoanDetailsViewModel
                {
                    BookId = loan.BookId,
                    BookTitle = book?.Title ?? "Unknown Title",
                    BookAuthor = book?.Author ?? "Unknown Author",
                    ReaderFullName = reader?.FullName ?? "Unknown Reader",
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate
                };
            }).ToList();

            if (!string.IsNullOrEmpty(searchName))
            {
                compiledLoans = compiledLoans
                    .Where(l => l.ReaderFullName.Contains(searchName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var viewModel = new LoansDashboardViewModel
            {
                ActiveLoans = compiledLoans.Where(l => l.ReturnDate == null).ToList(),
                LoanHistory = compiledLoans.Where(l => l.ReturnDate != null).ToList()
            };

            return View(viewModel);
        }
    }
}
