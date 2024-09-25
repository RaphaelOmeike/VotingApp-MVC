namespace VotingApp.Models.Entities
{
    public class CandidatePosition
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Statement { get; set; }
        public int? VotesNo { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool Winner { get; set; }
        public bool IsDisqualified { get; set; }
        public Guid? DisqualifierId { get; set; }
        public Guid CandidateId { get; set; }
        public Candidate Candidate { get; set; } = null!;
        public Guid PositionId { get; set; }
        public Position Position { get; set; } = null!;
        public ICollection<VoteCastingInfo> Votes { get; } = [];
    }
}
