namespace VotingApp.Models.Entities
{
    public class Session
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Election> Elections { get; } = [];
    }
}
