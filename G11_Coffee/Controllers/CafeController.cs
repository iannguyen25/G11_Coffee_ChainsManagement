using G11_Coffee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

public class CafeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public CafeController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
    }

    // GET: Cafe
    public async Task<IActionResult> Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            var cafes = await _context.Cafes
            .Select(c => new CafeViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                Phone = c.Phone,
                Image = c.Image
            }).ToListAsync();

            return View(cafes);
        }
        else
        {
            return RedirectToAction("Login", "Authentication");
        }
    }

    // GET: Cafe/CreatePartial
    public IActionResult CreatePartial()
    {
        return PartialView("_CreatePartial", new CafeViewModel());
    }

    // POST: Cafe/CreatePartial
    [HttpPost]
    public async Task<IActionResult> CreatePartial(CafeViewModel model)
    {
        ModelState.Remove("Image");

        string imagePath = null;
        imagePath = "/images/" + await SaveImage(model.ImageFile);


        if (ModelState.IsValid)
        {
            var cafe = new Cafe
            {
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Image = imagePath
            };

            _context.Cafes.Add(cafe);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Cafe created successfully." });
        }
        return PartialView("_CreatePartial", model);
    }

    // GET: Cafe/EditPartial/5
    public async Task<IActionResult> EditPartial(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cafe = await _context.Cafes.FindAsync(id);
        if (cafe == null)
        {
            return NotFound();
        }

        var model = new CafeViewModel
        {
            Id = cafe.Id,
            Name = cafe.Name,
            Address = cafe.Address,
            Phone = cafe.Phone,
            Image = cafe.Image
        };

        return PartialView("_EditPartial", model);
    }

    // POST: Cafe/EditPartial
    [HttpPost]
    public async Task<IActionResult> EditPartial(CafeViewModel model)
    {
        ModelState.Remove("Image");

        if (ModelState.IsValid)
        {
            var cafe = await _context.Cafes.FindAsync(model.Id);
            if (cafe == null)
            {
                return NotFound();
            }

            cafe.Name = model.Name;
            cafe.Address = model.Address;
            cafe.Phone = model.Phone;

            if (model.ImageFile != null)
            {
                var img = cafe.Image = await SaveImage(model.ImageFile);
                cafe.Image = "/images/" + img;
            }

            _context.Update(cafe);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Cafe updated successfully." });
        }
        return PartialView("_EditPartial", model);
    }

    // GET: Cafe/DeletePartial/5
    public async Task<IActionResult> DeletePartial(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cafe = await _context.Cafes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cafe == null)
        {
            return NotFound();
        }

        var model = new CafeViewModel
        {
            Id = cafe.Id,
            Name = cafe.Name
        };

        return PartialView("_DeletePartial", model);
    }

    // POST: Cafe/DeletePartial/5
    [HttpPost, ActionName("DeletePartial")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cafe = await _context.Cafes.FindAsync(id);
        _context.Cafes.Remove(cafe);
        await _context.SaveChangesAsync();
        return Json(new { success = true, message = "Cafe deleted successfully." });
    }

    private async Task<string> SaveImage(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            return null;
        }

        var fileName = imageFile.FileName;
        var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return fileName;
    }
}
