using System.ComponentModel;
using VotingApp.Models.Enums;

namespace VotingApp.Models.ResponseModels
{
    public class RuleResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Gender Gender { get; set; }//consider all genders also
        public Guid CourseId { get; set; }
        public CourseResponseModel? Course { get; set; }
        [DisplayName("Minimum CGPA")]
        public decimal MinCGPA { get; set; }
        [DisplayName("Minimum Level")]
        public Levels MinLevel { get; set; }
        [DisplayName("Maximum Level")]
        public Levels MaxLevel { get; set; }
        public List<PositionResponseModel> Positions { get; set; } = [];
        public List<ElectionResponseModel> Elections { get; set; } = [];
    }
}
