using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Domain.Models
{
    public class Employees
    {
        [Key]
        public Guid EmployeeId { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [Required, MaxLength(200), EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(15), MinLength(10)]
        public string? PhoneNumber { get; set; }

        [Required, MaxLength(12), MinLength(12)]
        public string? NationalIdNumber { get; set; } // Aadhar/SSN/etc.

        [Required, MaxLength(250)]
        public string? Address { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }

        [Required,MaxLength(100)]
        public string? Designation { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public DateTime? DateOfJoining { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal? Salary { get; set; }

        [Required]
        public string? Gender { get; set; }

        [Required]
        public string? BloodGroup { get; set; }

        [Required]
        public string? MaritalStatus { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

}
