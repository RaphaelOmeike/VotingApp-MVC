using VotingApp.Models.Enums;

namespace VotingApp.Models.RequestModels
{
    public class RuleRequestModel
    {
        public string Name { get; set; } = null!;
        public Gender Gender { get; set; }
        public Guid CourseId { get; set; }
        public decimal MinCGPA { get; set; }
        public Levels MinLevel { get; set; }
        public Levels MaxLevel { get; set; }
    }
}
