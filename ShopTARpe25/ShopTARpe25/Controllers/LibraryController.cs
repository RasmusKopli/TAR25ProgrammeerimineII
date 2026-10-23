using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Library;

namespace ShopTARpe25.Controllers
{
    public class LibraryController
    {
        private readonly ShopTARpe25Context _context;

        public LibraryController
            (
                ShopTARpe25Context context
            )
        {
            _context = context;
        }

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

            return base.View(result);
        }
    }
}
