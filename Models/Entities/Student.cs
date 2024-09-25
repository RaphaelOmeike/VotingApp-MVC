using VotingApp.Models.Enums;

namespace VotingApp.Models.Entities
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MatricNo { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public Gender Gender { get; set; }
        public Levels Level { get; set; }
        public decimal CGPA { get; set; }
        public bool CanVote { get; set; }
        public bool IsDeleted { get; set; }
        public Candidate? Candidate { get; set; }
        public ICollection<VoteCastingInfo> Votes { get; } = [];
    }
}
