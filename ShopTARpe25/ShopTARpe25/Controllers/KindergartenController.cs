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
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await _context.Kindergartens
                .Where(x => x.Id == id)
                .Select(x => new KindergartenDetailsViewModel
                {
                    Id = x.Id,
                    KindergartenName = x.KindergartenName,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    TeacherName = x.TeacherName,
                    CreatedAt = x.CreatedAt,
                    ModifiedAt = x.ModifiedAt
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _context.Kindergartens
                .Where(x => x.Id == id)
                .Select(x => new KindergartenEditViewModel
                {
                    Id = x.Id,
                    KindergartenName = x.KindergartenName,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    TeacherName = x.TeacherName,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, KindergartenEditViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            var kindergarten = await _context.Kindergartens.FindAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            kindergarten.KindergartenName = vm.KindergartenName;
            kindergarten.GroupName = vm.GroupName;
            kindergarten.ChildrenCount = vm.ChildrenCount;
            kindergarten.TeacherName = vm.TeacherName;
            kindergarten.ModifiedAt = DateTime.UtcNow;

            try
            {
                _context.Update(kindergarten);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Kindergartens.Any(e => e.Id == kindergarten.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
