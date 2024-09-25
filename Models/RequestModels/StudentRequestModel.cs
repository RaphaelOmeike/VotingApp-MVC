using System.ComponentModel.DataAnnotations;
using VotingApp.Models.Enums;

namespace VotingApp.Models.RequestModels
{
    public class StudentRequestModel
    {
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;
        public string MatricNo { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Gender Gender { get; set; }
        public Levels Level { get; set; }
        public decimal CGPA { get; set; }
    }
}
