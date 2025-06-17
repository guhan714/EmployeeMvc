using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Domain.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress, Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        [PasswordPropertyText, Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty; 
        [Required(ErrorMessage = "PhoneNumber is required"), MinLength(10), MaxLength(10)]
        public string PhoneNumber { get; set; } = string.Empty;
    
    }
}
