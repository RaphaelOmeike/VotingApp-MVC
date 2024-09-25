using VotingApp.Models.Enums;

namespace VotingApp.Models.RequestModels
{
    public class UpdateStudentRequestModel
    {
        public string Name { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Gender Gender { get; set; }
        public Levels Level { get; set; }
        public decimal CGPA { get; set; }
    }
}
