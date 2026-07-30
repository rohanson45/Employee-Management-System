using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee name is required")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Salary is required")]
        [Range(1000, 10000000, ErrorMessage = "Salary must be between ₹1,000 and ₹1,00,00,000")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Date of joining is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }

        // Foreign Key
        [Required(ErrorMessage = "Please select a department")]
        public int DepartmentId { get; set; }

        // Navigation property - each employee belongs to one department
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
    }
}