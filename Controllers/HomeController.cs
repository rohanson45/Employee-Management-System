using System.Diagnostics;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalEmployees = await _context.Employees.CountAsync();
            ViewBag.TotalDepartments = await _context.Departments.CountAsync();

            ViewBag.AverageSalary = await _context.Employees.AnyAsync()
                ? await _context.Employees.AverageAsync(e => e.Salary)
                : 0;
            var topDepartment = await _context.Employees
    .Include(e => e.Department)
    .Where(e => e.Department != null)
    .GroupBy(e => e.Department!.DepartmentName)
    .Select(g => new { DepartmentName = g.Key, EmployeeCount = g.Count() })
    .OrderByDescending(g => g.EmployeeCount)
    .FirstOrDefaultAsync();

            ViewBag.TopDepartmentName = topDepartment?.DepartmentName ?? "N/A";
            ViewBag.TopDepartmentCount = topDepartment?.EmployeeCount ?? 0;
            ViewBag.RecentEmployees = await _context.Employees
                .Include(e => e.Department)
                .OrderByDescending(e => e.EmployeeId)
                .Take(5)
                .ToListAsync();

            ViewBag.DepartmentList = await _context.Departments
                .Select(d => new { d.DepartmentId, d.DepartmentName })
                .ToListAsync();
            var departmentChartData = await _context.Departments
    .Select(d => new
    {
        DepartmentName = d.DepartmentName,
        EmployeeCount = d.Employees != null ? d.Employees.Count : 0
    })
    .ToListAsync();

            ViewBag.ChartLabels = departmentChartData.Select(d => d.DepartmentName).ToList();
            ViewBag.ChartData = departmentChartData.Select(d => d.EmployeeCount).ToList();

            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}