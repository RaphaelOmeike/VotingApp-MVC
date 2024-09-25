using System.ComponentModel.DataAnnotations.Schema;

namespace VotingApp.Models.Entities
{
    public class Position
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAvailable { get; set; }
        public Guid ElectionId { get; set; }
        [ForeignKey("ElectionId")]
        public Election Election { get; set; } = null!;
        public Guid RuleId { get; set; }
        [ForeignKey("RuleId")]
        public Rule Rule { get; set; } = null!;
        public ICollection<CandidatePosition> CandidatePositions { get; } = [];
    }
}
