using System.ComponentModel.DataAnnotations.Schema;

namespace VotingApp.Models.Entities
{
    public class Election
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool IsClosed { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid SessionId { get; set; }
        [ForeignKey("SessionId")]
        public Session Session { get; set; } = null!;
        public Guid RuleId { get; set; }
        [ForeignKey("RuleId")]
        public Rule Rule { get; set; } = null!;
        public ICollection<Position> Positions { get; } = [];
    }
}
