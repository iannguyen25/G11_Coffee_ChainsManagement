using G11_Coffee.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    ICompositeViewEngine _viewEngine;

    public OrderController(ApplicationDbContext context, ICompositeViewEngine viewEngine)
    {
        _context = context;
        _viewEngine = viewEngine;
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
        ViewBag.Cafes = new SelectList(_context.Cafes, "Id", "Name");
        ViewBag.Employees = new SelectList(_context.Employees, "Id", "FullName");
        ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
        return PartialView("_CreateOrderPartial", new OrderViewModel { OrderDate = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrderViewModel model)
    {
        if (ModelState.IsValid)
        {
            var order = new Order
            {
                CafeId = model.CafeId,
                EmployeeId = model.EmployeeId,
                OrderDate = model.OrderDate,
                TotalAmount = model.TotalAmount,
                OrderDetails = model.OrderDetails.Select(od => new OrderDetail
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Order created successfully" });
        }

        return Json(new { success = false, message = "Invalid model state" });
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
            return Json(new { success = false, message = "Order not found" });
        }

        var orderDetailsView = await this.RenderViewToStringAsync("_OrderDetailsPartial", order);
        return Json(new { success = true, html = orderDetailsView });
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
            return Json(new { success = false, message = "Order not found" });
        }

        var printInvoiceView = await this.RenderViewToStringAsync("_PrintInvoicePartial", order);
        return Json(new { success = true, html = printInvoiceView });
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

    private async Task<string> RenderViewToStringAsync(string viewName, object model)
    {
        if (string.IsNullOrEmpty(viewName))
            viewName = ControllerContext.ActionDescriptor.ActionName;
        ViewData.Model = model;

        using (var writer = new StringWriter())
        {
            var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

            if (viewResult.View == null)
            {
                throw new ArgumentNullException($"{viewName} does not match any available view");
            }

            var viewContext = new ViewContext(
                ControllerContext,
                viewResult.View,
                ViewData,
                TempData,
                writer,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);

            return writer.GetStringBuilder().ToString();
        }
    }
}