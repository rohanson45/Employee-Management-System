using Microsoft.AspNetCore.Mvc.Rendering;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class EmployeesController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployeesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: EMPLOYEES
    public async Task<IActionResult> Index(string searchString, string sortOrder)
    {
        ViewBag.SalarySortParam = sortOrder == "salary_asc" ? "salary_desc" : "salary_asc";
        ViewBag.DateSortParam = sortOrder == "date_asc" ? "date_desc" : "date_asc";
        ViewBag.CurrentFilter = searchString;

        var employees = _context.Employees.Include(e => e.Department).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            employees = employees.Where(e =>
                e.FullName.Contains(searchString) ||
                (e.Department != null && e.Department.DepartmentName.Contains(searchString)));
        }

        switch (sortOrder)
        {
            case "salary_asc":
                employees = employees.OrderBy(e => e.Salary);
                break;
            case "salary_desc":
                employees = employees.OrderByDescending(e => e.Salary);
                break;
            case "date_asc":
                employees = employees.OrderBy(e => e.DateOfJoining);
                break;
            case "date_desc":
                employees = employees.OrderByDescending(e => e.DateOfJoining);
                break;
            default:
                employees = employees.OrderBy(e => e.EmployeeId);
                break;
        }

        return View(await employees.ToListAsync());
    }

    // GET: EMPLOYEES/Details/5
    public async Task<IActionResult> Details(int? employeeid)
    {
        if (employeeid == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees
            .FirstOrDefaultAsync(m => m.EmployeeId == employeeid);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    // GET: EMPLOYEES/Create
    public IActionResult Create()
    {
        ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
        return View();
    }

    // POST: EMPLOYEES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EmployeeId,FullName,Email,PhoneNumber,Salary,DateOfJoining,DepartmentId,Department")] Employee employee)
    {
        if (ModelState.IsValid)
        {
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
        return View(employee);
    }

    // GET: EMPLOYEES/Edit/5
    public async Task<IActionResult> Edit(int? employeeid)
    {
        if (employeeid == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees.FindAsync(employeeid);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }

    // POST: EMPLOYEES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? employeeid, [Bind("EmployeeId,FullName,Email,PhoneNumber,Salary,DateOfJoining,DepartmentId,Department")] Employee employee)
    {
        if (employeeid != employee.EmployeeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employee.EmployeeId))
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
        return View(employee);
    }

    // GET: EMPLOYEES/Delete/5
    public async Task<IActionResult> Delete(int? employeeid)
    {
        if (employeeid == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees
            .FirstOrDefaultAsync(m => m.EmployeeId == employeeid);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    // POST: EMPLOYEES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? employeeid)
    {
        var employee = await _context.Employees.FindAsync(employeeid);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EmployeeExists(int? employeeid)
    {
        return _context.Employees.Any(e => e.EmployeeId == employeeid);
    }
}
