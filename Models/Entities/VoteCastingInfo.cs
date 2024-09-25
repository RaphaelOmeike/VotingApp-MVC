namespace VotingApp.Models.Entities
{
    public class VoteCastingInfo
    {
        public Guid CandidatePositionId { get; set; }
        public CandidatePosition CandidatePosition { get; set; } = null!;
        public Guid StudentId { get; set; }
        public DateTime DateCasted { get; set; } = DateTime.Now;
        public Student Student { get; set; } = null!;
    }
}
