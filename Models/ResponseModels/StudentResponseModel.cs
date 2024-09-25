using VotingApp.Models.Enums;

namespace VotingApp.Models.ResponseModels
{
    public class StudentResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MatricNo { get; set; } = null!;
        public Guid CourseId { get; set; }
        public CourseResponseModel? Course { get; set; }
        public bool CanVote { get; set; }
        public Gender Gender { get; set; }
        public Levels Level { get; set; }
        public decimal CGPA { get; set; }
        public List<VoteResponseModel> Votes { get; set; } = [];
    }
}
