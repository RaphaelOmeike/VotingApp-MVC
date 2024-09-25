using System.ComponentModel.DataAnnotations.Schema;

namespace VotingApp.Models.Entities
{
    public class Candidate
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsDeleted { get; set; }
        public Guid StudentId { get; set; } 
        [ForeignKey("StudentId")]
        public Student Student { get; set; } = null!;
        public ICollection<CandidatePosition> CandidatePositions { get; } = [];
    }
}
