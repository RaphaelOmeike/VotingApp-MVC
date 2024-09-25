using System.ComponentModel.DataAnnotations;

namespace VotingApp.Models.RequestModels
{
    public class UserRequestModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
