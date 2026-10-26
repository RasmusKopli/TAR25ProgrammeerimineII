using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index(string searchTitle, int? searchYear)
        {
            // Alustame baaspäringuga
            var query = from book in _context.Books
                        select book;

            // 1. Filtreerime pealkirja järgi (kui otsingukast pole tühi)
            if (!string.IsNullOrEmpty(searchTitle))
            {
                query = query.Where(b => b.Title.Contains(searchTitle));
            }

            // 2. Filtreerime aasta järgi (kui otsingukast pole tühi)
            if (searchYear.HasValue)
            {
                query = query.Where(b => b.PublishedYear == searchYear.Value);
            }

            // Viime andmed viewModeli struktuuri
            var result = query.Select(book => new BookIndexViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedYear = book.PublishedYear,
                IsAvailable = book.IsAvailable,
                Location = book.Location
            }).ToList();

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

        //Edit for Books
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();

            var activeLoan = await _context.Loans.FirstOrDefaultAsync(l => l.BookId.ToString() == book.Id.ToString());


            var viewModel = new BookEditViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedYear = book.PublishedYear,
                SelectedReaderId = activeLoan != null
                ? _context.Readers.AsEnumerable().FirstOrDefault(r => r.Id.ToString() == activeLoan.ReaderId.ToString())?.Id
                : null,


                ReadersList = await _context.Readers.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.FullName
                }).ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BookEditViewModel vm)
        {
            if (id != vm.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var domainBook = await _context.Books.FindAsync(id);
                if (domainBook == null) return NotFound();

                domainBook.Title = vm.Title;
                domainBook.Author = vm.Author;
                domainBook.PublishedYear = vm.PublishedYear;

                var existingLoan = await _context.Loans.FirstOrDefaultAsync(l => l.BookId.ToString() == id.ToString());

                if (vm.SelectedReaderId.HasValue)
                {
                    domainBook.IsAvailable = false;

                    if (existingLoan != null)
                    {
                        existingLoan.ReaderId = vm.SelectedReaderId.Value;
                        _context.Loans.Update(existingLoan);
                    }
                    else
                    {
                        var newLoan = new Loan
                        {
                            Id = Guid.NewGuid(),
                            BookId = domainBook.Id,
                            ReaderId = vm.SelectedReaderId.Value,
                            LoanDate = DateOnly.FromDateTime(DateTime.Today),
                            ReturnDate = null
                        };
                        await _context.Loans.AddAsync(newLoan);
                    }
                }
                else
                {
                    domainBook.IsAvailable = true;

                    if (existingLoan != null)
                    {
                        _context.Loans.Remove(existingLoan);
                    }
                }

                _context.Books.Update(domainBook);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm.ReadersList = await _context.Readers.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.FullName
            }).ToListAsync();

            return View(vm);
        }

        //Delete for books
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookEditViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedYear = book.PublishedYear
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var activeLoans = await _context.Loans.ToListAsync();
            var bookLoan = activeLoans.FirstOrDefault(l => l.BookId.ToString() == id.ToString());
            if (bookLoan != null)
            {
                _context.Loans.Remove(bookLoan);
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
