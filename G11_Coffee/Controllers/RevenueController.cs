using G11_Coffee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class RevenueController : Controller
{
    private readonly ApplicationDbContext _context;

    public RevenueController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Revenue/Index
    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
    {
        var ordersQuery = _context.Orders
            .Include(o => o.Employee)
            .Include(o => o.Cafe)
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .AsQueryable();

        if (startDate.HasValue && endDate.HasValue)
        {
            ordersQuery = ordersQuery
                .Where(o => o.OrderDate >= startDate.Value && o.OrderDate <= endDate.Value);
        }

        var orders = await ordersQuery.ToListAsync();
        var revenueViewModel = new RevenueViewModel
        {
            TotalRevenue = orders.Sum(o => o.TotalAmount),
            Orders = orders,
            StartDate = startDate,
            EndDate = endDate
        };

        ViewData["StartDate"] = startDate;
        ViewData["EndDate"] = endDate;

        return View(revenueViewModel);
    }
}
