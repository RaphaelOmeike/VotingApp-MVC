namespace VotingApp.Models.Entities
{
    public class Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Student> Students { get; } = [];
        public ICollection<Rule> Rules { get; } = [];
    }
}
