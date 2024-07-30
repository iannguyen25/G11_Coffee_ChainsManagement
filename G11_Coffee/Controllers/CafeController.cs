using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using G11_Coffee.Models;

public class CafeController : Controller
{
    private readonly ApplicationDbContext _context;

    public CafeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var cafes = _context.Cafes.ToList();
        var cafeViewModels = cafes.Select(c => new CafeViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Address = c.Address,
            Phone = c.Phone
        }).ToList();

        return View(cafeViewModels);
    }

    [HttpGet]
    public IActionResult CreatePartial()
    {
        return PartialView("_CreatePartial", new CafeViewModel());
    }

    [HttpPost]
    public IActionResult CreatePartial(CafeViewModel model)
    {
        if (ModelState.IsValid)
        {
            var cafe = new Cafe
            {
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone
            };
            _context.Cafes.Add(cafe);
            _context.SaveChanges();
            return Json(new { success = true, message = "Chi nhánh đã được tạo thành công!" });
        }
        return PartialView("_CreatePartial", model);
    }

    [HttpGet]
    public IActionResult EditPartial(int id)
    {
        var cafe = _context.Cafes.Find(id);
        if (cafe == null)
        {
            return NotFound();
        }

        var model = new CafeViewModel
        {
            Id = cafe.Id,
            Name = cafe.Name,
            Address = cafe.Address,
            Phone = cafe.Phone
        };

        return PartialView("_EditPartial", model);
    }

    [HttpPost]
    public IActionResult EditPartial(CafeViewModel model)
    {
        if (ModelState.IsValid)
        {
            var cafe = _context.Cafes.Find(model.Id);
            if (cafe == null)
            {
                return NotFound();
            }

            cafe.Name = model.Name;
            cafe.Address = model.Address;
            cafe.Phone = model.Phone;

            _context.Cafes.Update(cafe);
            _context.SaveChanges();

            return Json(new { success = true, message = "Thông tin chi nhánh đã được cập nhật!" });
        }

        return PartialView("_EditPartial", model);
    }

    [HttpGet]
    public IActionResult DeletePartial(int id)
    {
        var cafe = _context.Cafes.Find(id);
        if (cafe == null)
        {
            return NotFound();
        }

        var model = new CafeViewModel
        {
            Id = cafe.Id,
            Name = cafe.Name,
            Address = cafe.Address,
            Phone = cafe.Phone
        };

        return PartialView("_DeletePartial", model);
    }

    [HttpPost]
    public IActionResult DeletePartial(CafeViewModel model)
    {
        var cafe = _context.Cafes.Find(model.Id);
        if (cafe == null)
        {
            return Json(new { success = false, message = "Không tìm thấy chi nhánh" });
        }

        _context.Cafes.Remove(cafe);
        _context.SaveChanges();

        return Json(new { success = true, message = "Chi nhánh đã được xóa thành công!" });
    }

}
