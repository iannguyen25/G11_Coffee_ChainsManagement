using G11_Coffee.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products.Include(p => p.Category).ToList();
        return View(products);
    }

    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
        return PartialView("_ProductFormPartial", new Product());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        ModelState.Remove("Category");
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        return PartialView("_ProductFormPartial", product);
    }

    public IActionResult Edit(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        return PartialView("_ProductFormPartial", product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Product product)
    {
        ModelState.Remove("Category");
        if (ModelState.IsValid)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        return PartialView("_ProductFormPartial", product);
    }

    [HttpGet]
    public IActionResult DeletePartial(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        return PartialView("_DeletePartial", product);
    }

    [HttpPost]
    public async Task<IActionResult> DeletePartialConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }

    public IActionResult Search(string query)
    {
        var products = string.IsNullOrWhiteSpace(query) ?
            _context.Products.Include(p => p.Category).ToList() :
            _context.Products.Include(p => p.Category).Where(p => p.Name.Contains(query)).ToList();
        return PartialView("_ProductListPartial", products);
    }
}
