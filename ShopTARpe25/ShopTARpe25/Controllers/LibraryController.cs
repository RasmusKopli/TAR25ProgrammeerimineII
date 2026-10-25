using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Library;

namespace ShopTARpe25.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ShopTARpe25Context _context;

        public LibraryController
            (
                ShopTARpe25Context context
            )
        {
            _context = context;
        }

        //Index for Books/Library
        public IActionResult Index()
        {
            var result = _context.Books
                .Select(x => new BookIndexViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    PublishedYear = x.PublishedYear,
                    IsAvailable = x.IsAvailable
                });

            return View(result);
        }

        //Create for Books
        [HttpGet]
        public IActionResult Create()
        {
            var model = new BookCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Id = Guid.NewGuid(),
                    Title = vm.Title,
                    Author = vm.Author,
                    PublishedYear = vm.PublishedYear,
                    IsAvailable = vm.IsAvailable
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        //Details for Books
        public IActionResult Details(Guid id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var activeLoan = _context.Loans.FirstOrDefault(l => l.BookId.ToString() == id.ToString());

            var matchingReader = activeLoan != null
                ? _context.Readers.FirstOrDefault(r => r.Id.ToString() == activeLoan.ReaderId.ToString())
                : null;

            var viewModel = new BookDetailsViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedYear = book.PublishedYear,
                IsAvailable = book.IsAvailable,
                ReaderFullName = matchingReader?.FullName,
                LoanDate = activeLoan?.LoanDate,
                ReturnDate = activeLoan?.ReturnDate
            };

            return View(viewModel);
        }
    }
}
