using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Kindergarten;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace ShopTARpe25.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly ShopTARpe25Context _context;

        public KindergartenController(ShopTARpe25Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _context.Kindergartens
                .Select(x => new KindergartenIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    TeacherName = x.TeacherName,
                    CreatedAt = x.CreatedAt,
                    ModifiedAt = x.ModifiedAt
                }).ToListAsync();

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new KindergartenCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KindergartenCreateViewModel vm)
        {
            var kindergarten = new Kindergarten
            {
                Id = Guid.NewGuid(),
                KindergartenName = vm.KindergartenName,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                TeacherName = vm.TeacherName,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            _context.Add(kindergarten);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
