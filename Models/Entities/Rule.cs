using VotingApp.Models.Enums;

namespace VotingApp.Models.Entities
{
    public class Rule
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public Gender Gender { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;//all programs
        public decimal MinCGPA { get; set; }
        public Levels MinLevel { get; set; }
        public Levels MaxLevel { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Position> Positions { get; } = [];
        public ICollection<Election> Elections { get; } = [];
    }
}
