using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using G11_Coffee.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace G11_Coffee.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display list of employees
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var employees = await _context.Employees.Include(e => e.Cafe).ToListAsync();
                return View(employees);
            }
            else
            {
                return RedirectToAction("Login", "Authentication");
            }
        }

        // Render the create form
        public IActionResult Create()
        {
            ViewBag.Cafes = new SelectList(_context.Cafes, "Id", "Name");
            return PartialView("_CreateEmployeePartial", new Employee());
        }

        // Handle create form submission
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }

            ModelState.Remove("Cafe"); 

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();
                    employee.Cafe = await _context.Cafes.FindAsync(employee.CafeId);
                    return Json(new
                    {
                        success = true,
                        employee = new
                        {
                            Id = employee.Id,
                            FullName = employee.FullName,
                            Position = employee.Position,
                            Email = employee.Email,
                            Phone = employee.Phone,
                            CafeName = employee.Cafe?.Name
                        }
                    });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            ViewBag.Cafes = new SelectList(_context.Cafes, "Id", "Name");
            return PartialView("_CreateEmployeePartial", employee);
        }

        // Render the edit form
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            ViewBag.Cafes = new SelectList(_context.Cafes, "Id", "Name", employee.CafeId);
            return PartialView("_EditEmployeePartial", employee);
        }

        // Handle edit form submission
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            ModelState.Remove("Cafe"); // Remove any errors for the Cafe property

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(employee).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    employee.Cafe = await _context.Cafes.FindAsync(employee.CafeId);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

            ViewBag.Cafes = new SelectList(_context.Cafes, "Id", "Name", employee.CafeId);
            return PartialView("_EditEmployeePartial", employee);
        }

        // Handle employee search
        public IActionResult Search(string query)
        {
            var employees = _context.Employees.Include(e => e.Cafe)
                .Where(e => e.FullName.Contains(query) || e.Position.Contains(query) || e.Email.Contains(query))
                .ToList();
            return PartialView("_EmployeeListPartial", employees);
        }
    }
}