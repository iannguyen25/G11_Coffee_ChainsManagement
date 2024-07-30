using G11_Coffee.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _context.Orders
            .Include(o => o.Employee)
            .Include(o => o.Cafe)
            .ToListAsync();
        return View(orders);
    }

    public IActionResult Create()
    {
        ViewBag.Employees = new SelectList(_context.Employees, "Id", "FullName");
        ViewBag.Cafes = new SelectList(_context.Cafes, "Id", "Name");
        ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
        return View(new Order { OrderDate = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Order order, List<OrderDetail> orderDetails)
    {
        if (ModelState.IsValid)
        {
            order.OrderDetails = orderDetails;
            order.TotalAmount = orderDetails.Sum(od => od.Quantity * od.Price);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Employees = new SelectList(_context.Employees, "Id", "FullName");
        ViewBag.Cafes = new SelectList(_context.Cafes, "Id", "Name");
        ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
        return View(order);
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Employee)
            .Include(o => o.Cafe)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    public async Task<IActionResult> Print(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Employee)
            .Include(o => o.Cafe)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    public async Task<IActionResult> Search(string query)
    {
        var orders = await _context.Orders
            .Include(o => o.Employee)
            .Include(o => o.Cafe)
            .Where(o => o.OrderDate.ToString().Contains(query)
                || o.Employee.FullName.Contains(query)
                || o.Cafe.Name.Contains(query))
            .ToListAsync();

        return PartialView("_OrderListPartial", orders);
    }
}