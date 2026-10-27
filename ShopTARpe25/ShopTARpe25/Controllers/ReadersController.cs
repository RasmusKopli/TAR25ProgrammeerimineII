using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Library;

namespace ShopTARpe25.Controllers
{
    public class ReadersController : Controller
    {
        private readonly ShopTARpe25Context _context;

        public ReadersController(ShopTARpe25Context context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var readers = _context.Readers.ToList();
            var loans = _context.Loans.ToList();

            var result = readers.Select(reader => new ReaderIndexViewModel
            {
                Id = reader.Id,
                FullName = reader.FullName,
                Email = reader.Email ?? "No Email Registered",
                ActiveLoansCount = loans.Count(l => l.ReaderId == reader.Id)
            }).ToList();

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ReaderCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReaderCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var domainReader = new Reader
                {
                    Id = Guid.NewGuid(),
                    FullName = vm.FullName,
                    Email = vm.Email
                };

                _context.Readers.Add(domainReader);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        public IActionResult Details(Guid id)
        {
            var reader = _context.Readers.FirstOrDefault(r => r.Id == id);
            if (reader == null)
            {
                return NotFound();
            }

            var readerLoans = _context.Loans.Where(l => l.ReaderId.ToString() == id.ToString()).ToList();

            var borrowedBooks = readerLoans.Select(loan => {
                var book = _context.Books.FirstOrDefault(b => b.Id.ToString() == loan.BookId.ToString());

                return new BorrowedBookViewModel
                {
                    BookId = loan.BookId,
                    Title = book?.Title ?? "Unknown Title",
                    Author = book?.Author ?? "Unknown Author",
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate
                };
            }).ToList();

            var viewModel = new ReaderDetailsViewModel
            {
                Id = reader.Id,
                FullName = reader.FullName,
                Email = reader.Email ?? "No Email Registered",
                BorrowedBooks = borrowedBooks
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnBook(Guid bookId, Guid readerId)
        {
            var activeLoan = _context.Loans
                .AsEnumerable()
                .FirstOrDefault(l => l.BookId.ToString() == bookId.ToString() && l.ReturnDate == null);

            if (activeLoan != null)
            {
                activeLoan.ReturnDate = DateOnly.FromDateTime(DateTime.Today);
                _context.Loans.Update(activeLoan);
            }

            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                book.IsAvailable = true;
                _context.Books.Update(book);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Details), new { id = readerId });
        }
    }
}
