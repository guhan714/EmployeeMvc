using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Employee.UI.Models
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "Invalid email address"), Required]
        public string Email { get; set; } = string.Empty;
        [PasswordPropertyText, Required, MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = string.Empty;
    }
}
