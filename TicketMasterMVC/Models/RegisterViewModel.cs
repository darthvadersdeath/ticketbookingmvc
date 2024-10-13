using System.ComponentModel.DataAnnotations;

namespace TicketMasterMVC.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public required  string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public  required string ConfirmPassword { get; set; }

        public required string OTP { get; set; } // For OTP Verification
    }

}
