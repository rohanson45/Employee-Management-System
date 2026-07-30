using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty;

        // Navigation property - one department has many employees
        public List<Employee>? Employees { get; set; }
    }
}