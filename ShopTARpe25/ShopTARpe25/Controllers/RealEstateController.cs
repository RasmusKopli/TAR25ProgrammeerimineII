using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.RealEstate;

namespace ShopTARpe25.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly ShopTARpe25Context _context;
        private readonly IRealEstateServices _realEstateService;
        private readonly IFileServices _fileService;

        public RealEstateController
            (
                ShopTARpe25Context context,
                IRealEstateServices realEstateService,
                IFileServices fileService
            )
        {
            _context = context;
            _realEstateService = realEstateService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            var result = _context.RealEstates
                .Select(x => new RealEstateIndexViewModel
                {
                    Id = x.Id,
                    Area = x.Area,
                    Address = x.Address,
                    RoomNumber = x.RoomNumber,
                    BuildingType = x.BuildingType
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel vm = new();

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto
            {
                Id = vm.Id,
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                Files = vm.Files,
                FileToApiDtos = vm.Image
                    .Select(file => new FileToApiDto
                    {
                        Id = file.ImageId,
                        ExistingFilePath = file.FilePath,
                        RealEstateId = file.RealEstateId
                    }).ToArray()
            };

            var result = await _realEstateService.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var realEstate = await _realEstateService.DetailsAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    RealEstateId = y.RealEstateId
                }).ToArrayAsync();

            var vm = new RealEstateDetailsViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.Address = vm.Address;
            vm.RoomNumber = vm.RoomNumber;
            vm.BuildingType = vm.BuildingType;
            vm.CreatedAt = realEstate.CreatedAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realEstate = await _realEstateService.DetailsAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.RealEstateId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    RealEstateId = y.SpaceshipId
                }).ToArrayAsync();

            var vm = new RealEstateUpdateViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.Address = realEstate.Address;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.BuildingType = realEstate.BuildingType;
            vm.CreatedAt = realEstate.CreatedAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RealEstateUpdateViewModel vm)
        {
            var dto = new RealEstateDto
            {
                Id = vm.Id,
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                Files = vm.Files,
                FileToApiDtos = vm.Image
                    .Select(file => new FileToApiDto
                    {
                        Id = file.ImageId,
                        ExistingFilePath = file.FilePath,
                        RealEstateId = file.RealEstateId
                    }).ToArray()
            };

            var result = await _realEstateService.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _realEstateService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.RealEstateId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    RealEstateId = y.SpaceshipId
                }).ToArrayAsync();

            var vm = new RealEstateUpdateViewModel();

            vm.Id = realEstate.Id;
            vm.Area = v.Area;
            vm.Address = vm.Address;
            vm.RoomNumber = vm.RoomNumber;
            vm.BuildingType = vm.BuildingType;
            vm.CreatedAt = realEstate.CreatedAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Image.AddRange(images);
        }
    }
}
