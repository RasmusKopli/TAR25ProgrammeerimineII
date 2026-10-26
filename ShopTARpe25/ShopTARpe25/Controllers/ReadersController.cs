using Microsoft.AspNetCore.Mvc;
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
    }
}
