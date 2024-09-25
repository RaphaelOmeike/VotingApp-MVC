namespace VotingApp.Models.ResponseModels
{
    public class CandidatePositionResponseModel
    {
        public Guid Id { get; set; }
        public string? Statement { get; set; }
        public int? VotesNo { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool Winner { get; set; }
        public bool IsDisqualified { get; set; }
        public Guid? DisqualifierId { get; set; }
        public Guid CandidateId { get; set; }
        public CandidateResponseModel? Candidate { get; set; }
        public Guid PositionId { get; set; }
        public PositionResponseModel? Position { get; set; }
        public List<VoteResponseModel> Votes { get; set; } = [];
    }
}
